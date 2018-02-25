namespace RouterPlacement
{
    public class Cell
    {
        public CellType Type { get; set; }
        public bool IsCovered { get; set; }

        public int Row { get; }
        public int Column { get; }

        public Cell(int row, int column, CellType cellType)
        {
            Row = row;
            Column = column;
            Type = cellType;
        }
    }
}