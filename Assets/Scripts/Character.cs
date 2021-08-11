using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine.SceneManagement;

namespace BreakoutStars
{
    public class Character : NetworkBehaviour
    {
        [SerializeField] private Renderer pants;
        [SerializeField] private GameObject mesh;
        public Animation anim;
        public Movement movement;
        public ParticleSpawner particleSpawner;

        [Range(0, 100)] public float health = 100;
        [Range(0, 100)] public float energy = 100;
        private float energyRegen = 2.5f;

        private NetworkVariableString playerName = new NetworkVariableString("Player");
        private NetworkVariableColor playerColor = new NetworkVariableColor();
        private NetworkVariableFloat playerHealth = new NetworkVariableFloat(100);

        private void Start()
        {
            if (IsOwner)
            {
                pants.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                SetPlayerColorServerRpc(pants.material.color);
            }
            movement = GetComponent<Movement>();
            particleSpawner = GetComponent<ParticleSpawner>();
            anim = GetComponent<Animation>();
        }


        private void OnEnable()
        {
            playerName.OnValueChanged += OnNameChanged;
            playerColor.OnValueChanged += OnColorChanged;
            playerHealth.OnValueChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            playerName.OnValueChanged -= OnNameChanged;
            playerColor.OnValueChanged -= OnColorChanged;
            playerHealth.OnValueChanged -= OnHealthChanged;
        }

        private void OnNameChanged(string previousValue, string newValue)
        {
            name = newValue;
        }

        private void OnHealthChanged(float previousValue, float newValue)
        {
            health = newValue;
        }

        private void OnColorChanged(Color previousValue, Color newValue)
        {
            if (IsClient)
                pants.material.color = newValue;
        }

        private void LateUpdate()
        {
            health = Mathf.Clamp(health, 0, 100);
            energy = Mathf.Clamp(energy, 0, 100);

            if (health <= 0)
                KO();
        }

        internal void PlayAnimation(string animation)
        {
            anim.Play(animation);
            PlayAnimationServerRpc(animation);
        }

        [ServerRpc]
        private void PlayAnimationServerRpc(string animation)
        {
            PlayAnimationClientRpc(animation);
        }

        [ClientRpc]
        private void PlayAnimationClientRpc(string animation)
        {
            if (!IsOwner)
                anim.Play(animation);
        }
        internal void Dash(int cost)
        {
            energy = energy - cost;
            particleSpawner.Emit(particleSpawner.dash);
        }

        internal void Jump(int cost)
        {
            energy = energy - cost;
            particleSpawner.Emit(particleSpawner.jump);
        }

        internal void RegenEnergy(float speed)
        {
            float regenRate = energyRegen * Time.deltaTime / speed;
            regenRate = Mathf.Clamp(regenRate, 0, energyRegen * Time.deltaTime);
            energy += regenRate;
        }

        void KO()
        {
            //TEMP Do this with Animator
            mesh.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
            mesh.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }

        internal void TakeDamage(float damage, Vector3 direction)
        {
            health -= damage;
            SetPlayerHealthServerRpc(health);
            KnockbackServerRpc(damage, direction);
        }

        [ServerRpc]
        public void SetPlayerColorServerRpc(Color color)
        {
            playerColor.Value = color;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerHealthServerRpc(float health)
        {
            playerHealth.Value = health;
        }

        [ServerRpc(RequireOwnership = false)]
        private void KnockbackServerRpc(float damage, Vector3 direction)
        {
            KnockbackClientRpc(damage, direction);
        }

        [ClientRpc]
        private void KnockbackClientRpc(float damage, Vector3 direction)
        {
            movement.Knockback(damage, direction);
        }
    }

}


