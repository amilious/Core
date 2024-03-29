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

using FishNet.Serializing;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.FishNet.Groups {
    
    /// <summary>
    /// This class is used by the <see cref="FishNet"/> serializerMethods when sending user id's to the client.
    /// </summary>
    public static class GroupIdentitySerializer {
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to write a group identity's values.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="identity">The identity that you want to write.</param>
        public static void WriteGroupIdentity(this Writer writer, GroupIdentity identity) {
            writer.WriteUInt32(identity.Id, AutoPackType.Unpacked);
            writer.WriteString(identity.Name);
            writer.WriteUInt32(identity.OwnerId, AutoPackType.Unpacked);
            writer.WriteByte((byte)identity.Type);
            writer.WriteByte((byte)identity.AuthType);
            writer.WriteString(identity.Salt);
        }

        /// <summary>
        /// This method is used to read a group identity's values.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Group Identity build from the reader's values.</returns>
        public static GroupIdentity ReadGroupIdentity(this Reader reader) {
            var id = reader.ReadUInt32(AutoPackType.Unpacked);
            var name = reader.ReadString();
            var owner = reader.ReadUInt32(AutoPackType.Unpacked);
            var type = (GroupType)reader.ReadByte();
            var auth = (GroupAuthType)reader.ReadByte();
            var salt = reader.ReadString();
            var manager = reader.NetworkManager.GetGroupManager();
            return new GroupIdentity(manager, id, name, type, owner, auth, salt);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}