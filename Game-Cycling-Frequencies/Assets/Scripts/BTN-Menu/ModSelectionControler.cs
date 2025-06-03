using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class ModSelectionController : MonoBehaviour
{
    private SerialPort serial;

    [Header("Mod Buttons to select")]
    public List<Button> modButtons = new List<Button>();
    private int selectedIndex = 0;

    [Header("Mod Scene Names")]
    public List<string> modMenuScenes = new List<string>();

    [Header("Mod Popups")]
    public List<GameObject> modPopups = new List<GameObject>();

    private int popupConfirmIndex = 0;
    private bool isPopupActive = false;

    private Button activeConfirmButton;
    private Button activeCancelButton;

    void Start()
    {
        serial = SerialManager.Instance.serial;
        UpdateButtonStyles();
        HideAllPopups();
    }

    void Update()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (!isPopupActive)
                {
                    if (input == "+")
                    {
                        selectedIndex = (selectedIndex + 1) % modButtons.Count;
                        UpdateButtonStyles();
                    }
                    else if (input == "-")
                    {
                        selectedIndex = (selectedIndex - 1 + modButtons.Count) % modButtons.Count;
                        UpdateButtonStyles();
                    }
                    else if (input == "C")
                    {
                        ShowPopupForSelectedMod();
                    }
                }
                else
                {
                    if (input == "+" || input == "-")
                    {
                        popupConfirmIndex = 1 - popupConfirmIndex;
                        UpdatePopupButtonStyles();
                    }
                    else if (input == "C")
                    {
                        if (popupConfirmIndex == 0)
                        {
                            ConfirmSelectedMod();
                        }
                        else
                        {
                            CancelPopup();
                        }
                    }
                }
            }
            catch (System.Exception) { }
        }
    }

    void UpdateButtonStyles()
    {
        for (int i = 0; i < modButtons.Count; i++)
        {
            if (modButtons[i] == null) continue;

            ColorBlock colors = modButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.green : Color.white;
            modButtons[i].colors = colors;
        }
    }

    void ShowPopupForSelectedMod()
    {
        HideAllPopups();

        if (selectedIndex >= 0 && selectedIndex < modPopups.Count)
        {
            GameObject popup = modPopups[selectedIndex];
            popup.SetActive(true);
            isPopupActive = true;
            popupConfirmIndex = 0;

            // Zoek knoppen binnen de popup
            Button[] buttonsInPopup = popup.GetComponentsInChildren<Button>(true);
            foreach (Button b in buttonsInPopup)
            {
                if (b.name.ToLower().Contains("next"))
                {
                    activeConfirmButton = b;
                }
                else if (b.name.ToLower().Contains("back"))
                {
                    activeCancelButton = b;
                }
            }

            UpdatePopupButtonStyles();
        }
    }

    void CancelPopup()
    {
        HideAllPopups();
        isPopupActive = false;
    }

    void HideAllPopups()
    {
        foreach (GameObject popup in modPopups)
        {
            if (popup != null)
                popup.SetActive(false);
        }
    }

    void ConfirmSelectedMod()
    {
        string selectedModName = modButtons[selectedIndex].name;

        switch (selectedModName)
        {
            case "SpectrumRideButton":
                NEWGameManager.Instance.SetCurrentMod(NEWGameManager.ModType.SpectrumRide);
                break;
            case "OverdriveButton":
                NEWGameManager.Instance.SetCurrentMod(NEWGameManager.ModType.Overdrive);
                break;
            case "CruiseControlButton":
                NEWGameManager.Instance.SetCurrentMod(NEWGameManager.ModType.CruiseControl);
                break;
        }

        SceneManager.LoadScene(modMenuScenes[selectedIndex]);
    }

    void UpdatePopupButtonStyles()
    {
        if (activeConfirmButton == null || activeCancelButton == null) return;

        ColorBlock confirmColors = activeConfirmButton.colors;
        ColorBlock cancelColors = activeCancelButton.colors;

        if (popupConfirmIndex == 0)
        {
            confirmColors.normalColor = Color.green;
            cancelColors.normalColor = Color.white;
        }
        else
        {
            confirmColors.normalColor = Color.white;
            cancelColors.normalColor = Color.red;
        }

        activeConfirmButton.colors = confirmColors;
        activeCancelButton.colors = cancelColors;
    }
}
