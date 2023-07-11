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

using FishNet.Object;
using FishNet.Connection;

namespace Amilious.Core.FishNet.Chat {
    
    public partial class FishNetChatManager {

        #region Client Rpcs ////////////////////////////////////////////////////////////////////////////////////////////
        
        [ObserversRpc]
        private void Clients_ReceiveGlobalMessage(uint senderId, string message) {
            OnReceiveGlobalMessage?.Invoke(UserManager[senderId], message);
        }

        [TargetRpc]
        private void Client_ReceiveGroupMessage(NetworkConnection con, uint senderId, uint groupId, string message) =>
            OnReceiveGroupMessage?.Invoke(UserManager[senderId], GroupManager[groupId], message);

        [TargetRpc]
        private void Client_ReceivePrivateMessage(NetworkConnection con, uint senderId, string message) =>
            OnReceivePrivateMessage?.Invoke(UserManager[senderId], message);

        [TargetRpc]
        private void Client_ReceiveServerMessage(NetworkConnection con, string message) =>
            OnReceiveServerMessage?.Invoke(message);

        [ObserversRpc]
        private void Clients_ReceiveServerMessages(string message) => 
            OnReceiveServerMessage?.Invoke(message);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}