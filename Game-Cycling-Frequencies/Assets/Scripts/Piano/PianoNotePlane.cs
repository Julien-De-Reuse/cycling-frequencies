using UnityEngine;
using System.IO.Ports;

public class PianoNotePlane : MonoBehaviour
{
    public enum Side { Left, Right }
    public Side noteSide;

    private bool playerInZone = false;
    private SerialPort serial;

    void Start()
    {
        serial = SerialManager.Instance.serial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInZone = true;
            Debug.Log("🎯 Camera in note zone (" + noteSide + ")");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInZone = false;
            Debug.Log("🚫 Camera uit note zone");
        }
    }

    void Update()
    {
        if (!playerInZone || serial == null || !serial.IsOpen) return;

        try
        {
            string input = serial.ReadLine().Trim();

            if ((noteSide == Side.Left && input == "-") ||
                (noteSide == Side.Right && input == "+"))
            {
                RegisterHit();
            }
        }
        catch (System.Exception)
        {
            // Voorkom spam als er niets binnenkomt
        }
    }

    void RegisterHit()
    {
        Debug.Log("✅ Juist gedrukt!");
        PianoXPManager.Instance.AddXP(1);
        Destroy(gameObject); // note verdwijnt
    }
}
