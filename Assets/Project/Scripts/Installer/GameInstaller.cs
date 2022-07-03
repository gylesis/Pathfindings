using Project.Grid;
using Project.Grid.Cells;
using Project.Pathfinding;
using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private DrawToolsSwitcher _drawToolsSwitcher;
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private UIController _uiController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private StaticData _staticData;
        public override void InstallBindings()
        {
            Container.Bind<StaticData>().FromInstance(_staticData).AsSingle();
            
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
            Container.BindInterfacesAndSelfTo<CameraConfigureService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GridGenerator>().FromInstance(_gridGenerator).AsSingle();
            Container.Bind<GridInfo>().AsSingle();

            Container.Bind<UIController>().FromInstance(_uiController).AsSingle();

            Container.BindInterfacesAndSelfTo<CellsDrawService>().AsSingle();
            Container.Bind<DrawStrategiesContainer>().AsSingle();
            Container.Bind<IDrawStrategy>().To<MakeWalkableDrawStrategy>().AsSingle();
            Container.Bind<IDrawStrategy>().To<PlaceStartEndCellsDrawStrategy>().AsSingle();
            
            Container.Bind<DrawToolsSwitcher>().FromInstance(_drawToolsSwitcher).AsSingle();

            Container.Bind<PathfindingController>().AsSingle();
            Container.Bind<PathfindingAlgorithmsContainer>().AsSingle().NonLazy();
            Container.Bind<IPathfindingAlgorithm>().To<AStarPathfindingAlgorithm>().AsTransient();
            Container.Bind<IPathfindingAlgorithm>().To<WavePathfindingAlgorithm>().AsTransient();
        }
    }
}