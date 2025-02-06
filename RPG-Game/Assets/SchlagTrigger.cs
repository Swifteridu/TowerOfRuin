using UnityEngine;

public class SchlagTrigger : MonoBehaviour
{

    public Hitbox hitbox;
    public void StartSchlagen()
    {
        hitbox.isAttacking = true;
    }
    public void StopSchlagen()
    {
        hitbox.isAttacking = false;
    }
}
