using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class StartController : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM3", 9600); // Change COM port if needed

    public Button startButton;
    public Button exitButton;
    private int selectedIndex = 0; // 0 = Start, 1 = Exit

    private Start startScript;

    void Start()
    {
        if (!serial.IsOpen)
        {
            serial.Open();
            serial.ReadTimeout = 25;
        }

        // Load your Start script (must be on same GameObject)
        startScript = GetComponent<Start>();

        UpdateButtonStyles();
    }

    void Update()
    {
        if (serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (input == "+")
                {
                    selectedIndex = 0; // Start
                    UpdateButtonStyles();
                    Debug.Log("Selected Start");
                }
                else if (input == "-")
                {
                    selectedIndex = 1; // Exit
                    UpdateButtonStyles();
                    Debug.Log("Selected Exit");
                }
                else if (input == "C")
                {
                    ConfirmSelection();
                }
            }
            catch (System.Exception) { }
        }
    }

    void ConfirmSelection()
    {
        if (selectedIndex == 0)
        {
            startScript.StartGame();
        }
        else if (selectedIndex == 1)
        {
            startScript.QuitGame();
        }
    }

    void UpdateButtonStyles()
    {
        ColorBlock startColors = startButton.colors;
        ColorBlock exitColors = exitButton.colors;

        // Highlight selected button
        if (selectedIndex == 0)
        {
            startColors.normalColor = Color.green;
            exitColors.normalColor = Color.white;
        }
        else
        {
            startColors.normalColor = Color.white;
            exitColors.normalColor = Color.red;
        }

        startButton.colors = startColors;
        exitButton.colors = exitColors;
    }

    void OnApplicationQuit()
    {
        if (serial.IsOpen) serial.Close();
    }
}
