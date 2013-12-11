using UnityEngine;
using System.Collections;

public class MainMenuView
{
	private string roomName = "my room";
	private Vector2 scrollPosition = Vector2.zero;

	public void Render()
	{
		GUIHelper.RenderInsideArea(() => 
		{
			GUILayout.Label("Main Menu");

			RenderPlayerName();			
			GUILayout.Space(15);

			RenderCreateRoomButton();
			GUILayout.Space(15);

			var availableRooms = PhotonNetwork.GetRoomList();
			if (availableRooms.Length > 0) 
			{
				RenderJoinRoomButton();
				RenderJoinRandomRoomButton();
			}

			GUILayout.Label("ROOM LISTING:");
			if (availableRooms.Length > 0)
			{
				RenderRoomListing(availableRooms);
			}
			else
			{
				GUILayout.Label("..no games available..");
			}
		});
	}

	private void RenderPlayerName()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player name:", GUILayout.Width(150));
		GUILayout.TextField(PhotonNetwork.playerName);
		GUILayout.EndHorizontal();
	}

	private void RenderCreateRoomButton()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("CREATE ROOM:", GUILayout.Width(150));
		roomName = GUILayout.TextField(roomName);
		
		if (GUILayout.Button("GO"))
		{
			PhotonNetwork.CreateRoom(roomName, true, true, 10);
		}
		
		GUILayout.EndHorizontal();
	}

	private void RenderJoinRoomButton()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("JOIN ROOM:", GUILayout.Width(150));
		roomName = GUILayout.TextField(roomName);

		if (GUILayout.Button("GO"))
		{
			PhotonNetwork.JoinRoom(roomName);
		}

		GUILayout.EndHorizontal();
	}

	private void RenderJoinRandomRoomButton()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("JOIN RANDOM ROOM:", GUILayout.Width(150));

		if (GUILayout.Button("GO"))
		{
			PhotonNetwork.JoinRandomRoom();
		}

		GUILayout.EndHorizontal();
	}

	private void RenderRoomListing(RoomInfo[] availableRooms)
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		foreach (RoomInfo game in availableRooms)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);

			if (GUILayout.Button("JOIN"))
			{
				PhotonNetwork.JoinRoom(game.name);
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.EndScrollView();
	}
}