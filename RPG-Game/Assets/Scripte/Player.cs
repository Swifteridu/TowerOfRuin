using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    private float runSpeed = 6f;
    [SerializeField] private float dashSpeed = 12f;
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
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    private const float attackRange = 2f;
    public const int attackDamage = 20;
    public static int AttackDamage { get { return attackDamage; } }

    [Header("References")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Rigidbody playerRigid;
    [SerializeField] private Transform playerTrans;

    private bool stopMoving;
    private bool DashOnCooldown = false;
    private bool isLockedOn = false;
    public bool isBlocking = false;
    private bool isDashing = false;
    private bool doingSomething = false;
    private float horizontal;
    private float vertical;
    private float speed;
    private void LockOnTarget()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isLockedOn = !isLockedOn;
        }

        if (isLockedOn)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector3 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToEnemy.x, 0, directionToEnemy.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPosition);
            if (distance < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = distance;
            }
        }
        return nearestEnemy;
    }

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
        print(damage);
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth;
    }

    private void Die()
    {
        playerAnim.SetTrigger("Die");
        Invoke(nameof(NotifyGameManagerPlayerDied), 0.5f);
    }
    private IEnumerator UnBlock()
    {
        
        doingSomething = true;
        playerAnim.Play("unblock");
        yield return new WaitForSeconds(0.1f);
        isBlocking = false;
        doingSomething = false;
        stopMoving = false;
    }
    private IEnumerator Block()
    {
        stopMoving = true;
        doingSomething = true;
        playerAnim.Play("block");
        yield return new WaitForSeconds(0.1f);
        isBlocking = true;
        doingSomething = false;
    }

    private void NotifyGameManagerPlayerDied()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadScene("MainMenu");
        }
    }

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    public IEnumerator Dash()
    {
        doingSomething = true;
        float normalSpeed = walkSpeed;
        isDashing = true;
        DashOnCooldown = true;
        playerAnim.Play("dash");
        walkSpeed = dashSpeed;


        yield return new WaitForSeconds(0.3f);

        walkSpeed = normalSpeed;
        isDashing = false;

        doingSomething = false;
        yield return new WaitForSeconds(0.75f);
        DashOnCooldown = false;

    }

    private void Update()
    {
        if (!stopMoving)
        {
            
            HandleMovement();
        }
        else if (stopMoving)
        {
            playerAnim.SetFloat("x", 0);
            playerAnim.SetFloat("y", 0);
            playerRigid.linearVelocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !DashOnCooldown && !doingSomething)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!doingSomething && !isBlocking)
            {
                StartCoroutine(Block());
            }
            else
            {
                StartCoroutine(UnBlock());
            }
        }
        StartCoroutine(HandleAttack());
        LockOnTarget();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NotifyGameManagerPlayerDied();
        }
        if (!stopMoving)
        {
            HandleMovement();
        }
        if (Input.GetKeyDown(KeyCode.V) && !DashOnCooldown)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isBlocking)
            {
                Block();
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isBlocking = false;
            playerAnim.SetTrigger("isBlocking");
        }
    }

    private void HandleMovement()
    {
        if (isDashing)
        {
            Vector3 moveDir = transform.forward;
            playerRigid.linearVelocity = moveDir.normalized * dashSpeed;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

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
            speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            playerRigid.linearVelocity = moveDir.normalized * speed;
        }
        else
        {
            playerRigid.linearVelocity = Vector3.zero;
        }
    }


    private IEnumerator HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown + 0.1f && !doingSomething)
        {
<<<<<<< Updated upstream
            doingSomething = true;
            stopMoving = true;
=======
            stopMoving = false;
>>>>>>> Stashed changes
            playerAnim.Play("attack", -1, 0f);
            lastAttackTime = Time.time;
            yield return new WaitForSeconds(attackCooldown - 0.7f);
            stopMoving = false;
            doingSomething = false;
        }
    }
}