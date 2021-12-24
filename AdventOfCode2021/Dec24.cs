using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    /// <summary>
    /// Pretty much got lazy by Day 22, so I cribbed the solution from here:
    /// https://www.reddit.com/user/Ill_Caramel3106/
    /// </summary>
    public static class Dec24
    {
        public static void Solve(bool partTwo = false)
        {
            string[] instructions = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec24.txt").ToArray();

            long result = CalculateNumber(instructions, goBig: !partTwo);
            Console.WriteLine(result);
        }

        private static long CalculateNumber(string[] input, bool goBig = true)
        {
            var inputStash = new Stack<(int, int)>();
            int[] finalDigits = new int[14];

            int targetIndex = 0;
            for (int block = 0; block < input.Length; block += 18)
            {
                int check = int.Parse(input[block + 5].Split(' ')[2]);
                int offset = int.Parse(input[block + 15].Split(' ')[2]);
                if (check > 0)
                {
                    inputStash.Push((targetIndex, offset));
                }
                else
                {
                    (int sourceIndex, int offset) rule = inputStash.Pop();
                    int totalOffset = rule.offset + check;
                    if (totalOffset > 0)
                    {
                        if (goBig)
                        {
                            finalDigits[rule.sourceIndex] = 9 - totalOffset;
                            finalDigits[targetIndex] = 9;
                        }
                        else
                        {
                            finalDigits[rule.sourceIndex] = 1;
                            finalDigits[targetIndex] = 1 + totalOffset;
                        }
                    }
                    else
                    {
                        if (goBig)
                        {
                            finalDigits[rule.sourceIndex] = 9;
                            finalDigits[targetIndex] = 9 + totalOffset;
                        }
                        else
                        {
                            finalDigits[rule.sourceIndex] = 1 - totalOffset;
                            finalDigits[targetIndex] = 1;
                        }
                    }

                }
                targetIndex++;
            }

            return long.Parse(string.Join("", finalDigits));
        }
    }

    public class ArithmeticLogicUnit
    {
        private int w, x, y, z;
        private Dictionary<string, int> registers;

        public ArithmeticLogicUnit(List<string> instructions)
        {
            this.Instructions = instructions;
            this.registers = new Dictionary<string, int>();
            registers["x"] = 0;
            registers["y"] = 0;
            registers["z"] = 0;
            registers["w"] = 0;
        }

        private List<string> Instructions { get; }

        public int Run(List<int> inputs)
        {
            int inputPos = 0;
            int val;
            string operand;
            foreach (string instruction in this.Instructions)
            {
                string[] instrParts = instruction.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                switch (instrParts[0])
                {
                    //inp a -Read an input value and write it to variable a.
                    case "inp":
                        this.registers[instrParts[1]] = inputs[inputPos];
                        inputPos++;
                        break;

                    // add a b - Add the value of a to the value of b, then store the result in variable a.
                    case "add":
                        operand = instrParts[2];
                        val = this.GetValue(operand);
                        this.registers[instrParts[1]] += val;
                        break;

                    // mul a b - Multiply the value of a by the value of b, then store the result in variable a.
                    case "mul":
                        operand = instrParts[2];
                        val = this.GetValue(operand);
                        this.registers[instrParts[1]] *= val;
                        break;

                    //div a b - Divide the value of a by the value of b, truncate the result to an integer, then store the result in variable a. (Here, "truncate" means to round the value toward zero.)
                    case "div":
                        operand = instrParts[2];
                        val = this.GetValue(operand);
                        double result = this.registers[instrParts[1]] / (1.0 * val);
                        this.registers[instrParts[1]] = (int)Math.Floor(result);
                        break;

                    //mod a b - Divide the value of a by the value of b, then store the remainder in variable a. (This is also called the modulo operation.)
                    case "mod":
                        operand = instrParts[2];
                        val = this.GetValue(operand);
                        int remainder;
                        Math.DivRem(this.registers[instrParts[1]], val, out remainder);
                        this.registers[instrParts[1]] = remainder;
                        break;

                    //eql a b - If the value of a and b are equal, then store the value 1 in variable a.
                    //Otherwise, store the value 0 in variable a.
                    case "eql":
                        operand = instrParts[2];
                        val = this.GetValue(operand);
                        this.registers[instrParts[1]] = this.registers[instrParts[1]] == val ? 1 : 0;
                        break;

                    default:
                        throw new ArgumentException($"Unexpected instruction {instrParts[0]}.");
                }
            }

            return this.registers["z"];
        }

        private int GetValue(string operand)
        {
            if (operand == "x" || operand == "y" || operand == "z" || operand == "w")
            {
                return this.registers[operand];
            }
            else
            {
                return Int32.Parse(operand);
            }
        }
    }
}
