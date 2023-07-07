using System;

namespace Amilious.Core {
    
    [Serializable]
    public enum ClientServer : byte {
        Client = 0,
        Server = 1,
        Both = 2
    }
    
}