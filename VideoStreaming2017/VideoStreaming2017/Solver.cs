using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStreaming2017.Entities;

namespace VideoStreaming2017
{
    public class Solver
    {
        private readonly List<Cache> _caches;

        public Solver(List<Cache> caches)
        {
            _caches = caches;
        }

        public void AddVideoPretenders()
        {
            foreach (var cache in _caches)
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

        public void AddOptimalVideosToCorrespondingCaches()
        {
            foreach (var cache in _caches)
            {
                ReorderPretenders(cache);

                foreach (var videoPretender in cache.VideoPretenders.OrderByDescending(v => v.TotalTimeSaved))
                {
                    if (cache.SavedVideos.Contains(videoPretender.Video)) continue;

                    if (cache.Size >= videoPretender.Video.Size)
                    {
                        cache.SavedVideos.Add(videoPretender.Video);
                    }              
                }
            }
        }

        private void ReorderPretenders(Cache cache)
        {
            foreach (var videoPretender in cache.VideoPretenders)
            {
                var cachesWithVideo = GetCachesForVideo(videoPretender.Video.Id);

                if (!cachesWithVideo.Any())
                {
                    if (cache.Size >= videoPretender.Video.Size)
                    {
                        cache.SavedVideos.Add(videoPretender.Video);
                        cache.Size -= videoPretender.Video.Size;
                    }
                }
                else
                {
                    var intersectingEndpoints = GetIntersectingEndpoints(cachesWithVideo, cache);

                    foreach (var endpoint in intersectingEndpoints)
                    {
                        var requestCountForVideoPretender = endpoint.VideoRequestCountList.Single(v => v.Video.Id == videoPretender.Video.Id).RequestCount;
                        var dataCenterLatencyForVideoPretender = endpoint.DatacenterLatency;
                        var endpointLatencyToCache = endpoint.CacheLatencies.Single(c => c.Cache.Id == cache.Id).Latency;

                        videoPretender.TotalTimeSaved -= requestCountForVideoPretender *
                                                         (dataCenterLatencyForVideoPretender - endpointLatencyToCache);
                    }
                }
            }
        }

        private static List<Endpoint> GetIntersectingEndpoints(List<Cache> cachesWithVideo, Cache cache)
        {
            var endpointsWithRequestedVideo = new List<Endpoint>();

            foreach (var cacheWithVideo in cachesWithVideo)
            {
                foreach (var endpoint in cacheWithVideo.Endpoints)
                {
                    endpointsWithRequestedVideo.Add(cache.Endpoints.Single(e => e.Id == endpoint.Id));
                }
            }

            return endpointsWithRequestedVideo.Distinct().ToList();
        }

        public List<Cache> GetCachesForVideo(int videoId)
        {
            return _caches.Where(cache => cache.SavedVideos.Any(v => v.Id == videoId)).ToList();
        }
    }
}
