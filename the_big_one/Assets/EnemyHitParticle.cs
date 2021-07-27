using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitParticle : MonoBehaviour
{
    private AudioSource hitSound;
    public AudioClip[] hitSounds;

    void Start()
    {
        hitSound = GetComponent<AudioSource>();
        SetHitSound();
    }


    void SetHitSound()
    {
        float hitNumber = Random.Range(0, hitSounds.Length);
        hitSound.clip = hitSounds[Mathf.RoundToInt(hitNumber)];
        hitSound.Play();
    }
}
