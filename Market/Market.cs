using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Class.ObjectManager;
using Mirror;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class Market : NetworkBehaviour
{
    private ObjectData objectData;
    private GameObject gameControllerObject;
    private GameController gameController;
    
    private void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameManager");
        objectData = gameControllerObject.GetComponent<ObjectData>();
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    [Command(requiresAuthority = false)]
    public void InitMarket(GameObject player)
    {
        var objectManager = player.GetComponent<ObjectManager>();
        if (objectData.objectIdVegetableArray.Contains(objectManager.equipedObjectId))
        {
            switch (objectManager.equipedObjectId)
            {
                case ObjectId.Tomato:
                    gameController.score += 100;
                    break;
                case ObjectId.Carrot:
                    gameController.score += 100;
                    break;
            }

            objectManager.DeleteEquipedObject();
        }
    }
}
