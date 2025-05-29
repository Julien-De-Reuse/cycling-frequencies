using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class LevelChoiceStep : MonoBehaviour

{
    public GameObject nextPanel;
    public GameObject previousPanel;

    [Header("Label boven de knoppen")]
    public TMP_Text[] levelLabels; // Sleep hier Label_Level1 t.e.m. Label_Level5 in

    void OnEnable()
    {
        int suggested = CalculateSuggestedLevel();
        GameManager.Instance.idealLevel = suggested;

        // Zet alleen bij de juiste knop 'Aanbevolen'
        for (int i = 0; i < levelLabels.Length; i++)
        {
            if (i == suggested - 1)
                levelLabels[i].text = "recomended";
            else
                levelLabels[i].text = "";
        }
    }

    int CalculateSuggestedLevel()
    {
        float bmi = GameManager.Instance.BMI;
        int sportDays = GameManager.Instance.sportDays;
        string ageRange = GameManager.Instance.ageRange;


        float baseLevel = sportDays * 1.2f;

        // Bonus voor gezonde BMI
        if (bmi >= 18.5f && bmi <= 25f)
            baseLevel += 1f;

        // Leeftijdscorrectie op basis van ageRange
        if (ageRange == "51-60" || ageRange == "+60")
            baseLevel -= 2f;
        else if (ageRange == "36-50" )
            baseLevel -= 1f;

        int level = Mathf.Clamp(Mathf.RoundToInt(baseLevel), 1, 5);

        return level;
    }


    public void SelectLevel(int level)
    {
        GameManager.Instance.idealLevel = level;

        string sceneToLoad = GameManager.Instance.targetScene;

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("Level gekozen: " + level + " → Laden van scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Geen geldige scene geselecteerd! Zorg ervoor dat EnvironmentChoiceStep de juiste targetScene instelt.");
        }
    }
         public void ReturnToPrevious()
    {
        gameObject.SetActive(false);       // Hide this panel
        previousPanel.SetActive(true);     // Show the previous one
    }
}
