using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Grid;

namespace Project.Pathfinding
{
    public class WavePathfindingAlgorithm : IPathfindingAlgorithm
    {
        private readonly GridInfo _gridInfo;
        private readonly Dictionary<int, int> _cellsWaveValue = new Dictionary<int, int>();

        public WavePathfindingAlgorithm(GridInfo gridInfo)
        {
            _gridInfo = gridInfo;
        }

        public async UniTask<List<int>> FindPath(int start, int target)
        {
            _cellsWaveValue.Clear();
            
            _cellsWaveValue.Add(start, 0);
            _cellsWaveValue.Add(target, -1);
            
            Queue<int> openSet = new Queue<int>();
            
            openSet.Enqueue(start);
            
            while (openSet.Count > 0)
            {
                int currentCell = openSet.Dequeue();   

                var crossNeighbours = _gridInfo.GetCrossNeighbours(currentCell);
                
                foreach (int neighbourId in crossNeighbours)
                {
                    if(openSet.Contains(neighbourId))
                        continue;

                    if (_cellsWaveValue.ContainsKey(neighbourId) == false)
                    {
                        _cellsWaveValue.Add(neighbourId, _cellsWaveValue[currentCell] + 1);
                        //_gridInfo.GetCellById(neighbourId).CellView.SetText(_cellsWaveValue[currentCell] + 1);
                        openSet.Enqueue(neighbourId);
                    }

                    if (_cellsWaveValue[neighbourId] == -1)
                    {
                        _cellsWaveValue[neighbourId] = _cellsWaveValue[currentCell] + 1;

                        await UniTask.CompletedTask;
                        return CreatePath(start,neighbourId);
                    }
                }
            }
            
            return null;
        }

        private List<int> CreatePath(int start, int target)
        {
            int currentCell = target;

            List<int> path = new List<int>();

            path.Add(target);

            while (_cellsWaveValue[currentCell] != _cellsWaveValue[start])
            {
                var neighbours = _gridInfo.GetCrossNeighbours(currentCell);

                foreach (int neighbour in neighbours)
                {
                    if (_cellsWaveValue.ContainsKey(neighbour) == false) continue;

                    if (_cellsWaveValue[neighbour] < _cellsWaveValue[currentCell])
                    {
                        currentCell = neighbour;
                        path.Add(currentCell);
                        break;
                    }
                }
            }

            path.Reverse();

            return path;
        }
    }
}