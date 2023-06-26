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

using Amilious.Core.Authentication;

namespace Amilious.Core.Identity.Group {
    
    /// <summary>
    /// This struct is used to represent the identity of a group.
    /// </summary>
    public readonly struct GroupIdentity : IIdentity {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string GROUP_NAME_KEY = "_group_name_";
        public const string GROUP_TYPE_KEY = "_group_type_";
        public const string GROUP_AUTH_TYPE_KEY = "_group_auth_type_";
        public const string GROUP_PASSWORD_KEY = "_group_password_";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private readonly int? _id;
        private readonly string _name;
        private readonly GroupType? _groupType;
        private readonly AuthType? _authType;
        private readonly string _link;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Instances ///////////////////////////////////////////////////////////////////////////////////////

        public static readonly GroupIdentity Default = default;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the group's identity manager.
        /// </summary>
        public static IGroupIdentityManager Manager { get; private set; }
        
        /// <inheritdoc />
        public int Id => _id ?? int.MinValue;

        /// <inheritdoc />
        public string Name => _name ?? "Default Group";

        /// <summary>
        /// This property is used to get the group type.
        /// </summary>
        public GroupType GroupType => _groupType ?? GroupType.Global;

        /// <summary>
        /// This property is used to get the group link.
        /// </summary>
        public string Link => _link ?? Name;

        /// <inheritdoc />
        public IdentityType IdentityType => IdentityType.Group;

        /// <summary>
        /// This property is used to get the groups authentication type.
        /// </summary>
        public AuthType AuthType => _authType ?? AuthType.None;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new group representation.
        /// </summary>
        /// <param name="id">The id of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="groupType">The type of the group.</param>
        /// <param name="authType">The authentication type of the group.</param>
        public GroupIdentity(int id, string name, GroupType groupType, AuthType authType = AuthType.None) {
            _id = id;
            _name = name;
            _groupType = groupType;
            _link = $"<link=group|{id}>{name}</link>";
            _authType = authType;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set the group identity manger for group identities.
        /// </summary>
        /// <param name="manager">The group identity manager.</param>
        /// <returns>True if the manager was set, otherwise false if the manager has already been set.</returns>
        /// <remarks>This should only be called by the group identity manager.</remarks>
        public static bool SetIdentityManager(IGroupIdentityManager manager) {
            if(Manager != null) return false;
            Manager = manager;
            return true;
        }
        
    }
    
}