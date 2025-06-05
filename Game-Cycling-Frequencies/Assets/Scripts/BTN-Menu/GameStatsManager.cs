using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public float totalSessionTime = 0f;
    public float totalXP = 0f;
    public List<float> speedSamples = new List<float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSpeedSample(float speed)
    {
        speedSamples.Add(speed);
    }

    public void AddXP(float xp)
    {
        totalXP += xp;
    }

    public void UpdateSessionTime(float deltaTime)
    {
        totalSessionTime += deltaTime;
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
    }
}
// This script manages game statistics such as total session time, total XP, and speed samples.
// It provides methods to add speed samples, add XP, update session time, and calculate average/max speed and XP per second.