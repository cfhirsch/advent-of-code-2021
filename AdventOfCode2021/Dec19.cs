using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode2021
{
    /// <summary>
    /// This one stumped me, so I adopted this solution from C++ into C#:
    /// https://itnext.io/modern-c-in-advent-of-code-day19-ff9525afb2ee
    /// </summary>
    public static class Dec19
    {
        public static void Solve()
        {
            var scanners = new List<Scanner>();
            var points = new HashSet<Point3D>();
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec19.txt"))
            {
                if (line.StartsWith("---"))
                {
                    points = new HashSet<Point3D>();
                }
                else if (string.IsNullOrEmpty(line))
                {
                    var scanner = new Scanner(points);
                    scanners.Add(scanner);
                }
                else
                {
                    int[] coords = line.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                       .Select(l => Int32.Parse(l)).ToArray();
                    points.Add(new Point3D(coords[0], coords[1], coords[2]));
                }
            }

            scanners.Add(new Scanner(points));

            NormalizeScanners(scanners, 12);

            var uniqueBeacons = new HashSet<Point3D>();
            foreach (Scanner scanner in scanners)
            {
                foreach (Point3D beacon in scanner.Beacons)
                {
                    uniqueBeacons.Add(beacon);
                }
            }

            Console.WriteLine("Detected {0} unique beacons.", uniqueBeacons.Count);

            int best = Int32.MinValue;
            foreach (Scanner lhs in scanners)
            {
                foreach (Scanner rhs in scanners)
                {
                    best = Math.Max(best, (lhs.Position - rhs.Position).Manhattan());
                }
            }

            Console.WriteLine("The greatest distance between two scanners is {0}.", best);
        }

        private static void NormalizeScanners(List<Scanner> scanners, int threshold)
        {
            var fixedSet = new HashSet<int>();

            var queue = new Deque<int>();
            fixedSet.Add(0);

            queue.PushBack(0);
            scanners[0].Position = new Point3D(0, 0, 0);

            while (!queue.Empty)
            {
                int tested = queue.PopFront();

                for (int other = 0; other < scanners.Count; other++)
                {
                    if (fixedSet.Contains(other))
                    {
                        continue;
                    }

                    ScannerOrientation? result = scanners[tested].Overlaps(scanners[other], threshold);

                    if (!result.HasValue)
                    {
                        continue;
                    }

                    scanners[other].Normalize(result.Value);

                    queue.PushBack(other);
                    fixedSet.Add(other);
                }
            }
        }
    }

    public class Point3D : IEquatable<Point3D>
    {
        private int[] values;

        public Point3D(int x, int y, int z)
        {
            this.values = new int[] { x, y, z };
        }

        public int X { get { return this.values[0]; } }

        public int Y { get { return this.values[1]; } }
        public int Z { get { return this.values[2]; } }

        public bool Equals([AllowNull] Point3D other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.X == other.X && this.Y == other.Y && this.Z == other.Z);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Point3D);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public int Manhattan()
        {
            return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);
        }

        public Point3D Rotate(Rotation rotation)
        {
            return new Point3D(
                this.values[rotation.Axis1] * rotation.Multiplier1,
                this.values[rotation.Axis2] * rotation.Multiplier2,
                this.values[rotation.Axis3] * rotation.Multiplier3);
        }

        public override string ToString()
        {
            return $"{this.X}, {this.Y}, {this.Z}";
        }

        public static Point3D operator +(Point3D first, Point3D second)
        {
            return new Point3D(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        public static Point3D operator -(Point3D first, Point3D second)
        {
            return new Point3D(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }
    }

    public class Rotation
    {
        public Rotation(int axis1, int multiplier1, int axis2, int multiplier2, int axis3, int multiplier3)
        {
            this.Axis1 = axis1;
            this.Multiplier1 = multiplier1;

            this.Axis2 = axis2;
            this.Multiplier2 = multiplier2;

            this.Axis3 = axis3;
            this.Multiplier3 = multiplier3;
        }

        public int Axis1 { get; }

        public int Multiplier1 { get; }

        public int Axis2 { get; }

        public int Multiplier2 { get; }

        public int Axis3 { get; }

        public int Multiplier3 { get; }
    }

    public struct ScannerOrientation
    {
        public ScannerOrientation(Point3D point, Rotation rotation)
        {
            this.Position = point;
            this.Rotation = rotation;
        }

        public Point3D Position { get; }

        public Rotation Rotation { get; }
    }

    public class Scanner
    {
        private const int X = 0;
        private const int Y = 1;
        private const int Z = 2;
        private const int POS = 1;
        private const int NEG = -1;

        private static List<Rotation> Rotations;

        static Scanner()
        {
            Rotations = new List<Rotation>(
                    new[]
                    {
                    new Rotation(X, POS, Y, POS, Z, POS),
                    new Rotation(X, POS, Z, POS, Y, NEG),
                    new Rotation(X, POS, Y, NEG, Z, NEG),
                    new Rotation(X, POS, Z, NEG, Y, POS),
                    new Rotation(X, NEG, Y, POS, Z, NEG),
                    new Rotation(X, NEG, Z, NEG, Y, NEG),
                    new Rotation(X, NEG, Y, NEG, Z, POS),
                    new Rotation(X, NEG, Z, POS, Y, POS),
                    new Rotation(Y, POS, X, POS, Z, NEG),
                    new Rotation(Y, POS, Z, NEG, X, NEG),
                    new Rotation(Y, POS, X, NEG, Z, POS),
                    new Rotation(Y, POS, Z, POS, X, POS),
                    new Rotation(Y, NEG, X, POS, Z, POS),
                    new Rotation(Y, NEG, Z, POS, X, NEG),
                    new Rotation(Y, NEG, X, NEG, Z, NEG),
                    new Rotation(Y, NEG, Z, NEG, X, POS),
                    new Rotation(Z, POS, X, POS, Y, POS),
                    new Rotation(Z, POS, Y, POS, X, NEG),
                    new Rotation(Z, POS, X, NEG, Y, NEG),
                    new Rotation(Z, POS, Y, NEG, X, POS),
                    new Rotation(Z, NEG, X, POS, Y, NEG),
                    new Rotation(Z, NEG, Y, NEG, X, NEG),
                    new Rotation(Z, NEG, X, NEG, Y, POS),
                    new Rotation(Z, NEG, Y, POS, X, POS)
                    });
        }

        public Scanner(HashSet<Point3D> beacons)
        {
            this.Position = new Point3D(0, 0, 0);
            this.Beacons = beacons;
        }

        public Point3D Position { get; set; }

        public HashSet<Point3D> Beacons { get; private set; }

        public ScannerOrientation? Overlaps(Scanner other, int threshold)
        {
            foreach (Rotation rotation in Rotations)
            {
                var cnts = new Dictionary<Point3D, int>();

                foreach (Point3D lhs in this.Beacons)
                {
                    foreach (Point3D rhs in other.Beacons)
                    {
                        Point3D key = lhs - rhs.Rotate(rotation);
                        if (!cnts.ContainsKey(key))
                        {
                            cnts[key] = 0;
                        }

                        cnts[key]++;
                    }
                }

                foreach (KeyValuePair<Point3D, int> kvp in cnts)
                {
                    if (kvp.Value >= threshold)
                    {
                        return new ScannerOrientation(kvp.Key, rotation);
                    }
                }
            }

            return null;
        }

        public void Normalize(ScannerOrientation orientation)
        {
            HashSet<Point3D> normalized = this.Beacons.
                Select(b => b.Rotate(orientation.Rotation) + orientation.Position).ToHashSet();

            this.Beacons = normalized;
            this.Position = orientation.Position;
        }
    }
}
