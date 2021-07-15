using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Hit");
            MeleeEnemy enemy = other.gameObject.GetComponentInParent<MeleeEnemy>();

            enemy.TakeDamage(damage);
        }

    }
}
