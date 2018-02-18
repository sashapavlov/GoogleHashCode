using System;
using System.IO;

namespace VideoStreaming2017
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input\\kittens.in.txt");

            var chaches = Parser.Parse(input);

            var solver = new Solver(chaches);
            solver.AddVideoPretenders();
            solver.AddOptimalVideosToCorrespondingCaches();
        }
    }
}
