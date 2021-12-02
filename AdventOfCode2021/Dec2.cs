using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021
{
    public class Dec2
    {
        public static void Solve_Part_One()
        {
            int x = 0;
            int y = 0;
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec2.txt"))
            {
                var command = new Command(line);
                switch (command.Direction)
                {
                    case Direction.Down:
                        y += command.Distance;
                        break;

                    case Direction.Forward:
                        x += command.Distance;
                        break;

                    case Direction.Up:
                        y -= command.Distance;
                        break;

                    default:
                        throw new ArgumentException($"Unknown direction {command.Direction}.");
                }

                Console.WriteLine("x = {0}, y = {1}, x * y = {2}", x, y, x * y);
            }
        }
    }

    public struct Command
    {
        public Command(string line)
        {
            string[] lineParts = line.Split(' ');
            this.Direction = (Direction)Enum.Parse(typeof(Direction), lineParts[0], ignoreCase: true);
            this.Distance = Int32.Parse(lineParts[1]);
        }

        public Direction Direction { get; }
        public int Distance { get; }
    }

    public enum Direction
    {
        Forward,
        Down,
        Up
    }
}
