using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RouterPlacement.utils;

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

        public List<Cell> Routers { get; set; } = new List<Cell>();

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
            var graph = AddMatrixToGraph();


            var shortest = new BreadthFirstPaths<Cell>(graph, _dataSet.Matrix[3,8]);
            var path = shortest.pathTo(_dataSet.Matrix[3, 20]);

            foreach (var cell in path)
            {
                _dataSet.Matrix[cell.Row, cell.Column] = new Cell(cell.Row, cell.Column, CellType.Backbone);

                Console.SetCursorPosition(cell.Column, cell.Row);
                Console.Write("b");
            }

            /*
            DrawMatrix(_dataSet);

            CurrentAcceptableRouterCoverage = FindBestRouterCoverage();

            while (RemainigBudget > 0)
	        {
		        PlaceRouters();

	            ConnectRouters();

                CurrentAcceptableRouterCoverage--;
	        }
            */
        }

        private void ConnectRouters()
        {
            var graph = AddMatrixToGraph();

            /*
            for (int i = 0; i < Routers.Count-1; i++)
            {
                var shortest = new BreadthFirstPaths<Cell>(graph, Routers[i]);
                var path = shortest.pathTo(Routers[i+1]);

                //var path = ShortestPathFunction(graph, Routers.First())(Routers[9]);

                foreach (var cell in path)
                {
                    _dataSet.Matrix[cell.Row, cell.Column] = new Cell(cell.Row, cell.Column, CellType.Backbone);

                    Console.SetCursorPosition(cell.Column, cell.Row);
                    Console.Write("b");
                }
            }
            */

            /*
            var shortest = new BreadthFirstPaths<Cell>(graph, Routers.First());
            var path = shortest.pathTo(Routers[9]);

            //var path = ShortestPathFunction(graph, Routers.First())(Routers[9]);

            foreach (var cell in path)
            {
                _dataSet.Matrix[cell.Row, cell.Column] = new Cell(cell.Row, cell.Column, CellType.Backbone);

                Console.SetCursorPosition(cell.Column, cell.Row);
                Console.Write("b");
            }
            */
            /*
             * var graph = AddMatrixToGraph();

            for (int i = 0; i < Routers.Count; i++)
            {
                var shortest = new BreadthFirstPaths<Cell>(graph, Routers[i]);
                var path = shortest.pathTo(Routers[i+1]);

                foreach (var cell in path)
                {
                    _dataSet.Matrix[cell.Row, cell.Column] = new Cell(cell.Row, cell.Column, CellType.Backbone);

                    Console.SetCursorPosition(cell.Column, cell.Row);
                    Console.Write("b");
                }
            }
            */
        }

        /*
        public Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start)
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v => {
                var path = new List<T> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
        */
        private Graph<Cell> AddMatrixToGraph()
        {
            var graph = new Graph<Cell>();

            for (var i = 0; i < _dataSet.Matrix.GetLongLength(0); i++)
            {
                for (var j = 0; j < _dataSet.Matrix.GetLongLength(1); j++)
                {
                    graph.AddVertex(_dataSet.Matrix[i,j]);

                    if (i - 1 >= 0)
                    {
                        var cell = _dataSet.Matrix[i - 1, j];
                        graph.AddEdge(_dataSet.Matrix[i, j], cell);
                    }

                    if (i + 1 < _dataSet.RowCount)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i + 1, j]);
                    }

                    if (j - 1 >= 0)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i, j - 1]);
                    }

                    if (j + 1 < _dataSet.ColumnCount)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i, j + 1]);
                    }
                    if (i + 1 < _dataSet.RowCount && j + 1 < _dataSet.ColumnCount)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i + 1, j + 1]);
                    }
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i - 1, j - 1]);
                    }
                    if (i - 1 >= 0 && j + 1 < _dataSet.ColumnCount)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i - 1, j + 1]);
                    }
                    if (i + 1 < _dataSet.RowCount && j - 1 >= 0)
                    {
                        graph.AddEdge(_dataSet.Matrix[i, j], _dataSet.Matrix[i + 1, j - 1]);
                    }
                }
            }

            return graph;
        }

	    public void PlaceRouters()
	    {
		    for (var i = 0; i < _dataSet.Matrix.GetLongLength(0); i++)
		    {
			    for (var j = 0; j < _dataSet.Matrix.GetLongLength(1); j++)
			    {
				    if (ShouldPlaceRouter(_dataSet.Matrix[i,j], out var _))
				    {
					    if(RemainigBudget <= 0) return;

					    var router = new Cell(i, j, CellType.Router) {IsCovered = true};

				        Routers.Add(router);

                        _dataSet.Matrix[i, j] = router;
					    RemainigBudget -= _dataSet.RouterCost;

					    Console.SetCursorPosition(j, i);
					    Console.Write("r");

					    //Thread.Sleep(100);

					    CoverReachableCells(router);
				    }
			    }
		    }
	    }

        public int FindBestRouterCoverage()
        {
            var maxCoveredCellsCount = 0;
            for (int i = 0; i < _dataSet.RowCount; i++)
            {
                for (int j = 0; j < _dataSet.ColumnCount; j++)
                {
                    ShouldPlaceRouter(_dataSet.Matrix[i,j], out int currentCoverage);

                    if (currentCoverage > 0)
                        maxCoveredCellsCount = currentCoverage;
                }
            }

            return maxCoveredCellsCount;
        }

        private bool ShouldPlaceRouter(Cell cell, out int coveredCellsCount)
        {
            coveredCellsCount = 0;

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

		    coveredCellsCount = 0;
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
                            Console.Write("b");
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
