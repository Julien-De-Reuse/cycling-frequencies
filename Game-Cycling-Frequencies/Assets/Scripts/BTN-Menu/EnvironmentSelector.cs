using UnityEngine;

public class EnvironmentSelector : MonoBehaviour
{
    public void SelectEnvironment(string environmentName)
    {
        var gm = NEWGameManager.Instance;

        switch (gm.currentMod)
        {
            case NEWGameManager.ModType.SpectrumRide:
                gm.spectrumRideData.environment = environmentName;
                break;

            case NEWGameManager.ModType.Overdrive:
                gm.overdriveData.environment = environmentName;
                break;

            case NEWGameManager.ModType.CruiseControl:
                gm.cruiseControlData.environment = environmentName;
                break;

            default:
                Debug.LogWarning("Geen geldige mod actief bij environmentkeuze.");
                return;
        }

        Debug.Log("Environment gekozen en opgeslagen: " + environmentName);
        // Scene wordt NIET geladen hier!
    }
}
