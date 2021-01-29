using System;
using UnityEngine;

public class BallReturn : MonoBehaviour
{
    public static event Action onGameOver;
    public static event Action<GameObject> onBallReturned;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Game Over if block crosses ball return line
        if (collision.gameObject.tag == "Block")
        {
            onGameOver?.Invoke();
        }
        else
        {
            // Broadcast ball returned and pass in the ball gameobject
            ObjectPool.ReturnBall(collision.gameObject);
            onBallReturned?.Invoke(collision.gameObject);

            audioSource.Play();
        }
    }
}
