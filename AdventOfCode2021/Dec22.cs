using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    public static class Dec22
    {
        public static void Solve()
        {
            // on x=10..12,y=10..12,z=10..12
            var regex = new Regex(
                @"(on|off) x=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)",
                RegexOptions.Compiled);

            var grid = new bool[101, 101, 101];
            int numOnCells = 0;
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec22.txt"))
            {
                Match match = regex.Match(line);
                if (!match.Success)
                {
                    throw new Exception("Unable to parse line.");
                }

                bool on = match.Groups[1].Value == "on";
                int minX = Int32.Parse(match.Groups[2].Value);
                int maxX = Int32.Parse(match.Groups[3].Value);
                int minY = Int32.Parse(match.Groups[4].Value);
                int maxY = Int32.Parse(match.Groups[5].Value);
                int minZ = Int32.Parse(match.Groups[6].Value);
                int maxZ = Int32.Parse(match.Groups[7].Value);

                for (int x = Math.Max(minX, -50); x <= Math.Min(maxX, 50); x++)
                {
                    for (int y = Math.Max(minY, -50); y <= Math.Min(maxY, 50); y++)
                    {
                        for (int z = Math.Max(minZ, -50); z <= Math.Min(maxZ, 50); z++)
                        {
                            int i = x + 50;
                            int j = y + 50;
                            int k = z + 50;

                            if (!grid[i, j, k] && on)
                            {
                                numOnCells++;
                            }

                            if (grid[i, j, k] && !on)
                            {
                                numOnCells--;
                            }

                            grid[i, j, k] = on;
                        }
                    }
                }

                Console.WriteLine("{0} cubes are on.", numOnCells);
            }
        }
    }
}      
