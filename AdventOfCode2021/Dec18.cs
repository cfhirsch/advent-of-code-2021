using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec18
    {
        public static void Solve(bool show = false)
        {
            SnailFishNumber sum = null;
            foreach (string line in PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec18.txt"))
            {
                int pos = 0;
                var num = new SnailFishNumber(line, ref pos);

                if (sum == null)
                {
                    sum = num;
                }
                else
                {
                    sum += num;
                    Console.WriteLine("Sum = {0}", sum);
                }
            }

            Console.WriteLine("Result = {0}.", sum.Evaluate());
        }
    }

    public class SnailFishNumber
    {
        public SnailFishNumber(string input, ref int pos)
        {
            // Assume that we are staring on a '[' character.
            pos++;
            Char ch = input[pos];
            if (Char.IsNumber(ch))
            {
                this.LeftValue = Int32.Parse(ch.ToString());
            }
            else
            {
                this.Left = new SnailFishNumber(input, ref pos);
                this.Left.Parent = this;
            }

            // Seek past the comma.
            pos++;
            if (input[pos] != ',')
            {
                throw new Exception($"Expected comma, got {input[pos]} instead.");
            }

            pos++;
            ch = input[pos];
            if (Char.IsNumber(ch))
            {
                this.RightValue = Int32.Parse(ch.ToString());
            }
            else
            {
                this.Right = new SnailFishNumber(input, ref pos);
                this.Right.Parent = this;
            }

            // Seek past the ']' character.
            pos++;
            if (input[pos] != ']')
            {
                throw new Exception($"Expected comma, got {input[pos]} instead.");
            }
        }

        public SnailFishNumber(
            SnailFishNumber left = null, 
            SnailFishNumber right = null,
            int leftVal = 0,
            int rightVal = 1,
            SnailFishNumber parent = null)
        {
            this.Left = left;
            if (this.Left != null)
            {
                this.Left.Parent = this;
            }

            this.Right = right;
            if (this.Right != null)
            {
                this.Right.Parent = this;
            }

            this.LeftValue = leftVal;
            this.RightValue = rightVal;
            this.Parent = parent;
        }

        public SnailFishNumber Parent { get; set; }

        public SnailFishNumber Left { get; set; }

        public SnailFishNumber Right { get; set; }

        public int LeftValue { get; set; }

        public int RightValue { get; set; }

        public long Evaluate()
        {
            //The magnitude of a pair is 3 times the magnitude of its left element plus 2 times the magnitude
            // of its right element. The magnitude of a regular number is just that number.
            long result = 0;
            if (this.Left == null)
            {
                result += (3 * this.LeftValue);
            }
            else
            {
                result += 3 * this.Left.Evaluate();
            }

            if (this.Right == null)
            {
                result += (2 * this.RightValue);
            }
            else
            {
                result += 2 * this.Right.Evaluate();
            }

            return result;
        }

        public static SnailFishNumber operator+ (SnailFishNumber x, SnailFishNumber y)
        {
            // To add two snailfish numbers, form a pair from the left and right parameters of the addition operator.
            // For example, [1, 2] + [[3, 4],5] becomes[[1, 2],[[3,4],5]].
            var z = new SnailFishNumber(x, y);

            // There's only one problem: snailfish numbers must always be reduced.

            bool exploded;
            bool split;
            do
            {
                split = false;

                // If any pair is nested inside four pairs, the leftmost such pair explodes.
                exploded = Explode(z);

                if (exploded)
                {
                    //Console.WriteLine("Exploded: {0}", z);
                }

                if (!exploded)
                {
                    // If any regular number is 10 or greater, the leftmost such regular number splits.
                    split = Split(z);
                    if (split)
                    {
                        //Console.WriteLine("Split: {0}", z);
                    }
                }
            } while (exploded || split);

            return z;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            if (this.Left != null)
            {
                sb.Append(this.Left);
            }
            else
            {
                sb.Append(this.LeftValue);
            }

            sb.Append(",");

            if (this.Right != null)
            {
                sb.Append(this.Right);
            }
            else
            {
                sb.Append(this.RightValue);
            }

            sb.Append("]");

            return sb.ToString();
        }

        private static bool Explode(SnailFishNumber num)
        {
            return ExplodeHelper(num, 0);
        }
        
        private static bool ExplodeHelper(SnailFishNumber num, int level)
        {
            if (level == 4)
            {
                // Explode the left most pair.
                // To explode a pair, the pair's left value is added to the first regular number
                // to the left of the exploding pair (if any),
                // and the pair's right value is added to the first regular number to the right of the exploding pair
                // (if any).
                // Exploding pairs will always consist of two regular numbers.
                // Then, the entire exploding pair is replaced with the regular number 0.
                AddToFirstRegularNumberOnLeft(num);
                AddToFirstRegularNumberOnRight(num);

                SnailFishNumber parent = num.Parent;
                if (num == parent.Left)
                {
                    parent.Left = null;
                    parent.LeftValue = 0;
                }
                else
                {
                    parent.Right = null;
                    parent.RightValue = 0;
                }

                return true;
            }

            bool exploded;
            if (num.Left != null)
            {
                exploded = ExplodeHelper(num.Left, level + 1);

                if (exploded)
                {
                    return true;
                }
            }

            if (num.Right != null)
            {
                exploded = ExplodeHelper(num.Right, level + 1);
                if (exploded)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool Split(SnailFishNumber num)
        {
            // To split a regular number, replace it with a pair; the left element of the pair should be the
            // regular number divided by two and rounded down, while the right element of the pair should be
            // the regular number divided by two and rounded up.
            // For example, 10 becomes [5,5], 11 becomes [5,6], 12 becomes [6,6], and so on.
            if (num.Left == null && num.LeftValue >= 10)
            {
                int newLeftVal = (int)Math.Floor(num.LeftValue/2.0);
                int newRightVal = (int)Math.Ceiling(num.LeftValue / 2.0);
                num.LeftValue = 0;
                num.Left = new SnailFishNumber(leftVal: newLeftVal, rightVal: newRightVal);
                num.Left.Parent = num;
                return true;
            }

            if (num.Left != null && Split(num.Left))
            {
                return true;
            }

            if (num.Right == null && num.RightValue >= 10)
            {
                int newLeftVal = (int)Math.Floor(num.RightValue / 2.0);
                int newRightVal = (int)Math.Ceiling(num.RightValue / 2.0);
                num.RightValue = 0;
                num.Right = new SnailFishNumber(leftVal: newLeftVal, rightVal: newRightVal);
                num.Right.Parent = num;
                return true;
            }

            if (num.Right != null && Split(num.Right))
            {
                return true;
            }

            return false;
        }

        private static void AddToFirstRegularNumberOnLeft(SnailFishNumber num)
        {
            SnailFishNumber current = num;
            SnailFishNumber parent;
            while (current != null && current.Parent != null)
            {
                parent = current.Parent;
                if (current == parent.Left)
                {
                    current = current.Parent;
                }
                else
                {
                    // Current node is right child of its parent. 
                    // Look for the right most child of the parent's left subtree.
                    current = parent;
                    if (current.Left == null)
                    {
                        current.LeftValue += num.LeftValue;
                    }
                    else
                    {
                        current = current.Left;
                        while (current.Right != null)
                        {
                            current = current.Right;
                        }

                        current.RightValue += num.LeftValue;
                    }

                    return;
                }
            }
        }

        private static void AddToFirstRegularNumberOnRight(SnailFishNumber num)
        {
            SnailFishNumber current = num;
            SnailFishNumber parent;
            while (current != null && current.Parent != null)
            {
                parent = current.Parent;
                if (current == parent.Right)
                {
                    current = parent;
                }
                else
                {
                    // Current node is left child of its parent. 
                    // Look for the left most child of the parent's right subtree.
                    current = parent;
                    if (current.Right == null)
                    {
                        current.RightValue += num.RightValue;
                    }
                    else
                    {
                        current = current.Right;
                        while (current.Left != null)
                        {
                            current = current.Left;
                        }

                        current.LeftValue += num.RightValue;
                    }

                    return;
                }
            }
        }
    }
}
