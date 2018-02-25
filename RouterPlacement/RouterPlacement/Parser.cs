using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterPlacement
{
    public static class Parser
    {
        public static DataSet Parse(string filePath)
        {
            var fileLines = File.ReadAllLines(filePath);

            var description = fileLines[0].Split(' ');
            var rowCount = int.Parse(description[0]);
            var columnCount = int.Parse(description[1]);
            var routerRangeRadius = int.Parse(description[2]);

            var resources = fileLines[1].Split(' ');
            var backboneCosts = int.Parse(resources[0]);
            var routerCosts = int.Parse(resources[1]);
            var budget = int.Parse(resources[2]);

            var initialCell = fileLines[2].Split(' ');
            var initCellRow = int.Parse(initialCell[0]);
            var initCellColumn = int.Parse(initialCell[0]);
            var initialConnectedCell = new Cell(initCellRow, initCellColumn, CellType.Backbone);

            var dataSet = new DataSet(rowCount, columnCount, routerRangeRadius, backboneCosts, routerCosts, budget, initialConnectedCell);

            FillCells(dataSet, fileLines);

            return dataSet;
        }

        private static void FillCells(DataSet dataSet, string[] fileLines)
        {
            for (var i = 0; i < dataSet.Matrix.GetLongLength(0); i++)
            {
                var line = fileLines[3 + i];

                for (var j = 0; j < dataSet.Matrix.GetLongLength(1); j++)
                {
                    var cellSymbol = line[j];

                    var cell = CreateCell(i, j, cellSymbol);

                    dataSet.Matrix[i, j] = cell;
                }
            }
        }

        private static Cell CreateCell(int row, int column, char cellSymbol)
        {
            switch (cellSymbol)
            {
                case '#':
                    return new Cell(row, column, CellType.Wall);
                case '.':
                    return new Cell(row, column, CellType.TargetCell);
                case '-':
                    return new Cell(row, column, CellType.VoidCell);
                default:
                    throw new ArgumentException("Invalid argument", nameof(cellSymbol));
            }
        }
    }
}
