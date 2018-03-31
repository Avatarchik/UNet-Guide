using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerData{
    public int playerId;
    public int connectionId;
}

//The server's definition of a client, only stored on the server.
[System.Serializable]
public class ServerClientDefinition{
    public int connectionId;
}
