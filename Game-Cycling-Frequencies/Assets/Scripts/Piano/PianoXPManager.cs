using UnityEngine;

public class PianoXPManager : MonoBehaviour
{
    public static PianoXPManager Instance;

    private int currentXP = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("🎵 XP toegevoegd: " + amount + " → Totaal XP: " + currentXP);
    }

    public void ResetXP()
    {
        currentXP = 0;
    }

    public int GetXP()
    {
        return currentXP;
    }
}
