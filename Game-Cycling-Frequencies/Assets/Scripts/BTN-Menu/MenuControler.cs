using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class MenuController : MonoBehaviour
{
    [Header("Serial Settings")]
    public string portName = "COM3";
    public int baudRate = 9600;
    private SerialPort serial;

    [Header("Menu Buttons")]
    public List<Button> menuButtons = new List<Button>(); // Assign 5 buttons in Inspector
    private int selectedIndex = 0;

    [Header("Confirmation Popup")]
    public GameObject confirmPanel;
    public Button yesButton;
    public Button noButton;
    private int confirmIndex = 0; // 0 = Yes, 1 = No
    private bool isConfirming = false;

    void Start()
    {
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 25;

        if (!serial.IsOpen)
        {
            serial.Open();
        }

        UpdateButtonStyles();
    }

    void Update()
    {
        if (!serial.IsOpen) return;

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

    void HandleMenuNavigation(string input)
    {
        if (input == "+")
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Count) % menuButtons.Count;
            UpdateButtonStyles();
        }
        else if (input == "-")
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
            confirmIndex = 1 - confirmIndex; // Toggle between 0 and 1
            UpdateConfirmationButtonStyles();
        }
        else if (input == "C")
        {
            if (confirmIndex == 0)
            {
                ExecuteMenuOption();
            }

            confirmPanel.SetActive(false);
            isConfirming = false;
        }
    }

    void UpdateButtonStyles()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.cyan : Color.white;
            menuButtons[i].colors = colors;
        }
    }

    void ShowConfirmationPopup()
    {
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
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Option 1 triggered"); break;
            case 1:
                Debug.Log("Option 2 triggered"); break;
            case 2:
                Debug.Log("Option 3 triggered"); break;
            case 3:
                Debug.Log("Option 4 triggered"); break;
            case 4:
                Debug.Log("Option 5 triggered"); break;
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
            serial.Close();
    }
}
