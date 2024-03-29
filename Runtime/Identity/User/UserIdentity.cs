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

namespace Amilious.Core.Identity.User {
    
    /// <summary>
    /// This struct is used to represent a user identity.
    /// </summary>
    public readonly struct UserIdentity : IIdentity {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string USER_NAME_KEY = "_user_name_";
        public const string AUTHORITY_KEY = "_authority_";
        public const string PASSWORD_SALT_KEY = "_salt_";
        public const string PASSWORD_KEY = "_password_";
        public const string LAST_DISCONNECTED_KEY = "_last_disconnected_";
        public const string LAST_CONNECTED_KEY = "_last_connected_";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly uint? _id;
        private readonly string _userName;
        private readonly string _link;
        private readonly int? _authority;
        private readonly UserType? _userType;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the console's user identity.
        /// </summary>
        public static UserIdentity Console = new UserIdentity("Amilious Console",UserType.AmiliousConsole);
        
        /// <summary>
        /// This is the default user identity that is used when offline.
        /// </summary>
        public static UserIdentity DefaultUser = new UserIdentity("User",UserType.DefaultUser);

        /// <summary>
        /// This is the server's identity.
        /// </summary>
        public static UserIdentity Server = new UserIdentity("Server",UserType.Server);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the user's identity manager.
        /// </summary>
        public static IUserIdentityManager Manager { get; private set; }
        
        /// <summary>
        /// This property contains the user's identification number.
        /// </summary>
        public uint Id => _id ?? 0;

        /// <summary>
        /// This property contains the user's user name.
        /// </summary>
        public string Name => _userName ?? "Amilious Console";

        /// <summary>
        /// This property contains a TMP link for the user.
        /// </summary>
        public string Link => _link ?? Name;

        /// <summary>
        /// This property contains the type of the identity.
        /// </summary>
        public UserType UserType => _userType ?? UserType.AmiliousConsole;

        /// <inheritdoc />
        public IdentityType IdentityType => IdentityType.User;

        /// <summary>
        /// This property is true if the Manager has been registered and the user is online, otherwise false.
        /// </summary>
        public bool IsOnline => Manager?.IsOnline(Id) ?? false;

        /// <summary>
        /// This property is true if the user is a friend of the local user.
        /// </summary>
        /// <remarks>This property should only be used on the client.</remarks>
        public bool IsFriend => Manager?.Client_IsFriend(Id) ?? false;

        /// <summary>
        /// This property is true if the user is the local user.
        /// </summary>
        /// <remarks>This property should only be used on the client.</remarks>
        public bool IsLocalUser => (Manager is not null) && Manager.Client_GetIdentity() == this;

        /// <summary>
        /// This property is true if the user is blocked by the local user.
        /// </summary>
        /// <remarks>This property should only be used on the client.</remarks>
        public bool IsBlocked => Manager?.Client_IsBlocked(Id) ?? false;

        /// <summary>
        /// This property is true if the local user is waiting for the user to accept friendship.
        /// </summary>
        /// <remarks>This property should only be used on the client.</remarks>
        public bool IsPendingFriendship => Manager?.Client_IsPendingFriendship(Id) ?? false;

        /// <summary>
        /// This property is true if the user is waiting for friendship approval from the local user.
        /// </summary>
        /// <remarks>The property should only be used on the client.</remarks>
        public bool IsRequestingFriendship => Manager?.Client_IsRequestingFriendship(Id) ?? false;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new user identity.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="userName">The display name of the user.</param>
        public UserIdentity(uint id, string userName) {
            _id = id;
            _userName = userName;
            _authority = null;
            _link = $"<link=user|{id}>{userName}</link>";
            _userType = UserType.User;
        }

        /// <summary>
        /// This constructor is used to create a new user identity with the given authority.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="userName">The display name of the user.</param>
        /// <param name="authority">The user's server authority.</param>
        public UserIdentity(uint id, string userName, int authority) {
            _id = id;
            _userName = userName;
            _authority = authority;
            _link = $"<link=user|{id}>{userName}</link>";
            _userType = UserType.User;
        }

        /// <summary>
        /// This constructor is used to create static instances of user identities.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="userType">The identity type.</param>
        private UserIdentity(string displayName, UserType userType) {
            _id = 0;
            _userName = displayName;
            _authority = null;
            _link = displayName;
            _userType = userType;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the user has required authority.
        /// </summary>
        /// <param name="required">The required authority.</param>
        /// <param name="orGreater">If true greater authority will also be accepted.</param>
        /// <returns>True if the user has authority, otherwise false.</returns>
        public bool HasAuthority(int required,bool orGreater = true) {
            if(_authority==-1) return false;
            return orGreater? _authority<=required : _authority==required;
        }
        
        /// <summary>
        /// This method is used to get the authority of the user.
        /// </summary>
        /// <returns>The authority of the user, or -1 if the user does not have authority.</returns>
        public int GetAuthority() => _authority??-1;
        
        /// <summary>
        /// This method is used to check if two identities are equal.
        /// </summary>
        /// <param name="other">The other identity.</param>
        /// <returns>True if the identities are equal, otherwise false.</returns>
        public bool Equals(UserIdentity other) {
            return _id == other._id && 
                   string.Equals(_userName, other._userName, StringComparison.InvariantCulture) && 
                   string.Equals(_link, other._link, StringComparison.InvariantCulture) && 
                   _userType == other._userType;
        }

        /// <summary>
        /// This method is used to check if two objects are equal.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the objects are equal, otherwise false.</returns>
        public override bool Equals(object obj) {
            return obj is UserIdentity other && Equals(other);
        }

        /// <summary>
        /// This method is used to get the hash code of the object.
        /// </summary>
        /// <returns>The hashcode of the object.</returns>
        public override int GetHashCode() {
            unchecked {
                var hashCode = _id.GetHashCode();
                hashCode = (hashCode * 397) ^ (_userName != null ? StringComparer.InvariantCulture.GetHashCode(_userName) : 0);
                hashCode = (hashCode * 397) ^ (_link != null ? StringComparer.InvariantCulture.GetHashCode(_link) : 0);
                hashCode = (hashCode * 397) ^ _userType.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// This method is used to set the identity manger for user identities.
        /// </summary>
        /// <param name="manager">The identity manager.</param>
        /// <returns>True if the manager was set, otherwise false if the manager has already been set.</returns>
        /// <remarks>This should only be called by the identity manager.</remarks>
        public static bool SetIdentityManager(IUserIdentityManager manager) {
            if(Manager != null) return false;
            Manager = manager;
            return true;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Operators //////////////////////////////////////////////////////////////////////////////////////////////
        
        public static bool operator ==(UserIdentity identity, UserIdentity identity2) => identity.Id == identity2.Id && 
            identity.Name.Equals(identity2.Name,StringComparison.InvariantCultureIgnoreCase) && 
            identity.UserType==identity2.UserType&&identity._authority==identity2._authority;
        
        public static bool operator !=(UserIdentity identity, UserIdentity identity2) => identity.Id != identity2.Id || 
            !identity.Name.Equals(identity2.Name,StringComparison.InvariantCultureIgnoreCase) || 
            identity.UserType!=identity2.UserType||identity._authority!=identity2._authority;
        
        public static bool operator ==(UserIdentity identity, int id) => identity.Id == id;
        
        public static bool operator !=(UserIdentity identity, int id) => identity.Id != id;

        public static bool operator ==(UserIdentity identity, string userName) =>
            identity.Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase);
        
        public static bool operator !=(UserIdentity identity, string userName) =>
            identity.Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase);

        public static bool operator >(UserIdentity identity, UserIdentity identity2) {
            if(!identity._authority.HasValue && identity2._authority.HasValue) return false;
            if(!identity2._authority.HasValue) return true;
            //higher value means less authority
            return identity2._authority > identity._authority;
        }
        
        public static bool operator <(UserIdentity identity, UserIdentity identity2) {
            if(!identity._authority.HasValue && identity2._authority.HasValue) return true;
            if(!identity2._authority.HasValue) return false;
            //higher value means less authority
            return identity2._authority < identity._authority;
        }
        
        public static bool operator >=(UserIdentity identity, UserIdentity identity2) {
            if(!identity._authority.HasValue && !identity2._authority.HasValue) return true;
            if(identity._authority.HasValue&&!identity2._authority.HasValue) return true;
            if(!identity._authority.HasValue) return false;
            //higher value means less authority
            return identity2._authority >= identity._authority;
        }
        
        public static bool operator <=(UserIdentity identity, UserIdentity identity2) {
            if(!identity._authority.HasValue && !identity2._authority.HasValue) return true;
            if(!identity._authority.HasValue) return true;
            if(!identity2._authority.HasValue) return false;
            //higher value means less authority
            return identity2._authority <= identity._authority;
        }
        
        /// <summary>
        /// Converts the <see cref="UserIdentity"/> to an int.
        /// </summary>
        /// <param name="identity">The <see cref="UserIdentity"/> instance.</param>
        public static implicit operator uint(UserIdentity identity) { return identity.Id; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}