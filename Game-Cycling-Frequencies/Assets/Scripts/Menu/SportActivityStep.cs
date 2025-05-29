using UnityEngine;

public class SportActivityStep : MonoBehaviour
{
    public GameObject nextPanel;
    public GameObject previousPanel;

    public void SetSportLevel(int level)
    {
        GameManager.Instance.sportDays = level;
        Debug.Log("Sportactiviteit: " + level + " dagen/week");

        gameObject.SetActive(false);
        nextPanel.SetActive(true);
    }
         public void ReturnToPrevious()
    {
        gameObject.SetActive(false);       // Hide this panel
        previousPanel.SetActive(true);     // Show the previous one
    }
}
