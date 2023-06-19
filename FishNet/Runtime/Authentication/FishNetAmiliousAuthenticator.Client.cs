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
using FishNet.Transporting;
using Amilious.Core.Security;
using FishNet.Managing.Logging;

namespace Amilious.Core.FishNet.Authentication {
    
    public partial class FishNetAmiliousAuthenticator {
        
        #region Executed On Client /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to initialize the client.
        /// </summary>
        /// <param name="networkManager">The network manager.</param>
        /// <remarks>This method should only be called from a client.</remarks>
        private void InitializeClient(NetworkManager networkManager) {
            //listen for connection as client
            networkManager.ClientManager.OnClientConnectionState += Client_OnConnectionState;
            //listen for broadcast from server on client
            networkManager.ClientManager.RegisterBroadcast<PasswordRequestBroadcast>(
                Client_OnPasswordRequestBroadcast);
            networkManager.ClientManager.RegisterBroadcast<AuthenticationResultBroadcast>(
                Client_OnAuthenticationResultBroadcast);
        }

        /// <summary>
        /// This method is called when the client's connection state is updated.
        /// </summary>
        /// <param name="args">The state arguments.</param>
        /// <remarks>This method should only be called from a client.</remarks>
        private void Client_OnConnectionState(ClientConnectionStateArgs args) {
            if(args.ConnectionState != LocalConnectionState.Started) return;
            var authenticationInfo = new AuthenticationBroadcast() {
                UserId = userId,
                UserName = userName,
            };
            if(logBroadcasts)Debug.Log("[Client] Authentication Broadcast Sent!");
            NetworkManager.ClientManager.Broadcast(authenticationInfo);
        }

        /// <summary>
        /// This method is called when the server is requesting a password.
        /// </summary>
        /// <param name="passRequestBroadcast">The broadcast requesting the password.</param>
        /// <remarks>This method should only be called from a client.</remarks>
        private void Client_OnPasswordRequestBroadcast(PasswordRequestBroadcast passRequestBroadcast) {
            if(logBroadcasts)Debug.Log("[Client] Password Request Broadcast Received!");
            if(passRequestBroadcast.NewPassword) {
                PasswordRequestProvider.RequestNewPassword( pass=> {
                    password = pass;
                    pass = PasswordTools.HashPasswordSHA512(pass, passRequestBroadcast.Salt);
                    var passwordBroadcast = new PasswordBroadcast() {
                        RequestId = passRequestBroadcast.RequestId,
                        HashedPassword = pass
                    };
                    if(logBroadcasts)Debug.Log("[Client] Password Broadcast Sent!");
                    NetworkManager.ClientManager.Broadcast(passwordBroadcast);
                });
                return;
            }
            var firstHash = PasswordTools.HashPasswordSHA512(password, passRequestBroadcast.Salt);
            var passwordBroadcast = new PasswordBroadcast() {
                RequestId = passRequestBroadcast.RequestId,
                HashedPassword = PasswordTools.ClientSecurePasswordHash(firstHash,hashIterations)
            };
            if(logBroadcasts)Debug.Log("[Client] Password Broadcast Sent!");
            NetworkManager.ClientManager.Broadcast(passwordBroadcast);
        }

        /// <summary>
        /// This method is called when the server response to a connection attempt.
        /// </summary>
        /// <param name="authenticationResult">The authentication result.</param>
        /// <remarks>This method should only be called from a client.</remarks>
        private void Client_OnAuthenticationResultBroadcast(AuthenticationResultBroadcast authenticationResult) {
            if(logBroadcasts)Debug.Log("[Client] Authentication Result Broadcast Received!");
            if(authenticationResult.Passed) {
                //assign the user id to the client side as well.
                NetworkManager.ClientManager.Connection.AssignUserId(authenticationResult.UserId);
                NetworkManager.AssignLocalUserId(authenticationResult.UserId);
                userId = authenticationResult.UserId;
            }
            var result = authenticationResult.Passed ? 
                $"Authentication complete for {authenticationResult.UserName}." : 
                $"Authentication failed! {authenticationResult.Response}";
            if (NetworkManager.CanLog(LoggingType.Common)) Debug.Log(result);
            if(authenticationResult.Passed) {
                if(rememberLast) {
                    UserDataManager.Client_StoreLastUserName(userName,false);
                    UserDataManager.Client_StoreLastPassword(password);
                }
                OnSuccessfulConnection?.Invoke(authenticationResult.Response);
            }
            else OnConnectionRejected?.Invoke(authenticationResult.Response);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        
    }
}