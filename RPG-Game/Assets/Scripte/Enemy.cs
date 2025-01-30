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
    [SerializeField] private float attackCooldown;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 101;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Rigidbody enemyRigid;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform enemyTrans;

    private float lastAttackTime;
    private float chaseRangeSqr;
    private float attackRangeSqr;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
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
            Debug.LogWarning("Enemy: NavMeshAgent is either null or not placed on a NavMesh.");
            return;
        }

        float distanceToPlayerSqr = (player.position - transform.position).sqrMagnitude;

        if (distanceToPlayerSqr <= chaseRangeSqr)
        {
            MoveAndChasePlayer(distanceToPlayerSqr);
            HandleRotation();
            HandleAttack(distanceToPlayerSqr);
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

    private void MoveAndChasePlayer(float distanceToPlayerSqr)
    {
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

    private void HandleRotation()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemyTrans.rotation = Quaternion.Lerp(enemyTrans.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void HandleAttack(float distanceToPlayerSqr)
    {
        if (Time.time >= lastAttackTime + attackCooldown && distanceToPlayerSqr <= attackRangeSqr)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void StopMovement()
    {
        enemyRigid.linearVelocity = Vector3.zero;
        enemyAnim.SetBool("walk", false);
        enemyAnim.SetBool("idle", true);
    }

    private void AttackPlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            enemyAnim.Play("attack");
            playerScript.TakeDamage(damage);
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
    }

    private void Die()
    {
        enemyAnim.SetTrigger("die");
        Destroy(gameObject);
    }
}
