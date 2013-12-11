using Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInitializer : Photon.MonoBehaviour
{
	private PhotonView photonView;
	private PlayerWrapper localPlayer;
	private Dictionary<int, PlayerWrapper> players;
	
	void Awake()
	{
		photonView = PhotonView.Get(this);
		localPlayer = new PlayerWrapper(PhotonNetwork.player);
		players = new Dictionary<int, PlayerWrapper>();
		
		AddPlayer(localPlayer);
	}
	
	private void AddPlayer(PlayerWrapper player)
	{
		players.Add(player.Id, player);
	}
	
	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (players.Count > 1)
		{
			// Drop player from room.
			return;
		}
		
		RenderStartButton();
		
		AddPlayer(new PlayerWrapper(player));
	}
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		
	}
	
	private void RenderStartButton()
	{
		if (GUILayout.Button("Start game"))
		{
			photonView.RPC("OnPlayerReadyForLoading", PhotonTargets.All, localPlayer.Id);
			
			if (players.Values.All(x => x.IsReadyForLoading))
			{
				photonView.RPC("LoadGame", PhotonTargets.All);
			}
		}
	}
	
	[RPC]
	void OnPlayerReadyForLoading(int playerId)
	{
		players[playerId].IsReadyForLoading = true;
	}
	
	[RPC]
	void LoadGame()
	{
		// Loading goes here.
		// Load necessary data about all players.
		// Init all necessary objects;
		
		photonView.RPC("OnLoadingFinished", PhotonTargets.All, localPlayer.Id);
		
		if (players.Values.All(x => x.IsLoadingFinished))
		{
			photonView.RPC("StartGame", PhotonTargets.All);
		}
	}
	
	[RPC]
	void OnLoadingFinished(int playerId)
	{
		players[playerId].IsReadyForLoading = true;
	}
	
	private class PlayerWrapper
	{
		private PhotonPlayer player;
		
		public PlayerWrapper(PhotonPlayer player)
		{
			this.player = player;
		}
		
		public int Id { get { return player.ID; } }
		
		public string Name { get { return player.name; } }
		
		public bool IsReadyForLoading { get; set; }
		
		public bool IsLoadingFinished { get; set; }
	}
}