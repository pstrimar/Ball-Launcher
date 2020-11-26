using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text currentScoreText;
    public Text bestScoreText;
    public Text ballCountText;
    public ObjectSpawner spawner;
    public MainMenuUI mainMenuUI;
    public BallLauncher launcher;
    private int startingScore = 0;
    private int startingBallCount = 1;
    private int currentScore;
    private int bestScore;

    private void Awake()
    {
        currentScore = startingScore;
    }

    void Start()
    {
        currentScoreText.text = currentScore.ToString();
        bestScoreText.text = currentScore.ToString();
        ballCountText.text = startingBallCount.ToString();
    }

    private void OnEnable()
    {
        if (spawner != null)
        {
            spawner.onRowSpawned += HandleRowSpawned;
        }

        if (mainMenuUI != null)
        {
            mainMenuUI.onPlay += HandleRetry;
        }

        if (launcher != null)
        {
            launcher.onBallAdded += HandleBallAdded;
        }
    }

    private void OnDisable()
    {
        if (spawner != null)
        {
            spawner.onRowSpawned -= HandleRowSpawned;
        }

        if (mainMenuUI != null)
        {
            mainMenuUI.onPlay -= HandleRetry;
        }

        if (launcher != null)
        {
            launcher.onBallAdded -= HandleBallAdded;
        }
    }

    private void HandleRowSpawned(int score)
    {
        currentScore = score;
        if (bestScore <= score)
            bestScore = score;

        currentScoreText.text = currentScore.ToString();

        bestScoreText.text = bestScore.ToString();
    }

    private void HandleBallAdded(int ballCount)
    {
        ballCountText.text = ballCount.ToString();
    }

    private void HandleRetry()
    {
        currentScore = startingScore;
        currentScoreText.text = currentScore.ToString();
    }
}
