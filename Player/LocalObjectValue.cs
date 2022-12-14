using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LocalObjectValue : NetworkBehaviour
{
    [SyncVar] public int tomatoSeedsInBag;
    [SyncVar] public int carrotSeedInBag;

    [Command(requiresAuthority = false)]
    public void CmdAddTomatoSeed(int value)
    {
        tomatoSeedsInBag += value;
    }
    
    [Command(requiresAuthority = false)]
    public void CmdAddCarrotSeed(int value)
    {
        carrotSeedInBag += value;
    }
    
    public void FullResetValue()
    {
        tomatoSeedsInBag = 0;
        carrotSeedInBag = 0;
    }
    
}

