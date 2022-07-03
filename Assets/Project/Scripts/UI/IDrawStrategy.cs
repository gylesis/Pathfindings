using Project.Grid.Cells;

namespace Project.UI
{
    public interface IDrawStrategy
    {
        void Process(Cell cell, bool state);
    }
}