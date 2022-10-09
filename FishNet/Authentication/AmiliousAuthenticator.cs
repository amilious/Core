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
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using System.Collections.Generic;

namespace Amilious.Core.FishNet.Authentication {
    
    public abstract class AmiliousAuthenticator : Authenticator {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        [Header("Amilious Authenticator")]
        [SerializeField, AmiliousBool(true)]
        [Tooltip("If true the authenticator will log broadcasts.")]
        private bool logBroadcasts;
        [SerializeField, AmiliousBool(true)] 
        [Tooltip("If true a user id will be used for authentication, otherwise a user name will be used.")] 
        private bool useUserId = false;
        [SerializeField, AmiliousBool(true)]
        [Tooltip("If true the user will be required to use a password to join the game.")] 
        private bool usePassword = true;
        [SerializeField, AmiliousBool(true), HideIf(nameof(useUserId))]
        [Tooltip("If true a new user will be created when joining with an unused user id.")]
        private bool autoRegister;
        [SerializeField, ShowIf(nameof(useUserId)), Tooltip("This optional field contains the user's id.")] 
        private int userId;
        [SerializeField, HideIf(nameof(useUserId)), Tooltip("This optional field contains the user's user name.")] 
        private string userName;
        [SerializeField, PasswordPropertyText, ShowIf(nameof(usePassword))] 
        [Tooltip("The user's password!")]
        private string password;

        private readonly Dictionary<int, AuthenticationRequest> _authenticationRequests =
            new Dictionary<int, AuthenticationRequest>();

        private int _nextRequest = int.MinValue;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used to request a new password.
        /// </summary>
        protected delegate void GenerateNewPassword(string newPassword);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Authenticator Events ///////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Authenticator Methods ///////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void InitializeOnce(NetworkManager networkManager) {
            InitializeServer(networkManager);
            InitializeClient(networkManager);
            base.InitializeOnce(networkManager);
        }

        /// <summary>
        /// This method is used to set credentials before attempting to connect.
        /// </summary>
        /// <param name="userId">The user id of the user.</param>
        /// <param name="password">The password of the user.</param>
        // ReSharper disable ParameterHidesMember
        public virtual void SetCredentials(int userId, string password = null) {
            if(useUserId == false) {
                Debug.LogWarning("Credentials are being set with the user id, but the authenticator is set to use user names!");
                return;
            }
            this.userId = userId;
            this.password = password;
        }

        /// <summary>
        /// This method is used to set the credentials before attempting to connect.
        /// </summary>
        /// <param name="userName">The user name of the user.</param>
        /// <param name="password">The password of the user.</param>
        public virtual void SetCredentials(string userName, string password = null) {
            if(useUserId) {
                Debug.LogWarning("Credentials are being set with the user name, but the authenticator is set to use user ids!");
                return;
            }
            this.userName = userName;
            this.password = password;
        }
        // ReSharper enable ParameterHidesMember
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Executed On Server /////////////////////////////////////////////////////////////////////////////////////

        private void InitializeServer(NetworkManager networkManager) {
            //listen for broadcast from client on server
            networkManager.ServerManager.RegisterBroadcast<AuthenticationBroadcast>(
                Server_OnAuthenticationBroadcast, false);
            networkManager.ServerManager.RegisterBroadcast<PasswordBroadcast>(
                Server_OnPasswordBroadcast, false);
        }

        private void Server_OnAuthenticationBroadcast(NetworkConnection con, 
            AuthenticationBroadcast authenticationInfo) {
            if(logBroadcasts)Debug.Log("[Server] Authentication Broadcast Received!");
            //check if user is already logged in
            if(con.Authenticated) { con.Disconnect(true); return; }
            //authenticate the connection
            if(Server_AuthenticateGetUserId(authenticationInfo, useUserId, out var id,out var newUser, 
                   out var response)){
                if(newUser && !autoRegister) {
                    //invalid connection request
                    var result3 = new AuthenticationResultBroadcast() {
                        Passed = false,
                        NewUser = true,
                        ServerIdentifier = Server_GetServerIdentifier(),
                        UserId = id,
                        Response = response
                    };
                    //send the authentication response
                    if(logBroadcasts)Debug.Log("[Server] Authentication Result Broadcast Sent!");
                    NetworkManager.ServerManager.Broadcast(con,result3,false);
                    //trigger authentication result event
                    OnAuthenticationResult?.Invoke(con,false);
                    return;
                }
                //request password if using password
                if(usePassword) {
                    var passwordRequest = new PasswordRequestBroadcast() {
                        UserId = userId,
                        RequestId = Server_SetAuthenticationRequest(id,
                            newUser ? AuthenticationRequestType.NewPassword : AuthenticationRequestType.Password),
                        ServerIdentifier = Server_GetServerIdentifier(),
                        NewUser = newUser,
                        Salt = Server_GetUserPasswordSalt(id)
                    };
                    //send password request
                    if(logBroadcasts)Debug.Log("[Server] Password Request Broadcast Sent!");
                    NetworkManager.ServerManager.Broadcast(con,passwordRequest,false);
                    return;
                }
                //no password required
                var result = new AuthenticationResultBroadcast() {
                    Passed = true,
                    NewUser = newUser,
                    ServerIdentifier = Server_GetServerIdentifier(),
                    UserId = id,
                    UserName = authenticationInfo.UserName,
                    Response = response
                };
                //send the authentication response
                con.AssignUserId(id);
                if(logBroadcasts)Debug.Log("[Server] Authentication Result Broadcast Sent!");
                NetworkManager.ServerManager.Broadcast(con,result,false);
                //trigger authentication result event
                OnAuthenticationResult?.Invoke(con,true);
                return;
            }
            //invalid connection request
            var result2 = new AuthenticationResultBroadcast() {
                Passed = false,
                NewUser = false,
                ServerIdentifier = Server_GetServerIdentifier(),
                UserId = id,
                Response = response
            };
            //send the authentication response
            if(logBroadcasts)Debug.Log("[Server] Authentication Result Broadcast Sent!");
            NetworkManager.ServerManager.Broadcast(con,result2,false);
            //trigger authentication result event
            OnAuthenticationResult?.Invoke(con,false);
        }

        /// <summary>
        /// This broadcast is received after requesting the password from the client.
        /// </summary>
        /// <param name="con">The clients connection.</param>
        /// <param name="passBroadcast">The broadcast.</param>
        private void Server_OnPasswordBroadcast(NetworkConnection con, PasswordBroadcast passBroadcast) {
            if(logBroadcasts)Debug.Log("[Server] Password Broadcast Received!");
            if(!_authenticationRequests.TryGetValueFix(passBroadcast.UserId, out var request) ||
               request.RequestId != passBroadcast.RequestId) {
                //invalid request
                var result2 = new AuthenticationResultBroadcast() {
                    Passed = false,
                    NewUser = false,
                    ServerIdentifier = Server_GetServerIdentifier(),
                    UserId = passBroadcast.UserId,
                    Response = "Invalid request!"
                };
                //send the authentication response
                if(logBroadcasts)Debug.Log("[Server] Authentication Result Broadcast Sent!");
                NetworkManager.ServerManager.Broadcast(con,result2,false);
                //trigger authentication result event
                OnAuthenticationResult?.Invoke(con,false);
                return;
            }
            var newUser = request.RequestType == AuthenticationRequestType.NewPassword;
            string response;
            bool valid;
            if(newUser) {
                valid = Server_SetNewPassword(passBroadcast.UserId, 
                    passBroadcast.HashedPassword, out response);
            }else {
                valid = Server_AuthenticateCheckPassword(passBroadcast.UserId,
                    passBroadcast.HashedPassword, out response);
            }
            //create result
            var result = new AuthenticationResultBroadcast() {
                Passed = valid,
                ServerIdentifier = Server_GetServerIdentifier(),
                UserId = passBroadcast.UserId,
                NewUser = newUser,
                Response = response
            };
            //send the authentication response
            con.AssignUserId(passBroadcast.UserId);
            if(logBroadcasts)Debug.Log("[Server] Authentication Result Broadcast Sent!");
            NetworkManager.ServerManager.Broadcast(con,result,false);
            //trigger authentication result event
            OnAuthenticationResult?.Invoke(con,valid);
        }

        /// <summary>
        /// This method is used to create an authentication request id.
        /// </summary>
        /// <param name="userId">The id of the user the request is for.</param>
        /// <param name="type">The type of request.</param>
        /// <returns>The request id.</returns>
        private int Server_SetAuthenticationRequest(int userId, AuthenticationRequestType type) {
            var request = new AuthenticationRequest() {
                RequestId = _nextRequest++,
                RequestType = type
            };
            _authenticationRequests[userId] = request;
            return request.RequestId;
        }
        
        /// <summary>
        /// This method is used to get the salt used for the given user's password. 
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>The user's password salt.</returns>
        protected abstract string Server_GetUserPasswordSalt(int userId);

        /// <summary>
        /// This method is used to check the password given from the client.
        /// </summary>
        /// <param name="infoUserId">The user's id.</param>
        /// <param name="hashedPassword">The given hashed password.</param>
        /// <param name="response">A response that will be given upon failure.</param>
        /// <returns>True if the password is correct, otherwise false.</returns>
        /// <remarks>This method should only be called from the server.</remarks>
        protected abstract bool Server_AuthenticateCheckPassword(int infoUserId, string hashedPassword, 
            out string response);

        /// <summary>
        /// This method is used to try set the new password for the given user.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="hashedPassword">The user's new hashed password.</param>
        /// <param name="response">A response to the new password.</param>
        /// <returns>True if the new password is accepted, otherwise false.</returns>
        protected abstract bool Server_SetNewPassword(int userId, string hashedPassword, out string response);

        /// <summary>
        /// This method is used to get the user's id associated with the authentication info.
        /// </summary>
        /// <param name="authenticationInfo">The authentication info sent from the client.</param>
        /// <param name="usingUserId">If true the authenticator is using user ids to authenticate, otherwise
        /// it is using user names.</param>
        /// <param name="userId">The client's user id.</param>
        /// <param name="newUser">True if the user was just created, otherwise false.</param>
        /// <param name="response">A message that will be sent if the authentication failed.</param>
        /// <returns>True if the authentication was valid, otherwise false.</returns>
        /// <remarks>This method should only be called from the server.</remarks>
        protected abstract bool Server_AuthenticateGetUserId(AuthenticationBroadcast authenticationInfo, 
            bool usingUserId, out int userId, out bool newUser, out string response);
        
        /// <summary>
        /// This method is used to get the server's identifier.
        /// </summary>
        /// <returns>The server's identifier.</returns>
        /// <remarks>This method should only be called from the server.</remarks>
        protected abstract string Server_GetServerIdentifier();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

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
            if(passRequestBroadcast.NewUser) {
                Client_GenerateNewPassword(passRequestBroadcast, (pass)=> {
                    var passwordBroadcast = new PasswordBroadcast() {
                        UserId = passRequestBroadcast.UserId,
                        RequestId = passRequestBroadcast.RequestId,
                        HashedPassword = pass
                    };
                    if(logBroadcasts)Debug.Log("[Client] Password Broadcast Sent!");
                    NetworkManager.ClientManager.Broadcast(passwordBroadcast);
                });
                return;
            }
            var passwordBroadcast = new PasswordBroadcast() {
                UserId = passRequestBroadcast.UserId,
                RequestId = passRequestBroadcast.RequestId,
                HashedPassword = Client_GetHashedPassword(password, passRequestBroadcast.Salt)
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
                userId = authenticationResult.UserId;
            }
            string result = authenticationResult.Passed ? $"Authentication complete for {authenticationResult.UserName}." : "Authentication failed.";
            if (NetworkManager.CanLog(LoggingType.Common)) Debug.Log(result);
            Client_OnAuthenticationResult(authenticationResult);
        }

        /// <summary>
        /// This method is called when the server needs a password for a newly created user.
        /// </summary>
        /// <param name="passRequestBroadcast">The password request.</param>
        /// <param name="generateNewPassword">A method to return the newly generated password.</param>
        /// <remarks>This method should only be called from a client.</remarks>
        protected abstract void Client_GenerateNewPassword(PasswordRequestBroadcast passRequestBroadcast,
            GenerateNewPassword generateNewPassword);
        
        /// <summary>
        /// This method is used to hash the clients password before sending it to the server.
        /// </summary>
        /// <param name="password">The user's raw password.</param>
        /// <param name="salt">The user's salt value.</param>
        /// <returns>The hashed password.</returns>
        /// <remarks>This method should only be called from a client.</remarks>
        protected abstract string Client_GetHashedPassword(string password, string salt);

        /// <summary>
        /// This method is called after an authentication result has been received and processed.
        /// </summary>
        /// <param name="authenticationResult">The authentication result.</param>
        /// <remarks>This method should only be called from a client.</remarks>
        protected abstract void Client_OnAuthenticationResult(AuthenticationResultBroadcast authenticationResult);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}