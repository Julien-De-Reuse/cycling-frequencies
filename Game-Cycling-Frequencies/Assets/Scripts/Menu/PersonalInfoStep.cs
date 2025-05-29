using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PersonalInfoStep : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Dropdown ageDropdown;
    public TMP_Dropdown genderDropdown;
    public GameObject nextPanel;

    public void SaveAndContinue()
    {
        string name = nameInput.text;

        if (ageDropdown.options.Count == 0)
        {
            Debug.LogError("❌ Age dropdown has no options!");
            return;
        }
        string ageRange = ageDropdown.options[ageDropdown.value].text;

        if (genderDropdown.options.Count == 0)
        {
            Debug.LogError("❌ Gender dropdown has no options!");
            return;
        }
        string gender = genderDropdown.options[genderDropdown.value].text;

        GameManager.Instance.userName = name;
        GameManager.Instance.ageRange = ageRange;
        GameManager.Instance.gender = gender;

        Debug.Log($"Naam: {name}, Leeftijdsbereik: {ageRange}, Geslacht: {gender}");

        gameObject.SetActive(false);
        nextPanel.SetActive(true);
    }

    public void ReturnToScene()
    {
    Debug.Log($"Terug naar scene: {"StartScene"}");
        SceneManager.LoadScene("StartScene");
    }
}
