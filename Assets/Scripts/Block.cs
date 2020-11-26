using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int hitsRemaining = 5;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        text.SetText(hitsRemaining.ToString());
        spriteRenderer.color = Color.Lerp(Color.white, new Color32(63, 224, 160, 255), hitsRemaining / 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitsRemaining--;
        audioSource.Play();

        if (hitsRemaining > 0)
            UpdateVisualState();
        else
            Destroy(gameObject);
    }

    internal void SetHits(int hits)
    {
        hitsRemaining = hits;
        UpdateVisualState();
    }
}
