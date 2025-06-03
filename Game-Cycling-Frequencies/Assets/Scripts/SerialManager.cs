using UnityEngine;
using System.IO.Ports;

public class SerialManager : MonoBehaviour
{
    public static SerialManager Instance;

    public string portName = "COM3";
    public int baudRate = 9600;

    public SerialPort serial;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            serial = new SerialPort(portName, baudRate);
            serial.ReadTimeout = 25;

            try
            {
                serial.Open();
                Debug.Log("Serial port opened on " + portName);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error opening serial port: " + e.Message);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
