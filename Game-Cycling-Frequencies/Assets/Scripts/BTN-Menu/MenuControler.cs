using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class MenuController : MonoBehaviour
{
    private SerialPort serial;

    [Header("Panels")]
    public GameObject currentPanel;
    public GameObject confirmPanel;
    public GameObject nextPanel;

    [Header("Menu Buttons")]
    public List<Button> menuButtons = new List<Button>();
    private int selectedIndex = 0;

    [Header("Confirmation Buttons")]
    public Button yesButton;
    public Button noButton;
    private int confirmIndex = 0; // 0 = Yes, 1 = No
    private bool isConfirming = false;

    void Start()
    {
        serial = SerialManager.Instance.serial;
        UpdateButtonStyles();
    }

    void Update()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (!isConfirming)
                {
                    HandleMenuNavigation(input);
                }
                else
                {
                    HandleConfirmationNavigation(input);
                }
            }
            catch (System.Exception) { }
        }
    }

    void HandleMenuNavigation(string input)
    {
        if (input == "-")
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Count) % menuButtons.Count;
            UpdateButtonStyles();
        }
        else if (input == "+")
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Count;
            UpdateButtonStyles();
        }
        else if (input == "C")
        {
            ShowConfirmationPopup();
        }
    }

    void HandleConfirmationNavigation(string input)
    {
        if (input == "+")
        {
            confirmIndex = 0; // Yes
            UpdateConfirmationButtonStyles();
            ExecuteMenuOption();
        }
        else if (input == "-")
        {
            confirmIndex = 1; // No
            UpdateConfirmationButtonStyles();
            isConfirming = false;
            confirmPanel.SetActive(false);
            currentPanel.SetActive(true);
            UpdateButtonStyles();
        }
    }

    void UpdateButtonStyles()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.green : Color.white;
            menuButtons[i].colors = colors;
        }
    }

    void ShowConfirmationPopup()
    {
        Debug.Log("Showing confirmation popup for: " + menuButtons[selectedIndex].name);
        isConfirming = true;
        confirmIndex = 0;
        confirmPanel.SetActive(true);
        UpdateConfirmationButtonStyles();
    }

    void UpdateConfirmationButtonStyles()
    {
        ColorBlock yesColors = yesButton.colors;
        ColorBlock noColors = noButton.colors;

        if (confirmIndex == 0)
        {
            yesColors.normalColor = Color.green;
            noColors.normalColor = Color.white;
        }
        else
        {
            yesColors.normalColor = Color.white;
            noColors.normalColor = Color.red;
        }

        yesButton.colors = yesColors;
        noButton.colors = noColors;
    }

    void ExecuteMenuOption()
{
    if (selectedIndex >= 0 && selectedIndex < menuButtons.Count)
    {
        string buttonName = menuButtons[selectedIndex].name;
        Debug.Log("🧭 Bevestigd: " + buttonName);
        Debug.Log("📍 currentPanel: " + currentPanel.name);

        isConfirming = false;

        // Koppel de juiste actie aan de knop
        menuButtons[selectedIndex].onClick.Invoke();

// Als StepToScene net werd geactiveerd → laadt meteen scene
if (currentPanel.name == "StepToScene")
{
    Debug.Log("🚀 StepToScene geactiveerd – scène wordt automatisch geladen");
    TryLoadSelectedScene();
}


        // 🔁 Panel wissel
        confirmPanel.SetActive(false);
        currentPanel.SetActive(false);

        if (nextPanel != null)
        {
            Debug.Log("➡️ Ga naar volgend panel: " + nextPanel.name);
            nextPanel.SetActive(true);

            // 👇 BELANGRIJK: zet currentPanel om mee te geven aan volgende stap
            currentPanel = nextPanel;
        }
    }
    else
    {
        Debug.LogWarning("Invalid selection index: " + selectedIndex);
    }
}
void TryLoadSelectedScene()
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
            sessionDurationValid = NEWGameManager.Instance.cruiseControlData.selectedSessionDuration > 0;
            break;

        case NEWGameManager.ModType.Overdrive:
            sceneToLoad = NEWGameManager.Instance.overdriveData.environment;
            sessionDurationValid = true;
            break;
    }

    if (!sessionDurationValid)
    {
        Debug.LogWarning("⚠️ Geen geldige sessieduur gekozen – scene wordt niet geladen.");
        return;
    }

    if (!string.IsNullOrEmpty(sceneToLoad))
    {
        Debug.Log("✅ Scene wordt geladen: " + sceneToLoad);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    else
    {
        Debug.LogWarning("⚠️ Geen environment gekozen – scene kan niet geladen worden.");
    }
}

}



