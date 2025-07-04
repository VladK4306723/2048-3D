using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI HighScoreText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private Button ReloadGameBTN;
    [SerializeField] private Button ExitGameBTN;

    private ILevelManager _levelManager;

    private const string HighScoreKey = "HighScore";

    [Inject]
    public void Construct(
        ILevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void Start()
    {
        ReloadGameBTN.onClick.AddListener(ReloadLevel);
        ExitGameBTN.onClick.AddListener(ExitGame);

        ScoreText.text = "Score: 0";
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        HighScoreText.text = "High Score: " + highScore.ToString();
    }

    public void SetScore(int score)
    {
        ScoreText.text = "Score: " + score.ToString();
    }

    public void OpenWinScreen(int score)
    {
        WinPanel.SetActive(true);

        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
            HighScoreText.text = "High Score: " + score.ToString();
        }
    }

    private void ReloadLevel()
    {
        WinPanel.SetActive(false);
        _levelManager.Reload();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
