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
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Amilious.Core.Attributes;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Editor.Editors.Tabs;
using Amilious.Core.Editor.Editors.Links;

namespace Amilious.Core.Editor.Editors {
    
    
    /// <summary>
    /// This is the base class for the amilious editor.
    /// </summary>
    public class AmiliousEditor : UnityEditor.Editor {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string UNITY_ASSET_STORE_ICON = "AssetStore Icon";
        public const string WEBSITE_ICON = "BuildSettings.Web";
        public const string SCRIPT = "m_Script";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        public TabController TabController {
            get {
                _tabController ??= new TabController(this);
                return _tabController;
            }
        }
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static Type _renderType;
        private static MethodInfo _renderMethod;
        private static bool _initializedGetSpritePreview;
        private GUIStyle _style;
        private bool _initialized;
        private readonly List<string> _dontDraw = new List<string>();
        private readonly List<string> _disabled = new List<string>();
        private readonly List<LinkInfo> _links = new List<LinkInfo>();
        private readonly Dictionary<string, int> _memberIndexes = new Dictionary<string, int>();
        private readonly List<AmiButtonAttribute> _buttons = new List<AmiButtonAttribute>();
        private List<AmiHelpBoxAttribute> _helpBoxAttributes = new List<AmiHelpBoxAttribute>();
        private TabController _tabController;
        
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public override void OnInspectorGUI() {
            InitializeAmiliousEditor();
            
            TabController.ClearDraw();
            //_drawnTabs.Clear();
            DrawLinks();
            DrawHelpBoxes();
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            BeforeDefaultDraw();
            // draw default properties
            var iterator = serializedObject.GetIterator();
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren)) {
                enterChildren = false;
                if (!_dontDraw.Contains<string>(iterator.name)) {
                    if (iterator.hasChildren && iterator.GetAttributes<AmiModifierAttribute>()
                            .Any(a => a.ShouldHide(iterator))) continue;
                    var disable = _disabled.Contains(iterator.name)||iterator
                        .GetAttributes<AmiModifierAttribute>()
                        .Any(a => a.ShouldDisable(iterator));
                    if(disable) EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(iterator, true);
                    if(disable) EditorGUI.EndDisabledGroup();
                }
                else {
                    //skipped drawing the property
                    TabController.TryDrawTabGroup(iterator.name);
                    //DrawTabGroup(iterator.name);
                }
            }
            //draw the buttons
            foreach(var button in _buttons) {
                if(button.Modifiers.Any(x => x.ShouldHide(serializedObject))) continue;
                var disable = button.Modifiers.Any(x => x.ShouldDisable(serializedObject));
                disable = disable || button.OnlyWhenPlaying && !Application.isPlaying;
                if(disable)EditorGUI.BeginDisabledGroup(true);
                if(GUILayout.Button(button.Name)) {
                    if (button.MethodInfo.IsStatic) { button.MethodInfo.Invoke(null, null); }
                    else { button.MethodInfo.Invoke(target, null); }
                    serializedObject.UpdateIfRequiredOrScript();
                }
                if(disable)EditorGUI.EndDisabledGroup();
            }
            //end draw default properties
            AfterDefaultDraw();
            if (EditorGUI.EndChangeCheck()){ serializedObject.ApplyModifiedProperties();}
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get a Texture2D for the given sprite.
        /// </summary>
        /// <param name="icon">The sprite that you want to make a texture from.</param>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>A Texture2D created from the given sprite.</returns>
        protected static Texture2D GetSpritePreview(Sprite icon, Color color, int width, int height) {
            if(!_initializedGetSpritePreview) {
                _initializedGetSpritePreview = true;
                if(_renderType==null) _renderType = GetType("UnityEditor.SpriteUtility");
                //_renderType ??= GetType("UnityEditor.SpriteUtility");
                if(_renderType == null) return null;
                if(_renderMethod==null) _renderMethod = _renderType.GetMethod("RenderStaticPreview",
                    new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
                /*_renderMethod ??= _renderType.GetMethod("RenderStaticPreview",
                    new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });*/
            }
            if(_renderMethod == null) return null;
            return _renderMethod.Invoke("RenderStaticPreview", parameters:
                new object[] { icon, color, width, height }) as Texture2D;
        }

        /// <summary>
        /// This method is used to skip drawing a property with the default property drawer.
        /// </summary>
        /// <param name="propertyName">The names of the properties that you want to skip drawing.</param>
        protected void SkipPropertyDraw(params string[] propertyName) {
            foreach(var property in propertyName)
                if(!_dontDraw.Contains(property))_dontDraw.Add(property);
        }

        /// <summary>
        /// This method is used to skip drawing a property with the default property drawer.
        /// </summary>
        /// <param name="property">The properties that you want to skip drawing.</param>
        public void SkipPropertyDraw(params SerializedProperty[] property) =>
            SkipPropertyDraw(property.Select(x => x.name).ToArray());

        protected void DisableProperty(params string[] propertyName) {
            foreach(var property in propertyName)
                if(!_disabled.Contains(property))_disabled.Add(property);
        }

        protected void DisableProperty(params SerializedProperty[] property) {
            DisableProperty(property.Select(x => x.name).ToArray());
        }
        
        protected void EnableProperty(params string[] propertyName) {
            foreach(var property in propertyName)
                if(_disabled.Contains(property))_disabled.Remove(property);
        }

        protected void EnableProperty(params SerializedProperty[] property) {
            EnableProperty(property.Select(x => x.name).ToArray());
        }
        
        /// <summary>
        /// This method is called before drawing the default properties.
        /// </summary>
        protected virtual void BeforeDefaultDraw(){}
        
        /// <summary>
        /// This method is called after drawing the default properties.
        /// </summary>
        protected virtual void AfterDefaultDraw() {}
        
        /// <summary>
        /// This method is called to initialize data for the editor.
        /// </summary>
        protected virtual void Initialize() { }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        private void OnEnable() {
            AmiliousCoreEditor.OnShowScriptsChanged += ShowScriptsChanged;
        }

        private void OnDisable() {
            AmiliousCoreEditor.OnShowScriptsChanged -= ShowScriptsChanged;
        }

        private void ShowScriptsChanged(bool showScripts) {
            var current = _dontDraw.Contains(SCRIPT);
            if(showScripts && current) _dontDraw.Remove(SCRIPT);
            if(!showScripts && !current) _dontDraw.Add(SCRIPT);
            Repaint();
        }
        
        /// <summary>
        /// This method is used to add a link button to the top of the inspector.
        /// </summary>
        /// <param name="toolTip">The tooltip that you want to used for the button.</param>
        /// <param name="iconResourcePath">The icon that you want to use for the button.</param>
        /// <param name="link">The url that should be triggered when the button is clicked.</param>
        /// <param name="linkName">The text displayed on the button.</param>
        protected void AddLinkButton(string toolTip, string iconResourcePath, string link, string linkName = null) {
            var icon = Resources.Load<Texture>(iconResourcePath);
            if(icon==null) icon = EditorGUIUtility.IconContent(iconResourcePath).image;
            //icon ??= EditorGUIUtility.IconContent(iconResourcePath).image;
            if(icon != null) { AddLinkButton(toolTip, icon, link, linkName); return; }
            Debug.LogErrorFormat("Unable to load a Texture from the path \"{0}\"!",iconResourcePath);
        }

        /// <summary>
        /// This method is used to add a link button to the top of the inspector.
        /// </summary>
        /// <param name="toolTip">The tooltip that you want to used for the button.</param>
        /// <param name="icon">The icon that you want to use for the button.</param>
        /// <param name="link">The url that should be triggered when the button is clicked.</param>
        /// <param name="linkName">The text displayed on the button.</param>
        protected void AddLinkButton(string toolTip, Texture icon, string link, string linkName = null) {
            if(string.IsNullOrWhiteSpace(link)||icon==null) return;
            _links.Add(new LinkInfo{Link = link, 
                GUIContent = new GUIContent(linkName, icon, toolTip)});
        }
        
        /// <summary>
        /// This method is used to sort tab items based on priority.
        /// </summary>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <returns>The sorted value.</returns>
        private static int SortTabs(TabProperty a, TabProperty b) {
            return a.Order - b.Order;
        }

        /// <summary>
        /// This method is called to handle the drawing of links.
        /// </summary>
        private void DrawLinks() {
            if(_links.Count == 0) return;
            GUILayout.Space(5);
            GUILayout.BeginHorizontal("",GUIStyle.none, GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            //buttons
            var options = new [] {GUILayout.ExpandWidth(false), GUILayout.MaxHeight(22) };
            foreach(var link in _links) 
                if(GUILayout.Toggle(false,link.GUIContent,_style,options)) Application.OpenURL(link.Link);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawHelpBoxes() {
            foreach(var att in _helpBoxAttributes) {
                if(!att.ShouldShowHelp(serializedObject))continue;
                EditorGUILayout.HelpBox(att.Message,att.MessageType);
            }
        }
        
        /// <summary>
        /// This method is used to get a type based on the passed string.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The type.</returns>
        private static Type GetType(string typeName) {
            var type = Type.GetType(typeName);
            if(type!=null) return type;
            if(typeName.Contains(".")) {
                var assemblyName = typeName.Substring(0,typeName.IndexOf('.'));
                //var assemblyName = typeName[..typeName.IndexOf('.')];
                var assembly = Assembly.Load(assemblyName);
                if(assembly==null) return null;
                type=assembly.GetType(typeName);
                if(type!=null) return type;
            }
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach(var assemblyName in referencedAssemblies) {
                var assembly = Assembly.Load(assemblyName);
                if(assembly == null) continue;
                type=assembly.GetType(typeName);
                if(type!=null) return type;
            }
            return null;
        }
        
        /// <summary>
        /// When unity validates this inspector it should reinitialize.
        /// </summary>
        private void OnValidate() { _initialized = false; }

        /// <summary>
        /// This method is called to initialize the editor.
        /// </summary>
        private void InitializeAmiliousEditor() {
            if(_initialized) return;
            _initialized = true;
            _dontDraw.Clear();
            TabController.Reset();
            _links.Clear();
            _style = new GUIStyle { fixedHeight = 12, alignment = TextAnchor.MiddleLeft,
                fontSize = 10, fontStyle = FontStyle.Bold,
                //_style.fixedWidth = 85;
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { textColor = Color.white }
            };
            //hide the script
            _disabled.Add(SCRIPT);
            if(!AmiliousCoreEditor.ShowScripts && !_dontDraw.Contains(SCRIPT)) _dontDraw.Add(SCRIPT);
            //check link attributes
            var linkAttributes = 
                target.GetType().GetCustomAttributes<AmiLinkAttribute>(true);
            _helpBoxAttributes = target.GetType().GetCustomAttributes<AmiHelpBoxAttribute>(true).ToList();
            foreach(var attribute in linkAttributes)
                AddLinkButton(attribute.ToolTip, attribute.IconResourcePath, attribute.Link, attribute.LinkName);
            //check hide property attributes
            foreach(var hide in target.GetType().GetCustomAttributes<AmiHideAttribute>())
                _dontDraw.Add(hide.PropertyName);
            // Get all the methods, public and non-public, on the instance.
            var methods = target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo method in methods) {
                // Check if the method has the AmiButtonAttribute, returns void, and takes no parameters.
                var buttonAtt = (AmiButtonAttribute)method.GetCustomAttribute(typeof(AmiButtonAttribute), false);
                if(buttonAtt==null) continue;
                if(method.ReturnType != typeof(void))continue;
                if(method.GetParameters().Length != 0)continue;
                var tab = method.GetCustomAttribute<AmiTabAttribute>();
                buttonAtt.MethodInfo = method;
                buttonAtt.Modifiers = method.GetCustomAttributes<AmiModifierAttribute>(true).ToArray();
                if(tab!=null) TabController.AddToGroup(tab,buttonAtt);
                else _buttons.Add(buttonAtt);
            }
            //get tabs
            var iterator = serializedObject.GetIterator( );
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren)) {
                enterChildren = false;
                var attributes = 
                    iterator.GetAttributes(typeof(AmiTabAttribute),false).Cast<AmiTabAttribute>();
                foreach(var attribute in attributes) 
                    TabController.AddToGroup(new TabProperty(attribute,serializedObject.FindProperty(iterator.name)));
            }
            Initialize();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}