using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float w_speed = 20f, wb_speed = 6f, olw_speed = 5f, rn_speed = 10f, ro_speed = 100f;
    public bool walking;
    public Transform playerTrans;
    private float moveSpeed;

    void Start()
    {
        moveSpeed = w_speed; // Initialize with walking speed
    }

    void FixedUpdate()
    {
        // Move forward
        if (Input.GetKey(KeyCode.W))
        {
            playerRigid.linearVelocity = transform.forward * moveSpeed * Time.deltaTime;
        }
        // Move backward
        else if (Input.GetKey(KeyCode.S))
        {
            playerRigid.linearVelocity = -transform.forward * wb_speed * Time.deltaTime;
        }
        else
        {
            playerRigid.linearVelocity = Vector3.zero; // Stop movement when no keys are pressed
        }
    }

    void Update()
    {
        // Handle Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnim.SetTrigger("Jump");
            playerRigid.linearVelocity = new Vector3(playerRigid.linearVelocity.x, 5f, playerRigid.linearVelocity.z); // Add jump force
            walking = true;
        }

        // Handle Walk
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("walk");
            walking = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
            walking = false;
        }

        // Handle Run Backwards
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("runback");
            walking = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("runback");
            playerAnim.SetTrigger("idle");
            walking = false;
        }

        // Rotate Left and Right
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }

        // Handle Running (when walking is true and LeftShift is held)
        if (walking)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed = w_speed + rn_speed;
                playerAnim.SetTrigger("running");
                playerAnim.ResetTrigger("walk");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed = olw_speed;
                playerAnim.ResetTrigger("running");
                playerAnim.SetTrigger("walk");
            }
        }
    }
}
