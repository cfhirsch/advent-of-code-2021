using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2021
{
    public static class PuzzleInputReader
    {
        public static IEnumerable<string> GetPuzzleLines(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return reader.ReadLine();
                    }
                }
            }
        }
    }
}
