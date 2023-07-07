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
using System.Collections.Generic;
using Amilious.Core.Identity.Group.Data;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group.GroupEventArgs;

namespace Amilious.Core.Identity.Group {
    
    public interface IGroupIdentityManager {
        
        #region Client & Server Properties /////////////////////////////////////////////////////////////////////////////
                           
        /// <summary>
        /// This property is used to get the group identity for the given id.
        /// </summary>
        /// <param name="userId">The group's id.</param>
        public GroupIdentity this[uint userId] { get; }
        
        /// <summary>
        /// This property is used to get a collection of groups.
        /// </summary>
        IEnumerable<GroupIdentity> Identities { get; }
        
        /// <summary>
        /// This property is used to get the data manager.
        /// </summary>
        public IGroupDataManager DataManager { get; }
        
        /// <summary>
        /// This property is used to get the user manager.
        /// </summary>
        public IUserIdentityManager UserManager { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////

        public delegate void DisbandDelegate(uint group, bool isServer);
        
        public delegate void GroupCreatedDelegate(GroupIdentity group, bool isServer);

        public delegate void GroupUserDelegate(GroupIdentity group, UserIdentity user, bool isServer);

        public delegate void GroupInviteDelegate(GroupIdentity group, UserIdentity inviter, UserIdentity user,
            bool isServer);

        public delegate void GroupRankChangeDelegate(GroupIdentity group, UserIdentity ranker, UserIdentity user,
            short rank, bool isServer);

        public delegate void UserKickedDelegate(GroupIdentity group, UserIdentity kicker, UserIdentity user,
            bool isServer);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        public event DisbandDelegate OnDisband;
        
        public event GroupCreatedDelegate OnGroupCreated;

        public event GroupUserDelegate OnUserJoined;

        public event GroupUserDelegate OnReceiveApplication;

        public event GroupInviteDelegate OnUserInvited;

        public event GroupRankChangeDelegate OnRankChanged;

        public event GroupUserDelegate OnUserLeft;

        public event UserKickedDelegate OnUserKicked;

        public event GroupUserDelegate OnOwnerChanged;

        /// <summary>
        /// This event is triggered when trying to create a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<CreatingGroupEventArgs> OnCreating;

        /// <summary>
        /// This event is triggered when trying to change a group's owner.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<ChangingOwnerGroupEventArgs> OnChangingOwner; 

        /// <summary>
        /// This event is triggered when trying to join a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<JoiningGroupEventArgs> OnJoining;

        /// <summary>
        /// This event is triggered when trying to invite a user to a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<InvitingGroupEventArgs> OnInviting;

        /// <summary>
        /// This event is triggered when trying to approve a join request for a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<ApprovingJoinGroupEventArgs> OnApproving;

        /// <summary>
        /// This event is triggered when trying to change the rank for a user in a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<RankingGroupEventArgs> OnRanking;

        /// <summary>
        /// This event is triggered when trying to leave a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<LeavingGroupEventArgs> OnLeaving;
        
        /// <summary>
        /// This event is triggered when trying to kick a user from a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<KickingGroupEventArgs> OnKicking;

        /// <summary>
        /// This event is triggered when trying to disband a group.
        /// </summary>
        /// <remarks>This event is triggered on both the client and on the server.</remarks>
        public event EventHandler<DisbandingGroupEventArgs> OnDisbanding;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client & Server Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if a user is a member of a group.
        /// </summary>
        /// <param name="group">The group in which you want to check for the user.</param>
        /// <param name="user">The user that you want to check if is a member of a group.</param>
        /// <returns>True if the given user is a member of the given group.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        bool IsMember(uint group, uint user);

        /// <summary>
        /// This method is used to check if a user has been invited to the given group.
        /// </summary>
        /// <param name="group">The group in which you want to check if the user has been invited.</param>
        /// <param name="user">The user that you want to check is has been invited.</param>
        /// <returns>True if the user has been invited.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        bool IsInvited(uint group, uint user);

        /// <summary>
        /// This method is used to check if a user has apply to a group.
        /// </summary>
        /// <param name="group">The group in which you want to check if a user has applied.</param>
        /// <param name="user">The user that you want to check if has applied to the group.</param>
        /// <returns>True if the user has applied to the group, otherwise false.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        bool HasApplied(uint group, uint user);

        /// <summary>
        /// This method is used to get the number of members within a given group.
        /// </summary>
        /// <param name="group">The group in which you want to get a member count.</param>
        /// <returns>The number of members within the given group.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        int GetMemberCount(uint group);
        
        /// <summary>
        /// This method is used to get the rank of a user within the given group.
        /// </summary>
        /// <param name="group">The group in which you wan to get the rank of the user.</param>
        /// <param name="user">The user that you want to get the rank of within the group.</param>
        /// <returns>The rank of the user in the group or <see cref="short.MinValue"/> if not in the group.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        short GetRank(uint group, uint user);

        /// <summary>
        /// This method is used to get the status of a user within a group.
        /// </summary>
        /// <param name="group">The group in which you want to get the user's status.</param>
        /// <param name="user">The user that you want to get the status of within the group.</param>
        /// <returns>The status of the user within the group, otherwise <see cref="MemberStatus.None"/> if not
        /// in the group.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        MemberStatus GetStatus(uint group, uint user);

        /// <summary>
        /// This method is used to get the user's within a group.
        /// </summary>
        /// <param name="group">The group in which you want to get the members.</param>
        /// <returns>The user identities for the members of the group.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        IEnumerable<UserIdentity> TryGetMembers(uint group);

        /// <summary>
        /// This method is used to get the user's within a group.
        /// </summary>
        /// <param name="group">The group in which you want to get the members.</param>
        /// <returns>The user member data for the members of the group.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        IEnumerable<GroupMemberData> TryGetAllMemberData(uint group);

        /// <summary>
        /// This method is used to try get the group from the group id.
        /// </summary>
        /// <param name="group">The id of the group</param>
        /// <param name="groupIdentity">The group identity for the given group.</param>
        /// <returns>True if a group with the given id exists, otherwise false.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        bool TryGetGroup(uint group, out GroupIdentity groupIdentity);

        /// <summary>
        /// This method is used to try get the member data for the given user and the given group.
        /// </summary>
        /// <param name="group">The group that you want to get the data for.</param>
        /// <param name="user">The user that you want to get the data for.</param>
        /// <param name="data">The data for the given user in the given group.</param>
        /// <returns>True if data for the given user and group exists, otherwise false.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        bool TryGetMemberData(uint group, uint user, out GroupMemberData data);

        /// <summary>
        /// This method is used to try get the member data for the given user and the given group.
        /// </summary>
        /// <param name="group">The group that you want to get the data for.</param>
        /// <param name="user">The user that you want to get the data for.</param>
        /// <param name="groupIdentity">The group identity for the given group.</param>
        /// <param name="userIdentity">The user identity for the given user.</param>
        /// <param name="data">The data for the given user in the given group.</param>
        /// <returns>True if data and identities for the given user and group exists, otherwise false.</returns>
        /// <remarks>This method can be called from the <b>Client</b> or <b>Server</b>!</remarks>
        bool TryGetMemberData(uint group, uint user, out GroupIdentity groupIdentity,
            out UserIdentity userIdentity, out GroupMemberData data);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to try create a group on the server.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="type">The type of the group.</param>
        /// <param name="owner">The owner of the group.</param>
        /// <param name="group">The newly created group.</param>
        /// <param name="authType">The authorization type of the group.</param>
        /// <param name="saltedPassword">(optional) The salted password of the group.</param>
        /// <param name="salt">(optional) The password salt.</param>
        /// <returns>True if able to create the group, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnCreating"/> event.</remarks>
        /// <seealso cref="RequestCreate">RequestCreate (if on client)</seealso>
        public bool Create(string name, GroupType type, UserIdentity owner, out GroupIdentity group, 
            GroupAuthType authType = GroupAuthType.None, string saltedPassword = null, string salt = null);

        /// <summary>
        /// This method is used to try change the owner of a group.
        /// </summary>
        /// <param name="group">The group that you want to change.</param>
        /// <param name="owner">The owner that you want to set.</param>
        /// <returns>True if the owner was changed, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnChangingOwner"/> event.</remarks>
        /// <seealso cref="RequestOwnerChange">RequestOwnerChange (if on client)</seealso>
        public bool ChangeOwner(GroupIdentity group, UserIdentity owner);

        /// <summary>
        /// This method is used to add a user to the group.
        /// </summary>
        /// <param name="group">The group that you want to add the user to.</param>
        /// <param name="user">The user that you want to add to the group.</param>
        /// <returns>True if the user was added to the group, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnJoining"/> event.</remarks>
        /// <seealso cref="RequestApply">RequestApply (if on client)</seealso>
        public bool AddMember(GroupIdentity group, UserIdentity user);

        /// <summary>
        /// This method is used to have a user apply to a group.
        /// </summary>
        /// <param name="group">The group that you want to request to join.</param>
        /// <param name="user">The user that you want to add to join the group</param>
        /// <param name="request">(optional) The application request.</param>
        /// <param name="password">(optional) The group password.</param>
        /// <returns>True if able to apply to the group</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnJoining"/> event.</remarks>
        /// <seealso cref="RequestApply">RequestApply (if on client)</seealso>
        public bool Apply(GroupIdentity group, UserIdentity user, string request = null, string password = null);

        /// <summary>
        /// This method is used to approve of a join request.
        /// </summary>
        /// <param name="group">The group that you want to approve of the join request.</param>
        /// <param name="user">The user that you want to approve to join the group.</param>
        /// <param name="approver">The user that is approving of the join.</param>
        /// <returns>True if the application was approved.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnApproving"/> event.</remarks>
        /// <seealso cref="RequestApproveApplication">RequestApproveApplication (if on client)</seealso>
        public bool ApproveApplication(GroupIdentity group, UserIdentity user, UserIdentity approver);

        /// <summary>
        /// This method is used to invite a user to a group.
        /// </summary>
        /// <param name="group">The group that you want to invite the user to.</param>
        /// <param name="inviter">The member that is inviting the user to the group.</param>
        /// <param name="user">The user that you want to invite.</param>
        /// <returns>True if the user was invited, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnInviting"/> event.</remarks>
        /// <seealso cref="RequestInvite">RequestInvite (if on client)</seealso>
        public bool Invite(GroupIdentity group, UserIdentity inviter, UserIdentity user);

        /// <summary>
        /// This method is used to change a user's rank in a group.
        /// </summary>
        /// <param name="group">The group in which you want to change the user's rank.</param>
        /// <param name="ranker">The member that is setting the rank of the user.</param>
        /// <param name="user">The user that you want to change the rank of.</param>
        /// <param name="rank">The rank that you want to apply to the user.</param>
        /// <returns>True if able to change the user's rank, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnRanking"/> event.</remarks>
        /// <seealso cref="RequestRankChange">RequestRankChange (if on client)</seealso>
        public bool RankChange(GroupIdentity group,  UserIdentity ranker, UserIdentity user, short rank);

        /// <summary>
        /// This method is used to remove a user from a group.
        /// </summary>
        /// <param name="group">The group that you want to remove the user from.</param>
        /// <param name="user">The user that you want to remove from the group.</param>
        /// <returns>True if able to remove the user from the group, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnLeaving"/> event.</remarks>
        /// <seealso cref="RequestLeave">RequestLeave (if on client)</seealso>
        public bool Remove(GroupIdentity group, UserIdentity user);

        /// <summary>
        /// This method is used to kick a user from the group.
        /// </summary>
        /// <param name="group">The group that the user is being kicked from.</param>
        /// <param name="kicker">The member that is kicking the user from the group.</param>
        /// <param name="user">The user that is being kicked from the group.</param>
        /// <returns>True if able to kick the user from the group, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnKicking"/> event.</remarks>
        /// <seealso cref="RequestKick">RequestKick (if on client)</seealso>
        public bool Kick(GroupIdentity group, UserIdentity kicker, UserIdentity user);

        /// <summary>
        /// This method is used to remove a user from a group.
        /// </summary>
        /// <param name="group">The group that you want to remove the user from.</param>
        /// <returns>True if able to remove the user from the group, otherwise false.</returns>
        /// <remarks>This method should only be called from the <b>Server</b>!  It also skips the
        /// <see cref="OnDisbanding"/> event.</remarks>
        /// <seealso cref="RequestDisband">RequestKick (if on client)</seealso>
        public bool Disband(GroupIdentity group);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client Only Methods ////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to request the creation of a new group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="type">The type of the group.</param>
        /// <param name="authType">The authorization type of the group.</param>
        /// <param name="password">(optional) The password of the group.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="Create">Create (if on server)</seealso>
        bool RequestCreate(string name, GroupType type, GroupAuthType authType = GroupAuthType.None, 
            string password = null);

        /// <summary>
        /// This method is used to request the owner change of a group.
        /// </summary>
        /// <param name="group">The group that you want to change the owner for.</param>
        /// <param name="user">The user that you want to set as the owner.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="ChangeOwner">ChangeOwner (if on server)</seealso>
        bool RequestOwnerChange(uint group, uint user);
        
        /// <summary>
        /// This method is used to request entry to a group.
        /// </summary>
        /// <param name="group">The group that you are requesting to join.</param>
        /// <param name="request">The request to join.</param>
        /// <param name="password">(optional) The group password.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="AddMember">AddMember (if on server)</seealso>
        /// <seealso cref="Apply">Apply (if on server)</seealso>
        bool RequestApply(uint group, string request = null, string password = null);

        /// <summary>
        /// This method is used to request an invitation of the given user to the given group.
        /// </summary>
        /// <param name="group">The group in which you want to invite the user.</param>
        /// <param name="user">The user that you want to invite to the group.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="Invite">Invite (if on server)</seealso>
        bool RequestInvite(uint group, uint user);

        /// <summary>
        /// This method is used to request approval of an application to a group.
        /// </summary>
        /// <param name="group">The group in which you want to accept an application.</param>
        /// <param name="user">The user's whose application you want to approve.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="ApproveApplication">ApproveApplication (if on server)</seealso>
        bool RequestApproveApplication(uint group, uint user);

        /// <summary>
        /// This method is used to request changing the rank of a user in the group.
        /// </summary>
        /// <param name="group">The group that you want to change the user's rank in.</param>
        /// <param name="user">The user that you want to change the rank of.</param>
        /// <param name="rank">The rank that you want to assign to the user.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="RankChange">RankChange (if on server)</seealso>
        bool RequestRankChange(uint group, uint user, short rank);

        /// <summary>
        /// This method is used to request leaving a group.
        /// </summary>
        /// <param name="group">The group that you want to leave.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="Remove">Remove (if on server)</seealso>
        bool RequestLeave(uint group);

        /// <summary>
        /// This method is used to request the kicking of a user in the group.
        /// </summary>
        /// <param name="group">The group that you want to kick the user from.</param>
        /// <param name="user">The user that you want to kick from the group.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="Kick">Kick (if on server)</seealso>
        bool RequestKick(uint group, uint user);

        /// <summary>
        /// This method is used to request a group disband.
        /// </summary>
        /// <param name="group">The group that you want to disband.</param>
        /// <returns>True if able to send the request.</returns>
        /// <remarks>This method should only be called from the <b>Client</b>!</remarks>
        /// <seealso cref="Disband">Disband (if on server)</seealso>
        bool RequestDisband(uint group);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}