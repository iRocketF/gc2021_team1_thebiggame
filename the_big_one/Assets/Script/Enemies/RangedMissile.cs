using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMissile : MonoBehaviour
{
    private float damage = 2f;
    public float thrust = 15f;
    public GameObject particlePrefab;
    private Rigidbody rb;
    private GameObject pEmitter;
    private ParticleSystem particles;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pEmitter = Instantiate(particlePrefab, transform.position, transform.rotation);
        particles = pEmitter.GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * thrust);
        pEmitter.transform.position = transform.position;
        pEmitter.transform.rotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        particles.Stop();
        Destroy(particles.transform.gameObject, 1f);

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

            Debug.Log("Player took damage");
        }

        Destroy(gameObject);

    }
}
