using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoStreaming.Entities;

namespace VideoStreaming
{
    public class Solver
    {
        private readonly List<Cache> _caches;
        public int TotalTimeSavedForAllRequests { get { return totalTimeSavedForAllRequests; } }
        private int totalTimeSavedForAllRequests;

        public Solver(List<Cache> caches)
        {
            _caches = caches;
        }

        public void AddVideoPretenders()
        {
            Console.WriteLine("Adding video pretenders...");

            foreach (var cache in _caches)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                //var enps = cache.Endpoints.OrderByDescending(e => e.VideoRequestCountList.Sum(v => v.RequestCount));

                //var test = enps.Select(e => e.VideoRequestCountList.Sum(v => v.RequestCount));

                foreach (var endpoint in cache.Endpoints)
                {
                    Parallel.ForEach(endpoint.VideoRequestCountList, videoRequestCount =>
                    {
                        var enpointToCacheLatency = endpoint.CacheLatencies.Single(c => c.Cache.Id == cache.Id).Latency;
                        var timeSaved = videoRequestCount.RequestCount * (endpoint.DatacenterLatency - enpointToCacheLatency);
                        var requestCount = videoRequestCount.RequestCount;

                        var pretender = cache.VideoPretenders.FirstOrDefault(vp => vp.Video.Id == videoRequestCount.Video.Id);
                        if (pretender != null)
                        {
                            pretender.TotalTimeSaved += timeSaved;
                            pretender.TotalRequestCount += requestCount;
                        }
                        else
                        {
                            cache.VideoPretenders.Enqueue(new VideoPretender
                            {
                                Video = videoRequestCount.Video,
                                TotalTimeSaved = timeSaved,
                                TotalRequestCount = requestCount
                            });
                        }
                    });
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
                Console.WriteLine("RunTime " + elapsedTime);
            }
        }

        public void AddOptimalVideosToCorrespondingCaches()
        {
            Console.WriteLine("Adding optimal videos to corresponding caches...");

            foreach (var cache in _caches)
            {
                ReorderPretenders(cache);

                foreach (var videoPretender in cache.VideoPretenders.OrderByDescending(v => v.TotalTimeSaved))
                {
                    if (videoPretender.TotalTimeSaved == 0) break;

                    if (cache.SavedVideos.Contains(videoPretender.Video)) continue;

                    if (cache.Size >= videoPretender.Video.Size)
                    {
                        cache.SavedVideos.Add(videoPretender.Video);
                        cache.Size -= videoPretender.Video.Size;
                        totalTimeSavedForAllRequests += videoPretender.TotalTimeSaved;
                    }              
                }
            }
        }

        private void ReorderPretenders(Cache cache)
        {
            foreach (var videoPretender in cache.VideoPretenders.OrderByDescending(v => v.TotalTimeSaved))
            {
                if (videoPretender.TotalTimeSaved == 0) break;

                var cachesWithVideo = GetCachesForVideo(videoPretender.Video.Id);

                if (!cachesWithVideo.Any())
                {
                    if (cache.Size >= videoPretender.Video.Size)
                    {
                        cache.SavedVideos.Add(videoPretender.Video);
                        cache.Size -= videoPretender.Video.Size;
                        totalTimeSavedForAllRequests += videoPretender.TotalTimeSaved;
                    }
                }
                else
                {
                    var intersectingEndpoints = GetIntersectingEndpoints(cachesWithVideo, cache);

                    foreach (var endpoint in intersectingEndpoints)
                    {
                        var intersectingEnpointWithVideo =
                            endpoint.VideoRequestCountList.SingleOrDefault((v =>
                                v.Video.Id == videoPretender.Video.Id));

                        if (intersectingEnpointWithVideo != null)
                        {
                            var requestCountForVideoPretender = intersectingEnpointWithVideo.RequestCount;
                            var dataCenterLatencyForVideoPretender = endpoint.DatacenterLatency;
                            var endpointLatencyToCache = endpoint.CacheLatencies.Single(c => c.Cache.Id == cache.Id).Latency;

                            videoPretender.TotalTimeSaved -= requestCountForVideoPretender *
                                                             (dataCenterLatencyForVideoPretender - endpointLatencyToCache);
                        }
                    }
                }
            }
        }

        private static Endpoint[] GetIntersectingEndpoints(IEnumerable<Cache> cachesWithVideo, Cache cache)
        {
            var endpointsWithRequestedVideo = new List<Endpoint>();

            foreach (var cacheWithVideo in cachesWithVideo)
            {
                foreach (var endpoint in cacheWithVideo.Endpoints)
                {
                    var targetEndpoint = cache.Endpoints.SingleOrDefault(e => e.Id == endpoint.Id);

                    if (targetEndpoint != null)
                    {
                        endpointsWithRequestedVideo.Add(targetEndpoint);
                    }
                }
            }

            return endpointsWithRequestedVideo.Distinct().ToArray();
        }

        public Cache[] GetCachesForVideo(int videoId)
        {
            return _caches.Where(cache => cache.SavedVideos.Any(v => v.Id == videoId)).ToArray();
        }
    }
}
