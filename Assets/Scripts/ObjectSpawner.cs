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

    [SerializeField] GameObject[] objectPrefabs;
    [SerializeField] int startingRows = 3;

    private int rowsSpawned;    
    private List<GameObject> objectsSpawned = new List<GameObject>();

    private void OnEnable()
    {
        MainMenuUI.onPlay += HandlePlay;
        BallLauncher.onAllBallsReturned += HandleBallsReturned;
        Block.onBlockHit += HandleBlockHit;
    }

    private void OnDisable()
    {
        MainMenuUI.onPlay -= HandlePlay;
        BallLauncher.onAllBallsReturned -= HandleBallsReturned;
        Block.onBlockHit -= HandleBlockHit;
    }

    public void SpawnRowOfObjects()
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
                    var block = Instantiate(objectPrefabs[UnityEngine.Random.Range(0, 2)], GetPosition(i), Quaternion.identity);

                    // Number of hits to be number of total rows spawned + 1 or 2
                    int hits = UnityEngine.Random.Range(1, 3) + rowsSpawned;

                    // Set number of hits remaining on block
                    block.GetComponent<Block>().SetHits(hits);

                    // Add block to our list of spawned objects
                    objectsSpawned.Add(block);
                }
                // 15% of the time, spawn a ball pickup
                else
                {
                    var ball = Instantiate(objectPrefabs[2], GetPosition(i), Quaternion.identity);
                    // Add ball to our list of spawned objects
                    objectsSpawned.Add(ball);
                }
            }
        }

        // Increase rows spawned count
        rowsSpawned++;

        // After initial row, broadcast each row spawned to increase score
        if (rowsSpawned > startingRows)
            onRowSpawned?.Invoke(rowsSpawned - startingRows);

        RowSpawnedAudioSource.Play();
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
        // Destroys all block gameobjects
        foreach (var block in objectsSpawned)
        {
            if (block != null)
                Destroy(block.gameObject);
        }

        // Clears our list of objects
        objectsSpawned.Clear();

        // Resets rows spawned count
        rowsSpawned = 0;

        // Spawn starting rows
        for (int i = 0; i < startingRows; i++)
        {
            SpawnRowOfObjects();
        }
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
