using System.Linq;
using Project.Grid;
using Project.Grid.Cells;
using UnityEngine;
using Zenject;

namespace Project
{
    public class CameraConfigureService : IInitializable
    {
        private readonly GridInfo _gridInfo;
        private readonly CameraController _cameraController;
        private readonly StaticData _staticData;

        public CameraConfigureService(GridInfo gridInfo, CameraController cameraController, StaticData staticData)
        {
            _staticData = staticData;
            _cameraController = cameraController;
            _gridInfo = gridInfo;
        }
        
        public void Initialize()
        {
            Cell first = _gridInfo.GetCellById(0);
            Cell last = _gridInfo.GetCellById(_gridInfo.Cells.Count - 1);

            Vector3 firstPos = first.transform.position;
            Vector3 secondPos = last.transform.position;

            Vector3 diagonal = secondPos - firstPos;

            Vector3 movePos = diagonal / 2;

            _cameraController.MoveTo(movePos);

            var height = ((_staticData.XSize + _staticData.YSize) / 2) + 0.1f;
            
            _cameraController.SetHeight(height);
        }
    }
}