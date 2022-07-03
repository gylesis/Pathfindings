using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project.Grid.Cells
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellView _cellView;
        
        private CellData _cellData;

        public CellView CellView => _cellView;
        public CellData CellData => _cellData;

        public void Init(CellData cellData)
        {
            _cellData = cellData;
            UpdateData(cellData);
            
            name += $" {cellData.ID}";
        }
        
        public void UpdateData(CellData cellData, bool changeColor = false)
        {
            _cellData = cellData;

            if (changeColor)
            {
                Color color = _cellData.IsWalkable ? Color.white : Color.red;
                _cellView.Highlight(color);
            }
        }
        
    }
}