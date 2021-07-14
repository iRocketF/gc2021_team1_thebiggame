using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MeleeEnemy : Enemies
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float pingPongLength = 0.5f;
    [SerializeField] private float minPingPongHeight = 1f;
    private float pingPongSpeed;

    [SerializeField] [Tooltip("Child game object of this parent")]
    private GameObject enemyObject;
    private GameObject player;
    [SerializeField] private GameObject hitParticle;


    [SerializeField] private int health = 10;
    [SerializeField] private float inVulnerabilityTime = 0.2f;
    private bool isInVulnerable = false;

    [SerializeField] private int damageAmount = 5;
    [SerializeField] private float attackSphereRadius = 3f;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float attackTimerAmount = 2f;
    private float attackTimer;

    public void Start()
    {
        player = GameObject.Find("Player");
        SetUp();
    }

    void Update()
    {
        if (player != null)
        {
            FollowPlayer();
        }

        PingPong();
        PrepareToAttack();
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

    void PrepareToAttack()
    {
        Vector3 enemyOrigin = enemyObject.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(enemyOrigin, attackSphereRadius, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player"))
            {
                PlayerHealth health = hitCollider.gameObject.GetComponent<PlayerHealth>();
                Attack(health);
                break;
            }
        }
    }

    void Attack(PlayerHealth healthSystem)
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            healthSystem.TakeDamage(damageAmount);

            Debug.Log("Player took damage from melee");
            attackTimer = attackTimerAmount;
        }
    }

    public virtual void DeActivate()
    {
        FindObjectOfType<ExitHandler>().EnemyKilled();
        base.Deactivate();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("You hit me");
        if (!isInVulnerable)
        {
            health -= damage;
            attackTimer = attackTimerAmount;

        }
        
        if (health <= 0)
        {
            GameObject hitParticleClone = Instantiate(hitParticle, enemyObject.transform.position, enemyObject.transform.rotation);
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

    void SetUp()
    {
        pingPongSpeed = Random.Range(0.2f, 0.6f);

        attackTimer = attackTimerAmount;
    }
}
