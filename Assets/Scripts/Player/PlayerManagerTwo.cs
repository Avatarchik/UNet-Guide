using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManagerTwo : SuperStateMachine, BasePlayerManager{

	SuperCharacterController sCC;
	Vector3 moveDir = Vector3.zero;
	float rotSpeed = 0;
	[Header("Turning")]
	public float maxAngVelo = 1;
	public float rotateSpeed = .5f;
	public float rotationMaxSpeed = 10;
	public float rotateSlowdown = 0.75f;
	[Header("Movement")]
	public float thrustAcceleration = 1;
	public float thrustLimit = 10;
	public float thrustSlowdown = 0.75f;
	public float movingDrag = 0;
	public float stoppedDrag = 4;
	[Header("Inputs")]
	public bool turnLeft = false;
	public bool turnRight = false;
	public bool thrust = false;
	public bool slowDown = false;

	[Header("Online Settings")]
	public float posCorrection = 0.01f;
	public float rotCorrection = 0.01f;
	public int mTag = 0;
	public int tagsBack = 60;
	public Dictionary<int, PlayerTrackedInfo> trackedPos = new Dictionary<int, PlayerTrackedInfo>();

	void Start(){
		sCC = GetComponent<SuperCharacterController>();
	}

	public void pUpdate(bool tLeft, bool tRight, bool thrustB, bool slowDownB, bool shoot){
		turnLeft = tLeft;
		turnRight = tRight;
		thrust = thrustB;
		slowDown = slowDownB;
	}

	public void pFixedUpdate(){
		HandleMovement(turnLeft, turnRight, thrust, slowDown);
		CmdMovement(turnLeft, turnRight, thrust, slowDown, mTag);
		if(mTag > tagsBack){
			trackedPos.Remove(mTag-tagsBack);
		}
		mTag++;
	}

	void HandleMovement(bool tLeft, bool tRight, bool thrustB, bool slowDownB){
		//MOVEMENT

		if(thrustB || slowDownB){
			if(thrustB)
				moveDir += transform.forward * thrustAcceleration;
			if(slowDownB)
				moveDir -= transform.forward * thrustAcceleration;
		}else{
		}

		if(tLeft || tRight){
			if(tLeft)
				rotSpeed -= rotateSpeed;
			if(tRight)
				rotSpeed += rotateSpeed;
			rotSpeed = Mathf.Clamp(rotSpeed, -rotationMaxSpeed, rotationMaxSpeed);
		}else{
			if(rotSpeed < 0){
				rotSpeed += rotateSlowdown;
				if(rotSpeed > 0){
					rotSpeed = 0;
				}
			}else if(rotSpeed > 0){
				rotSpeed -= rotateSlowdown;
				if(rotSpeed < 0){
					rotSpeed = 0;
				}
			}
		}

		moveDir = Vector3.ClampMagnitude(moveDir, thrustLimit);

		sCC.ManualUpdate(Time.fixedDeltaTime);
		trackedPos.Add(mTag, new PlayerTrackedInfo(transform.position, transform.eulerAngles));
	}

	//Move player on the server, then send that updated position out.
	[Command]
	public void CmdMovement(bool tLeft, bool tRight, bool thrustB, bool slowDownB, int moveTag){
		mTag = moveTag;
		if(!isLocalPlayer){
			HandleMovement(tLeft, tRight, thrustB, slowDownB);
		}
		if(mTag > tagsBack){
			trackedPos.Remove(mTag-tagsBack);
		}

		RpcUpdateTransform(transform.position, transform.eulerAngles, moveTag);
	}
	
	[ClientRpc]
	public void RpcUpdateTransform(Vector3 pos, Vector3 eAn, int moveTag){
		if(isLocalPlayer){
			//Make sure the local player is in the correct position.
			if(Mathf.Abs(Vector3.Distance(trackedPos[moveTag].pos, pos)) > posCorrection){
				Debug.Log("------------------------------------------------");
				Debug.Log("POS CORRECTION: " + Vector3.Distance(trackedPos[moveTag].pos, pos));
				transform.position = pos;
			}
			if(Mathf.Abs(Vector3.Distance(trackedPos[moveTag].rotOther, eAn)) > rotCorrection){
				Debug.Log("------------------------------------------------");
				Debug.Log("ROTATION CORRECTION: " + Mathf.Abs(Vector3.Distance(trackedPos[moveTag].rotOther, eAn)));
				transform.eulerAngles = eAn;
			}
			trackedPos.Remove(moveTag);
		}else{
			//Update other players with what the server thinks is your position.
			transform.position = pos;
			transform.eulerAngles = eAn;
		}
	}

    protected override void LateGlobalSuperUpdate(){
        // Put any code in here you want to run AFTER the state's update function.
        // This is run regardless of what state you're in

        // Move the player by our velocity every frame
        transform.position += moveDir * sCC.deltaTime;
		transform.RotateAround(transform.position, Vector3.up, rotSpeed * sCC.deltaTime);
    }

	//DEBUG//
	void OnGUI() {
		GUI.TextArea(new Rect(0+(isLocalPlayer ? 0 : 500), 0, 200, 50), mTag + " Pos: " + transform.position.ToString("F2") );
		GUI.TextArea(new Rect(0+(isLocalPlayer ? 0 : 500), 200, 200, 50), mTag + " Rot: " + transform.rotation.ToString("F2") );
    }
}