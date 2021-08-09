using MLAPI.Serialization;

namespace BreakoutStars
{
    public struct LobbyPlayerState : INetworkSerializable
    {
        public ulong ClientId;
        public string PlayerName;
        public bool IsReady;

        public LobbyPlayerState(ulong clientId, string playerName, bool isReady)
        {
            ClientId = clientId;
            PlayerName = playerName;
            IsReady = isReady;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref ClientId);
            serializer.Serialize(ref PlayerName);
            serializer.Serialize(ref IsReady);
        }
    }
}

