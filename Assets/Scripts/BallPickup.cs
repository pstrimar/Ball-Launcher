using UnityEngine;

public class BallPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            FindObjectOfType<BallLauncher>().CreateBall();

            Destroy(this.gameObject);
        }
    }
}
