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

namespace Amilious.Core.Identity.Group {
    
    public interface IGroupIdentityManager {
        
        /// <summary>
        /// This method is used to check if the given identities id is part of the given group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="identityId">The entities id.</param>
        /// <returns>True if the given identities id is part of the given group, otherwise false.</returns>
        bool IsMember(int groupId, int identityId);

        /// <summary>
        /// This method is used to get the member id's for the given group.
        /// </summary>
        /// <param name="groupId">The id of the group.</param>
        /// <param name="members">The members Identity id's of the group.</param>
        /// <returns>True if the group exists, otherwise false.</returns>
        bool TryGetMemberIds(int groupId, out int[] members);

        /// <summary>
        /// This method is used to get the group's info from the group id.
        /// </summary>
        /// <param name="groupId">The group's id.</param>
        /// <param name="groupIdentity">The group info.</param>
        /// <returns>True if the group id is valid, otherwise false.</returns>
        bool TryGetGroupInfo(int groupId, out GroupIdentity groupIdentity);

        /// <summary>
        /// This method is used to create a new group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="identityId">The identity id of the creator.</param>
        /// <param name="groupType">The type of the group.</param>
        /// <param name="groupId">The id of the newly created group.</param>
        /// <param name="password">(optional) The password for the group.</param>
        /// <returns>True if a group was created, otherwise false.</returns>
        bool CreateGroup(string name, int identityId, GroupType groupType, out int groupId, string password = null);

        /// <summary>
        /// This method is used to try leave a group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to leave.</param>
        /// <param name="identityId">The identity id of the user that want's to leave the group.</param>
        /// <returns>True if the user was in the group but now is not, otherwise false.</returns>
        bool TryLeaveGroup(int groupId, int identityId);
        
        /// <summary>
        /// This method is used to try join a group.
        /// </summary>
        /// <param name="groupId">The id of the group that you want to join.</param>
        /// <param name="identityId">The identity id of the user that want's to join the group.</param>
        /// <param name="password">(optional) The password for the group.</param>
        /// <returns>True if the user was not in the group but now is, otherwise false.</returns>
        bool TryJoinGroup(int groupId, int identityId, string password = null);

    }
    
}