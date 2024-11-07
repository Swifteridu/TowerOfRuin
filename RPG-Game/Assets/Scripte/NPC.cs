using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator NPCAnim;
    public Rigidbody NPCRigid;
    public Transform Player; // Referenz auf den Spieler
    public float chaseRange = 10f; // Reichweite, in der der NPC den Spieler verfolgen soll
    public float rotationSpeed = 5f; // Geschwindigkeit der Drehung

    void Update()
    {
        // Stelle sicher, dass die "idle" Animation abgespielt wird
        NPCAnim.SetBool("idle", true);

        if (Player != null)
        {
            // Berechne die Richtung zum Spieler
            Vector3 directionToPlayer = Player.position - transform.position;
            directionToPlayer.y = 0f; // Nur in der horizontalen Ebene drehen

            // Überprüfe, ob der Spieler in Reichweite ist
            if (directionToPlayer.magnitude <= chaseRange)
            {
                // Spieler in Reichweite, spiele "idle" Animation ab und drehe den NPC zum Spieler
                NPCAnim.SetBool("idle", true);
                RotateTowardsPlayer(directionToPlayer);
            }
            else
            {
                // Spieler außerhalb der Reichweite, spiele "idle" Animation ab
                NPCAnim.SetBool("idle", true);
            }
        }
    }

    void RotateTowardsPlayer(Vector3 directionToPlayer)
    {
        if (directionToPlayer != Vector3.zero)
        {
            // Berechne die Zielausrichtung des Enemies zum Spieler
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Drehen Sie den NPC sanft in Richtung des Spielers
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
