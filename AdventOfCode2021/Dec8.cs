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

        public static void Solve_Part_Two()
        {
            List<string> entries = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec8.txt").ToList();

            long sum = 0;
            foreach (string entry in entries)
            {
                string[] entryParts = entry.Split('|');
                string[] digitString = entryParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] samples = entryParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var digitDict = new Dictionary<int, string>();

                // The entry of length 2 = 1.
                digitDict[1] = digitString.Where(s => s.Length == 2).First();

                // The entry of length 3 = 7.
                digitDict[7] = digitString.Where(s => s.Length == 3).First();

                // The entry of length 4 = 4.
                digitDict[4] = digitString.Where(s => s.Length == 4).First();

                // The entry of length 7 = 8.
                digitDict[8] = digitString.Where(s => s.Length == 7).First();

                // Of the entries of length 5, the entry that has both chars in 1 = 3.
                digitDict[3] = digitString.Where(s => s.Length == 5 && ContainsAll(s, digitDict[1])).First();

                // Of the remaining entries of length 5, the entry that has 3 chars in common with 4 = 5.
                digitDict[5] = digitString.Where(s => s.Length == 5 && !digitDict.Any(kvp => kvp.Value == s) && s.Intersect(digitDict[4]).Count() == 3).First();

                // The remaining entry of length 5 must be 2.
                digitDict[2] = digitString.Where(s => s.Length == 5 && !digitDict.Any(kvp => kvp.Value == s)).First();

                // The entry of length 6 that has all the chars in 4 = 9.
                digitDict[9] = digitString.Where(s => s.Length == 6 && ContainsAll(s, digitDict[4])).First();

                // Out of the remaining entries of length 6, the one that contains 1 = 0.
                digitDict[0] = digitString.Where(s => s.Length == 6 && !digitDict.Any(kvp => kvp.Value == s) && ContainsAll(s, digitDict[1])).First();

                // The remaining entry of length 6 must be 6.
                digitDict[6] = digitString.Where(s => s.Length == 6 && !digitDict.Any(kvp => kvp.Value == s)).First();

                var sb = new StringBuilder();
                foreach (string sample in samples)
                {
                    List<KeyValuePair<int, string>> candidates = digitDict.Where(kvp => kvp.Value.Length == sample.Length).ToList();
                    if (candidates.Count == 1)
                    {
                        sb.Append(candidates[0].Key);
                    }
                    else
                    {
                        foreach (KeyValuePair<int, string> candidate in candidates)
                        {
                            if (AreEquivalent(sample, candidate.Value))
                            {
                                sb.Append(candidate.Key);
                                break;
                            }
                        }
                    }
                }

                int value = Int32.Parse(sb.ToString());
                Console.WriteLine(value);
                sum += value;
            }

            Console.WriteLine("Sum = {0}", sum);
        }

        private static bool AreEquivalent(string first, string second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            foreach (Char ch in first)
            {
                if (!second.Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ContainsAll(string first, string second)
        {
            foreach (Char ch in second)
            {
                if (!first.Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<int> NewList(
            string output,
            char ch,
            Dictionary<char, List<int>> outputCandidates,
            Dictionary<int, List<int>> outputSegments,
            int digit)
        {
            if (!outputSegments.ContainsKey(digit))
            {
                return outputCandidates[ch];
            }
            
            if (output.Contains(ch))
            {
                return outputSegments[digit];
            }
            else
            {
                return TrimList(
                    outputCandidates[ch],
                    outputSegments[digit]);
            }
        }

        private static List<int> TrimList(List<int> list, List<int> toRemove)
        {
            return list.Except(toRemove).ToList();
        }
    }
}
