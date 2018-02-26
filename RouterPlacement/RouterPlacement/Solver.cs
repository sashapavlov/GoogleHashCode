using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
	                var router = new Cell(i, j, CellType.Router);
	                if (ShouldPlaceRouter(router))
	                {
		                _dataSet.Matrix[i, j] = router;
	                }
                }
            }
        }

	    private bool ShouldPlaceRouter(Cell cell)
	    {
		    var rowStart = cell.Row - _dataSet.RouterRangeRadius;
		    var columnStart = cell.Column - _dataSet.RouterRangeRadius;

		    if (rowStart < 0) rowStart = 0;
		    if (columnStart < 0) columnStart = 0;

		    var rowEnd = cell.Row + _dataSet.RouterRangeRadius;
		    var columnEnd = cell.Column + _dataSet.RouterRangeRadius;

		    if (rowEnd > _dataSet.RowCount) rowEnd = _dataSet.RowCount;
		    if (columnEnd > _dataSet.ColumnCount) columnEnd = _dataSet.ColumnCount;

		    var coveredCellsCount = 0;
		    for (int i = rowStart; i < rowEnd; i++)
		    {
			    for (int j = columnStart; j < columnEnd; j++)
			    {
					if (_dataSet.Matrix[i, j].IsCovered) continue;

				    if (IsCellCovered(cell, _dataSet.Matrix[i, j]))
				    {
					    coveredCellsCount++;
				    }
			    }
		    }

		    if (coveredCellsCount == (2 * _dataSet.RouterRangeRadius + 1) * 2) return true;

		    return true;
	    }

	    public void CoverReachableCells(Cell router)
	    {
		    var rowStart = router.Row - _dataSet.RouterRangeRadius;
		    var columnStart = router.Column - _dataSet.RouterRangeRadius;

		    if (rowStart < 0) rowStart = 0;
		    if (columnStart < 0) columnStart = 0;

		    var rowEnd = router.Row + _dataSet.RouterRangeRadius;
		    var columnEnd = router.Column + _dataSet.RouterRangeRadius;

		    if (rowEnd > _dataSet.RowCount) rowEnd = _dataSet.RowCount;
		    if (columnEnd > _dataSet.ColumnCount) columnEnd = _dataSet.ColumnCount;

		    var coveredCellsCount = 0;
		    for (int i = rowStart; i < rowEnd; i++)
		    {
			    for (int j = columnStart; j < columnEnd; j++)
			    {
				    if (_dataSet.Matrix[i, j].IsCovered) continue;

				    if (!IsCellCovered(router, _dataSet.Matrix[i, j]))
				    {
					    _dataSet.Matrix[i, j].IsCovered = true;
					    coveredCellsCount++;
				    }
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

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startColumn; j < endColumn; j++)
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
