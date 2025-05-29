using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectrum : MonoBehaviour
{
    public float movementMultiplier = 20f; // Wordt aangepast via level
    public float smoothSpeed = 10f;
    public float minSpeed = 10f; // ➕ Minimum snelheid
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

        // ➕ Zorg dat je altijd minstens minSpeed meeneemt
        float movement = Mathf.Max(totalMovement * Time.deltaTime, minSpeed * Time.deltaTime);
        targetPosition += transform.forward * movement;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    // Wordt aangeroepen door SessionStartManager
    public void StartDriving(int level)
    {
        movementMultiplier = 10f + (level * 5f); // pas snelheid aan op basis van level
        isActive = true;
    }
}
