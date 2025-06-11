using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using UnityEngine.InputSystem; // <-- toevoegen
using System.Collections.Generic;
using UnityEngine.UI;

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
    private float pauseThreshold = 2f; // seconds speed must remain 0 to trigger pause
    private float sessionStartTimer = 0f;
    private float pauseEnableDelay = 5f; // seconden na start sessie voordat pauze mogelijk is

    void Start()
    {
        serial = SerialManager.Instance.serial;
    }

    void Update()
    {
        // Pauzeer alleen als de sessie gestart is
        if (sessionStartManager == null || !sessionStartManager.SessionStarted)
            return;

        // Tel tijd sinds sessiestart op
        sessionStartTimer += Time.unscaledDeltaTime;

        // Pauze mag pas na de delay
        if (sessionStartTimer < pauseEnableDelay)
            return;

        // Pause with ESC key (nieuw Input System)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
            Debug.Log("⏸ Game paused");
        }

        // Gebruik de speed van CameraFollower
        currentSpeed = CameraFollower.LatestSpeed;

        // If speed is 0.0 for over 2 seconds, pause
        if (currentSpeed <= 0.01f)
        {
            speedZeroTimer += Time.unscaledDeltaTime;

            if (speedZeroTimer >= pauseThreshold && !isPaused)
            {
                TogglePause();
                Debug.Log("⏸ Game paused due to speed being 0 for over " + pauseThreshold + " seconds");
            }
        }
        else
        {
            speedZeroTimer = 0f;
        }

        // Navigatie via serial input als panel actief is
        if (pausePanel != null && pausePanel.activeSelf && serial != null && serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (input == "+")
                {
                    pauseSelectedIndex = (pauseSelectedIndex + 1) % pauseButtons.Count;
                    UpdatePauseButtonStyles();
                }
                else if (input == "-")
                {
                    pauseSelectedIndex = (pauseSelectedIndex - 1 + pauseButtons.Count) % pauseButtons.Count;
                    UpdatePauseButtonStyles();
                }
                else if (input == "C")
                {
                    // Voer de geselecteerde actie uit
                    switch (pauseSelectedIndex)
                    {
                        case 0: ResumeGame(); break;
                        case 1: RestartGame(); break;
                        case 2: GoToMenu(); break;
                    }
                }
            }
            catch (System.Exception) { }
        }
    }

    void UpdatePauseButtonStyles()
    {
        for (int i = 0; i < pauseButtons.Count; i++)
        {
            var btn = pauseButtons[i];
            var colors = btn.colors;
            if (i == pauseSelectedIndex)
            {
                colors.normalColor = Color.green;
                btn.image.color = Color.green; // Force image color
            }
            else
            {
                colors.normalColor = Color.white;
                btn.image.color = Color.white;
            }
            btn.colors = colors;
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

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
        sessionStartTimer = 0f; // <-- reset timer bij hervatten

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
