using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static event Action<int> onRowSpawned;
    public int PlayWidth = 7;
    public float DistanceBetweenObjects = 0.75f;
    public AudioSource RowSpawnedAudioSource;
    public AudioSource BlockHitAudioSource;

    [SerializeField] GameObject ballPickupPrefab;
    [SerializeField] int startingRows = 3;

    private int rowsSpawned;
    private List<GameObject> objectsSpawned = new List<GameObject>();

    private void OnEnable()
    {
        MainMenuUI.onPlay += HandlePlay;
        BallLauncher.onAllBallsReturned += HandleBallsReturned;
        Block.onBlockHit += HandleBlockHit;
        BallReturn.onGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        MainMenuUI.onPlay -= HandlePlay;
        BallLauncher.onAllBallsReturned -= HandleBallsReturned;
        Block.onBlockHit -= HandleBlockHit;
        BallReturn.onGameOver -= HandleGameOver;
    }

    private void SpawnRowOfObjects()
    {
        foreach (var _object in objectsSpawned)
        {
            // If we have objects in a row already, move them down by our distance
            if (_object != null)
            {
                _object.transform.position += Vector3.down * DistanceBetweenObjects;
            }
        }

        // for each potential spawn location
        for (int i = 0; i < PlayWidth; i++)
        {
            // 50% of the time, spawn something
            if (UnityEngine.Random.Range(0, 100) < 50)
            {
                // 85% of the time, spawn a block
                if (UnityEngine.Random.Range(0, 100) < 85)
                {
                    // Instantiate either a square block or a diamond block
                    var block = ObjectPool.GetBlock();
                    block.transform.position = GetPosition(i);

                    // Number of hits to be number of total rows spawned + 1 or 2
                    int hits = UnityEngine.Random.Range(1, 3) + rowsSpawned;

                    // Add block to our list of spawned objects
                    objectsSpawned.Add(block);

                    // Set number of hits remaining on block
                    block.GetComponent<Block>().SetHits(hits);

                    block.SetActive(true);
                }
                // 15% of the time, spawn a ball pickup
                else
                {
                    var ball = Instantiate(ballPickupPrefab, GetPosition(i), Quaternion.identity);
                    // Add ball to our list of spawned objects
                    objectsSpawned.Add(ball);
                }
            }
        }

        // Increase rows spawned count
        rowsSpawned++;

        // After initial rows, broadcast each row spawned to increase score
        if (rowsSpawned > startingRows)
            onRowSpawned?.Invoke(rowsSpawned - startingRows);

        RowSpawnedAudioSource.Play();
    }

    public void RemoveFromList(GameObject obj)
    {
        objectsSpawned.Remove(obj);
    }

    private Vector3 GetPosition(int i)
    {
        // Position of our spawner
        Vector3 position = transform.position;

        // Moves position right by our distance for each new object in the row
        position += Vector3.right * i * DistanceBetweenObjects;
        return position;
    }

    private void HandlePlay()
    {
        // Resets rows spawned count
        rowsSpawned = 0;

        // Spawn starting rows
        for (int i = 0; i < startingRows; i++)
        {
            SpawnRowOfObjects();
        }
    }

    private void HandleGameOver()
    {
        // Return all block gameobjects to pool
        foreach (var _object in objectsSpawned)
        {
            if (_object.GetComponent<Block>() != null)
            {
                ObjectPool.ReturnBlock(_object);
            }
            else
            {
                Destroy(_object);
            }
        }

        // Clears our list of objects
        objectsSpawned.Clear();
    }

    private void HandleBallsReturned()
    {
        SpawnRowOfObjects();
    }

    private void HandleBlockHit()
    {
        BlockHitAudioSource.Play();
    }
}
