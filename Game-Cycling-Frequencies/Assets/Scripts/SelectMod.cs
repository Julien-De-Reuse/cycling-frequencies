using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMod : MonoBehaviour{
    public void SelectSpectrumRide()
    {
        NEWGameManager.Instance.SetCurrentMod(NEWGameManager.ModType.SpectrumRide);
        SceneManager.LoadScene("MenuSpectrumRideScene"); // laad menu scene van deze mod
    }

    public void SelectOverdrive()
    {
        NEWGameManager.Instance.SetCurrentMod(NEWGameManager.ModType.Overdrive);
        SceneManager.LoadScene("MenuOverdriveScene");
    }

    public void SelectCruiseControl()
    {
        NEWGameManager.Instance.SetCurrentMod(NEWGameManager.ModType.CruiseControl);
        SceneManager.LoadScene("MenuCruiseControlScene");
    }
}
