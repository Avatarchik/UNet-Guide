using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

    public static LobbyManager lmSingleton;
    [SerializeField]
    private GameObject[] gamePlayers;
    public bool serverAuthority = false;
    public int seed = 1;

    void Awake(){
        lmSingleton = this;
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId){
        Debug.Log("Created game player.");
        Random.InitState(1);
        return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    }

    public override void OnClientConnect(NetworkConnection conn){
        Debug.Log("Client connected.");
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0);
    }

    public override void OnLobbyClientEnter(){
        Debug.Log("Client entered.");
    }

    public override void OnLobbyStartServer(){
        base.OnLobbyStartServer();
        Debug.Log("Server started.");
    }

    public void SetCharacter(int characterIndex){
        gamePlayerPrefab = gamePlayers[characterIndex];
    }
}
