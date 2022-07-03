using System;
using System.Collections.Generic;
using System.Linq;
using Project.Grid.Cells;
using Project.Pathfinding;
using UnityEngine;
using UnityEngine.Profiling;

namespace Project.Grid
{
    public class GridInfo
    {
        private readonly GridGenerator _gridGenerator;
        private readonly List<Cell> _unWalkableCells = new List<Cell>();
        private readonly StaticData _staticData;
        private readonly CameraController _cameraController;

        private Cell _lastLeftPressedCell;
        private Cell _lastRightPressedCell;

        private Cell _startCell;
        private Cell _targetCell;

        public HashSet<Cell> Cells => _gridGenerator.Cells;
        public Cell StartCell => _startCell;
        public Cell TargetCell => _targetCell;

        public event Action<Cell> StartCellSet;
        public event Action<Cell> TargetCellSet;

        public GridInfo(GridGenerator gridGenerator, StaticData staticData, CameraController cameraController)
        {
            _cameraController = cameraController;
            _staticData = staticData;
            _gridGenerator = gridGenerator;
        }

        public void MakeCellWalkable(Cell cell, bool isWalkable)
        {
            if (cell == _startCell || cell == _targetCell) return;

            CellData newCellData = cell.CellData.MakeWalkable(isWalkable);
            cell.UpdateData(newCellData, true);

            if (isWalkable)
            {
                if (_unWalkableCells.Contains(cell))
                    _unWalkableCells.Remove(cell);
            }
            else
            {
                if (_unWalkableCells.Contains(cell) == false)
                    _unWalkableCells.Add(cell);
            }
        }

        public void SetStartCell(Cell cell)
        {
            if (_unWalkableCells.Contains(cell)) return;

            if(cell == _targetCell) return;
            
            if (_startCell != null)
                _startCell.CellView.HighlightDefault();

            _startCell = cell;
            _startCell.CellView.Highlight(Color.green);

            StartCellSet?.Invoke(cell);
        }

        public void SetEndCell(Cell cell)
        {
            if (_unWalkableCells.Contains(cell)) return;

            if(cell == _startCell) return;
            
            if (_targetCell != null)
                _targetCell.CellView.HighlightDefault();

            _targetCell = cell;
            _targetCell.CellView.Highlight(Color.black);

            TargetCellSet?.Invoke(cell);
        }

        public bool GetCellByClick(out Cell cell)
        {
            cell = null;

            var raycast = Raycast(out var hit);

            if (raycast == false) return false;

            var hasCellComponent = hit.collider.TryGetComponent<Cell>(out var foundCell);

            cell = foundCell;

            if (hasCellComponent == false) return false;

            return foundCell;
        }

        private bool Raycast(out RaycastHit hitInfo)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = _cameraController.Camera.ScreenPointToRay(mousePosition);

            Vector3 localScale = _staticData.CellPrefab.transform.localScale;

            var sphereCast = Physics.SphereCast(ray, localScale.x / 2, out RaycastHit hit);

            hitInfo = hit;

            return sphereCast;
        }

        public Cell GetCellById(int id)
        {
            return _gridGenerator.CellsDictionary[id];
        }

        public List<int> GetNeighbours(int id)
        {
            var potentialNeighbours = _gridGenerator.GetNeighbours(id);

            var neighbours = new List<int>();

            for (int i = 0; i < potentialNeighbours.Length; i++)
            {
                int neighbour = potentialNeighbours[i];

                Cell cell = GetCellById(neighbour);

                if (cell.CellData.IsWalkable == false)
                    continue;

                neighbours.Add(neighbour);
            }

            return neighbours;
        }

        public List<int> GetCrossNeighbours(int id)
        {
            var potentialNeighbours = GetNeighbours(id);

            var neighbours = new List<int>();

            for (int i = 0; i < potentialNeighbours.Count; i++)
            {
                int neighbour = potentialNeighbours[i];

                var distance = GetDistance(id, neighbour);

                Vector3 localScale = _staticData.CellPrefab.transform.localScale;

                float cellSize = ((localScale.x + localScale.z) / 2) + 0.05f;

                if (distance > cellSize)
                    continue;

                neighbours.Add(neighbour);
            }

            return neighbours;
        }

        public float GetDistance(int origin, int target)
        {
            Cell startCell = _gridGenerator.CellsDictionary[origin];
            Cell targetCell = _gridGenerator.CellsDictionary[target];

            int xDistance = Mathf.Abs(startCell.CellData.Position.x - targetCell.CellData.Position.x);
            int yDistance = Mathf.Abs(startCell.CellData.Position.y - targetCell.CellData.Position.y);

            if (xDistance > yDistance)
                return 1.4f * yDistance + 1 * (xDistance - yDistance);

            return 1.4f * xDistance + 1 * (yDistance - xDistance);
        }
    }
}