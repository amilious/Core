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
using System.Diagnostics.CodeAnalysis;
using Amilious.Core.Extensions;
using UnityEngine;

namespace Amilious.Core {

    /// <summary>
    /// This enum is used to represent a transform scope.
    /// </summary>
    [Serializable]
    public enum TransformScope {
        
        /// <summary>
        /// This value represents local position, scale, and rotation.
        /// </summary>
        Local, 
        
        /// <summary>
        /// This value represents world position, scale, and rotation.
        /// </summary>
        World
        
    }
    
    public static class TransformScopeExtensions {

        public static bool IsWorld(this TransformScope scope) => scope == TransformScope.World;

        public static bool IsLocal(this TransformScope scope) => scope == TransformScope.Local;

        
        public static Vector3 GetPosition(this TransformScope scope, Transform transform) {
            return scope == TransformScope.World ? transform.position : transform.localPosition;
        }

        public static void SetPosition(this TransformScope scope, Transform transform, Vector3 position) {
            if(scope == TransformScope.World) transform.position = position;
            else transform.localPosition = position;
        }
        
        public static float GetAxisPosition(this TransformScope scope, Transform transform, int axis) {
            return scope.GetPosition(transform).GetAxis(axis);
        }

        public static void SetAxisPosition(this TransformScope scope, Transform transform, int axis, float value) {
            if(scope == TransformScope.World) transform.position = transform.position.SetAxis(axis, value);
            else transform.localPosition = transform.localPosition.SetAxis(axis, value);
        }

        public static Quaternion GetRotation(this TransformScope scope, Transform transform) {
            return scope == TransformScope.World ? transform.rotation : transform.localRotation;
        }

        public static void SetRotation(this TransformScope scope, Transform transform, Quaternion rotation) {
            if(scope == TransformScope.World) transform.rotation = rotation;
            else transform.localRotation = rotation;
        }

    }
    
}