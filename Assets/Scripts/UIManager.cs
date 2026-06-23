using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [InspectorName("MainMenu")]
    [SerializeField] Canvas MainMenuCanvas;

    [Space(10)]
    [InspectorName("GameLoss")]
    [SerializeField] Canvas GameLossCanvas;
    [SerializeField] TextMeshProUGUI gameLossScoreText;
    [SerializeField] TextMeshProUGUI gameLossHighscoreText;

    [Space(10)]
    [InspectorName("Gameplay")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] Canvas GameplayCanvas;

    [Space(10)]
    [InspectorName("GameSettings")]
    [SerializeField] GameDifficulty gameDifficulty;

    public static UIManager Instance;

    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        SnakeManager.OnIncrementScore += UpdateScore;
        SnakeManager.OnGameLossEvent += GameLoss;
    }

    private void OnDisable()
    {
        SnakeManager.OnIncrementScore -= UpdateScore;
        SnakeManager.OnGameLossEvent -= GameLoss;
    }

    void Start()
    {
        if (scoreText != null)
        {
            highscore = PlayerPrefs.GetInt("Highscore", 0);
            scoreText.text = $"Score: {score}";
            highscoreText.text = $"Highscore: {highscore}";
        }

        if(gameDifficulty != null)
            gameDifficulty.SetGameDifficulty(DifficultySettings.Easy);
    }
    
    void Update()
    {
        
    }

    void UpdateScore()
    {
        score = SnakeManager.Instance.score;
        scoreText.text = $"Score: {score}";

        if(highscore < score)
            PlayerPrefs.SetInt("Highscore", score);
    }

    void GameLoss()
    {
        Time.timeScale = 0f;
        gameLossScoreText.text = $"Score: {score}";
        gameLossHighscoreText.text = $"Highscore: {PlayerPrefs.GetInt("Highscore", 0)}";

        GameplayCanvas.gameObject.SetActive(false);
        GameLossCanvas.gameObject.SetActive(true);
    }

    public void OnClickPlayButton()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void OnClickRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void OnClickDifficultyButton()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        TextMeshProUGUI buttonText = clickedButton.GetComponentInChildren<TextMeshProUGUI>();


        switch (gameDifficulty.difficultySettings)
        {
            case DifficultySettings.Easy:
                gameDifficulty.SetGameDifficulty(DifficultySettings.Medium);
                break;
            case DifficultySettings.Medium:
                gameDifficulty.SetGameDifficulty(DifficultySettings.Hard);
                break;
            case DifficultySettings.Hard:
                gameDifficulty.SetGameDifficulty(DifficultySettings.Easy);
                break;
        }

        buttonText.text = $"Difficulty: {gameDifficulty.difficultySettings}";
    }
}
