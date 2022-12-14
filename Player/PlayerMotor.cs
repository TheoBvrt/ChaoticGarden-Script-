using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour
{
    private float speed = 10;
    private float sprintSpeed = 14;
    private float tmpSpeed;
    
    [SerializeField] private Transform cam;
    Rigidbody rb;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        tmpSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var direction = new Vector3(horizontal, 0f, vertical).normalized;
        
            var targetAngle = cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(direction * (speed * Time.deltaTime));
            }

            if (Input.GetKeyDown(KeyCode.Q) && GetComponent<ObjectManager>().objectEquiped)
            {
                CmdToolEquiped(0);
                GetComponent<ObjectManager>().DropObject();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = tmpSpeed;
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdToolEquiped(int value)
    {
        if (value == 0)
            GetComponent<ObjectManager>().objectEquiped = false;
        if (value == 1)
            GetComponent<ObjectManager>().objectEquiped = true;
    }
}
