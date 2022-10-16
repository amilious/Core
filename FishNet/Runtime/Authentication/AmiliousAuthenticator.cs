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
using Amilious.Core.Users;
using Amilious.Core.Saving;
using FishNet.Transporting;
using System.ComponentModel;
using FishNet.Authenticating;
using FishNet.Managing.Logging;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.FishNet.Users;

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
        [SerializeField, AmiliousBool(true)] [Tooltip("If true the failure reason will be reported.")]
        private bool reportFailReason;
        [SerializeField, ShowIf(nameof(useUserId)), Tooltip("This optional field contains the user's id.")] 
        private int userId;
        [SerializeField, HideIf(nameof(useUserId)), Tooltip("This optional field contains the user's user name.")] 
        private string userName;
        [SerializeField, PasswordPropertyText, ShowIf(nameof(usePassword))] 
        [Tooltip("The user's password!")]
        private string password;

        private readonly Dictionary<NetworkConnection, AuthenticationRequest> _authenticationRequests =
            new Dictionary<NetworkConnection, AuthenticationRequest>();

        private int _nextRequest = int.MinValue;
        private FishNetIdentityManager _identityManager;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used to request a new password.
        /// </summary>
        protected delegate void GenerateNewPassword(string newPassword);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties

        /// <summary>
        /// This property is used to get the authenticator's data manager.
        /// </summary>
        public abstract AbstractIdentityDataManager DataManager { get; }

        #endregion
        
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

        private bool IsUserCurrentlyActive(int id, string userName) {
            foreach(var tmpCon in NetworkManager.ClientManager.Clients.Values) {
                //get the user id
                if(!tmpCon.TryGetUserId(out var tmpId)) continue;
                //check the user id
                switch(useUserId) {
                    case true when tmpId == id: return true;
                    case true: continue;
                }
                //get user name
                if(!DataManager.Server_TryReadUserData(tmpId,UserIdentity.USER_NAME_KEY,
                       out string tmpUserName)) continue;
                //check user name
                if(tmpUserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            return false;
        }

        private void Server_SendAuthorizationResponse(NetworkConnection con, bool passed, int id, string userName, 
            bool newUser, string reason) {
            var result = new AuthenticationResultBroadcast() {
                Passed = passed, UserName = userName,
                ServerIdentifier = Server_GetServerIdentifier(),
                UserId = id, NewUser = newUser, Response = reportFailReason? reason : "Unable to complete request!"
            };
            //set id
            if(passed) {
                con.AssignUserId(id);
            }
            //send the authentication response
            if(logBroadcasts)Debug.Log("[Server] Authentication Result Broadcast Sent!");
            NetworkManager.ServerManager.Broadcast(con,result,false);
            //trigger authentication result event
            OnAuthenticationResult?.Invoke(con,passed);
        }

        private void Server_OnAuthenticationBroadcast(NetworkConnection con, 
            AuthenticationBroadcast authenticationInfo) {
            if(logBroadcasts)Debug.Log("[Server] Authentication Broadcast Received!");
            //check if user is already logged in
            if(con.Authenticated) { con.Disconnect(true); return; }
            //authenticate the connection
            if(!Server_AuthenticateGetUserId(authenticationInfo, useUserId, autoRegister, out var id, 
                   // ReSharper disable once LocalVariableHidesMember
                   out var userName, out var newUser, out var response)) {
                Server_SendAuthorizationResponse(con,false,id,userName,newUser,response);
                return;
            }
            //check if user is currently active
            if(IsUserCurrentlyActive(id, userName)) {
                response = "That user is already logged into the server!";
                Server_SendAuthorizationResponse(con,false,id,userName,newUser,response);
                return;
            }
            //if using password send a password request
            if(usePassword) {
                //request password if one is not set
                var newPassword = newUser || !DataManager.Server_HasUserSetPassword(id);
                var passwordRequest = new PasswordRequestBroadcast() {
                    RequestId = Server_SetAuthenticationRequest(con,id, userName, newUser,
                        newPassword ? AuthenticationRequestType.NewPassword : AuthenticationRequestType.Password),
                    ServerIdentifier = Server_GetServerIdentifier(),
                    NewPassword = newPassword,
                    Salt = Server_GetUserPasswordSalt(id)
                };
                //send password request
                if(logBroadcasts)Debug.Log("[Server] Password Request Broadcast Sent!");
                NetworkManager.ServerManager.Broadcast(con,passwordRequest,false);
                return;
            }
            //if no password required log in.
            Server_SendAuthorizationResponse(con,true,id,userName,true,null);
        }

        /// <summary>
        /// This broadcast is received after requesting the password from the client.
        /// </summary>
        /// <param name="con">The clients connection.</param>
        /// <param name="passBroadcast">The broadcast.</param>
        private void Server_OnPasswordBroadcast(NetworkConnection con, PasswordBroadcast passBroadcast) {
            if(logBroadcasts)Debug.Log("[Server] Password Broadcast Received!");
            //make sure the request type matches
            if(!_authenticationRequests.TryGetValueFix(con, out var request) ||
               request.RequestId != passBroadcast.RequestId) {
                //someone is possibly trying to do something malicious
                con.Disconnect(true); return;
            }
            var newPassword = request.RequestType == AuthenticationRequestType.NewPassword;
            string response;
            //validate password
            var valid = newPassword ? 
                Server_SetNewPassword(request.UserId, passBroadcast.HashedPassword, out response) : 
                Server_AuthenticateCheckPassword(request.UserId, passBroadcast.HashedPassword, out response);
            //login success
            Server_SendAuthorizationResponse(con,valid,request.UserId,request.UserName,request.NewUser,response);
        }

        /// <summary>
        /// This method is used to create an authentication request id.
        /// </summary>
        /// <param name="userId">The id of the user the request is for.</param>
        /// <param name="type">The type of request.</param>
        /// <returns>The request id.</returns>
        // ReSharper disable InvalidXmlDocComment
        private int Server_SetAuthenticationRequest(NetworkConnection con, int userId, string userName, bool newUser, 
            AuthenticationRequestType type) {
            var request = new AuthenticationRequest() {
                UserId = userId,
                UserName = userName,
                NewUser = newUser,
                RequestId = _nextRequest++,
                RequestType = type
            };
            _authenticationRequests[con] = request;
            return request.RequestId;
        }
        // ReSharper enable InvalidXmlDocComment
        
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
        /// <param name="autoRegister">If true new user's should be auto registered, otherwise this method should
        /// return false if the user does not exist.</param>
        /// <param name="userId">The client's user id.</param>
        /// <param name="userName">The client's user name.</param>
        /// <param name="newUser">True if the user was just created, otherwise false.</param>
        /// <param name="response">A message that will be sent if the authentication failed.</param>
        /// <returns>True if the authentication was valid, otherwise false.</returns>
        /// <remarks>This method should only be called from the server.</remarks>
        protected abstract bool Server_AuthenticateGetUserId(AuthenticationBroadcast authenticationInfo, 
            bool usingUserId, bool autoRegister, out int userId, out string userName, out bool newUser, 
            out string response);
        
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
            if(passRequestBroadcast.NewPassword) {
                Client_GenerateNewPassword(passRequestBroadcast, (pass)=> {
                    var passwordBroadcast = new PasswordBroadcast() {
                        RequestId = passRequestBroadcast.RequestId,
                        HashedPassword = pass
                    };
                    if(logBroadcasts)Debug.Log("[Client] Password Broadcast Sent!");
                    NetworkManager.ClientManager.Broadcast(passwordBroadcast);
                });
                return;
            }
            var passwordBroadcast = new PasswordBroadcast() {
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
                NetworkManager.AssignLocalUserId(authenticationResult.UserId);
                userId = authenticationResult.UserId;
            }
            var result = authenticationResult.Passed ? 
                $"Authentication complete for {authenticationResult.UserName}." : 
                $"Authentication failed! {authenticationResult.Response}";
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