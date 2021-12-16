using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Dec15
    {
        public static void Solve(bool show = false, bool partTwo = false)
        {
            // Load in puzzle input.
            List<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec15.txt").ToList();
            var map = new int[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    map[i, j] = Int32.Parse(lines[i][j].ToString());
                }
            }

            if (partTwo)
            {
                int x = map.GetLength(0);
                int y = map.GetLength(1);
                var newMap = new int[x * 5, y * 5];
               
                // Copy in original array.
                for (int i = 0; i < newMap.GetLength(0); i++)
                {
                    for (int j = 0; j < newMap.GetLength(1); j++)
                    {
                        int newVal = map[i % x, j % y] + (i / x) + (j / y);
                        if (newVal > 9)
                        {
                            newVal -= 9;
                        }

                        newMap[i, j] = newVal;
                    }
                }

                map = newMap;
            }

            if (show)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        Console.Write(map[i, j]);
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine();

            // A* finds a path from start to goal.
            // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
            // The set of discovered nodes that may need to be (re-)expanded.
            // Initially, only the start node is known.
            // This is usually implemented as a min-heap or priority queue rather than a hash-set.
            // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
            // how short a path from start to finish can be if it goes through n.
            var fScore = new Dictionary<Point, int>();

            var startPoint = new Point(0, 0);
            var goalPoint = new Point(map.GetLength(0) - 1, map.GetLength(1) - 1);
            fScore[startPoint] = ManhattenDistance(startPoint, goalPoint);

            var start = new Dec15QueueEntry(startPoint, fScore[startPoint]);
            var goal = new Dec15QueueEntry(goalPoint, Int32.MaxValue);

            var openSet = new PriorityQueue<Dec15QueueEntry>();
            openSet.Enqueue(start);

            // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
            // to n currently known.
            var cameFrom = new Dictionary<Point, Point>();

            // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
            var gScore = new Dictionary<Point, int>();
            gScore[start.Point] = 0;

            List<Point> path = null;
            
            while (openSet.Count() > 0)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Queue size = {0}.", openSet.Count());

                // This operation can occur in O(1) time if openSet is a min-heap or a priority queue
                Dec15QueueEntry current = openSet.Dequeue();

                Console.WriteLine("Current = {0}", current.Point);
                Console.WriteLine("MaxX = {0}, MaxY = {1}", map.GetLength(0) - 1, map.GetLength(1) - 1);

                //current := the node in openSet having the lowest fScore[] value
                if (current.Point == goal.Point)
                {
                    Point currentPoint = current.Point;
                    path = new List<Point>();
                    path.Add(currentPoint);
                    while (cameFrom.ContainsKey(currentPoint))
                    {
                        currentPoint = cameFrom[currentPoint];
                        path.Insert(0, currentPoint);
                    }

                    break;
                    //return reconstruct_path(cameFrom, current)
                }

                foreach (Point neighbor in GetNeighbors(current.Point, map.GetLength(0), map.GetLength(1)))
                {
                    // d(current,neighbor) is the weight of the edge from current to neighbor
                    // tentative_gScore is the distance from start to the neighbor through current
                    int tentative_gScore = GetScore(gScore, current.Point) + map[neighbor.X, neighbor.Y];
                    if (tentative_gScore < GetScore(gScore, neighbor))
                    {
                        // This path to neighbor is better than any previous one. Record it!
                        cameFrom[neighbor] = current.Point;
                        gScore[neighbor] = tentative_gScore;
                        fScore[neighbor] = tentative_gScore + ManhattenDistance(neighbor, goal.Point);

                        var neighborQueueEntry = openSet.Data.FirstOrDefault(d => d.Point == neighbor);
                        // if neighbor not in openSet

                        if (neighborQueueEntry == null)
                        {
                            neighborQueueEntry = new Dec15QueueEntry(neighbor, fScore[neighbor]);
                            openSet.Enqueue(neighborQueueEntry);
                        }
                        else
                        {
                            neighborQueueEntry.Weight = fScore[neighbor];
                            openSet.Reprioritize(neighborQueueEntry);
                        }
                    }
                }
            }

            if (path == null)
            {
                Console.WriteLine("Failed to find path!");
            }
            else
            {
                int cost = 0;
                foreach (Point point in path.Skip(1))
                {
                    cost += map[point.X, point.Y];
                }

                Console.WriteLine("Cost of minimal path = {0}.", cost);
            }
        }

        private static int GetScore(Dictionary<Point, int> dict, Point key)
        {
            if (!dict.ContainsKey(key))
            {
                return Int32.MaxValue;
            }

            return dict[key];
        }

        private static IEnumerable<Point> GetNeighbors(Point current, int maxX, int maxY)
        {
            // North
            if (current.X > 0)
            {
                yield return new Point(current.X - 1, current.Y);
            }

            // East
            if (current.Y < maxY - 1)
            {
                yield return new Point(current.X, current.Y + 1);
            }

            // South
            if (current.X < maxX - 1)
            {
                yield return new Point(current.X + 1, current.Y);
            }

            // West
            if (current.Y > 0)
            {
                yield return new Point(current.X, current.Y - 1);
            }
        }

        private static int ManhattenDistance(Point source, Point target)
        {
            return Math.Abs(target.X - source.X) + Math.Abs(target.Y - source.Y);
        }
    }

    public class Dec15QueueEntry : IComparable<Dec15QueueEntry>, IEquatable<Dec15QueueEntry>
    {
        public Dec15QueueEntry(Point point, int weight)
        {
            this.Point = point;
            this.Weight = weight;
            this.Neighbors = new HashSet<Dec15QueueEntry>();
        }

        public Point Point { get; }

        public int Weight { get; set; }

        public HashSet<Dec15QueueEntry> Neighbors { get; }

        public int CompareTo([AllowNull] Dec15QueueEntry other)
        {
            return this.Weight.CompareTo(other.Weight);
        }

        public bool Equals([AllowNull] Dec15QueueEntry other)
        {
            return this.Point.Equals(other.Point);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Dec15QueueEntry;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Point.GetHashCode();
        }
    }
}
