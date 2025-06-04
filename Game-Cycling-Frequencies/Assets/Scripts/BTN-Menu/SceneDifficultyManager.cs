using UnityEngine;

public class SceneDifficultyManager : MonoBehaviour
{
    void Start()
    {
        int level = NEWGameManager.Instance.spectrumRideData.idealLevel;

        Spectrum spectrum = FindObjectOfType<Spectrum>();
        if (spectrum != null)
        {
            spectrum.StartDriving(level);
            Debug.Log("Spectrum started with difficulty level: " + level);
        }
        else
        {
            Debug.LogWarning("Geen Spectrum script gevonden in de scene.");
        }
    }
}
