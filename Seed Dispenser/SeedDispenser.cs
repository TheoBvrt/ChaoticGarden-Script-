using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SeedDispenser : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void StartInteraction()
    {
        RpcInit();
    }

    [ClientRpc]
    private void RpcInit()
    {
        print("Seed dispensed");
    }
}
