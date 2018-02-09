using System;
using System.IO;

namespace VideoStreaming2017
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input\\videos_worth_spreading.in");

            var chaches = Parser.Parse(input);
        }
    }
}
