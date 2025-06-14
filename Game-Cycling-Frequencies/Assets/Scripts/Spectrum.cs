using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectrum : MonoBehaviour
{
    [Header("Bewegingsinstellingen")]
    public float movementMultiplier = 20f;    // number based on difficulty
    public float smoothSpeed = 0.1f;           // Smoothness of the movement, 1f = instant, 0.1f = very smooth
    public float minSpeed = 9f;               // Minimal speed

    private Vector3 targetPosition;
    private bool isActive = false;

    void Start()
    {
        targetPosition = transform.position;
        isActive = false;
    }

    void Update()
    {
        if (!isActive) return;

        float[] spectrumData = new float[256];
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float totalMovement = 0f;

        for (int i = 0; i < spectrumData.Length; i++)
        {
            float tmp = spectrumData[i] * movementMultiplier;
            if (tmp >= 1f)
            {
                totalMovement += tmp;
            }
        }

        float movement = Mathf.Max(totalMovement * Time.deltaTime, minSpeed * Time.deltaTime);
        targetPosition += transform.forward * movement;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    public void StartDriving(int level)
    {
        // Multiplier based on choosen difficulty
        movementMultiplier = 10f + ((level - 1) * 6f); // Example: Level 1 = 10, Level 2 = 16, Level 3 = 22, etc.

        Debug.Log($"[Spectrum] StartDriving - Level {level}, MovementMultiplier = {movementMultiplier}");

        isActive = true;
    }
}
