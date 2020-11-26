using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public event Action onPlay;
    public GameObject gameOverImage;
    public GameObject titleImage;
    public RectTransform playButton;
    public RectTransform quitButtton;
    public Text playButtonText;
    public BallLauncher launcher;
    public BallReturn ballReturn;
    public GameObject menuUI;
    public RectTransform menuImages;
    public GameObject postProcessing;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        postProcessing.SetActive(false);
#endif
    }
    private void OnEnable()
    {
        FadeIn();

        if (ballReturn != null)
        {
            ballReturn.onGameOver += HandleGameOver;
        }
    }

    private void OnDisable()
    {
        if (ballReturn != null)
        {
            ballReturn.onGameOver -= HandleGameOver;
        }
    }

    public void Play()
    {
        launcher.enabled = true;
        onPlay?.Invoke();

        FadeOut();
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void HandleGameOver()
    {
        launcher.enabled = false;
        menuUI.SetActive(true);
        FadeIn();
        if (titleImage.activeSelf)
            titleImage.SetActive(false);
        if (!gameOverImage.activeSelf)
            gameOverImage.SetActive(true);

        playButtonText.text = "REPLAY";
    }

    private void TurnMenuOff()
    {
        menuUI.SetActive(false);
    }

    private void FadeIn()
    {
        LeanTween.alphaCanvas(menuUI.GetComponent<CanvasGroup>(), 1, .5f).setFrom(0);
    }

    private void FadeOut()
    {
        LeanTween.alphaCanvas(menuUI.GetComponent<CanvasGroup>(), 0, .5f).setFrom(1).setOnComplete(TurnMenuOff); ;
    }
}
