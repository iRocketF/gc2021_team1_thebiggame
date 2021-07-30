using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private GameManager manager;
    [SerializeField] private GameObject pModel;
    [SerializeField] private GameObject particleHit;
    [SerializeField] private GameObject particleDeath;

    public float maxHealth;
    public float currentHealth;

    public AudioSource hurtSound;
    public AudioClip[] hurtSounds;

    void Start()
    {
        manager = GameManager.Instance;

        if (manager.playerHP > 0)
            currentHealth = manager.playerHP;
        else
            currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {

        if(currentHealth > 0f)
        {
            currentHealth -= damage;
            SetHurtSound();
            Instantiate(particleHit, transform.position, Quaternion.identity);

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                Instantiate(particleDeath, transform.position, Quaternion.identity);
                Die();
            }
        }
        

    }

    public void AddHealth(float healthAmount)
    {
        currentHealth += healthAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    void Die()
    {
        pModel.SetActive(false);
    }

    void SetHurtSound()
    {
        float hitNumber = Random.Range(0, hurtSounds.Length);
        hurtSound.clip = hurtSounds[Mathf.RoundToInt(hitNumber)];
        hurtSound.Play();
    }
}
