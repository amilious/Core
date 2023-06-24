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

using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using Amilious.Core.Extensions;
using System.Collections.Generic;

namespace Amilious.Core.Editor.Windows {
    
    /// <summary>
    /// This class is used to find missing scripts.
    /// This class is a modified version of the script found here
    /// https://gigadrillgames.com/2020/03/19/finding-missing-scripts-in-unity/
    /// </summary>
    public class FindMissingScripts : EditorWindow {
        
        private static int GoCount, ComponentsCount, MissingCount;
        private static readonly Dictionary<string, MissingInfo> MissingScripts = new Dictionary<string, MissingInfo>();
        private static Vector2 Scroll = new Vector2(0,0);
        private static FindMissingScripts Instance;
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        [MenuItem("Amilious/Editor/Find Missing Scripts")]
        public static void ShowWindow() {
            if(Instance==null) Instance = GetWindow<FindMissingScripts>(false, "Find Missing Scripts");
            Instance.Focus();
        }

        public void OnGUI() {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if(GUILayout.Button("Find In Selected", EditorStyles.toolbarButton))FindInSelected();
            if(GUILayout.Button("Find All", EditorStyles.toolbarButton))FindAll();
            GUILayout.FlexibleSpace();
            GUILayout.Label($"Time: {Stopwatch.Elapsed.ToEasyReadString(abbreviations:true)}", EditorStyles.toolbarButton);
            GUILayout.Label($"Objects: {GoCount}", EditorStyles.toolbarButton);
            GUILayout.Label($"Components: {ComponentsCount}", EditorStyles.toolbarButton);
            GUILayout.Label($"Missing: {MissingCount}", EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Label("Click \"Find In Selected\" or \"Find All\" to search for missing scripts.\n"
                +"Click on the blue links to open the game object in the inspector!", EditorStyles.helpBox);   
            
            Scroll = EditorGUILayout.BeginScrollView(Scroll);
            foreach(var missing in MissingScripts) {
                if(GUILayout.Button($"  {missing.Key} ({missing.Value.Missing.Count})", EditorStyles.linkLabel)) {
                    Selection.activeGameObject = missing.Value.GameObject;
                    SceneView.FrameLastActiveSceneView();
                }
                GUILayout.Label($"    missing components: {string.Join(", ", missing.Value.Missing)}");
            }
            EditorGUILayout.EndScrollView();
        }

        private static void FindAll() {
            ComponentsCount = 0;
            GoCount = 0;
            MissingCount = 0;
            MissingScripts.Clear();
            Stopwatch.Restart();
            var assetsPaths = AssetDatabase.GetAllAssetPaths();
            foreach (var assetPath in assetsPaths) {
                var data = LoadAllAssetsAtPath(assetPath);
                foreach (Object o in data) {
                    if(o == null) continue;
                    if (o is GameObject gameObject) 
                        FindInGo(gameObject);
                }
            }
            Stopwatch.Stop();
        }

        private static IEnumerable<Object> LoadAllAssetsAtPath(string assetPath) {
            return typeof(SceneAsset) == AssetDatabase.GetMainAssetTypeAtPath(assetPath) ?
                new[] {AssetDatabase.LoadMainAssetAtPath(assetPath)} : AssetDatabase.LoadAllAssetsAtPath(assetPath);
        }

        private static void FindInSelected() {
            var go = Selection.gameObjects;
            GoCount = 0;
            ComponentsCount = 0;
            MissingCount = 0;
            MissingScripts.Clear();
            Stopwatch.Restart();
            foreach (var g in go) FindInGo(g);
            Stopwatch.Stop();
        }

        private static void FindInGo(GameObject g) {
            GoCount++;
            var components = g.GetComponents<Component>();
            for (var i = 0; i < components.Length; i++) {
                ComponentsCount++;
                if(components[i] != null) continue;
                MissingCount++;
                var s = g.name;
                var t = g.transform;
                while (t.parent != null)                    {
                    var parent = t.parent;
                    s = parent.name + "/" + s;
                    t = parent;
                }
                if(!MissingScripts.TryGetValue(s, out var missingInfo)) {
                    missingInfo = new MissingInfo { GameObject = g };
                    MissingScripts.Add(s,missingInfo);
                }
                missingInfo.Missing.Add(i);
            }
            foreach (Transform childT in g.transform) FindInGo(childT.gameObject);
        }

        private class MissingInfo {
            public GameObject GameObject { get; set; }
            public readonly List<int> Missing = new List<int>();
        }
        
    }
    
}