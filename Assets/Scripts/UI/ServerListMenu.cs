using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class ServerListMenu : MonoBehaviour {

	public ServerListingEntry serverListingPrefab;
	public Transform serverListParent;

	public GameObject mainMenu;
	public GameObject lobby;

	void OnEnable(){
		LobbyManager.singleton.StartMatchMaker();	
		ListMatches();
	}

	public void ToMainMenu(){
		LobbyManager.singleton.StopMatchMaker();
		mainMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	public void ListMatches(){
		LobbyManager.singleton.matchMaker.ListMatches(0, 10, "", false, 0, 0, OnListMatches);
	}

	public void OnListMatches(bool success, string extendedInfo, List<MatchInfoSnapshot> matches){
		
		foreach(Transform t in serverListParent){
			if(t.name != "Header"){
				Destroy(t.gameObject);
			}
		}

		if(matches.Count == 0){
			Debug.Log("No matches found.");
			return;
		}

		for(int i = 0; i < matches.Count; i++){
			GameObject s = GameObject.Instantiate(serverListingPrefab.gameObject);
			s.GetComponent<ServerListingEntry>().Fill(matches[i]);
			s.transform.SetParent(serverListParent, false);
		}
	}

	public void CreateMatch(){
		LobbyManager.singleton.matchMaker.CreateMatch("Random Room " + Random.Range(0, 500), 4, true, "", "", "", 0, 0, LobbyManager.singleton.OnMatchCreate);
	}
}
