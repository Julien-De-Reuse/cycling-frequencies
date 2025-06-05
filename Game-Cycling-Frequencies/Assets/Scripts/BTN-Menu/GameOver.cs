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

    public void OnGameOver()
    {
        Debug.Log("✅ Game Over triggered");

        // Stop the music
        if (sessionStartManager != null && sessionStartManager.music != null)
        {
            sessionStartManager.music.Stop();
            Debug.Log("🛑 Music stopped via SessionStartManager");
        }
        else
        {
            Debug.LogWarning("❌ SessionStartManager or music reference missing in GameOver.cs");
        }

        // Disable camera movement
        CameraFollower cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
        {
            cam.DisableCameraFollow();
            Debug.Log("📷 Camera movement disabled via GameOver");
        }
        else
        {
            Debug.LogWarning("❌ CameraFollower not found");
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
        else
        {
            Debug.LogWarning("❌ ResultsPanel is not assigned in the inspector.");
        }

        // Show final session stats
        if (resultText != null)
        {
            var stats = GameStatsManager.Instance;

            resultText.text =
                "<b>Game Over!</b>\n" +
                $"⏱ Time: {stats.totalSessionTime:F1} sec\n" +
                $"🚴 Avg Speed: {stats.GetAverageSpeed():F2} km/h\n" +
                $"🏁 Max Speed: {stats.GetMaxSpeed():F2} km/h\n" +
                $"⭐ Total XP: {stats.totalXP:F1}\n" +
                $"⚡ XP / sec: {stats.GetXPPerSecond():F2}";
        }
    }

    public void RestartGame()//restart current session
    {
        Time.timeScale = 1f;
        GameStatsManager.Instance.ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()//go back to main menu
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void ExitGame()//exit the game
    {
        Debug.Log("🚪 Exiting game...");
        Application.Quit();
    }
}
