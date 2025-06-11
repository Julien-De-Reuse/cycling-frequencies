using UnityEngine;
using TMPro;

public class PianoResultUI : MonoBehaviour
{
    public TMP_Text xpText;

    public void ShowResults()
    {
        if (xpText != null)
        {
            int xp = PianoXPManager.Instance.GetXP();
            xpText.text = "Je score: " + xp.ToString() + " XP";
        }
    }
}
