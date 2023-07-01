
using System;
using UnityEngine;
using FishNet.Object;
using System.Collections.Generic;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.User;
using FishNet.Object.Synchronizing;
using Amilious.Core.Identity.Group.Data;
using Amilious.Core.Identity.Group.GroupEventArgs;
using FishNet.Transporting;

namespace Amilious.Core.FishNet.Groups {
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(short.MinValue+99)]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet Group Identity Manager")]
    public partial class FishNetGroupIdentityManager : NetworkBehaviour, IGroupIdentityManager {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private IUserIdentityManager _userManager;
        private IGroupDataManager _dataManager;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Sync Variables /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This dictionary is used to store all of the user's identities.
        /// </summary>
        [SyncObject] 
        private readonly SyncDictionary<uint, GroupIdentity> _groupLookup = new SyncDictionary<uint, GroupIdentity>();

        /// <summary>
        /// This hash set is used to store all of the online user ids.
        /// </summary>
        [SyncObject]
        private readonly SyncDictionary<ulong, GroupMemberData> _memberData =
            new SyncDictionary<ulong, GroupMemberData>();


        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public GroupIdentity this[uint userId] => throw new NotImplementedException();

        /// <inheritdoc />
        public IEnumerable<GroupIdentity> Identities { get; }

        /// <inheritdoc />
        public IGroupDataManager DataManager {
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
        
        #region FishNet Methods ////////////////////////////////////////////////////////////////////////////////////////

        public override void OnStartNetwork() {
            base.OnStartNetwork();
            NetworkManager.RegisterInstance(this);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server And Client Methods //////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool IsMember(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsInvited(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool HasApplied(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public short GetRank(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public MemberStatus GetStatus(uint group, uint user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryGetMembers(uint group, out UserIdentity[] members) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryGetGroup(uint groupId, out GroupIdentity groupIdentity) {
            throw new NotImplementedException();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
       
    }
}