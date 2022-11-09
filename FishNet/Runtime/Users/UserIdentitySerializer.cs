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
using Amilious.Core.Identity.User;
using Amilious.Core.Indentity.User;
using FishNet.Serializing;

namespace Amilious.Core.FishNet.Users {
    
    /// <summary>
    /// This class is used by the <see cref="FishNet"/> serializerMethods when sending user id's to the client.
    /// </summary>
    public static class UserIdentitySerializer {
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to write a user identity's values.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="identity">The identity that you want to write.</param>
        public static void WriteUserIdentity(this Writer writer, UserIdentity identity) {
            writer.WriteByte((byte)identity.UserType);
            if(identity.UserType != UserType.User) return;
            writer.WriteInt32(identity.Id);
            writer.WriteString(identity.Name);
        }

        /// <summary>
        /// This method is used to read a user identity's values.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>User Identity build from the reader's values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if invalid identity type.</exception>
        public static UserIdentity ReadUserIdentity(this Reader reader) {
            var type = (UserType)reader.ReadByte();
            switch(type) {
                case UserType.Server: return UserIdentity.Server;
                case UserType.AmiliousConsole: return UserIdentity.Console;
                case UserType.DefaultUser: return  UserIdentity.DefaultUser;
                case UserType.User: return new UserIdentity(reader.ReadInt32(),reader.ReadString());
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}