using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void Solve_PartTwo()
        {
            IEnumerable<string> lines = PuzzleInputReader.GetPuzzleLines(@"c:\docs\adventofcode2021\dec21.txt");
            int player1Pos = Int32.Parse(lines.First().Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
            int player2Pos = Int32.Parse(lines.Last().Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);

            var table = new Dictionary<Tuple<int, int, int, int, bool>, long>();
            long numPlayer1WinsUniverses = NumWinningUniversesForPlayer(
                player1Pos,
                0,
                player2Pos,
                0,
                true,
                table);

            long numPlayer2WinsUniverses = NumWinningUniversesForPlayerTwo(
                player2Pos,
                0,
                player1Pos,
                0,
                table);

            Console.WriteLine("Player 1 wins in {0} universes.", numPlayer1WinsUniverses);
            Console.WriteLine("Player 2 wins in {0} universes.", numPlayer2WinsUniverses);

            Console.WriteLine("Max = {0}", Math.Max(numPlayer1WinsUniverses, numPlayer2WinsUniverses));
        }

        private static long NumWinningUniversesForPlayerTwo(
            int position,
            int score,
            int otherPos,
            int otherScore,
            Dictionary<Tuple<int, int, int, int, bool>, long> table)
        {
            long sum = 0;
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        int newPos = otherPos + i + j + k;
                        if (newPos > 10)
                        {
                            newPos -= 10;
                        }

                        int newScore = score + newPos;

                        sum += NumWinningUniversesForPlayer(
                            position,
                            score,
                            newPos,
                            newScore,
                            true,
                            table);   
                    }
                }
            }
            
            return sum;
        }

        // Calculates the number of universes where a player wins, 
        // from the given position and score, and whether it's the
        // current player's move.
        private static long NumWinningUniversesForPlayer(
            int position,
            int score,
            int otherPos,
            int otherScore,
            bool currentPlayersMove,
            Dictionary<Tuple<int, int, int, int, bool>, long> table)
        {
            var key = new Tuple<int, int, int, int, bool>(
                position, 
                score, 
                otherPos,
                otherScore,
                currentPlayersMove);

            if (table.ContainsKey(key))
            {
                return table[key];
            }

            long sum = 0;
            
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        if (currentPlayersMove)
                        { 
                            int newPos = position + i + j + k;
                            if (newPos > 10)
                            {
                                newPos -= 10;
                            }

                            int newScore = score + newPos;
                            if (newScore >= 21)
                            {
                                sum++;
                            }
                            else
                            {
                                sum += NumWinningUniversesForPlayer(
                                    newPos,
                                    newScore,
                                    otherPos,
                                    otherScore,
                                    false,
                                    table);
                            }
                        }
                        else
                        {
                            int newPos = otherPos + i + j + k;
                            if (newPos > 10)
                            {
                                newPos -= 10;
                            }

                            int newScore = otherScore + newPos;
                            if (newScore < 21)
                            {
                                sum += NumWinningUniversesForPlayer(
                                    position,
                                    score,
                                    newPos,
                                    newScore,
                                    true,
                                    table);
                            }
                        }
                    }
                }
            }

            table[key] = sum;
            return sum;
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
