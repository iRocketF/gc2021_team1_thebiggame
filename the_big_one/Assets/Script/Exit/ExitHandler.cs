using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    [SerializeField] private int enemiesToKill = 5;
    [SerializeField] private Renderer dooRenderer;
    private int enemiesKilled;
    private bool isExitOpen = false;

    public bool isMusicFading;
    public bool enemiesAlive;
    public bool startCombat;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyKilled();
        }

        if(enemiesToKill > enemiesKilled)
            enemiesAlive = true;
        else if (enemiesToKill <= enemiesKilled)
            enemiesAlive = false;
    }

    public void OpenExit()
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
        //gameObject.GetComponent<Renderer>().material.color = Color.green;
        dooRenderer.material.color = Color.green;
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
