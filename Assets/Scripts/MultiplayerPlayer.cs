using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System;
using UnityEngine;

public class MultiplayerPlayer : NetworkBehaviour
{
    private void Awake()
    {
        name = "Player";
    }
}