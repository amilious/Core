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

using FishNet.Broadcast;

namespace Amilious.Core.FishNet.Authentication {
    
    public struct AuthenticationBroadcast : IBroadcast {
        public string UserName;
        public int UserId;
    }

    public struct UserNameTakenBroadcast : IBroadcast {
        public string UserName;
        public string ServerIdentifier;
    }

    public struct PasswordRequestBroadcast : IBroadcast {
        public int UserId;
        public bool NewUser;
        public int RequestId;
        public string Salt;
        public string ServerIdentifier;
    }
    
    public struct PasswordBroadcast : IBroadcast {
        public int UserId;
        public int RequestId;
        public string HashedPassword;
    }

    public struct AuthenticationResultBroadcast : IBroadcast {
        public bool Passed;
        public bool NewUser;
        public int UserId;
        public string ServerIdentifier;
        public string UserName;
        public string Response;
    }

}