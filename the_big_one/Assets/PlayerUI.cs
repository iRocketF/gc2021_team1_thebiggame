using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    private Canvas canvas;

    public TextMeshProUGUI healthText;

    private PlayerHealth pHealth;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1f;

        pHealth = GetComponentInParent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = pHealth.currentHealth + " / " + pHealth.maxHealth;
    }
}
