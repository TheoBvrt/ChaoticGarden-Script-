using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using Mirror;
using UnityEngine;

public class ObjectManager : NetworkBehaviour
{
    [SerializeField] [SyncVar(hook = nameof(ToolSwitching))]  public ObjectId equipedObjectId = ObjectId.Nothing;
    [SerializeField] [SyncVar] public bool objectEquiped;
    [SerializeField] private GameObject[] localObject;
    [SerializeField] private GameObject[] spawnableObject;
    [SerializeField] private GameObject spawnPos;
    [SyncVar] private Vector3 spawnPoint;
    private Interaction interaction;
    private void Start()
    {
        interaction = GetComponent<Interaction>();
        spawnPoint = interaction.hitPoint;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Action");
        }
    }

    [Command]
    public void ToolSelector(ObjectId objectId)
    {
        equipedObjectId = objectId;
    }
    
    [Command(requiresAuthority = false)]
    public void DropObject()
    {
        if (objectEquiped == false)
        {                
            var tmp = equipedObjectId;
            var index = 0;
            var localObjectValue = GetComponent<LocalObjectValue>();
            
            for (int i = 0; i < localObject.Length; i++)
            {
                localObject[i].SetActive(false);
            }

            equipedObjectId = ObjectId.Nothing;
            objectEquiped = false;
            while (spawnableObject[index].GetComponent<ObjectIdentification>().objectId != tmp)
                index++;
            var objectInstance = Instantiate(spawnableObject[index]);
            //objectInstance.transform.position = spawnPoint;
            objectInstance.transform.position = spawnPos.transform.position;
            objectInstance.transform.rotation = spawnPos.transform.rotation;
            SetNewValue(objectInstance, localObjectValue);
            localObjectValue.FullResetValue();
            NetworkServer.Spawn(objectInstance);
        }
    }
    
    private void ToolSwitching(ObjectId _new, ObjectId _old)
    {
        var index = 0;
        if (equipedObjectId == ObjectId.Nothing)
        {
            for (int i = 0; i < localObject.Length; i++)
            {
                localObject[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < localObject.Length; i++)
            {
                localObject[i].SetActive(false);
            }
            while (localObject[index].GetComponent<ObjectIdentification>().objectId != equipedObjectId)
                index++;
            localObject[index].SetActive(true);
        }
    }

    private void SetNewValue(GameObject instantiatedObject, LocalObjectValue localObjectValue)
    {
        ObjectIdentification objectIdentification = instantiatedObject.GetComponent<ObjectIdentification>();
        if (objectIdentification.objectId == ObjectId.TomatoSeedBag)
            instantiatedObject.GetComponent<TomatoSeedBag>().seedValue = localObjectValue.tomatoSeedsInBag;
        if (objectIdentification.objectId == ObjectId.CarrotSeedBag)
            instantiatedObject.GetComponent<CarrotSeedBag>().seedValue = localObjectValue.carrotSeedInBag;
    }
}
