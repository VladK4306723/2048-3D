using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour 
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private CubeInteractionHandler CubeInteractionHandler;
    [SerializeField] private int IntForWin;
    [SerializeField] private TextMeshProUGUI ScoreText;

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
        ScoreText.text = "Score: 0";
    }

    private void FixedUpdate()
    {
        _levelManager.Update();
    }

    private void SetScore(int score)
    {
        ScoreText.text = new string("Score: " + score.ToString());
    }
}