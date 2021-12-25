using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec25
    {
        public static void Solve_PartOne(bool show = false)
        {
            List<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec25.txt").ToList();
            int rows = lines.Count;
            int cols = lines[0].Length;

            var grid = new char[rows, cols];

            int i, j;
            for (i = 0; i < rows; i++)
            {
                for (j = 0; j < cols; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            PrintGrid(grid, show);

            int step = 0;
            int numCucumbersMoved;
            char[,] nextGrid;
            
            do
            {
                numCucumbersMoved = 0;
                step++;

                nextGrid = new char[rows, cols];
                // First pass, east facing cucumbers try to move.
                for (i = 0; i < rows; i++)
                {
                    j = 0;
                    while (j < cols)
                    {
                        if (grid[i, j] == '>')
                        {
                            int nextJ = (j + 1) % cols;
                            if (grid[i, nextJ] == '.')
                            {
                                nextGrid[i, j] = '.';
                                nextGrid[i, nextJ] = '>';
                                numCucumbersMoved++;
                                j++;
                            }
                            else
                            {
                                nextGrid[i, j] = grid[i, j];
                            }
                        }
                        else
                        {
                            nextGrid[i, j] = grid[i, j];
                        }

                        j++;
                    }
                }

                grid = nextGrid;
                nextGrid = new char[rows, cols];

                // Second pass, south facing cucumbers try to move.
                
                for (j = 0; j < cols; j++)
                {
                    i = 0;
                    while (i < rows)
                    {
                        if (grid[i, j] == 'v')
                        {
                            int nextI = (i + 1) % rows;
                            if (grid[nextI, j] == '.')
                            {
                                nextGrid[i, j] = '.';
                                nextGrid[nextI, j] = 'v';
                                numCucumbersMoved++;
                                i++;
                            }
                            else
                            {
                                nextGrid[i, j] = grid[i, j];
                            }
                        }
                        else
                        {
                            nextGrid[i, j] = grid[i, j];
                        }

                        i++;
                    }
                }

                grid = nextGrid;

                if (show)
                {
                    Console.WriteLine("After {0} steps.", step);
                }

                Console.WriteLine("Step {0}, {1} sea cucumbers moved.", step, numCucumbersMoved);

                PrintGrid(grid, show);
            } while (numCucumbersMoved != 0);

            Console.WriteLine("Sea cucumbers stopped moving after {0} steps.", step);
        }

        private static void PrintGrid(char[,] grid, bool show = false)
        {
            if (!show)
            {
                return;
            }

            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(grid[i, j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
