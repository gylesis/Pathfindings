using Project.Grid;
using Project.Grid.Cells;

namespace Project.UI
{
    public class MakeWalkableDrawStrategy : IDrawStrategy
    {
        private readonly GridInfo _gridInfo;

        public MakeWalkableDrawStrategy(GridInfo gridInfo)
        {
            _gridInfo = gridInfo;
        }
        
        public void Process(Cell cell, bool state)
        {
            _gridInfo.MakeCellWalkable(cell, !state);
        }
    }
}