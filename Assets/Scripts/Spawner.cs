using UnityEngine;

public class Spawner : MonoBehaviour {
    public const string UnityTag = "Respawn";

    void Start()
    {
        if (!tag.Equals(UnityTag)) tag = UnityTag;
    }

	public Unit Spawn (GameObject unitPrefab)
	{
	    var unit = Instantiate(unitPrefab, transform.position, unitPrefab.transform.rotation) as GameObject;
	    
        return unit != null ? unit.GetComponent<Unit>() : null;
	}
}
