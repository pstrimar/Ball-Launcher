using System;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static event Action onBlockHit;

    private int hitsRemaining = 5;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
    }

    // Sets block text to show hits remaining, and sets color to lerp from start color to white based on hits remaining
    private void UpdateVisualState()
    {
        text.SetText(hitsRemaining.ToString());
        spriteRenderer.color = Color.Lerp(Color.white, new Color32(63, 224, 160, 255), hitsRemaining / 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitsRemaining--;

        onBlockHit?.Invoke();

        // If block is not destroyed, update visual state
        if (hitsRemaining > 0)
            UpdateVisualState();
        else
            Destroy(gameObject);
    }

    // Sets the number of hits remaining and updates visual state accordingly
    internal void SetHits(int hits)
    {
        hitsRemaining = hits;
        UpdateVisualState();
    }
}
