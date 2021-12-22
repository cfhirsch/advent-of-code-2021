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
        public static void Solve_PartOne()
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

        /// <summary>
        /// My eyes glazed over on part two, so I cribbed the solution from here:
        /// https://topaz.github.io/paste/#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
        /// </summary>
        public static void Solve_PartTwo()
        {
            // on x=10..12,y=10..12,z=10..12
            var regex = new Regex(
                @"(on|off) x=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)",
                RegexOptions.Compiled);

            List<Cube> cubes = new List<Cube>();
            Dictionary<(long, long, long), bool> cubesOn = new Dictionary<(long, long, long), bool>();
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

                cubes.Add(new Cube(on, minX, maxX, minY, maxY, minZ, maxZ));

                Console.WriteLine("{0} cubes are on.", numOnCells);
            }

            var intersectedCubes = new List<Cube>();

            // cycle through each cube in our main cube list
            foreach (Cube currentCube in cubes)
            {
                List<Cube> intersectedCubesToAdd = new List<Cube>();
                // if the cube is on, add it to the add list.
                // it hasn't been intersected, but it will be in the next pass
                if (currentCube.On)
                {
                    intersectedCubesToAdd.Add(currentCube);
                }

                // check the current cube against all previously intersected cubes and
                // if there's an intersect, add that resulting cube to the list to add
                foreach (Cube previouslyIntersectedCube in intersectedCubes)
                {
                    // we send the opposite value of ON for the intersection so we don't double count on/on || off/off cubes that overlap
                    Cube newlyIntersectedCube = IntersectCubes(currentCube, previouslyIntersectedCube, !previouslyIntersectedCube.On);
                    if (newlyIntersectedCube != null)
                    {
                        intersectedCubesToAdd.Add(newlyIntersectedCube);
                    }
                }

                // add all the cubes in the ToAdd list to the intersectedCubes list
                foreach (Cube c in intersectedCubesToAdd)
                {
                    intersectedCubes.Add(c);
                }
            }

            // total up the volumes of each cube in the intersectedCubes list. 
            // since adding a new cube calculates its volume we just need to cycle through them
            // any cube that is on adds volume, and off subtracts volume.
            long part2 = 0;
            foreach (Cube c in intersectedCubes)
            {
                if (c.On)
                {
                    part2 += c.Volume;
                }
                else
                {
                    part2 -= c.Volume;
                }

                Console.WriteLine("Part 2: {0}", part2);
            }
        }

        public static Cube IntersectCubes(Cube current, Cube previouslyIntersected, bool on)
        {
            // If there's no intersection, we return null as the cubes don't overlap.
            if (current.XMin > previouslyIntersected.XMax || current.XMax < previouslyIntersected.XMin ||
                current.YMin > previouslyIntersected.YMax || current.YMax < previouslyIntersected.YMin ||
                current.ZMin > previouslyIntersected.ZMax || current.ZMax < previouslyIntersected.ZMin)
            {
                return null;
            }
            // Otherwise we return a new cube that describes the overlap
            else
            {
                return new Cube(
                    on,
                    Math.Max(current.XMin, previouslyIntersected.XMin),
                    Math.Min(current.XMax, previouslyIntersected.XMax),
                    Math.Max(current.YMin, previouslyIntersected.YMin),
                    Math.Min(current.YMax, previouslyIntersected.YMax),
                    Math.Max(current.ZMin, previouslyIntersected.ZMin),
                    Math.Min(current.ZMax, previouslyIntersected.ZMax));
            }
        }
    }

    public class Cube
    {
        public Cube(bool on, long xMin, long xMax, long yMin, long yMax, long zMin, long zMax)
        {
            On = on;
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
            ZMin = zMin;
            ZMax = zMax;

            // adding one to each axis as this subtraction isn't inclusive
            Volume = (Math.Abs(XMax - XMin + 1) * Math.Abs(YMax - YMin + 1) * Math.Abs(ZMax - ZMin + 1));
        }

        public bool On { get; set; }
        public long XMin { get; set; }
        public long XMax { get; set; }
        public long YMin { get; set; }
        public long YMax { get; set; }
        public long ZMin { get; set; }
        public long ZMax { get; set; }

        public long Volume { get; set; }
    }
}
