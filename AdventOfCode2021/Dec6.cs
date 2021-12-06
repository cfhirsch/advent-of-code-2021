using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec6
    {
        public static void Solve_Part_One()
        {
            Solve(80);
        }

        public static void Solve_Part_Two()
        {
            Solve(256);
        }

        public static void Solve(int numDays)
        {
            List<int> timers = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec6.txt")
                                .First()
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => Int32.Parse(s))
                                .ToList();

            // This dictionary keeps track of the number of fish at 
            // each given timer value.
            var fishDict = new Dictionary<int, long>();
            foreach (int timer in timers)
            {
                AddKey(fishDict, timer, 1);
            }

            for (int day = 1; day <= numDays; day++)
            {
                var newFishDict = new Dictionary<int, long>();
                foreach (KeyValuePair<int, long> kvp in fishDict)
                {
                    if (kvp.Key == 0)
                    {
                        AddKey(newFishDict, 6, kvp.Value);
                        AddKey(newFishDict, 8, kvp.Value);
                    }
                    else
                    {
                        AddKey(newFishDict, kvp.Key - 1, kvp.Value);
                    }
                }

                fishDict = newFishDict;
                Console.WriteLine("Day {0}: {1} fish.", day, fishDict.Select(kvp => kvp.Value).Sum());
            }
        }

        private static void AddKey(Dictionary<int, long> dict, int key, long value)
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = 0;
            }

            dict[key] += value;
        }
    }
}
