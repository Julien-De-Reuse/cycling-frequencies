using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Needed for List<>
using System.IO.Ports; // Needed for SerialPort

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

    public List<Button> gameOverButtons; // Assign in Inspector: [Restart, Menu, Exit]
    private int selectedButtonIndex = 0;

    private SerialPort serial; // Assign in Inspector or FindObjectOfType in Start()

    private bool gameOverTriggered = false;

    public bool IsGameOver => gameOverTriggered;

    public void OnGameOver()
    {
        if (gameOverTriggered) return;
        gameOverTriggered = true;

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
            UpdateButtonStyles(); // <-- Add this line
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

        // Zet het pauzepanel uit en deactiveer PauseManager volledig
        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        if (pauseManager != null)
        {
            if (pauseManager.pausePanel != null)
                pauseManager.pausePanel.SetActive(false);

            pauseManager.enabled = false; // Volledig uitschakelen
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

    void Start()
    {
        serial = SerialManager.Instance.serial;
        UpdateButtonStyles();
    }

    void Update()
    {
        if (!IsGameOver) return;

        if (serial != null && serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (input == "-")
                {
                    selectedButtonIndex = (selectedButtonIndex - 1 + gameOverButtons.Count) % gameOverButtons.Count;
                    UpdateButtonStyles();
                }
                else if (input == "+")
                {
                    selectedButtonIndex = (selectedButtonIndex + 1) % gameOverButtons.Count;
                    UpdateButtonStyles();
                }
                else if (input == "C")
                {
                    gameOverButtons[selectedButtonIndex].onClick.Invoke();
                }
            }
            catch (System.Exception) { }
        }
    }

    private void UpdateButtonHighlight()
    {
        for (int i = 0; i < gameOverButtons.Count; i++)
        {
            var btn = gameOverButtons[i];
            var colors = btn.colors;
            if (i == selectedButtonIndex)
            {
                colors.normalColor = Color.green;
                btn.image.color = Color.green;
            }
            else
            {
                colors.normalColor = Color.white;
                btn.image.color = Color.white;
            }
            btn.colors = colors;
        }
    }

    void UpdateButtonStyles()
    {
        for (int i = 0; i < gameOverButtons.Count; i++)
        {
            ColorBlock colors = gameOverButtons[i].colors;
            colors.normalColor = (i == selectedButtonIndex) ? Color.green : Color.white;
            gameOverButtons[i].colors = colors;
        }
    }
}
