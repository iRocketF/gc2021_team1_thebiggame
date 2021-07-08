using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// Basic information on a pool. Tag, the prefab it clones from, and the max size.
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<IPooledObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;

        // Create the dictionary
        poolDictionary = new Dictionary<string, Queue<IPooledObject>>();

        // Populate all the Queues
        foreach (Pool pool in pools)
        {
            Queue<IPooledObject> objectPool = new Queue<IPooledObject>();

            // Loop through the size of the pool, enqueue and instantiate deactivated objects.
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);

                // Fetch the interface and ensure it exists. If not, print error and return
                IPooledObject pooled = obj.GetComponent<IPooledObject>();

                if (pooled == null)
                {
                    Debug.LogError(obj.name + " in the object pool doesn't implement IPooledObject interface");
                    return;
                }

                // Switch the object off and add it to the queue
                pooled.Deactivate();
                objectPool.Enqueue(pooled);
            }

            // Add the new queue to the dictionary
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Dequeues and resets an object from the pool corresponding to given tag. Then activates and
    /// returns that GameObject. If tag was not found, return null.
    /// </summary>
    /// <param name="tag">Name of the pool</param>
    /// <param name="position">Position of the new object</param>
    /// <param name="rotation">Rotation of the new object</param>
    /// <returns>The GameObject from the object pool. Null if tag not found</returns>
    public IPooledObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        IPooledObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.Activate(position, rotation);
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
