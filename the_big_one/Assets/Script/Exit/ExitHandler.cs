using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    [SerializeField] private int enemiesToKill = 5;

    [SerializeField] private GameObject lamp1;
    [SerializeField] private GameObject lamp2;

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

        lamp1.SetActive(true);
        lamp2.SetActive(true);
    }

    public void EnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled == enemiesToKill)
        {
            OpenExit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isExitOpen)
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
