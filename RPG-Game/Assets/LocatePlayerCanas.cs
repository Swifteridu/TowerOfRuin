using UnityEngine;

public class LocatePlayerCanas : MonoBehaviour
{
    public Camera cam;
    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = cam.transform.eulerAngles;
    }
}
