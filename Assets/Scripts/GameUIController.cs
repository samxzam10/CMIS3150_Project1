using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Interacts with TextMeshPro components

// Manages all HUD updates, victory/loss UI overlays, and button navigation
public class GameUIController : MonoBehaviour
{
    // --- INSPECTOR FIELDS ---

    [Header("HUD Text Elements")]
    // Reference to TextMeshPro UI element displaying coins collected
    [SerializeField] private TextMeshProUGUI coinText;
    // Reference to TextMeshPro UI element displaying remaining lives
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("UI Panels")]
    // Reference to Game Over overlay panel
    [SerializeField] private GameObject gameOverPanel;
    // Reference to Victory/Win overlay panel
    [SerializeField] private GameObject winPanel;

    // Flags preventing game state methods from running multiple times
    private bool isGameEnded = false;

    private void Start()
    {
        // Hide overlay panels on scene start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        
        // Ensure normal frame time playback
        Time.timeScale = 1f;
    }

    // --- HUD UPDATES ---

    public void UpdateCoinText(int currentCoins, int totalCoins)
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + currentCoins + " / " + totalCoins;
        }
    }

    public void UpdateLivesText(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    // --- GAME OVER & WIN TRIGGERS ---

    public void TriggerGameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("Game Over!");
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        
        PauseGameAndUnlockCursor();
    }

    public void TriggerWin()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("You Win!");
        if (winPanel != null) winPanel.SetActive(true);
        
        PauseGameAndUnlockCursor();
    }

    private void PauseGameAndUnlockCursor()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- BUTTON ACTIONS ---

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}