using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemies
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float pingPongLength = 0.5f;
    [SerializeField] private float minPingPongHeight = 1f;
    private float pingPongSpeed;

    [SerializeField] [Tooltip("Child game object of this parent")]
    private GameObject enemyObject;
    private GameObject player;

    [SerializeField] private int health = 10;
    [SerializeField] private float inVulnerabilityTime = 0.2f;
    private bool isInVulnerable = false;

    [SerializeField] private int damageAmount = 5;

    public void Start()
    {
        player = GameObject.Find("Player");

        pingPongSpeed = Random.Range(0.2f, 0.6f);
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

    public virtual void DeActivate()
    {
        FindObjectOfType<ExitHandler>().EnemyKilled();
        base.Deactivate();
    }

    public void TakeDamage(int damage)
    {
        if (!isInVulnerable)
        {
            health -= damage;
        }
        
        if (health <= 0)
        {
            DeActivate();
        }
        else
        {
            isInVulnerable = true;
            Invoke("InvulnerabilityOff", inVulnerabilityTime);
        }
    }

    void InvulnerabilityOff()
    {
        isInVulnerable = false;
    }
}
