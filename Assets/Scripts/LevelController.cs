using UnityEditor.EditorTools;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour 
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private CubeInteractionHandler CubeInteractionHandler;

    private ILevelManager _levelManager;
    private ICubeFactory _cubeFactory;
    private ICubePool _cubePool;

    [Inject]
    public void Construct(
        ILevelManager levelManager, ICubePool cubePool, ICubeFactory cubeFactory)
    {
        _levelManager = levelManager;
        _cubePool = cubePool;
        _cubeFactory = cubeFactory;
    }


    private void Start()
    {
        _levelManager.Init(SpawnPoint, _cubePool, _cubeFactory, CubeInteractionHandler);
        _levelManager.Launch();
    }

    private void FixedUpdate()
    {
        _levelManager.Update();
    }
}