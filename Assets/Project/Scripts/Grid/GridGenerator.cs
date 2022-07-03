using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Grid.Cells;
using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Grid
{
    public class GridGenerator : MonoBehaviour, IInitializable
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _cellParent;

        private StaticData _staticData;

        private readonly Dictionary<int, int[]> _cellNeighbours = new Dictionary<int, int[]>();
        private readonly HashSet<Cell> _cells = new HashSet<Cell>();

        public HashSet<Cell> Cells => _cells;

        public readonly Dictionary<int, Cell> CellsDictionary = new Dictionary<int, Cell>();

        [Inject]
        private void Init(StaticData staticData)
        {
            _staticData = staticData;
            
            CreateGrid();
        }

        public async void Initialize()  
        {
            await UniTask.Delay(1000);
            
            SetNeighbours();
        }

        private void CreateGrid()
        {
            Vector3 startPos = _pivot.position;
            Cell cellPrefab = _staticData.CellPrefab;

            int gridSizeX = _staticData.XSize;
            int gridSizeY = _staticData.YSize;
            float cellSize = (cellPrefab.transform.localScale.x + cellPrefab.transform.localScale.z) / 2 + 0.1f;

            int id = 0;

            for (int width = 0; width < gridSizeX; width++)
            {
                for (int height = 0; height < gridSizeY; height++)
                {
                    Vector3 spawnPos = startPos;

                    spawnPos.x += (width * cellSize);
                    spawnPos.z += (height * cellSize);

                    Cell cell = Instantiate(cellPrefab, _cellParent);
                    cell.transform.position = spawnPos;

                    var posInArray = new Vector2Int(width, height);
                    
                    var cellData = new CellData(id,true,posInArray);

                    cell.Init(cellData);
                    
                    _cellNeighbours.Add(cell.CellData.ID, null);
                    _cells.Add(cell);
                    
                    CellsDictionary.Add(id, cell);
                    
                    id++;
                }
            }
        }

        private void SetNeighbours()
        {
            foreach (Cell cell in _cells)
            {
                var overlapSphere = Physics.OverlapSphere(cell.transform.position, cell.transform.localScale.x * 1.2f);

               // Debug.Log($"count {overlapSphere.Length}");
                
                var neighbours = new List<int>();

                for (var index = 0; index < overlapSphere.Length; index++)
                {
                    Collider collider = overlapSphere[index];
                    
                    if (collider.TryGetComponent<Cell>(out var neighbour))
                    {
                        if (neighbour == cell) continue;

                        neighbours.Add(neighbour.CellData.ID);
                    }
                }

                _cellNeighbours[cell.CellData.ID] = neighbours.ToArray();
            }
        }

        public int[] GetNeighbours(int cell)
        {
            var cellNeighbour = _cellNeighbours[cell];
            return cellNeighbour;
        }
        
    }
}