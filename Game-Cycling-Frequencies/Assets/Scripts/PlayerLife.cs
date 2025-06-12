using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerLife : MonoBehaviour
{
    public int lives = 3;
    public TextMeshProUGUI lifeText;
    public float bounceDistance = -5f;
    public float bounceDuration = 0.5f;
    private bool isBouncing = false;

    public GameOver gameOverManager;

void Start()
{
    if (gameOverManager == null)
    {
        gameOverManager = FindObjectOfType<GameOver>();
        if (gameOverManager != null)
            Debug.Log("🔗 GameOver manager automatically assigned in Start()");
        else
            Debug.LogWarning("❌ Could not find GameOver in scene!");
    }

    UpdateUI();
}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            lives--;
            UpdateUI();

            if (!isBouncing)
            {
                StartCoroutine(SmoothBounceBack());
            }

            if (lives <= 0)
{
    Debug.Log("💀 Lives reached 0!");

    if (gameOverManager != null)
    {
        Debug.Log("📞 gameOverManager is NOT null, calling OnGameOver()");
        gameOverManager.OnGameOver();
    }
    else
    {
        Debug.LogWarning("❌ gameOverManager is NULL!");
    }
}

        }
    }

    IEnumerator SmoothBounceBack()
    {
        isBouncing = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 start = rb.position;
            Vector3 end = start + new Vector3(0, 0, -bounceDistance);
            float elapsed = 0f;
            rb.linearVelocity = Vector3.zero;

            while (elapsed < bounceDuration)
            {
                rb.MovePosition(Vector3.Lerp(start, end, elapsed / bounceDuration));
                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            rb.MovePosition(end);
        }

        isBouncing = false;
    }

    void UpdateUI()
    {
        if (lifeText != null)
        {
            lifeText.text = lives > 0 ? lives + " lives left" : "Game Over!";
        }
    }
}
