using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    private Canvas canvas;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI levelCounterText;

    private PlayerHealth pHealth;
    private GameManager manager;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.Instance;

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1f;

        pHealth = GetComponentInParent<PlayerHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = pHealth.currentHealth + " / " + pHealth.maxHealth;

        if (pHealth.currentHealth <= 0)
            deathText.enabled = true;
        else
            deathText.enabled = false;

        levelCounterText.text = "Rooms cleared: " + GameManager.Instance.levelCounter.ToString();
    }
}
