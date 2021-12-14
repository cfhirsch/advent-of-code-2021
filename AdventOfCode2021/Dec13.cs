using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec13
    {
        public static void Solve(bool show = false, bool partTwo = false)
        {
            var dots = new HashSet<Point>();
            bool loadingDots = true;

            var folds = new List<Tuple<FoldDirection, int>>();

            List<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec13.txt").ToList();

            // load points
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    loadingDots = false;
                    continue;
                }

                if (loadingDots)
                {
                    string[] lineParts = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    dots.Add(new Point(Int32.Parse(lineParts[0]), Int32.Parse(lineParts[1])));
                }
                else
                {
                    string foldInstr = line.Substring("fold along ".Length);
                    string[] foldInstrParts = foldInstr.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    switch (foldInstrParts[0])
                    {
                        case "x":
                            folds.Add(new Tuple<FoldDirection, int>(FoldDirection.Left, Int32.Parse(foldInstrParts[1].ToString())));
                            break;

                        case "y":
                            folds.Add(new Tuple<FoldDirection, int>(FoldDirection.Up, Int32.Parse(foldInstrParts[1].ToString())));
                            break;

                        default:
                            throw new ArgumentException($"Unexpected coord {foldInstrParts[0]}.");
                    }
                }
            }

            // first coord (x) increases to the right, second coord (y) increases down.
            int maxX = dots.Select(d => d.X).Max();
            int maxY = dots.Select(d => d.Y).Max();

            var paper = new char[maxY + 1, maxX + 1];
            for (int y = 0; y < paper.GetLength(0); y++)
            {
                for (int x = 0; x < paper.GetLength(1); x++)
                {
                    paper[y, x] = dots.Contains(new Point(x, y)) ? '#' : '.';
                }
            }

            if (show)
            {
                PrintPaper(paper);
            }

            foreach (Tuple<FoldDirection, int> fold in folds)
            {
                paper = Fold(paper, fold.Item2, fold.Item1);

                if (show)
                {
                    PrintPaper(paper);
                }

                if (!partTwo)
                {
                    int numDots = 0;
                    for (int y = 0; y < paper.GetLength(0); y++)
                    {
                        for (int x = 0; x < paper.GetLength(1); x++)
                        {
                            if (paper[y, x] == '#')
                            {
                                numDots++;
                            }
                        }
                    }

                    Console.WriteLine("There are {0} dots after the first fold.", numDots);
                    break;
                }
            }

            PrintPaper(paper);
        }

        private static char[,] Fold(char[,] paper, int coord, FoldDirection dir)
        {
            char[,] result;

            switch(dir)
            {
                case FoldDirection.Up:
                    // # rows = coord, # col cols
                    result = new char[coord, paper.GetLength(1)];
                    for (int i = 0; i < result.GetLength(0); i++)
                    {
                        for (int j = 0; j < result.GetLength(1); j++)
                        {
                            result[i, j] = paper[i, j];
                        }
                    }

                    for (int i = 1; i <= paper.GetLength(0) - coord; i++)
                    {
                        int sourceY = coord + i;
                        int targetY = coord - i;
                        if (targetY >= 0)
                        {
                            for (int x = 0; x < paper.GetLength(1); x++)
                            {
                                result[targetY, x] = (result[targetY, x] == '#' || paper[sourceY, x] == '#') ? '#' : '.';
                            }
                        }
                    }

                    break;

                case FoldDirection.Left:
                    // # rows =  rows, # cols = coord
                    result = new char[paper.GetLength(0), coord];
                    for (int i = 0; i < result.GetLength(0); i++)
                    {
                        for (int j = 0; j < result.GetLength(1); j++)
                        {
                            result[i, j] = paper[i, j];
                        }
                    }

                    for (int j = 1; j <= paper.GetLength(1) - coord; j++)
                    {
                        int sourceX = coord + j;
                        int targetX = coord - j;
                        if (targetX >= 0)
                        {
                            for (int y = 0; y < paper.GetLength(0); y++)
                            {
                                result[y, targetX] = (result[y, targetX] == '#' || paper[y, sourceX] == '#') ? '#' : '.';
                            }
                        }
                    }

                    break;

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }

            return result;
        }

        private static void PrintPaper(char[,] paper)
        {
            for (int y = 0; y < paper.GetLength(0); y++)
            {
                for (int x = 0; x < paper.GetLength(1); x++)
                {
                    Console.Write(paper[y, x]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    public enum FoldDirection
    {
        Up,
        Left
    }
}
