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

using UnityEngine;
using UnityEngine.UI;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="VertexHelper"/>.
    /// </summary>
    public static class VertexHelperExtension {

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a vertex to a <see cref="VertexHelper"/> at the given position.
        /// </summary>
        /// <param name="vh">The vertex helper.</param>
        /// <param name="uiVertex">The vertex that you want to add to the vertex helper at the given position.</param>
        /// <param name="x">The x position of the vertex.</param>
        /// <param name="y">The y position of the vertex.</param>
        public static void AddVertex(this VertexHelper vh,ref UIVertex uiVertex, float x, float y) {
            if(vh == null) return;
            uiVertex.position = new Vector3(x, y);
            vh.AddVert(uiVertex);
        }
        
        /// <summary>
        /// This method is used to add a vertex to a <see cref="VertexHelper"/> at the given position.
        /// </summary>
        /// <param name="vh">The vertex helper.</param>
        /// <param name="uiVertex">The vertex that you want to add to the vertex helper at the given position.</param>
        /// <param name="x">The x position of the vertex.</param>
        /// <param name="y">The y position of the vertex.</param>
        /// <param name="z">The z position of the vertex.</param>
        public static void AddVertex(this VertexHelper vh,ref UIVertex uiVertex, float x, float y, float z) {
            if(vh == null) return;
            uiVertex.position = new Vector3(x, y, z);
            vh.AddVert(uiVertex);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}