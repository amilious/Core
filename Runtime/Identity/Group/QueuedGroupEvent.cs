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

using Amilious.Core.Identity.Group.Data;

namespace Amilious.Core.Identity.Group {
    
    public class QueuedGroupEvent {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the type of the event.
        /// </summary>
        public GroupEvent Type { get; }
        
        /// <summary>
        /// This property contains the new group member data for the event.
        /// </summary>
        public GroupMemberData Data { get; }
        
        /// <summary>
        /// This property can be used to save an extra id for the event based on the type.
        /// </summary>
        public uint? ExtraId { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to queue an events data when the event is not ready to be triggered yet.
        /// </summary>
        /// <param name="type">The type of the event.</param>
        /// <param name="data">The group member data associated with the event.</param>
        /// <param name="extraId">(optional)An extra id that can be used for an event.</param>
        public QueuedGroupEvent(GroupEvent type, GroupMemberData data, uint? extraId = null) {
            Type = type;
            Data = data;
            ExtraId = extraId;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}