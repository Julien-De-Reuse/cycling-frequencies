using UnityEngine;

public class PianoGameManager : MonoBehaviour
{
    public static PianoGameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartPianoSession()
    {
        Debug.Log("🎬 Piano sessie gestart");

        PianoXPManager.Instance.ResetXP();

        CameraFollower cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
        {
            cam.EnableCameraFollow();
        }
        else
        {
            Debug.LogError("❌ CameraFollower script niet gevonden!");
        }
    }

    public void EndPianoSession()
    {
        CameraFollower cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
        {
            cam.DisableCameraFollow();
        }
    }
}
