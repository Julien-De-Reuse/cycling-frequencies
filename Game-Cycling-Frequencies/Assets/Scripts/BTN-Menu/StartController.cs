using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class StartController : MonoBehaviour
{
    SerialPort serial;

    public Button startButton;
    public Button exitButton;
    private int selectedIndex = 0; // 0 = Start, 1 = Exit

    private Start startScript;

    void Start()
    {
        serial = SerialManager.Instance.serial;


        // Laad startscript (op zelfde GameObject)
        startScript = GetComponent<Start>();

        UpdateButtonStyles();
    }

    void Update()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                string input = serial.ReadLine().Trim();

                if (input == "+")
                {
                    selectedIndex = 0;
                    UpdateButtonStyles();
                    Debug.Log("Selected Start");
                }
                else if (input == "-")
                {
                    selectedIndex = 1;
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
        // De poort wordt netjes gesloten door SerialManager → geen dubbele .Close() nodig hier.
    }
}
