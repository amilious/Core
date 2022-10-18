/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Amilious.Core.Users;
using Amilious.Core.Saving;
using FishNet.Transporting;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using FishNet.Object.Synchronizing;

namespace Amilious.Core.FishNet.Users {
    
    [RequireComponent(typeof(IdentityDataManager))]
    public class FishNetIdentityManager : NetworkBehaviour, IIdentityManager {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private IdentityDataManager _identityDataManager;
        private int localUserId;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        public event IIdentityManager.UserConnectionChangedDelegate OnUserConnectionChanged;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Sync Variables /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This dictionary is used to store all of the user's identities.
        /// </summary>
        [SyncObject] 
        private readonly SyncDictionary<int, UserIdentity> _userLookup = new SyncDictionary<int, UserIdentity>();
        
        /// <summary>
        /// This hash set is used to store all of the online user ids.
        /// </summary>
        [SyncObject] private readonly SyncHashSet<int> _online = new SyncHashSet<int>();

        /// <summary>
        /// This dictionary is used to store all of the user's friends on the client.
        /// </summary>
        private static HashSet<int> _friends = new HashSet<int>();
        
        /// <summary>
        /// This hash set is used to store all of the user's blocked users on the client.
        /// </summary>
        private static HashSet<int> _blocked = new HashSet<int>();

        [SyncVar(Channel = Channel.Reliable)] 
        private string _serverIdentifier;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property is used to get the server identifier.
        /// </summary>
        public string ServerIdentifier => _serverIdentifier;

        /// <inheritdoc />
        public IEnumerable<UserIdentity> Identities => _userLookup.Values;

        /// <inheritdoc />
        public IEnumerable<UserIdentity> Friends => _userLookup.Values; //TODO: update

        /// <inheritdoc />
        public IEnumerable<UserIdentity> BlockedUsers { get; } = null; //TODO: update

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Monobehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            _online.OnChange += OnlineChanged;
            if(!IsServer) return; //load on server only
            _identityDataManager = GetComponent<IdentityDataManager>();
            _serverIdentifier = _identityDataManager.Server_GetServerIdentifier();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Interface Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public virtual bool TryGetIdentity(int id, out UserIdentity identity) {
            var found = _userLookup.TryGetValueFix(id, out identity);
            if(!found) identity = UserIdentity.DefaultUser;
            return found;
        }

        /// <inheritdoc />
        public bool TryGetIdentity(string userName, out UserIdentity identity) {
            foreach(var ident in _userLookup.Values) {
                if(!ident.UserName.Equals(userName,StringComparison.InvariantCultureIgnoreCase)) continue;
                identity = ident;
                return true;
            }
            identity = UserIdentity.DefaultUser;
            return false;
        }

        /// <inheritdoc />
        public virtual UserIdentity GetIdentity() {
            if(!NetworkManager.TryGetLocalUserId(out var id)) return UserIdentity.DefaultUser;
            return TryGetIdentity(id, out var identity) ? identity : UserIdentity.DefaultUser;
        }

        /// <inheritdoc />
        public virtual bool CanSendMessageTo(int sender, int recipient) {
            return !_identityDataManager.Server_HasBlocked(sender, recipient) && 
                   !_identityDataManager.Server_HasBlocked(recipient, sender) && 
                   _online.Contains(recipient);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region FishNet Only Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void OnlineChanged(SyncHashSetOperation op, int item, bool server) {
            if(server) return;
            var online = op != SyncHashSetOperation.Remove;
            if(!TryGetIdentity(item, out var identity)) return;
            OnUserConnectionChanged?.Invoke(identity,online);
        }
        
        public override void OnStartServer() {
            base.OnStartServer();
            //register authenticator
            if(_identityDataManager == null) _identityDataManager = GetComponent<IdentityDataManager>();
            NetworkManager.ServerManager.OnAuthenticationResult += OnAuthenticationResult;
            NetworkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
            //load the users
            foreach(var id in _identityDataManager.Server_GetStoredUserIds()) UpdateIdentity(id);
        }

        private void OnRemoteConnectionState(NetworkConnection con, RemoteConnectionStateArgs args) {
            if(!con.TryGetUserId(out var userId)) return;
            if(args.ConnectionState != RemoteConnectionState.Stopped) return;
            //update disconnect time
            _identityDataManager.Server_StoreUserData(userId,UserIdentity.LAST_DISCONNECTED_KEY,DateTime.UtcNow);
            UpdateIdentity(userId);
            _online.Remove(userId);
        }

        /// <summary>
        /// This method should be called when the user identity information is updated.
        /// </summary>
        /// <param name="id">The updated identity id.</param>
        [Server]
        public void UpdateIdentity(int id) {
            if(!_identityDataManager.Server_TryGetUserIdentity(id, out var identity)) {
                _userLookup.Remove(id); 
                return;
            }
            //update the user info
            if(!_userLookup.TryGetValueFix(id, out var sIdentity) || identity != sIdentity) {
                _userLookup[id] = identity;
                _userLookup.Dirty(id);
            } 
        }
        
        /// <summary>
        /// This method will be called when a user is authenticated on the server.
        /// </summary>
        /// <param name="con">The user's connection.</param>
        /// <param name="authenticated">True if the user was successfully authenticated, otherwise false.</param>
        private void OnAuthenticationResult(NetworkConnection con, bool authenticated) {
            if(!authenticated) return;
            if(!con.TryGetUserId(out var id)) return;
            if(!_online.Contains(id)) _online.Add(id);
            _identityDataManager.Server_StoreUserData(id,UserIdentity.LAST_CONNECTED_KEY,DateTime.UtcNow);
            if(_userLookup.ContainsKey(id)) {
                UpdateIdentity(id);
                return;
            }
            if(!_identityDataManager.Server_TryGetUserIdentity(id,out var identity))return;
            _userLookup.Add(id,identity);
        }
        
        /// <summary>
        /// This method is used to get a <see cref="NetworkConnection"/> from a <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identity">The identity that you want to get the connection for.</param>
        /// <param name="connection">The connection associated with the given identity</param>
        /// <returns>True if able to find a connection for the given identity, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        [Server]
        public virtual bool TryGetConnection(UserIdentity identity, out NetworkConnection connection) {
            return identity.TryGetConnection(NetworkManager,out connection);
        }

        /// <summary>
        /// This method is used to get a <see cref="NetworkConnection"/> from a <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identityId">The identity id that you want to get the connection for.</param>
        /// <param name="connection">The connection associated with the given identity id</param>
        /// <returns>True if able to find a connection for the given identity id, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        [Server]
        public virtual bool TryGetConnection(int identityId, out NetworkConnection connection) {
            if(TryGetIdentity(identityId, out var identity)) 
                return identity.TryGetConnection(NetworkManager,out connection);
            connection = null;
            return false;
        }

        /// <summary>
        /// This method is used to get a <see cref="UserIdentity"/> from the given <see cref="NetworkConnection"/>.
        /// </summary>
        /// <param name="connection">The connection that you want to get the <see cref="UserIdentity"/> for.</param>
        /// <param name="identity">The <see cref="UserIdentity"/> associated with the given connection.</param>
        /// <returns>True if a <see cref="UserIdentity"/> was found for the given connection, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        [Server]
        public virtual bool TryGetIdentity(NetworkConnection connection, out UserIdentity identity) {
            if(connection.TryGetUserId(out var identityId)) 
                return TryGetIdentity(identityId, out identity);
            identity = default;
            return false;
        }

        /// <inheritdoc />
        [Server]
        public virtual bool TrySetAuthority(UserIdentity identity, int value) => TrySetAuthority(identity.Id, value);

        /// <inheritdoc />
        [Server]
        public virtual bool TrySetAuthority(int userId, int value) {
            if(!_identityDataManager.Server_IsUserIdValid(userId)) return false;
            _identityDataManager.Server_StoreUserData(userId,UserIdentity.AUTHORITY_KEY,value);
            UpdateIdentity(userId);
            return true;
        }

        /// <inheritdoc />
        [Server]
        public virtual bool TryUpdateUserName(UserIdentity identity, string userName) =>
            TryUpdateUserName(identity.Id, userName);   
        
        /// <inheritdoc />
        [Server]
        public virtual bool TryUpdateUserName(int userId, string userName) {
            if(!_identityDataManager.Server_IsUserIdValid(userId)) return false;
            //make sure the user name is not being used
            if(_identityDataManager.Server_TryGetIdFromUserName(userName, out _)) return false;
            _identityDataManager.Server_StoreUserData(userId,UserIdentity.USER_NAME_KEY,userName);
            UpdateIdentity(userId);
            return true;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region RPCs ///////////////////////////////////////////////////////////////////////////////////////////////////

        [TargetRpc]
        private void SendFriendsToClient(NetworkConnection con, int[] friendIds) {
            _friends.Clear();
            foreach(var id in friendIds) _friends.Add(id);
        }

        [TargetRpc]
        private void SendBlockedToClient(NetworkConnection con, int[] blockedIds) {
            _blocked.Clear();
            foreach(var id in blockedIds) _blocked.Add(id);
        }

        [TargetRpc]
        private void SendBlockedUpdated(NetworkConnection con, int userId, bool isBlocked) {
            if(isBlocked && !_blocked.Contains(userId)) _blocked.Add(userId);
            if(!isBlocked && _blocked.Contains(userId)) _blocked.Remove(userId);
        }

        [TargetRpc]
        private void SendFriendUpdated(NetworkConnection con, int userId, bool isFriend) {
            if(isFriend&&!_friends.Contains(userId)) _friends.Add(userId);
            if(!isFriend && _friends.Contains(userId)) _friends.Remove(userId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateFriendsOnServer(int userId, bool isFriend, NetworkConnection con = null) {
            if(!con.TryGetUserId(out var id)) return;
            //TODO: update body
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateBlockedOnServer(int userId, bool isBlocked, NetworkConnection con = null) {
            if(!con.TryGetUserId(out var id)) return;
            //TODO: update body
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}