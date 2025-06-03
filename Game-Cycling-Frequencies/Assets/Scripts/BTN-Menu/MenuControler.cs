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
        if (input == "+" || input == "-")
        {
            confirmIndex = 1 - confirmIndex;
            UpdateConfirmationButtonStyles();
        }
        else if (input == "C")
        {
            if (confirmIndex == 0)
            {
                ExecuteMenuOption();
            }
            else
            {
                isConfirming = false;
                confirmPanel.SetActive(false);
                currentPanel.SetActive(true);
                UpdateButtonStyles();
            }
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
            Debug.Log("Executing: " + menuButtons[selectedIndex].name);

            confirmPanel.SetActive(false);
            currentPanel.SetActive(false);

            if (nextPanel != null)
            {
                nextPanel.SetActive(true);
            }

            isConfirming = false;
            menuButtons[selectedIndex].onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("Invalid selection index: " + selectedIndex);
        }
    }
}
