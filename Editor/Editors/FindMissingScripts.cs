using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using Amilious.Core.Extensions;
using System.Collections.Generic;

namespace Amilious.Core.Editor.Editor.Editors {
    
    /// <summary>
    /// This class is used to find missing scripts.
    /// This class is a modified version of the script found here
    /// https://gigadrillgames.com/2020/03/19/finding-missing-scripts-in-unity/
    /// </summary>
    public class FindMissingScripts : EditorWindow {
        
        private static int _goCount = 0, _componentsCount = 0, _missingCount = 0;
        private static readonly Dictionary<string, MissingInfo> MissingScripts = new Dictionary<string, MissingInfo>();
        private static Vector2 _scroll = new Vector2(0,0);
        private static FindMissingScripts _instance;
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        [MenuItem("Amilious/Editor/Find Missing Scripts")]
        public static void ShowWindow() {
            if(_instance==null) _instance = GetWindow<FindMissingScripts>(false, "Find Missing Scripts");
            _instance.Focus();
        }

        public void OnGUI() {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if(GUILayout.Button("Find In Selected", EditorStyles.toolbarButton))FindInSelected();
            if(GUILayout.Button("Find All", EditorStyles.toolbarButton))FindAll();
            GUILayout.FlexibleSpace();
            GUILayout.Label($"Time: {Stopwatch.Elapsed.ToEasyReadString(abbreviations:true)}", EditorStyles.toolbarButton);
            GUILayout.Label($"Objects: {_goCount}", EditorStyles.toolbarButton);
            GUILayout.Label($"Components: {_componentsCount}", EditorStyles.toolbarButton);
            GUILayout.Label($"Missing: {_missingCount}", EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Label("Click \"Find In Selected\" or \"Find All\" to search for missing scripts.\n"
                +"Click on the blue links to open the game object in the inspector!", EditorStyles.helpBox);   
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
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
            _componentsCount = 0;
            _goCount = 0;
            _missingCount = 0;
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

        public static IEnumerable<Object> LoadAllAssetsAtPath(string assetPath) {
            return typeof(SceneAsset) == AssetDatabase.GetMainAssetTypeAtPath(assetPath) ?
                new[] {AssetDatabase.LoadMainAssetAtPath(assetPath)} : AssetDatabase.LoadAllAssetsAtPath(assetPath);
        }

        private static void FindInSelected() {
            var go = Selection.gameObjects;
            _goCount = 0;
            _componentsCount = 0;
            _missingCount = 0;
            MissingScripts.Clear();
            Stopwatch.Restart();
            foreach (var g in go) FindInGo(g);
            Stopwatch.Stop();
        }

        private static void FindInGo(GameObject g) {
            _goCount++;
            var components = g.GetComponents<Component>();
            for (var i = 0; i < components.Length; i++) {
                _componentsCount++;
                if(components[i] != null) continue;
                _missingCount++;
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