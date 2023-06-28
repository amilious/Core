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

using Amilious.Core.Identity.User;

namespace Amilious.Core.Identity.Group.GroupEventArgs {
    
    /// <summary>
    /// This class is used for the <see cref="IGroupIdentityManager.OnRanking"/> event.
    /// </summary>
    public class RankingGroupEventArgs : AbstractGroupMemberEventArgs {

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the user's current rank in the group.
        /// </summary>
        public short CurrentRank => Group.GetRank(User);
        
        /// <summary>
        /// The user that is requesting the rank change.
        /// </summary>
        public UserIdentity Requester { get; }
        
        /// <summary>
        /// The new rank that is being requested for the user.
        /// </summary>
        public short NewRank { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new <see cref="RankingGroupEventArgs"/>.
        /// </summary>
        /// <param name="group">The group that the event is for.</param>
        /// <param name="requester">The user that is requesting the rank change.</param>
        /// <param name="user">The user that the rank change is for.</param>
        /// <param name="rank">The new rank that you want to give the user.</param>
        /// <param name="server">True if executing for the server, otherwise false for the client.</param>
        public RankingGroupEventArgs(GroupIdentity group, UserIdentity requester, UserIdentity user, short rank,
            bool server) : base(group, user, server) {
            Requester = requester;
            NewRank = rank;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}