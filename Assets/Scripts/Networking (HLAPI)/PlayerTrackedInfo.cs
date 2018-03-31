using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackedInfo{

	public Vector3 pos;
	public Vector3 rotOther;

	public PlayerTrackedInfo(Vector3 position, Vector3 eAng){
		pos = position;
		rotOther = eAng;
	}
}
