using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Panel that appears when the session ends")]
    public GameObject resultsPanel;

    [Tooltip("Optional: Text to show final result (time, score, etc.)")]
    public TMP_Text resultText;

    [Header("Gameplay Elements")]
    [Tooltip("Reference to the SessionStartManager (for music and camera control)")]
    public SessionStartManager sessionStartManager;

    [Tooltip("Car GameObject to stop movement")]
    public GameObject car;

    [Tooltip("Script controlling car movement")]
    public MonoBehaviour carControllerScript;

    public TMP_Text statsLabelsText;
    public TMP_Text statsValuesText;

    private bool gameOverTriggered = false;

    public void OnGameOver()
    
    {
        Debug.Log("🟢 Entered GameOver.OnGameOver()");

        if (gameOverTriggered)
        {
            Debug.Log("⚠️ GameOver already triggered, skipping.");
            return;
        }

        gameOverTriggered = true;
        Debug.Log("✅ Game Over triggered");

        // Stop the music
        if (sessionStartManager != null && sessionStartManager.music != null)
        {
            sessionStartManager.music.Stop();
            Debug.Log("🛑 Music stopped via SessionStartManager");
        }

        // Disable camera movement
        CameraFollower cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
        {
            cam.DisableCameraFollow();
            Debug.Log("📷 Camera movement disabled via GameOver");
        }

        // Stop the car movement
        if (carControllerScript != null)
            carControllerScript.enabled = false;

        if (car != null)
        {
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero;
        }

        // Show the results panel
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            Debug.Log("📦 Results panel shown.");
        }

        // Show final session stats
        if (resultText != null && statsValuesText != null)
        {
            var stats = GameStatsManager.Instance;

            resultText.text =
                "<b>Time:</b>\n" +
                "<b>Avg Speed:</b>\n" +
                "<b>Max Speed:</b>\n" +
                "<b>Total XP:</b>\n" +
                "<b>XP/sec:</b>";

            float xpPerSecond = stats.totalSessionTime > 0 ? stats.GetXPPerSecond() : 0f;

            statsValuesText.text =
                $"{stats.totalSessionTime:F1} sec\n" +
                $"{stats.GetAverageSpeed():F2} km/h\n" +
                $"{stats.GetMaxSpeed():F2} km/h\n" +
                $"{stats.totalXP:F1} XP\n" +
                $"{xpPerSecond:F2}";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameStatsManager.Instance.ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void ExitGame()
    {
        Debug.Log("🚪 Exiting game...");
        Application.Quit();
    }
}
