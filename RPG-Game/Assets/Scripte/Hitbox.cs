using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemyScript = other.GetComponentInChildren<Enemy>();
        Debug.Log($"{other.name} ist in die Hitbox eingetreten!");

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(Player.AttackDammage); // Schaden wird ausgeteilt
            Debug.Log($"{other.gameObject.name} Take Damage");
        }
    }
}