using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterPlacement
{
    public class DataSet
    {
        public int RowCount { get; }
        public int ColumnCount { get; }
        public int RouterRangeRadius { get; }
        public int BackboneCost { get; }
        public int RouterCost { get; }
        public int Budget { get; }
        public Cell InitialConnectedCell { get; }
        public Cell[,] Matrix { get; set; }

        public DataSet(int rowCount, int columnCount, int routerRangeRadius, int backboneCost, int routerCost, int budget, Cell initialConnectedCell)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            RouterRangeRadius = routerRangeRadius;
            BackboneCost = backboneCost;
            RouterCost = routerCost;
            Budget = budget;
            InitialConnectedCell = initialConnectedCell;

            Matrix = new Cell[RowCount, ColumnCount];
        }
    }
}
