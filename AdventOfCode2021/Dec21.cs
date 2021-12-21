using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Dec21
    {
        public static void Solve()
        {
            IEnumerable<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec21.txt");
            int player1Pos = Int32.Parse(lines.First().Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
            int player2Pos = Int32.Parse(lines.Last().Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
            int numDieRolls = 0;
            int dieResult = 1;

            int player1Score = 0;
            int player2Score = 0;

            while (true)
            {
                int dieScore = RollDice(ref dieResult, ref numDieRolls);
                player1Pos = player1Pos + dieScore;
                while (player1Pos > 10)
                {
                    player1Pos -= 10;
                }

                player1Score += player1Pos;

                Console.WriteLine(
                    "Player 1 rolls {0}+{1}+{2} and moves to space {3} for a total score of {4}.",
                    dieResult - 2,
                    dieResult - 1,
                    dieResult,
                    player1Pos,
                    player1Score);

                if (player1Score >= 1000)
                {
                    break;
                }

                dieScore = RollDice(ref dieResult, ref numDieRolls);
                player2Pos = player2Pos + dieScore;
                while (player2Pos > 10)
                {
                    player2Pos -= 10;
                }

                player2Score += player2Pos;

                Console.WriteLine(
                    "Player 2 rolls {0}+{1}+{2} and moves to space {3} for a total score of {4}.",
                    dieResult - 2,
                    dieResult - 1,
                    dieResult,
                    player2Pos,
                    player2Score);

                if (player2Score >= 1000)
                {
                    break;
                }
            }

            int losingScore = Math.Min(player1Score, player2Score);

            Console.WriteLine(
                "Losing score = {0}, num die rolls = {1}, product = {2}",
                losingScore,
                numDieRolls,
                losingScore * numDieRolls);
        }

        private static int RollDice(ref int dieResult, ref int numDieRolls)
        {
            int sum = 0;
            for (int i = 0; i < 3; i++)
            {
                sum += dieResult;
                dieResult++;
                numDieRolls++;
            }

            return sum;
        }
    }
}
