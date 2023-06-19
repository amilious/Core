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
using FishNet.Managing;
using FishNet.Connection;
using FishNet.Managing.Server;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.FishNet.Chat;
using Amilious.Core.Identity.User;
using Amilious.Core.FishNet.Users;
using Amilious.Core.FishNet.Groups;
using Amilious.Core.FishNet.Authentication;

namespace Amilious.Core.FishNet {
    
    /// <summary>
    /// This class is used to add extension methods to fish net.
    /// </summary>
    public static class FishNetExtensions {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string IDENTITY_MANAGERS_GAME_OBJECT_NAME = "Identity Managers (FishNet)";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary is used to look up a user id based on the connection.
        /// </summary>
        private static readonly Dictionary<int, int> ConnectionToUserId = new Dictionary<int, int>();

        /// <summary>
        /// This dictionary is used to look up the id of the local connection.
        /// </summary>
        private static readonly Dictionary<NetworkManager, int> LocalIdStorage = new Dictionary<NetworkManager, int>();

        /// <summary>
        /// This dictionary is used to look up the connection based on the user's id.
        /// </summary>
        private static readonly Dictionary<int, NetworkConnection> UserIdToConnection =
            new Dictionary<int, NetworkConnection>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used by the Amilious Authenticator to assign a user id to a connection.
        /// </summary>
        /// <param name="con">The connection that you want to assign to the user id to.</param>
        /// <param name="userId">The user id for the connection.</param>
        /// <remarks>This method should only be used by the
        /// <see cref="FishNetAmiliousAuthenticator"/>.</remarks>
        public static void AssignUserId(this NetworkConnection con, int userId) {
            ConnectionToUserId[con.ClientId] = userId;
            UserIdToConnection[userId] = con;
        }

        /// <summary>
        /// This method is used to try get the user id associated with the connection.
        /// </summary>
        /// <param name="con">The connection that you want to get the user id for.</param>
        /// <param name="userId">The user id associated with the connection.</param>
        /// <returns>True if a user id has been assigned for the connection, otherwise false.</returns>
        /// <remarks>This method should only be used on the server and only when using the
        /// <see cref="FishNetAmiliousAuthenticator"/>.</remarks>
        public static bool TryGetUserId(this NetworkConnection con, out int userId) {
            return ConnectionToUserId.TryGetValueFix(con.ClientId, out userId);
        }

        /// <summary>
        /// This method is used to try get the connection associated with the <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identity">The identity that you want to get the connection for.</param>
        /// <param name="manager">The network manager.</param>
        /// <param name="connection">The connection for the <see cref="UserIdentity"/>.</param>
        /// <returns>True if the <see cref="UserIdentity"/> has a connection assigned to it,
        /// otherwise false.</returns>
        /// <remarks>This method should only be used on the server and only when using the
        /// <see cref="FishNetAmiliousAuthenticator"/>.</remarks>
        public static bool TryGetConnection(this UserIdentity identity, NetworkManager manager, 
            out NetworkConnection connection) {
            var result = UserIdToConnection.TryGetValueFix(identity.Id, out connection);
            if(!result || connection.NetworkManager != null || !manager.IsLocalUser(identity.Id)) return result;
            connection = manager.ClientManager.Connection;
            connection.AssignUserId(identity.Id);
            return true;
        }

        /// <summary>
        /// This method is used to get the amilious authenticator if it exists.
        /// </summary>
        /// <param name="serverManager">The server manager.</param>
        /// <returns>The authenticator for the server if it is an amilious authenticator, otherwise null. </returns>
        public static FishNetAmiliousAuthenticator GetAmiliousAuthenticator(this ServerManager serverManager) {
            var authenticator = serverManager.GetAuthenticator() as FishNetAmiliousAuthenticator;
            if(authenticator==null) 
                Debug.LogWarning("Unable to get the Amilious Authenticator from the Server Manager!");
            return authenticator;
        }
        
        public static void AssignLocalUserId(this NetworkManager networkManager, int localId) {
            LocalIdStorage[networkManager] = localId;
        }

        public static bool TryGetLocalUserId(this NetworkManager networkManager, out int localId) {
            return LocalIdStorage.TryGetValueFix(networkManager, out localId);
        }

        public static bool IsLocalUser(this NetworkManager networkManager, int id) {
            return LocalIdStorage.TryGetValueFix(networkManager, out var localId) && id==localId;
        }
        
        #region Registered Instances ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the registered <see cref="FishNetChatManager"/> of the
        /// <see cref="NetworkBehaviour"/>s <see cref="NetworkManager"/>.
        /// </summary>
        /// <param name="networkBehaviour">The network behavior.</param>
        /// <param name="warning">If true a warning will be displayed if there is not a registered
        /// <see cref="FishNetChatManager"/></param>
        /// <returns>The registered <see cref="FishNetChatManager"/> or null if not registered.</returns>
        public static FishNetChatManager GetChatManager(this NetworkBehaviour networkBehaviour, bool warning = true) =>
            networkBehaviour.NetworkManager.GetChatManager(warning);

        /// <summary>
        /// This method is used to get the registered <see cref="FishNetChatManager"/> of the
        /// <see cref="NetworkManager"/>.
        /// </summary>
        /// <param name="networkManager">The network manager.</param>
        /// <param name="warning">If true a warning will be displayed if there is not a registered
        /// <see cref="FishNetChatManager"/></param>
        /// <returns>The registered <see cref="FishNetChatManager"/> or null if not registered.</returns>
        public static FishNetChatManager GetChatManager(this NetworkManager networkManager, bool warning = true) {
            var instance = networkManager.GetInstance<FishNetChatManager>();
            if(instance!=null) return instance;
            if(warning) NoRegisteredInstance<FishNetChatManager>(networkManager);
            return instance;
        }

        /// <summary>
        /// This method is used to get the registered <see cref="FishNetUserIdentityManager"/> of the
        /// <see cref="NetworkBehaviour"/>s <see cref="NetworkManager"/>.
        /// </summary>
        /// <param name="networkBehaviour">The network behavior.</param>
        /// <param name="warning">If true a warning will be displayed if there is not a registered
        /// <see cref="FishNetUserIdentityManager"/></param>
        /// <returns>The registered <see cref="FishNetUserIdentityManager"/> or null if not registered.</returns>
        public static FishNetUserIdentityManager GetUserManager(this NetworkBehaviour networkBehaviour,
            bool warning = true) => networkBehaviour.NetworkManager.GetUserManager();
        
        /// <summary>
        /// This method is used to get the registered <see cref="FishNetUserIdentityManager"/> of the
        /// <see cref="NetworkManager"/>.
        /// </summary>
        /// <param name="networkManager">The network manager.</param>
        /// <param name="warning">If true a warning will be displayed if there is not a registered
        /// <see cref="FishNetUserIdentityManager"/></param>
        /// <returns>The registered <see cref="FishNetUserIdentityManager"/> or null if not registered.</returns>
        public static FishNetUserIdentityManager GetUserManager(this NetworkManager networkManager, 
            bool warning = true) {
            var instance = networkManager.GetInstance<FishNetUserIdentityManager>();
            if(instance != null) return instance;
            if(warning) NoRegisteredInstance<FishNetUserIdentityManager>(networkManager);
            return instance;
        }

        /// <summary>
        /// This method is used to get the registered <see cref="FishNetGroupIdentityManager"/> of the
        /// <see cref="NetworkBehaviour"/>s <see cref="NetworkManager"/>.
        /// </summary>
        /// <param name="networkBehaviour">The network behavior.</param>
        /// <param name="warning">If true a warning will be displayed if there is not a registered
        /// <see cref="FishNetGroupIdentityManager"/></param>
        /// <returns>The registered <see cref="FishNetGroupIdentityManager"/> or null if not registered.</returns>
        public static FishNetGroupIdentityManager GetGroupManager(this NetworkBehaviour networkBehaviour,
            bool warning = true) => networkBehaviour.NetworkManager.GetGroupManager();
        
        /// <summary>
        /// This method is used to get the registered <see cref="FishNetGroupIdentityManager"/> of the
        /// <see cref="NetworkManager"/>.
        /// </summary>
        /// <param name="networkManager">The network manager.</param>
        /// <param name="warning">If true a warning will be displayed if there is not a registered
        /// <see cref="FishNetGroupIdentityManager"/></param>
        /// <returns>The registered <see cref="FishNetGroupIdentityManager"/> or null if not registered.</returns>
        public static FishNetGroupIdentityManager GetGroupManager(this NetworkManager networkManager, 
            bool warning = true) {
            var instance = networkManager.GetInstance<FishNetGroupIdentityManager>();
            if(instance != null) return instance;
            if(warning) NoRegisteredInstance<FishNetGroupIdentityManager>(networkManager);
            return instance;
        }

        public static FishNetUserDataManager GetUserDataManager(this NetworkBehaviour networkBehaviour,
            bool warning = true) => networkBehaviour.NetworkManager.GetUserDataManager(warning);

        public static FishNetUserDataManager GetUserDataManager(this NetworkManager networkManager, 
            bool warning = true) {
            var instance = networkManager.GetInstance<FishNetUserDataManager>();
            if(instance != null) return instance;
            if(warning) NoRegisteredInstance<FishNetGroupIdentityManager>(networkManager);
            return instance;
        }
        
        public static FishNetGroupDataManager GetGroupDataManager(this NetworkBehaviour networkBehaviour,
            bool warning = true) => networkBehaviour.NetworkManager.GetGroupDataManager(warning);

        public static FishNetGroupDataManager GetGroupDataManager(this NetworkManager networkManager, 
            bool warning = true) {
            var instance = networkManager.GetInstance<FishNetGroupDataManager>();
            if(instance != null) return instance;
            if(warning) NoRegisteredInstance<FishNetGroupIdentityManager>(networkManager);
            return instance;
        }

        /// <summary>
        /// This method is used to display a warning when an instance is not registered.
        /// </summary>
        /// <param name="networkManager">The network manager.</param>
        /// <typeparam name="T">The instance type.</typeparam>
        private static void NoRegisteredInstance<T>(Object networkManager){
            Debug.LogWarningFormat(networkManager,
                "There is no registered instance of <color={1}>{0}</color> in the <color={2}>NetworkManager</color>!",
                typeof(T).Namespace, "#0080ff", "#00FF00");
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}