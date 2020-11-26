using UnityEngine;

public class LaunchPreview : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 dragStartPoint;

    [SerializeField] GameObject pointPrefab;
    [SerializeField] int numberOfPoints = 10;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetStartPoint(Vector3 worldPoint)
    {
        dragStartPoint = worldPoint;
        lineRenderer.SetPosition(0, dragStartPoint);
    }

    public void SetEndPoint(Vector3 worldPoint)
    {
        Vector3 pointOffset = worldPoint - dragStartPoint;
        Vector3 endPoint = transform.position + pointOffset;

        float distance = Vector3.Distance(transform.position, endPoint);
        lineRenderer.material.mainTextureScale = new Vector2(distance * 2, 1);

        lineRenderer.SetPosition(1, endPoint);
    }
}
