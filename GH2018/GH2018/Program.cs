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
            var input = File.ReadAllLines("input\\a_example.in");

            var dataset = Parser.Parse(input);

            var solver = new Solver(dataset);

            solver.SendVehicles();


        }
    }
}
