using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class GOTOMENU : MonoBehaviour
{
    // Reference to your serial manager or serial input
    public SerialManager serialManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (serialManager == null)
            serialManager = FindObjectOfType<SerialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (serialManager != null && serialManager.serial != null && serialManager.serial.IsOpen)
        {
            try
            {
                string input = serialManager.serial.ReadLine();
                if (input.Trim() == "C")
                {
                    GoToMenu();
                }
            }
            catch (System.TimeoutException)
            {
                // Ignore timeout, just means no data this frame
            }
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
