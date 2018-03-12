using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackedInfo{

	public Vector3 pos;
	public Quaternion rot;

	public PlayerTrackedInfo(Vector3 position, Quaternion rotation){
		pos = position;
		rot = rotation;
	}
}
