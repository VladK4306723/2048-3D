using UnityEngine;
using Zenject;

public class ContextInstaller : MonoInstaller
{
    [SerializeField] private GameObject _cubePrefab;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().FromInstance(_cubePrefab).AsSingle();
        Container.Bind<ICubePool>().To<CubePool>().AsSingle();
        Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();
        Container.Bind<ICubeFactory>().To<CubeFactory>().AsSingle();
    }
}