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
        rb.velocity = rb.velocity.normalized * moveSpeed;

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
