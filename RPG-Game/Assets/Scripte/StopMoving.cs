using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.LowLevel;

public class StopMoving : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player p;
    public void AttackStop(){
        p.ChangeStateStopMovement();
        Debug.Log("Ausgeführt");
    }
}