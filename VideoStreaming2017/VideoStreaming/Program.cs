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

            var caches = Parser.Parse(input, out var cacheSize, out var totalVideoRequestCount);

            var solver = new Solver(caches);
            solver.AddVideoPretenders();
            solver.AddOptimalVideosToCorrespondingCaches();

            var usedCachesCount = 0;

            var usedCachesIndexes = new List<int>();

            for (int i = 0; i < caches.Count; i++)
            {
                if (caches[i].Size == cacheSize) continue;

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

                    foreach (var video in caches[usedCachesIndex].SavedVideos)
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
            Console.WriteLine("Time passed " + elapsedTime);

            var score = ScoreCounter.CountScore(solver.TotalTimeSavedForAllRequests, totalVideoRequestCount);

            Console.WriteLine("Score is: " + score);

            Console.ReadKey();
        }
    }
}
