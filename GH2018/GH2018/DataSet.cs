using System.Collections.Generic;

namespace GH2018
{
    public class DataSet
    {
        public int RowsCount { get; set; }
        public int ColumsCount { get; set; }
        public int VehiclesCount { get; set; }
        public int RidesCount { get; set; }
        public int PerRideBonus { get; set; }
        public int StepsLimit { get; set; }
        public List<Ride> Rides = new List<Ride>();
        public List<Car> Cars = new List<Car>();
    }
}