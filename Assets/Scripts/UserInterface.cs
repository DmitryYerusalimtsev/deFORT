using System;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    private Scores scores;
    public GameObject[] Buildings;
	public GUISkin skin;

    private bool drawMenu;
    private int selectedSlot = -1;
    private bool leftSideBuilding;
    private bool drawLineManager;
    private Controller controller;

    private void Start()
    {
        scores = FindObjectOfType<Scores>();
        controller = FindObjectOfType<Controller>();
    }

    void OnGUI()
    {
		GUI.skin = skin;

        if (scores.GameOver)
        {
            GUILayout.Label("GameOver");
            if (GUILayout.Button("Restart"))
            {
                Restart();
            }
            return;
        }
        DrawPlayerStats("Left", scores.LeftManaCount, scores.LeftScorePoints, scores.LeftBuildingCount);
        DrawPlayerStats("Right", scores.RightManaCount, scores.RightScorePoints, scores.RightBuildingCount);

        GUILayout.BeginHorizontal();
        DrawBuildingSlots(true);

        GUILayout.Label("---");
        DrawBuildingSlots(false);

        GUILayout.EndHorizontal();
        if (drawMenu) DrawBuildingShop();
    }

    private void DrawBuildingShop()
    {
        GUILayout.BeginHorizontal();
        foreach (var building in Buildings)
        {
            if (GUILayout.Button(building.GetComponent<UnitBuilding>().Texture))
            {
                CreateBuilding(building, leftSideBuilding, selectedSlot);
                drawMenu = false;
                selectedSlot = -1;
            }
        }
        GUILayout.EndHorizontal();
    }

    private void DrawBuildingSlots(bool leftSide)
    {
        var array = leftSide ? scores.leftBuildings : scores.rightBuildings;

        for (int i = 0; i < array.Count; i++)
        {
            if (array[i] == null)
            {
                if (GUILayout.Button("Buy"))
                {
                    drawLineManager = false;
                    if (selectedSlot == i && leftSideBuilding == leftSide)
                    {
                        drawMenu = false;
                        selectedSlot = -1;
                    }
                    else
                    {
                        drawMenu = true;
                        selectedSlot = i;
                        leftSideBuilding = leftSide;
                    }
                }
            }
            else
            {
                if (GUILayout.Button(array[i].Texture))
                {
                    drawMenu = false;
                    if (selectedSlot == i && leftSideBuilding == leftSide)
                    {
                        drawLineManager = false;
                        selectedSlot = -1;
                    }
                    else
                    {
                        drawLineManager = true;
                        selectedSlot = i;
                        leftSideBuilding = leftSide;
                    }
                }
                
                if (selectedSlot == i && leftSide == leftSideBuilding && drawLineManager)
                {
                    GUILayout.BeginVertical();
                    DrawLineManager(array[i]);
                    GUILayout.EndVertical();
                }
            }
        }
    }

    private void DrawPlayerStats(string name, int mana, int points, int buildings)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(String.Format("{0}: ", name));
        GUILayout.Label("Mana: " + mana);
        GUILayout.Label("Points: " + points);
        GUILayout.Label("Buidings: " + buildings);

        GUILayout.EndHorizontal();
    }

    private void CreateBuilding(GameObject buildingPrefab, bool left, int slot)
    {
        if (left)
        {
            controller.CreateLeftBuilding(buildingPrefab, slot);
        }
        else
        {
            controller.CreateRightBuilding(buildingPrefab, slot);
        }
    }

    private void Restart()
    {
        drawMenu = false;
        drawLineManager = false;
        selectedSlot = -1;

        for (int i = 0; i < scores.LeftBuildingSlots; i++)
        {
            if (scores.leftBuildings[i] != null)
            Destroy(scores.leftBuildings[i].gameObject);
        }

        for (int i = 0; i < scores.RightBuildingSlots; i++)
        {
            if (scores.rightBuildings[i] != null)
            Destroy(scores.rightBuildings[i].gameObject);
        }

        foreach (var unit in FindObjectsOfType<Unit>())
        {
            unit.DestroyUnit();
        }

        foreach (var castle in FindObjectsOfType<Castle>())
        {
            castle.Restart();
        }

        scores.LeftManaCount = scores.RightManaCount = Scores.ManaCount;
        scores.LeftScorePoints = scores.RightScorePoints = 0;
        scores.GameOver = false;
        Pauser.Pause(false);
    }

    private void DrawLineManager(UnitBuilding building)
    {
        var lines = FindObjectsOfType<Line>();

        foreach (var line in lines)
        {

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-"))
            {
                building.DeleteFromLine(line);
            }
            GUILayout.Label(building.CountOnLine(line).ToString());
            if (GUILayout.Button("+"))
            {
                building.AddToLine(line);
            }
            GUILayout.EndHorizontal();
        }
    }
}
