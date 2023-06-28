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
    
    /// <summary>
    /// This struct is used to represent the identity of a group.
    /// </summary>
    public readonly struct GroupIdentity : IIdentity {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string GROUP_NAME_KEY = "_group_name_";
        public const string GROUP_TYPE_KEY = "_group_type_";
        public const string GROUP_PASSWORD_KEY = "_group_password_";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private readonly int? _id;
        private readonly int? _owner;
        private readonly string _name;
        private readonly GroupType? _groupType;
        private readonly GroupAuthType? _authType;
        public readonly string _link;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Instances ///////////////////////////////////////////////////////////////////////////////////////

        public static readonly GroupIdentity Default = default;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public int Id => _id ?? int.MinValue;

        public int OwnerId => _owner ?? int.MinValue;

        public string Name => _name ?? "Default Group";

        public GroupType Type => _groupType ?? GroupType.Global;

        public GroupAuthType AuthType => _authType ?? GroupAuthType.None;

        public string Link => _link ?? Name;

        public IdentityType IdentityType => IdentityType.Group;
        
        public IGroupIdentityManager Manager { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        public GroupIdentity(IGroupIdentityManager manager, int id, string name, GroupType groupType, 
            int owner = int.MinValue, GroupAuthType authType = GroupAuthType.None) {
            _id = id;
            _name = name;
            _groupType = groupType;
            _authType = authType;
            _owner = owner;
            _link = $"<link=group|{id}>{name}</link>";
            Manager = manager;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the rank of the given user within the group.
        /// </summary>
        /// <param name="user">The user that you want to get the rank of.</param>
        /// <returns>The user's rank or <see cref="short.MinValue"/> if the user does not exist or is not within the
        /// group.</returns>
        public short GetRank(int user) => Manager.GetRank(Id,user);

        /// <summary>
        /// This method is used to check if the given user is a member of the group.
        /// </summary>
        /// <param name="user">The user that you want to check.</param>
        /// <returns>True if the user is a member in the group, otherwise false.</returns>
        public bool IsMember(int user) => Manager.IsMember(Id, user);

        /// <summary>
        /// This method is used to get the user's status within the group.
        /// </summary>
        /// <param name="user">The user that you want to check.</param>
        /// <returns>The status of the user within the group or <see cref="MemberStatus.None"/> if not in the group.
        /// </returns>
        public MemberStatus GetStatus(int user) => Manager.GetStatus(Id, user);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operators //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Converts the <see cref="GroupIdentity"/> to an int.
        /// </summary>
        /// <param name="identity">The <see cref="GroupIdentity"/> instance.</param>
        public static implicit operator int(GroupIdentity identity) { return identity.Id; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}