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

using UnityEngine;
using Amilious.Core.Saving;
using Amilious.Core.Attributes;
using System.Collections.Generic;

namespace Amilious.Core.Indentity.Group {

    /// <summary>
    /// This class is used to access, store, and edit information.
    /// </summary>
    [AmiliousHelpBox(HELP_BOX_TEXT,HelpBoxType.Info)]
    public class GroupIdentityDataManager : AmiliousBehavior {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string HELP_BOX_TEXT =
            "This singleton data manager is used for saving and loading group information.";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private static GroupIdentityDataManager _instance;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the singleton instance of the class.
        /// </summary>
        public static GroupIdentityDataManager Instance {
            get {
                if(_instance != null) return _instance;
                _instance = FindObjectOfType<GroupIdentityDataManager>();
                _instance ??= new GameObject("Group Identity Data Manager").AddComponent<GroupIdentityDataManager>();
                return _instance;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the script is being loaded.
        /// </summary>
        private void Awake() {
            //make singleton
            if(_instance != null && _instance != this) {
                Destroy(this);
                return;
            }
            _instance = this;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Server Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get data for the group with the given id.
        /// </summary>
        /// <param name="id">The group's id.</param>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value for the given key.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>True if able to read the data, otherwise false.</returns>
        public virtual bool Server_TryReadGroupData<T>(int id, string key, out T value) {
            return IdentitySave.Server_TryReadGroupData(id, key, out value);
        }

        /// <summary>
        /// This method is used to store data for the group with the given id.
        /// </summary>
        /// <param name="id">The group's id.</param>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value for the given key.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        public virtual void Server_StoreGroupData<T>(int id, string key, T value) {
            IdentitySave.Server_StoreGroupData(id,key,value);
        }

        /// <summary>
        /// This method is used to add a user to a group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to add a user to.</param>
        /// <param name="userId">The id of the user that you want to add to the channel.</param>
        /// <returns>True if the user was added to the group, otherwise false if the user
        /// was already in the group.</returns>
        public virtual bool Server_AddUserToGroup(int groupId, int userId) {
            return IdentitySave.Server_AddUserToGroup(groupId,userId);
        }

        /// <summary>
        /// This method is used to remove a user from a group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to remove a user from.</param>
        /// <param name="userId">The id of the user that you want to remove from the group.</param>
        /// <returns>True if the user was removed from the group, otherwise false if the user was not
        /// part of the group.</returns>
        public virtual bool Server_RemoveUserFromGroup(int groupId, int userId) {
            return IdentitySave.Server_RemoveUserFromGroup(groupId,userId);
        }

        /// <summary>
        /// This method is used to get all of the group ids.
        /// </summary>
        /// <returns>The group ids.</returns>
        public virtual IEnumerable<int> Server_GetGroupIds() {
            return IdentitySave.Server_GetGroupIds();
        }

        /// <summary>
        /// This method is used to try get the groups for a given user.
        /// </summary>
        /// <param name="userId">The id of the user that you want to get the groups for.</param>
        /// <param name="groups">The groups that the user is a member of.</param>
        /// <returns>True if the user is a member of a group, otherwise false.</returns>
        public virtual bool Server_TryGetGroupChannels(int userId, out IEnumerable<int> groups) {
            return IdentitySave.Server_TryGetUsersGroups(userId, out groups);
        }

        /// <summary>
        /// This method is used to get the members of the given group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to get the members of.</param>
        /// <param name="members">The members of the group with the given id.</param>
        /// <returns>True if the group contains members, otherwise false.</returns>
        public virtual bool Server_TryGetGroupMembers(int groupId, out IEnumerable<int> members) {
            return IdentitySave.Server_TryGetGroupMembers(groupId, out members);
        }

        /// <summary>
        /// This method is used to get the number of members for a given group.
        /// </summary>
        /// <param name="groupId">The id of the channel.</param>
        /// <param name="count">The number of members in the group.</param>
        /// <returns>True if the group exists, otherwise false.</returns>
        public virtual bool Server_TryGetGroupMemberCount(int groupId, out int count) {
            return IdentitySave.Server_TryGetGroupMemberCount(groupId, out count);
        }

        /// <summary>
        /// This method is used to remove a groups.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to remove.</param>
        /// <returns>True if the group was removed, otherwise false if the group did not exist.</returns>
        public virtual bool Server_RemoveGroup(int groupId) {
            return IdentitySave.Server_RemoveGroup(groupId);
        }

        /// <summary>
        /// This method is used to add a new group to the server.
        /// </summary>
        /// <param name="channelName">The name for the group.</param>
        /// <param name="groupType">The type of group that you want to create.</param>
        /// <returns>The id for the new group.</returns>
        public virtual int Server_AddGroup(string channelName = null, GroupType groupType = GroupType.Chat) {
            return IdentitySave.Server_AddGroup(channelName, groupType);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}