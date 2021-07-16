using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private const int menu = 0;

    [SerializeField] int[] levelList1;
    [SerializeField] int[] levelList2;
    [SerializeField] int[] levelList3;
    [SerializeField] int[] levelList4;
    [SerializeField] int[] levelList5;

    private int[] levelOrder;

    private int currentLevel;
    private int lastLevel = 5;  // Always one smaller than the real last level

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
            LoadMenu();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            FindObjectOfType<ExitHandler>().OpenExit();
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
                break;
            case 2:
                levelOrder = levelList2;
                break;
            case 3:
                levelOrder = levelList3;
                break;
            case 4:
                levelOrder = levelList4;
                break;
            case 5:
                levelOrder = levelList5;
                break;
            default:
                levelOrder = levelList1;
                break;
        }
    }

    public void LoadFirstLevel()
    {
        currentLevel = 0;
        ChooseRandomLevels();
        SceneManager.LoadScene(levelOrder[0]);
    }

    public void LoadNextLevel()
    {
        if (currentLevel < lastLevel)
        {
            Debug.Log("Next level");
            currentLevel++;
            int level = levelOrder[currentLevel];
            SceneManager.LoadScene(level);
        }
        else if (currentLevel == lastLevel)
        {
            Debug.Log("Max level reached, starting again");
            ChooseRandomLevels();
            LoadFirstLevel();
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menu);
    }
}
