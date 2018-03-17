using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthHandler : NetworkBehaviour {

	[SyncVar]
	public float maxHealth = 100;
	[SerializeField]
	float health = 100;
	public bool serverAuthority = false;


	[Command]
	public void CmdDamage(float damageAmount){
		if(!isServer)
			return;

		health -= damageAmount;
		health = Mathf.Clamp(health, 0, maxHealth);
	}

	[Command]
	public void CmdHeal(float healAmount){
		if(!isServer)
			return;

		health += healAmount;
		health = Mathf.Clamp(health, 0, maxHealth);
	}

	//Server Authority
	public void AuthoritativeDamage(float damageAmount){
		health -= damageAmount;
		health = Mathf.Clamp(health, 0, maxHealth);
	}

	public void AuthoritativeHeal(float healAmount){
		health += healAmount;
		health = Mathf.Clamp(health, 0, maxHealth);
	}
}
