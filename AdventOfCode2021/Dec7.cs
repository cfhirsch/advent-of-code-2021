using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Dec7
    {
        public static void Solve_Part_One()
        {
            Solve(MetropolitanCost);
        }

        public static void Solve_Part_Two()
        {
            Solve(GaussCost);
        }

        private static void Solve(Func<int, int, int> costFunc)
        {
            List<int> positions = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec7.txt")
                                    .First()
                                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => Int32.Parse(s))
                                    .ToList();

            int min = positions.Min();
            int max = positions.Max();

            int minPos = Int32.MinValue;
            long minFuel = Int32.MaxValue;
            for (int i = min; i <= max; i++)
            {
                long fuel = positions.Select(s => costFunc(s, i)).Sum();
                if (fuel < minFuel)
                {
                    minFuel = fuel;
                    minPos = i;
                }
            }

            Console.WriteLine("MinPos = {0}, MinFuel = {1}", minPos, minFuel);
        }

        private static int MetropolitanCost(int x, int y)
        {
            return Math.Abs(x - y);
        }

        private static int GaussCost(int x, int y)
        {
            int diff = Math.Abs(x - y);

            // Using Gauss's formula; the distance is the sum 1 + 2 + ... + |x - y|
            // 1 + 2 + ... + n = (n * (n + 1))/2
            return (diff * (diff + 1)) / 2;
        }
    }
}