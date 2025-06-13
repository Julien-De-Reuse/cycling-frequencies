using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;
    public bool sessionActive = false;
    public float totalSessionTime = 0f;
    public float totalXP = 0f;

    private List<float> speedSamples = new List<float>();

    public GameOver gameOverManager; // Sleep deze in de Inspector

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Stop XP en tijd als game over is
        if (!sessionActive || (gameOverManager != null && gameOverManager.IsGameOver))
            return;

        totalSessionTime += Time.deltaTime;
        // XP logic here
    }

    public void AddSpeedSample(float speed)
    {
        speedSamples.Add(speed);
    }
public void AddXP(float xp)
{
    if (!sessionActive)
    {
        Debug.LogWarning("⚠️ XP added while session NOT active! Ignoring. Value: " + xp);
        return;
    }

    totalXP += xp;
    Debug.Log("✅ XP added: " + xp + " | Total: " + totalXP);
}


    public void UpdateSessionTime(float deltaTime)
    {
        if (sessionActive)
        {
            totalSessionTime += deltaTime;
        }
    }

    public float GetAverageSpeed()
    {
        if (speedSamples.Count == 0) return 0f;
        float total = 0f;
        foreach (float s in speedSamples) total += s;
        return total / speedSamples.Count;
    }

    public float GetMaxSpeed()
    {
        if (speedSamples.Count == 0) return 0f;
        float max = speedSamples[0];
        foreach (float s in speedSamples)
        {
            if (s > max) max = s;
        }
        return max;
    }

    public float GetXPPerSecond()
    {
        if (totalSessionTime == 0f) return 0f;
        return totalXP / totalSessionTime;
    }

    public void ResetStats()
    {
        totalSessionTime = 0f;
        totalXP = 0f;
        speedSamples.Clear();
        sessionActive = false;
    }

    public void StartSessionStats()
    {
        totalSessionTime = 0f;
        totalXP = 0f;
        // Reset other stats as needed
        sessionActive = true;
    }

    public void StopSessionStats()
    {
        sessionActive = false;
    }
}
// This script manages game statistics such as total session time, total XP, and speed samples.
// It provides methods to add speed samples, add XP, update session time, and calculate average/max speed and XP per second.