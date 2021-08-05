using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

namespace BreakoutStars
{
    public class Movement : NetworkBehaviour
    {
        private Character character;
        private Transform camTransform;
        private InputMaster inputMaster;
        private CharacterController mover;

        private Vector2 input;

        private Vector3 camF;
        private Vector3 camR;

        private Vector3 intent;
        private Vector3 velocity;
        private Vector3 velocityXZ;
        private float speed = 10;
        private float turnSpeed = 8;
        private float accel = 10;
        private float jumpSpeed = 12;
        private float dashSpeed = 30;
        private float walkSpeedMultiplier = .4f;

        private float grav = 50;
        private bool grounded = false;
        private bool walking = false;

        private void Awake()
        {
            character = GetComponent<Character>();
            mover = GetComponent<CharacterController>();
            camTransform = Camera.main.transform;
            inputMaster = new InputMaster();
        }

        private void OnEnable()
        {
            inputMaster.Player.Jump.Enable();
            inputMaster.Player.Jump.performed += Jump;

            inputMaster.Player.Walk.Enable();
            inputMaster.Player.Walk.performed += Walk;

            inputMaster.Player.Dash.Enable();
            inputMaster.Player.Dash.performed += Dash;

            inputMaster.Player.AnalogMovement.Enable();
            inputMaster.Player.AnalogMovement.performed += CaptureMovement;

            inputMaster.Player.DigitalMovement.Enable();
            inputMaster.Player.DigitalMovement.performed += CaptureMovement;
        }

        private void Update()
        {
            if (!IsOwner)
                this.enabled = false;

            character.RegenEnergy(mover.velocity.magnitude);
            CalculateMovement();
            CalculateCam();
            DetectGround();
            CalculateGravity();
            DoMove();
        }

        private void Dash(InputAction.CallbackContext obj)
        {
            int cost = 10;
            if (character.energy >= cost)
            {
                velocity = dashSpeed * transform.forward;
                character.Dash(cost);
            }
        }

        private void Walk(InputAction.CallbackContext obj)
        {
            walking = obj.ReadValueAsButton();
        }

        private void CaptureMovement(InputAction.CallbackContext obj)
        {
            input = obj.ReadValue<Vector2>();
        }

        private void CalculateMovement()
        {
            if (character.health > 0)
                input = Vector2.ClampMagnitude(input, 1);
            intent = camF * input.y + camR * input.x;

            if (input.magnitude > 0)
            {
                Quaternion rot = Quaternion.LookRotation(intent);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
            }

            velocityXZ = velocity;
            velocityXZ.y = 0;

            if (grounded)
            {
                if (walking)
                {
                    velocityXZ = Vector3.Lerp(velocityXZ, intent * input.magnitude * speed * walkSpeedMultiplier, accel * Time.deltaTime);
                }
                else
                {
                    velocityXZ = Vector3.Lerp(velocityXZ, intent * input.magnitude * speed, accel * Time.deltaTime);
                }
            }

            velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
        }

        private void CalculateCam()
        {
            camF = camTransform.forward;
            camR = camTransform.right;
            camF.y = 0;
            camR.y = 0;
            camF = camF.normalized;
            camR = camR.normalized;
        }

        private void DetectGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.left + Vector3.up * 0.1f, -Vector3.up, out hit, 0.3f) ||
                Physics.Raycast(transform.position + Vector3.right + Vector3.up * 0.1f, -Vector3.up, out hit, 0.3f) ||
                Physics.Raycast(transform.position + Vector3.back + Vector3.up * 0.1f, -Vector3.up, out hit, 0.3f) ||
                Physics.Raycast(transform.position + Vector3.forward + Vector3.up * 0.1f, -Vector3.up, out hit, 0.3f))
                grounded = true;
            else
                grounded = false;
        }

        private void CalculateGravity()
        {
            velocity.y -= grav * Time.deltaTime;
            if (grounded == true)
                velocity.y = Mathf.Clamp(velocity.y, -10, 50);
            velocity.y = Mathf.Clamp(velocity.y, -50, 50);
            //Temporary Respawn
            if (transform.position.y <= -50)
            {
                velocity.y = -10;
                mover.enabled = false;
                transform.position = new Vector3(0, 10, 0);
                mover.enabled = true;
            }
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            int cost = 10;
            if (character.energy >= cost && grounded)
            {
                velocity.y = jumpSpeed;
                character.Jump(cost);
            }
        }

        internal void Knockback(float damage, Vector3 direction)
        {
            velocity = damage * -direction;
        }

        private void DoMove()
        {
            mover.Move(velocity * Time.deltaTime);
        }
    }

}
