using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

    PlayerListingEntry listEntry;

    void Awake(){
        DontDestroyOnLoad(gameObject);
    }
    void Start(){
        GameObject lE = (GameObject)Instantiate(MainMenuRoot.singleton.lobbyMenu.playerListingPrefab, transform.position, transform.rotation);
        listEntry = lE.GetComponent<PlayerListingEntry>();
        listEntry.ready.onClick.AddListener(delegate{ChangeReadyStatus();});
        listEntry.characterOne.onClick.AddListener(delegate{SelectCharacterOne();});
        listEntry.characterTwo.onClick.AddListener(delegate{SelectCharacterTwo();});
        NetworkServer.SpawnWithClientAuthority(lE, gameObject);
    }

    void ChangeReadyStatus(){
        if(!readyToBegin){
            SendReadyToBeginMessage();
        }else{
            SendNotReadyToBeginMessage();
        }
    }

    void SelectCharacterOne(){
        LobbyManager.lmSingleton.SetCharacter(0);
    }

    void SelectCharacterTwo(){
        LobbyManager.lmSingleton.SetCharacter(1);
    }
}
