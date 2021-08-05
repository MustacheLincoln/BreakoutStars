using MLAPI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Text;
using MLAPI.Transports.UNET;

namespace BreakoutStars
{
    public class MultiplayerManager : NetworkBehaviour
    {
        [SerializeField] private TMP_InputField IPInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private GameObject entryUI;
        [SerializeField] private GameObject leaveButton;

        private static Dictionary<ulong, PlayerData> clientData;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        public void Host()
        {
            clientData = new Dictionary<ulong, PlayerData>();
            clientData[NetworkManager.Singleton.LocalClientId] = new PlayerData(nameInputField.text);
            
            // Hook up password approval check
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost(new Vector3(0, 0, -2), Quaternion.Euler(0, 90, 0));
        }

        public void Join()
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                ip = IPInputField.text,
                password = passwordInputField.text,
                playerName = nameInputField.text
            });

            byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);

            // Set connection payload ready to send to the server to validate
            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
            // Set IP ready to attempt to join
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = IPInputField.text;
            NetworkManager.Singleton.StartClient();
        }

        public void Leave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }

            entryUI.SetActive(true);
            leaveButton.SetActive(false);
        }

        public static PlayerData? GetPlayerData(ulong clientId)
        {
            if(clientData.TryGetValue(clientId, out PlayerData playerData))
            {
                return playerData;
            }

            return null;
        }
        private void HandleServerStarted()
        {
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                entryUI.SetActive(false);
                leaveButton.SetActive(true);
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                clientData.Remove(clientId);
            }

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                entryUI.SetActive(true);
                leaveButton.SetActive(false);
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            string payload = Encoding.ASCII.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            bool approveConnection = connectionPayload.password == passwordInputField.text;

            //Client Spawn Position
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            if (approveConnection)
            {
                switch (NetworkManager.Singleton.ConnectedClients.Count)
                {
                    case 1:
                        spawnPos = new Vector3(0, 0, 0);
                        spawnRot = Quaternion.Euler(0, 90, 0);
                        break;
                    case 2:
                        spawnPos = new Vector3(2, 0, 0);
                        spawnRot = Quaternion.Euler(0, 90, 0);
                        break;
                    case 3:
                        spawnPos = new Vector3(-2, 0, 0);
                        spawnRot = Quaternion.Euler(0, 90, 0);
                        break;
                }

                clientData[clientId] = new PlayerData(connectionPayload.playerName);
            }

            callback(true, null, approveConnection, spawnPos, spawnRot);
        }

        
    }

}

