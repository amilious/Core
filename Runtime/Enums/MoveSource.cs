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

namespace Amilious.Core {

    [Serializable]
    public enum MoveSource { BottomLeft = 0, TopLeft = 1, TopRight = 2, BottomRight = 3, Standard =-1, Center = -2 }
    
    public static class PositionType2DExtensions {

        public static Vector2 ToVector(this MoveSource positionType) {
            switch (positionType) {
                case MoveSource.Standard:
                case MoveSource.Center:
                    return new Vector2(0.5f, 0.5f);
                case MoveSource.TopLeft:
                    return new Vector2(0, 1);
                case MoveSource.TopRight:
                    return new Vector2(1, 1);
                case MoveSource.BottomLeft:
                    return new Vector2(0, 0);
                case MoveSource.BottomRight:
                    return new Vector2(1, 0);
                default: throw new ArgumentOutOfRangeException(nameof(positionType), positionType, null);
            }
        }
        
    }
    
}