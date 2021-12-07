using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
    using System.Linq;

    public static class Dec1
    {
        public static void Solve_Part_One()
        {
            int numIncrease = 0;
            int previous = Int32.MaxValue;
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec1.txt"))
            {
                int current = Int32.Parse(line);
                if (current > previous)
                {
                    numIncrease++;
                }

                previous = current;
            }

            Console.WriteLine("{0} increases.", numIncrease);
        }

        public static void Solve_Part_Two()
        {
            List<int> readings =
                PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec1.txt")
                    .Select(s => Int32.Parse(s)).ToList();
            int numIncreases = 0;

            int previous = Int32.MaxValue;
            for (int i = 0; i < readings.Count - 2; i++)
            {
                int current = readings[i] + readings[i + 1] + readings[i + 2];
                if (current > previous)
                {
                    numIncreases++;
                }

                previous = current;
            }

            Console.WriteLine("Num increases = {0}.", numIncreases);
        }
    }
}
