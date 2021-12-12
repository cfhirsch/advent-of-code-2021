using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Dec12
    {
        public static void Solve(bool show = false, bool partTwo = false)
        {
            // Build the map.
            var map = new Dictionary<string, HashSet<string>>();
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec12.txt"))
            {
                string[] lineParts = line.Split("-", StringSplitOptions.RemoveEmptyEntries);
                AppendNeighbor(map, lineParts[0], lineParts[1]);
                AppendNeighbor(map, lineParts[1], lineParts[0]);
            }

            int numPaths = 0;
            var pathQueue = new Queue<List<string>>();
            pathQueue.Enqueue(new List<string>() { "start" });
            while (pathQueue.Count > 0)
            {
                List<string> path = pathQueue.Dequeue();
                string current = path.Last();
                foreach (string neighbor in map[current])
                {
                    // A path visits lower case caves at most once.
                    if (FilterNeighbor(path, neighbor, partTwo))
                    {
                        continue;
                    }

                    if (neighbor == "end")
                    {
                        if (show)
                        {
                            foreach (string node in path)
                            {
                                Console.Write("{0},", node);
                            }

                            Console.WriteLine("end");
                        }

                        numPaths++;
                    }
                    else
                    {
                        var newPath = new List<string>();
                        newPath.AddRange(path);
                        newPath.Add(neighbor);
                        pathQueue.Enqueue(newPath);
                    }
                }
            }

            Console.WriteLine("There are {0} paths.", numPaths);
        }

        private static void AppendNeighbor(Dictionary<string, HashSet<string>> map, string current, string neighbor)
        {
            if (!map.ContainsKey(current))
            {
                map[current] = new HashSet<string>();
            }

            map[current].Add(neighbor);
        }

        private static bool FilterNeighbor(List<string> path, string neighbor, bool isPartTwo)
        {
            if (!isPartTwo)
            {
                return (IsLowerCase(neighbor) && path.Contains(neighbor));
            }

            if (neighbor == "start")
            {
                return true;
            }

            if (IsLowerCase(neighbor))
            {
                bool lowerCaseCaveVisitedTwice = path.Where(p => IsLowerCase(p))
                        .GroupBy(p => p)
                        .Select(p => new { Char = p.Key, Count = p.Count() })
                        .Any(p => p.Count >= 2);

                if (lowerCaseCaveVisitedTwice && path.Contains(neighbor))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsLowerCase(string str)
        {
            return Char.IsLower(str[0]);
        }
    }
}
