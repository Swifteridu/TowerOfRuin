using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [HideInInspector]public static SaveManager saveManager;

    public List<SaveObject> saveObjects = new List<SaveObject>();

    void Start()
    {
        if (saveManager == null)
        {
            saveManager = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    public static void NewGame()
    {

    }
    public static void Game()
    {

    }
    public static void Save()
    {

    }
    public static void Load()
    {
        
    }
}
