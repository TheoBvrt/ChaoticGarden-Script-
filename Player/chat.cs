using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class chat : NetworkBehaviour
{
    private Setup setup;
    public TMP_Text messageBox;

    [SyncVar(hook = nameof(OnNewMessage))] public string message;
    void OnNewMessage(string _old, string _new)
    {
        messageBox.text = message;
    }
}
