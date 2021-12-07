using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec7
    {
        public static void Solve_Part_One()
        {
            List<int> positions = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec7.txt")
                                    .First()
                                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => Int32.Parse(s))
                                    .ToList();

            int median = Median(positions);
            int cost = positions.Select(p => Math.Abs(p - median)).Sum();
            Console.WriteLine("Median = {0}, Cost = {1}", median, cost);
        }

        public static void Solve_Part_Two()
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
                long fuel = positions.Select(s => Distance(s, i)).Sum();
                if (fuel < minFuel)
                {
                    minFuel = fuel;
                    minPos = i;
                }
            }

            Console.WriteLine("MinPos = {0}, MinFuel = {1}", minPos, minFuel);
        }

        private static int Distance(int x, int y)
        {
            int diff = Math.Abs(x - y);
            return (diff * (diff + 1)) / 2;
        }

        // Following code to calculate Median taken from https://stackoverflow.com/questions/4140719/calculate-median-in-c-sharp.
        /// <summary>
        /// Partitions the given list around a pivot element such that all elements on left of pivot are <= pivot
        /// and the ones at thr right are > pivot. This method can be used for sorting, N-order statistics such as
        /// as median finding algorithms.
        /// Pivot is selected randomly if random number generator is supplied else its selected as last element in the list.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
        /// </summary>
        private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
        {
            if (rnd != null)
                list.Swap(end, rnd.Next(start, end + 1));

            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++)
            {
                if (list[i].CompareTo(pivot) <= 0)
                    list.Swap(i, ++lastLow);
            }
            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T>
        {
            return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
        }

        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
        {
            while (true)
            {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n)
                    return list[pivotIndex];

                if (n < pivotIndex)
                    end = pivotIndex - 1;
                else
                    start = pivotIndex + 1;
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
                return;
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T Median<T>(this IList<T> list) where T : IComparable<T>
        {
            return list.NthOrderStatistic((list.Count - 1) / 2);
        }

        public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
        {
            var list = sequence.Select(getValue).ToList();
            var mid = (list.Count - 1) / 2;
            return list.NthOrderStatistic(mid);
        }
    }
}
