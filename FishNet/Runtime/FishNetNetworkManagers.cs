
using System;
using FishNet;
using UnityEngine;
using FishNet.Managing;
using Amilious.Core.Chat;
using FishNet.Transporting;
using Amilious.Core.Networking;
using Amilious.Core.FishNet.Chat;
using System.Collections.Generic;
using Amilious.Core.Authentication;
using Amilious.Core.FishNet.Authentication;
using Amilious.Core.FishNet.Users;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using Amilious.Core.FishNet.Groups;

namespace Amilious.Core.FishNet {
    
    public class FishNetNetworkManagers : AbstractNetworkManagers {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private FishNetUserIdentityManager userManager;
        [SerializeField] private FishNetGroupIdentityManager groupManager;
        [SerializeField] private FishNetChatManager chatManager;
        [SerializeField] private IAmiliousAuthenticator authenticator;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly List<ChatManagerRegisteredDelegate> _chatManagerCallbacks = new ();
        private readonly List<UserManagerRegisteredDelegate> _userManagerCallbacks = new ();
        private readonly List<GroupManagerRegisteredDelegate> _groupManagerCallbacks = new ();
        private readonly List<AuthenticatorRegisteredDelegate> _authenticatorCallbacks = new();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public override event Action OnServerStarted;
        public override event Action OnServerStopped;
        public override event Action OnClientStarted;
        public override event Action OnClientStopped;

        /// <inheritdoc />
        public override IUserIdentityManager UserManager => userManager;
        /// <inheritdoc />
        public override IGroupIdentityManager GroupManager => groupManager;
        /// <inheritdoc />
        public override IChatManager ChatManager => chatManager;
        /// <inheritdoc />
        public override IAmiliousAuthenticator Authenticator => authenticator;
        /// <inheritdoc />
        public override bool IsServer => networkManager.IsServer;
        /// <inheritdoc />
        public override bool IsClient => networkManager.IsClient;
        /// <inheritdoc />
        public override bool IsServerOnly => networkManager.IsServerOnly;
        /// <inheritdoc />
        public override bool IsClientOnly => networkManager.IsClientOnly;
        /// <inheritdoc />
        public override bool IsOffline => networkManager.IsOffline;
        /// <inheritdoc />
        public override bool IsHost => networkManager.IsHost;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        private void Awake() {
            networkManager ??= InstanceFinder.NetworkManager;
            if(networkManager == null) return;
            //register manager callbacks
            if(chatManager == null)
                networkManager.RegisterInvokeOnInstance<FishNetChatManager>(ChatManagerRegistered);
            if(userManager == null)
                networkManager.RegisterInvokeOnInstance<FishNetUserIdentityManager>(UserManagerRegistered);
            if(groupManager==null)
                networkManager.RegisterInvokeOnInstance<FishNetGroupIdentityManager>(GroupManagerRegistered);
            if(authenticator==null)
                networkManager.RegisterInvokeOnInstance<FishNetAmiliousAuthenticator>(AuthenticatorRegistered);
            //register
            if(networkManager.ServerManager!=null) 
                networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
            if(networkManager.ClientManager != null)
                networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void OnChatManagerRegistered(ChatManagerRegisteredDelegate callback) {
            if(chatManager!=null) callback.Invoke(chatManager);
            else _chatManagerCallbacks.Add(callback);
        }

        /// <inheritdoc />
        public override void OnUserManagerRegistered(UserManagerRegisteredDelegate callback) {
            if(userManager!=null) callback.Invoke(userManager);
            else _userManagerCallbacks.Add(callback);
        }

        /// <inheritdoc />
        public override void OnGroupManagerRegistered(GroupManagerRegisteredDelegate callback) {
            if(groupManager!=null) callback.Invoke(groupManager);
            else _groupManagerCallbacks.Add(callback);
        }

        /// <inheritdoc />
        public override void OnAuthenticatorRegistered(AuthenticatorRegisteredDelegate callback) {
            if(authenticator!=null) callback.Invoke(authenticator);
            else _authenticatorCallbacks.Add(callback);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Messages ///////////////////////////////////////////////////////////////////////////////////////

        
        private void AuthenticatorRegistered(Component obj) {
            networkManager.UnregisterInvokeOnInstance<FishNetAmiliousAuthenticator>(ChatManagerRegistered);
            if(obj is not FishNetAmiliousAuthenticator manager) return;
            authenticator = manager;
            foreach(var callback in _authenticatorCallbacks) callback?.Invoke(authenticator);
            _authenticatorCallbacks.Clear();
        }
        
        private void GroupManagerRegistered(Component obj) {
            networkManager.UnregisterInvokeOnInstance<FishNetGroupIdentityManager>(ChatManagerRegistered);
            if(obj is not FishNetGroupIdentityManager manager) return;
            groupManager = manager;
            foreach(var callback in _groupManagerCallbacks) callback?.Invoke(manager);
            _groupManagerCallbacks.Clear();
        }

        private void ChatManagerRegistered(Component obj) {
            Debug.Log("receive chat manager");
            networkManager.UnregisterInvokeOnInstance<FishNetChatManager>(ChatManagerRegistered);
            if(obj is not FishNetChatManager manager) return;
            chatManager = manager;
            foreach(var callback in _chatManagerCallbacks) callback?.Invoke(manager);
            _chatManagerCallbacks.Clear();
        }
        
        private void UserManagerRegistered(Component obj) {
            networkManager.UnregisterInvokeOnInstance<FishNetUserIdentityManager>(ChatManagerRegistered);
            if(obj is not FishNetUserIdentityManager manager) return;
            userManager = manager;
            foreach(var callback in _userManagerCallbacks) callback?.Invoke(manager);
            _userManagerCallbacks.Clear();
        }
        
        private void OnServerConnectionState(ServerConnectionStateArgs stateArgs) {
            if(stateArgs.ConnectionState == LocalConnectionState.Started) OnServerStarted?.Invoke();
            if(stateArgs.ConnectionState == LocalConnectionState.Stopped) OnServerStopped?.Invoke();
        }

        private void OnClientConnectionState(ClientConnectionStateArgs stateArgs) {
            if(stateArgs.ConnectionState == LocalConnectionState.Started) OnClientStarted?.Invoke();
            if(stateArgs.ConnectionState == LocalConnectionState.Stopped) OnClientStopped?.Invoke();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}