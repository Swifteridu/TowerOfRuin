using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public SaveManager SaveManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        // Einstellungen f�r den Cursor je nach Szene
        if (sceneName == "MainMenu")
        {
            // Cursor im MainMenu sichtbar und frei beweglich
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (sceneName == "Game")
        {
            // Cursor in der Game-Szene unsichtbar und gesperrt
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void StartNewGame()
    {
        LoadScene("Game");
    }

    public void ReturnToMainMenu()
    {
        LoadScene("MainMenu");
    }
}
