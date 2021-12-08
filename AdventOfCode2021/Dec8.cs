using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec8
    {
        public static void Solve_Part_One()
        {
            List<string> entries = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec8.txt").ToList();
            // 1 uses 2 segments
            // 4 uses 4 segments
            // 7 uses 3 segments
            // 8 uses 7 segments
            int numUniqueSegments = 0;
            foreach (string entry in entries)
            {
                string[] entryParts = entry.Split('|');
                string[] outputs = entryParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string output in outputs)
                {
                    if (output.Length == 2 ||
                        output.Length == 3 ||
                        output.Length == 4 ||
                        output.Length == 7)
                    {
                        numUniqueSegments++;
                    }
                }
            }

            Console.WriteLine("Num instances of digits that use unique number of seqments = {0}.", numUniqueSegments);
        }
    }
}
