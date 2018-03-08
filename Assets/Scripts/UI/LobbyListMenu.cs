using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class LobbyListMenu : MonoBehaviour {

	public LobbyListingEntry lobbyListingPrefab;
	public Transform lobbyListParent;

	public GameObject mainMenu;
	public GameObject lobby;

	int matchListPage = 0;

	void OnEnable(){
		LobbyManager.lmSingleton.StartMatchMaker();	
		ListMatches();
	}

	public void ToMainMenu(){
		LobbyManager.lmSingleton.StopMatchMaker();
		mainMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	public void ToLobby(){
		lobby.SetActive(true);
		gameObject.SetActive(false);
	}

	public void ListMatches(){
		LobbyManager.lmSingleton.matchMaker.ListMatches(matchListPage, 10, "", false, 0, 0, OnListMatches);
	}

	public void OnListMatches(bool success, string extendedInfo, List<MatchInfoSnapshot> matches){
		
		//Clears the lobby listings.
		foreach(Transform t in lobbyListParent){
			if(t.GetSiblingIndex() != 0){
				Destroy(t.gameObject);
			}
		}

		if(matches.Count == 0){
			Debug.Log("No matches found.");
			return;
		}

		//List the matches.
		for(int i = 0; i < matches.Count; i++){
			GameObject s = GameObject.Instantiate(lobbyListingPrefab.gameObject);
			s.GetComponent<LobbyListingEntry>().Fill(matches[i], this);
			s.transform.SetParent(lobbyListParent, false);
		}
	}

	public void CreateMatch(){
		LobbyManager.lmSingleton.matchMaker.CreateMatch("Random Room " + Random.Range(0, 500), 4, true, "", "", "", 0, 0, OnMatchCreate);
	}

	void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo){
		ToLobby();
		LobbyManager.lmSingleton.OnMatchCreate(success, extendedInfo, matchInfo);
	}

	public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo){
		ToLobby();
		LobbyManager.lmSingleton.OnMatchJoined(success, extendedInfo, matchInfo);
	}
}
