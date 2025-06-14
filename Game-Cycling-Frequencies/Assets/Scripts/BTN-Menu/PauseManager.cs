using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pausePanel;

    [Header("Gameplay Elements")]
    public SessionStartManager sessionStartManager;
    public GameObject car;
    public MonoBehaviour carControllerScript;

    [Header("Pause Buttons")]
    public List<Button> pauseButtons = new List<Button>();
    private int pauseSelectedIndex = 0;

    private SerialPort serial;
    private bool isPaused = false;

    private float currentSpeed = 0f;
    private float speedZeroTimer = 0.05f;
    private float pauseThreshold = 1f;
    private float sessionStartTimer = 0f;
    private float pauseEnableDelay = 5f;

    public GameOver gameOverManager;

    void Start()
    {
        serial = SerialManager.Instance.serial;
        UpdatePauseButtonStyles();
    }

    void Update()
    {
        // Block all logic if session hasn't started or game is over
        if (sessionStartManager == null || !sessionStartManager.SessionStarted || gameOverManager == null || gameOverManager.IsGameOver)
            return;

        // Time since session start
        sessionStartTimer += Time.unscaledDeltaTime;

        if (sessionStartTimer < pauseEnableDelay)
            return;

        // Pause with ESC key
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
            Debug.Log("⏸ Game paused (via ESC)");
        }

        // Read speed
        currentSpeed = CameraFollower.LatestSpeed;

        // BLOCK PAUSE if game is over
        if (!isPaused && currentSpeed <= 0.01f)
        {
            speedZeroTimer += Time.unscaledDeltaTime;

            if (speedZeroTimer >= pauseThreshold)
            {
                if (gameOverManager != null && gameOverManager.IsGameOver)
                {
                    Debug.Log("❌ Skip speed-pause: game is over");
                }
                else
                {
                    TogglePause();
                    Debug.Log($"⏸ Game paused due to speed = 0 for {pauseThreshold} seconds");
                }
            }
        }
        else
        {
            speedZeroTimer = 0f;
        }

        // Handle serial input if pause panel is active
        if (pausePanel != null && pausePanel.activeSelf && serial != null && serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (gameOverManager != null && gameOverManager.IsGameOver)
                    return;

                HandlePauseNavigation(input);
            }
            catch (System.Exception) { }
        }
    }

    void HandlePauseNavigation(string input)
    {
        if (input == "-")
        {
            pauseSelectedIndex = (pauseSelectedIndex - 1 + pauseButtons.Count) % pauseButtons.Count;
            UpdatePauseButtonStyles();
        }
        else if (input == "+")
        {
            pauseSelectedIndex = (pauseSelectedIndex + 1) % pauseButtons.Count;
            UpdatePauseButtonStyles();
        }
        else if (input == "C")
        {
            if (pauseSelectedIndex >= 0 && pauseSelectedIndex < pauseButtons.Count)
            {
                pauseButtons[pauseSelectedIndex].onClick.Invoke();
            }
        }

        Debug.Log("Selected pause button: " + pauseSelectedIndex);
    }

    void UpdatePauseButtonStyles()
    {
        for (int i = 0; i < pauseButtons.Count; i++)
        {
            ColorBlock colors = pauseButtons[i].colors;
            colors.normalColor = (i == pauseSelectedIndex) ? Color.green : Color.white;
            pauseButtons[i].colors = colors;
        }
    }

    public void TogglePause()
    {
        if (gameOverManager != null && gameOverManager.IsGameOver)
        {
            Debug.Log("❌ Cannot pause — game is over");
            return;
        }

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (gameOverManager != null && gameOverManager.IsGameOver)
        {
            Debug.Log("❌ Cannot pause — game is over");
            return;
        }

        Time.timeScale = 0f;
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        pauseSelectedIndex = 0; // Always start with first button selected
        UpdatePauseButtonStyles();

        if (sessionStartManager?.music != null)
            sessionStartManager.music.Pause();

        CameraFollower cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
            cam.DisableCameraFollow();

        if (carControllerScript != null)
            carControllerScript.enabled = false;

        if (car != null)
        {
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero;
        }

        Debug.Log("⏸ Game paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (sessionStartManager?.music != null)
            sessionStartManager.music.UnPause();

        if (carControllerScript != null)
            carControllerScript.enabled = true;

        CameraFollower cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
            cam.EnableCameraFollow();

        speedZeroTimer = 0f;
        sessionStartTimer = 0f;

        Debug.Log("▶️ Game resumed");
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
}
