using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    private const float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public Transform cam;
    private float animationMovementX;
    private float animationMovementY;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private const float attackRange = 2f;
    
    private const int attackDamage = 20;
    public static int AttackDammage {  get; }


    [Header("References")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Rigidbody playerRigid;
    [SerializeField] private Transform playerTrans;

    private bool stopMoving;

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
        Invoke(nameof(NotifyGameManagerPlayerDied), 0.5f);
    }

    private void NotifyGameManagerPlayerDied()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.LoadScene("MainMenu");
        }
        
    }

    private void FixedUpdate()
    {
        if (!stopMoving)
        {
            HandleMovement();
        }
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        animationMovementX = Vector3.MoveTowards(Vector3.one * animationMovementX, Vector3.one * horizontal, Time.deltaTime * 10).x;
        animationMovementY = Vector3.MoveTowards(Vector3.one * animationMovementY, Vector3.one * vertical, Time.deltaTime * 10).x;
        playerAnim.SetFloat("x", animationMovementX);
        playerAnim.SetFloat("y", animationMovementY);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            playerRigid.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            playerRigid.transform.position += moveDir.normalized * speed * Time.deltaTime;
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            stopMoving = true;
            //Attack();
            playerAnim.Play("attack", -1, 0f);
            lastAttackTime = Time.time;
        }
    }
    public void ChangeStateStopMovement()
    {
        stopMoving = false;
    }

    private void Attack()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                enemyScript?.TakeDamage(attackDamage);
            }
        }
    }
}
