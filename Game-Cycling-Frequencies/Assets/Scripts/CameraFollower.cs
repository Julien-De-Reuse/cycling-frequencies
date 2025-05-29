using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class CameraFollower : MonoBehaviour
{
    public Transform targetToFollow;
    public SerialController serialController;
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
        string message = serialController.ReadSerialMessage();

        if (message != null && float.TryParse(message, out float speedValue))
        {
            currentSpeed = speedValue * speedMultiplier;
            Debug.Log("Speed from Arduino: " + currentSpeed);

            if (speedText != null)
                speedText.text = "Speed: " + currentSpeed.ToString("F1") + " km/h";

            if (!pedalSignalSent && currentSpeed > 1f)
            {
                pedalSignalSent = true;
                FindObjectOfType<SessionStartManager>().OnFirstPedal();
            }
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
