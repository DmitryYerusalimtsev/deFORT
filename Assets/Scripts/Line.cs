using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public const string UnityTag = "Line";

    public GameObject LeftSpawn;
    public GameObject RightSpawn;

    public int Id { get; set; }

    private List<Unit> units = new List<Unit>(); 

	// Use this for initialization
	void Start () {
        if (!tag.Equals(UnityTag)) tag = UnityTag;
	}

    public Unit Spawn(GameObject unitPrefab, int side)
    {
        Spawner spawner = side == Scores.LeftSideNumber ? LeftSpawn.GetComponent<Spawner>() : RightSpawn.GetComponent<Spawner>();
        Unit unit = spawner.Spawn(unitPrefab);

        unit.Side = side;
        unit.BattleLine = this;
        units.Add(unit);

        return unit;
    }

    public Unit SpawnOnLeftSide(GameObject unitPrefab)
    {
        return Spawn(unitPrefab, Scores.LeftSideNumber);
    }

    public Unit SpawnOnRightSide(GameObject unitPrefab)
    {
        return Spawn(unitPrefab, Scores.RightSideNumber);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public IList<Unit> Units
    {
        get { return units; }
    }
}
