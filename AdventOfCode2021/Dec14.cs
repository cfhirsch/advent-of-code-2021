using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec14
    {
        public static void Solve()
        {
            // Parse input.
            bool first = true;
            string polymer = null;
            var productionRules = new List<Tuple<string, string>>();
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec14.txt"))
            {
                if (first)
                {
                    polymer = line;
                    first = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] lineParts = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
                productionRules.Add(new Tuple<string, string>(lineParts[0].Trim(), lineParts[1].Trim()));
            }

            Console.WriteLine("Template:\t{0}", polymer);

            for (int i = 1; i <= 10; i++)
            {
                var sb = new StringBuilder();

                // Apply rules.
                for (int j = 0; j < polymer.Length - 1; j++)
                {
                    string match = polymer.Substring(j, 2);
                    Tuple<string, string> rule = productionRules.FirstOrDefault(p => p.Item1 == match);
                    if (rule != null)
                    {
                        sb.AppendFormat("{0}{1}", match[0], rule.Item2);
                    }
                    else
                    {
                        sb.Append(match[0]);
                    }
                }

                sb.Append(polymer[polymer.Length - 1]);

                polymer = sb.ToString();

                if (i <= 4)
                {
                    Console.WriteLine("After step {0}: {1}", i, polymer);
                }
            }

            IEnumerable<IGrouping<char, char>> groupings = polymer.GroupBy(s => s).OrderByDescending(s => s.Count());
            
            IGrouping<char, char> mostCommon = groupings.First();
            int mostCommonCount = mostCommon.Count();
            Console.WriteLine("Most common char is {0} which occurs {1} times.", mostCommon.Key, mostCommonCount);

            IGrouping<char, char> leastCommon = groupings.Last();
            int leastCommonCount = leastCommon.Count();
            Console.WriteLine("Least common char is {0} which occurs {1} times.", leastCommon.Key, leastCommonCount);

            Console.WriteLine("Most common minus least common = {0}.", mostCommonCount - leastCommonCount);
        }
    }
}
