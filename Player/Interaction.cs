using System;
using System.Collections;
using System.Collections.Generic;
using Class.ObjectManager;
using Mirror;
using UnityEngine;

public class Interaction : NetworkBehaviour
{
    [SerializeField] private float rayRange;
    [SerializeField] private Camera mainCamera;
    [SyncVar] public Vector3 hitPoint;
    public bool onSwitching = false;
    public float maxRange;
    public float hitDistance;

    void Update()
    {
        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction * rayRange, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            hitDistance = Vector3.Distance(transform.position, hit.point);
            CmdHitPoint(hit.point);
            if (Input.GetKeyDown(KeyCode.E) && hitDistance <= maxRange)
            {
                if (hit.transform.gameObject.GetComponent<InitInteraction>() != null)
                {
                    hit.transform.gameObject.GetComponent<InitInteraction>().player = gameObject;
                    hit.transform.gameObject.GetComponent<InitInteraction>().StartInteraction(ActionType.Pickup);
                }
            }
            
            if (Input.GetMouseButtonDown(0) && hitDistance <= maxRange)
            {
                if (hit.transform.gameObject.GetComponent<InitInteraction>() != null)
                {
                    hit.transform.gameObject.GetComponent<InitInteraction>().player = gameObject;
                    hit.transform.gameObject.GetComponent<InitInteraction>().StartInteraction(ActionType.Interaction);
                }
            }
        }
    }

    [Command]
    private void CmdHitPoint(Vector3 point)
    {
        hitPoint = point;
    }
}
