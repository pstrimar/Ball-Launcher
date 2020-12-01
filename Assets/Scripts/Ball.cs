using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private int wallHitCount;

    [SerializeField] float moveSpeed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Sets our velocity to our move speed
        rb.velocity = rb.velocity.normalized * moveSpeed;

        // if ball hits walls consecutively 10 times, add gravity to prevent ball from being stuck
        if (wallHitCount >= 10)
        {
            rb.gravityScale = .5f;
            wallHitCount = 0;
        }            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BallPickup")
        {
            audioSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset wall hit count if we hit something else
        if (collision.transform.tag != "Wall")
        {
            wallHitCount = 0;
        }
        else
        {
            wallHitCount++;
        }
    }
}
