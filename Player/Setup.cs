using System;
using JetBrains.Annotations;
using UnityEngine;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;
// ReSharper disable All

public class Setup : NetworkBehaviour
{
    public TextMeshPro pseudo;
    public bool stopMenu;
    public GameObject finishedMessage;
    
    [SerializeField] private Behaviour[] componentsToDisable;
    [SerializeField] private GameObject localText;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string playerName;
    void OnNameChanged(string _old, string _new)
    {
        pseudo.text = playerName;
        gameObject.transform.name = playerName;
    }

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public override void OnStartClient()
    {
        if (isLocalPlayer)
            localText.GetComponent<MeshRenderer>().enabled = false;
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        
        var tmp = "player" + Random.Range(0, 500);
        SetupPlayer(tmp, "" + tmp + " Viens de connecter !");
        base.OnStartClient();
        CmdAddPlayer();
    }
    
    private void CmdAddPlayer()
    {
        if (isLocalPlayer)
        {
            var gameController = GameObject.FindWithTag("GameManager").GetComponent<GameController>();
            gameController.AddPlayer(gameObject);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            var tmp = "Player " + Random.Range(0, 500);
            SetupPlayer(tmp, tmp + " à changer son nom !");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (stopMenu == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                stopMenu = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                stopMenu = false; 
            }
        }
    }

    [Command(requiresAuthority = false)]
    void SetupPlayer(string newName, string newMessage)
    {
        playerName = newName;
    }
    
}
