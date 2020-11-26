using System;
using UnityEngine;

public class BallReturn : MonoBehaviour
{
    public event Action onGameOver;
    public BallLauncher ballLauncher;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            onGameOver?.Invoke();
        }
        else
        {
            ballLauncher.ReturnBall(collision.transform.position.x);
            collision.gameObject.SetActive(false);
            audioSource.Play();
        }
    }
}
