using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpawner : MonoBehaviour
{
    public ObjectPooler objectPooler;

    private List<MeleeEnemy> enemyList = new List<MeleeEnemy>();

    [SerializeField] private int amountToSpawn = 5;

    [SerializeField] private float spawnInterval = 1;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        for (int i = 0; i < amountToSpawn; i++)
        {
            Invoke("SpawnMeleeEnemies", spawnInterval + spawnInterval * i);
        }
    }

    private void SpawnMeleeEnemies()
    {
        IPooledObject enemyObj = objectPooler.SpawnFromPool("Melee", transform.position, Quaternion.identity);
        MeleeEnemy enemy = enemyObj.ReturnGameObject().GetComponent<MeleeEnemy>();
        enemyList.Add(enemy);
    }
}
