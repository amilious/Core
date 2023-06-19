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
using System.ComponentModel;
using FishNet.Authenticating;
using Amilious.Core.Attributes;
using Amilious.Core.Identity.User;
using Amilious.Core.Authentication;
using Amilious.Core.FishNet.Groups;
using Amilious.Core.FishNet.Users;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.FishNet.Authentication {
    
    //TODO: add a mode that can be used to login without a user name or password.  This mode should generate a device identifier and a server identifier.
    //TODO: build a custom editor that will display different options based on the selected mode.
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkManager))]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet Amilious Authenticator")]
    [RequireComponent(typeof(FishNetUserDataManager),typeof(FishNetGroupDataManager))]
    public partial class FishNetAmiliousAuthenticator : Authenticator, IAmiliousAuthenticator {
        
        #region Inspector Values ///////////////////////////////////////////////////////////////////////////////////////

        [Header("Authenticator Options")]
        [SerializeField, AmiBool(true)]
        [Tooltip("If true the authenticator will log broadcasts.")]
        private bool logBroadcasts;
        [SerializeField, AmiBool(true)] 
        [Tooltip("If true a user id will be used for authentication, otherwise a user name will be used.")] 
        private bool useUserId;
        [SerializeField, AmiBool(true), AmiHideIf(nameof(useUserId))]
        [Tooltip("If true a new user will be created when joining with an unused user id.")]
        private bool autoRegister;
        [SerializeField, AmiBool(true)] [Tooltip("If true the failure reason will be reported.")]
        private bool reportFailReason;
        [SerializeField, AmiBool(true)]
        [Tooltip("If true the user will be required to use a password to join the game.")] 
        private bool usePassword = true;
        
        [Header("Password Settings")]
        [Tooltip("The total number of hash iterations for the password.")]
        [SerializeField, Min(1)] private int hashIterations = 1000;
        [SerializeField] private PasswordRequestProvider newPasswordProvider;
     
        [Header("Credentials")]
        [Tooltip("If true the last entered credentials will be saved!")]
        [SerializeField,AmiBool(true)] private bool rememberLast = true;
        [SerializeField, AmiShowIf(nameof(useUserId)), Tooltip("This optional field contains the user's id.")] 
        private int userId;
        [SerializeField, AmiHideIf(nameof(useUserId)), Tooltip("This optional field contains the user's user name.")] 
        private string userName;
        [SerializeField, PasswordPropertyText, AmiShowIf(nameof(usePassword))] 
        [Tooltip("The user's password!")]
        private string password;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private AbstractUserDataManager _userDataManager;
        private AbstractGroupDataManager _groupDataManager;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties

        /// <summary>
        /// This property is used to get the user data manager.
        /// </summary>
        public AbstractUserDataManager UserDataManager {
            get {
                if(_userDataManager != null) return _userDataManager;
                _userDataManager = GetComponent<AbstractUserDataManager>();
                return _userDataManager;
            }
        }

        /// <summary>
        /// This property is used to get the group data manager.
        /// </summary>
        public AbstractGroupDataManager GroupDataManager {
            get {
                if(_groupDataManager != null) return _groupDataManager;
                _groupDataManager = GetComponent<AbstractGroupDataManager>();
                return _groupDataManager;
            }
        }

        /// <summary>
        /// This property is used to get a new password when requested.
        /// </summary>
        public PasswordRequestProvider PasswordRequestProvider => newPasswordProvider;

        /// <summary>
        /// If this property is true, the authenticator will remember the last used credentials.
        /// </summary>
        public bool RememberLastCredentials => rememberLast;

        #endregion
        
        #region Authenticator Events ///////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;

        /// <inheritdoc />
        public event Action<string> OnConnectionRejected;
        
        /// <inheritdoc />
        public event Action<string> OnSuccessfulConnection;
        
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
       
    }
    
}