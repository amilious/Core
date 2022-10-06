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
            networkManager.ServerManager.RegisterBroadcast<AuthenticationBroadcast>(
                Server_OnAuthenticationBroadcast, false);
        }

        private void Server_OnAuthenticationBroadcast(NetworkConnection con, 
            AuthenticationBroadcast authenticationInfo) {
            if(con.Authenticated) { con.Disconnect(true); return; }
            //authenticate the connection
            if(Server_AuthenticateGetUserId(authenticationInfo, out var userId,out bool newUser)){
                //check password
                var valid = Server_AuthenticateCheckPassword(authenticationInfo.UserId, authenticationInfo.HashedPassword);
                var result = new AuthenticationResultBroadcast() {
                    Passed = valid,
                    ServerIdentifier = Server_GetServerIdentifier(),
                    UserId = userId,
                    NewUser = newUser
                };
                //send the authentication response
                NetworkManager.ServerManager.Broadcast(con,result,false);
                //trigger authentication result event
                OnAuthenticationResult?.Invoke(con,valid);
                return;
            }
            
            var result2 = new AuthenticationResultBroadcast() {
                Passed = false,
                NewUser = false,
                ServerIdentifier = Server_GetServerIdentifier(),
                UserId = userId
            };
            //send the authentication response
            NetworkManager.ServerManager.Broadcast(con,result2,false);
            //trigger authentication result event
            OnAuthenticationResult?.Invoke(con,false);
        }

        protected abstract bool Server_AuthenticateCheckPassword(int infoUserId, string hashedPassword);

        protected abstract bool Server_AuthenticateGetUserId(AuthenticationBroadcast authenticationInfo, 
            out int userId,out bool newUser);
        protected abstract string Server_GetServerIdentifier();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Executed On Client /////////////////////////////////////////////////////////////////////////////////////

        private void InitializeClient(NetworkManager networkManager) {
            //listen for connection as client
            networkManager.ClientManager.OnClientConnectionState += Client_OnConnectionState;
            //listen for broadcast from server on client
            networkManager.ClientManager.RegisterBroadcast<AuthenticationResultBroadcast>(
                Client_OnAuthenticationResultBroadcast);
        }

        private void Client_OnConnectionState(ClientConnectionStateArgs args) {
            if(args.ConnectionState != LocalConnectionState.Started) return;
            var authenticationInfo = new AuthenticationBroadcast() {
                UserId = userId,
                UserName = userName,
                HashedPassword = Client_GetHashedPassword(password)
            };
            NetworkManager.ClientManager.Broadcast(authenticationInfo);
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

        protected abstract string Client_GetHashedPassword(string password);

        protected abstract void Client_OnAuthenticationResult(AuthenticationResultBroadcast authenticationResult);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////


    }
    
}