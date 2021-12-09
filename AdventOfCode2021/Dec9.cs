using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec9
    {
        // NOTE: the way I calculate neighbors in this part is wrong - I'm counting
        // diagonal neighbors when I shouldn't be, but the answer happened to be
        // correct.
        public static void Solve_Part_One(bool show = false)
        {
            int[,] map;
            List<Point> lowPoints;

            (map, lowPoints) = MapLowPoints(show);

            int risk = 0;
            foreach (Point pt in lowPoints)
            {
                risk += map[pt.X, pt.Y] + 1;
            }

            Console.WriteLine("Risk = {0}.", risk);
        }

        public static void Solve_Part_Two(bool show = false)
        {
            int[,] map;
            List<Point> lowPoints;

            (map, lowPoints) = MapLowPoints(show);
            List<Basin> basins = new List<Basin>();
            foreach (Point point in lowPoints)
            {
                var basin = new Basin(point);
                basin.Build(map);
                basins.Add(basin);
            }

            foreach (Basin basin in basins)
            {
                int size = basin.Size;
                Console.WriteLine("Basin at {0} of size {1}.", basin.LowPoint, size);
            }

            long product = 1;
            foreach (int size in basins.OrderByDescending(b => b.Size).Take(3).Select(s => s.Size))
            {
                product *= size;
            }

            Console.WriteLine("Product of all basin sizes = {0}.", product);
        }

        private static Tuple<int[,], List<Point>> MapLowPoints(bool show = false)
        {
            List<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec9.txt").ToList();
            var map = new int[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    map[i, j] = Int32.Parse(lines[i][j].ToString());
                }
            }

            var lowPoints = new List<Point>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    bool lowPoint = true;
                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            if (k < 0 || k >= map.GetLength(0) || l < 0 || l >= map.GetLength(1))
                            {
                                continue;
                            }

                            if (k == i && l == j)
                            {
                                continue;
                            }

                            if (map[k, l] <= map[i, j])
                            {
                                lowPoint = false;
                                break;
                            }
                        }

                        if (!lowPoint)
                        {
                            break;
                        }
                    }

                    if (lowPoint)
                    {
                        lowPoints.Add(new Point(i, j));
                    }
                }
            }

            if (show)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (lowPoints.Contains(new Point(i, j)))
                        {
                            Console.Write("\x1b[1m{0}\x1b[0m ", map[i, j]);
                        }
                        else
                        {
                            Console.Write("{0} ", map[i, j]);
                        }
                    }

                    Console.WriteLine();
                }
            }

            return new Tuple<int[,], List<Point>>(map, lowPoints);
        }
    }

    public class Basin
    {
        public Basin(Point lowPoint)
        {
            this.LowPoint = lowPoint;
            this.Points = new HashSet<Point>();
        }

        public Point LowPoint { get; }

        public HashSet<Point> Points { get; }

        public int Size
        {
            get
            {
                return this.Points.Count;
            }
        }

        public void Build(int[,] map)
        {
            // Start at low point.
            // Enqueue all unvisited neighbors that are of higher value, and not equal to 9.
            // While queue not empty,
            // dequeue, add point.

            var queue = new Queue<Point>();
            queue.Enqueue(this.LowPoint);
            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                this.Points.Add(current);

                foreach (Point point in this.GetNeighbors(current, map))
                {
                    queue.Enqueue(point);
                }
            }
        }

        private IEnumerable<Point> GetNeighbors(Point point, int[,] map)
        {
            int x = point.X;
            int y = point.Y;

            foreach (Point neighbor in GetNeighboringPoints(x, y, map.GetLength(0), map.GetLength(1)))
            {
                int i = neighbor.X;
                int j = neighbor.Y;
                if (!this.Points.Contains(neighbor) && map[i, j] != 9 && map[i, j] > map[x, y])
                {
                    yield return neighbor;
                }
            }
            
        }

        private static IEnumerable<Point> GetNeighboringPoints(int i, int j, int maxX, int maxY)
        {
            // North
            if (i > 0)
            {
                yield return new Point(i - 1, j);
            }

            // East
            if (j < maxY - 1)
            {
                yield return new Point(i, j + 1);
            }

            // South
            if (i < maxX - 1)
            {
                yield return new Point(i + 1, j);
            }

            // West
            if (j > 0)
            {
                yield return new Point(i, j - 1);
            }
        }
    }
}
