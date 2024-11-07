using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator enemyAnim;
    public Rigidbody enemyRigid;
    public Transform player; // Referenz auf den Spieler
    public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
    public float chaseRange = 10f; // Reichweite, in der der Enemy den Spieler verfolgen soll
    public Transform enemyTrans;
    public NavMeshAgent agent;


    void FixedUpdate()
    {
        // Bewegungsrichtung berechnen
        Vector3 moveDirection = Vector3.zero;

        if (Vector3.Distance(transform.position, player.position) <= chaseRange)
        {
            // Wenn der Spieler innerhalb der Reichweite ist, bewege den Enemy in Richtung des Spielers
           /* 
              moveDirection = (player.position - transform.position).normalized;
              enemyRigid.velocity = moveDirection * w_speed * Time.deltaTime;
           */
           agent.SetDestination(player.position);


            // Drehung des Enemies in Richtung des Spielers
            
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //enemyTrans.rotation = Quaternion.Lerp(enemyTrans.rotation, targetRotation, ro_speed * Time.deltaTime);
            

            // Wenn der Enemy zum Spieler schaut, spiele die "walk" Animation ab
            if (Quaternion.Angle(enemyTrans.rotation, targetRotation) < 5f)
            {
                enemyAnim.SetBool("walk", false);
            }
            else
            {
                enemyAnim.SetBool("walk", true);
            }
        }
        else
        {
            // Wenn der Spieler au�erhalb der Reichweite ist, halte den Enemy an
            enemyRigid.linearVelocity = Vector3.zero;
            enemyAnim.SetBool("walk", false); // Stelle sicher, dass die "walk" Animation gestoppt wird, wenn der Spieler au�er Reichweite ist
            enemyAnim.SetBool("idle", true); // Spiele die "idle" Animation ab, wenn der Spieler au�er Reichweite ist
        }
    }

    void Update()
    {
  
    }
}