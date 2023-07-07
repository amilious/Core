using System;
using FishNet.Object;
using FishNet.Connection;
using Amilious.Core.Security;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.Group.Data;
using Amilious.Core.Identity.Group.GroupEventArgs;

namespace Amilious.Core.FishNet.Groups {
    
    //TODO: retrieve the users and groups as part of the authentication that way no issues will arise from clients missing information.
    
    public partial class FishNetGroupIdentityManager {
        
        #region FishNet Methods ////////////////////////////////////////////////////////////////////////////////////////

        public override void OnStartServer() {
            base.OnStartServer();
            //add data listeners
            DataManager.OnGroupDataAdded += Data_OnGroupAdded;
            DataManager.OnGroupDataRemoved += Data_OnGroupRemoved;
            DataManager.OnGroupDataModified += Data_OnGroupModified;
            //load the groups this will build the sync dictionary even though it does not look like it.
            foreach(var id in DataManager.GetGroupIds()) {
                DataManager.TryGetGroupData(id, out var group);
                if(!DataManager.TryGetGroupMembers(id, out var members))continue;
                foreach(var member in members) {
                    DataManager.TryGetGroupMemberData(id, member, out _);
                }
            }
            //TODO: in the future only the member info should be synchronized for the groups that the member belongs to. 
        }

        public override void OnStopServer() {
            base.OnStopServer();
            //remove data listeners
            DataManager.OnGroupDataAdded -= Data_OnGroupAdded;
            DataManager.OnGroupDataRemoved -= Data_OnGroupRemoved;
            DataManager.OnGroupDataModified -= Data_OnGroupModified;
            //clear all the saved data
            _groupLookup.Clear();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Data Event Methods /////////////////////////////////////////////////////////////////////////////////////
        
        private void Data_OnGroupAdded(GroupData obj) => _groupLookup.Add(obj.Id, obj.GetGroupIdentity(this));

        /// <inheritdoc cref="IGroupDataManager.GroupDelegate"/>
        private void Data_OnGroupRemoved(uint group) => _groupLookup.Remove(group);

        /// <inheritdoc cref="IGroupDataManager.GroupDelegate"/>
        private void Data_OnGroupModified(uint group) => _groupLookup.Dirty(group);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get all of the current connections for the given group.
        /// </summary>
        /// <param name="group">The group in which you want to get the connections for.</param>
        /// <returns>The active connections for the given group.</returns>
        public IEnumerable<NetworkConnection> GetGroupConnections(uint group) {
            if(!DataManager.TryGetGroupMembers(group, out var members)) yield break;
            foreach(var userId in members) {
                if(!UserManager.TryGetIdentity(userId, out var userIdentity)) continue;
                if(!userIdentity.IsOnline) continue;
                if(!userIdentity.TryGetConnection(NetworkManager, out var connection)) continue;
                yield return connection;
            }
        }
        
        /// <inheritdoc />
        public bool Create(string name, GroupType type, UserIdentity owner, out GroupIdentity group, 
            GroupAuthType authType = GroupAuthType.None, string saltedPassword = null, string salt=null) {
            if(authType == GroupAuthType.Password && saltedPassword == null) {
                group = GroupIdentity.Default;
                return false;
            }
            //generate a salt for the string
            if(authType != GroupAuthType.Password) {
                saltedPassword = null;
                salt = null;
            }
            group = DataManager.AddGroup(name,type,owner,out var creator,authType,saltedPassword,salt)
                .GetGroupIdentity(this);
            OnGroupCreated?.Invoke(group,true);
            if(owner.IsOnline&&owner.TryGetConnection(NetworkManager,out var con)) 
                Client_ReceiveUserJoinedNotification(con, creator);
            return true;
        }

        /// <inheritdoc />
        public bool ChangeOwner(GroupIdentity group, UserIdentity owner) {
            if(!group.IsMember(owner)) return false;
            if(!DataManager.TryGetGroupData(group, out var groupData)) return false;
            groupData.Owner = owner;
            OnOwnerChanged?.Invoke(group, owner,false);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveOwnershipChange(con,group,owner);
            return true;
        }

        /// <inheritdoc />
        public bool AddMember(GroupIdentity group, UserIdentity user) {
            if(DataManager.TryGetGroupMemberData(group.Id,user.Id, out var data) && data.Status == MemberStatus.Member) 
                return false; //user is already a member of the group.
            if(!_groupLookup.ContainsKey(group)) return false;  //invalid group
            var memberData = DataManager.AddGroupMemberData(group, user, MemberStatus.Member);
            OnUserJoined?.Invoke(group,user,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveUserJoinedNotification(con, memberData);
            return true;
        }

        /// <inheritdoc />
        public bool Apply(GroupIdentity group, UserIdentity user, string request = null, string password = null) {
            if(!DataManager.TryGetGroupData(group, out var groupData)) return false;
            var hasMemberData = DataManager.TryGetGroupMemberData(group, user, out var memberData);
            var passedAuthentication = false;
            switch(group.AuthType) {
                case GroupAuthType.None: passedAuthentication = memberData.Status!=MemberStatus.Member; break;
                case GroupAuthType.Password:
                    passedAuthentication = PasswordTools.ServerValidateClientSecurePasswordHash(
                        groupData.Password, password, hashIterations); break;
                case GroupAuthType.InviteOnly:
                    passedAuthentication = memberData.InvitedBy.HasValue && 
                        IsMember(group, memberData.InvitedBy.Value); break;
                case GroupAuthType.ApprovalOrInvite:
                    if(memberData.Status == MemberStatus.Applying) return false;
                    if(hasMemberData) memberData.ApplicationRequest = request;
                    else memberData = DataManager.AddGroupMemberData(group, user, MemberStatus.Applying);
                    OnReceiveApplication(group, user, true);
                    foreach(var con in GetGroupConnections(group))
                        Client_ReceiveUserApplication(con, memberData);
                    return true;
                default: throw new ArgumentOutOfRangeException();
            }
            if(!passedAuthentication) return false;
            if(hasMemberData) memberData.Status = MemberStatus.Member;
            else memberData = DataManager.AddGroupMemberData(group, user, MemberStatus.Member);
            OnUserJoined?.Invoke(group,user,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveUserJoinedNotification(con,memberData);
            return true;
        }

        /// <inheritdoc />
        public bool ApproveApplication(GroupIdentity group, UserIdentity user, UserIdentity approver) {
            if(group == GroupIdentity.Default || user == UserIdentity.DefaultUser) return false;
            if(!DataManager.TryGetGroupData(group, out var groupData)) return false;
            if(groupData.AuthType != GroupAuthType.ApprovalOrInvite) return false;
            if(!IsMember(group, approver)) return false;
            if(!DataManager.TryGetGroupMemberData(group, user, out var memberData))
                memberData = DataManager.AddGroupMemberData(group, user, MemberStatus.None);
            if(memberData.Status != MemberStatus.Applying) return false;
            memberData.ApprovedBy = approver;
            OnUserJoined?.Invoke(group,user,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveUserJoinedNotification(con,memberData);
            return false;
        }

        /// <inheritdoc />
        public bool Invite(GroupIdentity group, UserIdentity inviter, UserIdentity user) {
            if(group == GroupIdentity.Default || user == UserIdentity.DefaultUser) return false;
            if(!DataManager.TryGetGroupData(group, out var groupData)) return false;
            if(groupData.AuthType is not (GroupAuthType.ApprovalOrInvite or GroupAuthType.InviteOnly)) return false;
            if(!DataManager.IsMember(group, inviter)) return false;
            var hasMemberData = DataManager.TryGetGroupMemberData(group, user, out var memberData);
            if(hasMemberData) memberData.InvitedBy = inviter;
            else memberData = DataManager.AddGroupMemberData(group, user, MemberStatus.Invited, invitedBy: inviter);
            OnUserInvited?.Invoke(group,inviter, user,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveInvite(con,memberData);
            return true;
        }

        /// <inheritdoc />
        public bool RankChange(GroupIdentity group, UserIdentity ranker, UserIdentity user, short rank) {
            if(group == GroupIdentity.Default || user == UserIdentity.DefaultUser) return false;
            if(DataManager.TryGetGroupMemberData(group, ranker, out var rankerData)) return false;
            if(rankerData.Status!=MemberStatus.Member) return false;
            if(!DataManager.TryGetGroupMemberData(group, user, out var userData)) return false;
            if(userData.Status != MemberStatus.Member) return false;
            userData.Rank = rank;
            OnRankChanged?.Invoke(group,ranker,user,rank,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveRankChange(con,userData,ranker);
            return true;
        }

        /// <inheritdoc />
        public bool Remove(GroupIdentity group, UserIdentity user) {
            if(group == GroupIdentity.Default || user == UserIdentity.DefaultUser) return false;
            if(!TryGetMemberData(group,user, out var data)) return false;
            if(data.Status is MemberStatus.Left or MemberStatus.None or MemberStatus.Kicked) return false;
            data.Status = MemberStatus.Left;
            OnUserLeft?.Invoke(group,user,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveUserLeftNotification(con,data);
            return true;
        }

        /// <inheritdoc />
        public bool Kick(GroupIdentity group, UserIdentity kicker, UserIdentity user) {
            if(group == GroupIdentity.Default || user == UserIdentity.DefaultUser) return false;
            if(DataManager.TryGetGroupMemberData(group, kicker, out var kickerData)) return false;
            if(kickerData.Status!=MemberStatus.Member) return false;
            if(DataManager.TryGetGroupMemberData(group, user, out var userData)) return false;
            if(userData.Status != MemberStatus.Member) return false;
            if(kickerData.Rank <= userData.Rank) return false;
            userData.Status = MemberStatus.Kicked;
            OnUserKicked?.Invoke(group,kicker,user,true);
            foreach(var con in GetGroupConnections(group))
                Client_ReceiveKickedNotification(con,userData,kicker);
            return true;
        }
        
        /// <inheritdoc />
        public bool Disband(GroupIdentity group) {
            if(group == GroupIdentity.Default) return false;
            DataManager.RemoveGroup(group);
            OnDisband?.Invoke(group,false);
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Server Rpcs ////////////////////////////////////////////////////////////////////////////////////////////
        
        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveUniqueDataRequest(NetworkConnection con = null) {
            if(!con.TryGetUserId(out var userId)) return;
            var memberInfo = new List<GroupMemberData>();
            foreach(var groupId in DataManager.GetUsersGroups(userId)) {
                if(!DataManager.TryGetGroupMembers(groupId, out var members))continue;
                foreach(var memberId in members) {
                    if(!DataManager.TryGetGroupMemberData(groupId, memberId, out var memberData)) continue;
                    memberInfo.Add(memberData);
                }
            }
            Client_ReceiveGroupMembers(con, memberInfo);
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveCreateRequest(string name, GroupType type, GroupAuthType authType,
            string saltedPassword = null, string salt = null, NetworkConnection con = null) {
            var nType = GroupEvent.Create;
            if(!TryGetIdentity(nType, con, out var owner)) return;
            if(authenticationMode != ClientServer.Client) {
                if(authType == GroupAuthType.Password && saltedPassword == null) {
                    Client_ReceiveFailure(con, nType, PASSWORD_REQUIRED);
                    return;
                }
                //validate
                var args = new CreatingGroupEventArgs(name, owner, type, authType, true);
                OnCreating?.Invoke(this, args);
                if(args.Canceled) {
                    Client_ReceiveFailure(con, nType, args.CancelReason);
                    return;
                }
            }
            //create
            if(!Create(name, type, owner, out _, authType, saltedPassword, salt)) {
                Client_ReceiveFailure(con,nType,$"Unable to create the group {name}");
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveOwnerChangeRequest(uint group, uint user, NetworkConnection con = null) {
            var nType = GroupEvent.OwnerChange;
            if(!TryGetIdentity(nType, con, out var requester)) return;
            if(!TryGetGroupAndUser(con,nType, group, user, out var groupIdentity, out var userIdentity)) return;
            if(authenticationMode != ClientServer.Client) {
                if(groupIdentity.OwnerId != requester) {
                    Client_ReceiveFailure(con, nType, OWNER_CHANGE_OWNER);
                    return;
                }
                var args = new ChangingOwnerGroupEventArgs(groupIdentity, requester, userIdentity, true);
                OnChangingOwner?.Invoke(this, args);
                if(args.Canceled) {
                    Client_ReceiveFailure(con, nType, args.CancelReason);
                    return;
                }
            }
            if(!ChangeOwner(groupIdentity,userIdentity))
                Client_ReceiveFailure(con,nType,$"Unable to change the owner of the {groupIdentity.Link} group to {userIdentity.Link}");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveApplyRequest(uint group, string request = null, string password = null,
            NetworkConnection con = null) {
            var nType = GroupEvent.Apply;
            if(!TryGetIdentity(GroupEvent.Apply, con, out var userIdentity)) return; //get the user
            if(!TryGetGroup(group, out var groupIdentity)) { //get the group data.
                Client_ReceiveFailure(con,nType, INVALID_GROUP_ID); return;
            }
            if(authenticationMode != ClientServer.Client) {
                if(IsMember(group, userIdentity)) { //make sure the user is not part of the group.
                    Client_ReceiveFailure(con, nType, ALREADY_A_MEMBER);
                    return;
                }
                if(groupIdentity.AuthType == GroupAuthType.Password && password == null) { //make user password is valid
                    Client_ReceiveFailure(con, nType, PASSWORD_REQUIRED);
                    return;
                }
                var args = new JoiningGroupEventArgs(groupIdentity, userIdentity, true);
                OnJoining?.Invoke(this, args); //make sure that the application has not been canceled.
                if(args.Canceled) {
                    Client_ReceiveFailure(con, nType, args.CancelReason);
                    return;
                }
            }
            if(!Apply(groupIdentity, userIdentity, request, password))
                Client_ReceiveFailure(con,nType,$"Unable to apply to the {groupIdentity.Link} group.");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveInviteRequest(uint group, uint user, NetworkConnection con = null) {
            const GroupEvent nType = GroupEvent.Invite;
            if(!TryGetIdentity(GroupEvent.Apply, con, out var inviter)) return; //get the user
            if(!TryGetGroupAndUser(con, nType,group, user, out var groupIdentity, out var userIdentity)) return;
            if(authenticationMode != ClientServer.Client) {
                if(!groupIdentity.IsMember(inviter)) { //check that the local user is a member of the group.
                    Client_ReceiveFailure(con, nType, NOT_A_MEMBER); return;
                }
                if(groupIdentity.IsMember(user)) { //make user that the user is not a member of the group.
                    Client_ReceiveFailure(con,nType, USER_ALREADY_MEMBER); return;
                }
                var args = new InvitingGroupEventArgs(groupIdentity, inviter, userIdentity, true);
                OnInviting?.Invoke(this,args);
                if(args.Canceled) { //make sure that the invite has not been canceled.
                    Client_ReceiveFailure(con, nType, args.CancelReason); return;
                }
            }
            if(!Invite(groupIdentity, inviter, userIdentity))
                Client_ReceiveFailure(con,nType,$"Unable to invite the user {userIdentity.Link} to the {groupIdentity.Link} group!");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveApproveApplicationRequest(uint group, uint user, NetworkConnection con = null) {
            const GroupEvent nType = GroupEvent.Approve;
            if(!TryGetIdentity(nType, con, out var approver)) return;
            if(!TryGetGroupAndUser(con,nType, group, user, out var groupIdentity, out var userIdentity)) return;
            if(authenticationMode != ClientServer.Client) {
                if(!groupIdentity.IsMember(approver)) { //make sure that the local user is a member of the group.
                    Client_ReceiveFailure(con,nType, NOT_A_MEMBER); return;
                }
                if(groupIdentity.IsMember(user)) { //make sure that the user is not a member of the group.
                    Client_ReceiveFailure(con,nType, USER_ALREADY_MEMBER); return;
                }
                if(groupIdentity.GetStatus(user) != MemberStatus.Applying) {
                    //make sure the user is applying to the group
                    Client_ReceiveFailure(con,nType, USER_NOT_APPLYING); return;
                }
                //check for cancellations
                var args = new ApprovingJoinGroupEventArgs(groupIdentity, approver, userIdentity, true);
                OnApproving?.Invoke(this,args);
                if(args.Canceled) { //make sure that that approval has not been canceled.
                    Client_ReceiveFailure(con,nType, args.CancelReason); return;
                }
            }
            if(!ApproveApplication(groupIdentity,userIdentity,approver))
                Client_ReceiveFailure(con,nType,$"Unable to approve of the user {userIdentity.Link} for the group {groupIdentity.Link}!");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveRankChangeRequest(uint group, uint user, short rank, NetworkConnection con = null) {
            const GroupEvent nType = GroupEvent.Rank;
            if(!TryGetIdentity(nType, con, out var ranker)) return;
            if(!TryGetGroupAndUser(con,nType, group, user, out var groupIdentity, out var userIdentity)) return;
            if(authenticationMode != ClientServer.Client) {
                var currentRank = groupIdentity.GetRank(user);
                var localRank = groupIdentity.GetRank(ranker);
                if(!groupIdentity.IsMember(ranker)) { //check that the local user is a member of the group.
                    Client_ReceiveFailure(con,nType, NOT_A_MEMBER); return;
                }
                if(!groupIdentity.IsMember(user)){ //check that the user is a member of the group.
                    Client_ReceiveFailure(con,nType, USER_NOT_MEMBER); return;
                }
                if(currentRank == rank) { //check that the rank is not the current rank.
                    Client_ReceiveFailure(con,nType, NO_CHANGE_IN_RANK); return;
                }
                if(localRank <= currentRank) { //make sure the user does not have a higher rank than your own.
                    Client_ReceiveFailure(con,nType, INVALID_RANK_TO_RANK); return;
                }
                if(localRank >= rank){ //make sure that the rank you are trying to assign is less than your own.
                    Client_ReceiveFailure(con,nType, UNAVAILABLE_RANK); return;
                }
                var args = new RankingGroupEventArgs(groupIdentity, ranker, userIdentity, rank, true);
                OnRanking?.Invoke(this,args);
                if(args.Canceled) { //make sure that the ranking was not canceled.
                    Client_ReceiveFailure(con,nType, args.CancelReason); return;
                }
            }
            Server_ReceiveRankChangeRequest(group, user, rank); //send the request to the server
            if(!RankChange(groupIdentity,ranker,userIdentity,rank))
                Client_ReceiveFailure(con,nType,$"Unable to change the rank of {userIdentity.Link} in the {groupIdentity.Link} group!");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveLeaveRequest(uint group, NetworkConnection con = null) {
            const GroupEvent nType = GroupEvent.Leave;
            if(!TryGetIdentity(nType, con, out var userIdentity)) return;
            if(!TryGetGroup(group, out var groupIdentity)) { //get the group data.
                Client_ReceiveFailure(con,nType, INVALID_GROUP_ID); return;
            }
            if(authenticationMode != ClientServer.Client) {
                if(!groupIdentity.IsMember(userIdentity)) { //make sure that the user is a member of the group.
                    Client_ReceiveFailure(con,nType, NOT_A_MEMBER); return;
                }
                if(groupIdentity.OwnerId == userIdentity) { //special situations for owners of groups
                    if(GetMemberCount(group) != 1) {
                        Client_ReceiveFailure(con,GroupEvent.Leave, MUST_ASSIGN_NEW_OWNER); return;
                    }
                    //disband instead of leave
                    var args2 = new DisbandingGroupEventArgs(groupIdentity, userIdentity, true);
                    OnDisbanding?.Invoke(this,args2);
                    if(args2.Canceled) {
                        Client_ReceiveFailure(con,GroupEvent.Disband,args2.CancelReason); return;
                    }
                    if(!Disband(groupIdentity))
                        Client_ReceiveFailure(con,GroupEvent.Disband,$"Unable to disband the {groupIdentity.Link} group!");
                    return; //return the result of the disband request.
                }
                var args = new LeavingGroupEventArgs(groupIdentity, userIdentity, true);
                OnLeaving?.Invoke(this,args);
                if(args.Canceled) { //make sure that the leaving was not canceled.
                    Client_ReceiveFailure(con,nType, args.CancelReason); return;
                }
            }
            if(!Remove(groupIdentity, userIdentity))
                Client_ReceiveFailure(con, nType, $"Unable to leave the {groupIdentity.Link} group!");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveKickRequest(uint group, uint user, NetworkConnection con = null) {
            const GroupEvent nType = GroupEvent.Kick;
            if(!TryGetIdentity(nType, con, out var kicker)) return;
            if(!TryGetGroupAndUser(con, nType, group, user, out var groupIdentity, out var userIdentity)) return;
            if(authenticationMode != ClientServer.Client) {
                if(!groupIdentity.IsMember(kicker)) { //check that local user is a member of the group.
                    Client_ReceiveFailure(con,nType, NOT_A_MEMBER); return;
                }
                if(!groupIdentity.IsMember(user)) { //check that the given user is a member of the group.
                    Client_ReceiveFailure(con,nType, USER_NOT_MEMBER); return;
                }
                if(groupIdentity.GetRank(user) >= groupIdentity.GetRank(kicker)){ //make sure your rank is higher
                    Client_ReceiveFailure(con,nType, UNABLE_TO_KICK_EQUAL_OR_HIGHER_RANK); return;
                }
                var args = new KickingGroupEventArgs(groupIdentity, kicker, userIdentity, true);
                OnKicking?.Invoke(this,args);
                if(args.Canceled) { // check to see if the kicking was canceled.
                    Client_ReceiveFailure(con,nType, args.CancelReason);  return;
                }
            }
            if(!Kick(groupIdentity,kicker,userIdentity))
                Client_ReceiveFailure(con,nType,$"Unable to kick the {userIdentity.Link} user from the {groupIdentity.Link} group!");
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_DisbandRequest(uint group, NetworkConnection con = null) {
            const GroupEvent nType = GroupEvent.Disband;
            if(!TryGetIdentity(nType, con, out var userIdentity)) return;
            if(!TryGetGroup(group, out var groupIdentity)) { //get the group data.
                Client_ReceiveFailure(con,nType, INVALID_GROUP_ID); return;
            }
            if(authenticationMode != ClientServer.Client) {
                if(!groupIdentity.IsMember(userIdentity)) { //make sure that the user is a member of the group.
                    Client_ReceiveFailure(con,nType, NOT_A_MEMBER); return;
                }
                if(groupIdentity.OwnerId != userIdentity) { //make sure that the user is the owner of the group.
                    Client_ReceiveFailure(con,nType, ONLY_OWNER_CAN_DISBAND); return;
                }
                var args = new DisbandingGroupEventArgs(groupIdentity, userIdentity, true);
                OnDisbanding?.Invoke(this, args);
                if(args.Canceled) { //make sure that the disbanding wasn't canceled.
                    Client_ReceiveFailure(con,nType, args.CancelReason); return;
                }
            }
            if(!Disband(groupIdentity))
                Client_ReceiveFailure(con,nType, $"Unable to disband the {groupIdentity.Link} group!");
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool TryGetIdentity(GroupEvent type, NetworkConnection connection, out UserIdentity user) {
            user = UserIdentity.DefaultUser;
            var result = connection.TryGetUserId(out var userId) && UserManager.TryGetIdentity(userId, out user);
            if(result) return true;
            Client_ReceiveFailure(connection,type,"Can not get the user from the connection!");
            return false;
        }
        
        private bool TryGetGroupAndUser(NetworkConnection con, GroupEvent type, uint groupId, uint userId, out GroupIdentity group,
            out UserIdentity user) {
            group = GroupIdentity.Default;
            user = UserIdentity.DefaultUser;
            if(!TryGetGroup(groupId, out group)) {
                Client_ReceiveFailure(con,type, INVALID_GROUP_ID);
                return false;
            }
            if(UserManager.TryGetIdentity(userId, out user)) {
                Client_ReceiveFailure(con,type, INVALID_USER_ID);
                return false;
            }
            return true;
        }
        
    }
}