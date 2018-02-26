using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterPlacement
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemUtils.Maximize();

            var dataSet = Parser.Parse("input\\charleston_road.in");

            var solver = new Solver(dataSet);

            solver.PlaceRouters();

            Console.ReadKey();
        }
    }
}
