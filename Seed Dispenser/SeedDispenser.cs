using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using Mirror;
using UnityEngine;

public class SeedDispenser : NetworkBehaviour
{
    private ObjectData objectData;
    public GameObject[] posPoint;
    [SerializeField] [SyncVar] private bool isUsable;

    private GameController gameController;
    private void Start()
    {
        if (isServer)
        {
            objectData = GameObject.FindWithTag("GameManager").GetComponent<ObjectData>();
            gameController = GameObject.FindWithTag("GameManager").GetComponent<GameController>();  
        }
    }

    [Command(requiresAuthority = false)]
    public void StartInteraction()
    {
        if (isUsable)
        {
            isUsable = false;
            GiveSeedBag();
            StartCoroutine(TimeBeforeReEnable());
        }
        else
        {
            print("Le dispenser est en train de recharger !");
        }
    }

    private void GiveSeedBag()
    {
        for (int i = 0; i < gameController.seedIdInGame.Length; i++)
        {
            var objectInstantiate = Instantiate(objectData.chaoticGardenObjects[gameController.seedIdInGame[i]]);
            objectInstantiate.transform.position = posPoint[i].transform.position;
            NetworkServer.Spawn(objectInstantiate);
            SetValue(objectInstantiate, ObjectId.Nothing);
        }
    }

    IEnumerator TimeBeforeReEnable()
    {
        yield return new WaitForSeconds(20);
        isUsable = true;
    }

    private void SetValue(GameObject objectInstantiate, ObjectId objectId)
    {
        ObjectIdentification objectIdentification = objectInstantiate.GetComponent<ObjectIdentification>();
        if (objectIdentification.objectId == ObjectId.TomatoSeedBag)
            objectInstantiate.GetComponent<TomatoSeedBag>().SetNewValue(gameController.tomatoSeedtarterValue);
        if (objectIdentification.objectId == ObjectId.CarrotSeedBag)
            objectInstantiate.GetComponent<CarrotSeedBag>().SetNewValue(gameController.carrotSeedStarterValue);
    }
}
