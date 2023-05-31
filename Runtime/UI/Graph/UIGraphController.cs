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
//  Website:        http://www.amilious.comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using TMPro;
using UnityEngine;
using Amilious.Core.UI.Layout;
using Amilious.Core.Extensions;
using Amilious.Core.Attributes;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Amilious.Core.UI.Graph {
    
    [ExecuteAlways]
    [AddComponentMenu(AmiliousCore.GRAPH_CONTEXT_MENU+"UI Graph Controller")]
    public class UIGraphController : AmiliousBehavior {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string TAB_GROUP = "Settings";
        private const string DISPLAY_TAB = "Display";
        private const string GRID_TAB = "Grid";
        private const string X_TAB = "X Settings";
        private const string Y_TAB = "Y Settings";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Ispector Fields ////////////////////////////////////////////////////////////////////////////////////////
        
        [Header("Text")]
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(gridTitleObject))] 
        private string title;
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(xLabelObject))] 
        private string xLabel;
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(xLabelObject))] 
        private string yLabel;
        
        [Header("Color")]
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(gridTitleObject))] 
        [AmiColor]private Color titleColor;
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(xLabelObject))] 
        [AmiColor]private Color xLabelColor;
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(xLabelObject))] 
        [AmiColor]private Color yLabelColor;
        [AmiTab(TAB_GROUP, DISPLAY_TAB), SerializeField, AmiEnableIf(nameof(gridRenderer))] 
        [AmiColor]private Color gridColor;
        
        [AmiTab(TAB_GROUP,GRID_TAB),SerializeField] private UIGridRenderer gridRenderer;
        [AmiTab(TAB_GROUP,GRID_TAB),SerializeField] private TMP_Text gridTitleObject;
        [AmiTab(TAB_GROUP,GRID_TAB),SerializeField] private GameObject labelPrefab;
        [AmiTab(TAB_GROUP,GRID_TAB),SerializeField] private GameObject linePrefab;

        [FormerlySerializedAs("xLabel")] [AmiTab(TAB_GROUP,X_TAB),SerializeField] 
        private TMP_Text xLabelObject;
        [AmiTab(TAB_GROUP,X_TAB),SerializeField] private Transform xGridLabels;
        [AmiTab(TAB_GROUP,X_TAB),SerializeField] private RotationType xRotationType;
        [AmiTab(TAB_GROUP,X_TAB),SerializeField] private bool reverseXLabels = false;

        [FormerlySerializedAs("yLabel")] [AmiTab(TAB_GROUP,Y_TAB),SerializeField] 
        private TMP_Text yLabelObject;
        [AmiTab(TAB_GROUP,Y_TAB),SerializeField] private Transform yGridLabels;
        [AmiTab(TAB_GROUP,Y_TAB),SerializeField] private RotationType yRotationType;
        [AmiTab(TAB_GROUP,Y_TAB),SerializeField] private bool reverseYLabels = true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set the title text.
        /// </summary>
        public string TitleText {
            get => gridTitleObject == null ? null : gridTitleObject.text;
            set {
                if(gridTitleObject == null) return;
                gridTitleObject.text = value;
            }
        }

        /// <summary>
        /// This property is used to get or set the x label text.
        /// </summary>
        public string XLabelText {
            get => xLabelObject == null ? null : xLabelObject.text;
            set {
                if(xLabelObject == null) return;
                xLabelObject.text = value;
            }
        }
        
        /// <summary>
        /// This property is used to get or set the y label text.
        /// </summary>
        public string YLabelText {
            get => yLabelObject == null ? null : yLabelObject.text;
            set {
                if(yLabelObject == null) return;
                yLabelObject.text = value;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void OnEnable() {
            gridRenderer.OnGridUpdated += FixLabels;
            #if UNITY_EDITOR
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextUpdated);
            TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Add(OnTextColorUpdated);
            #endif
        }

        private void OnDisable() {
            gridRenderer.OnGridUpdated -= FixLabels;
            #if UNITY_EDITOR
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextUpdated);
            TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Remove(OnTextColorUpdated);
            #endif
        }

        private void Start() {
            #if UNITY_EDITOR
            SetupEditor();
            #endif
            FixLabels();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR ///////////////////////////////////////////////////////////////////////////////////////////////

        private string _oldTitle;
        private string _oldXLabel;
        private string _oldYLabel;
        private Color _oldTitleColor;
        private Color _oldXLabelColor;
        private Color _oldYLabelColor;
        private Color _oldGridColor;

        private void SetupEditor() {
            if(gridTitleObject != null) { title = gridTitleObject.text; titleColor = gridTitleObject.color; }
            if(yLabelObject != null) { yLabel = yLabelObject.text; yLabelColor = yLabelObject.color; }
            if(xLabelObject != null) { xLabel = xLabelObject.text; xLabelColor = xLabelObject.color; }
            if(gridRenderer != null) gridColor = gridRenderer.color;

            _oldTitle = title;
            _oldXLabel = xLabel;
            _oldYLabel = yLabel;
            _oldTitleColor = titleColor;
            _oldXLabelColor = xLabelColor;
            _oldYLabelColor = yLabelColor;
            _oldGridColor = gridColor;
        }
        
        private void OnTextUpdated(Object obj) {
            if(obj == gridTitleObject) _oldTitle = title = gridTitleObject.text;
            if(obj == xLabelObject) _oldXLabel = xLabel = xLabelObject.text;
            if(obj == yLabelObject) _oldYLabel = yLabel = yLabelObject.text;
        }

        private void OnTextColorUpdated(Object obj) {
            if(obj == gridTitleObject) _oldTitleColor = titleColor = gridTitleObject.color;
            if(obj == xLabelObject) _oldXLabelColor = xLabelColor = xLabelObject.color;
            if(obj == yLabelObject) _oldYLabelColor = yLabelColor = yLabelObject.color;
        }

        private void OnValidate() {
            if(_oldTitle != title && gridTitleObject != null) 
                _oldTitle = gridTitleObject.text = title;
            if(_oldXLabel != xLabel && xLabelObject != null) 
                _oldXLabel = xLabelObject.text = xLabel;
            if(_oldYLabel != yLabel && yLabelObject != null) 
                _oldYLabel = yLabelObject.text = yLabel;
            if(_oldTitleColor != titleColor && gridTitleObject != null) 
                _oldTitleColor = gridTitleObject.color = titleColor;
            if(_oldXLabelColor != xLabelColor && xLabelObject != null) 
                _oldXLabelColor =  xLabelObject.color = xLabelColor;
            if(_oldYLabelColor != yLabelColor && yLabelObject != null) 
                _oldYLabelColor = yLabelObject.color = yLabelColor;
            if(_oldGridColor != gridColor && gridRenderer != null) 
                _oldGridColor = gridRenderer.color = gridColor;
            FixLabels();
        }
        
        #endif /////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        [ContextMenu("Fix Labels")]
        private void FixLabels() {
            UpdateXLabels();
            UpdateYLabels();
        }

        public void ClearLines() {
            if(gridRenderer == null) return;
            gridRenderer.transform.DestroyChildrenImmediate();
        }

        public UILineRenderer AddLine() {
            if(gridRenderer == null||linePrefab==null) return null;
            var go = Instantiate(linePrefab, gridRenderer.gameObject.transform);
            var line = go.GetComponent<UILineRenderer>();
            if(line == null) { DestroyImmediate(go); return null; }
            line.Grid = gridRenderer;
            return line;
        }

        private void UpdateXLabels() {
            if(xGridLabels == null||gridRenderer==null) return;
            var children = gridRenderer.Size.x;
            var step = gridRenderer.Scale.x;
            var count = reverseXLabels?gridRenderer.Origin.x+step*(children-1):gridRenderer.Origin.x;
            foreach(var child in GetLabels(xGridLabels.transform, children)) {
                child.RotationType = xRotationType;
                child.name = $"X Label {count}";
                child.SetText(count.ToString());
                if(reverseXLabels) count -= step;
                else count += step;
            }
        }

        private IEnumerable<LayoutTMProRotator> GetLabels(Transform parent, int count) {
            var foundChildren = 0;
            for(var i=0; i<parent.childCount;i++) {
                var item = parent.GetChild(i).GetComponent<LayoutTMProRotator>();
                if(item == null) continue;
                //we have a match
                foundChildren++;
                if(foundChildren > count) {
                    item.gameObject.SetActive(false);
                    item.name = "unused";
                }else {
                    item.gameObject.SetActive(true);
                    yield return item;
                }
            }
            //add children
            if(foundChildren > count) yield break;
            while(foundChildren < count) {
                foundChildren++;
                yield return Instantiate(labelPrefab, parent.transform).GetComponent<LayoutTMProRotator>();
            }
        }

        private void UpdateYLabels() {
            if(yGridLabels == null||gridRenderer==null) return;
            var children = gridRenderer.Size.y;
            var step = gridRenderer.Scale.y;
            var count = reverseYLabels?gridRenderer.Origin.y+step*(children-1):gridRenderer.Origin.y;
            foreach(var child in GetLabels(yGridLabels.transform, children)) {
                child.SetText(count.ToString());
                child.RotationType = yRotationType;
                child.name = $"Y Label {count}";
                if(reverseYLabels) count -= step;
                else count += step;
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}