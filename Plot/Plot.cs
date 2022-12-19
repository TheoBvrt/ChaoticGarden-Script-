using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using Mirror;
using Telepathy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
using Object = System.Object;

public class Plot : NetworkBehaviour
{
    [SyncVar]
    public bool plotUsable = true;
    
    [SyncVar]
    public bool readyToRecolt = false;
    
    [SyncVar]
    public ObjectId currentSeed;

    [SyncVar] 
    public GameObject currentPlayer;
    
    public GameObject[] carrotModels;
    public GameObject[] tomatoModels;

    private GameController gameController;
    private ObjectData objectData;
    
    private void Start()
    {
        DisableAllModel();
        plotUsable = true;
        gameController = GameObject.FindWithTag("GameManager").GetComponent<GameController>();
        objectData = GameObject.FindWithTag("GameManager").GetComponent<ObjectData>();
    }
    
    [Command(requiresAuthority = false)]
    public void PlotInit(ObjectId choice, GameObject player, int selectedSeedInBag)
    {
        if (plotUsable && readyToRecolt != true)
        {
            if (selectedSeedInBag > 0)
            {
                var playerObjectValue = player.GetComponent<LocalObjectValue>();
                plotUsable = false;
                currentSeed = choice;
                
                if (choice == ObjectId.TomatoSeedBag)
                    playerObjectValue.tomatoSeedsInBag--;
                if (choice == ObjectId.CarrotSeedBag)
                    playerObjectValue.carrotSeedInBag--;
                PlotManager(currentSeed);
            }
            return;
        }

        if (readyToRecolt)
        {
            currentPlayer = player;
            GiveItem(currentSeed, player);
            Recolt();
            readyToRecolt = false;
            plotUsable = true;
            currentSeed = ObjectId.Nothing;
        }
    }

    IEnumerator SowSeed(string seedName, int seedDelay, GameObject[] step)
    {
        for (int i = 0; i < 3; i++)
        {
            DisableModel(step);
            ActiveModel(step[i]);
            yield return new WaitForSeconds(seedDelay);
        }
        readyToRecolt = true;
    }
    
    private void ActiveModel(GameObject model)
    {
        model.SetActive(true);
    }
    
    private void DisableModel(GameObject[] step)
    {
        for (var i = 0; i < step.Length; i++)
        {
            step[i].SetActive(false);
        }
    }
    
    private void DisableAllModel()
    {
        DisableModel(carrotModels);
        DisableModel(tomatoModels);  
    }
    
    [ClientRpc]
    private void Recolt()
    {
        DisableAllModel();
    }

    private void GiveItem(ObjectId seed, GameObject player)
    {
        
        var localObjectValue = player.GetComponent<LocalObjectValue>();
        switch (seed)
        { 
            case ObjectId.TomatoSeedBag:
                localObjectValue.tomato += CheckPlayerInventory(gameController.giveTomatoValue, localObjectValue.tomato, 
                    ObjectId.Tomato);
                break;
            case ObjectId.CarrotSeedBag:
                localObjectValue.carrot += CheckPlayerInventory(gameController.giveCarrotValue, localObjectValue.carrot, 
                    ObjectId.Carrot);
                break;
        }
    }
    
    [ClientRpc]
    private void PlotManager(ObjectId seed)
    {
        switch (seed)
        {
            case ObjectId.TomatoSeedBag:
                StartCoroutine(SowSeed("Tomato", (int)gameController.tomatoSeedUpdate, tomatoModels));
                break;
            case ObjectId.CarrotSeedBag:
                StartCoroutine(SowSeed("Carrot", (int)gameController.carrotSeedUpdate, carrotModels));
                break;
        }
    }

    private int CheckPlayerInventory(int gameControllerGiveValue, int localObjectValue, ObjectId objectId)
    {
        var giveValue = gameControllerGiveValue;
        var objectManager =  currentPlayer.GetComponent<ObjectManager>();
        if (localObjectValue != 0 || objectManager.objectEquiped)
        {
            print("Instancing...");
            var objectInstantiated = Instantiate(objectData.chaoticGardenObjects[objectId]);
            objectInstantiated.transform.position = currentPlayer.transform.Find("SpawnPos").position;
            NetworkServer.Spawn(objectInstantiated);
            return (0);
        }
        objectManager.equipedObjectId = objectId;
        objectManager.objectEquiped = true;
        print(objectId);
        return (giveValue);
    }
}