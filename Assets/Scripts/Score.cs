using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text CurrentScoreText;
    public Text BestScoreText;
    public Text BallCountText;

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
        CurrentScoreText.text = currentScore.ToString();
        BestScoreText.text = currentScore.ToString();
        BallCountText.text = startingBallCount.ToString();
    }

    private void OnEnable()
    {
        ObjectSpawner.onRowSpawned += HandleRowSpawned;
        MainMenuUI.onPlay += HandleRetry;
        BallLauncher.onBallAdded += HandleBallAdded;
    }

    private void OnDisable()
    {
        ObjectSpawner.onRowSpawned -= HandleRowSpawned;
        MainMenuUI.onPlay -= HandleRetry;
        BallLauncher.onBallAdded -= HandleBallAdded;
    }

    private void HandleRowSpawned(int score)
    {
        currentScore = score;

        // Increase best score if current score exceeds current best score
        if (bestScore <= score)
            bestScore = score;

        CurrentScoreText.text = currentScore.ToString();

        BestScoreText.text = bestScore.ToString();
    }

    // Update ball count text to current ball count
    private void HandleBallAdded(int ballCount)
    {
        BallCountText.text = ballCount.ToString();
    }

    // Reset current score on retry
    private void HandleRetry()
    {
        currentScore = startingScore;
        CurrentScoreText.text = currentScore.ToString();
    }
}
