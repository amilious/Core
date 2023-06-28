
using System;
using UnityEngine;
using FishNet.Object;
using System.Collections.Generic;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group.GroupEventArgs;

namespace Amilious.Core.FishNet.Groups {
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(short.MinValue+99)]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet Group Identity Manager")]
    public partial class FishNetGroupIdentityManager : NetworkBehaviour, IGroupIdentityManager {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private IUserIdentityManager _userManager;
        private AbstractGroupDataManager _dataManager;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public GroupIdentity this[int userId] => throw new NotImplementedException();

        /// <inheritdoc />
        public IEnumerable<GroupIdentity> Identities { get; }

        /// <inheritdoc />
        public AbstractGroupDataManager DataManager {
            get {
                if(_dataManager is not null) return _dataManager;
                _dataManager = this.GetGroupDataManager();
                return _dataManager;
            }
        }

        /// <inheritdoc />
        public IUserIdentityManager UserManager {
            get {
                if(_userManager != null) return _userManager;
                _userManager = this.GetUserManager();
                return _userManager;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public event EventHandler<CreatingGroupEventArgs> OnCreating;
        
        /// <inheritdoc />
        public event EventHandler<ChangingOwnerGroupEventArgs> OnChangingOwner;
        
        /// <inheritdoc />
        public event EventHandler<JoiningGroupEventArgs> OnJoining;
        
        /// <inheritdoc />
        public event EventHandler<InvitingGroupEventArgs> OnInviting;
        
        /// <inheritdoc />
        public event EventHandler<ApprovingJoinGroupEventArgs> OnApproving;
        
        /// <inheritdoc />
        public event EventHandler<RankingGroupEventArgs> OnRanking;
        
        /// <inheritdoc />
        public event EventHandler<LeavingGroupEventArgs> OnLeaving;
        
        /// <inheritdoc />
        public event EventHandler<KickingGroupEventArgs> OnKicking;
        
        /// <inheritdoc />
        public event EventHandler<DisbandingGroupEventArgs> OnDisbanding;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server And Client Methods //////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool IsMember(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsInvited(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool HasApplied(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public short GetRank(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public MemberStatus GetStatus(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryGetMembers(int group, out UserIdentity[] members) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryGetGroup(int groupId, out GroupIdentity groupIdentity) {
            throw new NotImplementedException();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}