using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    private Scores scores;

    void Start()
    {
        scores = FindObjectOfType<Scores>();
    }

    public void CreateLeftBuilding(GameObject buildingPrefab, int slot)
    {
        var cost = buildingPrefab.GetComponent<UnitBuilding>().ManaCost;

        if (scores.LeftManaCount < cost) return;

        var o = Instantiate(buildingPrefab) as GameObject;
        var build = o.GetComponent<UnitBuilding>();

        build.Side = Scores.LeftSideNumber;
        scores.leftBuildings[slot] = build;
        scores.LeftManaCount -= cost;
        
    }

    public void CreateRightBuilding(GameObject buildingPrefab, int slot)
    {
        var cost = buildingPrefab.GetComponent<UnitBuilding>().ManaCost;

        if (scores.RightManaCount < cost) return;

        var o = Instantiate(buildingPrefab) as GameObject;
        var build = o.GetComponent<UnitBuilding>();


        build.Side = Scores.RightSideNumber;
        scores.rightBuildings[slot] = build;
        scores.RightManaCount -= cost;
    }

}
