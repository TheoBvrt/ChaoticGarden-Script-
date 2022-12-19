using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class.ObjectManager;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameController : NetworkBehaviour
{
    ////////////////////////////////////SyncVar
    
    //Seed data
    [SyncVar] public float tomatoSeedUpdate;
    [SyncVar] public float carrotSeedUpdate;
    
    [SyncVar] public int giveTomatoValue;
    [SyncVar] public int giveCarrotValue;
    
        //Seed starter value
    [SyncVar] public int tomatoSeedtarterValue;
    [SyncVar] public int carrotSeedStarterValue;
    
        //Game
    [SyncVar] public int gameTime;
    [SyncVar] public float score;

        //Other
    readonly SyncList<GameObject> playerList = new SyncList<GameObject>();
    public ObjectId[] seedIdInGame;

    ////////////////////////////////////LocalVar
    
    //Seed Data
    [SerializeField] private float tomatoSeedUpdateLocal;
    [SerializeField] private float carrotSeedUpdateLocal;
    
    [SerializeField] private int giveTomatoValueLocal;
    [SerializeField] private int giveCarrotValueLocal;
    
        //Seed starter value
    [SerializeField] private int tomatoSeedStarterValueLocal;
    [SerializeField] private int carrotSeedStarterValueLocal;

        //Other
    [SerializeField] private int gameDuration;
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject[] playersArray;
    [SerializeField] private GameObject finishedMessage;
    
    
    //Code
    private void Start()
    {
        if (isServer)
        {
            GameInit();
        }
    }

    [Server]
    private void GameInit()
    {
        tomatoSeedUpdate = tomatoSeedUpdateLocal;
        carrotSeedUpdate = carrotSeedUpdateLocal;

        tomatoSeedtarterValue = tomatoSeedStarterValueLocal;
        carrotSeedStarterValue = carrotSeedStarterValueLocal;

        giveTomatoValue = giveTomatoValueLocal;
        giveCarrotValue = giveCarrotValueLocal;
        //StartCoroutine(TimeManager());
    }

    IEnumerator TimeManager()
    {
        while (gameTime <= gameDuration)
        {
            yield return new WaitForSeconds(1);
            IncrementGameTime(); 
            if (gameTime == gameDuration)
                RpcStopGame();
        }
    }
    
    private void IncrementGameTime()
    {
        gameTime ++;
    }
    
    [ClientRpc]
    private void RpcStopGame()
    {
        finishedMessage.SetActive(true);
    }

    [Command(requiresAuthority = false)]
    public void AddPlayer(GameObject gameObject)
    {
        playerList.Add(gameObject);
    }
}
