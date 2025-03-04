using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnection : MonoBehaviour
{
    public int lenth = 0;
    public List<GameObject> nextRoom = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lenth > 0 && nextRoom.Count > 0)
        {
            GameObject g = Instantiate(nextRoom[Random.Range(0, nextRoom.Count)], transform.position, transform.rotation);
            g.GetComponent<RoomConnection>().lenth = lenth - 1;
        }
        //Destroy(gameObject);
    }
}
