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

namespace Amilious.Core.Extensions {
    
    public static class MonoBehaviorExtensions {

        /// <summary>
        /// This method is used to get an cache a component.
        /// </summary>
        /// <param name="behavior">The MonoBehavior.</param>
        /// <param name="cache">The cache for the component.</param>
        /// <typeparam name="T">The type of component that you want to get.</typeparam>
        /// <returns>The found or created component.</returns>
        public static T GetCacheComponent<T>(this MonoBehaviour behavior, ref T cache) where T : Component {
            if(behavior == null) return null;
            if(cache != null) return cache;
            cache = behavior.GetComponent<T>();
            return cache;
        } 
        
        /// <summary>
        /// This method is used to get a component from the game object or create it if it does not exist.
        /// </summary>
        /// <param name="behavior">The MonoBehavior.</param>
        /// <typeparam name="T">The type of component that you want to get.</typeparam>
        /// <returns>The found or created component.</returns>
        public static T GetOrAddComponent<T>(this MonoBehaviour behavior) where T : Component {
            return behavior.gameObject.GetOrAddComponent<T>();
        }

        /// <summary>
        /// This method is used to get a component from the game object or create it if it does not exist.
        /// </summary>
        /// <param name="behavior">The MonoBehavior.</param>
        /// <typeparam name="T">The type of component that you want to get.</typeparam>
        /// <typeparam name="T2">The extended version of T that should be created if no T is found.</typeparam>
        /// <returns>The found or created component.</returns>
        public static T GetOrAddComponent<T, T2>(this MonoBehaviour behavior) where T2 : T where T: Component {
            return behavior.gameObject.GetComponent<T>() ?? behavior.gameObject.AddComponent<T2>();
        }

        /// <summary>
        /// This method is used to get a component from the game object or create it if it does not exist.  If the
        /// cache exists it will be returned instead.  This method will also set the cache value.
        /// </summary>
        /// <param name="behavior">The MonoBehavior.</param>
        /// <param name="cache">The cache for the component.</param>
        /// <typeparam name="T">The type of component that you want to get.</typeparam>
        /// <returns>The found or created component.</returns>
        public static T GetOrAddComponent<T>(this MonoBehaviour behavior, ref T cache) where T : Component {
            if(cache != null) return cache;
            cache = behavior.GetComponent<T>() ?? behavior.gameObject.AddComponent<T>();
            return cache;
        }

        /// <summary>
        /// This method is used to get a component from the game object or create it if it does not exist.  If the
        /// cache exists it will be returned instead.  This method will also set the cache value.
        /// </summary>
        /// <param name="behavior">The MonoBehavior.</param>
        /// <param name="cache">The cache for the component.</param>
        /// <typeparam name="T">The type of component that you want to get.</typeparam>
        /// <typeparam name="T2">The extended version of T that should be created if no T is found.</typeparam>
        /// <returns>The found or created component.</returns>
        public static T GetOrAddComponent<T, T2>(this MonoBehaviour behavior, ref T cache)
            where T2 : T where T : Component {
            if(cache != null) return cache;
            cache = behavior.GetComponent<T>() ?? behavior.gameObject.AddComponent<T2>();
            return cache;
        }
        
        /// <summary>
        /// This method is used to get a component from the game object or create it if it does not exist.  If the
        /// cache exists it will be returned instead.  This method will also set the cache value.
        /// </summary>
        /// <param name="behavior">The MonoBehavior.</param>
        /// <param name="cache">The cache for the component.</param>
        /// <typeparam name="T">The type of component that you want to get.</typeparam>
        /// <typeparam name="T2">The extended version of T that should be created if no T is found.</typeparam>
        /// <returns>The found or created component.</returns>
        public static T GetOrAddComponent<T, T2>(this MonoBehaviour behavior, ref T2 cache)
            where T2 : T where T : Component {
            if(cache != null) return cache;
            cache = behavior.GetComponent<T2>() ?? behavior.gameObject.AddComponent<T2>();
            return cache;
        }
        
    }
    
}