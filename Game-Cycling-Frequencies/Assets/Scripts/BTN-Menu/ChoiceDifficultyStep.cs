using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChoiceDifficultyStep : MonoBehaviour
{
    [Header("Label boven de knoppen")]
    public TMP_Text[] levelLabels;

    void OnEnable()
    {
        int suggested = CalculateSuggestedLevel();
        NEWGameManager.Instance.spectrumRideData.idealLevel = suggested;

        for (int i = 0; i < levelLabels.Length; i++)
        {
            levelLabels[i].text = (i == suggested - 1) ? "recommended" : "";
        }
    }

    int CalculateSuggestedLevel()
    {
        var data = NEWGameManager.Instance.spectrumRideData;

        float weight = data.weightRange;
        float height = data.heightRange / 100f; // cm → meter
        float bmi = weight / (height * height);
        int sportDays = data.sportDays;
        string ageRange = data.ageRange;

        float baseLevel = sportDays * 1.2f;

        if (bmi >= 18.5f && bmi <= 25f)
            baseLevel += 1f;

        if (ageRange == "51-65" || ageRange == "+65")
            baseLevel -= 2f;
        else if (ageRange == "36-50")
            baseLevel -= 1f;

        int level = Mathf.Clamp(Mathf.RoundToInt(baseLevel), 1, 5);
        return level;
    }

    public void SelectLevel(int level)
{
    NEWGameManager.Instance.spectrumRideData.idealLevel = level;

    Debug.Log("📌 Moeilijkheid geselecteerd: " + level);

    // De scene mag pas geladen worden in MenuController → StepToScene
    // SceneManager.LoadScene(...) wordt dus NIET hier uitgevoerd
}

}
