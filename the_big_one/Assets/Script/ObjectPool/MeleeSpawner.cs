using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpawner : MonoBehaviour
{
    private void FixedUpdate()
    {
        ObjectPooler.Instance.SpawnFromPool("Melee", transform.position, Quaternion.identity);
    }
}
