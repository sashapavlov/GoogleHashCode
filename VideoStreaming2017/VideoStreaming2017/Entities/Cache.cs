﻿using System.Collections.Generic;

namespace VideoStreaming2017.Entities
{
    public class Cache
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();
    }
}