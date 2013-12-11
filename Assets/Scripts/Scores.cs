using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    public const string UnityTag = "Scores";
    public const int LeftSideNumber = 1;        // Номер левой стороны по умолчанию
    public const int RightSideNumber = 2;       // Номер правой стороны по умолчанию
    public const int ManaCount = 50;            // Количество маны по умолчанию
    public const int CastleHits = 100;          // Жизни юнита по умолчанию
    public const int DefaultBuildingSlots = 2;  // Количество зданий по умолчанию
    public const int ManaRegen = 10;            // Количество регенерации маны
    public int WinScorePoints = 100;            // Очки за выигрыш
    public int LoseScorePoints = 10;             // Очки за проигрыш
    public float ManaSpawnDelay = 3f;

    public GameObject LeftDefaultTarget;        // Цель которую надо атаковать по умолчанию левым
    public GameObject RightDefaultTarget;       // Цель которую надо атаковать по умолчанию правым

    public int LeftUnitCount { get; set; }      // Количество юнитов слева
    public int RightUnitCount { get; set; }     // Количество юнитов справа

    public int LeftManaCount { get; set; }      // Количество маны левого игрока
    public int RightManaCount { get; set; }     // Количество маны правого игрока

    public int LeftScorePoints { get; set; }    // Количество очков за игру у левого
    public int RightScorePoints { get; set; }   // Количество очков за игру у правого

    public int LeftBuildingSlots { get; set; }
    public int RightBuildingSlots { get; set; }

    public IList<UnitBuilding> leftBuildings;
    public IList<UnitBuilding> rightBuildings; 

    public bool GameOver { get; set; }

    private void Start()
    {
        LeftManaCount = ManaCount;
        RightManaCount = ManaCount;
        LeftBuildingSlots = DefaultBuildingSlots;
        RightBuildingSlots = DefaultBuildingSlots;
        leftBuildings = new UnitBuilding[LeftBuildingSlots];
        rightBuildings = new UnitBuilding[LeftBuildingSlots];
        InvokeRepeating("SpawnMana", 0f, ManaSpawnDelay);
    }

    private void Update()
    {
        if (GameOver) return;

		if (LeftDefaultTarget == null || RightDefaultTarget == null) return;

        if (LeftDefaultTarget.GetComponent<Health>().Dead || RightDefaultTarget.GetComponent<Health>().Dead)
        {
            GameOver = true;
            PresentAwards();
            Pauser.Pause(true);
        }
    }

    private void PresentAwards()
    {
        if (LeftDefaultTarget.GetComponent<Health>().Dead)
        {
            RightScorePoints += WinScorePoints;
            LeftScorePoints += LoseScorePoints;
        }
        else
        {
            LeftScorePoints += WinScorePoints;
            RightScorePoints += LoseScorePoints;
        }
    }

    private void SpawnMana()
    {
        LeftManaCount += ManaRegen;
        RightManaCount += ManaRegen;
    }

    public int TotalUnits
    {
        get
        {
            return LeftUnitCount + RightUnitCount;
        }
    }

    public int TotalBuildings
    {
        get
        {
            return LeftBuildingCount + RightBuildingCount;
        }
    }

    // Количество зданий слева
    public int LeftBuildingCount
    {
        get { return leftBuildings.Count; }
    }

    // Количество зданий справа
    public int RightBuildingCount
    {
        get { return rightBuildings.Count; }
    }
}
