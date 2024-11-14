using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float chaseRange = 20f;
    [SerializeField] private float attackRange = 2f;

    [Header("Attack Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 101;
    private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Rigidbody enemyRigid;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform enemyTrans;

    private float lastAttackTime;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        currentHealth = maxHealth;
        InitializeHealthSlider();
    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("Enemy: NavMeshAgent is either null or not placed on a NavMesh.");
            return;
        }

        // Überprüfen, ob der Spieler in Reichweite ist und entsprechend handeln
        if (Vector3.Distance(transform.position, player.position) <= chaseRange)
        {
            MoveAndChasePlayer();
            HandleRotation();
            HandleAttack();
        }
        else
        {
            StopMovement();
        }
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

    private void MoveAndChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // Überprüfen, ob der Abstand zum Spieler kleiner oder gleich 2f ist
        if (distanceToPlayer <= attackRange)
        {
            StopMovement(); // Gegner anhalten, wenn der Abstand kleiner oder gleich 1f ist
        }
        else
        {
            // Bewegungsrichtung berechnen und Spieler verfolgen
            Vector3 moveDirection = (player.position - transform.position).normalized;
            enemyRigid.linearVelocity = moveDirection * walkSpeed * Time.deltaTime;
            agent.SetDestination(player.position);
            enemyAnim.SetBool("walk", true);
        }
    }
    }


    private void HandleRotation()
    {
        // Drehung in Richtung des Spielers
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemyTrans.rotation = Quaternion.Lerp(enemyTrans.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void HandleAttack()
    {
        // Wenn der Spieler in Reichweite ist und der Angriff cooldown abgelaufen ist
        if (Time.time >= lastAttackTime + attackCooldown && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void StopMovement()
    {
        // Stoppe Bewegung und Animation, wenn der Spieler außerhalb der Reichweite ist
        enemyRigid.linearVelocity = Vector3.zero;
        enemyAnim.SetBool("walk", false);
        enemyAnim.SetBool("idle", true);
    }

    private void AttackPlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            enemyAnim.SetTrigger("attack");
            playerScript.TakeDamage(damage);
        }
    }

    private void UpdateHealthSlider()
    {
        // Aktualisiere den Health-Slider mit dem aktuellen Leben des Gegners
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
    }

    private void Die()
    {
        enemyAnim.SetTrigger("die");
        // Gegner wird nach dem Tod zerstört
        Destroy(gameObject);
    }
}
