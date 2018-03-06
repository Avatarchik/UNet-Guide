using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyMenu : MonoBehaviour {

	public void LeaveLobby(){
		NetworkLobbyManager.singleton.StopHost();
	}
}
