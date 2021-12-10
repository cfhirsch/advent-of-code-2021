using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec10
    {
        public static void Solve_Part_One()
        {
            long score = 0;
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec10.txt"))
            {
                var stack = new Stack<char>();
                foreach(char ch in line)
                {
                    if (IsChunkBegin(ch))
                    {
                        stack.Push(ch);
                    }
                    else
                    {
                        char endCh = stack.Pop();
                        char expectedCh = GetExpectedEndChar(endCh);
                        if (expectedCh != ch)
                        {
                            Console.WriteLine("Expected {0}, but found {1} instead.", expectedCh, ch);
                            score += GetScore(ch);
                            continue;
                        }
                    }
                }
            }

            Console.WriteLine("Total score = {0}.", score);
        }

        private static char GetExpectedEndChar(char ch)
        {
            ;
            switch (ch)
            {
                case '(':
                    return ')';

                case '[':
                    return ']';

                case '{':
                    return '}';

                case '<':
                    return '>';

                default:
                    throw new ArgumentException($"Unexpected character {ch}.");
            }
        }

        private static int GetScore(char ch)
        {
            /*): 3 points.
]: 57 points.
}: 1197 points.
>: 25137 points.*/

            switch (ch)
            {
                case ')':
                    return 3;

                case ']':
                    return 57;

                case '}':
                    return 1197;

                case '>':
                    return 25137;

                default:
                    throw new ArgumentException($"Unexpected character {ch}.");
            }
        }

        private static bool IsChunkBegin(char ch)
        {
            return (ch == '(' || ch == '[' || ch == '{' || ch == '<');
        }
    }
}
