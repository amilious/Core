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
using UnityEngine;
using Amilious.Core.Serializables;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the <see cref="Quaternion"/> struct.
    /// </summary>
    public static class QuaternionExtensions {
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to convert a Quaternion into a SerializableQuaternion.
        /// </summary>
        /// <param name="quaternion">The Quaternion that you want to convert.</param>
        /// <returns>A Serializable version of the given Quaternion.</returns>
        public static SerializableQuaternion ToSerializable(this Quaternion quaternion) {
            return new SerializableQuaternion(quaternion);
        }
        
        public static Quaternion SetAxis(this Quaternion quaternion, int axis, float value) {
            var euler = quaternion.eulerAngles;
            if(axis == 0) euler.x = value;
            else if(axis == 1) euler.y = value;
            else if(axis == 2) euler.z = value;
            else throw new IndexOutOfRangeException("A Quaternion is represented as a three-axis rotation.");
            return Quaternion.Euler(euler);
        }

        public static float GetAxis(this Quaternion quaternion, int axis) {
            var euler = quaternion.eulerAngles;
            if(axis == 0) return euler.x;
            if(axis == 1) return euler.y;
            if(axis == 2) return euler.z;
            throw new IndexOutOfRangeException("A Quaternion is represented as a three-axis rotation.");
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}