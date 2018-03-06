using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

    public GameObject[] gamePlayers;

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId){
        Debug.Log("Create game player.");
        gamePlayerPrefab = gamePlayers[Random.Range(0, gamePlayers.Length+1)];
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

    //public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer){
    //    return true;
    //}
}
