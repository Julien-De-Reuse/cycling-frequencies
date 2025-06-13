using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectrum : MonoBehaviour
{
    [Header("Bewegingsinstellingen")]
    public float movementMultiplier = 20f;     // Wordt aangepast via level
    public float smoothSpeed = 10f;
    public float minSpeed = 6f;               // Minimale snelheid

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

    /// <summary>
    /// Activeert beweging en past de snelheid aan volgens het difficulty level.
    /// Deze methode wordt aangeroepen door de SceneDifficultyManager.
    /// </summary>
    /// <param name="level">De difficulty level (1 t.e.m. 5)</param>
    public void StartDriving(int level)
    {
        // Pas multiplier aan op basis van gekozen difficulty
        // Voorbeeld: 20 → 50 → 80 → 110 → 140
        movementMultiplier = 10f + ((level - 1) * 30f);

        Debug.Log($"[Spectrum] StartDriving - Level {level}, MovementMultiplier = {movementMultiplier}");

        isActive = true;
    }
}
