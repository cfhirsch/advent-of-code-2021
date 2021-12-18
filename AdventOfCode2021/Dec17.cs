using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2021
{
    public static class Dec17
    {
        public static void Solve()
        {
            // target area: x=20..30, y=-10..-5
            string line = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec17.txt").First();

            var regex = new Regex(@"x=(\d+)\.\.(\d+), y=-(\d+)..-(\d+)");
            Match match = regex.Match(line);

            if (!match.Success)
            {
                throw new Exception("Could not match regex!");
            }

            int minX = Int32.Parse(match.Groups[1].Value);
            int maxX = Int32.Parse(match.Groups[2].Value);
            int minY = -1 * Int32.Parse(match.Groups[3].Value);
            int maxY = -1 * Int32.Parse(match.Groups[4].Value);

            int highest_init_x = 0, highest_init_y = 0;

            int highestY = 0;
            
            // Calculate minimal x velocity that ensures we will reach left hand side
            // of boundary square before x velocity reaches zero.
            int minInitV_x = (int)Math.Ceiling((-1 + Math.Sqrt(1 + 8 * minX)) / 2);

            for (int init_vel_x = minInitV_x ; init_vel_x <= maxX; init_vel_x++)
            {
                int currentMaxHeight = 0;
                int currentMaxY = 0;

                for (int init_vel_y = 0; init_vel_y < Math.Abs(minY); init_vel_y++)
                { 
                    var init_v = new Point(init_vel_x, init_vel_y);
                    Tuple<SimulationResult, int> result = Simulate(init_v, minX, minY, maxX, maxY);
                    
                    SimulationResult simulationResult = result.Item1;

                    //Console.SetCursorPosition(0, 0);
                    //Console.WriteLine("init v_x = {0}, int v_y = {1}, result = {2}", init_vel_x, init_vel_y, simulationResult);

                    if (simulationResult == SimulationResult.Bullseye)
                    {
                        if (result.Item2 > currentMaxHeight)
                        {
                            currentMaxHeight = result.Item2;
                            currentMaxY = init_vel_y;
                        }
                    }
                }

                if (currentMaxHeight > highestY)
                {
                    highestY = currentMaxHeight;
                    highest_init_x = init_vel_x;
                    highest_init_y = currentMaxY;
                }
            }
            
            Console.WriteLine("Highest Y = {0}, found at ({1}, {2}).", highestY, highest_init_x, highest_init_y);
        }

        private static Tuple<SimulationResult, int> Simulate(Point initV, int minX, int minY, int maxX, int maxY)
        {
            // The probe's x,y position starts at 0,0.
            var current = new Point(0, 0);
            Point currentV = initV;

            int highestY = current.Y;

            // Then, it will follow some trajectory by moving in steps. 
            // On each step, these changes occur in the following order:
            while (true)
            {
                // The probe's x position increases by its x velocity.
                int newX = current.X + currentV.X;

                // The probe's y position increases by its y velocity.
                int newY = current.Y + currentV.Y;

                // Due to drag, the probe's x velocity changes by 1 toward the value 0;
                // that is, it decreases by 1 if it is greater than 0, increases by 1 if it is less than 0,
                // or does not change if it is already 0.
                int newVelX;
                if (currentV.X == 0)
                {
                    newVelX = 0;
                }
                else
                {
                    newVelX = (currentV.X > 0) ? currentV.X - 1 : currentV.X + 1;
                }

                // Due to gravity, the probe's y velocity decreases by 1.
                int newVelY = currentV.Y - 1;

                current = new Point(newX, newY);
                currentV = new Point(newVelX, newVelY);

                //Console.WriteLine("Probe at {0}, {1}", current.X, current.Y);
                //Console.WriteLine("Velocity = {0}, {1}", currentV.X, currentV.Y);

                //Thread.Sleep(100);

                if (current.Y  > highestY)
                {
                    highestY = current.Y;
                }

                // If we are in the bounding box, we have succeeded.
                if (current.X >= minX && current.X <= maxX && current.Y >= minY && current.Y <= maxY)
                {
                    return new Tuple<SimulationResult, int>(SimulationResult.Bullseye, highestY);
                }

                // If we are past maxX, then we overshot.
                if (current.X > maxX)
                {
                    return new Tuple<SimulationResult, int>(SimulationResult.Overshot, highestY);
                }

                // If we have fallen below minY, we have either undershot or passed through.
                if (current.Y < minY)
                {
                    if (current.X < minX)
                    {
                        return new Tuple<SimulationResult, int>(SimulationResult.Undershot, highestY);
                    }

                    return new Tuple<SimulationResult, int>(SimulationResult.PassedThrough, highestY);
                }
            }
        }
    }

    public enum SimulationResult
    {
        None,
        Overshot,
        Undershot,
        PassedThrough,
        Bullseye
    }
}
