using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VideoStreaming.Entities
{
    public class Cache
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();
        //public List<VideoPretender> VideoPretenders { get; set; } = new List<VideoPretender>();
        public ConcurrentQueue<VideoPretender> VideoPretenders { get; set; } = new ConcurrentQueue<VideoPretender>();
        public List<Video> SavedVideos { get; set; } = new List<Video>();
    }
}