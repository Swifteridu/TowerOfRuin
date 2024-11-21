using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        PlayerPrefs.SetString("save", "Test");   //Später mit auswahl (SetString("save", "Test"))
        SceneManager.LoadScene("Game");
        print("asdfg");
    }
}
