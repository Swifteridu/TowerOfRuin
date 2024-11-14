using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseSettings : MonoBehaviour
{
    [Header("Button Settings")]

    [SerializeField] private Button optionsButton;

    [Header("Canvas Settings")]

    [SerializeField] private GameObject HideCanvas;
    [SerializeField] private GameObject ShowCanvas;
    void Start()
    {
        optionsButton.onClick.AddListener(showOptions);
    }
    void showOptions()
    {
        HideCanvas.SetActive(false);
        ShowCanvas.SetActive(true);
    }
}
