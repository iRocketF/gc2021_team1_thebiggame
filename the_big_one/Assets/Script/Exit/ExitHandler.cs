using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    [SerializeField] private int enemiesToKill = 5;
    private int enemiesKilled;
    private bool isExitOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyKilled();
        }
    }

    void OpenExit()
    {
        isExitOpen = true;
        ChangeExitColor();
    }

    public void EnemyKilled()
    {
        Debug.Log("Enemy killed");
        enemiesKilled++;

        if (enemiesKilled == enemiesToKill)
        {
            Debug.Log("Exit open");
            OpenExit();
        }
    }

    void ChangeExitColor()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isExitOpen)
        {
            Debug.Log("You won the level!");
            GameManager.Instance.LoadNextLevel();
        }
    }
}
