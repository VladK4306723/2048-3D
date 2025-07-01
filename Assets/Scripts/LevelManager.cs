using UnityEngine;
using Zenject;

public interface ILevelManager
{
    void Launch();
    void Init(Transform spawnPoint, ICubePool cubePool, ICubeFactory cubeFactory);
}

public class LevelManager : ILevelManager
{
    private const int c_poolSize = 6;

    private ICubeFactory _cubeFactory;
    private ICubePool _cubePool;
    private Transform _spawnPoint;

    private CubeController currentCube;
    private Rigidbody cubeRigidbody;
    private Camera _mainCamera;

    

    public void Init(Transform spawnPoint, ICubePool cubePool, ICubeFactory cubeFactory)
    {
        _spawnPoint = spawnPoint;
        _cubeFactory = cubeFactory;
        _cubePool = cubePool;

        _mainCamera = Camera.main;

        _cubePool.Init(c_poolSize);
        _cubeFactory.Init(_cubePool);
    }

    public void Launch()
    {
        currentCube = _cubeFactory.Create(_spawnPoint);
        cubeRigidbody = currentCube.GetComponent<Rigidbody>();


        cubeRigidbody.isKinematic = true;
    }


}
