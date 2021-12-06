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
            List<int> timers = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec6.txt")
                                .First()
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => Int32.Parse(s))
                                .ToList();

            for (int day = 1; day <= 80; day++)
            {
                int numEightsToAdd = 0;
                for (int i = 0; i < timers.Count; i++)
                {
                    if (timers[i] == 0)
                    {
                        timers[i] = 6;
                        numEightsToAdd++;
                    }
                    else
                    {
                        timers[i]--;
                    }
                }

                timers.AddRange(Enumerable.Repeat(8, numEightsToAdd));

                Console.WriteLine("Day {0}: {1} fish.", day, timers.Count);
            }
        }
    }
}
