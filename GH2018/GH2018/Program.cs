using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH2018
{
    class Program
    {
        static void Main(string[] args)
        {

            var inputs = new List<string>()
            {
                "input\\b_should_be_easy.in",
                "input\\c_no_hurry.in",
                "input\\d_metropolis.in",
                "input\\e_high_bonus.in",
            };

            foreach (var inputItem in inputs)
            {
                var input = File.ReadAllLines(inputItem);

                var dataset = Parser.Parse(input);

                var solver = new Solver(dataset);

                solver.SendVehicles();

                using (StreamWriter sw = File.CreateText(inputItem + ".out"))
                {
                    foreach (Car car in dataset.Cars)
                    {
                        if (car.TakenRides.Count == 0) continue;

                        sw.Write(car.TakenRides.Count + " ");

                        for (int i = 0; i < car.TakenRides.Count; i++)
                        {
                            if (i == car.TakenRides.Count - 1)
                                sw.Write(car.TakenRides[i].Id);
                            else
                                sw.Write(car.TakenRides[i].Id + " ");
                        }

                        sw.Write('\n');
                    }
                }

            }
        }
    }
}
