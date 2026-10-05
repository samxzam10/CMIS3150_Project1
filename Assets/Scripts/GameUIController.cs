using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    private bool isGameEnded = false;

    private void Start()
    {
        // Ensure panels are hidden when the game starts
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        
        // Reset time scale in case it was paused previously
        Time.timeScale = 1f;
    }

    public void TriggerGameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        gameOverPanel.SetActive(true);
        PauseGame();
    }

    public void TriggerWin()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        winPanel.SetActive(true);
        PauseGame();
    }

    private void PauseGame()
    {
        // Freezes physics and time-dependent actions
        Time.timeScale = 0f; 
        
        // Unlock cursor if using a 3D/FPS camera
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Button Actions

    public void RestartGame()
    {
        Time.timeScale = 1f; // Always restore time scale before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}