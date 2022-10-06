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

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly int? _id;
        private readonly string _displayName;
        /*private readonly string _link;*/

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
                              
        /// <summary>
        /// This is the server's identity.
        /// </summary>
        public static UserIdentity Server = new UserIdentity(-2, "Server");
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The user's identification number.
        /// </summary>
        public int Id => _id ?? -1;

        /// <summary>
        /// This user's display name.
        /// </summary>
        public string DisplayName => _displayName ?? "User";

        /// <summary>
        /// This property contains the user's link.
        /// </summary>
        /*public string Link => _link ?? ZString.StyleFormat(StyleFormat.User, 
            "<link=\"user|{0}\">{1}</link>", Id, DisplayName);*/
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new UserId.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="displayName">The display name of the user.</param>
        public UserIdentity(int id, string displayName) {
            _id = id;
            _displayName = displayName;
            /*_link = ZString.StyleFormat(id == -2 ? StyleFormat.Server : StyleFormat.User, 
                "<link=\"user|{0}\">{1}</link>", id, displayName);*/
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}