﻿/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

namespace Amilious.Core {
    
    /// <summary>
    /// This enum is a wrapper for UnityEditor.MessageType that can be used without importing UnityEditor
    /// </summary>
    public enum HelpBoxType {
        
        /// <summary>
        /// used to represent no message style
        /// </summary>
        None, 
        
        /// <summary>
        /// used to represent the info message style
        /// </summary>
        Info, 
        
        /// <summary>
        /// used to represent the warning message style
        /// </summary>
        Warning, 
        
        /// <summary>
        /// used to represent the error message style
        /// </summary>
        Error
    }

    public static class HelpBoxTypeExtension{

        #if UNITY_EDITOR
        
        public static UnityEditor.MessageType ToMessageType(this HelpBoxType type) {
            switch(type) {
                case HelpBoxType.None: return UnityEditor.MessageType.None;
                case HelpBoxType.Info: return UnityEditor.MessageType.Info;
                case HelpBoxType.Warning: return UnityEditor.MessageType.Warning;
                case HelpBoxType.Error: return UnityEditor.MessageType.Error;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
        }
        
        #endif
        
    }
}