using Project.Grid.Cells;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(menuName = "Static Data", fileName = "StaticData", order = 0)]
    public class StaticData : ScriptableObject
    {
        [SerializeField] private int _xSize = 5;
        [SerializeField] private int _ySize = 5;
        [SerializeField] private Cell _cellPrefab;

        public float Seconds;
        
        public Cell CellPrefab => _cellPrefab;
        public int YSize => _ySize;
        public int XSize => _xSize;
    }
}