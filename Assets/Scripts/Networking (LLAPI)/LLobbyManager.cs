using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LLobbyManager : MonoBehaviour {

	public static LLobbyManager instance;
	public GlobalConfig networkConfiguration;
	public ConnectionConfig connectionConfig;
	public int maxPlayersPerRoom = 2;

	[Header("General Information")]
	public int reliableChannel = -1;
	public int unreliableChannel = -1;
	[Header("Hosting Information")]
	public NetworkedServer networkServer;
	private int socketId = -1;
	private int webSocketId = -1;
	private int socketPort = 5002;
	public List<ServerClientDefinition> clients = new List<ServerClientDefinition>();

	[Header("Client information")]
	public NetworkedClient networkClient;
	private int connectionId = -1;
	private float connectionTime;
	public Text debugText;

	void Awake(){
		if(instance == null){
			instance = this;
		}else{
			if(instance != this){
				Destroy(this);
			}
		}
	}

	public void InitNetwork(){
		NetworkTransport.Init(networkConfiguration);

        connectionConfig.Channels.Clear();
        reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
        unreliableChannel = connectionConfig.AddChannel(QosType.Unreliable);
	}

	public void Shutdown(){
		NetworkTransport.Shutdown();
	}

	void OnApplicationQuit(){
		Shutdown();
	}

	void Update(){
		networkClient.InfoUpdate();
		networkServer.InfoUpdate();
	}

	public void StartHosting(){
		InitNetwork();
		
		networkServer.StartHosting(connectionConfig, maxPlayersPerRoom, out socketId, out webSocketId, socketPort);

		Debug.Log("Started hosting server on port " + socketPort);
	}

	public void ConnectToHost(string ipAdress){
		InitNetwork();

		networkClient.ConnectToHost(ipAdress, connectionConfig, maxPlayersPerRoom, ref connectionId, ref socketId, ref socketPort, ref connectionTime);
		Debug.Log("Connected to server. ConnectionId: " + connectionId);
	}

	public void StartSinglePlayer(){

	}

	//EVENTS//
	public void OnClientConnectToServer(int connectionId){
		Debug.Log("Stuff");
		ServerClientDefinition sC = new ServerClientDefinition();
		sC.connectionId = connectionId;
		clients.Add(sC);
		SendMovementMessage(connectionId);
	}

	public void SendMovementMessage(int connectionId){
		networkServer.Send("MOVE|1.2,2.2|", reliableChannel, connectionId);
	}

	#region Attributes
	public int SocketID{
		get{
			return socketId;
		}
	}
	#endregion
}
