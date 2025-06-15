using UnityEngine;
using TMPro;

public class OverdriveTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameOver gameOverManager;

    private float elapsedTime = 0f;
    private float lastXP = 0f;
    private float timeSinceLastXP = 0f;
    private bool isGameOver = false;

    void Start()
    {
        if (gameOverManager == null)
            gameOverManager = FindObjectOfType<GameOver>();
        lastXP = GameStatsManager.Instance.totalXP;
    }

    void Update()
    {
        if (gameOverManager != null && gameOverManager.IsGameOver)
            return;

        // Timer display
        elapsedTime += Time.deltaTime;
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // XP check
        float currentXP = GameStatsManager.Instance.totalXP;
        if (currentXP > lastXP)
        {
            Debug.Log("XP earned! Reset timer.");
            lastXP = currentXP;
            timeSinceLastXP = 0f;
        }
        else
        {
            timeSinceLastXP += Time.deltaTime;
            Debug.Log($"No XP. Timer: {timeSinceLastXP:F2}");
        }

        // Game over if no XP for 3 seconds
        if (!isGameOver && timeSinceLastXP >= 3f)
        {
            isGameOver = true;
            if (gameOverManager != null)
                gameOverManager.OnGameOver();
        }
    }
}