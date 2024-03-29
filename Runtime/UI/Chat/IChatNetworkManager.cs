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

using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.UI.Chat {
    
    /// <summary>
    /// This interface is used for sending and receiving chat messages.
    /// </summary>
    public interface IChatNetworkManager {

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for the <see cref="IChatNetworkManager.OnReceiveServerMessage"/> event.
        /// </summary>
        /// <param name="message">The message sent by the server.</param>
        public delegate void ReceiveServerMessageDelegate(string message);
        
        /// <summary>
        /// This delegate is used for the <see cref="IChatNetworkManager.OnReceiveGlobalMessage"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message that was sent.</param>
        public delegate void ReceiveGlobalMessageDelegate(UserIdentity sender, string message);
        
        /// <summary>
        /// This delegate is used for the <see cref="IChatNetworkManager.OnReceivePrivateMessage"/> event.
        /// </summary>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message that was sent.</param>
        public delegate void ReceivePrivateMessageDelegate(UserIdentity sender, string message);

        /// <summary>
        /// This delegate is used for the <see cref="IChatNetworkManager.OnPrivateMessageSent"/> event.
        /// </summary>
        /// <param name="receiver">The user that the message was sent to.</param>
        /// <param name="message">The message that was sent.</param>
        public delegate void SendPrivateMessageDelegate(UserIdentity receiver, string message);
        
        /// <summary>
        /// This delegate is used for the <see cref="IChatNetworkManager.OnReceiveGroupMessage"/> event.
        /// </summary>
        /// <param name="sender">The message sender.</param>
        /// <param name="group">The group that the message was sent to.</param>
        /// <param name="message">The message that was sent.</param>
        public delegate void ReceiveGroupMessageDelegate(UserIdentity sender, GroupIdentity group, string message);

        /// <summary>
        /// This delegate is used for the <see cref="IChatNetworkManager.OnGroupMessageSent"/> event.
        /// </summary>
        /// <param name="group">The group that the message was sent to.</param>
        /// <param name="message">The message that was sent to the group.</param>
        public delegate void SendGroupMessageDelegate(GroupIdentity group, string message);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when a server message is received from the server.
        /// </summary>
        /// <remarks>This event is only triggered on the client.</remarks>
        public event ReceiveServerMessageDelegate OnReceiveServerMessage;
        
        /// <summary>
        /// This event is triggered when a global message is received from the server.
        /// </summary>
        /// <remarks>This event is only triggered on the client.</remarks>
        public event ReceiveGlobalMessageDelegate OnReceiveGlobalMessage;
        
        /// <summary>
        /// This event is triggered when a private message is received from the server.
        /// </summary>
        /// <remarks>This event is only triggered on the client.</remarks>
        public event ReceivePrivateMessageDelegate OnReceivePrivateMessage;
        
        /// <summary>
        /// This event is triggered when the local user sends a private message to the server.
        /// </summary>
        /// <remarks>This event is only triggered on the client.</remarks>
        public event SendPrivateMessageDelegate OnPrivateMessageSent;
        
        /// <summary>
        /// This event is triggered when a group message is received from the server.
        /// </summary>
        /// <remarks>This event is only triggered on the client.</remarks>
        public event ReceiveGroupMessageDelegate OnReceiveGroupMessage;

        /// <summary>
        /// This event is triggered when the local user sends a group message to the server.
        /// </summary>
        /// <remarks>This event is only triggered on the client.</remarks>
        public event SendGroupMessageDelegate OnGroupMessageSent;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to send a global message.
        /// </summary>
        /// <param name="message">The message that you want to send to everyone.</param>
        /// <remarks>This method should only be called by the client and not a server.</remarks>
        public void SendGlobalMessage(string message);

        /// <summary>
        /// This method is used to send a group message.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to receive the message.</param>
        /// <param name="message">The message that you want to send to the group.</param>
        /// <remarks>This method should only be called by the client and not a server.</remarks>
        public void SendGroupMessage(uint groupId, string message);

        /// <summary>
        /// This method is used to send a private message.
        /// </summary>
        /// <param name="recipientId">The id of the user that you want to receive the message.</param>
        /// <param name="message">The message that you want to send to the recipient.</param>
        /// <remarks>This method should only be called by the client and not a server.</remarks>
        public void SendPrivateMessage(uint recipientId, string message);

        /// <summary>
        /// This method is used to send a server message to the given recipient.
        /// </summary>
        /// <param name="recipientId">The id of the user that you want to receive the message.</param>
        /// <param name="message">The message that you want the recipient to receive.</param>
        /// <remarks>This method should only be called by the server not a client.</remarks>
        public void SendMessageToClient(uint recipientId, string message);

        /// <summary>
        /// This method is used to send a message to all of the clients.
        /// </summary>
        /// <param name="message">The message that you want all of the clients to receive.</param>
        /// <remarks>This method should only be called by the server not a client.</remarks>
        public void SendMessageToClients(string message);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}