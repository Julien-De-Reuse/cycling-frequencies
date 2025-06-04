using UnityEngine;
using UnityEngine.UI;

public class SessionDurationButton : MonoBehaviour
{
    public int durationInSeconds = 60; // Deze stel je in per knop in de Inspector

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetDurationForCurrentMod);
    }

    void SetDurationForCurrentMod()
    {
        if (NEWGameManager.Instance == null)
        {
            Debug.LogError("❌ Geen NEWGameManager.Instance gevonden.");
            return;
        }

        switch (NEWGameManager.Instance.currentMod)
        {
            case NEWGameManager.ModType.SpectrumRide:
                NEWGameManager.Instance.spectrumRideData.selectedSessionDuration = durationInSeconds;
                break;

            case NEWGameManager.ModType.CruiseControl:
                NEWGameManager.Instance.cruiseControlData.selectedSessionDuration = durationInSeconds;
                break;

            case NEWGameManager.ModType.Overdrive:
                // Als je geen sessieduur gebruikt in Overdrive, kun je deze leeg laten of standaardwaarde zetten
                Debug.Log("⚠️ Overdrive gebruikt (voorlopig) geen sessieduur.");
                break;

            default:
                Debug.LogWarning("⚠️ Geen geldige mod actief bij klikken op sessieknop.");
                break;
        }

        Debug.Log($"⏱️ Sessieduur ingesteld op {durationInSeconds} seconden voor mod: {NEWGameManager.Instance.currentMod}");
    }
}
