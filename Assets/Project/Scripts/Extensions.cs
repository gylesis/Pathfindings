using Project.Grid.Cells;

namespace Project
{
    public static class Extensions
    {
        public static CellData MakeWalkable(this CellData cellData, bool isWalkable)
        {
            return new(cellData.ID, isWalkable, cellData.Position);
        }

        public static CellData SetHCost(this CellData cellData, float hCost)
        {
            return new(cellData.ID, cellData.IsWalkable, cellData.Position, hCost, cellData.GCost);
        }

        public static CellData SetGCost(this CellData cellData, float gCost)
        {
            return new(cellData.ID, cellData.IsWalkable, cellData.Position, cellData.HCost, gCost);
        }

        public static CellData Reset(this CellData cellData)
        {
            return new CellData(cellData.ID, cellData.IsWalkable, cellData.Position);
        }
        
    }
}