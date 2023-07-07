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

namespace Amilious.Core.Identity.Group.Data {

    /// <summary>
    /// This class is used to access, store, and edit information.
    /// </summary>
    
    public interface IGroupDataManager {

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for group member data events.
        /// </summary>
        /// <param name="group">The group the event is for.</param>
        /// <param name="user">The user the event is for.</param>
        public delegate void GroupMemberDelegate(uint group, uint user);

        /// <summary>
        /// This delegate is used for group data events.
        /// </summary>
        /// <param name="group">The group that the event is for.</param>
        public delegate void GroupDelegate(uint group);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when a user's data has been removed from a group.
        /// </summary>
        /// <remarks>This event will only be triggered on the <b>SERVER</b>!</remarks>
        public event GroupMemberDelegate OnGroupMemberDataRemoved;

        /// <summary>
        /// This event is triggered when a group's data has been removed.
        /// </summary>
        /// <remarks>This event will only be triggered on the <b>SERVER</b>!</remarks>
        public event GroupDelegate OnGroupDataRemoved;

        /// <summary>
        /// This event is triggered when a user's data is modified for a group.
        /// </summary>
        /// <remarks>This event will only be triggered on the <b>SERVER</b>!</remarks>
        public event GroupMemberDelegate OnGroupMemberDataModified;

        /// <summary>
        /// This event is triggered when a group's data has been modified.
        /// </summary>
        /// <remarks>This event will only be triggered on the <b>SERVER</b>!</remarks>
        public event GroupDelegate OnGroupDataModified;

        /// <summary>
        /// This event is triggered when a group's data has been added.
        /// </summary>
        /// <remarks>This event will only be triggered on the <b>SERVER</b>!</remarks>
        public event Action<GroupData> OnGroupDataAdded;

        /// <summary>
        /// This event is triggered when a user's data has been added to a group.
        /// </summary>
        /// <remarks>This event will only be triggered on the <b>SERVER</b>!</remarks>
        public event Action<GroupMemberData> OnGroupMemberDataAdded;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Data Modified Callbacks ////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the data of a member is updated.
        /// </summary>
        /// <param name="data">The data that was updated.</param> 
        /// <remarks>This method will only be called on the <b>SERVER</b>!</remarks>
        public void OnMemberDataUpdated(GroupMemberData data);

        /// <summary>
        /// This method is called when the data of a group is updated.
        /// </summary>
        /// <param name="data">The data that was updated.</param>
        /// <remarks>This method will only be called on the <b>SERVER</b>!</remarks>
        public void OnGroupDataUpdated(GroupData data);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Group Data Methods /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the given user is a member of the group.
        /// </summary>
        /// <param name="group">The group that you want to check.</param>
        /// <param name="user">The user that you want to check.</param>
        /// <returns>True if the given user is a member of the given group.</returns>
        public bool IsMember(uint group, uint user);
        
        /// <summary>
        /// This method is used to remove a user from a group.
        /// </summary>
        /// <param name="group">The id of the group that you want to remove a user from.</param>
        /// <param name="user">The id of the user that you want to remove from the group.</param>
        /// <returns>True if the user was removed from the group, otherwise false if the user was not
        /// part of the group.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public bool RemoveUserFromGroup(uint group, uint user);

        /// <summary>
        /// This method is used to get all of the group ids.
        /// </summary>
        /// <returns>The group ids.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public IEnumerable<uint> GetGroupIds();

        /// <summary>
        /// This method is used to get the members of the given group.
        /// </summary>
        /// <param name="group">The id of the group that you want to get the members of.</param>
        /// <param name="members">The members of the group with the given id.</param>
        /// <returns>True if the group contains members, otherwise false.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public bool TryGetGroupMembers(uint group, out IEnumerable<uint> members);

        /// <summary>
        /// This method is used to get the group data for the given group.
        /// </summary>
        /// <param name="group">The group that you want to get the data for.</param>
        /// <param name="groupData">The group data.</param>
        /// <returns>True if the group data exists, otherwise false.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public bool TryGetGroupData(uint group, out GroupData groupData);

        /// <summary>
        /// This method is used to get the group member data for the given group and user.
        /// </summary>
        /// <param name="group">The group that you want to get the member data for.</param>
        /// <param name="user">The user that you want to get the member data for.</param>
        /// <param name="groupMemberData">The user's group member data.</param>
        /// <returns>True if able to get the group member data, otherwise false.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public bool TryGetGroupMemberData(uint group, uint user, out GroupMemberData groupMemberData);

        /// <summary>
        /// This method is used to get the number of members for a given group.
        /// </summary>
        /// <param name="groupId">The id of the group.</param>
        /// <param name="count">The number of members in the group.</param>
        /// <returns>True if the group exists, otherwise false.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public bool TryGetGroupMemberCount(uint groupId, out int count);

        /// <summary>
        /// This method is used to remove a group.
        /// </summary>
        /// <param name="group">The id of the group that you want to remove.</param>
        /// <returns>True if the group was removed, otherwise false if the group did not exist.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public bool RemoveGroup(uint group);

        /// <summary>
        /// This method is used to add a new group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="groupType">The group type.</param>
        /// <param name="creator">The creator of the group.</param>
        /// <param name="creatorData">The value will out put the creator's member data.</param>
        /// <param name="authType">The group authentication type.</param>
        /// <param name="password">The group password.</param>
        /// <param name="salt">The password salt.</param>
        /// <returns>The group member data of the newly created group.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public GroupData AddGroup(string name, GroupType groupType, uint creator,
            out GroupMemberData creatorData, GroupAuthType authType = GroupAuthType.None, string password = null, string salt = null);

        /// <summary>
        /// This method is used to add member data for the given user.
        /// </summary>
        /// <param name="group">The group that you want to add member data for.</param>
        /// <param name="user">The user the member data is for.</param>
        /// <param name="status">The status of the user.</param>
        /// <param name="rank">The rank of the user.</param>
        /// <param name="invitedBy">The id of the user's invitor.</param>
        /// <param name="approvedBy">The id of the user's approver.</param>
        /// <param name="applicationRequest">The user's application request.</param>
        /// <returns>This method returns the user member data for the given user and group.</returns>
        /// <remarks>This method should only be called on the <b>SERVER</b>!</remarks>
        public GroupMemberData AddGroupMemberData(uint group, uint user, MemberStatus status,
            short rank = 0, uint? invitedBy = null, uint? approvedBy = null, string applicationRequest = null);

        /// <summary>
        /// This method is used to get the groups that the user belongs to.
        /// </summary>
        /// <param name="user">The user that you want to get the group for.</param>
        /// <returns>The groups for the given user.</returns>
        public IEnumerable<uint> GetUsersGroups(uint user);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}