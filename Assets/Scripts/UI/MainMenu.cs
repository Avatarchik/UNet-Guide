using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject serverListingMenu;

	public void Awake(){
		if(SceneManager.sceneCount == 1){
			SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
		}
	}

	public void StartSP(){
		LobbyManager.lmSingleton.StopMatchMaker();
		LobbyManager.lmSingleton.StartHost();
	}

	public void OpenServerListMenu(){
		serverListingMenu.SetActive(true);
		gameObject.SetActive(false);
	}
}
