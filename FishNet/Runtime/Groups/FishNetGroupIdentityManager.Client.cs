using System;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Amilious.Core.Security;
using Amilious.Core.Collections;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using FishNet.Object.Synchronizing;
using Amilious.Core.Identity.Group.Data;
using Amilious.Core.Identity.Group.GroupEventArgs;

namespace Amilious.Core.FishNet.Groups {
    
    public partial class FishNetGroupIdentityManager {
       
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly Dictionary<uint, Queue<QueuedGroupEvent>> _notificationQueue =
            new Dictionary<uint, Queue<QueuedGroupEvent>>();
        
        private readonly DoubleDictionary<uint, uint, GroupMemberData> _memberLookup =
            new DoubleDictionary<uint, uint, GroupMemberData>();

        private readonly Dictionary<uint, uint> _ownerChanged = new Dictionary<uint, uint>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        private UserIdentity LocalUser => UserManager.Client_GetIdentity();
        
        #region FishNet Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        public override void OnStartClient() {
            base.OnStartClient();
            _groupLookup.OnChange += OnGroupChanged;
            Server_ReceiveUniqueDataRequest();
        }

        private void OnGroupChanged(SyncDictionaryOperation op, uint key, GroupIdentity value, bool isServer) {
            if(isServer || op == SyncDictionaryOperation.Complete) return;
            if(op == SyncDictionaryOperation.Remove) {
                _notificationQueue.Remove(value);
                _ownerChanged.Remove(value);
                OnDisband?.Invoke(key,false);
                return;
            }
            if(op == SyncDictionaryOperation.Set&&_ownerChanged.TryGetValue(key, out var ownerId)) {
                if(value.OwnerId != ownerId) return;
                if(!UserManager.TryGetIdentity(ownerId, out var ownerIdentity)) return;
                OnOwnerChanged?.Invoke(value,ownerIdentity,false);
                return;
            }
            if(op != SyncDictionaryOperation.Add) return;
            OnGroupCreated?.Invoke(value,false);
            Client_TriggerQueuedNotifications(value);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        // ReSharper disable once ParameterHidesMember
        public bool RequestCreate(string name, GroupType type, GroupAuthType authType = GroupAuthType.None, 
            string password = null) {
            string saltedPassword = null;
            string salt = null;
            if(authenticationMode != ClientServer.Server) {
                var args = new CreatingGroupEventArgs(name, UserManager.Client_GetIdentity(), type, authType, false);
                OnCreating?.Invoke(this, args);
                if(authType == GroupAuthType.Password) {
                    if(password == null) return LogFailure(GroupEvent.Create, PASSWORD_REQUIRED);
                }
                if(args.Canceled) return LogFailure(GroupEvent.Create, args.CancelReason);
            }
            if(password != null) {
                salt = PasswordTools.GetRandomString(10);
                saltedPassword = PasswordTools.HashPasswordSHA512(password, salt);
            }
            Server_ReceiveCreateRequest(name, type, authType, saltedPassword, salt);
            return true;
        }

        /// <inheritdoc />
        public bool RequestOwnerChange(uint group, uint user) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroupAndUser(GroupEvent.OwnerChange, group, user, out var groupIdentity,
                    out var userIdentity)) return false;
                //validate the group
                if(groupIdentity.OwnerId != LocalUser)
                    return LogFailure(GroupEvent.OwnerChange, OWNER_CHANGE_OWNER);
                var args = new ChangingOwnerGroupEventArgs(groupIdentity, LocalUser, userIdentity, false);
                OnChangingOwner?.Invoke(this, args);
                if(args.Canceled) return LogFailure(GroupEvent.OwnerChange, args.CancelReason);
            }
            Server_ReceiveOwnerChangeRequest(group,user); //send the request to the server.
            return true; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestApply(uint group, string request = null, string password = null) {
            if(!TryGetGroup(group, out var groupIdentity)) //get the group data.
                return LogFailure(GroupEvent.Apply, INVALID_GROUP_ID);
            if(authenticationMode != ClientServer.Server) {
                if(IsMember(group, LocalUser)) //make sure the user is not part of the group.
                    return LogFailure(GroupEvent.Apply, ALREADY_A_MEMBER);
                if(groupIdentity.AuthType == GroupAuthType.Password) { //make user password is valid
                    if(password == null) return LogFailure(GroupEvent.Apply, PASSWORD_REQUIRED);
                }else password = null; //no password is needed so don't waste data sending it to the server.
                var args = new JoiningGroupEventArgs(groupIdentity, LocalUser, false);
                OnJoining?.Invoke(this, args); //make sure that the application has not been canceled.
                if(args.Canceled) return LogFailure(GroupEvent.Apply, args.CancelReason);
            }
            if(password != null) {
                password = PasswordTools.HashPasswordSHA512(password, groupIdentity.Salt); //salt the password
                password = PasswordTools.ClientSecurePasswordHash(password, hashIterations); //secure the password
            }
            Server_ReceiveApplyRequest(group, request, password); //send the request to the server.
            return true; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestInvite(uint group, uint user) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroupAndUser(GroupEvent.Invite, group, user, out var groupIdentity, 
                    out var userIdentity)) return false;
                if(!groupIdentity.IsMember(LocalUser)) //check that the local user is a member of the group.
                    return LogFailure(GroupEvent.Invite, NOT_A_MEMBER);
                if(groupIdentity.IsMember(user)) //make user that the user is not a member of the group.
                    return LogFailure(GroupEvent.Invite, USER_ALREADY_MEMBER);
                var args = new InvitingGroupEventArgs(groupIdentity, LocalUser, userIdentity, false);
                OnInviting?.Invoke(this,args);
                if(args.Canceled) //make sure that the invite has not been canceled.
                    return LogFailure(GroupEvent.Invite, args.CancelReason);
            }
            Server_ReceiveInviteRequest(group, user); //send the request to the server
            return true; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestApproveApplication(uint group, uint user) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroupAndUser(GroupEvent.Approve, group, user, out var groupIdentity, 
                    out var userIdentity)) return false;
                if(!groupIdentity.IsMember(LocalUser)) //make sure that the local user is a member of the group.
                    return LogFailure(GroupEvent.Approve, NOT_A_MEMBER);
                if(groupIdentity.IsMember(user)) //make sure that the user is not a member of the group.
                    return LogFailure(GroupEvent.Approve, USER_ALREADY_MEMBER);
                if(groupIdentity.GetStatus(user) != MemberStatus.Applying) //make sure the user is applying to the group
                    return LogFailure(GroupEvent.Apply, USER_NOT_APPLYING);
                //check for cancellations
                var args = new ApprovingJoinGroupEventArgs(groupIdentity, LocalUser, userIdentity, false);
                OnApproving?.Invoke(this,args);
                if(args.Canceled) //make sure that that approval has not been canceled.
                    return LogFailure(GroupEvent.Approve, args.CancelReason);
            }
            Server_ReceiveApproveApplicationRequest(group, user); //send the request to the server
            return true; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestRankChange(uint group, uint user, short rank) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroupAndUser(GroupEvent.Rank, group, user, out var groupIdentity, 
                    out var userIdentity)) return false;
                var currentRank = groupIdentity.GetRank(user);
                var localRank = groupIdentity.GetRank(LocalUser);
                if(!groupIdentity.IsMember(LocalUser)) //check that the local user is a member of the group.
                    return LogFailure(GroupEvent.Rank, NOT_A_MEMBER);
                if(!groupIdentity.IsMember(user)) //check that the user is a member of the group.
                    return LogFailure(GroupEvent.Rank, USER_NOT_MEMBER);
                if(currentRank == rank) //check that the rank is not the current rank.
                    return LogFailure(GroupEvent.Rank, NO_CHANGE_IN_RANK);
                if(localRank <= currentRank) //make sure the user does not have a higher rank than your own.
                    return LogFailure(GroupEvent.Rank, INVALID_RANK_TO_RANK);
                if(localRank >= rank) //make sure that the rank you are trying to assign is less than your own.
                    return LogFailure(GroupEvent.Rank, UNAVAILABLE_RANK);
                var args = new RankingGroupEventArgs(groupIdentity, LocalUser, userIdentity, rank, false);
                OnRanking?.Invoke(this,args);
                if(args.Canceled) //make sure that the ranking was not canceled.
                    return LogFailure(GroupEvent.Rank, args.CancelReason);
            }
            Server_ReceiveRankChangeRequest(group, user, rank); //send the request to the server
            return true; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestLeave(uint group) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroup(group, out var groupIdentity)) //make sure that the group is valid.
                    return LogFailure(GroupEvent.Leave, INVALID_GROUP_ID);
                if(!groupIdentity.IsMember(LocalUser)) //make sure that the user is a member of the group.
                    return LogFailure(GroupEvent.Leave, NOT_A_MEMBER);
                if(groupIdentity.OwnerId == LocalUser) { //special situations for owners of groups
                    if(GetMemberCount(group) != 1) return LogFailure(GroupEvent.Leave, MUST_ASSIGN_NEW_OWNER);
                    //disband instead of leave
                    return RequestDisband(group); //return the result of the disband request.
                }
                var args = new LeavingGroupEventArgs(groupIdentity, LocalUser, false);
                OnLeaving?.Invoke(this,args);
                if(args.Canceled) //make sure that the leaving was not canceled.
                    return LogFailure(GroupEvent.Leave, args.CancelReason);
            }
            Server_ReceiveLeaveRequest(group); //send the request to the server
            return true; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestKick(uint group, uint user) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroupAndUser(GroupEvent.Kick, group, user, out var groupIdentity, 
                    out var userIdentity)) return false;
                if(!groupIdentity.IsMember(LocalUser)) //check that local user is a member of the group.
                    return LogFailure(GroupEvent.Kick, NOT_A_MEMBER);
                if(!groupIdentity.IsMember(user)) //check that the given user is a member of the group.
                    return LogFailure(GroupEvent.Kick, USER_NOT_MEMBER);
                if(groupIdentity.GetRank(user) >= groupIdentity.GetRank(LocalUser)) //make sure your rank is higher
                    return LogFailure(GroupEvent.Kick, UNABLE_TO_KICK_EQUAL_OR_HIGHER_RANK);
                var args = new KickingGroupEventArgs(groupIdentity, LocalUser, userIdentity, false);
                OnKicking?.Invoke(this,args);
                if(args.Canceled) // check to see if the kicking was canceled.
                    return LogFailure(GroupEvent.Kick, args.CancelReason);
            }
            Server_ReceiveKickRequest(group, user); //send the request to the server
            return false; //return true because the request was valid.
        }

        /// <inheritdoc />
        public bool RequestDisband(uint group) {
            if(authenticationMode != ClientServer.Server) {
                if(!TryGetGroup(group, out var groupIdentity)) //make sure that the group is valid.
                    return LogFailure(GroupEvent.Disband, INVALID_GROUP_ID);
                if(!groupIdentity.IsMember(LocalUser)) //make sure that the user is a member of the group.
                    return LogFailure(GroupEvent.Disband, NOT_A_MEMBER);
                if(groupIdentity.OwnerId != LocalUser) //make sure that the user is the owner of the group.
                    return LogFailure(GroupEvent.Disband, ONLY_OWNER_CAN_DISBAND);
                var args = new DisbandingGroupEventArgs(groupIdentity, LocalUser, false);
                OnDisbanding?.Invoke(this, args);
                if(args.Canceled) //make sure that the disbanding wasn't canceled.
                    return LogFailure(GroupEvent.Disband, args.CancelReason);
            }
            Server_DisbandRequest(group); //send the request to the server
            return false; //return true because the request was valid.
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Client Rpcs ////////////////////////////////////////////////////////////////////////////////////////////
        
        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveGroupMembers(NetworkConnection con, List<GroupMemberData> members) {
            foreach(var member in members) _memberLookup[member.GroupId, member.UserId] = member;
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveUserJoinedNotification(NetworkConnection con, GroupMemberData data) {
            if(Client_EnqueueIfNotReady(GroupEvent.Join, data)) return;
            _memberLookup[data.GroupId, data.UserId] = data; //update the data
            if(!TryGetGroupAndUser(data, out var group, out var user)) return;
            OnUserJoined?.Invoke(group,user,false);
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveUserApplication(NetworkConnection con, GroupMemberData data) {
            if(Client_EnqueueIfNotReady(GroupEvent.Apply, data)) return;
            _memberLookup[data.GroupId, data.UserId] = data; //update the data
            if(!TryGetGroupAndUser(data, out var group, out var user)) return;
            OnReceiveApplication?.Invoke(group,user,false);
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveInvite(NetworkConnection con, GroupMemberData data) {
            if(Client_EnqueueIfNotReady(GroupEvent.Invite, data, data.InvitedBy)) return;
            _memberLookup[data.GroupId, data.UserId] = data; //update the data
            if(!data.InvitedBy.HasValue) return;
            if(!TryGetGroupAndUser(data, out var group, out var user)) return;
            if(!UserManager.TryGetIdentity(data.InvitedBy.Value, out var inviter)) return;
            OnUserInvited?.Invoke(group,inviter,user,false);
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveRankChange(NetworkConnection con, GroupMemberData data, uint ranker) {
            if(Client_EnqueueIfNotReady(GroupEvent.Rank, data, ranker)) return;
            _memberLookup[data.GroupId, data.UserId] = data; //update the data
            if(UserManager.TryGetIdentity(ranker, out var rankerIdentity)) return;
            if(!TryGetGroupAndUser(data, out var group, out var user)) return;
            OnRankChanged?.Invoke(group,rankerIdentity,user,data.Rank,false);
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveUserLeftNotification(NetworkConnection con, GroupMemberData data) {
            if(Client_EnqueueIfNotReady(GroupEvent.Leave, data)) return;
            _memberLookup[data.GroupId, data.UserId] = data; //update the data
            if(TryGetGroupAndUser(data, out var group, out var user)) return;
            OnUserLeft?.Invoke(group,user,false);
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveKickedNotification(NetworkConnection con, GroupMemberData data, uint kicker) {
            if(Client_EnqueueIfNotReady(GroupEvent.Kick, data, kicker)) return;
            _memberLookup[data.GroupId, data.UserId] = data; //update the data
            if(TryGetGroupAndUser(data, out var group, out var user)) return;
            if(!UserManager.TryGetIdentity(kicker, out var kickerIdentity)) return;
            OnUserKicked?.Invoke(group,kickerIdentity,user,false);
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveOwnershipChange(NetworkConnection con, uint group, uint owner) {
            if(!TryGetGroup(group, out var groupIdentity)) return;
            if(groupIdentity.OwnerId == owner) {
                if(!UserManager.TryGetIdentity(owner, out var ownerIdentity)) return;
                OnOwnerChanged?.Invoke(groupIdentity,ownerIdentity,false);
                return;
            }
            _ownerChanged[group] = owner;
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void Client_ReceiveFailure(NetworkConnection con, GroupEvent type, string failReason) 
            => LogFailure(type, failReason);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Helper Methods /////////////////////////////////////////////////////////////////////////////////

        private void Client_TriggerQueuedNotifications(GroupIdentity group) {
            if(!_notificationQueue.ContainsKey(group)) return;
            while(_notificationQueue[group].TryDequeue(out var notification)) {
                var data = notification.Data;
                _memberLookup[data.GroupId, data.UserId] = data; //update the data
                if(!UserManager.TryGetIdentity(data.UserId, out var user)) return;
                switch(notification.Type) {
                    case GroupEvent.Join: OnUserJoined?.Invoke(group,user,false); break;
                    case GroupEvent.Leave: OnUserLeft?.Invoke(group,user,false); break;
                    case GroupEvent.Apply: OnReceiveApplication?.Invoke(group,user,false); break;
                    case GroupEvent.Invite:
                        if(!notification.ExtraId.HasValue) return;
                        if(!UserManager.TryGetIdentity(notification.ExtraId.Value, out var inviter)) return;
                        OnUserInvited?.Invoke(group,inviter,user,false); break;
                    case GroupEvent.Rank:
                        if(!notification.ExtraId.HasValue) return;
                        if(!UserManager.TryGetIdentity(notification.ExtraId.Value, out var ranker)) return;
                        OnRankChanged?.Invoke(group,ranker,user,data.Rank,false); break;
                    case GroupEvent.Kick:
                        if(!notification.ExtraId.HasValue) return;
                        if(!UserManager.TryGetIdentity(notification.ExtraId.Value, out var kicker)) return;
                        OnUserKicked?.Invoke(group,kicker,user,false); break;
                    case GroupEvent.Create: return;
                    case GroupEvent.Approve: return;
                    case GroupEvent.OwnerChange: return;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private bool Client_EnqueueIfNotReady(GroupEvent type, GroupMemberData memberData, uint? extraId = null) {
            if(_groupLookup.ContainsKey(memberData.GroupId)) return false;
            if(!_notificationQueue.ContainsKey(memberData.GroupId))
                _notificationQueue[memberData.GroupId] = new Queue<QueuedGroupEvent>();
            _notificationQueue[memberData.GroupId].Enqueue(new QueuedGroupEvent(type,memberData,extraId));
            return true;
        }

        private bool TryGetGroupAndUser(GroupMemberData memberData, out GroupIdentity group, out UserIdentity user) {
            var result = TryGetGroup(memberData.GroupId, out group);
            result = UserManager.TryGetIdentity(memberData.UserId, out user) && result;
            return result;
        }

        private bool TryGetGroupAndUser(GroupEvent type, uint groupId, uint userId, out GroupIdentity group,
            out UserIdentity user) {
            group = GroupIdentity.Default;
            user = UserIdentity.DefaultUser;
            if(!TryGetGroup(groupId, out group)) {
                LogFailure(type, INVALID_GROUP_ID);
                return false;
            }
            if(UserManager.TryGetIdentity(userId, out user)) {
                LogFailure(type, INVALID_USER_ID);
                return false;
            }
            return true;
        }

        private bool LogFailure(GroupEvent type, string failReason) {
            Debug.LogFormat("[GroupIdentityManager] {0} Failure! {1}",type,failReason);
            return false;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}