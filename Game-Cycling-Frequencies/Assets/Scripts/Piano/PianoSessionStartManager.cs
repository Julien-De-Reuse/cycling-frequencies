using UnityEngine;

public class PianoSessionStartManager : MonoBehaviour
{
    private bool hasStarted = false;

    public void OnFirstPedal(float speed)
    {
        if (!hasStarted && speed > 1f)
        {
            hasStarted = true;
            Debug.Log("👟 Eerste trappuls gedetecteerd voor Piano → sessie starten");

            PianoGameManager.Instance.StartPianoSession();
        }
    }
    public void OnSessionEnd()
    {
        Debug.Log("🎹 Piano sessie beëindigd");
        PianoGameManager.Instance.EndPianoSession();
    }
}
        