using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec20
    {
        public static void Solve(bool show = false)
        {
            bool first = true;
            string algorithm = null;
            var lines = new List<string>();

            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec20.txt"))
            {
                if (first)
                {
                    algorithm = line;
                    first = false;
                }
                else if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            // start out with a grid that is nine times as big as the input image.
            var grid = new char[lines.Count * 3, lines[0].Length * 3];
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i >= rows / 3 && i < (2 * rows)/3 &&
                        j >= cols / 3 && j < (2 * cols)/3)
                    {
                        grid[i, j] = lines[i - (rows / 3)][j - (cols / 3)];
                    }
                    else
                    {
                        grid[i, j] = '.';
                    }
                }
            }
            char defaultVal = '.';

            if (show)
            {
                PrintGrid(grid);
            }

            int numLit = 0;
            for (int steps = 0; steps < 2; steps++)
            {
                numLit = 0;
                var nextGrid = new char[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        int index = GetValue(grid, i, j, rows, cols, defaultVal);
                        nextGrid[i, j] = algorithm[index];
                        if (nextGrid[i, j] == '#')
                        {
                            numLit++;
                        }
                    }
                }

                int defaultIndex = (defaultVal == '.') ? 0 : 511;
                defaultVal = algorithm[defaultIndex];

                grid = nextGrid;

                if (show)
                {
                    PrintGrid(grid);
                }
            }

            Console.WriteLine("{0} pixels are lit.", numLit);
        }

        private static int GetValue(char[,] grid, int x, int y, int maxX, int maxY, char defaultVal)
        {
            var sb = new StringBuilder();
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < maxX && j >= 0 && j < maxY)
                    {
                        sb.Append(grid[i, j]);
                    }
                    else
                    {
                        sb.Append(defaultVal);
                    }
                }
            }

            var strVal = sb.ToString();
            int sum = 0;
            int mul = 1;
            for (int i = strVal.Length - 1; i >= 0; i--)
            {
                int val = strVal[i] == '#' ? 1 : 0;
                sum += (mul * val);
                mul *= 2;
            }

            return sum;
        }
        
        private static void PrintGrid(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
