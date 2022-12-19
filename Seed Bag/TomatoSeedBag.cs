using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;

public class TomatoSeedBag : NetworkBehaviour
{
    //currentPlayer is player called StartInteraction;
    [SyncVar(hook = nameof(DeleteThis))] [SerializeField] protected GameObject currentPlayer;
    [SyncVar] [SerializeField] private int seedValue;
    
    private GameController gameController;
    private void Start()
    {
        gameController = GameObject.FindWithTag("GameManager").GetComponent<GameController>();
    }
    
    public void SetNewValue(int newSeedValue)
    {
        seedValue = newSeedValue;
    }
    
    [Command(requiresAuthority = false)]
    public void StartInteraction(GameObject player)
    {
        currentPlayer = player;
    }
    
    private void DeleteThis(GameObject _new, GameObject _old)
    {
        currentPlayer.GetComponent<LocalObjectValue>().CmdAddTomatoSeed(seedValue);
        NetworkServer.Destroy(gameObject);
    }
}
