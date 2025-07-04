using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private CubeInteractionHandler CubeInteractionHandler;
    [SerializeField] private int IntForWin;

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
        UIManager.OpenWinScreen(score);
    }
}