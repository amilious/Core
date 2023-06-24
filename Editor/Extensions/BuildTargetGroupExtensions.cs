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
using UnityEditor;

namespace Amilious.Core.Editor.Extensions {
    
    public static class BuildTargetGroupExtensions {

        /// <summary>
        /// This field is used to cache the build targets.
        /// </summary>
        private static BuildTarget[] _buildTargets;

        /// <summary>
        /// This method is used to check if the provided group is currently supported.
        /// </summary>
        /// <param name="targetGroup">The group that you want to check.</param>
        /// <returns>True if the given group is supported, otherwise false.</returns>
        public static bool IsSupported(this BuildTargetGroup targetGroup) {
            _buildTargets ??= (BuildTarget[])Enum.GetValues(typeof(BuildTarget));
            foreach (var target in _buildTargets)
                if (BuildPipeline.IsBuildTargetSupported(targetGroup, target)) return true;
            return false;
        }
        
    }
}