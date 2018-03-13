using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface BasePlayerManager{
	
	void pUpdate(bool tLeft, bool tRight, bool thrustB, bool slowDownB, bool shoot);
	void pFixedUpdate();
}
