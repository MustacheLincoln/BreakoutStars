using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ParticleSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject dash;

    public void Dash()
    {
        if (IsOwner)
        {
            Instantiate(dash, transform.position, transform.rotation);
            SpawnParticleServerRpc();
        }
    }

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnParticleServerRpc()
    {
        SpawnParticleClientRpc();
    }

    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnParticleClientRpc()
    {
        if(!IsOwner)
            Instantiate(dash, transform.position, transform.rotation);
    }
}
