using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectData : MonoBehaviour
{
    //objectID array of seedBag
    public GameObject[] gameObjectArray;
    public Dictionary<ObjectId, GameObject> chaoticGardenObjects = new Dictionary<ObjectId, GameObject>();
    public static ObjectId[] objectIdSeedBagArray = new[] { ObjectId.TomatoSeedBag, ObjectId.TomatoSeedBag };
    public ObjectId[] objectIdVegetableArray = new[] { ObjectId.Carrot, ObjectId.Tomato };

    private void Start()
    {
        for (int i = 0; i < gameObjectArray.Length ; i++)
            chaoticGardenObjects.Add(gameObjectArray[i].GetComponent<ObjectIdentification>().objectId, gameObjectArray[i].gameObject);
    }
}
