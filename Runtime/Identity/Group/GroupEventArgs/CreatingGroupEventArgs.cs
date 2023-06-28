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

using Amilious.Core.Events;
using Amilious.Core.Identity.User;

namespace Amilious.Core.Identity.Group.GroupEventArgs {
    
    /// <summary>
    /// This class is used for the <see cref="IGroupIdentityManager.OnCreating"/> event.
    /// </summary>
    public class CreatingGroupEventArgs : AbstractServerClientEventArgs {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the creator of the group.
        /// </summary>
        public UserIdentity Creator { get; }
        
        /// <summary>
        /// This property contains the group type.
        /// </summary>
        public GroupType Type { get; }
        
        /// <summary>
        /// This property contains the authentication type.
        /// </summary>
        public GroupAuthType AuthType { get; }
        
        /// <summary>
        /// This property contains the group name.
        /// </summary>
        public string Name { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new <see cref="CreatingGroupEventArgs"/>.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="creator">The user that is trying to create the group.</param>
        /// <param name="type">The type of group.</param>
        /// <param name="authType">The authentication type of the group.</param>
        /// <param name="server">True if executing for the server, otherwise false for the client.</param>
        public CreatingGroupEventArgs(string name, UserIdentity creator, GroupType type, GroupAuthType authType,
            bool server) : base(server) {
            Name = name;
            Creator = creator;
            Type = type;
            AuthType = authType;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}