﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RouterPlacement
{
    public class Solver
    {
        private readonly DataSet _dataSet;

        public int RemainigBudget { get; set; }
        public int RemainingRoutersCount { get; set; }
        public int BackboneCellsCount { get; set; }
	    public int MaxRouterCoverage { get; set; }
	    public int CurrentAcceptableRouterCoverage { get; set; }
	    public SortedList<int, Cell> Routers = new SortedList<int, Cell>();

        public Solver(DataSet dataSet)
        {
            _dataSet = dataSet;
            RemainigBudget = dataSet.Budget;
	        MaxRouterCoverage = (int) Math.Pow(2 * _dataSet.RouterRangeRadius + 1, 2);
	        CurrentAcceptableRouterCoverage = MaxRouterCoverage;
        }

        public void Solve()
        {
            DrawMatrix(_dataSet);

	        while(RemainigBudget > 0)
	        {
		        PlaceRouters();
		        CurrentAcceptableRouterCoverage--;
	        }
        }

	    public void ConnectRouters()
	    {
		    
	    }

		// wtf
	    public List<Cell> FindPath(Cell source, Cell destination, List<Cell> path)
	    {
		    if (source.Row == destination.Row && source.Column == destination.Column) return path;

		    if (source.Row == _dataSet.RowCount || source.Column == _dataSet.ColumnCount) return null;
		    if (source.Row == 0 || source.Column == 0) return null;

		    //if (source.Row > destination.Row || source.Column == destination.Column) return null;

		    path.Add(source);

		    var r1 = FindPath(new Cell(source.Row + 1, source.Column, CellType.Backbone), destination, path);
		    var r2 = FindPath(new Cell(source.Row - 1, source.Column, CellType.Backbone), destination, path);
		    var r3 = FindPath(new Cell(source.Row, source.Column + 1, CellType.Backbone), destination, path);
			var r4 = FindPath(new Cell(source.Row, source.Column - 1, CellType.Backbone), destination, path);
	    }

	    public void PlaceRouters()
	    {
		    for (var i = 0; i < _dataSet.Matrix.GetLongLength(0); i++)
		    {
			    for (var j = 0; j < _dataSet.Matrix.GetLongLength(1); j++)
			    {
				    if (ShouldPlaceRouter(_dataSet.Matrix[i,j]))
				    {
					    if(RemainigBudget <= 0) return;

					    var router = new Cell(i, j, CellType.Router) {IsCovered = true};

					    _dataSet.Matrix[i, j] = router;
					    Routers.Add(i+j, router);
					    RemainigBudget -= _dataSet.RouterCost;

					    Console.SetCursorPosition(j, i);
					    Console.Write("r");

					    //Thread.Sleep(100);

					    CoverReachableCells(router);
				    }
			    }
		    }
	    }

	    private bool ShouldPlaceRouter(Cell cell)
	    {
	        if (cell.Type == CellType.VoidCell || cell.Type == CellType.Wall || cell.Type == CellType.Router)
	            return false;

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

	        if (coveredCellsCount >= CurrentAcceptableRouterCoverage)
	        {
	            return true;
	        }

		    return false;
	    }

        public bool IsCellCovered(Cell router, Cell target)
        {
            if (target.Type == CellType.VoidCell || target.Type == CellType.Wall) return false;

            if (!(Math.Abs(router.Row - target.Row) <= _dataSet.RouterRangeRadius &&
                  Math.Abs(router.Column - router.Column) <= _dataSet.RouterRangeRadius)) return false;

            var startRow = Math.Min(router.Row, target.Row);
            var startColumn = Math.Min(router.Column, target.Column);

            var endRow = Math.Max(router.Row, target.Row);
            var endColumn = Math.Max(router.Column, router.Column);

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

				    if (IsCellCovered(router, _dataSet.Matrix[i, j]))
				    {
					    _dataSet.Matrix[i, j].IsCovered = true;

				        //Console.SetCursorPosition(j, i);
				        //Console.Write("c");
				        //Thread.Sleep(100);

                        coveredCellsCount++;
				    }
			    }
		    }
	    }

        private void DrawMatrix(DataSet dataSet)
        {
            Console.Clear();
            for (int i = 0; i < dataSet.RowCount; i++)
            {
                for (int j = 0; j < dataSet.ColumnCount; j++)
                {
                    switch (dataSet.Matrix[i, j].Type)
                    {
                        case CellType.TargetCell:
                            Console.Write(".");
                            break;
                        case CellType.VoidCell:
                            Console.Write("-");
                            break;
                        case CellType.Wall:
                            Console.Write("#");
                            break;
                        case CellType.Backbone:
                            Console.Write("~");
                            break;
                        case CellType.Router:
                            Console.Write("r");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
