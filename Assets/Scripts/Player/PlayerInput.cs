using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour {

	PlayerManager playerManager;
	[SerializeField]
	MeshRenderer meshRenderer;

	void Start(){
		SetupPlayer();
	}

	void SetupPlayer(){
		if(isLocalPlayer){
			playerManager = GetComponent<PlayerManager>();
		}else{

		}
	}

	void Update(){
		if(!isLocalPlayer){
			return;
		}

		playerManager.pUpdate(Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.Space));
	}

	void FixedUpdate(){
		if(!isLocalPlayer){
			return;
		}

		playerManager.pFixedUpdate();
	}
}
