using System;
using System.Collections;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public static event Action<int> onBallAdded;
    public static event Action onAllBallsReturned;
    public int StartingBallCount = 1;

    private Vector3 startDragPosition;
    private Vector3 endDragPosition;
    private LaunchPreview launchPreview;
    private int ballsTotal = 0;
    private int ballsReady;
    private bool gameIsActive;                                  // This will determine whether or not we can launch balls

    [SerializeField] Ball ballPrefab;

    private void Awake()
    {
        launchPreview = GetComponent<LaunchPreview>();
    }

    private void OnEnable()
    {
        MainMenuUI.onPlay += HandleRetry;
        BallPickup.onBallPickedUp += HandleBallPickup;
        BallReturn.onBallReturned += HandleBallReturned;
        BallReturn.onGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        MainMenuUI.onPlay -= HandleRetry;
        BallPickup.onBallPickedUp -= HandleBallPickup;
        BallReturn.onBallReturned -= HandleBallReturned;
        BallReturn.onGameOver -= HandleGameOver;
    }

    private void Update()
    {
        if (gameIsActive)
        {
            // Gets worldPosition point from mouse position, offset from camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10f;

            // If all balls have returned
            if (ballsReady == ballsTotal)
            {
                // If we left click
                if (Input.GetMouseButtonDown(0))
                {
                    StartDrag(worldPosition);
                }
                // If we continue to hold
                else if (Input.GetMouseButton(0))
                {
                    ContinueDrag(worldPosition);
                }
                // If we release
                else if (Input.GetMouseButtonUp(0))
                {
                    EndDrag();
                }
            }
        }
    }

    private IEnumerator LaunchBalls()
    {
        // Direction to launch balls
        Vector3 direction = startDragPosition - endDragPosition;
        direction.Normalize();

        for (int i = 0; i < ballsTotal; i++)
        {
            Ball ball = ObjectPool.GetBall();
            // Set each balls position to ball launcher position
            ball.transform.position = transform.position;
            // Turn gameobject on
            ball.gameObject.SetActive(true);
            ball.StartMoving(direction);
            // Launch every .1 seconds
            yield return new WaitForSeconds(0.1f);

            // Decrement balls ready count by 1 for each launch
            ballsReady--;
        }
    }

    private void StartDrag(Vector3 worldPosition)
    {
        // Start drag position is set to where we click in world space
        startDragPosition = worldPosition;

        // Pass our transform position for start position of line renderer launch preview
        launchPreview.SetStartPoint(transform.position);
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        // End drag position is where we move our mouse in world space while holding down
        endDragPosition = worldPosition;

        // Vector pointing to start position from end position
        Vector3 direction = startDragPosition - endDragPosition;

        // Passing the direction vector starting from our transform position
        launchPreview.SetEndPoint(transform.position + direction);
    }

    private void EndDrag()
    {
        // Resets the launch preview line renderer end position to transform position
        launchPreview.SetEndPoint(transform.position);
        StartCoroutine(LaunchBalls());
    }

    // Clears our list, sets balls ready to 0, creates 1 ball, sets game to active
    private void HandleRetry()
    {
        ballsReady = 0;
        for (int i = 0; i < StartingBallCount; i++)
        {
            ballsReady++;
            ballsTotal++;
        }
        gameIsActive = true;
    }

    private void HandleBallPickup()
    {
        ballsTotal++;
        ballsReady++;
        // Broadcast ball count
        onBallAdded?.Invoke(ballsTotal);
    }

    private void HandleBallReturned(GameObject ball)
    {
        // Increase balls ready count        
        ballsReady++;

        // If all balls are returned, broadcast message, and set our new x position to x position of last ball returned
        if (ballsReady == ballsTotal)
        {
            onAllBallsReturned?.Invoke();
            transform.position = new Vector2(ball.transform.position.x, transform.position.y);
        }
    }

    private void HandleGameOver()
    {
        ballsTotal = 0;
        ballsReady = 0;
        StartingBallCount = 1;
        gameIsActive = false;
    }
}
