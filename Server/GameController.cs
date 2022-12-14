using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameController : NetworkBehaviour
{
    
    [Server]
    private void ServerCheckPlayerConnected()
    {
        RpcCheckPlayerConnected();
    }
    
    [ClientRpc]
    private void RpcCheckPlayerConnected()
    {
        print("test");
    }
}
