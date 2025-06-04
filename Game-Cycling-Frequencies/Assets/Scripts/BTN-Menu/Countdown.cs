using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Countdown : MonoBehaviour
{
    [Tooltip("Aantal seconden dat de sessie duurt (wordt ingesteld door SessionStartManager)")]
    public float sessionDuration = 60f;

    [Tooltip("UI Text element dat de resterende tijd toont")]
    public TMP_Text textBox;

    private float remainingTime;
    private bool isCounting = false;

    public void StartCountdown()
    {
        if (sessionDuration <= 0f)
        {
            Debug.LogWarning("⚠️ sessionDuration is ongeldig, fallback naar 60 seconden.");
            sessionDuration = 60f;
        }

        remainingTime = sessionDuration;
        isCounting = true;

        if (textBox != null)
            textBox.gameObject.SetActive(true);
        else
            Debug.LogError("❌ textBox is niet ingesteld in Countdown.cs");
    }

    void Update()
    {
        if (!isCounting) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (textBox != null)
                textBox.text = Mathf.Ceil(remainingTime).ToString();
        }
        else
        {
            if (textBox != null)
            {
                textBox.text = "Game Over!";
                textBox.color = Color.red;
            }

            isCounting = false;
            StartCoroutine(HideAfterDelay(1f));
        }
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (textBox != null)
            textBox.gameObject.SetActive(false);
    }
}
