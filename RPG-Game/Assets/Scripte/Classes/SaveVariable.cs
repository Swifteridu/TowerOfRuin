using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum valueTypes{ Bool, Float, Int, String}
public class SaveVariable : SaveObject
{
    public valueTypes valueType;
    [SerializeField]private string value = "false";

    public void SetBool(bool vaue)
    {
        valueType = valueTypes.Bool;
        value = (vaue == true)+"";
    }
    public bool GetBool()
    {
        return false;
    }


    public void SetFloat(float vaue)
    {
        valueType = valueTypes.Float;
    }
    public float GetFloat()
    {
        return 0;
    }


    public void SetInt(int vaue)
    {
        valueType = valueTypes.Int;
    }
    public int GetInt()
    {
        return 0;
    }


    public void SetString(string vaue)
    {
        valueType = valueTypes.String;
    }
    public string GetString()
    {
        return value;
    }
}
