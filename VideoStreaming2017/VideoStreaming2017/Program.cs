using System;
using System.IO;

namespace VideoStreaming2017
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input\\simple_input.txt");

            var chaches = Parser.Parse(input);

            Solver.AddVideoPretenders(chaches);
        }
    }
}
