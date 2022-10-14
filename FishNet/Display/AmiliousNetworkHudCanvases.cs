using Amilious.Core.FishNet.Authentication;
using FishNet.Managing;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amilious.Core.FishNet.Display {
    
    public class AmiliousNetworkHudCanvases : MonoBehaviour {
    
        #region Types.
        /// <summary>
        /// Ways the HUD will automatically start a connection.
        /// </summary>
        private enum AutoStartType { Disabled, Host, Server, Client }
        #endregion

        #region Serialized.
        
        [Tooltip("What connections to automatically start on play.")]
        [SerializeField]
        private AutoStartType autoStartType = AutoStartType.Disabled;
        
        [Tooltip("Color when socket is stopped.")]
        [SerializeField]
        private Color stoppedColor;
        
        [Tooltip("Color when socket is changing.")]
        [SerializeField]
        private Color changingColor;
        
        [Tooltip("Color when socket is started.")]
        [SerializeField]
        private Color startedColor;
        [Header("Indicators")]
        
        [Tooltip("Indicator for server state.")]
        [SerializeField]
        private Image serverIndicator;
        /// <summary>
        /// Indicator for client state.
        /// </summary>
        [Tooltip("Indicator for client state.")]
        [SerializeField]
        private Image clientIndicator;

        [SerializeField]
        private Button serverButton;

        [SerializeField] private Button clientButton;
        
        [Tooltip("The user name field.")]
        [SerializeField]
        private TMP_InputField userName;

        [Tooltip("The password field.")]
        [SerializeField]
        private TMP_InputField password;
        [SerializeField]
        private AmiliousAuthenticator authenticator;
        
        #endregion

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

            _networkManager = FindObjectOfType<NetworkManager>();
            if (_networkManager == null) {
                Debug.LogError("NetworkManager not found, HUD will not function.");
                return;
            }
            UpdateColor(LocalConnectionState.Stopped, ref serverIndicator);
            UpdateColor(LocalConnectionState.Stopped, ref clientIndicator);
            _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
            _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;

            if (autoStartType == AutoStartType.Host || autoStartType == AutoStartType.Server)
                OnClick_Server();
            if (!Application.isBatchMode && (autoStartType == AutoStartType.Host || autoStartType == AutoStartType.Client))
                OnClick_Client();

            clientButton.onClick.AddListener(OnClick_Client);
            serverButton.onClick.AddListener(OnClick_Server);

        }

        private void OnDestroy() {
            if (_networkManager == null) return;
            _networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
            _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
        }

        /// <summary>
        /// Updates img color baased on state.
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
            if (_networkManager == null) return;
            if (_serverState != LocalConnectionState.Stopped)
                _networkManager.ServerManager.StopConnection(true);
            else _networkManager.ServerManager.StartConnection();
            DeselectButtons();
        }

        public void OnClick_Client() {
            if (_networkManager == null) return;
            if (_clientState != LocalConnectionState.Stopped)
                _networkManager.ClientManager.StopConnection();
            else {
                authenticator.SetCredentials(userName.text,password.text);
                _networkManager.ClientManager.StartConnection();
            }
            DeselectButtons();
        }

        private void DeselectButtons() {
            
        }
    }
}