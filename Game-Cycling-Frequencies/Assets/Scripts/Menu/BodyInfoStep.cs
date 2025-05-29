using UnityEngine;
using TMPro;

public class BodyInfoStep : MonoBehaviour
{
    public TMP_InputField weightInput;
    public TMP_InputField heightInput;
    public TMP_Text bmiDisplayText;
    public GameObject nextPanel;

    public GameObject previousPanel;
    public void SaveAndContinue()
    {
        float weight = float.Parse(weightInput.text);
        float heightCm = float.Parse(heightInput.text);
        float heightM = heightCm / 100f;

        float bmi = weight / (heightM * heightM);

        GameManager.Instance.weight = weight;
        GameManager.Instance.height = heightCm;
        GameManager.Instance.BMI = bmi;

        bmiDisplayText.text = "Je BMI is: " + bmi.ToString("F1");

        Debug.Log($"Gewicht: {weight}, Lengte: {heightCm}, BMI: {bmi}");

        // Ga naar volgende stap
        gameObject.SetActive(false);
        nextPanel.SetActive(true);
    }
     public void ReturnToPrevious()
    {
        gameObject.SetActive(false);       // Hide this panel
        previousPanel.SetActive(true);     // Show the previous one
    }
}
