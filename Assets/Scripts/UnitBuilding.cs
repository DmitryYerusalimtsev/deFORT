using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitBuilding : MonoBehaviour
{
    public const string UnityTag = "Building";

    public GameObject UnitPrefab;
    public Sprite Icon;
    private Dictionary<Line, int> SpawnLines; 
    public float SpawnTime = 5f;		// The amount of time between each spawn.
    public float SpawnDelay = 3f;		// The amount of time before spawning starts.
    public int ManaCost = 50;
    public int UnitCount = 3;

    private Scores info;

	// Use this for initialization
    void Start()
    {
        if (!tag.Equals(UnityTag)) tag = UnityTag;

        SpawnLines = new Dictionary<Line, int>();
        InitializeLines();
        InvokeRepeating("Spawn", SpawnDelay, SpawnTime);
        info = FindObjectOfType<Scores>();
    }

    void Spawn()
    {
        var defatultTarget = Side == Scores.LeftSideNumber ? info.LeftDefaultTarget : info.RightDefaultTarget;
        foreach (var spawnLine in SpawnLines)
        {
            for (int i = 0; i < spawnLine.Value; i++)
            {
                var unit = spawnLine.Key.Spawn(UnitPrefab, Side);
                unit.DefatultTarget = defatultTarget;

                if (Side == Scores.LeftSideNumber)
                {
                    ++info.LeftUnitCount;
                }
                else
                {
                    ++info.RightUnitCount;
                }
            }
        }
    }

    void InitializeLines()
    {
        var array = FindObjectsOfType<Line>();

        foreach (var line in array)
        {
            SpawnLines.Add(line, 0);
        }
    }

    public void AddToLine(Line line)
    {
        if (SpreadedCount < UnitCount) ++SpawnLines[line];
    }

    public void DeleteFromLine(Line line)
    {
        if (SpawnLines[line] > 0) --SpawnLines[line];
    }

    public int CountOnLine(Line line)
    {
        return SpawnLines[line];
    }

    public Texture2D Texture
    {
        get
        {
            return Icon.texture;
        }
    }

    public IList<Line> Lines
    {
        get
        {
            return SpawnLines.Keys.ToList();
        }
    }

    public int SpreadedCount
    {
        get { return SpawnLines.Values.Sum(); }
    }

    public int Side
    {
        get; set;
    }
}
