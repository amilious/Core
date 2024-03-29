﻿/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

using Cysharp.Text;
using FishNet.Object;
using Amilious.Console;
using FishNet.Connection;
using Amilious.Core.FishNet.Users;
using Amilious.Core.FishNet.Groups;

namespace Amilious.Core.FishNet.Chat {
    
    public partial class FishNetChatManager {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Use <see cref="GroupManager"/> and not this variable.
        /// </summary>
        private FishNetGroupIdentityManager _groupManager;
        
        /// <summary>
        /// User <see cref="UserManager"/> and not this variable.
        /// </summary>
        private FishNetUserIdentityManager _userManager;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the FishNet Group Identity Manager.
        /// </summary>
        private FishNetGroupIdentityManager GroupManager {
            get {
                if(_groupManager != null) return _groupManager;
                _groupManager = this.GetGroupManager();
                return _groupManager;
            }
        }

        /// <summary>
        /// This property contains the FishNet User Identity Manager.
        /// </summary>
        private FishNetUserIdentityManager UserManager {
            get {
                if(_userManager != null) return _userManager;
                _userManager = this.GetUserManager();
                return _userManager;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Server Rpcs ////////////////////////////////////////////////////////////////////////////////////////////
        
        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveGlobalMessage(string message, NetworkConnection con = null) {
            if(con == null ||!con.Authenticated) return;
            if(!con.TryGetUserId(out var id)) return;
            Clients_ReceiveGlobalMessage(id, message);
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceivePrivateMessage(uint recipientId, string message, NetworkConnection con = null) {
            UserManager.TryGetIdentity(con, out var sender);
            if(!UserManager.TryGetConnection(recipientId, out var recipientConnection)) {
                Client_ReceiveServerMessage(con,ZString.Style(StyleFormat.DebugError,"Unable to deliver the message."));
                return;
            };
            if(!UserManager.Server_CanSendMessage(sender.Id, recipientId)) {
                Client_ReceiveServerMessage(con, ZString.Style(StyleFormat.DebugError,"Unable to deliver the message."));
                return;
            }
            Client_ReceivePrivateMessage(recipientConnection,sender.Id,message);

        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveGroupMessage(uint groupId, string message, NetworkConnection con = null) {
            if(!UserManager.TryGetIdentity(con, out var sender)) return;
            if(!GroupManager.TryGetGroup(groupId, out var group)) return;
            if(!GroupManager.IsMember(group,sender)) return;
            //send the message
            foreach(var connection in GroupManager.GetGroupConnections(group)) {
                if(connection==con) continue;
                Client_ReceiveGroupMessage(connection,sender.Id,groupId,message);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}