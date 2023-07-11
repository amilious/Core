
using System;
using UnityEngine;
using FishNet.Object;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using FishNet.Object.Synchronizing;
using Amilious.Core.Identity.Group.Data;
using Amilious.Core.Identity.Group.GroupEventArgs;

namespace Amilious.Core.FishNet.Groups {
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(short.MinValue+99)]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet Group Identity Manager")]
    public partial class FishNetGroupIdentityManager : NetworkBehaviour, IGroupIdentityManager {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        private const string PASSWORD_REQUIRED = "A password was required but was not provided!";
        private const string OWNER_CHANGE_OWNER = "You must be the owner of a group to change the group's owner!";
        private const string INVALID_GROUP_ID = "Invalid group id provided.";
        private const string INVALID_USER_ID = "Invalid user id provided.";
        private const string ALREADY_A_MEMBER = "You are already a member of that group!";
        private const string USER_ALREADY_MEMBER = "That user is already a member of the group!";
        private const string NOT_A_MEMBER = "You are not a member of that group!";
        private const string USER_NOT_APPLYING = "That user is not currently applying to the group!";
        private const string USER_NOT_MEMBER = "That user is not a member of the group!";
        private const string NO_CHANGE_IN_RANK = "The user already has that rank!";
        private const string INVALID_RANK_TO_RANK = "You must have a higher rank that a user to change their rank!";
        private const string UNAVAILABLE_RANK = "You cannot assign a rank that is equal or higher than your own!";
        private const string MUST_ASSIGN_NEW_OWNER = "You cannot leave the group unless you asign a new owner!";
        private const string UNABLE_TO_KICK_EQUAL_OR_HIGHER_RANK =
            "Yoy cannot kick someone from the group who has an equal or greater than yourself.";
        private const string ONLY_OWNER_CAN_DISBAND = "Only the owner of a group can disband it.";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [Header("General Settings")][SerializeField] 
        private ClientServer authenticationMode = ClientServer.Both;
        
        [Header("Group Password Settings")]
        [SerializeField] private int hashIterations = 10;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
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
        public event IGroupIdentityManager.DisbandDelegate OnDisband;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupCreatedDelegate OnGroupCreated;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupUserDelegate OnReceiveApplication;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupInviteDelegate OnUserInvited;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupRankChangeDelegate OnRankChanged;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupUserDelegate OnUserLeft;
        /// <inheritdoc />
        public event IGroupIdentityManager.UserKickedDelegate OnUserKicked;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupUserDelegate OnOwnerChanged;
        /// <inheritdoc />
        public event IGroupIdentityManager.GroupUserDelegate OnUserJoined;
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
            if(IsServer) return DataManager.IsMember(group, user);
            return _memberLookup.TryGetValue(group, user, out var data) && data.Status == MemberStatus.Member;
        }

        /// <inheritdoc />
        public bool IsInvited(uint group, uint user) {
            if(!TryGetMemberData(group, user, out var data)) return false;
            return data is { Status: MemberStatus.Invited };
        }

        /// <inheritdoc />
        public bool HasApplied(uint group, uint user) {
            if(!TryGetMemberData(group, user, out var data)) return false;
            return data is { Status: MemberStatus.Applying };
        }

        /// <inheritdoc />
        public int GetMemberCount(uint group) {
            if(ServerManager) return DataManager.TryGetGroupMemberCount(group, out var count) ? count : 0;
            return _memberLookup.SubCount(group);
        }

        /// <inheritdoc />
        public short GetRank(uint group, uint user) {
            return !TryGetMemberData(group, user, out var data) ? (short)0 : data.Rank;
        }

        /// <inheritdoc />
        public MemberStatus GetStatus(uint group, uint user) {
            return !TryGetMemberData(group, user, out var data) ? MemberStatus.None : data.Status;
        }

        /// <inheritdoc />
        public IEnumerable<UserIdentity> TryGetMembers(uint group) {
            IEnumerable<uint> memberIds;
            if(IsServer) { if(!DataManager.TryGetGroupMembers(group, out memberIds)) yield break; }
            else memberIds = _memberLookup.SubKeys(group);
            foreach(var memberId in memberIds) {
                if(UserManager.TryGetIdentity(memberId, out var identity)) yield return identity;
            }
        }

        /// <inheritdoc />
        public IEnumerable<GroupMemberData> TryGetAllMemberData(uint group) {
            IEnumerable<uint> memberIds;
            if(IsServer) { if(!DataManager.TryGetGroupMembers(group, out memberIds)) yield break; }
            else memberIds = _memberLookup.SubKeys(group);
            foreach(var memberId in memberIds) {
                if(TryGetMemberData(group, memberId, out var data)) yield return data;
            }
        }

        /// <inheritdoc />
        public bool TryGetGroup(uint groupId, out GroupIdentity groupIdentity) {
            return _groupLookup.TryGetValueFix(groupId, out groupIdentity);
        }
        
        /// <inheritdoc />
        public bool TryGetMemberData(uint group, uint user, out GroupMemberData data) {
            return IsServer ? DataManager.TryGetGroupMemberData(group, user, out data) : 
                _memberLookup.TryGetValue(group, user, out data);
        }

        /// <inheritdoc />
        public bool TryGetMemberData(uint group, uint user, out GroupIdentity groupIdentity, 
            out UserIdentity userIdentity, out GroupMemberData data) {
            var result = TryGetMemberData(group, user, out data);
            if(result) {
                if(!TryGetGroup(group, out groupIdentity)) result = false;
                if(!UserManager.TryGetIdentity(user, out userIdentity)) result = false;
                return result;
            }
            userIdentity = UserIdentity.DefaultUser;
            groupIdentity = GroupIdentity.Default;
            return false;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}