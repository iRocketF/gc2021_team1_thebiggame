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
    public int loopCounter;
    public int levelCounter;

    [SerializeField] float chamberHeal;
    public float playerHP;

    public ExitHandler handler;
    public AudioSource ambientMusic;
    public AudioSource combatMusic;

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
        handler = FindObjectOfType<ExitHandler>();

        if (Input.GetKeyDown(KeyCode.N))
        {
            FindObjectOfType<ExitHandler>().OpenExit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if(handler != null)
        {
            UpdateMusic();
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
        playerHP = 0f;

        currentLevel = 0;
        ChooseRandomLevels();

        if (SceneManager.GetActiveScene().buildIndex == menu)
        {
            loopCounter = 0;
            levelCounter = 0;
        }

        SceneManager.LoadScene(levelOrder[0]);
    }

    public void LoadNextLevel()
    {
        if (currentLevel < lastLevel)
        {
            PlayerHealth pHealth = FindObjectOfType<PlayerHealth>();
            playerHP = pHealth.currentHealth;

            if(playerHP < pHealth.maxHealth)
            {
                playerHP += chamberHeal;
                if (playerHP > pHealth.maxHealth)
                    playerHP = pHealth.maxHealth;
            }

            Debug.Log("Next level");
            currentLevel++;
            int level = levelOrder[currentLevel];
            SceneManager.LoadScene(level);
        }
        else if (currentLevel == lastLevel)
        {
            Debug.Log("Max level reached, starting again");
            loopCounter++;
            LoadFirstLevel();
        }

        levelCounter++;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menu);
    }

    void Restart()
    {
        loopCounter = 0;
        levelCounter = 0;
        LoadFirstLevel();
    }

    void UpdateMusic()
    {
        if (handler.enemiesAlive && !handler.startCombat && !handler.isMusicFading)
        {
            StartCoroutine(FadeMusic.StartFade(ambientMusic, 2f, 0f));
            StartCoroutine(FadeMusic.StartFade(combatMusic, 2f, 0.5f));
            handler.startCombat = true;
        }
        else if (!handler.enemiesAlive && handler.startCombat && !handler.isMusicFading)
        {
            StartCoroutine(FadeMusic.StartFade(combatMusic, 2f, 0f));
            StartCoroutine(FadeMusic.StartFade(ambientMusic, 2f, 0.5f));
            handler.startCombat = false;
        }
    }
}
