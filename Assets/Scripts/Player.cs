using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Renderer pants;

    [Range(0, 100)] public float energy = 100;
    private float energyRegen = 2.5f;


    private void Start()
    {
        pants.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void LateUpdate()
    {
        energy = Mathf.Clamp(energy, 0, 100);
    }

    internal void Dash(int cost)
    {
        energy = energy - cost;
    }

    internal void Jump(int cost)
    {
        energy = energy - cost;
    }

    internal void RegenEnergy(float speed)
    {
        float regenRate = energyRegen * Time.deltaTime / speed;
        regenRate = Mathf.Clamp(regenRate, 0, energyRegen * Time.deltaTime);
        energy += regenRate;
    }
}
