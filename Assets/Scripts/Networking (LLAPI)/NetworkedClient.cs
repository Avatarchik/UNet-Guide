using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient: MonoBehaviour{

	public bool isConnected = false;

	void Start(){
		NetworkTransport.Init(LLobbyManager.instance.networkConfiguration);
	}
	public void ConnectToHost(string ipAdress, ConnectionConfig cc, int maxPlayersPerRoom, ref int connectionId, ref int socketId, ref int socketPort, ref float connectionTime){
		HostTopology topology = new HostTopology(cc, maxPlayersPerRoom);
		
		socketId = NetworkTransport.AddHost(topology, 0);

		byte error;
		connectionId = NetworkTransport.Connect(socketId, ipAdress, socketPort, 0, out error);

		connectionTime = Time.time;
		isConnected = true;
	}

	public void InfoUpdate(){
		if(!isConnected)
			return;

		int recHostId; 
		int connectionId; 
		int channelId; 
		byte[] recBuffer = new byte[1024]; 
		int bufferSize = 1024;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		switch (recData){
			case NetworkEventType.ConnectEvent:    //2
				break;
			case NetworkEventType.DataEvent:       //3
				string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
				string[] splitData = msg.Split('|');
				ReadMessage(splitData);
				break;
			case NetworkEventType.DisconnectEvent: //4
				break;
		}
	}

	void ReadMessage(string[] ms){
		switch(ms[0]){
			case "MOVE":
				LLobbyManager.instance.debugText.text = "MOVE COMMAND";
				Debug.Log("Move command.");
				break;
			case "CNN":
				break;
			default:
				LLobbyManager.instance.debugText.text = "INVALID MESSAGE";
				Debug.Log("Invalid message. ");
				break;
		}
	}
}
