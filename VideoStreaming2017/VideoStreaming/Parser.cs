using System.Collections.Generic;
using System.Linq;
using VideoStreaming.Entities;

namespace VideoStreaming
{
    public class Parser
    {
        const int EndpointCacheCountPositionInLine = 1;
        const int EndpointDatacenterLatencyPositionInLine = 0;

        public static List<Cache> Parse(string input, out int cacheSize)
        {
            var lines = input.Split('\n');

            var dataDescription = lines[0].Split(' ');

            int cacheCount = int.Parse(dataDescription[3]);
            cacheSize = int.Parse(dataDescription[4]);
            int endpointCount = int.Parse(dataDescription[1]);
            int requestCount = int.Parse(dataDescription[2]);

            var caches = new List<Cache>();
            for (int i = 0; i < cacheCount; i++)
            {
                caches.Add(new Cache
                {
                    Id = i,
                    Size = cacheSize
                });
            }

            var videosDescription = lines[1].Split(' ');
            var videos = new List<Video>();

            for (int i = 0; i < videosDescription.Length; i++)
            {
                videos.Add(new Video()
                {
                    Id = i,
                    Size = int.Parse(videosDescription[i])
                });
            }

            var endpointList = new List<Endpoint>();

            var endpointCountLine = 2;
            for (int i = 0; i < endpointCount; i++)
            {
                var endpointDescription = lines[endpointCountLine].Split(' ');

                var endpointCacheCount = int.Parse(endpointDescription[EndpointCacheCountPositionInLine]);
                var endpointDatacenterLatency = int.Parse(endpointDescription[EndpointDatacenterLatencyPositionInLine]);
                endpointList.Add(new Endpoint
                {
                    Id = i,
                    DatacenterLatency = endpointDatacenterLatency,
                });

                for (int j = 0; j < endpointCacheCount; j++)
                {
                    var latencyFromEndpointToCacheLine = lines[endpointCountLine + j + 1].Split(' ');
                    int cacheId = int.Parse(latencyFromEndpointToCacheLine[0]);
                    int cacheLatency = int.Parse(latencyFromEndpointToCacheLine[1]);

                    endpointList[i].CacheLatencies.Add(new CacheLatency
                    {
                        Cache = caches.Single(c => c.Id == cacheId),
                        Latency = cacheLatency
                    });

                    var cache = caches.Single(c => c.Id == cacheId);
                    cache.Endpoints.Add(endpointList[i]);

                    //endpointList[i]
                }

                endpointCountLine += ++endpointCacheCount; // jump to the next endpoint
            }

            //endpointCountLine++;

            for (int i = 0; i < requestCount; i++)
            {
                var request = lines[endpointCountLine + i].Split(' ');
                var videoId = int.Parse(request[0]);
                var endpointId = int.Parse(request[1]);
                var requestsFromEndpointToVideoCount = int.Parse(request[2]);

                var currentEndpoint = endpointList.Single(e => e.Id == endpointId);

                // more than one line in request description with same id for endpoint and video fix
                var currentVideoRequestCount = currentEndpoint.VideoRequestCountList.SingleOrDefault(vrc => vrc.Video.Id == videoId);

                if (currentVideoRequestCount == null)
                {
                    currentEndpoint.VideoRequestCountList.Add(new VideoRequestCount()
                    {
                        Video = videos.Single(v => v.Id == videoId),
                        RequestCount = requestsFromEndpointToVideoCount
                    });
                }
                else
                {
                    currentEndpoint.VideoRequestCountList.Single(vrc => vrc.Video.Id == currentVideoRequestCount.Video.Id).RequestCount += requestsFromEndpointToVideoCount;
                }
                
            }

            return caches;
        }
    }
}
