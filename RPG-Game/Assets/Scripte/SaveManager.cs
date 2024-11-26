using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [HideInInspector]public static SaveManager saveManager;

    public SaveObject saveObjects;
    

    void Start()
    {
        if (saveManager == null)
        {
            saveManager = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
        if (PlayerPrefs.GetString("save", "") != "")
        {
            print(PlayerPrefs.GetString("save", ""));
            Load(PlayerPrefs.GetString("save", ""));
            PlayerPrefs.DeleteKey("save");
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F1))
        {
            Save("Test");
        }
    }

    public void Save(string savename)
    {
        if(!Directory.Exists(Application.dataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.dataPath + "/saves");
        }
        saveObjects.enemy.Clear();
        foreach (var item in FindObjectsOfType<Enemy>())
        {
            saveObjects.enemy.Add(new SaveObject.SaveEnemys(item.id, item.transform.position, item.currentHealth));
        }
        if(FindFirstObjectByType<Player>() != null)
        {
            saveObjects.playerposition = FindFirstObjectByType<Player>().transform.position;
            saveObjects.playerLP = FindFirstObjectByType<Player>().currentHealth;
        }
        string j = JsonUtility.ToJson(saveObjects);
        File.WriteAllText(Application.dataPath + "/saves/" + savename + ".save", j);
    }
    public void Load(string savename)
    {
        if (Directory.Exists(Application.dataPath + "/saves"))
        {
            if (File.Exists(Application.dataPath + "/saves/" + savename + ".save"))
            {
                saveObjects = JsonUtility.FromJson<SaveObject>(File.
                                        ReadAllText(Application.dataPath + "/saves/" + savename + ".save"));
                FindFirstObjectByType<Player>().transform.position = saveObjects.playerposition;
                FindFirstObjectByType<Player>().currentHealth = saveObjects.playerLP;

                foreach(var item in FindObjectsOfType<Enemy>())
                {
                    Destroy(item.gameObject);
                }
                foreach (var item in saveObjects.enemy)
                {
                    GameObject goj = Instantiate(Resources.Load<GameObject>("Enemy"), item.enemyposition, Quaternion.identity); 
                    goj.GetComponent<Enemy>().id = item.id;
                    goj.GetComponent<Enemy>().currentHealth = item.enemyLP;
                }
            }
        }
    }
    private void OnApplicationQuit()
    {
        Save(PlayerPrefs.GetString("save", ""));
    }
}
