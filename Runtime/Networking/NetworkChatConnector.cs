using System;
using UnityEngine;
using Cysharp.Text;
using Amilious.Console;
using Amilious.Core.Chat;
using Amilious.Core.UI.Chat;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.Networking {
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ChatBox),typeof(AbstractNetworkManagers))]
    public class NetworkChatConnector : AmiliousBehavior {

        private const string DISPLAY = "Display Settings";
        private const string NOTIFICATIONS = "Notifications";

        [SerializeField, AmiTab(DISPLAY)] private bool showTime = true;
        [SerializeField, AmiTab(DISPLAY), AmiShowIf(nameof(showTime))] private string timeFormat = "hh:mm:ss tt";

        [SerializeField, AmiTab(NOTIFICATIONS), AmiBool(true)] private bool showSelfConnection = true;
        [SerializeField, AmiTab(NOTIFICATIONS), AmiBool(true)] private bool showFriendConnections = true;

        

        private AbstractNetworkManagers _networkManager;
        private Utf16ValueStringBuilder _sb = new Utf16ValueStringBuilder(false);

        public ChatBox ChatBox { get; private set; }
        public AbstractNetworkManagers NetworkManager => this.GetCacheComponent(ref _networkManager);
        public IUserIdentityManager UserManager { get; private set; }
        public IGroupIdentityManager GroupManager { get; private set; }
        public IChatManager ChatManager {get; private set; }
        
        
        private void Awake() {
            ChatBox = GetComponent<ChatBox>();
        }

        private void OnEnable() {
            NetworkManager.OnClientStarted += OnClientStarted;
            NetworkManager.OnClientStopped += OnClientStopped;
            ChatBox.OnMessageSubmitted += OnMessageSubmitted;
            if(ChatManager != null) {
                ChatManager.OnReceiveGlobalMessage += OnReceiveGlobalMessage;
                ChatManager.OnReceiveGroupMessage += OnReceiveGroupMessage;
                ChatManager.OnReceivePrivateMessage += OnReceivePrivateMessage;
                ChatManager.OnReceiveServerMessage += OnReceiveServerMessage;
                ChatManager.OnGroupMessageSent += OnGroupMessageSent;
                ChatManager.OnPrivateMessageSent += OnPrivateMessageSent;
            }
            if(UserManager != null) {
                UserManager.OnUserConnectionChanged += OnUserConnectionChanged;
            }
        }

        private void OnDisable() {
            NetworkManager.OnClientStarted -= OnClientStarted;
            NetworkManager.OnClientStopped -= OnClientStopped;
            ChatBox.OnMessageSubmitted -= OnMessageSubmitted;
            if(ChatManager != null) {
                ChatManager.OnReceiveGlobalMessage -= OnReceiveGlobalMessage;
                ChatManager.OnReceiveGroupMessage -= OnReceiveGroupMessage;
                ChatManager.OnReceivePrivateMessage -= OnReceivePrivateMessage;
                ChatManager.OnReceiveServerMessage -= OnReceiveServerMessage;
                ChatManager.OnGroupMessageSent -= OnGroupMessageSent;
                ChatManager.OnPrivateMessageSent -= OnPrivateMessageSent;
            }
            if(UserManager != null) {
                UserManager.OnUserConnectionChanged -= OnUserConnectionChanged;
            }
        }

        private void OnClientStopped() {
            if(!showSelfConnection) return;
            _sb.Clear();
            _sb.AppendStyle(StyleFormat.Server,"Server");
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.Server,"You have disconnected from the server!");
            ChatBox.AddMessage(_sb.ToString());
        }

        private void OnClientStarted() {
            if(!showSelfConnection) return;
            _sb.Clear();
            _sb.AppendStyle(StyleFormat.Server,"Server");
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.Server,"You have connected to the server!");
            ChatBox.AddMessage(_sb.ToString());
        }

        private void Start() {
            NetworkManager.OnChatManagerRegistered(OnChatManagerRegistered);
            NetworkManager.OnGroupManagerRegistered(OnGroupManagerRegistered);
            NetworkManager.OnUserManagerRegistered(OnUserManagerRegistered);
        }

        private void OnGroupManagerRegistered(IGroupIdentityManager groupManager) {
            GroupManager = groupManager;
        }

        private void OnUserManagerRegistered(IUserIdentityManager userManager) {
            UserManager = userManager;
            UserManager.OnUserConnectionChanged += OnUserConnectionChanged;
        }

        private void OnChatManagerRegistered(IChatManager chatManager) {
            ChatManager = chatManager;
            ChatManager.OnReceiveGlobalMessage += OnReceiveGlobalMessage;
            ChatManager.OnReceiveGroupMessage += OnReceiveGroupMessage;
            ChatManager.OnReceivePrivateMessage += OnReceivePrivateMessage;
            ChatManager.OnReceiveServerMessage += OnReceiveServerMessage;
            ChatManager.OnGroupMessageSent += OnGroupMessageSent;
            ChatManager.OnPrivateMessageSent += OnPrivateMessageSent;
        }

        private void OnPrivateMessageSent(UserIdentity receiver, string message) {
            var sender = UserManager.Client_GetIdentity();
            _sb.Clear();
            _sb.Append(GetUserLink(sender,false));
            _sb.AppendStyle(StyleFormat.Normal,"->");
            _sb.Append(GetUserLink(receiver,false));
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.PrivateSentText,message);
            ChatBox.AddMessage(_sb.ToString(),sender);
        }

        private void OnGroupMessageSent(GroupIdentity group, string message) {
            
        }
        
        private void OnUserConnectionChanged(UserIdentity identity, bool connected) {
            if(!showFriendConnections||!identity.IsFriend) return;
            _sb.Clear();
            _sb.AppendStyle(StyleFormat.Server,"Server");
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.Friend,identity.Link);
            _sb.Space();
            _sb.AppendStyle(StyleFormat.Friend,connected?"has connected!":"has disconnected!");
            ChatBox.AddMessage(_sb.ToString());
        }

        private void OnMessageSubmitted(ChatType type, string message, uint id) {
            if(ChatManager == null || NetworkManager.IsOffline || NetworkManager.IsServerOnly) {
                ChatBox.AddMessage(message);
                return;
            }
            switch(type) {
                case ChatType.Global:ChatManager.SendGlobalMessage(message); break;
                case ChatType.Private:ChatManager.SendPrivateMessage(id,message); break;
                case ChatType.Group: ChatManager.SendGroupMessage(id,message); break;
                default: ChatBox.AddMessage(message); break;
            }
        }
        
        private void OnReceiveServerMessage(string message) {
            _sb.Clear();
            _sb.AppendStyle(StyleFormat.Server,"Server");
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.Server,message);
            ChatBox.AddMessage(_sb.ToString());
        }

        private void OnReceivePrivateMessage(UserIdentity sender, string message) {
            _sb.Clear();
            _sb.Append(GetUserLink(sender,false));
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.PrivateReceivedText,message);
            ChatBox.AddMessage(_sb.ToString(),sender);
        }

        private void OnReceiveGroupMessage(UserIdentity sender, GroupIdentity group, string message) {
            _sb.Clear();
            _sb.Append(GetGroupLink(group));
            _sb.Space();
            _sb.Append(GetUserLink(sender,false));
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.Group,message);
            ChatBox.AddMessage(_sb.ToString(),sender, group);
        }

        private void OnReceiveGlobalMessage(UserIdentity sender, string message) {
            _sb.Clear();
            _sb.Append(GetUserLink(sender,false));
            if(showTime) {
                _sb.Space();
                _sb.AppendStyle(StyleFormat.TimeString, DateTime.Now.ToString(timeFormat));
            }
            _sb.AppendStyle(StyleFormat.Normal,": ");
            _sb.AppendStyle(StyleFormat.Normal,message);
            ChatBox.AddMessage(_sb.ToString(),sender);
        }

        public string GetUserLink(UserIdentity user, bool withColon = true) {
            if(UserManager==null||user==UserIdentity.DefaultUser) return $"<link=user|{user.Id}>user_{user.Id}</link>";
            var link = user.UserType switch {
                UserType.Server => ZString.Style(StyleFormat.Server, user.Link),
                UserType.AmiliousConsole => ZString.Style(StyleFormat.Command, user.Link),
                UserType.DefaultUser => ZString.Style(StyleFormat.User, user.Link),
                UserType.User => UserIdentity.Manager.Client_IsFriend(user)
                    ? ZString.Style(StyleFormat.Friend, user.Link)
                    : ZString.Style(StyleFormat.User, user.Link),
                _ => ZString.Style(StyleFormat.User, user.Link)
            };
            return withColon ? $"{link}{ZString.Style(StyleFormat.Normal, ":  ")}" : link;
        }
        private string GetGroupLink(GroupIdentity group) {
            if(GroupManager == null || group == GroupIdentity.Default)
                return $"[<link=group|{group.Id}>group_{group.Id}</link>]";
            return $"[{group.Link}]";
        }
        
    }
}