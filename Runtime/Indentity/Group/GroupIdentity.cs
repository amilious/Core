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

namespace Amilious.Core.Indentity.Group {
    
    /// <summary>
    /// This stuct is used to represent the identity of a group.
    /// </summary>
    public readonly struct GroupIdentity : IIdentity {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string GROUP_NAME_KEY = "_group_name_";
        public const string GROUP_TYPE_KEY = "_group_type_";
        public const string GROUP_PASSWORD_KEY = "_group_password_";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private readonly int? _id;
        private readonly string _name;
        private readonly GroupType? _groupType;
        public readonly string _link;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Instances ///////////////////////////////////////////////////////////////////////////////////////

        public static readonly GroupIdentity Default = default;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public int Id => _id ?? int.MinValue;

        public string Name => _name ?? "Default Group";

        public GroupType GroupType => _groupType ?? GroupType.Global;

        public string Link => _link ?? Name;

        public IdentityType IdentityType => IdentityType.Group;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        public GroupIdentity(int id, string name, GroupType groupType) {
            _id = id;
            _name = name;
            _groupType = groupType;
            _link = $"<link=group|{id}>{name}</link>";
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}