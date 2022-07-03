using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Grid.Cells;

namespace Project.Pathfinding
{
    public interface IPathfindingAlgorithm
    {
        UniTask<List<int>> FindPath(int start, int target);
    }
}