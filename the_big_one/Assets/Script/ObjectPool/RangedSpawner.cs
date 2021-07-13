using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpawner : MonoBehaviour
{
    public ObjectPooler objectPooler;

    private List<RangedEnemy> enemyList = new List<RangedEnemy>();

    [SerializeField] private int amountToSpawn = 2;

    [SerializeField] private float spawnInterval = 2;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;

        for (int i = 0; i < amountToSpawn; i++)
        {
            Invoke("SpawnRangedEnemies", spawnInterval + spawnInterval * i);
        }
    }

    private void SpawnRangedEnemies()
    {
        IPooledObject enemyObj = objectPooler.SpawnFromPool("Ranged", transform.position, Quaternion.identity);
        RangedEnemy enemy = enemyObj.ReturnGameObject().GetComponent<RangedEnemy>();
        enemyList.Add(enemy);
    }
}
