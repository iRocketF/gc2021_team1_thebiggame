using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonProjectile : MonoBehaviour
{
    public float thrust;
    public GameObject particlePrefab;

    private Rigidbody rigidBody;
    private ParticleSystem particles;
    private GameObject pEmitter;
    private bool particlesActive = true;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        pEmitter = Instantiate(particlePrefab, transform.position, transform.rotation);
        particles = pEmitter.GetComponent<ParticleSystem>();

        AddForce();
    }

    public void AddForce()
    {
        rigidBody.AddForce(transform.forward * thrust);
    }

    public int damage;
    
    private void FixedUpdate()
    {
        if (particlesActive)
        {
            pEmitter.transform.position = transform.position;
            pEmitter.transform.rotation = transform.rotation;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (particlesActive)
        {
            particlesActive = false;
            particles.Stop();
            Destroy(particles.transform.gameObject, 1f);
        }

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
        else
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (particlesActive)
        {
            particlesActive = false;
            particles.Stop();
            Destroy(particles.transform.gameObject, 1f);
        }

        if (!other.gameObject.CompareTag("Enemy"))
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
