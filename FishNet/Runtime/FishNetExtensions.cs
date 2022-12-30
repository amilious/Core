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
using FishNet.Managing;
using FishNet.Connection;
using FishNet.Managing.Server;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using Amilious.Core.FishNet.Authentication;

namespace Amilious.Core.FishNet {
    
    /// <summary>
    /// This class is used to add extension methods to fish net.
    /// </summary>
    public static class FishNetExtensions {
        
        /// <summary>
        /// This dictionary is used to look up a user id based on the connection.
        /// </summary>
        private static readonly Dictionary<int, int> ConnectionToUserId = new Dictionary<int, int>();

        private static readonly Dictionary<NetworkManager, int> LocalIdStorage = new Dictionary<NetworkManager, int>();

        /// <summary>
        /// This dictionary is used to look up the connection based on the user's id.
        /// </summary>
        private static readonly Dictionary<int, NetworkConnection> UserIdToConnection =
            new Dictionary<int, NetworkConnection>();

        /// <summary>
        /// This method is used by the Amilious Authenticator to assign a user id to a connection.
        /// </summary>
        /// <param name="con">The connection that you want to assign to the user id to.</param>
        /// <param name="userId">The user id for the connection.</param>
        /// <remarks>This method should only be used by the
        /// <see cref="AmiliousAuthenticator"/>.</remarks>
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
        /// <see cref="AmiliousAuthenticator"/>.</remarks>
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
        /// <see cref="AmiliousAuthenticator"/>.</remarks>
        public static bool TryGetConnection(this UserIdentity identity, NetworkManager manager, out NetworkConnection connection) {
            var result = UserIdToConnection.TryGetValueFix(identity.Id, out connection);
            if(result&&connection.NetworkManager == null&&manager.IsLocalUser(identity.Id)) {
                connection = manager.ClientManager.Connection;
                connection.AssignUserId(identity.Id);
                return true;
            }
            return result;
        }

        /// <summary>
        /// This method is used to get the amilious authenticator if it exists.
        /// </summary>
        /// <param name="serverManager">The server manager.</param>
        /// <returns>The authenticator for the server if it is an amilious authenticator, otherwise null. </returns>
        public static AmiliousAuthenticator GetAmiliousAuthenticator(this ServerManager serverManager) {
            var authenticator = serverManager.GetAuthenticator() as AmiliousAuthenticator;
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

    }
    
}