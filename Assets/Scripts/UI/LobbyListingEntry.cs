﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class LobbyListingEntry : MonoBehaviour {
	public GameObject password;
	public Text serverName;
	public Text playerCount;
	public Text map;
	public Text ping;
	public Button button;

	MatchInfoSnapshot ma;
	LobbyListMenu llm;

	void Awake(){
		button.onClick.AddListener(delegate{JoinMatch();});
	}

	public void Fill(MatchInfoSnapshot match, LobbyListMenu lListMenu){
		ma = match;
		password.SetActive(match.isPrivate ? true : false);
		serverName.text = match.name;
		playerCount.text = match.currentSize + "/" + match.maxSize;
		llm = lListMenu;
	}

	public void JoinMatch(){
		LobbyManager.lmSingleton.matchMaker.JoinMatch(ma.networkId, "", "", "", 0, 0, llm.OnMatchJoined);
	}
}
