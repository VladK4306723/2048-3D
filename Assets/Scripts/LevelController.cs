using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private AudioSource WinSound;
    [SerializeField] private CubeInteractionHandler CubeInteractionHandler;
    [SerializeField] private int IntForWin;
    [SerializeField] private int PoolSize;

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

        _cubePool.Init(PoolSize, gameObject.transform);
    }


    private void Start()
    {
        _levelManager.Init(SpawnPoint, _cubePool, _cubeFactory, CubeInteractionHandler, IntForWin);
        _levelManager.Launch();
        _levelManager.OnScoreChanged += SetScore;
        _levelManager.OnWinGame += OpenWinScreen;

    }

    private void FixedUpdate()
    {
        _levelManager.Update();
    }

    private void SetScore(int score)
    {
        UIManager.SetScore(score);
    }

    private void OpenWinScreen(int score)
    {
        WinSound.Play();
        UIManager.OpenWinScreen(score);
    }
}