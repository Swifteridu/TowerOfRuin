using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    bool LockedOn = false;
        
    public Transform Player, cameraTrans;


    void Update()
    {
        cameraTrans.LookAt(Player);
    }
}