using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
<<<<<<< Updated upstream
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float patrolSpeed = 2f; 
=======
    [SerializeField] private float walkSpeed = 3.5f;     // Geschwindigkeit beim Verfolgen
    [SerializeField] private float patrolSpeed = 2f;     // Geschwindigkeit beim Patrouillieren
>>>>>>> Stashed changes
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float chaseRange = 20f;
    [SerializeField] private float attackRange = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] public static int damage = 10;      // Schaden, den der Enemy verursacht

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 101;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("Patrol Settings")]
    [SerializeField] private bool autoGeneratePatrolPoints = true;
<<<<<<< Updated upstream
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float rectangleWidth = 10f;
    [SerializeField] private float rectangleHeight = 5f;
=======
    [SerializeField] private float rectangleWidth = 10f;   // Rechteck-Breite
    [SerializeField] private float rectangleHeight = 5f;   // Rechteck-Höhe
    [SerializeField] private Transform[] patrolPoints;     // Wird automatisch gefüllt, falls leer

    [Header("Animation Clips")]
    [Tooltip("Referenz auf das Death-Clip, damit wir die Länge auslesen können")]
    [SerializeField] private AnimationClip deathClip;
>>>>>>> Stashed changes

    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Rigidbody enemyRigid;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform enemyTrans;

    // Ping-Pong-Variablen für Patrouille: 0 → 1 → 2 → 3 → 2 → 1 → 0
    private int currentPatrolIndex = 0;
<<<<<<< Updated upstream
    private int patrolDirection = 1; 

=======
    private int patrolDirection = 1; // +1 vorwärts, -1 rückwärts

    // Attack- und Reichweiten-Variablen
>>>>>>> Stashed changes
    private float lastAttackTime;
    private float chaseRangeSqr;
    private float attackRangeSqr;

<<<<<<< Updated upstream
=======
    // Todes-Flag, damit nach dem Tod nichts mehr gemacht wird
    private bool isDead = false;

>>>>>>> Stashed changes
    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

<<<<<<< Updated upstream
        agent.stoppingDistance = 0.2f;

        if (autoGeneratePatrolPoints || patrolPoints == null || patrolPoints.Length == 0)
        {
            GenerateRectanglePatrolPoints();
        }

=======
        // HP setzen und Slider initialisieren
>>>>>>> Stashed changes
        currentHealth = maxHealth;
        InitializeHealthSlider();

        // Sqr-Werte für schnelleres Distance-Checking
        chaseRangeSqr = chaseRange * chaseRange;
        attackRangeSqr = attackRange * attackRange;

        // Falls keine Wegpunkte zugewiesen und autoGenerate aktiv: Erstelle 4 Rechteck-Wegpunkte
        if (autoGeneratePatrolPoints || patrolPoints == null || patrolPoints.Length == 0)
        {
            GenerateRectanglePatrolPoints();
        }

        // Für feineres Anhalten an Wegpunkten
        agent.stoppingDistance = 0.2f;
    }

    private void Update()
    {
        if (isDead) return;  // Keine Aktionen mehr, wenn tot

        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("Enemy: NavMeshAgent ist entweder null oder nicht auf einem NavMesh platziert.");
            return;
        }

        // Distanz (quadrat) zum Spieler
        float distanceToPlayerSqr = (player.position - transform.position).sqrMagnitude;

        if (distanceToPlayerSqr <= chaseRangeSqr)
        {
<<<<<<< Updated upstream
            agent.autoBraking = true;
=======
            // Spieler in Reichweite => Verfolgen
            agent.autoBraking = true;  // Beim Verfolgen kann AutoBraking nützlich sein
>>>>>>> Stashed changes
            MoveAndChasePlayer(distanceToPlayerSqr);
            HandleAttack(distanceToPlayerSqr);
        }
        else
        {
<<<<<<< Updated upstream
            agent.autoBraking = false;
            Patrol();
        }

=======
            // Spieler nicht in Reichweite => Patrouille
            agent.autoBraking = false; // Kontinuierliches Laufen
            Patrol();
        }

        // Gegner in Bewegungsrichtung ausrichten
>>>>>>> Stashed changes
        HandleRotation();

        // HP-Slider aktualisieren
        UpdateHealthSlider();
    }

    // Initialisiert den Health-Slider (falls zugewiesen)
    private void InitializeHealthSlider()
    {
        if (healthSlider == null)
        {
            // Falls du hier andere Pfade/Objektnamen hast, anpassen
            healthSlider = GameObject.Find("EnemyCanvas/EnemyLP").GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

<<<<<<< Updated upstream
    private Vector3 GetValidPoint(Vector3 point, float maxDistance = 2f)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return point; 
    }
=======
    // Erzeugt vier Wegpunkte in Form eines Rechtecks:
    //   0 = Startposition
    //   1 = rechts
    //   2 = rechts + vorne
    //   3 = vorne
    // Danach Ping-Pong: 0 → 1 → 2 → 3 → 2 → 1 → 0
>>>>>>> Stashed changes
    private void GenerateRectanglePatrolPoints()
    {
        List<Transform> points = new List<Transform>();

<<<<<<< Updated upstream
=======
        // Wegpunkt 0: Startposition
>>>>>>> Stashed changes
        GameObject p0 = new GameObject("PatrolPoint_0");
        p0.transform.position = GetValidPoint(transform.position);
        points.Add(p0.transform);

<<<<<<< Updated upstream
=======
        // Wegpunkt 1: rechts
>>>>>>> Stashed changes
        GameObject p1 = new GameObject("PatrolPoint_1");
        p1.transform.position = GetValidPoint(transform.position + transform.right * rectangleWidth);
        points.Add(p1.transform);

<<<<<<< Updated upstream
=======
        // Wegpunkt 2: rechts + vorne
>>>>>>> Stashed changes
        GameObject p2 = new GameObject("PatrolPoint_2");
        p2.transform.position = GetValidPoint(transform.position + transform.right * rectangleWidth + transform.forward * rectangleHeight);
        points.Add(p2.transform);

<<<<<<< Updated upstream
=======
        // Wegpunkt 3: vorne
>>>>>>> Stashed changes
        GameObject p3 = new GameObject("PatrolPoint_3");
        p3.transform.position = GetValidPoint(transform.position + transform.forward * rectangleHeight);
        points.Add(p3.transform);

        patrolPoints = points.ToArray();
    }

    // Versucht, einen gültigen Punkt auf dem NavMesh zu finden (max. 2m Abweichung)
    private Vector3 GetValidPoint(Vector3 point, float maxDistance = 2f)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return point;
    }

    // Verfolgung des Spielers
    private void MoveAndChasePlayer(float distanceToPlayerSqr)
    {
        // Animation: walk
        enemyAnim.SetBool("idle", false);
        enemyAnim.SetBool("walk", true);

        agent.speed = walkSpeed;

        // Nah genug => Stehen bleiben (StopMovement)
        if (distanceToPlayerSqr <= attackRangeSqr)
        {
            StopMovement();
        }
        else
        {
            agent.SetDestination(player.position);
<<<<<<< Updated upstream
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
=======
            // Rigidbody-Bewegung vermeiden, wenn der NavMeshAgent steuert
            enemyRigid.linearVelocity = Vector3.zero;
>>>>>>> Stashed changes
        }
    }

    // Patrouillen-Logik: Rechteck + Ping-Pong
    private void Patrol()
    {
        // Animation: walk
        enemyAnim.SetBool("idle", false);
        enemyAnim.SetBool("walk", true);

        agent.speed = patrolSpeed;

<<<<<<< Updated upstream
        if (patrolPoints == null || patrolPoints.Length != 4)
=======
        // Falls keine Punkte vorhanden
        if (patrolPoints == null || patrolPoints.Length < 4)
>>>>>>> Stashed changes
        {
            StopMovement();
            return;
        }

<<<<<<< Updated upstream
        if (!agent.hasPath || agent.remainingDistance < agent.stoppingDistance + 0.1f)
        {
=======
        // Nächsten Wegpunkt setzen, wenn wir nahe genug sind
        if (!agent.hasPath || agent.remainingDistance < agent.stoppingDistance + 0.1f)
        {
            // Richtungswechsel am Ende
>>>>>>> Stashed changes
            if (currentPatrolIndex == 0)
            {
                patrolDirection = 1;
            }
            else if (currentPatrolIndex == patrolPoints.Length - 1)
            {
                patrolDirection = -1;
            }
<<<<<<< Updated upstream
            currentPatrolIndex += patrolDirection;
            Vector3 targetPos = patrolPoints[currentPatrolIndex].position;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPos, path);
=======

            currentPatrolIndex += patrolDirection;

            // Ziel setzen, nur wenn Pfad komplett
            NavMeshPath path = new NavMeshPath();
            Vector3 targetPos = patrolPoints[currentPatrolIndex].position;
            agent.CalculatePath(targetPos, path);

>>>>>>> Stashed changes
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(targetPos);
            }
            else
            {
<<<<<<< Updated upstream
                Debug.LogWarning("Kein kompletter Pfad zum Wegpunkt " + currentPatrolIndex + " gefunden.");
=======
                Debug.LogWarning("Kein gültiger Pfad zu Wegpunkt " + currentPatrolIndex);
>>>>>>> Stashed changes
            }
        }
    }
<<<<<<< Updated upstream
=======

    // Gegner ausrichten anhand seiner Velocity
>>>>>>> Stashed changes
    private void HandleRotation()
    {
        Vector3 velocity = agent.velocity;
        if (velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            enemyTrans.rotation = Quaternion.Lerp(enemyTrans.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    // Angriff prüfen
    private void HandleAttack(float distanceToPlayerSqr)
    {
        // Nur angreifen, wenn wir cooldown-frei sind und in Reichweite
        if (Time.time >= lastAttackTime + attackCooldown && distanceToPlayerSqr <= attackRangeSqr)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void AttackPlayer()
    {
        // Animation: attack
        enemyAnim.Play("attack");

        // Hier kannst du Schaden am Spieler anrichten:
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
<<<<<<< Updated upstream
            enemyAnim.Play("attack");
=======
             playerScript.TakeDamage(damage);
>>>>>>> Stashed changes
        }
    }

    // Bewegung stoppen => idle
    private void StopMovement()
    {
        enemyRigid.linearVelocity = Vector3.zero;
        agent.ResetPath();

        enemyAnim.SetBool("walk", false);
        enemyAnim.SetBool("idle", true);
    }

    // Schaden nehmen
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        enemyAnim.Play("damage"); // Animation: damage

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            UpdateHealthSlider();
        }
    }

    // Sterben
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Bewegung und Agent stoppen
        agent.isStopped = true;
        enemyRigid.linearVelocity = Vector3.zero;

        // Death-Animation abspielen
        enemyAnim.Play("death");

        // Coroutine starten, die erst die Death-Animation abwartet, +1 Sek. Puffer
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // Warte die Länge der Death-Animation (falls kein Clip referenziert, fallback 2 Sek)
        float length = deathClip != null ? deathClip.length : 2f;
        yield return new WaitForSeconds(length + 1f);

        // Danach Enemy zerstören
        Destroy(gameObject);
    }

    private void UpdateHealthSlider()
    {
<<<<<<< Updated upstream
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
=======
        if (healthSlider != null)
>>>>>>> Stashed changes
        {
            healthSlider.value = currentHealth;
        }
    }
}
