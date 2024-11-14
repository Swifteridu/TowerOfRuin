using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameQuit : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button quitButton;
    void Start()
    {
        // Button-Event-Listener hinzufügen
        quitButton.onClick.AddListener(QuitApplication);
    
    }
    void QuitApplication()
    {
            EditorApplication.ExitPlaymode();
            Application.Quit();
    }
}
