using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 100f;
    [SerializeField] private float backwardSpeed = 60f;
    [SerializeField] private float idleWalkSpeed = 50f;
    [SerializeField] private float runSpeed = 150f;
    [SerializeField] private float rotationSpeed = 100f;
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

    private bool isWalking;
    private float currentSpeed;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentSpeed = walkSpeed;
        currentHealth = maxHealth;

        // Initialize health slider if not assigned
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
        HandleMovementInput();
        HandleRunning();
        HandleAttack();
    }

    private void HandleMovement()
    {
       float horizontal = Input.GetAxisRaw("Horizontal");
       float vertical = Input.GetAxisRaw("Vertical");
       Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

       if (direction.magnitude >= 0.1f)
       {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        playerRigid.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        playerRigid.transform.position += moveDir.normalized * 6f * Time.deltaTime;
       }
    }

    
    private void HandleMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetWalkingAnimation(true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            SetWalkingAnimation(false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetRunningBackAnimation(true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            SetRunningBackAnimation(false);
        }
        
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        this.isWalking = isWalking;
        //playerAnim.SetTrigger(isWalking ? "Walk" : "Idle");
    }

    private void SetRunningBackAnimation(bool isRunningBack)
    {
        this.isWalking = isRunningBack;
        //playerAnim.SetTrigger(isRunningBack ? "RunBack" : "Idle");
    }

    private void HandleRunning()
    {
        if (isWalking && Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed + runSpeed;
            //playerAnim.SetTrigger("Run");
            //playerAnim.ResetTrigger("Walk");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = idleWalkSpeed;
            //playerAnim.ResetTrigger("Run");
            //playerAnim.SetTrigger("Walk");
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
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