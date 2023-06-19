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
using System.IO;
using System.Linq;
using UnityEngine;
using Amilious.Core.Security;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using System.Runtime.Serialization.Formatters.Binary;

// ReSharper disable MemberCanBePrivate.Global
namespace Amilious.Core.IO {
    
    /// <summary>
    /// This class is used to save the user's identity information
    /// </summary>
    public static class IdentitySave {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string SAVE_NAME = "IdentitySave";
        private const string LABEL_COLOR = "#FFA500";
        private const string CURRENT_VERSION_STRING = "1.0.0.0";
        private const string BUILT_SAVE_FILE = "identity.data";
        private const string EDITOR_SAVE_FILE = "identity.editor.data";
        private const string DEVELOPER_SAVE_FILE = "identity.developer.data";
        private const string DEVELOPER_X_SAVE_FILE = "identity.developer{0}.data";
        private const string IDENTITY_SAVE_VERSION_KEY = "**identity_save_version**";
        private const string SERVER_IDENTIFIER = "**server_identifier**";
        private const string SERVER_USER_DATA = "**server_user_data**";
        private const string SERVER_USER_BLOCKED = "**server_user_blocked**";
        private const string SERVER_USER_FRIENDS = "**server_user_friends**";
        private const string SERVER_NEXT_IDENTITY_ID = "**server_next_identity_id**";
        private const string SERVER_NEXT_GROUP_ID = "**server_next_channel_id**";
        private const string SERVER_GROUP_DATA = "**server_group_data**";
        private const string SERVER_GROUP_MEMBERS = "**server_group_members**";
        private const int SERVER_IDENTIFIER_LENGTH = 24;
        private const string CLIENT_USER_NAME = "**client_user_name**";
        private const string CLIENT_PASSWORD = "**client_password**";
        private const string CLIENT_REPLY_IDENTITY = "**reply_identity";
        private const string TITLE = "<b><color="+LABEL_COLOR+">["+SAVE_NAME+"]</color></b>";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary contains all of the save data.
        /// </summary>
        private static Dictionary<string, object> _data = new Dictionary<string, object>();
        
        /// <summary>
        /// This dictionary is used to store all of the user data.
        /// </summary>
        private static Dictionary<int, Dictionary<string, object>> _serverUserData = 
            new Dictionary<int, Dictionary<string, object>>();
        
        /// <summary>
        /// This dictionary is used to store all of the users that other users have blocked.
        /// </summary>
        private static Dictionary<int, List<int>> _serverUserBlocked = new Dictionary<int, List<int>>();
        
        /// <summary>
        /// This dictionary is used to store all of the users that other users have friended.
        /// </summary>
        private static Dictionary<int, List<int>> _serverUserFriends = new Dictionary<int, List<int>>();

        private static Dictionary<int, List<int>> _serverGroupMembers = new Dictionary<int, List<int>>();

        private static Dictionary<int, Dictionary<string, object>> _serverGroupData = 
            new Dictionary<int, Dictionary<string, object>>();
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered after the data has been loaded or reloaded.
        /// </summary>
        public static event Action OnAfterLoad;

        /// <summary>
        /// This event is triggered before the data is saved.
        /// </summary>
        public static event Action OnBeforeSave;

        /// <summary>
        /// This event is triggered when the data is being reset.
        /// </summary>
        public static event Action OnResetting;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the newest version of the console data.
        /// </summary>
        public static Version Version { get; }

        /// <summary>
        /// This property contains the save path.
        /// </summary>
        public static string SavePath { get; }

        /// <summary>
        /// If this property is true the data will be saved whenever a value is updated.
        /// </summary>
        public static bool SaveWhenUpdated { get; set; } = false;

        /// <summary>
        /// If this property is true loading and saving will be logged.
        /// </summary>
        public static bool ShowSaveAndLoadLogs { get; set; } = true;

        public static bool DataChanged { get; private set; } = false;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Constructor /////////////////////////////////////////////////////////////////////////////////////
        
        static IdentitySave() {
            Version = Version.Parse(CURRENT_VERSION_STRING);
            var forward = Application.persistentDataPath.Contains('/');
            SavePath = BUILT_SAVE_FILE;
            if(Application.isEditor) SavePath = EDITOR_SAVE_FILE;
            else if(Debug.isDebugBuild) {
                SavePath = !AmiliousCore.TryGetInstanceId(out var id) ? 
                    DEVELOPER_SAVE_FILE : string.Format(DEVELOPER_X_SAVE_FILE, id);
            }
            SavePath = Path.Combine(Application.persistentDataPath,SavePath);
            //make the path uniform
            SavePath = forward ? SavePath.Replace('\\', '/') : SavePath.Replace('/', '\\');
            //load
            Load();
            //automatically save when the application is quitting
            Application.quitting += Save;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Saving And Loading /////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to load the console data.
        /// </summary>
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Amilious/Core/Identity Save/Reload Data",false,AmiliousCore.PACKAGE_ID)]
        #endif
        public static void Load() {
            //create new save data
            if(!File.Exists(SavePath)) {Reset(areYouSure: true);}
            //load existing data
            using(var fileStream = File.Open(SavePath, FileMode.Open)) {
                var formatter = new BinaryFormatter();
                _data = (Dictionary<string, object>)formatter.Deserialize(fileStream);
            }
            //get the version
            if(_data.TryGetCastValue(IDENTITY_SAVE_VERSION_KEY, out string versionText)) {
                var version = new Version(versionText);
                if(version != Version) UpgradeData(version, _data);
            }
            //load
            if(!_data.TryGetCastValue(SERVER_USER_DATA, out _serverUserData))
                _serverUserData = new Dictionary<int, Dictionary<string, object>>();
            if(!_data.TryGetCastValue(SERVER_USER_BLOCKED, out _serverUserBlocked))
                _serverUserBlocked = new Dictionary<int, List<int>>();
            if(!_data.TryGetCastValue(SERVER_USER_FRIENDS, out _serverUserFriends))
                _serverUserFriends = new Dictionary<int, List<int>>();
            if(!_data.TryGetCastValue(SERVER_GROUP_DATA, out _serverGroupData))
                _serverGroupData = new Dictionary<int, Dictionary<string, object>>();
            if(!_data.TryGetCastValue(SERVER_GROUP_MEMBERS, out _serverGroupMembers))
                _serverGroupMembers = new Dictionary<int, List<int>>();
            if(ShowSaveAndLoadLogs)
                Debug.Log($"{TITLE} <color=#00FF00>Loaded the save data!</color>");
            OnAfterLoad?.Invoke();
        }

        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Amilious/Core/Identity Save/Reset Data",false,AmiliousCore.PACKAGE_ID+2)]
        public static void Reset() {
            var sure = UnityEditor.EditorUtility.DisplayDialog("Reset Identity Save Data",
                "Are you sure that you want to delete the currently saved data?"
                , "Yes, I am sure!", "Cancel");
            Reset(sure);
        }
        #endif

        /// <summary>
        /// This method is used to reset the save data.
        /// </summary>
        /// <param name="areYouSure">If true the data will be reset.</param>
        public static void Reset(bool areYouSure) {
            if(!areYouSure) return;
            //clear data
            _serverUserData.Clear();
            _serverUserBlocked.Clear();
            _serverUserFriends.Clear();
            _serverGroupData.Clear();
            _serverGroupMembers.Clear();
            _data.Clear();
            //data containers
            _data[SERVER_USER_DATA] = _serverUserData;
            _data[SERVER_USER_BLOCKED] = _serverUserBlocked;
            _data[SERVER_USER_FRIENDS] = _serverUserFriends;
            _data[SERVER_GROUP_DATA] = _serverGroupData;
            _data[SERVER_GROUP_MEMBERS] = _serverGroupMembers;
            OnResetting?.Invoke();
            DataChanged = true;
            Debug.Log($"{TITLE}  <color=#FF0000>Reset the save data!</color>");
            Save();
        }

        /// <summary>
        /// This method is used to save the console data.
        /// </summary>
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Amilious/Core/Identity Save/Force Save",false,AmiliousCore.PACKAGE_ID+1)]
        #endif
        public static void Save() {
            OnBeforeSave?.Invoke();
            if(!DataChanged) return;
            if(!Application.isPlaying) {
                Debug.Log($"{TITLE} <color=#00FF00>Can only save when the game is running!</color>");
                return;
            }
            using(var fileStream = File.Open(SavePath, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, _data);
                fileStream.Close();
            }
            DataChanged = false;
            if(ShowSaveAndLoadLogs)
                Debug.Log($"{TITLE} <color=#00FF00>Saved the save data!</color>");
        }

        /// <summary>
        /// This method will be called if the data structure has been altered.
        /// </summary>
        /// <param name="oldVersion">The version that was loaded.</param>
        /// <param name="data">The loaded data that needs to be modified.</param>
        private static void UpgradeData(Version oldVersion, Dictionary<string, object> data) {}
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Data Specific Methods //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to save data to the identity save.
        /// </summary>
        /// <param name="key">The data's key.</param>
        /// <param name="value">The data's value.</param>
        /// <typeparam name="T">The type of data being stored.</typeparam>
        public static void StoreData<T>(string key, T value) {
            //we do not need to save the data if it has not changed.
            if(_data.TryGetCastValue(key,out T value2)&&value2.Equals(value)) return;
            _data[key] = value;
            DataChanged = true;
            if(SaveWhenUpdated) Save();
        }

        /// <summary>
        /// This method is used to read saved data from the identity save.
        /// </summary>
        /// <param name="key">The data's key.</param>
        /// <param name="value">The data's value.</param>
        /// <typeparam name="T">The type of data being stored.</typeparam>
        /// <returns>True if the data exists and is the correct type, otherwise false.</returns>
        public static bool TryReadData<T>(string key, out T value) {
            return _data.TryGetCastValue(key, out value);
        }
        
        /// <summary>
        /// This method is used to read saved data from the save file.
        /// </summary>
        /// <param name="key">The data's key.</param>
        /// <param name="defaultValue">This value will be returned and added to the save file if no value exists.</param>
        /// <typeparam name="T">The type of data being stored.</typeparam>
        /// <returns>The value of the given key or the default value.</returns>
        public static T ReadData<T>(string key, T defaultValue = default) {
            if(_data.TryGetCastValue<T>(key, out var value)) return value;
            StoreData(key,defaultValue);
            return defaultValue;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server & Users /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the server's identifier.
        /// </summary>
        /// <returns>The servers identifier.</returns>
        public static string Server_GetServerIdentifier() {
            //try get existing identifier
            if(TryReadData(SERVER_IDENTIFIER, out string serverIdentifier)) return serverIdentifier;
            //generate new identifier
            serverIdentifier = PasswordTools.GetRandomString(SERVER_IDENTIFIER_LENGTH);
            //store the identifier
            StoreData(SERVER_IDENTIFIER,serverIdentifier);
            // return the new identifier
            return serverIdentifier;
        }

        /// <summary>
        /// This method is used to check if the given user id is valid.
        /// </summary>
        /// <param name="userId">The user id that you want to check.</param>
        /// <returns>True if the given user id is valid, otherwise false.</returns>
        public static bool Server_IsUserIdValid(int userId) => _serverUserData.ContainsKey(userId);

        /// <summary>
        /// This method is used to try get a user identity from the user's user id.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="identity">The user's identity.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        public static bool Server_TryGetUserIdentity(int userId, out UserIdentity identity) {
            //make sure the user id is valid
            if(!Server_IsUserIdValid(userId)) {
                identity = default;
                return false;
            }
            //get the user's user name
            if(!Server_TryReadUserData(userId, UserIdentity.USER_NAME_KEY, out string userName)) {
                identity = default;
                return false;
            }
            //get the user's authority
            identity = Server_TryReadUserData(userId, UserIdentity.AUTHORITY_KEY, out int authority) ? 
                new UserIdentity(userId, userName, authority) : new UserIdentity(userId, userName);
            return true;
        }

        /// <summary>
        /// This method is used to get a user identity from the user's user name.
        /// </summary>
        /// <param name="userName">The user's user name.</param>
        /// <param name="identity">The user's identity.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        public static bool Server_TryGetUserIdentity(string userName, out UserIdentity identity) {
            if(Server_TryGetIdFromUserName(userName, out int id))
                return Server_TryGetUserIdentity(id, out identity);
            identity = default;
            return false;
        }

        /// <summary>
        /// This method is used to try get a user id from the given user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="userId">The user id associated with the user id.</param>
        /// <param name="caseSensitive">True if the user name is case sensitive, otherwise false.</param>
        /// <returns>True if able to find a user with the given user name, otherwise false.</returns>
        public static bool Server_TryGetIdFromUserName(string userName, out int userId, bool caseSensitive = false) {
            if(userName == null) { userId = 0; return false; }
            var culture = caseSensitive ? StringComparison.InvariantCulture : 
                StringComparison.InvariantCultureIgnoreCase;
            foreach(var data in _serverUserData) {
                if(!data.Value.TryGetCastValue(UserIdentity.USER_NAME_KEY, out string storedName) || 
                   !userName.Equals(storedName,culture)) continue;
                userId = data.Key;
                return true;
            }
            userId = 0;
            return false;
        }

        /// <summary>
        /// This method is used to try read data for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="key">The key for the data that you want to read.</param>
        /// <param name="value">The read value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>True if the data exists, otherwise false.</returns>
        public static bool Server_TryReadUserData<T>(int userId, string key, out T value) {
            value = default;
            return _serverUserData.TryGetValueFix(userId, out var info) && 
                   info.TryGetCastValue(key, out value);
        }

        /// <summary>
        /// This method is used to store data for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="key">The key for the data that you want to store.</param>
        /// <param name="value">The value that you want to store for the given key.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        public static void Server_StoreUserData<T>(int userId, string key, T value) {
            if(!_serverUserData.TryGetValueFix(userId, out var info)) {
                info = new Dictionary<string, object>();
                _serverUserData[userId] = info;
            }
            //we do not need to mark the data dirty if there is no change
            if(info.TryGetCastValue(key,out T value2)&&value2.Equals(value)) return;
            info[key] = value;
            DataChanged = true;
            if(SaveWhenUpdated) Save();
        }

        /// <summary>
        /// This method is used to add a new user to the server.
        /// </summary>
        /// <param name="userName">The user name for the user.</param>
        /// <returns>The id for the new user.</returns>
        public static UserIdentity Server_AddUser(string userName = null) {
            TryReadData(SERVER_NEXT_IDENTITY_ID, out int id); id++;
            StoreData(SERVER_NEXT_IDENTITY_ID,id);
            if(string.IsNullOrWhiteSpace(userName)) userName = $"User{id}";
            Server_StoreUserData(id, UserIdentity.USER_NAME_KEY ,userName);
            return Server_TryGetUserIdentity(id, out var identity)?identity:default;
        }

        /// <summary>
        /// This method is used to get the id's for all of the registered users.
        /// </summary>
        /// <returns>The id's of the registered users.</returns>
        public static IEnumerable<int> Server_GetStoredUserIds() => _serverUserData.Keys;

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="blocker">The user id of the user who is blocking another user.</param>
        /// <param name="blocked">The user id of the user that will be blocked.</param>
        public static void Server_BlockUser(int blocker, int blocked) {
            if(!_serverUserBlocked.ContainsKey(blocker)) _serverUserBlocked[blocker] = new List<int>();
            if(_serverUserBlocked[blocker].Contains(blocked)) return;
            //we only need to save if the data has changed.
            _serverUserBlocked[blocker].Add(blocked);
            DataChanged = true;
            if(SaveWhenUpdated) Save();
        }

        /// <summary>
        /// This method is used to unblock a user.
        /// </summary>
        /// <param name="blocker">The user id of the user who is blocking another user.</param>
        /// <param name="blocked">The user id of the user that will be unblocked.</param>
        public static void Server_UnblockUser(int blocker, int blocked) {
            //if there is no list the user was not blocked
            if(!_serverUserBlocked.ContainsKey(blocker)) return;
            if(!_serverUserBlocked[blocker].Contains(blocked)) return;
            //we only need to save if the data has changed.
            _serverUserBlocked[blocker].Remove(blocked);
            DataChanged = true;
            if(SaveWhenUpdated) Save();
        }

        /// <summary>
        /// This method is used to check if a user has blocked another.
        /// </summary>
        /// <param name="blocker">The user id of the user who blocked another user.</param>
        /// <param name="blocked">The user id of the user who was blocked.</param>
        /// <returns>True if the <paramref name="blocker"/> has blocked the <paramref name="blocked"/>, otherwise false.
        /// </returns>
        public static bool Server_HasBlocked(int blocker, int blocked) {
            return _serverUserBlocked.TryGetValueFix(blocker, out var hashSet)&&hashSet.Contains(blocked);
        }

        /// <summary>
        /// This method is used to get a list of the blocked user's for the given user.
        /// </summary>
        /// <param name="blockerId">The id of the blocker.</param>
        /// <returns>A list of the user's currently blocked by the given user.</returns>
        public static List<int> Server_GetBlockedUsers(int blockerId) {
            return _serverUserBlocked.TryGetValueFix(blockerId, out var blocked) ? 
                blocked.ToList() : new List<int>();
        }

        /// <summary>
        /// This method is used to friend a user.
        /// </summary>
        /// <param name="userId">The user id of the user who is friending another user.</param>
        /// <param name="friendId">The user id of the user that will be friended.</param>
        public static void Server_FriendUser(int userId, int friendId) {
            if(!_serverUserFriends.ContainsKey(userId)) _serverUserFriends[userId] = new List<int>();
            if(_serverUserFriends[userId].Contains(friendId)) return;
            //we only need to save if the data has changed.
            _serverUserFriends[userId].Add(friendId);
            DataChanged = true;
            if(SaveWhenUpdated) Save();
        }

        /// <summary>
        /// This method is used to unfriend a user.
        /// </summary>
        /// <param name="userId">The user id of the user who is unfriending another user.</param>
        /// <param name="friendId">The user id of the user that will be unfriended.</param>
        public static void Server_UnfriendUser(int userId, int friendId) {
            //if there is no list the user was not blocked
            if(!_serverUserFriends.ContainsKey(userId)) return;
            if(!_serverUserFriends[userId].Contains(friendId)) return;
            //we only need to save if the data has changed.
            _serverUserFriends[userId].Remove(friendId);
            DataChanged = true;
            if(SaveWhenUpdated) Save();
        }

        /// <summary>
        /// This method is used to check if a user has friended another.
        /// </summary>
        /// <param name="userId">The user id of the user who friended another user.</param>
        /// <param name="friendId">The user id of the user who was friended.</param>
        /// <returns>True if the <paramref name="userId"/> has friended the <paramref name="friendId"/>, otherwise
        /// false. This does not mean that the friendship has been approved.
        /// </returns>
        public static bool Server_HasFriended(int userId, int friendId) {
            return _serverUserFriends.TryGetValueFix(userId, out var friends)&&friends.Contains(friendId);
        }

        /// <summary>
        /// This method is used to check if their is an approved friendship between the two users.
        /// </summary>
        /// <param name="userId1">The first user.</param>
        /// <param name="userId2">The second user.</param>
        /// <returns>True if the two user's have an approved friendship together, otherwise false.</returns>
        public static bool Server_HasApprovedFriendship(int userId1, int userId2) {
            return Server_HasFriended(userId1, userId2) && Server_HasFriended(userId2, userId1);
        }

        /// <summary>
        /// This method is used to get a list of the friends for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of the user's friends including unaccepted friends.</returns>
        public static List<int> Server_GetFriends(int userId) {
            return _serverUserFriends.TryGetValueFix(userId, out var friends) ? 
                friends.ToList() : new List<int>();
        }
        
        /// <summary>
        /// This method is used to get a list of the approved friends for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of the user's friends excluding unaccepted friends.</returns>
        public static List<int> Server_GetApprovedFriends(int userId) {
            if(!_serverUserFriends.TryGetValueFix(userId, out var friends)) 
                return new List<int>();
            var friendList = new List<int>();
            foreach(var friend in friends) 
                if(Server_HasFriended(friend,userId))
                    friendList.Add(friend);
            return friendList;
        }
        
        /// <summary>
        /// This method is used to get a list of the friend requests that have not been approved yet.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of the user's friend requests that have not yet been approved.</returns>
        public static List<int> Server_GetNotApprovedFriends(int userId) {
            if(!_serverUserFriends.TryGetValueFix(userId, out var friends))
                return new List<int>();
            var notApproved = new List<int>();
            foreach(var friend in friends) 
                if(!Server_HasFriended(friend,userId))
                    notApproved.Add(friend);
            return notApproved;
        }

        /// <summary>
        /// This method is used to get a list of user's that are requesting friendship with the given user id.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of user ids that are requesting friendship.</returns>
        public static List<int> Server_GetRequestingFriends(int userId) {
            if(!_serverUserFriends.TryGetValueFix(userId, out var friends)) 
                return new List<int>();
            var requestList = new List<int>();
            foreach(var item in _serverUserFriends) {
                if(friends.Contains(item.Key)) continue; //already accepted
                if(!item.Value.Contains(userId)) continue; //not requesting
                requestList.Add(item.Key);
            }
            return requestList;
        }
        
        /// <summary>
        /// This method is used to check if the user with the given id has set their password.
        /// </summary>
        /// <param name="userId">The user id of the user you want to check.</param>
        /// <returns>True if the user has set their password, otherwise false.</returns>
        public static bool Server_HasUserSetPassword(int userId) {
            Server_TryReadUserData(userId, UserIdentity.PASSWORD_KEY, out string password);
            return !string.IsNullOrWhiteSpace(password);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Client Save Methods ////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to store the reply identity id.
        /// </summary>
        /// <param name="id">The id that should be used for a reply.</param>
        public static void Client_StoreReplyIdentity(int id) {
            StoreData(CLIENT_REPLY_IDENTITY,id);
        }

        /// <summary>
        /// This method is used to get the reply identity id.
        /// </summary>
        /// <param name="id">The reply identity id.</param>
        /// <returns>True if the reply id exists, otherwise false.</returns>
        public static bool Client_TryGetReplyIdentity(out int id) {
            return TryReadData(CLIENT_REPLY_IDENTITY, out id);
        }
        
        /// <summary>
        /// This method is used to remember the last entered user name.
        /// </summary>
        /// <param name="userName">The user name that you want to remember.</param>
        public static void Client_StoreLastUserName(string userName) {
            StoreData(CLIENT_USER_NAME,userName);
        }

        /// <summary>
        /// This method is used to remember the last entered password.
        /// </summary>
        /// <param name="password">The password that you want to remember.</param>
        public static void Client_StoreLastPassword(string password) {
            StoreData(CLIENT_PASSWORD,password);
        }

        /// <summary>
        /// This method is used to get the saved user name.
        /// </summary>
        /// <param name="userName">The user name that you saved.</param>
        /// <returns>True if there is a saved user name, otherwise false.</returns>
        public static bool Client_TryGetLastUserName(out string userName) {
            return TryReadData(CLIENT_USER_NAME, out userName);
        }

        /// <summary>
        /// This method is used to get the saved password.
        /// </summary>
        /// <param name="password">The password that you saved.</param>
        /// <returns>True if there is a saved password, otherwise false.</returns>
        public static bool Client_TryGetLastPassword(out string password) {
            return TryReadData(CLIENT_PASSWORD, out password);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Group Server Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get data for the group with the given id.
        /// </summary>
        /// <param name="id">The group's id.</param>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value for the given key.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>True if able to read the data, otherwise false.</returns>
        public static bool Server_TryReadGroupData<T>(int id, string key, out T value) {
            if(_serverGroupData.TryGetValueFix(id, out var channelInfo))
                return channelInfo.TryGetCastValue(key, out value);
            value = default;
            return false;
        }

        /// <summary>
        /// This method is used to store data for the group with the given id.
        /// </summary>
        /// <param name="id">The group's id.</param>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value for the given key.</param>
        /// <param name="save">If true the data will be written to a file after updating the value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        public static void Server_StoreGroupData<T>(int id, string key, T value, bool save = true) {
            if(!_serverGroupData.TryGetValueFix(id, out var groupIdentity)) {
                groupIdentity = new Dictionary<string, object>();
            }
            groupIdentity[key] = value;
            if(save) Save();
        }

        /// <summary>
        /// This method is used to add a user to a group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to add a user to.</param>
        /// <param name="userId">The id of the user that you want to add to the group.</param>
        /// <param name="save">If true the data will be written to a file after updating the value.</param>
        /// <returns>True if the user was added to the group, otherwise false if the user
        /// was already in the channel.</returns>
        public static bool Server_AddUserToGroup(int groupId, int userId, bool save = true) {
            if(!_serverGroupMembers.TryGetValueFix(groupId, out var groupMembers)) {
                groupMembers = new List<int>();
            }
            if(groupMembers.Contains(userId)) return false;
            groupMembers.Add(userId);
            if(save) Save();
            return true;
        }

        /// <summary>
        /// This method is used to remove a user from a group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to remove a user from.</param>
        /// <param name="userId">The id of the user that you want to remove from the group.</param>
        /// <param name="save">If true the data will be written to a file after updating the value.</param>
        /// <returns>True if the user was removed from the group, otherwise false if the user was not
        /// part of the group.</returns>
        public static bool Server_RemoveUserFromGroup(int groupId, int userId, bool save = true) {
            if(!_serverGroupMembers.TryGetValueFix(groupId, out var groupMembers)) return false;
            groupMembers.Remove(userId);
            if(save) Save();
            return true;
        }

        /// <summary>
        /// This method is used to get all of the group ids.
        /// </summary>
        /// <returns>The group ids.</returns>
        public static IEnumerable<int> Server_GetGroupIds() {
            return _serverGroupMembers.Keys;
        }

        /// <summary>
        /// This method is used to try get the groups for a given user.
        /// </summary>
        /// <param name="userId">The id of the user that you want to get the groups for.</param>
        /// <param name="channels">The groups that the user is a member of.</param>
        /// <returns>True if the user is a member of a group, otherwise false.</returns>
        public static bool Server_TryGetUsersGroups(int userId, out IEnumerable<int> channels) {
            var results = new List<int>();
            foreach(var group in _serverGroupMembers) {
                if(group.Value.Contains(userId))results.Add(group.Key);
            }
            channels = results;
            return results.Count > 0;
        }

        /// <summary>
        /// This method is used to get the members of the given group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to get the members of.</param>
        /// <param name="members">The members of the group with the given id.</param>
        /// <returns>True if the group contains members, otherwise false.</returns>
        public static bool Server_TryGetGroupMembers(int groupId, out IEnumerable<int> members) {
            if(_serverGroupMembers.TryGetValueFix(groupId, out var groupMembers)) {
                members = groupMembers;
                return true;
            }
            members = null;
            return false;
        }

        /// <summary>
        /// This method is used to get the number of members for a given group.
        /// </summary>
        /// <param name="groupId">The id of the group.</param>
        /// <param name="count">The number of members in the group.</param>
        /// <returns>True if the group exists, otherwise false.</returns>
        public static bool Server_TryGetGroupMemberCount(int groupId, out int count) {
            if(_serverGroupMembers.TryGetValueFix(groupId, out var groupMembers)) {
                count = groupMembers.Count;
                return true;
            }
            count = 0;
            return false;
        }

        /// <summary>
        /// This method is used to remove a group.
        /// </summary>
        /// <param name="gorupId">The id of the group that you want to remove.</param>
        /// <param name="save">If true the data will be written to a file after updating the value.</param>
        /// <returns>True if the group was removed, otherwise false if the group did not exist.</returns>
        public static bool Server_RemoveGroup(int gorupId, bool save = true) {
            if(!_serverGroupData.ContainsKey(gorupId)) return false;
            _serverGroupData.Remove(gorupId);
            if(save) Save();
            return true;
        }

        /// <summary>
        /// This method is used to add a new group to the server.
        /// </summary>
        /// <param name="groupName">The name for the group.</param>
        /// <param name="groupType">The type of group that you want to create.</param>
        /// <param name="save">If true the data will be written to a file after updating the value.</param>
        /// <returns>The id for the new group.</returns>
        public static int Server_AddGroup(string groupName = null, GroupType groupType = GroupType.Chat, 
            bool save = true) {
            var id = Server_TakeNextGroupId();
            if(string.IsNullOrWhiteSpace(groupName)) groupName = $"Group{id}";
            Server_StoreGroupData(id, GroupIdentity.GROUP_NAME_KEY ,groupName);
            Server_StoreGroupData(id,GroupIdentity.GROUP_TYPE_KEY, (byte)groupType);
            if(save) Save();
            return id;
        }
        
        /// <summary>
        /// This method is used to get the next available group id.
        /// </summary>
        /// <returns>The next available group id.</returns>
        public static int Server_TakeNextGroupId() {
            TryReadData(SERVER_NEXT_GROUP_ID, out int id);
            id++;
            StoreData(SERVER_NEXT_GROUP_ID,id);
            return id;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}