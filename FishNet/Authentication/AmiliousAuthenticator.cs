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
using FishNet.Managing;
using FishNet.Connection;
using FishNet.Transporting;
using System.ComponentModel;
using FishNet.Authenticating;
using FishNet.Managing.Logging;
using Amilious.Core.Extensions;
using System.Collections.Generic;

namespace Amilious.Core.FishNet.Authentication {
    
    public abstract class AmiliousAuthenticator : Authenticator {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        [SerializeField] private int userId;
        [SerializeField] private string userName;
        [SerializeField, PasswordPropertyText] private string password;
        
        private static readonly Dictionary<int, string> UserSecrets = new Dictionary<int, string>(); 
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Authenticator Events ///////////////////////////////////////////////////////////////////////////////////
        
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Authenticator Methods ///////////////////////////////////////////////////////////////////////////
        
        public override void InitializeOnce(NetworkManager networkManager) {
            base.InitializeOnce(networkManager);
            if(networkManager.IsServer) InitializeServer(networkManager);
            if(networkManager.IsClient) InitializeClient(networkManager);
        }

        public virtual void SetCredentials(int userId, string password) {
            this.userId = userId;
            this.password = password;
        }

        public virtual void SetCredentials(string userName, string password) {
            this.userName = userName;
            this.password = password;
        }

        public virtual void SetCredentials(int userId, string userName, string password) {
            this.userId = userId;
            this.userName = userName;
            this.password = password;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Executed On Server /////////////////////////////////////////////////////////////////////////////////////

        private void InitializeServer(NetworkManager networkManager) {
            //listen for broadcast from client on server
            networkManager.ServerManager.RegisterBroadcast<AuthenticationStep1Broadcast>(
                Server_OnAuthenticationBroadcast, false);
            networkManager.ServerManager.RegisterBroadcast<AuthenticationStep3Broadcast>(
                Server_OnAuthenticationBroadcast2, false);
        }

        private void Server_OnAuthenticationBroadcast(NetworkConnection con, 
            AuthenticationStep1Broadcast authenticationInfo) {
            if(con.Authenticated) { con.Disconnect(true); return; }
            //authenticate the connection
            if(Server_AuthenticateGetUserId(authenticationInfo, out var userId)){
                //assign user id to connection
                con.AssignUserId(userId);
                var secret = Server_GenerateSecret(userId);
                UserSecrets[con.ClientId] = secret;
                //create step 2
                var step2 = new AuthenticationStep2Broadcast() {
                    ServerIdentifier = Server_GetServerIdentifier(),
                    UserId = userId,
                    UserSecret = secret
                };
                NetworkManager.ServerManager.Broadcast(con,step2,false);
                return;
            }
            
            var result = new AuthenticationResultBroadcast() {
                Passed = false,
                ServerIdentifier = Server_GetServerIdentifier(),
                UserId = userId
            };
            //send the authentication response
            NetworkManager.ServerManager.Broadcast(con,result,false);
            //trigger authentication result event
            OnAuthenticationResult?.Invoke(con,false);
        }

        private void Server_OnAuthenticationBroadcast2(NetworkConnection con, AuthenticationStep3Broadcast info) {
            var valid = UserSecrets.TryGetValueFix(con.ClientId, out var secret);
            if(valid && secret != info.UserSecret) valid = false;
            if(valid) valid = Server_AuthenticateCheckPassword(info.UserId, secret, info.HashedPassword);
            var result = new AuthenticationResultBroadcast() {
                Passed = valid,
                ServerIdentifier = Server_GetServerIdentifier(),
                UserId = info.UserId
            };
            //send the authentication response
            NetworkManager.ServerManager.Broadcast(con,result,false);
            //trigger authentication result event
            OnAuthenticationResult?.Invoke(con,valid);
        }

        protected abstract bool Server_AuthenticateCheckPassword(int infoUserId, string secret, string hashedPassword);

        protected abstract string Server_GenerateSecret(int userId);

        protected abstract bool Server_AuthenticateGetUserId(AuthenticationStep1Broadcast authenticationInfo, 
            out int userId);
        protected abstract string Server_GetServerIdentifier();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Executed On Client /////////////////////////////////////////////////////////////////////////////////////

        private void InitializeClient(NetworkManager networkManager) {
            //listen for connection as client
            networkManager.ClientManager.OnClientConnectionState += Client_OnConnectionState;
            //listen for broadcast from server on client
            networkManager.ClientManager.RegisterBroadcast<AuthenticationStep2Broadcast>(Client_OnAuthenticationBroadCast);
            networkManager.ClientManager.RegisterBroadcast<AuthenticationResultBroadcast>(
                Client_OnAuthenticationResultBroadcast);
        }

        private void Client_OnConnectionState(ClientConnectionStateArgs args) {
            if(args.ConnectionState != LocalConnectionState.Started) return;
            var authenticationInfo = new AuthenticationStep1Broadcast() {
                UserId = userId,
                UserName = userName
            };
            NetworkManager.ClientManager.Broadcast(authenticationInfo);
        }

        private void Client_OnAuthenticationBroadCast(AuthenticationStep2Broadcast authenticationInfo) {
            var pass = Client_GetHashedPassword(authenticationInfo);
            var step3 = new AuthenticationStep3Broadcast() {
                UserId = authenticationInfo.UserId,
                HashedPassword = pass,
                UserSecret = authenticationInfo.UserSecret
            };
            NetworkManager.ClientManager.Broadcast(step3);
        }

        private void Client_OnAuthenticationResultBroadcast(AuthenticationResultBroadcast authenticationResult) {
            if(authenticationResult.Passed) {
                //assign the user id to the client side as well.
                NetworkManager.ClientManager.Connection.AssignUserId(authenticationResult.UserId);
            }
            string result = authenticationResult.Passed ? "Authentication complete." : "Authentication failed.";
            if (NetworkManager.CanLog(LoggingType.Common)) Debug.Log(result);
            Client_OnAuthenticationResult(authenticationResult);
        }

        protected abstract string Client_GetHashedPassword(AuthenticationStep2Broadcast authenticationInfo);

        protected abstract void Client_OnAuthenticationResult(AuthenticationResultBroadcast authenticationResult);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////


    }
    
}