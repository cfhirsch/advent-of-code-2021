using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec4
    {
        public static void Solve_Part_One(bool show = false)
        {
            // Read in numbers.
            var lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec4.txt");

            IEnumerable<int> numbers = lines.First().Split(",").Select(s => Int32.Parse(s));

            // Load bingo boards.
            var boards = new List<BingoBoard>();
            int row = 0;
            var squares = new BoardSquare[5, 5];
            foreach (string line in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] entries = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < 5; col++)
                {
                    squares[row, col] = new BoardSquare(Int32.Parse(entries[col]));
                }

                row++;
                if (row >= 5)
                {
                    row = 0;
                    boards.Add(new BingoBoard(squares));
                    squares = new BoardSquare[5, 5];
                }
            }

            // Now play the game.
            int lastNumber = Int32.MinValue;
            BingoBoard winningBoard = null;
            bool winner = false;
            foreach (int number in numbers)
            {
                Console.WriteLine("Calling {0}.", number);
                foreach (BingoBoard board in boards)
                {
                    board.NextNumber(number);
                    if (board.IsWinner())
                    {
                        lastNumber = number;
                        winningBoard = board;
                        winner = true;
                        break;
                    }
                }

                if (show)
                {
                    foreach (BingoBoard board in boards)
                    {
                        Console.WriteLine(board);
                        Console.WriteLine();
                    }
                }

                if (winner)
                {
                    break;
                }
            }

            // Calculate score.
            int unmarkedSum = winningBoard.GetUnmarkedSum();
            Console.WriteLine("sum = {0}, lastNum = {1}, score = {2}", lastNumber, unmarkedSum, lastNumber * unmarkedSum);
        }

        public static void Solve_Part_Two()
        {
            // Read in numbers.
            var lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec4.txt");

            IEnumerable<int> numbers = lines.First().Split(",").Select(s => Int32.Parse(s));

            // Load bingo boards.
            var boards = new List<BingoBoard>();
            int row = 0;
            var squares = new BoardSquare[5, 5];
            foreach (string line in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] entries = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < 5; col++)
                {
                    squares[row, col] = new BoardSquare(Int32.Parse(entries[col]));
                }

                row++;
                if (row >= 5)
                {
                    row = 0;
                    boards.Add(new BingoBoard(squares));
                    squares = new BoardSquare[5, 5];
                }
            }

            // Now play the game.
            int lastNumber = Int32.MinValue;
            int numWinningBoards = 0;
            BingoBoard lastWinningBoard = null;
            foreach (int number in numbers)
            {
                Console.WriteLine("Calling {0}.", number);
                foreach (BingoBoard board in boards)
                {
                    board.NextNumber(number);
                    if (!board.Winner && board.IsWinner())
                    {
                        lastNumber = number;
                        numWinningBoards++;
                        if (numWinningBoards == boards.Count())
                        {
                            lastWinningBoard = board;
                            break;
                        }
                    }
                }

                if (lastWinningBoard != null)
                {
                    break;
                }
            }

            // Calculate score.
            int unmarkedSum = lastWinningBoard.GetUnmarkedSum();
            Console.WriteLine("sum = {0}, lastNum = {1}, score = {2}", lastNumber, unmarkedSum, lastNumber * unmarkedSum);
        }
    }

    public class BingoBoard
    {
        public BingoBoard(BoardSquare[,] board)
        {
            this.Board = board;
        }

        public BoardSquare[,] Board { get; }

        public bool Winner { get; private set; }

        public int GetUnmarkedSum()
        {
            int sum = 0;
            for (int i = 0; i < this.Board.GetLength(0); i++)
            {
                for (int j = 0; j < this.Board.GetLength(1); j++)
                {
                    if (!this.Board[i, j].Marked)
                    {
                        sum += this.Board[i, j].Value;
                    }
                }
            }

            return sum;
        }

        public void NextNumber(int value)
        {
            for (int i = 0; i < this.Board.GetLength(0); i++)
            {
                for (int j = 0; j < this.Board.GetLength(1); j++)
                {
                    if (this.Board[i, j].Value == value)
                    {
                        this.Board[i, j].Marked = true;
                    }
                }
            }
        }

        public bool IsWinner()
        {
            // Check rows.
            for (int i = 0; i < this.Board.GetLength(0); i++)
            {
                bool win = true;
                for (int j = 0; j < this.Board.GetLength(1); j++)
                {
                    if (!this.Board[i, j].Marked)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    this.Winner = true;
                    return true;
                }
            }

            // Check columns
            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                bool win = true;
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    if (!this.Board[i, j].Marked)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    this.Winner = true;
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.Board.GetLength(0); i++)
            {
                for (int j = 0; j < this.Board.GetLength(1); j++)
                {
                    if (this.Board[i, j].Marked)
                    {
                        sb.AppendFormat("\x1b[1m{0}\x1b[0m ", this.Board[i, j].Value);
                    }
                    else
                    {
                        sb.AppendFormat("{0} ", this.Board[i, j].Value);
                    }
                }

                sb.Append("\r\n");
            }

            return sb.ToString();
        }
    }

    public class BoardSquare
    {
        public BoardSquare(int value)
        {
            this.Value = value;
        }

        public int Value { get; }

        public bool Marked { get; set; }
    }
}
