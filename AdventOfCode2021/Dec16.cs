using System;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec16
    {
        public static void Solve(bool show = false)
        {
            string line = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec16.txt").First();

            string transmission = HexToBinary(line);

            int index = 0;
            long versionSum;
            long result = ParsePacket(transmission, ref index, out versionSum, show);

            Console.WriteLine("Version sum = {0}.", versionSum);
            Console.WriteLine("Result = {0}.", result);
        }

        private static long ParsePacket(string transmission, ref int index, out long versionNumber, bool show)
        {
            long result = 0;

            // Every packet begins with a standard header: the first three bits encode the packet version.
            string version = transmission.Substring(index, 3);
            index += 3;

            versionNumber = BinaryStringToNum(version);

            // The next three bits encode the packet type ID.
            string packetTypeId = transmission.Substring(index, 3);
            index += 3;

            long newVersionNumber;
            switch (packetTypeId)
            {
                case "000":
                    if (show)
                    {
                        Console.Write("(+ ");
                    }
                    
                    // Sum packet.
                    result = ParseMode(
                        transmission, 
                        ref index, 
                        (x, y) => x + y, 
                        0,
                        out newVersionNumber,
                        show);

                    versionNumber += newVersionNumber;
                    if (show)
                    {
                        Console.Write(")");
                    }

                    break;

                case "001":
                    // Product packet.
                    if (show)
                    {
                        Console.Write("(* ");
                    }

                    result = ParseMode(
                        transmission,
                        ref index,
                        (x, y) => x * y,
                        1,
                        out newVersionNumber,
                        show);

                    if (show)
                    {
                        Console.Write(")");
                    }

                    versionNumber += newVersionNumber;
                    break;

                case "010":

                    if (show)
                    {
                        Console.Write("(min ");
                    }

                    // Minimum packet.
                    result = ParseMode(
                        transmission,
                        ref index,
                        (x, y) => Math.Min(x, y),
                        Int64.MaxValue,
                        out newVersionNumber,
                        show);

                    if (show)
                    {
                        Console.Write(")");
                    }

                    versionNumber += newVersionNumber;
                    break;

                case "011":
                    // Maximum packet.
                    if (show)
                    {
                        Console.Write("(max ");
                    }

                    result = ParseMode(
                        transmission,
                        ref index,
                        (x, y) => Math.Max(x, y),
                        Int64.MinValue,
                        out newVersionNumber,
                        show);

                    if (show)
                    {
                        Console.Write(")");
                    }

                    versionNumber += newVersionNumber;
                    break;

                case "100":
                    // Packet is a literal value.
                    string segment = null;
                    var sb = new StringBuilder();
                    do
                    {
                        segment = transmission.Substring(index, 5);
                        sb.Append(segment.Substring(1));
                        index += 5;
                    } while (segment[0] != '0');

                    result = BinaryStringToNum(sb.ToString());

                    if (show)
                    {
                        Console.Write(" {0} ", result);
                    }

                    break;

                case "101":
                    // Greater than.
                    if (show)
                    {
                        Console.Write("(> ");
                    }

                    result = ParseComparision(
                        transmission,
                        ref index,
                        ComparisionType.GreaterThan,
                        out newVersionNumber,
                        show);

                    if (show)
                    {
                        Console.Write(")");
                    }

                    versionNumber += newVersionNumber;
                    break;

                case "110":
                    // Less than.
                    if (show)
                    {
                        Console.Write("(< ");
                    }

                    result = ParseComparision(
                        transmission,
                        ref index,
                        ComparisionType.LessThan,
                        out newVersionNumber,
                        show);

                    if (show)
                    {
                        Console.Write(")");
                    }

                    versionNumber += newVersionNumber;
                    break;

                case "111":
                    // Equals.
                    if (show)
                    {
                        Console.Write("(= ");
                    }

                    result = ParseComparision(
                        transmission,
                        ref index,
                        ComparisionType.Equals,
                        out newVersionNumber,
                        show);

                    if (show)
                    {
                        Console.Write(")");
                    }

                    versionNumber += newVersionNumber;
                    break;

                default:
                    throw new ArgumentException($"Unexpected packet type {0}.", packetTypeId);
            }

            return result;
        }

        private static long ParseComparision(
            string transmission,
            ref int index,
            ComparisionType comparisionType,
            out long versionNumber,
            bool show)
        {
            long result1;
            long result2;
            long ver1;
            long ver2;

            string mode = transmission.Substring(index, 1);
            index++;

            // Just seek forward the requisuite number of bits; we already know that there
            // are going to be exactly two sub-packets.
            switch (mode)
            {
                case "0":
                    index += 15;
                    break;

                case "1":
                    index += 11;
                    break;

                default:
                    throw new ArgumentException($"Unexpected mode {mode}.");
            }

            result1 = ParsePacket(transmission, ref index, out ver1, show);
            result2 = ParsePacket(transmission, ref index, out ver2, show);

            versionNumber = ver1 + ver2;

            switch (comparisionType)
            {
                case ComparisionType.LessThan:
                    return result1 < result2 ? 1 : 0;

                case ComparisionType.GreaterThan:
                    return result1 > result2 ? 1 : 0;

                case ComparisionType.Equals:
                    return result1 == result2 ? 1 : 0;

                default:
                    throw new ArgumentException($"Unexpected ComparisonType {comparisionType}.");
            }
        }

        private static long ParseMode(
            string transmission, 
            ref int index, 
            Func<long, long, long> eval,
            long seed,
            out long versionNumber,
            bool show)
        {
            long result = seed;
            versionNumber = 0;

            string mode = transmission.Substring(index, 1);
            index++;

            long nextVersionNumber;
            switch (mode)
            {
                // If the length type ID is 0,
                // then the next 15 bits are a number that represents the total length in bits of the sub-packets
                // contained by this packet.
                case "0":
                    string lengthStr = transmission.Substring(index, 15);
                    index += 15;
                    long lengthInBits = BinaryStringToNum(lengthStr);
                    long nextIndex = index + lengthInBits;

                    do
                    {
                        result = eval(result, ParsePacket(transmission, ref index, out nextVersionNumber, show));
                        versionNumber += nextVersionNumber;
                    } while (index < nextIndex);

                    break;

                case "1":
                    // If the length type ID is 1, then the next 11 bits are a number that
                    // represents the number of sub-packets immediately contained by this packet.
                    string numPacketsStr = transmission.Substring(index, 11);
                    index += 11;
                    long numPackets = BinaryStringToNum(numPacketsStr);

                    for (int i = 0; i < numPackets; i++)
                    {
                        result = eval(result, ParsePacket(transmission, ref index, out nextVersionNumber, show));
                        versionNumber += nextVersionNumber;
                    }

                    break;

                default:
                    throw new ArgumentException($"Unexpected mode {0}.", mode);
            }

            return result;
        }

        private static long BinaryStringToNum(string binString)
        {
            long sum = 0;
            long mul = 1;
            for (int i = binString.Length - 1; i >= 0; i--)
            {
                sum += mul * Int32.Parse(binString[i].ToString());
                mul *= 2;
            }

            return sum;
        }

        private static string HexToBinary(string hexString)
        {
            var sb = new StringBuilder();
            foreach (char ch in hexString)
            {
                switch (ch)
                {
                    case '0':
                        sb.Append("0000");
                        break;

                    case '1':
                        sb.Append("0001");
                        break;

                    case '2':
                        sb.Append("0010");
                        break;

                    case '3':
                        sb.Append("0011");
                        break;

                    case '4':
                        sb.Append("0100");
                        break;

                    case '5':
                        sb.Append("0101");
                        break;

                    case '6':
                        sb.Append("0110");
                        break;

                    case '7':
                        sb.Append("0111");
                        break;

                    case '8':
                        sb.Append("1000");
                        break;

                    case '9':
                        sb.Append("1001");
                        break;

                    case 'A':
                        sb.Append("1010");
                        break;

                    case 'B':
                        sb.Append("1011");
                        break;

                    case 'C':
                        sb.Append("1100");
                        break;

                    case 'D':
                        sb.Append("1101");
                        break;

                    case 'E':
                        sb.Append("1110");
                        break;

                    case 'F':
                        sb.Append("1111");
                        break;

                    default:
                        throw new ArgumentException($"Unexpected character {ch}.");
                }
            }

            return sb.ToString();
        }
    }

    public enum ComparisionType
    {
        LessThan,
        GreaterThan,
        Equals
    }
}
