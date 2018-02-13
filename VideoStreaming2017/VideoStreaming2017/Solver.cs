using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStreaming2017.Entities;

namespace VideoStreaming2017
{
    public class Solver
    {
        public static void AddVideoPretenders(List<Cache> caches)
        {
            foreach (var cache in caches)
            {
                foreach (var endpoint in cache.Endpoints)
                {
                    foreach (var videoRequestCount in endpoint.VideoRequestCountList)
                    {
                        var enpointToCacheLatency = endpoint.CacheLatencies.Single(c => c.Cache.Id == cache.Id).Latency;
                        var timeSaved = videoRequestCount.RequestCount * (endpoint.DatacenterLatency - enpointToCacheLatency);
                        var requestCount = videoRequestCount.RequestCount;

                        var pretender = cache.VideoPretenders.SingleOrDefault(vp => vp.Video.Id == videoRequestCount.Video.Id);
                        if (pretender != null)
                        {
                            pretender.TotalTimeSaved += timeSaved;
                            pretender.TotalRequestCount += requestCount;
                        }
                        else
                        {
                            cache.VideoPretenders.Add(new VideoPretender()
                            {
                                Video = videoRequestCount.Video,
                                TotalTimeSaved = timeSaved,
                                TotalRequestCount = requestCount
                            });
                        }
                    }
                }
            }
        }
    }
}
