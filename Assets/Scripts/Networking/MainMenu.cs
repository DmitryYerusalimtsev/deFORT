using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	private MainMenuView menuRenderer = new MainMenuView();

	void Awake()
	{
		Init();				
	}

	private void Init()
	{
		if (!PhotonNetwork.connected)
		{
			PhotonNetwork.ConnectUsingSettings("v1.0");
		}

		PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

		Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
	}

	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUIHelper.RenderInsideArea(() => GUILayout.Label("Connecting to Photon server..."));
			return;
		}		
		
		if (PhotonNetwork.room != null)
		{
			return;
		}

		if (GUI.changed)
		{
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		}
		
		menuRenderer.Render();
	}
}