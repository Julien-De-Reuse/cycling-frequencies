using UnityEngine;

public class CruiseController : MonoBehaviour
{
    [Header("Cruise Control Settings")]
    public float speed = 10f; // Default speed, can be set from SessionStartManager

    private bool isMoving = false;

    public void SetSpeed(float newSpeed)
    {
        Debug.Log("SetSpeed called with: " + newSpeed);
        speed = newSpeed;
    }

    public void StartMoving()
    {
        Debug.Log("StartMoving called!");
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
        {
            // Move the car in its local forward direction at constant speed
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }
}