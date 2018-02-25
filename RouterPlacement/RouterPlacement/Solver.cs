using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterPlacement
{
    public class Solver
    {
        private readonly DataSet _dataSet;

        public int RemainigBudget { get; set; }
        public int RemainingRoutersCount { get; set; }
        public int BackboneCellsCount { get; set; }

        public Solver(DataSet dataSet)
        {
            _dataSet = dataSet;
        }
    }
}
