using System;
using UnityEngine;
using Amilious.Core.IO;
using System.Collections.Generic;

// ReSharper disable ParameterHidesMember

namespace Amilious.Core.Identity.Group.Data {
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(short.MinValue)]
    public abstract class DefaultGroupDataManager : AmiliousBehavior, IGroupDataManager {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        protected const string HELP_BOX_TEXT =
            "This singleton data manager is used for saving and loading group information.";

        private const bool SAVE_EVERY_CHANGE = true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private static IGroupDataManager RawInstance;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public event IGroupDataManager.GroupMemberDelegate OnGroupMemberDataRemoved;
        /// <inheritdoc />
        public event IGroupDataManager.GroupDelegate OnGroupDataRemoved;
        /// <inheritdoc />
        public event IGroupDataManager.GroupMemberDelegate OnGroupMemberDataModified;
        /// <inheritdoc />
        public event IGroupDataManager.GroupDelegate OnGroupDataModified;
        /// <inheritdoc />
        public event Action<GroupData> OnGroupDataAdded;
        /// <inheritdoc />
        public event Action<GroupMemberData> OnGroupMemberDataAdded;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Data Callback Methdods /////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public void OnMemberDataUpdated(GroupMemberData data) {
            IdentitySave.MarkDataChanged(SAVE_EVERY_CHANGE);
            OnGroupMemberDataModified?.Invoke(data.GroupId,data.UserId);
        }

        /// <inheritdoc />
        public void OnGroupDataUpdated(GroupData data) {
            IdentitySave.MarkDataChanged(SAVE_EVERY_CHANGE);
            OnGroupDataModified?.Invoke(data.Id);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Change Data ////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public bool RemoveUserFromGroup(uint group, uint user) {
            if(IdentitySave.Server_TryGetGroupMemberData(group,user, out var memberData))
                memberData.RegisterDataManager(null);
            var result = IdentitySave.Server_RemoveUserFromGroup(group, user, SAVE_EVERY_CHANGE);
            if(result) OnGroupMemberDataRemoved?.Invoke(group, user);
            return result;
        }

        /// <inheritdoc />
        public bool RemoveGroup(uint group) {
            if(IdentitySave.Server_TryGetGroupData(group, out var groupData)) groupData.RegisterDataManager(null);
            var result = IdentitySave.Server_RemoveGroup(group, SAVE_EVERY_CHANGE);
            OnGroupDataRemoved?.Invoke(group);
            return result;
        }

        /// <inheritdoc />
        public GroupData AddGroup(string name, GroupType groupType, uint creator, out GroupMemberData creatorData,
            GroupAuthType authType = GroupAuthType.None, string password = null, string salt = null) {
            var group = IdentitySave.Server_AddGroup(name, groupType, creator, out creatorData, authType, password, salt, 
                SAVE_EVERY_CHANGE);
            group.RegisterDataManager(this);
            OnGroupDataAdded?.Invoke(group);
            OnGroupMemberDataAdded?.Invoke(creatorData);
            return group;
        }

        /// <inheritdoc />
        public GroupMemberData AddGroupMemberData(uint group, uint user, MemberStatus status, short rank = 0,
            uint? invitedBy = null, uint? approvedBy = null, string applicationRequest = null) {
            var member = IdentitySave.Server_AddGroupMemberData(group, user, status, rank, invitedBy, approvedBy,
                applicationRequest,SAVE_EVERY_CHANGE);
            member.RegisterDataManager(this);
            OnGroupMemberDataAdded?.Invoke(member);
            return member;
        }

        /// <inheritdoc />
        public IEnumerable<uint> GetUsersGroups(uint user) => IdentitySave.Server_TryGetUsersGroups(user);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Get Data ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public bool IsMember(uint group, uint user) => IdentitySave.Server_IsMember(group, user);
        
        /// <inheritdoc />
        public IEnumerable<uint> GetGroupIds() => IdentitySave.Server_GetGroupIds();

        /// <inheritdoc />
        public bool TryGetGroupMembers(uint group, out IEnumerable<uint> members) =>
            IdentitySave.Server_TryGetGroupMembers(group, out members);

        /// <inheritdoc />
        public bool TryGetGroupData(uint group, out GroupData groupData) {
            var result = IdentitySave.Server_TryGetGroupData(group, out groupData);
            if(result) groupData.RegisterDataManager(this);
            return result;
        }

        /// <inheritdoc />
        public bool TryGetGroupMemberData(uint group, uint user, out GroupMemberData groupMemberData) {
            var result = IdentitySave.Server_TryGetGroupMemberData(group, user, out groupMemberData);
            if(result) groupMemberData.RegisterDataManager(this);
            return result;
        }

        /// <inheritdoc />
        public bool TryGetGroupMemberCount(uint group, out int count) =>
            IdentitySave.Server_TryGetGroupMemberCount(group, out count);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}