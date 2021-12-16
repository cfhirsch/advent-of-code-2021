using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec16
    {
        public static void Solve()
        {
            string line = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec16.txt").First();

            string transmission = HexToBinary(line);

            int index = 0;
            int versionSum = ParsePacket(transmission, ref index);
            Console.WriteLine("Version sum = {0}.", versionSum);
        }

        private static int ParsePacket(string transmission, ref int index)
        {
            int versionSum = 0;

            // Every packet begins with a standard header: the first three bits encode the packet version.
            string version = transmission.Substring(index, 3);
            index += 3;

            // TODO: this is binary, not hex.
            versionSum += BinaryStringToNum(version);

            // The next three bits encode the packet type ID.
            string packetTypeId = transmission.Substring(index, 3);
            index += 3;

            switch (packetTypeId)
            {
                case "100":
                    // Packet is a literal value.
                    string segment = null;
                    do
                    {
                        segment = transmission.Substring(index, 5);
                        index += 5;
                    } while (segment[0] != '0');

                    break;

                default:
                    string mode = transmission.Substring(index, 1);
                    index++;

                    switch (mode)
                    {
                        // If the length type ID is 0,
                        // then the next 15 bits are a number that represents the total length in bits of the sub-packets
                        // contained by this packet.
                        case "0":
                            string lengthStr = transmission.Substring(index, 15);
                            index += 15;
                            int lengthInBits = BinaryStringToNum(lengthStr);
                            int nextIndex = index + lengthInBits;
                            do
                            {
                                versionSum += ParsePacket(transmission, ref index);
                            } while (index < nextIndex);

                            break;

                        case "1":
                            // If the length type ID is 1, then the next 11 bits are a number that
                            // represents the number of sub-packets immediately contained by this packet.
                            string numPacketsStr = transmission.Substring(index, 11);
                            index += 11;
                            int numPackets = BinaryStringToNum(numPacketsStr);
                            for (int i = 0; i < numPackets; i++)
                            {
                                versionSum += ParsePacket(transmission, ref index);
                            }

                            break;

                        default:
                            throw new ArgumentException($"Unexpected mode {0}.", mode);
                    }

                    break;
            }

            return versionSum;
        }

        private static int BinaryStringToNum(string binString)
        {
            int sum = 0;
            int mul = 1;
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
}
