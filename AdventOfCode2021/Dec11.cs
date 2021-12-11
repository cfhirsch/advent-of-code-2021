using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec11
    {
        public static void Solve(bool show = false, bool partTwo = false)
        {
            // Load in the grid.
            List<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec11.txt").ToList();
            var grid = new int[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    grid[i, j] = Int32.Parse(lines[i][j].ToString());
                }
            }

            int step = 0;
            int maxStep = 100;
            long numFlashes = 0;

            if (show)
            {
                Console.WriteLine("Step {0}:", step);
                PrintGrid(grid);
                Console.WriteLine();
            }

            int firstSynchronize = Int32.MinValue;
            while ((!partTwo && step < maxStep) || (firstSynchronize < 0))
            {
                step++;
                var flashGrid = new bool[grid.GetLength(0), grid.GetLength(1)];
                // First, the energy level of each octopus increases by 1.
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        grid[i, j]++;
                    }
                }

                bool newFlashes;
                do
                {
                    newFlashes = false;
                    var newFlashGrid = new bool[grid.GetLength(0), grid.GetLength(1)];
                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < grid.GetLength(1); j++)
                        {
                            // Then, any octopus with an energy level greater than 9 flashes.
                            // (An octopus can only flash at most once per step.)
                            if (!flashGrid[i, j] && grid[i, j] > 9)
                            {
                                flashGrid[i, j] = true;
                                newFlashGrid[i, j] = true;
                                newFlashes = true;
                                numFlashes++;
                            }
                        }
                    }

                    // This increases the energy level of all adjacent octopuses by 1,
                    // including octopuses that are diagonally adjacent.
                    // If this causes an octopus to have an energy level greater than 9, it also flashes.
                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < grid.GetLength(1); j++)
                        {
                            if (newFlashGrid[i, j])
                            {
                                foreach (Point point in GetNeighbors(i, j, grid.GetLength(0), grid.GetLength(1)))
                                {
                                    grid[point.X, point.Y]++;
                                }
                            }
                        }
                    }
                } while (newFlashes); // This process continues as long as new octopuses keep having their energy level increased beyond 9.

                //Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        if (flashGrid[i, j])
                        {
                            grid[i, j] = 0;
                        }
                    }
                }

                if (show && (step <= 10 || (step % 10 == 0)))
                {
                    Console.WriteLine("Step {0}:", step);
                    PrintGrid(grid);
                    Console.WriteLine();
                }

                bool allZeros = true;
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        if (grid[i, j] != 0)
                        {
                            allZeros = false;
                            break;
                        }
                    }

                    if (!allZeros)
                    {
                        break;
                    }
                }

                if (allZeros)
                {
                    firstSynchronize = step;
                }
            }

            if (!partTwo)
            {
                Console.WriteLine("Num flashes = {0}.", numFlashes);
            }
            else
            {
                Console.WriteLine("Octopuses first sychronize at step {0}.", firstSynchronize);
            }
        }

        private static IEnumerable<Point> GetNeighbors(int x, int y, int maxX, int maxY)
        {
            // North.
            if (x > 0)
            {
                yield return new Point(x - 1, y);
            }

            // Northeast.
            if (x > 0 && y < maxY - 1)
            {
                yield return new Point(x - 1, y + 1);
            }

            // East.
            if (y < maxY - 1)
            {
                yield return new Point(x, y + 1);
            }

            // Southeast.
            if (x < maxX - 1 && y < maxY - 1)
            {
                yield return new Point(x + 1, y + 1);
            }

            // South.
            if (x < maxX - 1)
            {
                yield return new Point(x + 1, y);
            }

            // Southwest.
            if (x < maxX - 1 && y > 0)
            {
                yield return new Point(x + 1, y - 1);
            }

            // West.
            if (y > 0)
            {
                yield return new Point(x, y - 1);
            }

            // Northwest.
            if (x > 0 && y > 0)
            {
                yield return new Point(x - 1, y - 1);
            }
        }

        private static void PrintGrid(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}
