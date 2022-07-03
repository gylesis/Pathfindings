using Project.Grid;
using Project.Grid.Cells;

namespace Project.UI
{
    public class PlaceStartEndCellsDrawStrategy : IDrawStrategy
    {
        private readonly GridInfo _gridInfo;

        public PlaceStartEndCellsDrawStrategy(GridInfo gridInfo)
        {
            _gridInfo = gridInfo;
        }
        
        public void Process(Cell cell, bool state)
        {
            if (state)
            {
                _gridInfo.SetStartCell(cell);
            }
            else
            {
                _gridInfo.SetEndCell(cell);
            }
        }
    }
}