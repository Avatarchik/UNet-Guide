using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletOne : NetworkBehaviour {

	Rigidbody rigidbody;
	public float damage = 5;
	public float timeBeforeDestruction = 5;
	public bool activated = false;
	public Vector3 forceDir;
	public float force;

	void Start(){
		if(hasAuthority){
			StartCoroutine(DestroyAfter());
		}
	}

	void OnTriggerEnter(Collider other){
		if(hasAuthority){
			if(other.gameObject != LobbyManager.lmSingleton.client.connection.playerControllers[0].gameObject){
				HealthHandler hh = other.gameObject.GetComponent<HealthHandler>();
				if(hh != null){
					hh.CmdDamage(damage);
				}
			}
			NetworkServer.Destroy(gameObject);
		}
	}

	IEnumerator DestroyAfter(){
		yield return new WaitForSeconds(timeBeforeDestruction);
		NetworkServer.Destroy(gameObject);
	}

}
