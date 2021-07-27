using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonProjectile : MonoBehaviour
{
    public float thrust;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        AddForce();
    }

    public void AddForce()
    {
        rigidBody.AddForce(transform.forward * thrust);
    }

    public int damage;

    private void OnTriggerEnter(Collider other)
    {
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
