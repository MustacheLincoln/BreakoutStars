using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class Combat : MonoBehaviour
{
    private Player player;
    private InputMaster inputMaster;

    private float baseDamage = 10;

    private void OnEnable()
    {
        player = GetComponent<Player>();
        inputMaster = new InputMaster();
        inputMaster.Player.Attack.Enable();
        inputMaster.Player.Attack.performed += Attack;
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + transform.forward + transform.up, transform.localScale / 2, Quaternion.identity);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Player"))
                if (hitColliders[i].GetComponent<NetworkObject>().IsOwner == false)
                    hitColliders[i].GetComponent<Player>().TakeDamage(baseDamage);

            i++;
        }
    }
}
