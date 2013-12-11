using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	
	public float MaxHealth = 100.0f;				// Максимальное здоровье
	public float RegenerateSpeed = 0.0f;			// Регенерация за ед. времени
	
	public bool Invincible = false;					// Режим Бога
	public bool Dead = false;						// Жив ли юнит
	
	public float LastDamageTime = 0.0f;				// Время последнего урона (для регена)
    private float currentHealth = 100f;
	
	// Use this for initialization
	private void Awake() {
        if (MaxHealth < 1) MaxHealth = 1;           // Если максимальное хп задано менее единицы - ставим единицу
	}
	
	public void OnDamage (float amount) {
		// Take no damage if invincible, dead, or if the damage is zero
		if(Invincible)
			return;
		if (Dead)
			return;
		if (amount <= 0)
			return;
	
		// Decrease health by damage and send damage signals
	
		CurrentHealth -= amount;
		LastDamageTime = Time.time;
	
		// Enable so the Update function will be called
		// if regeneration is enabled
		if (RegenerateSpeed > 0)
			enabled = true;
	
		// Show damage effect if there is one
	
		// Die if no health left
        if (CurrentHealth <= 0)
		{
			// TODO: Game.RegisterDeath(gameObject);
	
			CurrentHealth = 0;
			Dead = true;
			enabled = false;
		}
	}

    public void UpdateHealthBar()
    {
        var healthBar = transform.transform.Find("health-bar");

        if (healthBar == null) return;
        var sprite = healthBar.GetChild(0).GetComponent<SpriteRenderer>();

        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        sprite.material.color = Color.Lerp(Color.green, Color.red, 1 - CurrentHealthPercent);

        // Set the scale of the health bar to be proportional to the player's health.
        sprite.transform.localScale = new Vector3(CurrentHealthPercent, 1, 1);
    }
	
	private void OnEnable()
	{
		StartCoroutine(Regenerate());
	}

    private IEnumerator Regenerate()
    {
        if (RegenerateSpeed > 0.0f)
        {
            while (enabled)
            {
                if (Time.time > LastDamageTime + 3)
                {
                    CurrentHealth += RegenerateSpeed;

                    yield return new WaitForEndOfFrame();

                    if (CurrentHealth >= MaxHealth)
                    {
                        CurrentHealth = MaxHealth;
                        enabled = false;
                    }
                }
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public float CurrentHealthPercent
    {
        get { return CurrentHealth / MaxHealth; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; UpdateHealthBar(); }
    }

}
