using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static event Action onPlay;
    public GameObject GameOverImage;
    public GameObject TitleImage;
    public RectTransform PlayButton;
    public RectTransform QuitButtton;
    public Text PlayButtonText;
    public GameObject MenuUI;
    public RectTransform MenuImages;
    public GameObject PostProcessing;

    private void Awake()
    {
        // Disable post processing if on mobile
#if UNITY_ANDROID || UNITY_IOS
        PostProcessing.SetActive(false);
#endif
    }
    private void OnEnable()
    {
        FadeIn();

        BallReturn.onGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        BallReturn.onGameOver -= HandleGameOver;
    }

    public void Play()
    {
        FadeOut();      
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void HandleGameOver()
    {
        FadeIn();

        // Switches title image to game over image
        if (TitleImage.activeSelf)
            TitleImage.SetActive(false);
        if (!GameOverImage.activeSelf)
            GameOverImage.SetActive(true);

        PlayButtonText.text = "REPLAY";
    }

    private void FadeIn()
    {
        CanvasGroup canvasGroup = MenuUI.GetComponent<CanvasGroup>();

        // Fades alpha of canvasgroup from 0 to 1 over .5 seconds
        LeanTween.alphaCanvas(canvasGroup, 1, .5f).setFrom(0);

        // Makes canvasgroup interactable and block raycasts
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Sets post processing to active if not on mobile
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        PostProcessing.SetActive(true);
#endif
    }

    private void FadeOut()
    {
        CanvasGroup canvasGroup = MenuUI.GetComponent<CanvasGroup>();

        // Fades alpha of canvasgroup from 1 to 0 over .5 seconds
        LeanTween.alphaCanvas(canvasGroup, 0, .5f).setFrom(1).setOnComplete(onPlay);

        // Makes canvasgroup not interactable or block raycasts
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Sets post processing to inactive if not on mobile
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        PostProcessing.SetActive(false);
#endif
    }
}
