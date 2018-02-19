using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VideoStreaming
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllText("Input\\kittens.in.txt");

            var chaches = Parser.Parse(input, out var cacheSize);

            var solver = new Solver(chaches);
            solver.AddVideoPretenders();
            solver.AddOptimalVideosToCorrespondingCaches();

            var usedCachesCount = 0;

            var usedCachesIndexes = new List<int>();

            for (int i = 0; i < chaches.Count; i++)
            {
                if (chaches[i].Size == cacheSize) continue;

                usedCachesIndexes.Add(i);

                usedCachesCount++;
            }

            using (StreamWriter sw = File.CreateText("kittens.out.txt"))
            {
                sw.WriteLine(usedCachesCount);

                var result = ""; 
                foreach (var usedCachesIndex in usedCachesIndexes)
                {
                    result += usedCachesIndex + " ";

                    foreach (var video in chaches[usedCachesIndex].SavedVideos)
                    {
                        result += video.Id + " ";
                    }

                    sw.WriteLine(result);
                    result = "";
                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            Console.WriteLine("Tite passed " + elapsedTime);

            Console.ReadKey();
        }
    }
}
