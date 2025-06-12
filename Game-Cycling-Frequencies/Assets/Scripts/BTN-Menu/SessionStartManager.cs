using UnityEngine;
using TMPro;
using System.Collections;

public class SessionStartManager : MonoBehaviour
{
    public GameObject popupPanel;

    [Tooltip("Countdown voor de start van de sessie (5 → GO!)")]
    public TMP_Text countdownText;

    [Tooltip("Sessie-timer na de start (tekst wordt getoond via Countdown.cs)")]
    public Countdown countdown;

    public Spectrum car;
    public AudioSource music;

    private bool hasStarted = false;
    public bool sessionActive = false;

    void Start()
    {
        Debug.Log("📦 SessionStartManager gestart");

        if (popupPanel != null)
            popupPanel.SetActive(true);
        else
            Debug.LogError("❌ popupPanel is niet toegewezen!");

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
        else
            Debug.LogError("❌ countdownText is niet toegewezen!");

        if (music != null)
            music.Stop();
        else
            Debug.LogError("❌ music (AudioSource) is niet toegewezen!");

        if (car == null)
            Debug.LogError("❌ car (Spectrum) is niet toegewezen!");

        if (countdown == null)
            Debug.LogError("❌ countdown is niet toegewezen!");
    }

    public void OnFirstPedal()
    {
        if (!hasStarted)
        {
            Debug.Log("🚴 Eerste trappuls gedetecteerd. Start countdown...");
            hasStarted = true;
            StartCoroutine(CountdownAndStart());
        }
        else
        {
            Debug.Log("⚠️ Trappuls genegeerd, sessie is al gestart.");
        }
    }

    IEnumerator CountdownAndStart()
    {
        // Visuele pre-start countdown
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            Debug.Log("⏳ Countdown: " + i);
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        if (popupPanel != null)
            popupPanel.SetActive(false);

        Debug.Log("✅ Countdown klaar. Start sessie...");

        // 🔁 Bepaal sessieduur op basis van de gekozen Mod
        int duration = 60; // fallback
        int levelToUse = 1;
        var manager = NEWGameManager.Instance;

        if (manager != null)
        {
            switch (manager.currentMod)
            {
                case NEWGameManager.ModType.SpectrumRide:
                    duration = manager.spectrumRideData.selectedSessionDuration;
                    levelToUse = manager.spectrumRideData.idealLevel;
                    break;

                case NEWGameManager.ModType.CruiseControl:
                    duration = manager.cruiseControlData.selectedSessionDuration;
                    levelToUse = 1; // Of afleiden uit snelheid als gewenst
                    break;

                case NEWGameManager.ModType.Overdrive:
                    // Voorlopig geen sessieduur of level logica
                    duration = 60;
                    levelToUse = 2;
                    break;
            }
        }

        // 🚗 Start de auto
        if (car != null)
        {
            car.StartDriving(levelToUse);
        }

        // 🎵 Start de muziek
        if (music != null && manager != null)
        {
            string musicName = manager.selectedMusicName;

            if (!string.IsNullOrEmpty(musicName))
            {
                AudioClip clip = Resources.Load<AudioClip>("Music/" + musicName);
                if (clip != null)
                {
                    music.clip = clip;
                    music.Play();
                    Debug.Log("🎵 Muziek gestart: " + musicName);
                }
                else
                {
                    Debug.LogError("❌ AudioClip niet gevonden in Resources/Music/: " + musicName);
                }
            }
        }

        // 🎥 Zet camera aan
        var cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
        {
            cam.EnableCameraFollow();
        }

        // 🕐 Start de sessie countdown
        if (countdown != null)
        {
            countdown.sessionDuration = duration;
            countdown.StartCountdown();
        }

        GameStatsManager.Instance.StartSessionStats();
    }

    public bool SessionStarted => hasStarted;

    void Update()
    {
        if (!sessionActive) return;

        // Only update stats/timer/XP here!
    }
}
