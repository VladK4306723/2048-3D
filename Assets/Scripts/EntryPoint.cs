using UnityEditor.EditorTools;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour 
{
    [SerializeField] private Transform SpawnPoint;

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
        _levelManager.Init(SpawnPoint, _cubePool, _cubeFactory);
        _levelManager.Launch();
    }
}
