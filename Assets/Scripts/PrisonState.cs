using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreakoutStars
{
    public class PrisonState : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        public void Start()
        {
            if (IsServer)
            {
                var clients = NetworkManager.Singleton.ConnectedClientsList;
                for (int i = 0; i < clients.Count; i++)
                {
                    GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                    go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clients[i].ClientId);
                }

            }

        }

    }
}

