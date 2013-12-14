using UnityEngine;
using System.Collections;
using SimpleJSON;
using Server;
using System.Collections.Generic;
//using UnityEditor;
using Defort;

public enum Window
{
    Login,
    MainMenu,
    Player,
    Lobby
}

public class Entrance : MonoBehaviour
{
/*
    string serverUrl = "http://defortrest.somee.com";

    Window window = Window.Login;

    string login = "";
    string password = "";
    string authorizationToken = "";

    string restServerQueryName = "RestServerQuery";

    RestServerQuery restServerQuery;
    UnityEngine.WWW results = null;
    // Use this for initialization
    void Start()
    {
        restServerQuery = GetComponent(restServerQueryName) as RestServerQuery;

        window = Window.Login;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        if (window == Window.Login)
        {
            Rect rect = new Rect(Screen.width * 0.5f - 70f, Screen.height * 0.5f - 100f, 230f, 600f);
            GUILayout.BeginArea(rect);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Login: ");
            login = GUILayout.TextField(login, GUILayout.Width(150));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Password: ");
            password = GUILayout.PasswordField(password, '*', GUILayout.Width(150));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Login"))
            {
                Login(login, password);
            }
            if (GUILayout.Button("Sign up"))
            {
                //TODO sign up
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            try
            {
                if (results != null)
                {
                    if (results.error == null)
                    {
                        JSONNode node = JSON.Parse(results.text);
                        string Errors = node["Errors"];
                        if (Errors == "null" && authorizationToken != null)
                        {
                            authorizationToken = node["AuthorizationToken"]["Token"];

                            Player player = new Player();
                            player.Login = login;
                            player.Token = authorizationToken;
                            player.Name = node["AuthorizationToken"]["Name"];
                            player.Rank = node["AuthorizationToken"]["Rank"].AsInt;
                            player.XP = node["AuthorizationToken"]["XP"].AsInt;

                            Game.Player = player;
                            window = Window.Player;

                            //post = new Dictionary<string, string>();
                            //post.Add("Token", authorizationToken);
                            //UnityEngine.WWW allUnitsresults = restServerQuery.POST(serverUrl+"/units", post);
                            //UnityEngine.WWW allUnitsresults = restServerQuery.GET(serverUrl+"/units");

                            string resText = "";
                            //resText = allUnitsresults.text;
                            resText = @"[
							{ ""Name"":""Unit-alienShip"" },
							{ ""Name"":""Unit-recruiter"" }
							];";
                            JSONNode allUnitsNode = JSON.Parse(resText);

                            List<GameObject> allUnits = new List<GameObject>();
                            for (int i = 0; i < allUnitsNode.Count; ++i)
                            {
                                try
                                {
                                    string localPath = "Assets/Prefabs/Units/" + allUnitsNode[i]["Name"] + ".prefab";
                                    GameObject unit = AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)) as GameObject;
                                    if (unit != null)
                                    {
                                        GUIUnit(unit, allUnits);
                                        allUnits.Add(unit);
                                    }
                                }
                                catch
                                {
                                }
                            }

                            Game.Player.AvailableUnits = allUnits;
                            Game.Units = allUnits;
                        }
                        else
                        {
                            if (EditorUtility.DisplayDialog("Login", "Wrong login or password", "OK"))
                            {
                                results = null;
                            }
                            Debug.LogError(node["Errors"]);
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch
            {
            }

        }
        else if (window == Window.Player)
        {
            Rect rect = new Rect(Screen.width * 0.5f - 70f, Screen.height * 0.5f - 100f, 230f, 600f);
            GUILayout.BeginArea(rect);
            if (GUILayout.Button("Start"))
            {
                Application.LoadLevel("Game");
            }

            Handles.BeginGUI();
            //Handles.DrawBezier(windowRect.center, windowRect2.center, new Vector2(windowRect.xMax + 50f,windowRect.center.y), new Vector2(windowRect2.xMin - 50f,windowRect2.center.y),Color.red,null,5f);
            Handles.EndGUI();

            foreach (GameObject unit in Game.Units)
            {
                GUIUnit(unit, Game.Units);
            }

            GUILayout.EndArea();
        }
    }

    private void Login(string login, string password)
    {
        try
        {
            Dictionary<string, string> post = new Dictionary<string, string>();
            post.Add("username", login);
            post.Add("password", password);
            results = restServerQuery.POST(serverUrl + "/user/validate", post);
        }
        catch
        {
        }

    }

    private void GUIUnit(GameObject unit, List<GameObject> units)
    {
        var sr = unit.transform.Find("unit-sprite");
        var sprite = sr.GetComponent<SpriteRenderer>();
        GUILayout.Button(sprite.sprite.texture, GUILayout.MaxWidth(150), GUILayout.MaxHeight(150));
    }*/
}
