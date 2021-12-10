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

        public static void Solve_Part_Two(bool show = false)
        {
            var completionScores = new List<long>();
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec10.txt"))
            {
                if (show)
                {
                    Console.Write("{0} ", line);
                }

                bool correct = true;
                var stack = new Stack<char>();
                foreach (char ch in line)
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
                            if (show)
                            {
                                Console.WriteLine("Expected {0}, but found {1} instead.", expectedCh, ch);
                            }

                            correct = false;
                            break;
                        }
                    }
                }

                if (correct)
                {
                    long score = 0;
                    while (stack.Count > 0)
                    {
                        char next = stack.Pop();
                        char endCh = GetExpectedEndChar(next);

                        if (show)
                        {
                            Console.Write(endCh);
                        }

                        score = (5 * score) + GetCompletionScore(endCh);
                    }

                    if (show)
                    {
                        Console.WriteLine(" - {0} total points.", score);
                    }

                    completionScores.Add(score);
                }
            }

            completionScores.Sort();

            int index = completionScores.Count / 2;
            Console.WriteLine("{0} scores, middle score will be at {1}.", completionScores.Count, index);

            Console.WriteLine("Score = {0}.", completionScores[index]);
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

        private static int GetCompletionScore(char ch)
        {
            switch (ch)
            {
                case ')':
                    return 1;

                case ']':
                    return 2;

                case '}':
                    return 3;

                case '>':
                    return 4;

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
