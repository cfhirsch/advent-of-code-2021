using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec5
    {
        public static void Solve_Part_One(bool show = false)
        {
            List<Line> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec5.txt")
                                    .Select(l => new Line(l))
                                    .Where(l => (l.IsHorizontal() || l.IsVertical()))
                                    .ToList();

            int bottomX = lines.Select(l => l.Begin.X).Max();
            int temp = lines.Select(l => l.End.X).Max();
            if (temp > bottomX)
            {
                bottomX = temp;
            }

            int bottomY = lines.Select(l => l.Begin.Y).Max();
            temp = lines.Select(l => l.End.Y).Max();
            if (temp > bottomY)
            {
                bottomY = temp;
            }

            var grid = new int[bottomY + 1, bottomX + 1];
            foreach (Line line in lines)
            {
                if (line.IsHorizontal())
                {
                    int minX = Math.Min(line.Begin.X, line.End.X);
                    int maxX = Math.Max(line.Begin.X, line.End.X);
                    for (int x = minX; x <= maxX; x++)
                    {
                        grid[line.Begin.Y, x]++;
                    }
                }
                else
                {
                    int minY = Math.Min(line.Begin.Y, line.End.Y);
                    int maxY = Math.Max(line.Begin.Y, line.End.Y);
                    for (int y = minY; y <= maxY; y++)
                    {
                        grid[y, line.Begin.X]++;
                    }
                }
            }

            int numDangerous = 0;
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] >= 2)
                    {
                        numDangerous++;
                    }

                    if (show)
                    {
                        string output = grid[y, x] > 0 ? grid[y, x].ToString() : ".";
                        Console.Write(output);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Num dangerous = {0}.", numDangerous);
        }

        public static void Solve_Part_Two(bool show = false)
        {
            List<Line> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec5.txt")
                                    .Select(l => new Line(l))
                                    .ToList();

            int bottomX = lines.Select(l => l.Begin.X).Max();
            int temp = lines.Select(l => l.End.X).Max();
            if (temp > bottomX)
            {
                bottomX = temp;
            }

            int bottomY = lines.Select(l => l.Begin.Y).Max();
            temp = lines.Select(l => l.End.Y).Max();
            if (temp > bottomY)
            {
                bottomY = temp;
            }

            var grid = new int[bottomY + 1, bottomX + 1];
            foreach (Line line in lines)
            {
                if (line.IsHorizontal())
                {
                    int minX = Math.Min(line.Begin.X, line.End.X);
                    int maxX = Math.Max(line.Begin.X, line.End.X);
                    for (int x = minX; x <= maxX; x++)
                    {
                        grid[line.Begin.Y, x]++;
                    }
                }
                else if (line.IsVertical())
                {
                    int minY = Math.Min(line.Begin.Y, line.End.Y);
                    int maxY = Math.Max(line.Begin.Y, line.End.Y);
                    for (int y = minY; y <= maxY; y++)
                    {
                        grid[y, line.Begin.X]++;
                    }
                }
                else
                {
                    int xDir = (line.Begin.X > line.End.X) ? -1 : 1;
                    int yDir = (line.Begin.Y > line.End.Y) ? -1 : 1;

                    int currX = line.Begin.X - xDir;
                    int currY = line.Begin.Y - yDir;
                    do
                    {
                        currX += xDir;
                        currY += yDir;
                        grid[currY, currX]++;

                    } while (currX != line.End.X && currY != line.End.Y);
                }
            }

            
            int numDangerous = 0;
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] >= 2)
                    {
                        numDangerous++;
                    }

                    if (show)
                    {
                        string output = grid[y, x] > 0 ? grid[y, x].ToString() : ".";
                        Console.Write(output);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Num dangerous = {0}.", numDangerous);
        }
    }

    public class Line
    {
        public Line(string input)
        {
            string[] inputParts = input.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
            string[] beginParts = inputParts[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
            string[] endParts = inputParts[1].Split(",", StringSplitOptions.RemoveEmptyEntries);

            this.Begin = new Point(Int32.Parse(beginParts[0]), Int32.Parse(beginParts[1]));
            this.End = new Point(Int32.Parse(endParts[0]), Int32.Parse(endParts[1]));
        }

        public Line(Point begin, Point end)
        {
            this.Begin = begin;
            this.End = end;
        }

        public Point Begin { get; }
        public Point End { get;  }

        public bool IsHorizontal()
        {
            // All points on a horizontal line have the same Y coordinate.
            return (this.Begin.Y == this.End.Y);
        }

        public bool IsVertical()
        {
            // All points on a vertical line have the same X coordinate.
            return (this.Begin.X == this.End.X);
        }
    }

}
