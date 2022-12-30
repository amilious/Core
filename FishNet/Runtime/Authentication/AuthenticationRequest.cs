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

namespace Amilious.Core.FishNet.Authentication {
    
    /// <summary>
    /// This struct is used for an authentication request from the server to the client.
    /// </summary>
    public struct AuthenticationRequest {
        
        /// <summary>
        /// This property contains the user's id.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// This property is true if the user was just automatically created.
        /// </summary>
        public bool NewUser { get; set; }
        
        /// <summary>
        /// This property contains the request id.
        /// </summary>
        public int RequestId { get; set; }
        
        /// <summary>
        /// This property contains the user's username.
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// This property contains the request type.
        /// </summary>
        public AuthenticationRequestType RequestType { get; set; }
        
    }
    
}