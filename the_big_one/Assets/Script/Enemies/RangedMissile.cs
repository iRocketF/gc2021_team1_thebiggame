using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMissile : MonoBehaviour
{
    private float damage = 2f;
    public float thrust = 15f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(transform.forward * thrust);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

            Debug.Log("Player took damage");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
