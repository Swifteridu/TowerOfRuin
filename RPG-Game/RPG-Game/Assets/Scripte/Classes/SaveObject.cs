using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{

    public Vector3 playerposition;
    public int playerLP;
    public List<SaveEnemys> enemy = new List<SaveEnemys>();

    public SaveObject() { 
    
    }
    [System.Serializable]
    public class SaveEnemys
    {
        public string id;
        public Vector3 enemyposition;
        public int enemyLP;

        public SaveEnemys(string _id, Vector3 _position, int _leben)
        {
            id = _id;
            enemyposition = _position;
            enemyLP = _leben;
        }
    }
}
