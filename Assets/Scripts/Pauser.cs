using UnityEngine;

public class Pauser : MonoBehaviour {
    public static void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
