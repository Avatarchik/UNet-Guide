using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class ServerListingEntry : MonoBehaviour {
	public GameObject password;
	public Text serverName;
	public Text playerCount;
	public Text map;
	public Text ping;
	public Button button;

	MatchInfoSnapshot ma;

	void Awake(){
		button.onClick.AddListener(delegate{JoinMatch();});
	}

	public void Fill(MatchInfoSnapshot match){
		ma = match;
		password.SetActive(match.isPrivate ? true : false);
		serverName.text = match.name;
		playerCount.text = match.currentSize + "/" + match.maxSize;
	}

	public void JoinMatch(){
		NetworkLobbyManager.singleton.matchMaker.JoinMatch(ma.networkId, "", "", "", 0, 0, NetworkLobbyManager.singleton.OnMatchJoined);
	}
}
