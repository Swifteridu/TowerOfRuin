using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{

    [Header("Canvas Settings")]

    [SerializeField] private GameObject Canvas;
    public void showInGame()
    {
        if(!Canvas.activeSelf)
        {
            Canvas.SetActive(true);
        }
        else if(Canvas.activeSelf)
        {
            Canvas.SetActive(false);
        }
    }
}
