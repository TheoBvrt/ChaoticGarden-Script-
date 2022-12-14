using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using Mirror;
using Telepathy;
using Unity.VisualScripting;
using UnityEngine;
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
    
    private void Start()
    {
        DisableAllModel();
        plotUsable = true;
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
            Recolt();
            readyToRecolt = false;
            plotUsable = true;
            currentSeed = 0;
        }
    }
    
    [ClientRpc]
    private void PlotManager(ObjectId seed)
    {
        switch (seed)
        {
            case ObjectId.TomatoSeedBag:
                StartCoroutine(SowSeed("Tomato", 2, tomatoModels));
                break;
            case ObjectId.CarrotSeedBag:
                StartCoroutine(SowSeed("Carrot", 2, carrotModels));
                break;
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
}