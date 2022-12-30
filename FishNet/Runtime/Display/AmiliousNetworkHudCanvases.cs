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

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Managing;
using FishNet.Transporting;
using Amilious.Core.Attributes;
using Amilious.Core.Identity.User;
using Amilious.Core.FishNet.Authentication;

namespace Amilious.Core.FishNet.Display {
    
    /// <summary>
    /// This class is a modified version of the FISHNET Network Hud.
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class AmiliousNetworkHudCanvases : MonoBehaviour {
    
        #region Private Enums //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Ways the HUD will automatically start a connection.
        /// </summary>
        private enum AutoStartType { Disabled, Host, Server, Client }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, AmiliousBool(true)] private bool rememberLastUserName = true;
        [SerializeField, AmiliousBool(true)] private bool rememberLastPassword = true;
        [SerializeField] private AmiliousAuthenticator authenticator;
        [Tooltip("What connections to automatically start on play.")]
        [SerializeField] private AutoStartType autoStartType = AutoStartType.Disabled;
        
        [Header("Colors")]
        [Tooltip("Color when socket is stopped.")] [SerializeField] private Color stoppedColor;        
        [Tooltip("Color when socket is changing.")] [SerializeField] private Color changingColor;
        [Tooltip("Color when socket is started.")] [SerializeField] private Color startedColor;
        
        [Header("Indicators")]
        [Tooltip("Indicator for server state.")] [SerializeField] private Image serverIndicator;
        [Tooltip("Indicator for client state.")] [SerializeField] private Image clientIndicator;

        [Header("Buttons")]
        [SerializeField] private Button serverButton;
        [SerializeField] private Button clientButton;
        
        [Header("Inputs")]
        [Tooltip("The user name field.")] [SerializeField]
        private TMP_InputField userName;
        [Tooltip("The password field.")] [SerializeField]
        private TMP_InputField password;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Found NetworkManager.
        /// </summary>
        private NetworkManager _networkManager;
        
        /// <summary>
        /// Current state of client socket.
        /// </summary>
        private LocalConnectionState _clientState = LocalConnectionState.Stopped;
        
        /// <summary>
        /// Current state of server socket.
        /// </summary>
        private LocalConnectionState _serverState = LocalConnectionState.Stopped;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is a shortcut to get the UserIdentityDataManager instance.
        /// </summary>
        private static UserIdentityDataManager DataManager => UserIdentityDataManager.Instance;

        /// <summary>
        /// This property is used to get and cache the network manager.
        /// </summary>
        private NetworkManager NetworkManager {
            get {
                if(_networkManager != null) return _networkManager;
                _networkManager = FindObjectOfType<NetworkManager>();
                if(_networkManager==null)
                    Debug.LogError("NetworkManager not found, HUD will not function.");
                return _networkManager;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Monobehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Start() {
            OnStoreCredentialsUpdated();
            UpdateColor(LocalConnectionState.Stopped, ref serverIndicator);
            UpdateColor(LocalConnectionState.Stopped, ref clientIndicator);
            if (autoStartType == AutoStartType.Host || autoStartType == AutoStartType.Server) OnClick_Server();
            if (!Application.isBatchMode && (autoStartType == AutoStartType.Host || 
                autoStartType == AutoStartType.Client)) OnClick_Client();
        }

        private void OnEnable() {
            if(NetworkManager == null) return;
            NetworkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
            NetworkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
            DataManager.Client_OnStoredCredentialsUpdated += OnStoreCredentialsUpdated;
            clientButton.onClick.AddListener(OnClick_Client);
            serverButton.onClick.AddListener(OnClick_Server);
        }

        private void OnDisable() {
            clientButton.onClick.RemoveListener(OnClick_Client);
            serverButton.onClick.RemoveListener(OnClick_Server);
            DataManager.Client_OnStoredCredentialsUpdated -= OnStoreCredentialsUpdated;
            if (NetworkManager == null) return;
            NetworkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
            NetworkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        public void OnClick_Server() {
            if (NetworkManager == null) return;
            if (_serverState != LocalConnectionState.Stopped)
                NetworkManager.ServerManager.StopConnection(true);
            else NetworkManager.ServerManager.StartConnection();
            DeselectButtons();
        }

        public void OnClick_Client() {
            if (NetworkManager == null) return;
            if (_clientState != LocalConnectionState.Stopped)
                NetworkManager.ClientManager.StopConnection();
            else {
                authenticator.SetCredentials(userName.text,password.text);
                NetworkManager.ClientManager.StartConnection();
            }
            DeselectButtons();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Updates img color based on state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="img"></param>
        protected virtual void UpdateColor(LocalConnectionState state, ref Image img) {
            Color c;
            if (state == LocalConnectionState.Started) c = startedColor;
            else if (state == LocalConnectionState.Stopped) c = stoppedColor;
            else c = changingColor;
            img.color = c;
        }
        
        protected virtual  void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj) {
            _clientState = obj.ConnectionState;
            if(_clientState == LocalConnectionState.Stopped) {
                userName.enabled = true;
                userName.textComponent.color = Color.white;
                password.enabled = true;
                password.textComponent.color = Color.white;
            }
            else {
                userName.enabled = false;
                userName.textComponent.color = userName.placeholder.color;
                password.enabled = false;
                password.textComponent.color = password.placeholder.color;
            }
            
            UpdateColor(obj.ConnectionState, ref clientIndicator);
        }

        protected virtual  void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj) {
            _serverState = obj.ConnectionState;
            UpdateColor(obj.ConnectionState, ref serverIndicator);
        }

        protected virtual  void OnStoreCredentialsUpdated() {
            if(rememberLastUserName&&userName!=null) {
                DataManager.Client_TryGetLastUserName(out var usrName);
                userName.text = usrName;
            }
            if(rememberLastPassword&&password!=null) {
                DataManager.Client_TryGetLastPassword(out var pass);
                password.text = pass;
            }
        }

        protected virtual  void DeselectButtons() { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}