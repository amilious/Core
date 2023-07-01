using System;
using Amilious.Core.Extensions;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.Group.Data;

namespace Amilious.Core.FishNet.Groups {
    
    public partial class FishNetGroupIdentityManager {
        
        #region FishNet Methods ////////////////////////////////////////////////////////////////////////////////////////

        public override void OnStartServer() {
            base.OnStartServer();
            //add data listeners
            DataManager.OnGroupDataAdded += Data_OnGroupAdded;
            DataManager.OnGroupDataRemoved += Data_OnGroupRemoved;
            DataManager.OnGroupDataModified += Data_OnGroupModified;
            DataManager.OnGroupMemberDataAdded += Data_OnGroupMemberAdded;
            DataManager.OnGroupMemberDataRemoved += Data_OnGroupMemberRemoved;
            DataManager.OnGroupMemberDataModified += Data_OnGroupMemberModified;
            //load the groups this will build the sync dictionary even though it does not look like it.
            foreach(var id in DataManager.GetGroupIds()) {
                DataManager.TryGetGroupData(id, out _);
                if(!DataManager.TryGetGroupMembers(id, out var members))continue;
                foreach(var member in members) {
                    DataManager.TryGetGroupMemberData(id, member, out _);
                }
            }
        }

        public override void OnStopServer() {
            base.OnStopServer();
            //remove data listeners
            DataManager.OnGroupDataAdded -= Data_OnGroupAdded;
            DataManager.OnGroupDataRemoved -= Data_OnGroupRemoved;
            DataManager.OnGroupDataModified -= Data_OnGroupModified;
            DataManager.OnGroupMemberDataAdded -= Data_OnGroupMemberAdded;
            DataManager.OnGroupMemberDataRemoved -= Data_OnGroupMemberRemoved;
            DataManager.OnGroupMemberDataModified -= Data_OnGroupMemberModified;
            //clear all the saved data
            _memberData.Clear();
            _groupLookup.Clear();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Data Event Methods /////////////////////////////////////////////////////////////////////////////////////
        
        private void Data_OnGroupAdded(GroupData obj) => _groupLookup.Add(obj.Id, obj.GetGroupIdentity(this));

        /// <inheritdoc cref="IGroupDataManager.GroupDelegate"/>
        private void Data_OnGroupRemoved(uint group) => _groupLookup.Remove(group);

        /// <inheritdoc cref="IGroupDataManager.GroupDelegate"/>
        private void Data_OnGroupModified(uint group) => _groupLookup.Dirty(group);
        
        private void Data_OnGroupMemberAdded(GroupMemberData obj) => _memberData.Add(obj.GroupId,obj.UserId,obj);

        /// <inheritdoc cref="IGroupDataManager.GroupMemberDelegate"/>
        private void Data_OnGroupMemberRemoved(uint group, uint user) => _memberData.Remove(group, user);

        /// <inheritdoc cref="IGroupDataManager.GroupMemberDelegate"/>
        private void Data_OnGroupMemberModified(uint group, uint user) => _memberData.Dirty(group.PackWith(user));
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        public bool Create(string name, GroupType type, UserIdentity owner, out GroupIdentity group, 
            GroupAuthType authType = GroupAuthType.None, string password = null) {
            throw new NotImplementedException();
        }

        public bool ChangeOwner(GroupIdentity group, UserIdentity owner) {
            throw new NotImplementedException();
        }

        public bool AddMember(GroupIdentity group, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Apply(GroupIdentity group, UserIdentity user, string request = null, string password = null) {
            throw new NotImplementedException();
        }

        public bool ApproveApplication(GroupIdentity group, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Invite(GroupIdentity group, UserIdentity inviter, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool RankChange(GroupIdentity group, UserIdentity user, short rank) {
            throw new NotImplementedException();
        }

        public bool Remove(GroupIdentity group, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Kick(GroupIdentity group, UserIdentity kicker, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Disband(GroupIdentity group) {
            throw new NotImplementedException();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveUniqueDataRequest(NetworkConnection con = null) {
            Debug.Log("The client is requesting the user's unique data!");
        }
        
    }
}