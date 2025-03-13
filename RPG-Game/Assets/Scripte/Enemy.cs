using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float patrolSpeed = 2f; // Langsamere Geschwindigkeit beim Patrouillieren
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
    [SerializeField] private int numberOfPatrolPoints = 3;
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private Transform[] patrolPoints;

    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Rigidbody enemyRigid;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform enemyTrans;

    private int currentPatrolIndex = 0;
    private float lastAttackTime;
    private float chaseRangeSqr;
    private float attackRangeSqr;

    // Verwende eine Coroutine für Start, um eine Verzögerung einzubauen
    private IEnumerator Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Warte 1 Sekunde, damit das NavMesh initialisiert und gebacken ist
        yield return new WaitForSeconds(4.0f);

        // Falls automatische Wegpunktgenerierung aktiviert ist oder keine Punkte gesetzt wurden
        if (autoGeneratePatrolPoints || patrolPoints == null || patrolPoints.Length == 0)
        {
            GeneratePatrolPoints();
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
            // Spieler in Reichweite: Normale Geschwindigkeit und Verfolgung
            MoveAndChasePlayer(distanceToPlayerSqr);
            HandleAttack(distanceToPlayerSqr);
        }
        else
        {
            // Spieler nicht in Reichweite: Patrouillieren mit langsamer Geschwindigkeit
            Patrol();
        }

        // Rotation anhand der aktuellen Bewegungsrichtung ausrichten
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

    // Automatische Generierung der Wegpunkte auf dem NavMesh mit Debug.Log-Meldungen
    private void GeneratePatrolPoints()
    {
        List<Transform> points = new List<Transform>();

        for (int i = 0; i < numberOfPatrolPoints; i++)
        {
            // Zufällige Richtung innerhalb eines Radius
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;

            // Finde eine gültige Position auf dem NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            {
                GameObject pointObject = new GameObject("PatrolPoint_" + i);
                pointObject.transform.position = hit.position;
                points.Add(pointObject.transform);
                Debug.Log("Gültiger Wegpunkt " + i + " gefunden an Position: " + hit.position);
            }
            else
            {
                Debug.Log("Kein gültiger Wegpunkt " + i + " gefunden bei der Position: " + randomDirection);
            }
        }

        patrolPoints = points.ToArray();

        if (patrolPoints.Length > 0)
        {
            // Setze das erste Ziel
            agent.SetDestination(patrolPoints[0].position);
        }
        else
        {
            Debug.LogWarning("Keine gültigen Wegpunkte wurden generiert!");
        }
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
            Vector3 moveDirection = (player.position - transform.position).normalized;
            enemyRigid.linearVelocity = moveDirection * walkSpeed;
            agent.SetDestination(player.position);
            enemyAnim.SetBool("walk", true);
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

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            StopMovement();
            return;
        }

        // Wechsel zum nächsten Wegpunkt, sobald der aktuelle fast erreicht wurde
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        enemyAnim.SetBool("walk", true);
    }

    // Rotation basierend auf der aktuellen Agentgeschwindigkeit
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
            //playerScript.TakeDamage(damage);
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
