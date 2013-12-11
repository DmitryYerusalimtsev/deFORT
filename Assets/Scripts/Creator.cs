using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject UnitPrefab;
    public GameObject BattleLine;

	private string leftSpeed = "5";
	private string rightSpeed = "5";
	
	private string leftAttackDistance = "5";
	private string rightAttackDistance = "5";
	
	private string leftDamage = "5";
	private string rightDamage = "5";

    private string leftReload = "5";
    private string rightReload = "5";

    private string leftHealth = "100";
    private string rightHealth = "100";

    private float lastCreate = 0f;
    private bool paused = false;

    private string pauseButtonName = "Pause";
    private Scores scores;

    void Update()
    {
        lastCreate += Time.deltaTime;
        scores = FindObjectOfType<Scores>();
    }

    void OnGUI()
    {
		
		// Control buttons.
		GUILayout.BeginHorizontal();
		GUILayout.Label("Width: " + Vector3.Distance(RightSpawn.transform.position, LeftSpawn.transform.position).ToString() + " тугриков");
        GUILayout.Label("Default Value: 5");
        GUILayout.Label("Total Units: " + scores.TotalUnits.ToString());
		GUILayout.Label("Time: " + ((int)Time.realtimeSinceStartup).ToString());
        GUILayout.Label("Last Create: " + ((int)lastCreate).ToString());
		GUILayout.EndHorizontal();

		// Left side units.
		GUILayout.BeginHorizontal();
		GUILayout.Label("Left unit   :");

		// Speed params.
		leftSpeed = DrawIssue("Speed(тугриков/sec)", leftSpeed);
		leftAttackDistance = DrawIssue("Attack Distance", leftAttackDistance);
        leftDamage = DrawIssue("Damage", leftDamage);
        leftReload = DrawIssue("Reload Time", leftReload);
        leftHealth = DrawIssue("Health", leftHealth);

		if (GUILayout.Button("Create"))
		{
			CreateUnit(true);
		}

		GUILayout.EndHorizontal();

		// Right side units.
		GUILayout.BeginHorizontal();
		GUILayout.Label("Right unit :");
		// Speed params.
		rightSpeed = DrawIssue("Speed (тугриков/sec)", rightSpeed);
		rightAttackDistance = DrawIssue("Attack Distance", rightAttackDistance);
		rightDamage = DrawIssue("Damage", rightDamage);
        rightReload = DrawIssue("Reload Time", rightReload);
        rightHealth = DrawIssue("Health", rightHealth);
		
		if (GUILayout.Button("Create"))
		{
			CreateUnit(false);
		}
		GUILayout.EndHorizontal();

		// Control buttons.
        GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Destroy all"))
		{
			DestroyUnits();
        }

        if (GUILayout.Button(pauseButtonName))
        {
            paused = !paused;
            Time.timeScale = paused ? 0 : 1;
            pauseButtonName = paused ? "Resume" : "Pause";
        }

		if (GUILayout.Button("Create both"))
		{
			CreateUnit(true);
			CreateUnit(false);
        }

        GUILayout.EndHorizontal();
    }

	string DrawIssue(string issue, string val)
	{
		GUILayout.BeginVertical();
		GUILayout.Label(issue);
		val = GUILayout.TextField(val);
		GUILayout.EndVertical();

		return val;
	}

	void CreateUnit(bool left)
	{
		Unit unit = InstantiateUnit(left);
		if (left)
		{
			unit.Speed = GetValidFloatValue(leftSpeed);
			unit.Damage = GetValidFloatValue(leftDamage);
			unit.Distance = GetValidFloatValue(leftAttackDistance);
            unit.ReloadTime = GetValidFloatValue(leftReload);
            unit.MaxHealth = GetValidFloatValue(leftHealth);
		    unit.Health = GetValidFloatValue(leftHealth);
		    unit.DefatultTarget = RightSpawn;
		    unit.Side = 1;
		    ++scores.LeftUnitCount;
		}
		else
		{
			unit.Speed = GetValidFloatValue(rightSpeed);
			unit.Damage = GetValidFloatValue(rightDamage);
            unit.Distance = GetValidFloatValue(rightAttackDistance);
            unit.ReloadTime = GetValidFloatValue(rightReload);
            unit.MaxHealth = GetValidFloatValue(rightHealth);
            unit.Health = GetValidFloatValue(rightHealth);
		    unit.DefatultTarget = LeftSpawn;
            unit.Side = 2;
            ++scores.RightUnitCount;
		}
	    unit.BattleLine = BattleLine.GetComponent<Line>();
	    lastCreate = 0f;
	}

    private Unit InstantiateUnit(bool left)
    {

        if (left)
        {
            return BattleLine.GetComponent<Line>().SpawnOnLeftSide(UnitPrefab);
        }
        else
        {
            return BattleLine.GetComponent<Line>().SpawnOnRightSide(UnitPrefab);
        }
    }

	float GetValidFloatValue(string s)
	{
		float res;
		if (!float.TryParse(s, out res)) res = 5;
		return res;
	}

	void DestroyUnits()
	{
        foreach (var unit in GameObject.FindGameObjectsWithTag(Unit.UnityTag))
        {
            unit.GetComponent<Unit>().DestroyUnit();
        }

	    scores.LeftUnitCount = 0;
	    scores.RightUnitCount = 0;
	}

    public GameObject LeftSpawn
    {
        get { return (GameObject) BattleLine.GetComponentsInChildren<Spawner>()[0].gameObject; }
    }

    public GameObject RightSpawn
    {
        get { return (GameObject)BattleLine.GetComponentsInChildren<Spawner>()[1].gameObject; }
    }
}
