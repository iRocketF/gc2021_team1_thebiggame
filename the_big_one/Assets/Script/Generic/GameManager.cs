using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] int[] levelList1;
    [SerializeField] int[] levelList2;
    [SerializeField] int[] levelList3;

    private int[] levelOrder;

    private int currentLevel;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Game manager not found, add one to the scene!");
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // To do: Pause menu
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ChooseRandomLevels();
            // To do: Next level
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
            // To do: Next level
        }
    }

    void ChooseRandomLevels()
    {
        // Max value is exclusive
        int randomizer = Random.Range(1, 4);

        switch (randomizer)
        {
            case 1:
                levelOrder = levelList1;
                Debug.Log("levelList1");
                break;
            case 2:
                levelOrder = levelList2;
                Debug.Log("levelList2");
                break;
            case 3:
                levelOrder = levelList3;
                Debug.Log("levelList3");
                break;
            default:
                levelOrder = levelList1;
                break;
        }

        Debug.Log(levelOrder[1]);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(levelOrder[0]);
    }

    void LoadNextLevel()
    {
        currentLevel++;
        int level = levelOrder[currentLevel];
        SceneManager.LoadScene(level);
    }
}
