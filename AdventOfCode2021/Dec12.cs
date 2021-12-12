using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec12
    {
        public static void Solve_Part_One()
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
                    if (IsLowerCase(neighbor) && path.Contains(neighbor))
                    {
                        continue;
                    }

                    if (neighbor == "end")
                    {
                        foreach(string node in path)
                        {
                            Console.Write("{0},", node);
                        }

                        Console.WriteLine("end");
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

        private static bool IsLowerCase(string str)
        {
            return Char.IsLower(str[0]);
        }
    }
}
