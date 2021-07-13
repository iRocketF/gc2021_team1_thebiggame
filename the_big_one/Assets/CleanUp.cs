using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUp : MonoBehaviour
{
    public float cleanUpTime;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cleanUpTime)
            Clean();
    }

    void Clean()
    {
        Destroy(gameObject);
    }
}
