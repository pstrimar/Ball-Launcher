using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public event Action<int> onRowSpawned;
    public MainMenuUI mainMenuUI;
    public int playWidth = 7;
    public float distanceBetweenObjects = 0.75f;

    [SerializeField] GameObject[] objectPrefabs;
    [SerializeField] int startingRows = 3;

    private int rowsSpawned;
    private AudioSource audioSource;
    private List<GameObject> objectsSpawned = new List<GameObject>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.onPlay += HandlePlay;
        }
    }

    private void OnDisable()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.onPlay -= HandlePlay;
        }
    }

    public void SpawnRowOfObjects()
    {
        foreach (var _object in objectsSpawned)
        {
            if (_object != null)
            {
                _object.transform.position += Vector3.down * distanceBetweenObjects;
            }
        }
        for (int i = 0; i < playWidth; i++)
        {
            if (UnityEngine.Random.Range(0, 100) <= 50)
            {
                if (UnityEngine.Random.Range(0, 100) <= 85)
                {
                    // Instantiate a block 85% of the time
                    var block = Instantiate(objectPrefabs[UnityEngine.Random.Range(0, 2)], GetPosition(i), Quaternion.identity);

                    int hits = UnityEngine.Random.Range(1, 3) + rowsSpawned;

                    block.GetComponent<Block>().SetHits(hits);
                    objectsSpawned.Add(block);
                }
                else
                {
                    // Instantiate a ball pickup
                    var ball = Instantiate(objectPrefabs[2], GetPosition(i), Quaternion.identity);
                    objectsSpawned.Add(ball);
                }
            }
        }

        rowsSpawned++;
        if (rowsSpawned > startingRows)
            onRowSpawned?.Invoke(rowsSpawned - startingRows);

        audioSource.Play();
    }

    private Vector3 GetPosition(int i)
    {
        Vector3 position = transform.position;
        position += Vector3.right * i * distanceBetweenObjects;
        return position;
    }

    private void HandlePlay()
    {
        foreach (var block in objectsSpawned)
        {
            if (block != null)
                Destroy(block.gameObject);
        }

        objectsSpawned.Clear();
        rowsSpawned = 0;

        for (int i = 0; i < startingRows; i++)
        {
            SpawnRowOfObjects();
        }
    }
}
