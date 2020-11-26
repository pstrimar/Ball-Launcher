using UnityEngine;

public class ScaleToScreenSize : MonoBehaviour
{
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject topWall;

    private float height;
    private float width;

    private void Start()
    {
        height = Camera.main.orthographicSize * 2;
        width = height * Screen.width / Screen.height;

        leftWall.transform.localScale = new Vector3(leftWall.transform.localScale.x, height, leftWall.transform.localScale.z);

        leftWall.transform.position = new Vector3(-width / 2, 0, 0);

        rightWall.transform.localScale = new Vector3(rightWall.transform.localScale.x, height, rightWall.transform.localScale.z);

        rightWall.transform.position = new Vector3(width / 2, 0, 0);

        topWall.transform.localScale = new Vector3(topWall.transform.localScale.x, width, topWall.transform.localScale.z);

        topWall.transform.position = new Vector3(0, height / 2, 0);

        ObjectSpawner spawner = FindObjectOfType<ObjectSpawner>();

        // 0.6f = width of block, so .75f is width of block plus .15f gap
        spawner.playWidth = Mathf.FloorToInt(width / (.75f));

        // .55f = .1f (width of wall) + .15f (gap) + .3f (half width of block)
        spawner.transform.position = new Vector3((-width / 2) + .55f, spawner.transform.position.y, spawner.transform.position.z);
    }
}
