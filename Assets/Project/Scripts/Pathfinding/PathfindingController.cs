using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Grid;
using Project.Grid.Cells;
using Project.UI;
using UnityEngine;
using UnityEngine.Profiling;

namespace Project.Pathfinding
{
    public class PathfindingController
    {
        private readonly GridInfo _gridInfo;
        private readonly CellsDrawService _cellsDrawService;
        private readonly PathfindingAlgorithmsContainer _pathfindingAlgorithmsContainer;

        private List<Cell> _lastPath = new List<Cell>();

        private bool _allowedToFindPath = true;
        public bool AllowedToFindPath => _allowedToFindPath;

        public PathfindingController(GridInfo gridInfo, CellsDrawService cellsDrawService, PathfindingAlgorithmsContainer pathfindingAlgorithmsContainer)
        {
            _pathfindingAlgorithmsContainer = pathfindingAlgorithmsContainer;
            _cellsDrawService = cellsDrawService;
            _gridInfo = gridInfo;
        }

        public async void FindPath(AlgorithmType algorithmType)
        {
            Profiler.BeginSample("Pathfinding");
            
            _allowedToFindPath = false;
            ResetCellsData();

            _cellsDrawService.UnHighlightLastPath();
            
            Cell startCell = _gridInfo.StartCell;
            Cell targetCell = _gridInfo.TargetCell;

            if (startCell == null || targetCell == null)
            {
                Debug.Log($"Start or target cell is not set");
                return;
            }
            
            IPathfindingAlgorithm pathfindingAlgorithm;
            
            if (algorithmType == AlgorithmType.Astar)
            {
                pathfindingAlgorithm = _pathfindingAlgorithmsContainer.GetAlgorithm<AStarPathfindingAlgorithm>();
            }
            else if(algorithmType == AlgorithmType.Wave)
            {
                pathfindingAlgorithm = _pathfindingAlgorithmsContainer.GetAlgorithm<WavePathfindingAlgorithm>();
            }
            else
            {
                pathfindingAlgorithm = _pathfindingAlgorithmsContainer.GetAlgorithm<AStarPathfindingAlgorithm>();
            }

            var path = await pathfindingAlgorithm.FindPath(startCell.CellData.ID, targetCell.CellData.ID);

            Profiler.EndSample();

            if (path == null)
            {
                Debug.Log("Unable to find path");
                return;
            }

            _lastPath.Clear();

            foreach (var cellId in path)
            {
                Cell cell = _gridInfo.GetCellById(cellId);
                _lastPath.Add(cell);
            }
            
            await _cellsDrawService.HighlightPath(_lastPath);

            _allowedToFindPath = true;
            
        }

        public void ResetCellsData()
        {
            if(_lastPath.Count == 0) return;

            for (var index = 0; index < _lastPath.Count; index++)
            {
                Cell cell = _lastPath[index];
                CellData cellData = cell.CellData.Reset();
                cell.UpdateData(cellData);

                if (index == 0 || index == _lastPath.Count - 1)
                    continue;

                cell.CellView.HighlightDefault();
            }
        }
        
    }
}