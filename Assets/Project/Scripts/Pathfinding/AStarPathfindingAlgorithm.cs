using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Grid;
using Project.Grid.Cells;
using UnityEngine.Profiling;

namespace Project.Pathfinding
{
    public class AStarPathfindingAlgorithm : IPathfindingAlgorithm
    {
        private readonly GridInfo _gridInfo;
        private readonly Dictionary<int, int> _cellsParents = new Dictionary<int, int>();

        public AStarPathfindingAlgorithm(GridInfo gridInfo)
        {
            _gridInfo = gridInfo;
        }

        public async UniTask<List<int>> FindPath(int start, int target)
        {
            _cellsParents.Clear();
            
            List<int> openSet = new List<int>();
            HashSet<int> closedSet = new HashSet<int>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                int currentCellId = openSet[0];
                CellData currentCellData = _gridInfo.GetCellById(currentCellId).CellData;

                var neighbours = _gridInfo.GetNeighbours(currentCellId);

                openSet.Remove(currentCellId);
                closedSet.Add(currentCellId);

                if (currentCellId == target)
                {
                    var path = CreatePath(target, start);

                    await UniTask.CompletedTask;
                    return path;
                }

                foreach (int neighbourId in neighbours)
                {
                    if (closedSet.Contains(neighbourId)) continue;

                    Cell neighbour = _gridInfo.GetCellById(neighbourId);

                    float newCostToNeighbour = currentCellData.GCost + _gridInfo.GetDistance(currentCellId, neighbourId);

                    if (newCostToNeighbour < neighbour.CellData.GCost || openSet.Contains(neighbourId) == false)
                    {
                        neighbour.UpdateData(neighbour.CellData.SetGCost(newCostToNeighbour)); 
                        var hCost = _gridInfo.GetDistance(neighbourId, target);
                        
                        neighbour.UpdateData(neighbour.CellData.SetHCost(hCost)); 

                        if (_cellsParents.ContainsKey(neighbourId) == false)
                        {
                            _cellsParents.Add(neighbourId, currentCellId);
                        }
                        else
                        {
                            _cellsParents[neighbourId] = currentCellId;
                        }
                        
                        if (openSet.Contains(neighbourId) == false)
                            openSet.Add(neighbourId);
                    }
                }
            }

            return null;
        }

        private List<int> CreatePath(int target, int start)
        {
            List<int> path = new List<int>();

            int currentNode = target;

            while (currentNode != start)
            {
                path.Add(currentNode);

                currentNode = _cellsParents[currentNode];
            }

            path.Add(start);

            path.Reverse();

            return path;
        }
    }
}