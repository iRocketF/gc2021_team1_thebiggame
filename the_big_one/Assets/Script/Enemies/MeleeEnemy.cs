using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour, IPooledObject
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float pingPongLength = 0.5f;
    [SerializeField] private float pingPongSpeed = 1f;
    [SerializeField] private float minPingPongHeight = 1f;

    [SerializeField] [Tooltip("Child game object of this parent")]
    private GameObject enemyObject;
    private GameObject player;

    public void OnObjectSpawn()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player != null)
        {
            FollowPlayer();
        }

        PingPong();
    }

    void FollowPlayer()
    {
        Vector3 targetPos = player.transform.position;

        transform.LookAt(targetPos);
        agent.SetDestination(targetPos);
    }

    void PingPong()
    {
        float minimum = transform.position.y + minPingPongHeight;

        float y = Mathf.PingPong(Time.time * pingPongSpeed, pingPongLength) + minimum;
        enemyObject.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
