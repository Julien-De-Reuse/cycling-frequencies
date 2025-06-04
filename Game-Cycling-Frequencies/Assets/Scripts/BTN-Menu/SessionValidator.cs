using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionValidator : MonoBehaviour
{
    [Tooltip("De naam van de scene die je wilt laden als alles is ingevuld")]
    public string sceneToLoad;

    public void TryStartSession()
    {
        var manager = NEWGameManager.Instance;
        var data = manager.spectrumRideData;

        // Controleer of muziek en sessieduur zijn ingevuld
        if (string.IsNullOrEmpty(manager.selectedMusicName) || string.IsNullOrEmpty(data.gameTime))
        {
            Debug.LogWarning("⚠️ Vul eerst muziek én sessieduur in!");
            // Hier kun je eventueel nog UI feedback tonen
            return;
        }

        // Alles in orde, laad de scene
        Debug.Log($"✅ Start sessie in scene '{sceneToLoad}' met muziek '{manager.selectedMusicName}' en tijd '{data.gameTime}'");
        SceneManager.LoadScene(sceneToLoad);
    }
}
