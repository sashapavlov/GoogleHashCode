using System;
using System.Collections.Generic;
using System.Linq;

namespace GH2018
{
    public class RideScore
    {
        public int Score { get; set; }
        public Ride Ride { get; set; }
    }

    public class Solver
    {
        public DataSet DataSet { get; set; }
        public int CurrentStep { get; set; }
        public Solver(DataSet dataSet)
        {
            this.DataSet = dataSet;
        }
        public void SendVehicles()
        {
            Console.WriteLine("Solving..");

            foreach (var car in DataSet.Cars)
            {
                var sortedOptimalRides = FillListOfOptimalRidesWithScore(car).OrderBy(or => or.Score).ToList();

                while (AddOneOptimalRide(sortedOptimalRides, car))
                {
                    sortedOptimalRides = FillListOfOptimalRidesWithScore(car).OrderBy(or => or.Score).ToList();

                    if(sortedOptimalRides.Where(i => i.Score == -1).ToList().Count == sortedOptimalRides.Count) break;
                }

                if (CurrentStep > DataSet.StepsLimit) return;
            }
        }

        private bool AddOneOptimalRide(List<RideScore> sortedOptimalRides, Car car)
        {
            foreach (var optimalRide in sortedOptimalRides)
            {
                if (optimalRide.Score == -1) continue;

                if (!AssignRideToCar(optimalRide.Ride, car)) return false;
            }
            return true;
        }

        public bool AssignRideToCar(Ride optimalRide, Car car)
        {
            var steps = FindDistanceFromCarToRideStart(car, optimalRide) + FindDistanceFromStartToFinish(optimalRide);

            car.CarStepCount += steps;

            CurrentStep += steps;

            if (CurrentStep > DataSet.StepsLimit) return false;

            car.TakenRides.Add(optimalRide);

            DataSet.Rides.Remove(optimalRide);

            return true;
        }

        public List<RideScore> FillListOfOptimalRidesWithScore(Car car)
        {
            var ridesWithScoreAccordingToGivenCar = new List<RideScore>();
            foreach (var ride in DataSet.Rides)
            {
                var rideScore = new RideScore();
                var distance = FindDistanceFromCarToRideStart(car, ride);
                var score = Math.Abs(
                    FindDiviationBetweenCarStepAndRideEarlierStart(distance, car.CarStepCount, ride.EarliestStart));

                rideScore.Ride = ride;
                rideScore.Score = score;

                if (FindDistanceToFinish(distance, ride) - ride.LatestFinish > 0) rideScore.Score = -1;

                ridesWithScoreAccordingToGivenCar.Add(rideScore);
            }

            return ridesWithScoreAccordingToGivenCar;
        }

        public int FindDistanceFromCarToRideStart(Car car, Ride ride)
        {
            var distance = Math.Abs((car.CarRow - ride.StartRow)) + Math.Abs((car.CarColumn - ride.StartColumn));
            return distance;
        }

        public int FindDistanceFromStartToFinish(Ride ride)
        {
            var distance = Math.Abs(ride.StartRow - ride.FinishRow) + Math.Abs(ride.StartColumn - ride.FinishColumn);
            return distance;
        }

        public int FindDiviationBetweenCarStepAndRideEarlierStart(int distanceFromCarToRide, int carStep, int rideEarlierStart)
        {
            var diviation = rideEarlierStart - (carStep + distanceFromCarToRide);
            return diviation;
        }

        public int FindDistanceToFinish(int distanceToStart, Ride ride)
        {
            var distanceToFinish = distanceToStart + FindDistanceFromStartToFinish(ride);
            return distanceToFinish;
        }

    }
}
