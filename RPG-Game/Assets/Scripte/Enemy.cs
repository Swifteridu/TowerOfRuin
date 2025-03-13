using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float patrolSpeed = 2f; 
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float chaseRange = 20f;
    [SerializeField] private float attackRange = 2f;

    [Header("Attack Settings")]
    [SerializeField] public static int damage = 10;
    [SerializeField] private float attackCooldown;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 101;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("Patrol Settings")]
    [SerializeField] private bool autoGeneratePatrolPoints = true;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float rectangleWidth = 10f;
    [SerializeField] private float rectangleHeight = 5f;

    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Rigidbody enemyRigid;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform enemyTrans;

    private int currentPatrolIndex = 0;
    private int patrolDirection = 1; 

    private float lastAttackTime;
    private float chaseRangeSqr;
    private float attackRangeSqr;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.stoppingDistance = 0.2f;

        if (autoGeneratePatrolPoints || patrolPoints == null || patrolPoints.Length == 0)
        {
            GenerateRectanglePatrolPoints();
        }

        currentHealth = maxHealth;
        InitializeHealthSlider();

        chaseRangeSqr = chaseRange * chaseRange;
        attackRangeSqr = attackRange * attackRange;
    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("Enemy: NavMeshAgent ist entweder null oder nicht auf einem NavMesh platziert.");
            return;
        }

        float distanceToPlayerSqr = (player.position - transform.position).sqrMagnitude;

        if (distanceToPlayerSqr <= chaseRangeSqr)
        {
            agent.autoBraking = true;
            MoveAndChasePlayer(distanceToPlayerSqr);
            HandleAttack(distanceToPlayerSqr);
        }
        else
        {
            agent.autoBraking = false;
            Patrol();
        }

        HandleRotation();

        UpdateHealthSlider();
    }

    private void InitializeHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("EnemyCanvas/EnemyLP").GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private Vector3 GetValidPoint(Vector3 point, float maxDistance = 2f)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return point; 
    }
    private void GenerateRectanglePatrolPoints()
    {
        List<Transform> points = new List<Transform>();

        GameObject p0 = new GameObject("PatrolPoint_0");
        p0.transform.position = GetValidPoint(transform.position);
        points.Add(p0.transform);

        GameObject p1 = new GameObject("PatrolPoint_1");
        p1.transform.position = GetValidPoint(transform.position + transform.right * rectangleWidth);
        points.Add(p1.transform);

        GameObject p2 = new GameObject("PatrolPoint_2");
        p2.transform.position = GetValidPoint(transform.position + transform.right * rectangleWidth + transform.forward * rectangleHeight);
        points.Add(p2.transform);

        GameObject p3 = new GameObject("PatrolPoint_3");
        p3.transform.position = GetValidPoint(transform.position + transform.forward * rectangleHeight);
        points.Add(p3.transform);

        patrolPoints = points.ToArray();
    }

    private void MoveAndChasePlayer(float distanceToPlayerSqr)
    {
        agent.speed = walkSpeed;

        if (distanceToPlayerSqr <= attackRangeSqr)
        {
            StopMovement();
        }
        else
        {
            agent.SetDestination(player.position);
            enemyAnim.SetBool("walk", true);
            enemyRigid.linearVelocity = Vector3.zero;
        }
    }

    private void HandleAttack(float distanceToPlayerSqr)
    {
        if (Time.time >= lastAttackTime + attackCooldown && distanceToPlayerSqr <= attackRangeSqr)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (patrolPoints == null || patrolPoints.Length != 4)
        {
            StopMovement();
            return;
        }

        if (!agent.hasPath || agent.remainingDistance < agent.stoppingDistance + 0.1f)
        {
            if (currentPatrolIndex == 0)
            {
                patrolDirection = 1;
            }
            else if (currentPatrolIndex == patrolPoints.Length - 1)
            {
                patrolDirection = -1;
            }
            currentPatrolIndex += patrolDirection;
            Vector3 targetPos = patrolPoints[currentPatrolIndex].position;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPos, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(targetPos);
            }
            else
            {
                Debug.LogWarning("Kein kompletter Pfad zum Wegpunkt " + currentPatrolIndex + " gefunden.");
            }
        }
        enemyAnim.SetBool("walk", true);
    }
    private void HandleRotation()
    {
        Vector3 velocity = agent.velocity;
        if (velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            enemyTrans.rotation = Quaternion.Lerp(enemyTrans.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void StopMovement()
    {
        enemyRigid.linearVelocity = Vector3.zero;
        agent.ResetPath();
        enemyAnim.SetBool("walk", false);
        enemyAnim.SetBool("idle", true);
    }

    private void AttackPlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            enemyAnim.Play("attack");
        }
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            healthSlider.value = 0;
            currentHealth = 0;
            Die();
        }
        UpdateHealthSlider();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
