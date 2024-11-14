using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button startButton;

    private void OnEnable()
    {
        // Button-Listener registrieren
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    private void OnDisable()
    {
        // Button-Listener entfernen, um Duplikate zu vermeiden
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.StartNewGame();
    }
}
