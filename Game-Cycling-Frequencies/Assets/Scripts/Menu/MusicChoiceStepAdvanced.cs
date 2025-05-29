using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicChoiceStepAdvanced : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text selectedText;
    public TMP_Text durationText;
    public GameObject nextPanel;

    private string selectedSong = "";
    private AudioClip currentClip;

    public void SelectSong(string songName)
    {
        selectedSong = songName;
        selectedText.text = "Gekozen song: " + songName;

        // Load en speel preview
        currentClip = Resources.Load<AudioClip>("Music/" + songName.ToLower());
        if (currentClip != null)
        {
            audioSource.clip = currentClip;
            audioSource.time = 0f;
            audioSource.Play();

            durationText.text = $"Duur: {Mathf.FloorToInt(currentClip.length)} sec";
        }
        else
        {
            Debug.LogWarning("Audio niet gevonden voor song: " + songName);
        }
    }

    public void ConfirmChoice()
    {
        GameManager.Instance.musicGenre = selectedSong; // Of musicSong als je dat gebruikt
        audioSource.Stop();
        gameObject.SetActive(false);
        nextPanel.SetActive(true);
    }
}
