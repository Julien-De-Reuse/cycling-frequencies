using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace

public class XPZoneDetector : MonoBehaviour
{
    private float xpPerSecond = 0f;
    private float xp = 0f;
        public TextMeshProUGUI xpText; // Reference to UI text element

    void Update()
    {
        if (xpPerSecond > 0f)
        {
            xp += xpPerSecond * Time.deltaTime;
            UpdateXPUI();
        }
    }

    void UpdateXPUI()
    {
        if (xpText != null)
        {
            xpText.text = "XP: " + Mathf.FloorToInt(xp).ToString();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        SetXPValue(other);
    }

    void OnTriggerStay(Collider other)
    {
        SetXPValue(other);
    }

    void OnTriggerExit(Collider other)
    {
        xpPerSecond = 0f; // stop earning XP when leaving the zone
    }

    void SetXPValue(Collider other)
    {
        switch (other.tag)
        {
            case "RedZone":
                xpPerSecond = 1f;
                break;
            case "OrangeZone":
                xpPerSecond = 4f;
                break;
            case "GreenZone":
                xpPerSecond = 7f;
                break;
            default:
                xpPerSecond = 0f;
                break;
        }
    }

    public float GetXP()
    {
        return xp;
    }
}
