using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec9
    {
        public static void Solve_Part_One(bool show = false)
        {
            List<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec9.txt").ToList();
            var map = new int[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    map[i, j] = Int32.Parse(lines[i][j].ToString());
                }
            }

            var lowPoints = new List<Point>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    bool lowPoint = true;
                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            if (k < 0 || k >= map.GetLength(0) || l < 0 || l >= map.GetLength(1))
                            {
                                continue;
                            }

                            if (k == i && l == j)
                            {
                                continue;
                            }

                            if (map[k, l] <= map[i, j])
                            {
                                lowPoint = false;
                                break;
                            }
                        }

                        if (!lowPoint)
                        {
                            break;
                        }
                    }

                    if (lowPoint)
                    {
                        lowPoints.Add(new Point(i, j));
                    }
                }
            }

            if (show)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (lowPoints.Contains(new Point(i, j)))
                        {
                            Console.Write("\x1b[1m{0}\x1b[0m ", map[i, j]);
                        }
                        else
                        {
                            Console.Write("{0} ", map[i, j]);
                        }
                    }

                    Console.WriteLine();
                }
            }

            int risk = 0;
            foreach (Point pt in lowPoints)
            {
                risk += map[pt.X, pt.Y] + 1;
            }

            Console.WriteLine("Risk = {0}.", risk);
        }
    }
}
