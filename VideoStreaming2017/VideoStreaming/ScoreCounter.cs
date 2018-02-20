using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStreaming.Entities;
namespace VideoStreaming
{
    public class ScoreCounter
    {
        public static double CountScore(int totalTimeSavedForAllRequests, int totalVideoRequestCount)
        {
            var score = ((double)totalTimeSavedForAllRequests * 1000) / (double)totalVideoRequestCount;
            return score;
        }
    }
}
