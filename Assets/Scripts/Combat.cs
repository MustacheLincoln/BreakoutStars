using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

namespace BreakoutStars
{
    public class Combat : NetworkBehaviour
    {
        private Character character;
        private InputMaster inputMaster;
        private bool charging;

        private float baseDamage = 10;
        private float maxDamage = 50;
        private float damage = 10;
        private float chargeSpeed = 20;

        private void OnEnable()
        {
            character = GetComponent<Character>();
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
            if (character.health > 0)
            {
                Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + transform.forward + transform.up, transform.localScale / 1.25f, transform.rotation);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    if (hitColliders[i].GetComponent<Character>())
                        if (hitColliders[i].GetComponent<NetworkObject>().IsOwner == false)
                        {
                            Vector3 direction = (transform.position - hitColliders[i].transform.position).normalized;
                            hitColliders[i].GetComponent<Character>().TakeDamage(damage, direction);
                        }
                    i++;
                }
                charging = false;
                damage = baseDamage;
                character.PlayAnimation("Swoosh");
            }

        }
    }

}
