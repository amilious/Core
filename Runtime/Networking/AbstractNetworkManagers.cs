using System;
using Amilious.Core.Authentication;
using Amilious.Core.Chat;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.User;

namespace Amilious.Core.Networking {
    
    public abstract class AbstractNetworkManagers : AmiliousBehavior {

        public delegate void ChatManagerRegisteredDelegate(IChatManager chatManager);

        public delegate void UserManagerRegisteredDelegate(IUserIdentityManager userManager);

        public delegate void GroupManagerRegisteredDelegate(IGroupIdentityManager groupManager);

        public delegate void AuthenticatorRegisteredDelegate(IAmiliousAuthenticator authenticator);

        public abstract event Action OnServerStarted;

        public abstract event Action OnServerStopped;

        public abstract event Action OnClientStarted;

        public abstract event Action OnClientStopped;

        public abstract  IUserIdentityManager UserManager { get; }
        
        public abstract  IGroupIdentityManager GroupManager { get; }
        
        public abstract  IChatManager ChatManager { get; }
        
        public abstract IAmiliousAuthenticator Authenticator { get; }
        
        public abstract bool IsServer { get; }
        
        public abstract bool IsClient { get; }
        
        public abstract bool IsServerOnly { get; }
        
        public abstract bool IsClientOnly { get; }
        
        public abstract bool IsOffline { get; }

        public abstract bool IsHost { get; }

        public abstract void OnChatManagerRegistered(ChatManagerRegisteredDelegate callback);

        public abstract void OnUserManagerRegistered(UserManagerRegisteredDelegate callback);

        public abstract void OnGroupManagerRegistered(GroupManagerRegisteredDelegate callback);

        public abstract void OnAuthenticatorRegistered(AuthenticatorRegisteredDelegate callback);

    }
    
}