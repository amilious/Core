using System;
using Amilious.Core.Attributes;
using Amilious.Core.FishNet.Authentication;
using Amilious.Core.Identity.User;
using FishNet.Managing;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Amilious.Core.FishNet.Display {
    
    
    public class AmiliousNetworkHudCanvases : MonoBehaviour {
    
        #region Types
        
        /// <summary>
        /// Ways the HUD will automatically start a connection.
        /// </summary>
        private enum AutoStartType { Disabled, Host, Server, Client }
        #endregion

        #region Serialized

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
        
        #endregion
        
        private UserIdentityDataManager DataManager => UserIdentityDataManager.Instance;

        public NetworkManager NetworkManager {
            get {
                if(_networkManager != null) return _networkManager;
                _networkManager = FindObjectOfType<NetworkManager>();
                if(_networkManager==null)
                    Debug.LogError("NetworkManager not found, HUD will not function.");
                return _networkManager;
            }
        }

        #region Private
        
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
        
        #endregion

        private void Start() {
            OnStoreCredentialsUpdated();
            UpdateColor(LocalConnectionState.Stopped, ref serverIndicator);
            UpdateColor(LocalConnectionState.Stopped, ref clientIndicator);
            if (autoStartType == AutoStartType.Host || autoStartType == AutoStartType.Server)
                OnClick_Server();
            if (!Application.isBatchMode && (autoStartType == AutoStartType.Host || autoStartType == AutoStartType.Client))
                OnClick_Client();
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

        /// <summary>
        /// Updates img color based on state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="img"></param>
        private void UpdateColor(LocalConnectionState state, ref Image img) {
            Color c;
            if (state == LocalConnectionState.Started) c = startedColor;
            else if (state == LocalConnectionState.Stopped) c = stoppedColor;
            else c = changingColor;
            img.color = c;
        }
        
        private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj) {
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

        private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj) {
            _serverState = obj.ConnectionState;
            UpdateColor(obj.ConnectionState, ref serverIndicator);
        }

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

        private void OnStoreCredentialsUpdated() {
            if(rememberLastUserName&&userName!=null) {
                DataManager.Client_TryGetLastUserName(out var usrName);
                userName.text = usrName;
            }
            if(rememberLastPassword&&password!=null) {
                DataManager.Client_TryGetLastPassword(out var pass);
                password.text = pass;
            }
        }

        private void DeselectButtons() {
            
        }
    }
}