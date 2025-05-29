using UnityEngine;
using TMPro;
using System.Collections;

public class SessionStartManager : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text countdownText;
    public Spectrum car;
    public AudioSource music;

    private bool hasStarted = false;

    void Start()
    {
        popupPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
        music.Stop();
    }

    public void OnFirstPedal()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(CountdownAndStart());
        }
    }

    IEnumerator CountdownAndStart()
{
    countdownText.gameObject.SetActive(true);

    for (int i = 5; i > 0; i--)
    {
        countdownText.text = i.ToString();
        yield return new WaitForSeconds(1f);
    }

    countdownText.gameObject.SetActive(false);
    popupPanel.SetActive(false);

    Debug.Log("✅ Countdown klaar. Start nu sessie...");

    car.StartDriving(GameManager.Instance.idealLevel);
    Debug.Log("🚗 car.StartDriving() aangeroepen");

    music.Play();
    Debug.Log("🎵 music.Play() aangeroepen met clip: " + music.clip?.name);

    FindObjectOfType<CameraFollower>().EnableCameraFollow();
    Debug.Log("🎥 CameraFollower.EnableCameraFollow() aangeroepen");
}

}
