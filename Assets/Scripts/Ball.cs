using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private int wallHitCount;
    private float impulseForce = 10;

    [SerializeField] float moveSpeed = 10f;

    public void Initialize()
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

    public void StartMoving(Vector3 direction)
    {
        Vector3 forceVector = direction * impulseForce;
        rb.gravityScale = 0;
        rb.AddForce(forceVector, ForceMode2D.Impulse);
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
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
