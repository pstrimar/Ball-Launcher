using System;
using UnityEngine;

public class BallPickup : MonoBehaviour
{
    public static event Action onBallPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            // Broadcast ball picked up
            onBallPickedUp?.Invoke();
            FindObjectOfType<ObjectSpawner>().RemoveFromList(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
