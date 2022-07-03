using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Grid.Cells
{
    public class CellsDrawService : ITickable
    {
        private readonly GridInfo _gridInfo;
        private readonly DrawToolsSwitcher _drawToolsSwitcher;
        private readonly DrawStrategiesContainer _drawStrategiesContainer;

        private List<Cell> _lastPath = new List<Cell>();

        private bool _allowedToDraw = true;

        public CellsDrawService(GridInfo gridInfo, DrawToolsSwitcher drawToolsSwitcher, DrawStrategiesContainer drawStrategiesContainer)
        {
            _drawStrategiesContainer = drawStrategiesContainer;
            _drawToolsSwitcher = drawToolsSwitcher;
            _gridInfo = gridInfo;
        }
        
        public void Tick()
        {
            if(_allowedToDraw == false) return;
            
            var leftMouseClicked = Input.GetMouseButton(0);
            var rightMouseClicked = Input.GetMouseButton(1);

            if (_gridInfo.GetCellByClick(out var cell) == false) return;

            if ((leftMouseClicked || rightMouseClicked) == false) return;
            
            var currentTool = _drawToolsSwitcher.CurrentTool;

            IDrawStrategy drawStrategy = GetStrategy(currentTool);

            bool state = leftMouseClicked ? true : false;
            
            drawStrategy.Process(cell, state);
        }

        public async UniTask HighlightPath(List<Cell> path)
        {
            if (_allowedToDraw == false) return;

            _allowedToDraw = false;
            
            _lastPath = path;

            foreach (Cell cell in path)
            {
                if(path.IndexOf(cell) == 0 || path.IndexOf(cell) == path.Count - 1)
                    continue;
                
                cell.CellView.Highlight(Color.blue);
                await UniTask.Delay(100);
            }

            _allowedToDraw = true;
        }

        public void UnHighlightLastPath()
        {
            if(_allowedToDraw == false) 
                return;

            for (var index = 0; index < _lastPath.Count; index++)
            {
                Cell cell = _lastPath[index];
                
                if (index == 0 || index == _lastPath.Count - 1)
                    continue;

                cell.CellView.HighlightDefault();
            }
        }
        
        private IDrawStrategy GetStrategy(int tool)
        {            
            IDrawStrategy drawStrategy;

            switch (tool)
            {
                case 0:
                    drawStrategy = _drawStrategiesContainer.GetStrategy<MakeWalkableDrawStrategy>();
                    break;
                case 1:
                    drawStrategy = _drawStrategiesContainer.GetStrategy<PlaceStartEndCellsDrawStrategy>();
                    break;
                default:
                    Debug.LogError($"Wrong tool selected {tool}");
                    return _drawStrategiesContainer.GetStrategy<MakeWalkableDrawStrategy>();
            }

            return drawStrategy;
        }
        
    }
}