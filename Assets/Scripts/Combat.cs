using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class Combat : NetworkBehaviour
{
    private Player player;
    private InputMaster inputMaster;
    private bool charging;

    private float baseDamage = 10;
    private float maxDamage = 50;
    private float damage = 10;
    private float chargeSpeed = 20;

    private void OnEnable()
    {
        player = GetComponent<Player>();
        inputMaster = new InputMaster();
        inputMaster.Player.Attack.Enable();
        inputMaster.Player.Attack.started += Charge;
        inputMaster.Player.Attack.canceled += Attack;
    }

    private void OnDisable()
    {
        charging = false;
        inputMaster.Player.Attack.Disable();
    }

    private void Update()
    {
        if (!IsOwner)
            this.enabled = false;

        if (charging)
        {
            damage += Time.deltaTime * chargeSpeed;
            damage = Mathf.Clamp(damage, baseDamage, maxDamage);
        }
    }

    private void Charge(InputAction.CallbackContext obj)
    {
        damage = baseDamage;
        charging = true;
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        if (player.health > 0)
        {
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + transform.forward + transform.up, transform.localScale / 2, Quaternion.identity);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].CompareTag("Player"))
                    if (hitColliders[i].GetComponent<NetworkObject>().IsOwner == false)
                    {
                        Vector3 direction = (transform.position - hitColliders[i].transform.position).normalized;
                        hitColliders[i].GetComponent<Player>().TakeDamage(damage, direction);
                    }

                i++;
            }
            charging = false;
            damage = baseDamage;
        }

    }
}
