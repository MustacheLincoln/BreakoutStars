using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Renderer pants;
    private ParticleSpawner particleSpawner;

    [Range(0, 100)] public float health = 100;
    [Range(0, 100)] public float energy = 100;
    private float energyRegen = 2.5f;


    private void Start()
    {
        pants.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        particleSpawner = GetComponent<ParticleSpawner>();
    }

    private void LateUpdate()
    {
        health = Mathf.Clamp(health, 0, 100);
        energy = Mathf.Clamp(energy, 0, 100);

        if (health <= 0)
            KO();
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
        transform.rotation = Quaternion.Euler(-90, transform.rotation.y, transform.rotation.z);
    }

    internal void TakeDamage(float damage)
    {
        health -= damage;
    }
}
