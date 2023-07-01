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

using FishNet;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing;
using Amilious.Core.Chat;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.FishNet.Chat {
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(short.MinValue+100)]
    public partial class FishNetChatManager : NetworkBehaviour, IChatManager {

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This can be used to get the instance of the first <see cref="NetworkManager"/>, otherwise you can get the
        /// instance from the <see cref="NetworkManager"/>.
        /// </summary>
        public static FishNetChatManager Instance => InstanceFinder.GetInstance<FishNetChatManager>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public event IChatManager.ReceiveGroupMessageDelegate OnReceiveGroupMessage;
        /// <inheritdoc />
        public event IChatManager.ReceiveServerMessageDelegate OnReceiveServerMessage;
        /// <inheritdoc />
        public event IChatManager.ReceiveGlobalMessageDelegate OnReceiveGlobalMessage;
        /// <inheritdoc />
        public event IChatManager.ReceivePrivateMessageDelegate OnReceivePrivateMessage;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public void SendGlobalMessage(string message) =>
            Server_ReceiveGlobalMessage(message);

        /// <inheritdoc />
        public void SendGroupMessage(GroupIdentity group, string message) =>
            Server_ReceiveGroupMessage(group.Id,message);

        /// <inheritdoc />
        public void SendGroupMessage(uint groupId, string message) => 
            Server_ReceiveGroupMessage(groupId, message);

        /// <inheritdoc />
        public void SendPrivateMessage(UserIdentity recipient, string message) =>
            Server_ReceivePrivateMessage(recipient.Id, message);

        /// <inheritdoc />
        public void SendPrivateMessage(uint recipientId, string message) =>
            Server_ReceivePrivateMessage(recipientId, message);

        /// <inheritdoc />
        public void SendMessageToClient(UserIdentity recipient, string message) {
            if(!IsServer) {
                Debug.LogWarning("Only the server can send messages to clients!");
                return;
            }
            if(!UserIdentityManager.TryGetConnection(recipient.Id, out var connection)) return;
            Client_ReceiveServerMessage(connection,message);
        }
        
        /// <inheritdoc />
        public void SendMessageToClient(uint recipientId, string message) {
            if(!IsServer) {
                Debug.LogWarning("Only the server can send messages to clients!");
                return;
            }
            if(!UserIdentityManager.TryGetConnection(recipientId, out var connection)) return;
            Client_ReceiveServerMessage(connection,message);
        }

        /// <inheritdoc />
        public void SendMessageToClients(string message) {
            if(!IsServer) {
                Debug.LogWarning("Only the server can send messages to clients!");
                return;
            }
            Clients_ReceiveServerMessages(message);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            if(NetworkManager.TryRegisterInstance(this)) return;
            AmiliousCore.RemoveDuplicateMessage(this);
            Destroy(this);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}