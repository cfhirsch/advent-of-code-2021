using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec19
    {
        static Dec19()
        {
            
        }
    }

    public class Point3D : IEquatable<Point3D>
    {
        private const int XAxis = 0;
        private const int YAxis = 1;
        private const int ZAxis = 2;
        private const int POS = 1;
        private const int NEG = -1;

        private int[] values;

        private static int[] test = { 1, 2, 3 };

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

        public static Point3D operator + (Point3D first, Point3D second)
        {
            return new Point3D(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        public static Point3D operator - (Point3D first, Point3D second)
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

        public Point3D Position { get; private set; }

        public HashSet<Point3D> Beacons { get; private set; }

        public ScannerOrientation? Overlaps(Scanner other, UInt64 threshold)
        {
            foreach (Rotation rotation in Rotations)
            {
                var cnts = new Dictionary<Point3D, UInt64>();

                foreach (Point3D lhs in this.Beacons)
                {
                    foreach (Point3D rhs in other.Beacons)
                    {
                        cnts[lhs - rhs.Rotate(rotation)]++;
                    }
                }

                foreach (KeyValuePair<Point3D, UInt64> kvp in cnts)
                {
                    if (kvp.Value >= threshold)
                    {
                        return new ScannerOrientation(kvp.Key, rotation);
                    }
                }
            }

            return null;
        }

        public void Normalize(ScannerOrientation orientation) {
            HashSet<Point3D> normalized = this.Beacons.
                Select(b => b.Rotate(orientation.Rotation) + orientation.Position).ToHashSet();

            this.Beacons = normalized;
            this.Position = orientation.Position;
        }
    }
}
