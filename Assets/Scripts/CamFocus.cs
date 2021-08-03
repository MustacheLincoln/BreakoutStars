using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class CamFocus : NetworkBehaviour
{
    private GameObject target;

    private void Update()
    {
        if (!target)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
                if (player.GetComponent<NetworkObject>().IsOwner)
                    target = player;
        }
        else
            transform.position = target.transform.position;
    }
}
