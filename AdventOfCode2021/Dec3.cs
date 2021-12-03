using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec3
    {
        public static void Solve_Part_One()
        {
            var gamma = new StringBuilder();
            var epsilon = new StringBuilder();

            List<string> report = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec3.txt").ToList();
            List<int> oneCounts = Enumerable.Repeat(0, report[0].Length).ToList();
            foreach (string line in report)
            {
                for (int i = 0; i < oneCounts.Count; i++)
                {
                    if (line[i] == '1')
                    {
                        oneCounts[i]++;
                    }
                }
            }

            for (int i = 0; i < oneCounts.Count; i++)
            {
                if (oneCounts[i] >= (report.Count / 2))
                {
                    gamma.Append("1");
                    epsilon.Append("0");
                }
                else
                {
                    gamma.Append("0");
                    epsilon.Append("1");
                }
            }

            Console.WriteLine("Binary: gamma = {0}, epsilon = {1}.", gamma, epsilon);

            int gammaDec = BinaryToDecimal(gamma.ToString());
            int epsilonDec = BinaryToDecimal(epsilon.ToString());

            Console.WriteLine("Decimal: gamma = {0}, epsilon = {1}, power level = {2}",
                gammaDec, epsilonDec, gammaDec * epsilonDec);
        }

        public static void Solve_Part_Two()
        {
            var gamma = new StringBuilder();
            var epsilon = new StringBuilder();

            List<string> report = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec3.txt").ToList();
            List<int> oneCounts = GetCounts(report);

            for (int i = 0; i < oneCounts.Count; i++)
            {
                if (oneCounts[i] >= (report.Count / 2))
                {
                    gamma.Append("1");
                    epsilon.Append("0");
                }
                else
                {
                    gamma.Append("0");
                    epsilon.Append("1");
                }
            }

            string oxGenStr = Eliminate(report, oneCounts, majority: true);
            string co2ScrubStr = Eliminate(report, oneCounts, majority: false);

            Console.WriteLine("Binary: O Gen = {0}, Co2 Scrub = {1}", oxGenStr, co2ScrubStr);

            int oxGen = BinaryToDecimal(oxGenStr);
            int co2Scrub = BinaryToDecimal(co2ScrubStr);

            Console.WriteLine(
                "Decimal: O Gen = {0}, Co2 Scrub = {1}, Result = {2}", 
                oxGen,
                co2Scrub,
                oxGen * co2Scrub);
        }

        private static List<int> GetCounts(List<string> candidates)
        {
            List<int> oneCounts = Enumerable.Repeat(0, candidates[0].Length).ToList();
            foreach (string line in candidates)
            {
                for (int i = 0; i < oneCounts.Count; i++)
                {
                    if (line[i] == '1')
                    {
                        oneCounts[i]++;
                    }
                }
            }

            return oneCounts;
        }

        private static string Eliminate(
            List<string> candidates, 
            List<int> oneCounts, 
            bool majority = true)
        {
            int length = candidates[0].Length;
            for (int i = 0; i < length; i++)
            {
                char mask;
                if (majority)
                {
                    mask = (oneCounts[i] >= (candidates.Count / 2.0)) ? '1' : '0';
                }
                else
                {
                    mask = (oneCounts[i] < (candidates.Count / 2.0)) ? '1' : '0';
                }

                candidates = candidates.Where(s => s[i] == mask).ToList();
                if (candidates.Count <= 1)
                {
                    break;
                }

                oneCounts = GetCounts(candidates);
            }

            if (candidates.Count == 0)
            {
                throw new Exception("Oops, no match!");
            }

            return candidates[0];
        }

        private static int BinaryToDecimal(string input)
        {
            int multiplier = 1;
            int result = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                int digit = (input[i] == '1') ? 1 : 0;
                result += (multiplier * digit);
                multiplier *= 2;
            }

            return result;
        }
    }
}
