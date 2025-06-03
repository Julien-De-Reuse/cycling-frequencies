using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{
    public NEWGameManager.ModType modType;
    public string targetScene;

    public void Confirm()
    {
        NEWGameManager.Instance.SetCurrentMod(modType);
        SceneManager.LoadScene(targetScene);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
