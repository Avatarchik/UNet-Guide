using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyMenu : MonoBehaviour {

	public Transform playerList;
	public GameObject playerListingPrefab;
	
	public void LeaveLobby(){
		NetworkLobbyManager.singleton.StopHost();
	}
}
