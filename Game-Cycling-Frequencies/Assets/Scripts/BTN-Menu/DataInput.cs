using UnityEngine;

public class DataInput : MonoBehaviour
{
    public void SetAgeRange(string ageRange)
    {
        NEWGameManager.Instance.spectrumRideData.ageRange = ageRange;
        Debug.Log("📌 Age range set to: " + ageRange);
    }

    public void SetWeight(float weight)
    {
        NEWGameManager.Instance.spectrumRideData.weightRange = weight;
        Debug.Log("📌 Weight set to: " + weight);
    }

    public void SetHeight(float height)
    {
        NEWGameManager.Instance.spectrumRideData.heightRange = height;
        Debug.Log("📌 Height set to: " + height);
    }

    public void SetSportDays(int days)
    {
        NEWGameManager.Instance.spectrumRideData.sportDays = days;
        Debug.Log("📌 Sport days set to: " + days);
    }
}
