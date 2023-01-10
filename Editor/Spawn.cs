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

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Amilious.Core.Editor {
    
    /// <summary>
    /// This class is used for spawning prefabs from an editor script.
    /// </summary>
    public static class Spawn {
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the spawn parent for spawning prefabs.
        /// </summary>
        /// <param name="createdParent">True if the parent was created by the method.</param>
        /// <param name="requireCanvas">If true the parent will contain a canvas.</param>
        /// <returns>The transform of the parent object.</returns>
        public static Transform GetSpawnParent(out bool createdParent, bool requireCanvas = false) {
            createdParent = false;
            var parent = Selection.activeGameObject ? Selection.activeGameObject.transform : null;
            if(parent == null) {
                //if there is no parent find a canvas
                var root = SceneManager.GetActiveScene()
                    .GetRootGameObjects().FirstOrDefault(x => x.GetComponentInChildren<Canvas>()!=null);
                if(root != null) parent = root.transform;
            }
            if(!requireCanvas) return parent;
            if(parent != null && parent.GetComponentInParent<Canvas>()) return parent;
            //setup canvas
            createdParent = true;
            var canvas = new GameObject("Canvas", typeof(Canvas), 
                typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            return canvas.transform;
        }

        /// <summary>
        /// This method is used to spawn a prefab from an editor script.
        /// </summary>
        /// <param name="path">The path of the prefab.</param>
        /// <param name="name">The name of the newly spawned game object.</param>
        /// <param name="requireCanvas">If true the object will be spawned on a canvas.</param>
        /// <returns>The spawned game object.</returns>
        /// <remarks>The prefab path must be within a Resources folder.</remarks>
        public static GameObject SpawnPrefab(string path, string name = null, bool requireCanvas = false) {
            var parent = GetSpawnParent(out var created, requireCanvas);
            var gameObject = Object.Instantiate(Resources.Load<GameObject>(path),parent);
            if(name!=null)gameObject.name = name;
            Selection.activeGameObject = gameObject;
            Undo.RegisterCreatedObjectUndo(created?parent.gameObject:gameObject,$"Create {name}");
            return gameObject;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}