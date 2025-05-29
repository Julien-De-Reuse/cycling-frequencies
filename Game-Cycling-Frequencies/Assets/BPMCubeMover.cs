using UnityEngine;

public class BPMCubeMover : MonoBehaviour
{
    public float bpm = 120f;
    public float speedMultiplier = 1f;
    public AudioSource audioSource; // Reference to the Audio Source

    private float beatsPerSecond;

    void Start()
    {
        beatsPerSecond = bpm / 60f;
    }

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            float speed = beatsPerSecond * speedMultiplier;
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
