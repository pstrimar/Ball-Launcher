using UnityEngine;

public class LaunchPreview : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 dragStartPoint;

    [SerializeField] GameObject pointPrefab;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Sets start position of line renderer to worldPoint passed in, which is transform position of ball launcher
    public void SetStartPoint(Vector3 worldPoint)
    {
        dragStartPoint = worldPoint;
        lineRenderer.SetPosition(0, dragStartPoint);
    }

    // Sets end position of line renderer to vector resulting from worldPoint minus start point, added to transform position
    public void SetEndPoint(Vector3 worldPoint)
    {
        Vector3 pointOffset = worldPoint - dragStartPoint;
        Vector3 endPoint = transform.position + pointOffset;

        // Scales the line renderer material in the x direction by twice the distance for desired effect
        float distance = Vector3.Distance(transform.position, endPoint);
        lineRenderer.material.mainTextureScale = new Vector2(distance * 2, 1);

        lineRenderer.SetPosition(1, endPoint);
    }
}
