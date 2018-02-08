﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStreaming2017.Entities;

namespace VideoStreaming2017
{
    public class Parser
    {
        const int EndpointCacheCountPositionInLine = 1;
        const int EndpointDatacenterLatencyPositionInLine = 0;

        public static List<Cache> Parse(string input)
        {
            var lines = input.Split('\n');

            var dataDescription = lines[0].Split(' ');

            int cacheCount = int.Parse(dataDescription[3]);
            int cacheSize = int.Parse(dataDescription[4]);
            int endpointCount = int.Parse(dataDescription[1]);

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
                    var latencyFromEndpointToCacheLine = lines[endpointCountLine + j + 1];
                    var cacheId = latencyFromEndpointToCacheLine[0];
                    var cacheLatency = latencyFromEndpointToCacheLine[1];

                    endpointList[i].CacheLatencies.Add(new CacheLatency
                    {
                        Cache = caches.Single(c => c.Id == cacheId),
                        Latency = cacheLatency
                    });
                    //endpointList[i]
                }

                endpointCountLine += endpointCacheCount; // jump to the next endpoint
            }

            // TODO: Parse requests

            throw new NotImplementedException();
        }
    }
}