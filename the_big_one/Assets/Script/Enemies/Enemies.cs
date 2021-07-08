using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour, IPooledObject
{
    public void Activate(Vector3 position, Quaternion rotation)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }
}
