using Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class GameInitializer : Photon.MonoBehaviour
{
	private PhotonView photonView;
	private List<PlayerWrapper> players;
	private PlayerWrapper player;
	private Room room;
	private bool renderStartButton = false;

	void Awake() 
	{
		photonView = PhotonView.Get(this);
		players = new List<PlayerWrapper>();
	}

	void OnJoinedRoom()
	{
		room = PhotonNetwork.room;
		player = new PlayerWrapper(PhotonNetwork.player);
		players.Add (player);

		var otherPlayers = PhotonNetwork.otherPlayers
										.Select(x => new PlayerWrapper(x));

		players.AddRange(otherPlayers);	
	}
	
	private bool IsGameLoadingFinished
	{
		get 
		{
			return players.Count == room.maxPlayers 
				&& players.All (x => x.IsLoadingFinished);
		}
	}

	private bool IsGameReadyForLoading
	{
		get
		{
			return players.Count == room.maxPlayers 
				&& players.All (x => x.IsReadyForLoading);
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		//Stop game
	}

	void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
	{
		players.Add (new PlayerWrapper(photonPlayer));				
		
		if (players.Count == PhotonNetwork.room.maxPlayers) 
		{
			networkView.RPC ("RenderStartButton", PhotonTargets.All);

		}
	}

	[RPC]
	void RenderStartButton()
	{
		renderStartButton = true;
	}

	void OnPhotonPlayerPropertiesChanged(PhotonPlayer player)
	{
		if (IsGameReadyForLoading)
		{
			Application.LoadLevel("Game");
		}

		if (IsGameLoadingFinished)
		{
			// go game :D
		}
	}
	
	void OnGUI() 
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		if (renderStartButton)
		{
			if (GUILayout.Button ("Start game"))
			{
				player.IsReadyForLoading = true;
			}
		}

		if (player != null)
		{
			GUILayout.Label("Player name: " + player.Name);
		}

		GUIHelper.RenderInsideArea(() =>
	    {
			GUILayout.Label("Players:");
			foreach (var playerWrapper in players)
			{
				GUILayout.Label(playerWrapper.Name + " Ready: " + playerWrapper.IsReadyForLoading);
			}
		});
	}
	
	private class PlayerWrapper
	{
		private PhotonPlayer player;
		
		public PlayerWrapper(PhotonPlayer player)
		{
			this.player = player;
			IsReadyForLoading = false;
			IsLoadingFinished = false;
		}
		
		public int Id { get { return player.ID; } }
		
		public string Name { get { return player.name; } }

		public bool IsMasterClient { get { return player.isMasterClient; } } 
		
		public bool IsReadyForLoading 
		{ 
			get { return GetCustomProperty<bool>("IsReadyForLoading"); }  
			set { SetCustomProperty("IsReadyForLoading", value); } 
		}
		
		public bool IsLoadingFinished 
		{ 
			get { return GetCustomProperty<bool>("IsLoadingFinished"); } 
			set { SetCustomProperty("IsLoadingFinished", value); }
		}

		private T GetCustomProperty<T>(string key)
		{
			return (T) player.customProperties[key];
		}

		private void SetCustomProperty(string key, object value)
		{
			var properties = new Hashtable();
			properties.Add(key, value);

			player.SetCustomProperties(properties);
		}
	}
}