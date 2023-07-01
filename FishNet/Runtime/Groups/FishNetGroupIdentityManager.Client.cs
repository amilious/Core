using System;
using UnityEngine;
using Amilious.Core.Extensions;
using Amilious.Core.Identity.Group;
using FishNet.Object.Synchronizing;
using Amilious.Core.Identity.Group.Data;

namespace Amilious.Core.FishNet.Groups {
    
    public partial class FishNetGroupIdentityManager {
        
        #region FishNet Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        public override void OnStartClient() {
            base.OnStartClient();
            _groupLookup.OnChange += OnGroupChanged;
            _memberData.OnChange += OnMemberDataChanged;
            Server_ReceiveUniqueDataRequest();
        }

        private void OnMemberDataChanged(SyncDictionaryOperation op, ulong key, GroupMemberData value, bool asserver) {
            if(asserver) return;
            if(op == SyncDictionaryOperation.Complete) return;
            key.UnpackUInts(out var group, out var user);
            Debug.LogFormat("{0} operation for group {1} and user {2}!",op,group,user);
            Debug.Log($"Member Data for group {group}|{value.GroupId} and user {user}|{value.UserId} was updated");
        }

        private void OnGroupChanged(SyncDictionaryOperation op, uint key, GroupIdentity value, bool asserver) {
            if(asserver) return;
            Debug.Log($"Group Data for group {key}|{value.Id} was updated");
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool RequestCreate(string name, GroupType type, GroupAuthType authType = GroupAuthType.None, 
            string password = null) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestOwnerChange(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestApply(uint group, string request = null, string password = null) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestInvite(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestApproveApplication(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestRankChange(uint group, uint user, short rank) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestLeave(uint group) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestKick(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestDisband(uint group) {
            throw new NotImplementedException();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}