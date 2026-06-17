using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText;

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
    }

    private void OnDisable()
    {
        SnakeManager.OnIncrementScore -= UpdateScore;
    }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        scoreText.text = $"Score: {score}";
        highscoreText.text = $"Highscore: {highscore}";
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
}
