using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

    //public override void OnMatchCreate(bool success, string extendedInfo, UnityEngine.Networking.Match.MatchInfo matchInfo){
    //    base.OnMatchCreate(success, extendedInfo, matchInfo);
    //}

    //public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId){
    //    return lobbyPlayerPrefab.gameObject;
    //}

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
}
