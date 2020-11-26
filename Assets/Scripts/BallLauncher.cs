using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public event Action<int> onBallAdded;
    public ObjectSpawner objectSpawner;
    public MainMenuUI mainMenuUI;

    private Vector3 startDragPosition;
    private Vector3 endDragPosition;
    private LaunchPreview launchPreview;
    private List<Ball> balls = new List<Ball>();
    private int ballsReady;

    [SerializeField] Ball ballPrefab;


    private void Awake()
    {
        launchPreview = GetComponent<LaunchPreview>();
        CreateBall();
    }

    private void OnEnable()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.onPlay += HandleRetry;
        }
    }

    private void OnDisable()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.onPlay -= HandleRetry;
        }
    }

    private void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10f;

        if (ballsReady == balls.Count)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDrag(worldPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                ContinueDrag(worldPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }

    public void CreateBall()
    {
        var ball = Instantiate(ballPrefab);
        ball.gameObject.SetActive(false);
        balls.Add(ball);
        ballsReady++;

        onBallAdded?.Invoke(balls.Count);
    }

    public void ReturnBall(float xPos)
    {
        ballsReady++;
        if (ballsReady == balls.Count)
        {
            objectSpawner.SpawnRowOfObjects();
            transform.position = new Vector2(xPos, transform.position.y);
        }
    }

    private IEnumerator LaunchBalls()
    {

        Vector3 direction = startDragPosition - endDragPosition;
        direction.Normalize();

        foreach (var ball in balls)
        {
            ball.transform.position = transform.position;
            ball.gameObject.SetActive(true);
            ball.GetComponent<Rigidbody2D>().gravityScale = 0f;
            ball.GetComponent<Rigidbody2D>().AddForce(direction);

            yield return new WaitForSeconds(0.1f);
            ballsReady--;
        }
    }

    private void EndDrag()
    {
        launchPreview.SetEndPoint(transform.position);
        StartCoroutine(LaunchBalls());
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        endDragPosition = worldPosition;

        Vector3 direction = startDragPosition - endDragPosition;
        launchPreview.SetEndPoint(transform.position + direction);
    }

    private void StartDrag(Vector3 worldPosition)
    {
        startDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }

    private void HandleRetry()
    {
        ballsReady = 0;
        balls.Clear();
        CreateBall();
    }
}
