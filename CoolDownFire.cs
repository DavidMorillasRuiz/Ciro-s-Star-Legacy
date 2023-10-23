using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class CoolDownFire : MonoBehaviour
{
    public Image imageCooldown;
    public float cooldown = 5;
    bool isCooldown = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        //Ability#1
        if(isCooldown == true)
        {
            imageCooldown.fillAmount += 1 / cooldown * Time.deltaTime;
            if (imageCooldown.fillAmount >= 1)
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }
    }


    public void Fire(InputAction.CallbackContext callbackContext)
    {
       if(isCooldown == true)
       {
            Debug.Log("FireBall");
       }
       else
       {
            isCooldown = true;
            imageCooldown.fillAmount = 0;
       }
    }
}
