using System;
using Amilious.Core.Identity.User;
using Amilious.Core.Saving;

namespace Amilious.Core.Authentication {
    
    public interface IAmiliousAuthenticator {
        
        /// <summary>
        /// This event is triggered when the local client was rejected connection to the server.
        /// </summary>
        public event Action<string> OnConnectionRejected;
        
        /// <summary>
        /// This event is triggered when the local client successfully joined the server.
        /// </summary>
        public event Action<string> OnSuccessfulConnection;
        
        /// <summary>
        /// This property is used to get the authenticator's data manager.
        /// </summary>
        public UserIdentityDataManager DataManager { get; }

    }
    
}