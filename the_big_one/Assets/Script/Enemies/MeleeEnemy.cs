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
    private float originalSpeed;

    [SerializeField] [Tooltip("Child game object of this parent")]
    private GameObject enemyObject;
    private GameObject player;
    private PlayerHealth playerHealth;
    [SerializeField] private GameObject hitParticle;

    [SerializeField] private int health = 10;
    [SerializeField] private float inVulnerabilityTime = 0.2f;
    private bool isInVulnerable = false;

    [SerializeField] private float damageAmount = 5;
    [SerializeField] private float attackSphereRadius = 3f;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float attackTimerAmount = 2f;
    private float attackTimer;
    [SerializeField] private float attackSpeedIncrease = 1.1f;
    private float originalAttackTime;

    [SerializeField] private GameObject healthPickUp;

    [SerializeField] private float speedIncrease = 1.1f;

    [SerializeField] private Animator animator;

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
        //Vector3 targetPos = player.transform.position;

        Vector3 targetPos = new Vector3(player.transform.position.x,
            this.transform.position.y,
            player.transform.position.z);
        //this.transform.LookAt(targetPos);

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
                playerHealth = hitCollider.gameObject.GetComponent<PlayerHealth>();
                Attack();
                break;
            }
        }
    }

    void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            animator.SetTrigger("attack");

            float timer = 0.5f;

            if (GameManager.Instance.loopCounter != 0)
            {
                timer = 0.5f * Mathf.Pow(attackSpeedIncrease, GameManager.Instance.loopCounter);
            }

            Invoke("DamagePlayer", timer);
            attackTimer = attackTimerAmount;
        }
    }

    void DamagePlayer()
    {
        playerHealth.TakeDamage(damageAmount);
    }

    public virtual void DeActivate()
    {
        FindObjectOfType<ExitHandler>().EnemyKilled();

        DropHealth();

        base.Deactivate();
    }

    public void TakeDamage(int damage)
    {
        if (!isInVulnerable)
        {
            GameObject hitParticleClone = Instantiate(hitParticle, enemyObject.transform.position, enemyObject.transform.rotation);

            health -= damage;
            attackTimer = attackTimerAmount;

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

    void SetUp()
    {
        animator.SetBool("isMoving", true);

        pingPongSpeed = Random.Range(0.2f, 0.6f);

        attackTimer = attackTimerAmount;

        SetDifficulty();
    }

    void SetDifficulty()
    {
        int loopCount = GameManager.Instance.loopCounter;

        originalSpeed = agent.speed;

        if (loopCount == 0)
        {
            agent.speed = originalSpeed;
        }
        else
        {
            agent.speed = originalSpeed * Mathf.Pow(speedIncrease, loopCount);
        }

        originalAttackTime = attackTimerAmount;

        if (loopCount != 0)
        {
            attackTimerAmount = attackSpeedIncrease * Mathf.Pow(attackSpeedIncrease, loopCount);
        }
    }

    void DropHealth()
    {
        int dropChance = Random.Range(1, 11);

        if (dropChance == 5)
        {
            Vector3 dropPos = transform.position;
            dropPos.y = 2f;
            Instantiate(healthPickUp, dropPos, Quaternion.identity);
        }
    }
}
