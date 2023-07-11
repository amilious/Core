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
        public event IChatManager.SendPrivateMessageDelegate OnPrivateMessageSent;

        /// <inheritdoc />
        public event IChatManager.ReceiveGroupMessageDelegate OnReceiveGroupMessage;

        /// <inheritdoc />
        public event IChatManager.SendGroupMessageDelegate OnGroupMessageSent;

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
        public void SendGroupMessage(uint group, string message) {
            Server_ReceiveGroupMessage(group, message);
            OnGroupMessageSent?.Invoke(GroupManager[group],message);
        }

        /// <inheritdoc />
        public void SendPrivateMessage(uint recipient, string message) {
            Server_ReceivePrivateMessage(recipient, message);
            OnPrivateMessageSent?.Invoke(UserManager[recipient],message);
        }

        /// <inheritdoc />
        public void SendMessageToClient(uint recipient, string message) {
            if(!IsServer) {
                Debug.LogWarning("Only the server can send messages to clients!");
                return;
            }
            if(!UserManager.TryGetConnection(recipient, out var connection)) return;
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
        
        public override void OnStartNetwork() {
            base.OnStartNetwork();
            NetworkManager.RegisterInstance(this);
            Debug.Log("ChatManager registered!");
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}