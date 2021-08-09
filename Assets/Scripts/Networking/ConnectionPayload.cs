using System;

namespace BreakoutStars
{
    [Serializable]
    public class ConnectionPayload
    {
        public string clientGUID;
        public int clientScene = -1;
        public string playerName;
        public string password;
    }
}


