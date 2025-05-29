using UnityEngine;

public class EnvironmentChoiceStep : MonoBehaviour
{
    public GameObject nextPanel;
    public GameObject previousPanel;

    public void ChooseEnvironment(string environment)
    {
        GameManager.Instance.environment = environment;

        // Scene kiezen op basis van omgeving
        switch (environment)
        {
            case "City":
                GameManager.Instance.targetScene = "SceneCity";
                break;
            case "Nature":
                GameManager.Instance.targetScene = "SceneNature";
                break;
            case "Space":
                GameManager.Instance.targetScene = "SceneSpace";
                break;
            case "Desert":
                GameManager.Instance.targetScene = "SceneDesert";
                break;
            case "Halloween":
                GameManager.Instance.targetScene = "SceneHalloween";
                break;
            default:
                GameManager.Instance.targetScene = ""; // of een fallback scene
                break;
        }

        Debug.Log("Gekozen omgeving: " + environment + " → Scene: " + GameManager.Instance.targetScene);

        gameObject.SetActive(false);
        nextPanel.SetActive(true);
    }
         public void ReturnToPrevious()
    {
        gameObject.SetActive(false);       // Hide this panel
        previousPanel.SetActive(true);     // Show the previous one
    }
}
