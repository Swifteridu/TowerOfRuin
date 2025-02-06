using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour
{
    public bool isAttacking = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isAttacking)
        {
            return;
        }

        Enemy enemyScript = other.GetComponentInParent<Enemy>();
        Player playerScript = other.GetComponentInParent<Player>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(Player.AttackDamage);
        }
        if (playerScript != null)
        {
            playerScript.TakeDamage(Enemy.damage);
        }
    }
}
