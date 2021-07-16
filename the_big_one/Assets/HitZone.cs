using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Hit");
            MeleeEnemy enemy = other.gameObject.GetComponentInParent<MeleeEnemy>();
            RangedEnemy rEnemy = other.gameObject.GetComponent<RangedEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (rEnemy != null)
            {
                rEnemy.TakeDamage(damage);
            }
        }

    }
}
