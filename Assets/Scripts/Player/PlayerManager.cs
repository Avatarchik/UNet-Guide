using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour, BasePlayerManager{

	Rigidbody rigidbody;
	public GameObject nozeObj;
	[Header("Turning")]
	public float maxAngVelo = 1;
	public float turnTorque = 10;
	public float rotatingDrag = 0.04f;
	public float nonRotatingDrag = 0.4f;
	[Header("Movement")]
	public float thrustAcceleration = 1;
	public float thrustLimit = 10;
	public float movingDrag = 0;
	public float stoppedDrag = 4;
	[Header("Shooting")]
	public GameObject bulletPrefab;
	public Color canShootNoseColor;
	public bool canShoot = true;
	public float reloadTime = 1;
	public float bulletSpeed = 2;
	[Header("Inputs")]
	public bool turnLeft = false;
	public bool turnRight = false;
	public bool thrust = false;
	public bool slowDown = false;
	public bool shoot = false;

	void Start(){
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.maxAngularVelocity = maxAngVelo;
	}
	public void pUpdate(bool tLeft, bool tRight, bool thrustB, bool slowDownB, bool shootB){
		turnLeft = tLeft;
		turnRight = tRight;
		thrust = thrustB;
		slowDown = slowDownB;
		shoot = shootB;
	}

	public void pFixedUpdate(){
		HandleMovement(turnLeft, turnRight, thrust, slowDown);
		if(canShoot && shoot){
			CmdShootBullet(transform.forward);
			StartCoroutine(ResetShootTimer());
		}
	}

	void HandleMovement(bool tLeft, bool tRight, bool thrustB, bool slowDownB){
		if(thrustB || slowDownB){
			rigidbody.drag = movingDrag;
			if(thrustB)
				rigidbody.AddForce(thrustAcceleration * transform.forward, ForceMode.VelocityChange);
			if(slowDownB)
				rigidbody.AddForce(thrustAcceleration * -transform.forward, ForceMode.VelocityChange);

			if(rigidbody.velocity.magnitude > thrustLimit){
				rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, thrustLimit);
			}
		}else{
			rigidbody.drag = stoppedDrag;
		}

		if(tLeft || tRight){
			rigidbody.angularDrag = rotatingDrag;
			if(tLeft)
				rigidbody.AddTorque(Vector3.up * -turnTorque);
			if(tRight)
				rigidbody.AddTorque(Vector3.up * turnTorque);
		}else{
			rigidbody.angularDrag = nonRotatingDrag;
		}
	}

	IEnumerator ResetShootTimer(){
		canShoot = false;
		nozeObj.GetComponent<Renderer>().material.color = Color.black;
		yield return new WaitForSeconds(reloadTime);
		canShoot = true;
		nozeObj.GetComponent<Renderer>().material.color = canShootNoseColor;
	}

	[Command]
	void CmdShootBullet(Vector3 forward){
		GameObject bul = GameObject.Instantiate(bulletPrefab, transform.position + forward*1.5f, Quaternion.identity);
		bul.GetComponent<Rigidbody>().velocity = forward * bulletSpeed;
		NetworkServer.SpawnWithClientAuthority(bul, base.connectionToClient);
	}
}
