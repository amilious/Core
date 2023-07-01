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
using Amilious.Core.Security;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using System.Security.Cryptography;

namespace Amilious.Core.FishNet.Authentication {
    
    public partial class FishNetAmiliousAuthenticator {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
                               
        private readonly Dictionary<NetworkConnection, AuthenticationRequest> _authenticationRequests =
            new Dictionary<NetworkConnection, AuthenticationRequest>();
        
        private int _nextRequest = int.MinValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Executed On Server /////////////////////////////////////////////////////////////////////////////////////

        private void InitializeServer(NetworkManager networkManager) {
            //listen for broadcast from client on server
            networkManager.ServerManager.RegisterBroadcast<AuthenticationBroadcast>(
                Server_OnAuthenticationBroadcast, false);
            networkManager.ServerManager.RegisterBroadcast<PasswordBroadcast>(
                Server_OnPasswordBroadcast, false);
        }

        private bool IsUserCurrentlyActive(uint id, string userName) {
            foreach(var tmpCon in NetworkManager.ClientManager.Clients.Values) {
                //get the user id
                if(!tmpCon.TryGetUserId(out var tmpId)) continue;
                //check the user id
                switch(useUserId) {
                    case true when tmpId == id: return true;
                    case true: continue;
                }
                //get user name
                if(!UserDataManager.Server_TryReadUserData(tmpId,UserIdentity.USER_NAME_KEY,
                       out string tmpUserName)) continue;
                //check user name
                if(tmpUserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            return false;
        }

        private void Server_SendAuthorizationResponse(NetworkConnection con, bool passed, uint id, string userName, 
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
                var newPassword = newUser || !UserDataManager.Server_HasUserSetPassword(id);
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
        private int Server_SetAuthenticationRequest(NetworkConnection con, uint userId, string userName, bool newUser, 
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
        protected virtual string Server_GetUserPasswordSalt(uint userId) {
            //return the old salt if it exists
            if(UserDataManager.Server_TryReadUserData(userId, UserIdentity.PASSWORD_SALT_KEY, out string salt)) 
                return salt;
            //generate new salt
            using var rng = new RNGCryptoServiceProvider();
            byte[] saltData;
            rng.GetBytes(saltData = new byte[16]);
            salt = Convert.ToBase64String(saltData);
            //store the newly generated salt
            UserDataManager.Server_StoreUserData(userId,UserIdentity.PASSWORD_SALT_KEY,salt);
            return salt;
        }

        /// <summary>
        /// This method is used to check the password given from the client.
        /// </summary>
        /// <param name="infoUserId">The user's id.</param>
        /// <param name="hashedPassword">The given hashed password.</param>
        /// <param name="response">A response that will be given upon failure.</param>
        /// <returns>True if the password is correct, otherwise false.</returns>
        /// <remarks>This method should only be called from the server.</remarks>
        protected virtual bool Server_AuthenticateCheckPassword(uint infoUserId, string hashedPassword,
            out string response) {
            if (!UserDataManager.Server_TryReadUserData(infoUserId, UserIdentity.PASSWORD_KEY,
                    out string storedPassword)) {
                response = "Invalid Request! No stored password!";
                return false;
            }

            //check the password
            if (PasswordTools.ServerValidateClientSecurePasswordHash(storedPassword, hashedPassword,
                    hashIterations)) {
                response = "Successfully authenticated!";
                return true;
            }
            //the password is invalid
            response = "Invalid Request! Incorrect password!";
            return false;
        }

        /// <summary>
        /// This method is used to try set the new password for the given user.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="hashedPassword">The user's new hashed password.</param>
        /// <param name="response">A response to the new password.</param>
        /// <returns>True if the new password is accepted, otherwise false.</returns>
        protected virtual bool Server_SetNewPassword(uint userId, string hashedPassword, out string response) {
            UserDataManager.Server_StoreUserData(userId,UserIdentity.PASSWORD_KEY,hashedPassword);
            response = "The password has been updated!";
            return true;
        }

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
        protected virtual bool Server_AuthenticateGetUserId(AuthenticationBroadcast authenticationInfo,
            bool usingUserId, bool autoRegister, out uint userId, out string userName, out bool newUser,
            out string response) {
            response = string.Empty;
            if(usingUserId) { //authenticating with user id
                newUser = false;
                userId = authenticationInfo.UserId;
                if(UserDataManager.Server_TryGetUserIdentity(authenticationInfo.UserId, out var identity)) {
                    userName = identity.Name;
                    return true;
                }
                userName = null;
                response = "Invalid Request! The provided user id is invalid!";
                return false;
            }
            //authenticating with user name
            if(UserDataManager.Server_TryGetUserIdentity(authenticationInfo.UserName, out var identity2)) {
                userId = identity2.Id;
                userName = identity2.Name;
                newUser = false;
                return true;
            }
            //the user has not been registered check if auto register
            if(!autoRegister) {
                userId = default;
                userName = default;
                newUser = true;
                response = "Invalid Request! The user does not exist!";
                return false;
            }
            //the user is new and auto register is enabled
            identity2 = UserDataManager.Server_AddUser(authenticationInfo.UserName);
            //the user has not been registered so make a new user
            userId = identity2.Id;
            userName = identity2.Name;
            newUser = true;
            response = "Auto-registering new user!";
            return true;
        }

        /// <summary>
        /// This method is used to get the server's identifier.
        /// </summary>
        /// <returns>The server's identifier.</returns>
        /// <remarks>This method should only be called from the server.</remarks>
        protected virtual string Server_GetServerIdentifier() => UserDataManager.Server_GetServerIdentifier();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        
    }
}