using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class MusicSelector : MonoBehaviour
{
    public List<AudioClip> musicClips;
    public List<string> clipNames;
    public AudioSource previewSource;

    public GameObject buttonPrefab;         // Prefab with a Button + Text
    public Transform buttonContainer;       // Parent with Vertical Layout Group

    public Button confirmButton;
    private int selectedIndex = -1;
    private bool isPreviewing = false;
    private List<Button> createdButtons = new();

    void Start()
    {
        for (int i = 0; i < musicClips.Count; i++)
        {
            int index = i;
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();

            btnText.text = clipNames[i];

            btn.onClick.AddListener(() => OnSongClicked(index));
            createdButtons.Add(btn);
        }

        confirmButton.onClick.AddListener(ConfirmSong);
        confirmButton.interactable = false;
    }

    void OnSongClicked(int index)
    {
        selectedIndex = index;
        confirmButton.interactable = true;

        // Highlight selected
        for (int i = 0; i < createdButtons.Count; i++)
        {
            ColorBlock colors = createdButtons[i].colors;
            colors.normalColor = (i == index) ? Color.green : Color.white;
            createdButtons[i].colors = colors;
        }

        // Play preview
        previewSource.Stop();
        previewSource.clip = musicClips[index];
        previewSource.Play();
        isPreviewing = true;
    }

    void ConfirmSong()
    {
        if (selectedIndex >= 0)
        {
            PlayerPrefs.SetInt("SelectedSongIndex", selectedIndex);
            previewSource.Stop();
            isPreviewing = false;

            // Move to next panel/scene here
        }
    }
}
