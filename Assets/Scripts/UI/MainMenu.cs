using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public GameObject serverListingMenu;

	public void StartSP(){
		LobbyManager.singleton.StopMatchMaker();
		LobbyManager.singleton.StartHost();
	}

	public void OpenServerListMenu(){
		serverListingMenu.SetActive(true);
		gameObject.SetActive(false);
	}
}
