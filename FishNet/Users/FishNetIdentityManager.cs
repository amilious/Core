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

using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Amilious.Core.Users;
using Amilious.Core.Saving;
using FishNet.Transporting;
using Amilious.Core.Extensions;
using FishNet.Object.Synchronizing;

namespace Amilious.Core.FishNet.Users {
    
    [RequireComponent(typeof(AbstractDataManager))]
    public class FishNetIdentityManager : NetworkBehaviour, IIdentityManager {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private AbstractDataManager _dataManager;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Sync Variables /////////////////////////////////////////////////////////////////////////////////////////

        [SyncObject(ReadPermissions = ReadPermission.Observers)] 
        private readonly SyncDictionary<int, UserIdentity> _userLookup = 
            new SyncDictionary<int, UserIdentity>();

        [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers)] 
        private string _serverIdentifier;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property is used to get the server identifier.
        /// </summary>
        public string ServerIdentifier => _serverIdentifier;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Monobehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            if(!IsServer) return; //load on server only
            _dataManager = GetComponent<AbstractDataManager>();
            _serverIdentifier = _dataManager.Server_GetServerIdentifier();
        }

        private void Start() {
            if(!IsServer) return; //load on server only
            //load the users
            foreach(var id in _dataManager.Server_GetStoredUserIds()) {
                if(!_dataManager.Server_TryReadUserData(id, UserIdentity.USER_NAME_KEY, out string userName))
                    continue;
                _userLookup[id] = new UserIdentity(id, userName);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Interface Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public virtual bool TryGetIdentity(int id, out UserIdentity identity) {
            return _userLookup.TryGetValueFix(id, out identity);
        }

        /// <inheritdoc />
        public virtual UserIdentity GetIdentity() {
            NetworkManager.ClientManager.Connection.TryGetUserId(out int id);
            TryGetIdentity(id, out var identity);
            return identity;
        }

        /// <inheritdoc />
        public virtual bool CanSendMessageTo(int sender, int recipient) {
            return _dataManager.Server_IsBlocked(sender, recipient) || 
                   _dataManager.Server_IsBlocked(recipient, sender);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region FishNet Only Methods ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get a <see cref="NetworkConnection"/> from a <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identity">The identity that you want to get the connection for.</param>
        /// <param name="connection">The connection associated with the given identity</param>
        /// <returns>True if able to find a connection for the given identity, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        public virtual bool TryGetConnection(UserIdentity identity, out NetworkConnection connection) {
            return identity.TryGetConnection(out connection);
        }

        /// <summary>
        /// This method is used to get a <see cref="NetworkConnection"/> from a <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identityId">The identity id that you want to get the connection for.</param>
        /// <param name="connection">The connection associated with the given identity id</param>
        /// <returns>True if able to find a connection for the given identity id, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        public virtual bool TryGetConnection(int identityId, out NetworkConnection connection) {
            if(!TryGetIdentity(identityId, out var identity)) {
                connection = null;
                return false;
            }
            return identity.TryGetConnection(out connection);
        }

        /// <summary>
        /// This method is used to get a <see cref="UserIdentity"/> from the given <see cref="NetworkConnection"/>.
        /// </summary>
        /// <param name="connection">The connection that you want to get the <see cref="UserIdentity"/> for.</param>
        /// <param name="identity">The <see cref="UserIdentity"/> associated with the given connection.</param>
        /// <returns>True if a <see cref="UserIdentity"/> was found for the given connection, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        public virtual bool TryGetIdentity(NetworkConnection connection, out UserIdentity identity) {
            if(!connection.TryGetUserId(out int identityId)) {
                identity = default;
                return false;
            }
            return TryGetIdentity(identityId, out identity);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}