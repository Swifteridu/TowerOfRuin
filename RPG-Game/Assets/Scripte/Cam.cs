using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
        
    public Transform Player, cameraTrans;


    void Update()
    {
        cameraTrans.LookAt(Player);
    }
}