using System;
using UnityEngine;

namespace Project.Grid.Cells
{
    public struct CellData
    {
        public CellData(int id, bool isWalkable, Vector2Int position, float hCost = default, float gCost = default)
        {
            ID = id;
            IsWalkable = isWalkable;
            Position = position;
            HCost = hCost;
            GCost = gCost;
        }
            
        public int ID { get; }
        public bool IsWalkable { get; }
        public Vector2Int Position { get;  }
        public float HCost { get;  }
        public float GCost { get; }
    }

}