using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour{

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
	[Header("Online")]
	public float posCorrection = 0.01f;
	public float rotCorrection = 0.01f;
	public int mTag = 0;
	public int tagsBack = 60;
	public Dictionary<int, PlayerTrackedInfo> trackedPos = new Dictionary<int, PlayerTrackedInfo>();

	void Start(){
		cCon = GetComponent<CharacterController>();
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
				cCon.Move(Vector3.up * thrustAcceleration);
			if(slowDownB)
				cCon.Move(Vector3.down * thrustAcceleration);
		}
		trackedPos.Add(mTag, new PlayerTrackedInfo(transform.position, transform.rotation));
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

		RpcUpdateTransform(transform.position, transform.rotation, moveTag);
	}
	
	[ClientRpc]
	public void RpcUpdateTransform(Vector3 pos, Quaternion rot, int moveTag){
		if(isLocalPlayer){
			//Make sure the local player is in the correct position.
			if(Mathf.Abs(Vector3.Distance(trackedPos[moveTag].pos, pos)) > posCorrection){
				Debug.Log("------------------------------------------------");
				Debug.Log("POS CORRECTION: " + trackedPos[moveTag].pos + " compared with " + pos);
				transform.position = pos;
			}
			if(Mathf.Abs(Quaternion.Angle(trackedPos[moveTag].rot, rot)) > rotCorrection){
				Debug.Log("------------------------------------------------");
				Debug.Log("ROTATION CORRECTION: " + trackedPos[moveTag].rot + " compared with " + rot);
				transform.rotation = rot;
			}
			trackedPos.Remove(moveTag);
		}else{
			//Update other players with what the server thinks is your position.
			transform.position = pos;
			transform.rotation = rot;
		}
	}

	//DEBUG//
	void OnGUI() {
		GUI.TextArea(new Rect(0+(isLocalPlayer ? 0 : 500), 0, 200, 50), mTag + " Pos: " + transform.position.ToString("F2") );
		GUI.TextArea(new Rect(0+(isLocalPlayer ? 0 : 500), 200, 200, 50), mTag + " Rot: " + transform.rotation.ToString("F2") );
    }
}
