using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Dec14
    {
        public static void Solve(int numIterations)
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

            // A dictionary to keep track of the count of adjacent pairs of characters in the polymer.
            var pairDict = new Dictionary<string, long>();

            // A dictionary to keep track of the count of characters in the polymer.
            var charCountDict = new Dictionary<string, long>();

            for (int i = 0; i < polymer.Length - 1; i++)
            {
                IncrementKey(pairDict, polymer.Substring(i, 2));
            }

            for (int i = 0; i < polymer.Length; i++)
            {
                IncrementKey(charCountDict, polymer[i].ToString());
            }

            for (int step = 1; step <= numIterations; step++)
            {
                var nextPairDict = pairDict.Select(k => k).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var nextCharCountDict = charCountDict.Select(k => k).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                foreach (KeyValuePair<string, long> kvp in pairDict)
                {
                    Tuple<string, string> rule = productionRules.FirstOrDefault(p => p.Item1 == kvp.Key);
                    if (rule != null)
                    {
                        // For example, the rule CH -> B will
                        // replace every instance of CH with CB and BH in terms of the pairs,
                        // and add a B for every instance of CH in the original string.
                        IncrementKey(nextPairDict, kvp.Key[0] + rule.Item2, kvp.Value);
                        IncrementKey(nextPairDict, rule.Item2 + kvp.Key[1], kvp.Value);
                        IncrementKey(nextPairDict, kvp.Key, -1 * kvp.Value);

                        IncrementKey(nextCharCountDict, rule.Item2, kvp.Value);
                    }
                }

                pairDict = nextPairDict;
                charCountDict = nextCharCountDict;
            }

            string minChar = null;
            string maxChar = null;
            long min = Int64.MaxValue;
            long max = Int64.MinValue;

            foreach (KeyValuePair<string, long> kvp in charCountDict)
            {
                if (kvp.Value < min)
                {
                    min = kvp.Value;
                    minChar = kvp.Key;
                }

                if (kvp.Value > max)
                {
                    max = kvp.Value;
                    maxChar = kvp.Key;
                }
            }

            Console.WriteLine("Most common char is {0} which occurs {1} times.", maxChar, max);

            Console.WriteLine("Least common char is {0} which occurs {1} times.", minChar, min);

            Console.WriteLine("Most common minus least common = {0}.", max - min);
        }

        private static void IncrementKey<T>(Dictionary<T, long> dict, T key, long value = 1)
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = 0;
            }

            dict[key] += value;
        }
    }
}
