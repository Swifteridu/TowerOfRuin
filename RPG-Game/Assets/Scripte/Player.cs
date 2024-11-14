using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public Transform cam;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private const float attackRange = 2f;
    private const int attackDamage = 20;

    [Header("References")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Rigidbody playerRigid;
    [SerializeField] private Transform playerTrans;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentHealth = maxHealth;

        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("PlayerCanvas/LP").GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        playerAnim.SetTrigger("Die");
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        playerAnim.SetFloat("x", horizontal);
        playerAnim.SetFloat("y", vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            playerRigid.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift))
        {
            playerRigid.transform.position += moveDir.normalized * runSpeed * Time.deltaTime;
        } else playerRigid.transform.position += moveDir.normalized * walkSpeed * Time.deltaTime;
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            playerAnim.Play("attack", -1, 0f);
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(attackDamage); // Schaden wird ausgeteilt
                    //playerAnim.SetTrigger("Attack"); 
                }
            }
        }
    }
}