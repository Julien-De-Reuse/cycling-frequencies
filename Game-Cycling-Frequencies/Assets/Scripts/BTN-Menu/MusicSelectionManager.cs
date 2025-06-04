using UnityEngine;

public class MusicSelectionManager : MonoBehaviour
{
    public void SelectMusic(string musicName)
    {
        if (NEWGameManager.Instance != null)
        {
            NEWGameManager.Instance.selectedMusicName = musicName;
            Debug.Log("🎵 Muziek gekozen: " + musicName);
        }
        else
        {
            Debug.LogError("❌ NEWGameManager.Instance is null — kan geen muziek opslaan.");
        }
    }
}
