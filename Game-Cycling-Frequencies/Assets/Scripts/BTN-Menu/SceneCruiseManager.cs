using UnityEngine;

public class SceneCruiseManager : MonoBehaviour
{
    public GameOver gameOverManager;
    public float xpTimeout = 3f; // seconds without XP before game over

    private float lastXP = 0f;
    private float timeSinceLastXP = 0f;
    private bool isGameOver = false;

    void Start()
    {
        float cruiseSpeed = 10f;
        float.TryParse(NEWGameManager.Instance.cruiseControlData.speed, out cruiseSpeed);

        var cruiseController = FindObjectOfType<CruiseController>();
        if (cruiseController != null)
        {
            cruiseController.SetSpeed(cruiseSpeed);
        }
        else
        {
            Debug.LogWarning("Geen CruiseController script gevonden in de scene.");
        }

        if (gameOverManager == null)
            gameOverManager = FindObjectOfType<GameOver>();

        lastXP = GameStatsManager.Instance.totalXP;
    }

    void Update()
    {
        // Only check for XP inactivity if the session is active
        if (!GameStatsManager.Instance.sessionActive)
            return;

        if (gameOverManager != null && gameOverManager.IsGameOver)
            return;

        // XP check
        float currentXP = GameStatsManager.Instance.totalXP;
        if (currentXP > lastXP)
        {
            lastXP = currentXP;
            timeSinceLastXP = 0f;
        }
        else
        {
            timeSinceLastXP += Time.deltaTime;
        }

        // Game over if no XP for xpTimeout seconds
        if (!isGameOver && timeSinceLastXP >= xpTimeout)
        {
            isGameOver = true;
            if (gameOverManager != null)
                gameOverManager.OnGameOver();
        }
    }

    public void SetCruiseSpeed(float value)
    {
        NEWGameManager.Instance.cruiseControlData.speed = value.ToString();
    }
}