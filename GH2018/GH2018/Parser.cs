using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH2018
{
    public static class Parser
    {
        public static DataSet Parse(string[] lines)
        {
            Console.WriteLine("Parsing...");

            var description = lines[0].Split(' ');

            int rowsCount = int.Parse(description[0]);
            int columsCount = int.Parse(description[1]);
            int vehiclesCount = int.Parse(description[2]);
            int ridesCount = int.Parse(description[3]);

            int perRideBonus = int.Parse(description[4]);
            int stepsLimit = int.Parse(description[5]);

            var dataSet = new DataSet()
            {
                RowsCount = rowsCount,
                ColumsCount = columsCount,
                VehiclesCount = vehiclesCount,
                RidesCount = ridesCount,
                PerRideBonus = perRideBonus,
                StepsLimit = stepsLimit
            };

            for (int i = 0; i < lines.Length-1; i++)
            {
                var rideLine = lines[i+1].Split(' ');

                var ride = new Ride
                {
                    Id = i,
                    StartRow = int.Parse(rideLine[0]),
                    StartColumn = int.Parse(rideLine[1]),
                    FinishRow = int.Parse(rideLine[2]),
                    FinishColumn = int.Parse(rideLine[3]),
                    EarliestStart = int.Parse(rideLine[4]),
                    LatestFinish = int.Parse(rideLine[5])
                };

                dataSet.Rides.Add(ride);
            }

            for (int i = 0; i < vehiclesCount; i++)
            {
                dataSet.Cars.Add(new Car
                {
                    Id = i
                });
            }

            return dataSet;
        }
    }
}
