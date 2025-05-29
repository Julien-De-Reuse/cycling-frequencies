using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

//Q1
    public string userName;
    public string ageRange;

    public string gender;

//Q2
    public float weight;
    public float height;
    public float BMI;

//Q3
    public int sportDays;

//Q4
    public string musicGenre;
    public string environment;
    public string targetScene;


    // Levels
    public int idealLevel;
    public int[] suggestedLevels = new int[5];

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
}
