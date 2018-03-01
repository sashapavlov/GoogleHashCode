using System.Collections.Generic;

namespace GH2018
{
    public class Car
    {
        public int Id { get; set; }
        public int CarRow { get; set; }
        public int CarColumn { get; set; }
        public int CarStepCount { get; set; }
        public List<Ride> TakenRides { get; set; } = new List<Ride>();
    }
}