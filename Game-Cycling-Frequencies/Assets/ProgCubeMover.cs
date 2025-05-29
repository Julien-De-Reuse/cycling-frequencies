using UnityEngine;

public class ProgCubeMover : MonoBehaviour
{
    public float initialSpeed = 1f;         // Starting speed of the cube
    public float acceleration = 0.2f;       // How much the speed increases per second

    private float currentSpeed;

    void Start()
    {
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        currentSpeed += acceleration * Time.deltaTime; // Increase speed over time
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
    }
}
