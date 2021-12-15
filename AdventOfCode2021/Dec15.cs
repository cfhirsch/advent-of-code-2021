using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec15
    {
        public static void Solve()
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

            /*function Dijkstra(Graph, source):
2
3      create vertex set Q
4
5      for each vertex v in Graph:            
6          dist[v] ← INFINITY                 
7          prev[v] ← UNDEFINED                
8          add v to Q                     
9      dist[source] ← 0     */
            var vertices = new HashSet<Point>();
            var dist = new Dictionary<Point, int>();
            var prev = new Dictionary<Point, Point?>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    var pt = new Point(i, j);
                    dist[pt] = Int32.MaxValue;
                    prev[pt] = null;
                    vertices.Add(pt);
                }
            }

            var source = new Point(0, 0);
            var target = new Point(map.GetLength(0) - 1, map.GetLength(1) - 1);
            dist[source] = 0;
            /*while Q is not empty:
12          u ← vertex in Q with min dist[u]   
13                                             
14          remove u from Q - terminate if u = target
15         
16          for each neighbor v of u still in Q:
17              alt ← dist[u] + length(u, v)
18              if alt < dist[v]:              
19                  dist[v] ← alt
20                  prev[v] ← u
21
22      return dist[], prev[]*/
            while (vertices.Count > 0)
            {
                Point current = dist.Select(kvp => kvp).Where(kvp => vertices.Contains(kvp.Key)).OrderBy(kvp => kvp.Value).First().Key;
                vertices.Remove(current);

                if (current == target)
                {
                    break;
                }

                foreach (Point neighbor in GetNeighbors(current, map.GetLength(0), map.GetLength(1)))
                {
                    if (!vertices.Contains(neighbor))
                    {
                        continue;
                    }

                    int alt = dist[current] + map[neighbor.X, neighbor.Y];
                    if (alt < dist[neighbor])
                    {
                        dist[neighbor] = alt;
                        prev[neighbor] = current;
                    }
                }
            }

            // Reconstruct shortest path.
            /*
 * S ← empty sequence
2  u ← target
3  if prev[u] is defined or u = source:          // Do something only if the vertex is reachable
4      while u is defined:                       // Construct the shortest path with a stack S
5          insert u at the beginning of S        // Push the vertex onto the stack
6          u ← prev[u]                           // Traverse from target to source*/
            var path = new List<Point>();
            Point? u = target;
            if (prev[u.Value] != null || u == source)
            {
                while (u != null)
                {
                    path.Insert(0, u.Value);
                    u = prev[u.Value];
                }
            }

            // Calculate path cost.
            int cost = 0;
            foreach (Point pt in path.Skip(1))
            {
                cost += map[pt.X, pt.Y];
            }

            Console.WriteLine("Total cost of minimal path = {0}.", cost);
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
    }
}
