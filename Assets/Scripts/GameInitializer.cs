using System;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        ObjectPool.Initialize();
    }
}
