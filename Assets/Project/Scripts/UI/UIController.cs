using Project.Grid;
using Project.Grid.Cells;
using Project.Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private AlgorithmSwitcher _algorithmSwitcher;
        [SerializeField] private Image _buttonImage;
        
        private PathfindingController _pathfindingController;
        private GridInfo _gridInfo;
        private Cell _startCell;
        private Cell _targetCell;

        [Inject]
        public void Init(PathfindingController pathfindingController, GridInfo gridInfo)
        {
            _gridInfo = gridInfo;
            _pathfindingController = pathfindingController;
        }

        public void Start()
        {
            _gridInfo.StartCellSet += OnStartCellSet;
            _gridInfo.TargetCellSet += OnTargetCellSet;
            
            _startButton.onClick.AddListener((OnStartButtonClicked));
            HideStartButton();
        }

        private void OnTargetCellSet(Cell cell)
        {
            _targetCell = cell;

            CheckForStartAndTargetCells();
        }

        private void OnStartCellSet(Cell cell)
        {
            _startCell = cell;

            CheckForStartAndTargetCells();
        }

        public void ShowStartButton()
        {
            _startButton.enabled = true;
            _buttonImage.color = Color.white;
        }

        private void CheckForStartAndTargetCells()
        {
            _pathfindingController.ResetCellsData();

            if (_startCell != null && _targetCell != null)
            {
                ShowStartButton();
            }
            else
            {
                HideStartButton();
            }
        }
        
        public void HideStartButton()
        {
            _startButton.enabled = false;

            Color color = Color.white;
            color.a = 0.5f;

            _buttonImage.color = color;
        }

        private void OnStartButtonClicked()
        {
            if(_pathfindingController.AllowedToFindPath == false) return;
            
            //Debug.Log($"Started finding path with algorithm {_algorithmSwitcher.AlgorithmType}");
            _pathfindingController.FindPath(_algorithmSwitcher.AlgorithmType);
        }
 
        private void OnDestroy()
        {
            _startButton.onClick.RemoveAllListeners();
            _gridInfo.StartCellSet -= OnStartCellSet;
            _gridInfo.TargetCellSet -= OnTargetCellSet;
        }
    }
    
}