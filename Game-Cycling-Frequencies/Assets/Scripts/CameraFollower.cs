using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CameraFollower : MonoBehaviour
{
    public Transform targetToFollow;
    public float heightOffset = 2f;
    public float speedMultiplier = 1f;
    public TextMeshProUGUI speedText;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool pedalSignalSent = false;
    private bool cameraCanMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var serial = SerialManager.Instance.serial;

        if (serial != null && serial.IsOpen)
        {
            try
            {
                string message = serial.ReadLine().Trim();

if (!string.IsNullOrEmpty(message) && float.TryParse(message, out float speedValue))
{
    currentSpeed = speedValue * speedMultiplier;
    Debug.Log("Speed from Arduino: " + currentSpeed);

    if (speedText != null)
        speedText.text = "Speed: " + currentSpeed.ToString("F1") + " km/h";

    // Add speed sample
    GameStatsManager.Instance.AddSpeedSample(currentSpeed);

    if (!pedalSignalSent && currentSpeed > 1f)
    {
        pedalSignalSent = true;
        FindObjectOfType<SessionStartManager>().OnFirstPedal();
    }
}

                
            }
            catch (System.Exception) { }
        }
    }

    void FixedUpdate()
    {
        if (cameraCanMove)
        {
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
    public void DisableCameraFollow()
{
    cameraCanMove = false;
    currentSpeed = 0f; // just to be safe
    Debug.Log("📷 Camera movement DISABLED");
}


    void LateUpdate()
    {
        if (targetToFollow != null)
        {
            Vector3 targetLookAt = targetToFollow.position + Vector3.up * heightOffset;
            transform.LookAt(targetLookAt);
        }
    }

    public void EnableCameraFollow()
    {
        Debug.Log("🎥 Camera following ENABLED");
        cameraCanMove = true;
    }
}
