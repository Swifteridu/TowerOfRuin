using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Disolve : MonoBehaviour
{
    [Range(0, 1)]
    public float value;
    public Renderer[] render;
    void Update()
    {
        foreach (Renderer r in render)
        {
            if(r != null)
            {
                Material temp_Mat = new Material(r.sharedMaterial);

                temp_Mat.SetFloat("_value", value);

                r.sharedMaterial = temp_Mat;
            }
        }
    }
}
