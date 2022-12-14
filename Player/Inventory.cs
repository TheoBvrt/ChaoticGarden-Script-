using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class Inventory : NetworkBehaviour
{

    [SyncVar]
    public int tomatoSeed;
    [SyncVar]
    public int carrotSeed;
    
    private void Start()
    {
        tomatoSeed = 5;
        carrotSeed = 5;
    }
}
