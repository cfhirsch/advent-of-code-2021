using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021
{
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
    }
}
