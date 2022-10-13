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

namespace Amilious.Core.Users {
    
    /// <summary>
    /// This struct is used to represent a user identity.
    /// </summary>
    public readonly struct UserIdentity {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string USER_NAME_KEY = "_userName_";
        public const string AUTHORITY_KEY = "_authority_";
        public const string PASSWORD_SALT_KEY = "_salt_";
        public const string PASSWORD_KEY = "_password_";
        private const int RESERVED_ID = int.MinValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly int? _id;
        private readonly string _userName;
        private readonly string _link;
        private readonly int? _authority;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        public static UserIdentity Console = new UserIdentity("Amilious Console");
        
        /// <summary>
        /// This is the server's identity.
        /// </summary>
        public static UserIdentity DefaultUser = new UserIdentity("User");

        public static UserIdentity Server = new UserIdentity("Server");
        
        /// <summary>
        /// This is the default user.
        /// </summary>
        public static UserIdentity Default = default(UserIdentity);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the user's identification number.
        /// </summary>
        public int Id => _id ?? int.MinValue;

        /// <summary>
        /// This property contains the user's user name.
        /// </summary>
        public string UserName => _userName ?? "Amilious Console";

        /// <summary>
        /// This property contains a TMP link for the user.
        /// </summary>
        public string Link => _link ?? UserName;

        /// <summary>
        /// This property is true if the identity is a network user.
        /// </summary>
        public bool IsNetworkUser { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new user identity.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="userName">The display name of the user.</param>
        public UserIdentity(int id, string userName) {
            _id = id;
            _userName = userName;
            _authority = null;
            _link = $"<link=user|{id}>{userName}</link>";
            IsNetworkUser = true;
        }

        /// <summary>
        /// This constructor is used to create a new user identity with the given authority.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="userName">The display name of the user.</param>
        /// <param name="authority">The user's server authority.</param>
        public UserIdentity(int id, string userName, int authority) {
            _id = id;
            _userName = userName;
            _authority = authority;
            _link = $"<link=user|{id}>{userName}</link>";
            IsNetworkUser = true;
        }

        private UserIdentity(string displayName) {
            _id = RESERVED_ID;
            _userName = displayName;
            _authority = null;
            _link = displayName;
            IsNetworkUser = false;
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
            if(!_authority.HasValue) return false;
            return orGreater? _authority.Value<=required : _authority.Value==required;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}