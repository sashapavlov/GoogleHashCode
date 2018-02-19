using System.Collections.Generic;

namespace VideoStreaming.Entities
{
    public class Endpoint
    {
        public int Id { get; set; }
        public int DatacenterLatency { get; set; }
        public List<VideoRequestCount> VideoRequestCountList { get; set; } = new List<VideoRequestCount>();
        public List<CacheLatency> CacheLatencies { get; set; } = new List<CacheLatency>();
    }
}
