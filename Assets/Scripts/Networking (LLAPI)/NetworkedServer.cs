using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedServer : MonoBehaviour{

	public bool isHosting = false;

	void Start(){
		NetworkTransport.Init(LLobbyManager.instance.networkConfiguration);
	}
	public void StartHosting(ConnectionConfig cc, int maxPlayersPerRoom, out int socketId, out int webSocketId, int socketPort){
		HostTopology topology = new HostTopology(cc, maxPlayersPerRoom);

		socketId = NetworkTransport.AddHost(topology, socketPort);
		webSocketId = NetworkTransport.AddWebsocketHost(topology, socketPort);
		isHosting = true;
	}

	public void InfoUpdate(){
		if(!isHosting)
			return;


		int recHostId; 
		int connectionId; 
		int channelId; 
		byte[] recBuffer = new byte[1024]; 
		int bufferSize = 1024;
		int dataSize;
		byte error;
		NetworkEventType recData;
		recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		switch (recData){
			case NetworkEventType.ConnectEvent:    //2
				Debug.Log("Client connected.");
				LLobbyManager.instance.OnClientConnectToServer(connectionId);
				break;
			case NetworkEventType.DataEvent:       //3
				string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
				Debug.Log("Got message " + msg);
				break;
			case NetworkEventType.DisconnectEvent: //4
				Debug.Log("Client disconnected.");
				break;
		}
	}

	public void Send(string message, int channelId, int connectionId){
		List<ServerClientDefinition> targets = new List<ServerClientDefinition>();
		targets.Add(LLobbyManager.instance.clients.Find(x => x.connectionId == connectionId));
		Send(message, channelId, targets);
	}

	public void Send(string message, int channelId, List<ServerClientDefinition> targetClients){
		byte error;
		byte[] msg = Encoding.Unicode.GetBytes(message);
		foreach(ServerClientDefinition sc in targetClients){
			NetworkTransport.Send(LLobbyManager.instance.SocketID, sc.connectionId, channelId, msg, message.Length*sizeof(char), out error);
		}
	}
}
