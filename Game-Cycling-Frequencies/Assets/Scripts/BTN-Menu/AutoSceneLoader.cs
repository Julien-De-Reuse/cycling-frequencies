using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    private bool sceneLoadingStarted = false;

    void OnEnable()
    {
        if (!sceneLoadingStarted)
        {
            sceneLoadingStarted = true;
            Debug.Log("⏳ StepToScene actief - scene wordt binnen 1 seconde geladen...");
            Invoke("LoadScene", 1f); // kleine wachttijd zodat je de tekst kan zien
        }
    }

    void LoadScene()
    {
        string sceneToLoad = "";
        bool sessionDurationValid = false;

        switch (NEWGameManager.Instance.currentMod)
        {
            case NEWGameManager.ModType.SpectrumRide:
                sceneToLoad = NEWGameManager.Instance.spectrumRideData.environment;
                sessionDurationValid = NEWGameManager.Instance.spectrumRideData.selectedSessionDuration > 0;
                break;

            case NEWGameManager.ModType.CruiseControl:
                sceneToLoad = NEWGameManager.Instance.cruiseControlData.environment;
                sessionDurationValid = true; // No duration required in this mod
                break;

            case NEWGameManager.ModType.Overdrive:
                sceneToLoad = NEWGameManager.Instance.overdriveData.environment;
                sessionDurationValid = true; // No duration required in this mod
                break;
        }

        if (!sessionDurationValid)
        {
            Debug.LogWarning("⚠️ Geen geldige sessieduur gekozen – scene wordt niet geladen.");
            return;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("🚀 Scene wordt geladen: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("⚠️ Geen environment ingesteld – scene kan niet geladen worden.");
        }
    }
}
