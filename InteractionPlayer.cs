using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;


public class InteractionPlayer : MonoBehaviour
{
    public Transform cam;
    public float playerActivateDistance;
    bool active = false;
    [SerializeField] LayerMask layerInteractuable;
    public GameObject dialogueMark;
    

    private void Start()
    {
        dialogueMark.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.position, cam.forward * playerActivateDistance, Color.red);

        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.forward, out hit, playerActivateDistance, layerInteractuable);
        
    }


    public void Interaction(InputAction.CallbackContext callbackContext)
    {
        dialogueMark.SetActive(true);

        if(callbackContext.performed && active == true)
        {    
            Debug.Log("Interaction <3");
        }
    }
}
