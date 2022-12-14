using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;

public class CarrotSeedBag : NetworkBehaviour
{
    //currentPlayer is player called StartInteraction;
    [SyncVar(hook = nameof(DeleteThis))] [SerializeField] protected GameObject currentPlayer;
    [SyncVar] public int seedValue;
    
    [Command(requiresAuthority = false)]
    public void StartInteraction(GameObject player)
    {
        currentPlayer = player;
    }
    
    private void DeleteThis(GameObject _new, GameObject _old)
    {
        currentPlayer.GetComponent<LocalObjectValue>().CmdAddCarrotSeed(seedValue);
        NetworkServer.Destroy(gameObject);
    }
}