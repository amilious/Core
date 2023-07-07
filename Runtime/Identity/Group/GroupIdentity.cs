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

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private readonly uint? _id;
        private readonly uint? _owner;
        private readonly string _name;
        private readonly GroupType? _groupType;
        private readonly GroupAuthType? _authType;
        private readonly string _link;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Instances ///////////////////////////////////////////////////////////////////////////////////////

        public static readonly GroupIdentity Default = default;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public uint Id => _id ?? 0;

        public uint OwnerId => _owner ?? 0;

        public string Name => _name ?? "Default Group";

        public GroupType Type => _groupType ?? GroupType.Global;

        public GroupAuthType AuthType => _authType ?? GroupAuthType.None;

        public string Link => _link ?? Name;

        public IdentityType IdentityType => IdentityType.Group;
        
        public string Salt { get; }
        
        public IGroupIdentityManager Manager { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        public GroupIdentity(IGroupIdentityManager manager, uint id, string name, GroupType groupType, 
            uint owner = 0, GroupAuthType authType = GroupAuthType.None, string salt = null) {
            _id = id;
            _name = name;
            _groupType = groupType;
            _authType = authType;
            _owner = owner;
            _link = $"<link=group|{id}>{name}</link>";
            Manager = manager;
            Salt = salt;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the rank of the given user within the group.
        /// </summary>
        /// <param name="user">The user that you want to get the rank of.</param>
        /// <returns>The user's rank or <see cref="short.MinValue"/> if the user does not exist or is not within the
        /// group.</returns>
        public short GetRank(uint user) => Manager.GetRank(Id,user);

        /// <summary>
        /// This method is used to check if the given user is a member of the group.
        /// </summary>
        /// <param name="user">The user that you want to check.</param>
        /// <returns>True if the user is a member in the group, otherwise false.</returns>
        public bool IsMember(uint user) => Manager.IsMember(Id, user);

        /// <summary>
        /// This method is used to get the user's status within the group.
        /// </summary>
        /// <param name="user">The user that you want to check.</param>
        /// <returns>The status of the user within the group or <see cref="MemberStatus.None"/> if not in the group.
        /// </returns>
        public MemberStatus GetStatus(uint user) => Manager.GetStatus(Id, user);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operators //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Converts the <see cref="GroupIdentity"/> to an int.
        /// </summary>
        /// <param name="identity">The <see cref="GroupIdentity"/> instance.</param>
        public static implicit operator uint(GroupIdentity identity) { return identity.Id; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}