using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRoot : MonoBehaviour {

	public static MainMenuRoot singleton;
	public LobbyMenu lobbyMenu;

	void Awake(){
		singleton = this;
	}
}
