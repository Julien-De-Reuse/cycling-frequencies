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
        {
            music.Stop();
            Debug.Log("🔇 music.Stop() uitgevoerd bij Start()");
        }
        else
        {
            Debug.LogError("❌ music (AudioSource) is niet toegewezen!");
        }

        if (car == null)
        {
            Debug.LogError("❌ car (Spectrum) is niet toegewezen!");
        }
    }

    public void OnFirstPedal()
    {
        if (!hasStarted)
        {
            Debug.Log("🚴‍♂️ Eerste trappuls gedetecteerd. Start countdown...");
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
        if (countdownText == null)
        {
            Debug.LogError("❌ countdownText is niet toegewezen!");
            yield break;
        }

        countdownText.gameObject.SetActive(true);

        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();
            Debug.Log("⏳ Countdown: " + i);
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);

        if (popupPanel != null)
            popupPanel.SetActive(false);

        Debug.Log("✅ Countdown klaar. Start nu sessie...");

        // Bepaal level op basis van de gekozen mod
        int levelToUse = 1; // default fallback
        var manager = NEWGameManager.Instance;

        if (manager != null)
        {
            switch (manager.currentMod)
            {
                case NEWGameManager.ModType.SpectrumRide:
                    levelToUse = manager.spectrumRideData.idealLevel;
                    Debug.Log("🧠 Mod: SpectrumRide - idealLevel = " + levelToUse);
                    break;

                case NEWGameManager.ModType.Overdrive:
                    levelToUse = 2; // pas dit aan indien nodig
                    Debug.Log("🧠 Mod: Overdrive - gebruikt level 2");
                    break;

                case NEWGameManager.ModType.CruiseControl:
                    levelToUse = 1; // of afleiden uit speed als je wil
                    Debug.Log("🧠 Mod: CruiseControl - gebruikt level 1");
                    break;

                default:
                    Debug.LogWarning("⚠️ Geen geldige ModType gevonden — fallback level");
                    break;
            }
        }
        else
        {
            Debug.LogError("❌ NEWGameManager.Instance is null!");
        }

        // 🚗 Start de auto
        if (car != null)
        {
            car.StartDriving(levelToUse);
            Debug.Log("🚗 car.StartDriving() aangeroepen met level: " + levelToUse);
        }
        else
        {
            Debug.LogError("❌ car is null");
        }

        // 🎵 Muziek laden uit Resources
        if (music != null)
        {
            string musicName = NEWGameManager.Instance?.selectedMusicName;

            if (!string.IsNullOrEmpty(musicName))
            {
                AudioClip clip = Resources.Load<AudioClip>("Music/" + musicName);
                if (clip != null)
                {
                    music.clip = clip;
                    music.Play();
                    Debug.Log("🎵 music.Play() aangeroepen op clip: " + musicName);
                }
                else
                {
                    Debug.LogError("❌ AudioClip niet gevonden in Resources/Music/: " + musicName);
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Geen muziek gekozen in NEWGameManager.selectedMusicName");
            }
        }
        else
        {
            Debug.LogError("❌ music is null, AudioSource niet ingesteld.");
        }

        // 🎥 Start camera volgen
        var cam = FindObjectOfType<CameraFollower>();
        if (cam != null)
        {
            cam.EnableCameraFollow();
            Debug.Log("🎥 CameraFollower.EnableCameraFollow() aangeroepen");
        }
        else
        {
            Debug.LogError("❌ Geen CameraFollower gevonden in scene!");
        }
    }
}
