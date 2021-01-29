using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool : MonoBehaviour
{
    static GameObject prefabBall;
    static GameObject prefabBlock;
    static GameObject prefabDiamond;
    static Dictionary<PooledObjectName, List<GameObject>> pools;

    const int initialBallPoolCapacity = 25;
    const int initialBlockPoolCapacity = 30;

    public static void Initialize()
    {
        // load prefabs
        prefabBall = Resources.Load<GameObject>("Ball");
        prefabBlock = Resources.Load<GameObject>("Block");
        prefabDiamond = Resources.Load<GameObject>("Diamond");

        // initialize dictionary
        pools = new Dictionary<PooledObjectName, List<GameObject>>();
        pools.Add(PooledObjectName.Ball,
            new List<GameObject>(initialBallPoolCapacity));
        pools.Add(PooledObjectName.Block,
            new List<GameObject>(initialBlockPoolCapacity));

        // fill ball pool
        for (int i = 0; i < pools[PooledObjectName.Ball].Capacity; i++)
        {
            pools[PooledObjectName.Ball].Add(GetNewObject(PooledObjectName.Ball));
        }

        // fill block pool
        for (int i = 0; i < pools[PooledObjectName.Block].Capacity; i++)
        {
            pools[PooledObjectName.Block].Add(GetNewObject(PooledObjectName.Block));
        }
    }

    public static Ball GetBall()
    {
        return GetPooledObject(PooledObjectName.Ball).GetComponent<Ball>();
    }

    public static GameObject GetBlock()
    {
        return GetPooledObject(PooledObjectName.Block);
    }

    static GameObject GetPooledObject(PooledObjectName name)
    {
        List<GameObject> pool = pools[name];

        // check for available object in pool
        if (pool.Count > 0)
        {
            // remove object from pool and return
            GameObject obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            return obj;
        }
        else
        {
            // pool empty, so expand pool and return new object
            pool.Capacity++;
            if (name == PooledObjectName.Ball)
            {
                return GetNewObject(PooledObjectName.Ball);
            }
            else
            {
                return GetNewObject(PooledObjectName.Block);
            }
        }
    }

    public static void ReturnBall(GameObject ball)
    {
        ReturnPooledObject(PooledObjectName.Ball, ball);
    }

    public static void ReturnBlock(GameObject block)
    {
        ReturnPooledObject(PooledObjectName.Block, block);
    }

    public static void ReturnPooledObject(PooledObjectName name,
        GameObject obj)
    {
        obj.SetActive(false);
        if (name == PooledObjectName.Ball)
        {
            obj.GetComponent<Ball>().StopMoving();
        }
        pools[name].Add(obj);
    }

    static GameObject GetNewObject(PooledObjectName name)
    {
        GameObject obj;
        if (name == PooledObjectName.Ball)
        {
            obj = Instantiate(prefabBall);
            obj.GetComponent<Ball>().Initialize();
        }
        else
        {
            obj = Random.Range(0, 2) == 0 ? Instantiate(prefabBlock) : Instantiate(prefabDiamond);
            obj.GetComponent<Block>().Initialize();
        }
        obj.SetActive(false);
        DontDestroyOnLoad(obj);
        return obj;
    }

    public static void EmptyPools()
    {
        foreach (KeyValuePair<PooledObjectName, List<GameObject>> kvp in pools)
        {
            pools[kvp.Key].Clear();
        }
    }
}
