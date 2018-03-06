using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class ServerListingEntry : MonoBehaviour {
	public GameObject password;
	public Text serverName;
	public Text playerCount;
	public Text map;
	public Text ping;

	public void Fill(MatchInfoSnapshot match){
		password.SetActive(match.isPrivate ? true : false);
		serverName.text = match.name;
		playerCount.text = match.currentSize + "/" + match.maxSize;
		
	}
}
