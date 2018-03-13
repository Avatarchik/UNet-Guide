using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkManager, BasePlayerManager{

	CharacterController cCon;
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
	[Header("Inputs")]
	public bool turnLeft = false;
	public bool turnRight = false;
	public bool thrust = false;
	public bool slowDown = false;

	public void pUpdate(bool tLeft, bool tRight, bool thrustB, bool slowDownB, bool shoot){
		turnLeft = tLeft;
		turnRight = tRight;
		thrust = thrustB;
		slowDown = slowDownB;
	}

	public void pFixedUpdate(){
		//HandleMovement(turnLeft, turnRight, thrust, slowDown);
		//CmdMovement(turnLeft, turnRight, thrust, slowDown, mTag);
	}
}
