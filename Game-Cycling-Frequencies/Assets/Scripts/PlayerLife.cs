using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoBehaviour
{
    public int lives = 3;
    public TextMeshProUGUI lifeText;

    void Start()
    {
        UpdateUI();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer ==LayerMask.NameToLayer("Obstacle"))
        {
            lives--;
            UpdateUI();
            if (lives <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
    }

    void UpdateUI()
    {
        lifeText.text = lives + "lives left";
        if (lives <= 0)
        {
            lifeText.text = "Game Over!";
        }
    }
}
