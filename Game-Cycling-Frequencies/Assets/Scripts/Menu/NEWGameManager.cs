using UnityEngine;

public class NEWGameManager : MonoBehaviour
{
    public static NEWGameManager Instance;

    public enum ModType { None, SpectrumRide, Overdrive, CruiseControl }
    public ModType currentMod = ModType.None;
public string selectedMusicName; // Algemene muziekkeuze

    // Gegevens voor SpectrumRide
    [System.Serializable]
    public class SpectrumRideData
    {
        public string ageRange;
        public float weightRange;
        public float heightRange;
        public int sportDays;

        public string environment;
        public string difficulty;
        public string gameTime;
        public int idealLevel;
        public int[] suggestedLevels = new int[5];
    }

    public SpectrumRideData spectrumRideData = new SpectrumRideData();

    // Gegevens voor Overdrive
    [System.Serializable]
    public class OverdriveData
    {

        public string environment;
    }

    public OverdriveData overdriveData = new OverdriveData();

    // Gegevens voor CruiseControl
    [System.Serializable]
    public class CruiseControlData
    {

        public string environment;
        public string speed;
    }

    public CruiseControlData cruiseControlData = new CruiseControlData();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentMod(ModType selectedMod)
    {
        currentMod = selectedMod;
    }
}
