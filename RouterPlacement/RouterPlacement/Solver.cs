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

        public void PlaceRouters()
        {
            for (var i = 0; i < _dataSet.Matrix.GetLongLength(0); i++)
            {
                for (var j = 0; j < _dataSet.Matrix.GetLongLength(1); j++)
                {
                    
                }
            }
        }

        public bool IsCellCovered(Cell router, Cell target)
        {
            if (!(Math.Abs(router.Row - target.Row) <= _dataSet.RouterRangeRadius &&
                  Math.Abs(router.Column - router.Column) <= _dataSet.RouterRangeRadius)) return false;

            var startRow = Math.Min(router.Row, target.Row);
            var startColumn = Math.Min(router.Column, router.Row);

            var endRow = Math.Max(router.Row, target.Row);
            var endColumn = Math.Max(router.Column, router.Row);

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startColumn; j <= endColumn; j++)
                {
                    var currentCell = _dataSet.Matrix[i, j];

                    if (currentCell.Type == CellType.Wall)
                    {
                        if ((startRow <= currentCell.Row && currentCell.Row <= endRow) &&
                            (startColumn <= currentCell.Column && currentCell.Column <= endColumn))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
