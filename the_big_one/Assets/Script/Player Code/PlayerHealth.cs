using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private GameManager manager;
    [SerializeField] private GameObject pModel;

    public float maxHealth;
    public float currentHealth;

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
        currentHealth -= damage;

        if (currentHealth <= 0f)
            Die();
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
}
