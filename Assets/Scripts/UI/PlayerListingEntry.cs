using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerListingEntry : NetworkBehaviour {

	public Text playerName;
	public GameObject characterSelect;
	public GameObject adminPanel;

	public Button characterOne;
	public Button characterTwo;
	public Button kick;
	public Button ready;

	void Start(){
		transform.SetParent(MainMenuRoot.singleton.lobbyMenu.playerList, false);
		if(localPlayerAuthority){
			characterSelect.SetActive(true);
			adminPanel.SetActive(false);
		}else if(isServer){
			adminPanel.SetActive(true);
			characterSelect.SetActive(false);
			ready.gameObject.SetActive(false);
		}else{
			ready.gameObject.SetActive(false);
		}
	}
}
