using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : NetworkBehaviour
{
    [SerializeField] [SyncVar(hook = nameof(ToolSwitching))]  public ObjectId equipedObjectId = ObjectId.Nothing;
    [SerializeField] [SyncVar] public bool objectEquiped;
    [SerializeField] private GameObject[] localObject;
    [SerializeField] private GameObject[] spawnableObject;
    [SerializeField] private GameObject spawnPos;
    [SerializeField] private Image currentItemIcon;
    private GameController gameController;
    private Interaction interaction;
    private void Start()
    {
        interaction = GetComponent<Interaction>();
    }
    

    [Command]
    public void ToolSelector(ObjectId objectId)
    {
        equipedObjectId = objectId;
    }
    
    [Command(requiresAuthority = false)]
    public void DropObject(DropType dropType)
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
            if (dropType == DropType.DropOnPoint && interaction.hitDistance <= interaction.maxRange)
            {
                objectInstance.transform.position = new Vector3(interaction.hitPoint.x, 
                    interaction.hitPoint.y + (100 * Time.deltaTime), interaction.hitPoint.z);
            }
            else
            {
                objectInstance.transform.position = spawnPos.transform.position;
                objectInstance.transform.rotation = spawnPos.transform.rotation; 
            }
            
            SetNewValue(objectInstance, localObjectValue);
            NetworkServer.Spawn(objectInstance);
            localObjectValue.FullResetValue();
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

            if (isLocalPlayer)
                currentItemIcon.sprite = null;
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
            if (isLocalPlayer)
                currentItemIcon.sprite = localObject[index].GetComponent<ObjectIdentification>().icon;
        }
    }

    public void DeleteEquipedObject()
    {
        objectEquiped = false;
        equipedObjectId = ObjectId.Nothing;
        GetComponent<LocalObjectValue>().FullResetValue();
    }

    private void SetNewValue(GameObject instantiatedObject, LocalObjectValue localObjectValue)
    {
        ObjectIdentification objectIdentification = instantiatedObject.GetComponent<ObjectIdentification>();
        if (objectIdentification.objectId == ObjectId.TomatoSeedBag)
            instantiatedObject.GetComponent<TomatoSeedBag>().SetNewValue(localObjectValue.tomatoSeedsInBag);
        if (objectIdentification.objectId == ObjectId.CarrotSeedBag)
            instantiatedObject.GetComponent<CarrotSeedBag>().SetNewValue(localObjectValue.carrotSeedInBag);
    }
}
