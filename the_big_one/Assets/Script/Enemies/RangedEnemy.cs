using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemies
{

    [SerializeField] private NavMeshAgent agent;

    private GameObject player;

    [SerializeField] private int health = 10;
    [SerializeField] private float inVulnerabilityTime = 0.2f;
    private bool isInVulnerable = false;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject hitParticle;

    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float targetThreshold = 0.1f;

    private Vector3 currentTarget;
    private Vector3 previousTarget;

    private float moveTimer;
    [SerializeField] private float moveTimerAmount = 5f;
    private bool isMoving = true;

    private float shootTimer;
    [SerializeField] private float shootTimerAmount = 1.5f;

    [SerializeField] private float evadeSphereRadius = 5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float evadeTimerAmount = 3;
    private float evadeTimer;

    [SerializeField] private Animator animator;

    void Start()
    {
        player = GameObject.Find("Player");
        SetUp();
    }


    void Update()
    {
        if (Vector3.Distance(transform.position, currentTarget) < targetThreshold && isMoving)
        {
            isMoving = false;
            moveTimer = moveTimerAmount;
            animator.SetTrigger("idle");
        }

        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            isMoving = true;
            moveTimer = moveTimerAmount;
            GetNewDestination();
        }

        if (!isMoving)
        {
            Shoot();
        }

        EvadePlayer();

        if (isMoving)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void GetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
        randomDirection += transform.position;

        NavMeshHit hit;

        bool destinationOK = false;

        for (int it = 0; it < 10; it++)
        {
            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, 1))
            {
                previousTarget = currentTarget;
                currentTarget = hit.position;
                agent.SetDestination(currentTarget);
                destinationOK = true;
                break;
            }
            else
            {
                randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
                randomDirection += transform.position;
            }
        }

        if (!destinationOK)
        {
            Vector3 temporarySave = currentTarget;
            currentTarget = previousTarget;
            previousTarget = currentTarget;
            agent.SetDestination(currentTarget);
        }
    }

    private void Shoot()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x, 
                                        this.transform.position.y,
                                        player.transform.position.z);
        this.transform.LookAt(targetPos);

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            animator.SetTrigger("attack");
            Invoke("InstantiateMissile", 0.9f);
            shootTimer = shootTimerAmount;
        }
    }

    void InstantiateMissile()
    {
        GameObject projectileClone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("You hit me");
        if (!isInVulnerable)
        {
            GameObject hitParticleClone = Instantiate(hitParticle, transform.position, transform.rotation);

            health -= damage;
            shootTimer = shootTimerAmount;
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

    void EvadePlayer()
    {
        Vector3 enemyOrigin = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(enemyOrigin, evadeSphereRadius, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player"))
            {
                evadeTimer -= Time.deltaTime;

                if (evadeTimer <= 0)
                {
                    Debug.Log("Player too close to me, evade");
                    moveTimer = 0f;

                    evadeTimer = evadeTimerAmount;
                }
            }
        }
    }

    void InvulnerabilityOff()
    {
        isInVulnerable = false;
    }

    public virtual void DeActivate()
    {
        FindObjectOfType<ExitHandler>().EnemyKilled();
        base.Deactivate();
    }

    void SetUp()
    {
        previousTarget = transform.position;
        shootTimer = shootTimerAmount;
        moveTimer = moveTimerAmount;
        evadeTimer = evadeTimerAmount;

        GetNewDestination();
    }
}
