using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ParticleSpawner : NetworkBehaviour
{
    public GameObject dash;
    public GameObject jump;

    public void Emit(GameObject type)
    {
        if (IsOwner)
        {
            Instantiate(type, transform.position, transform.rotation);
            SpawnParticleServerRpc(type.name);
        }
    }

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnParticleServerRpc(string name)
    {
        SpawnParticleClientRpc(name);
    }

    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnParticleClientRpc(string name)
    {
        if (!IsOwner)
        {
            if (name == dash.name)
                Instantiate(dash, transform.position, transform.rotation);
            if (name == jump.name)
                Instantiate(jump, transform.position, transform.rotation);
        }
    }
}
