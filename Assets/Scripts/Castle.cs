using UnityEngine;

public class Castle : MonoBehaviour {

    public int Side
    {
        get { return transform.position.x < 0 ? Scores.LeftSideNumber : Scores.RightSideNumber; }
    }

    public float Health
    {
        get { return (float) GetComponentInChildren<Health>().CurrentHealth; }
    }

    public void Restart()
    {
        var health = GetComponentInChildren<Health>();
        health.CurrentHealth = health.MaxHealth;
        health.Dead = false;
    }
}
