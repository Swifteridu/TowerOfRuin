using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button startButton;
    [SerializeField] private string Scene;
    void Start()
    {
        // Button-Event-Listener hinzufügen
        startButton.onClick.AddListener(LoadGameSceneOnClick);
    }
    void LoadGameSceneOnClick()
    {
        // Szene mit dem Namen "Game" laden
        SceneManager.LoadScene(Scene);
    }
}
