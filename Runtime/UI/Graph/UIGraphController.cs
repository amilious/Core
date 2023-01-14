
 using UnityEngine;
using Amilious.Core.UI.Layout;
using Amilious.Core.Extensions;
using System.Collections.Generic;

namespace Amilious.Core.UI.Graph {
    
    [ExecuteAlways]
    public class UIGraphController : AmiliousBehavior {

        [SerializeField] private UIGridRenderer gridRenderer;
        [SerializeField] private GameObject labelPrefab;
        [SerializeField] private GameObject linePrefab;
        
        [Header("X Labels")]
        [SerializeField] private Transform xGridLabels;
        [SerializeField] private RotationType xRotationType;
        [SerializeField] private bool reverseXLabels = false;
        
        [Header("Y Labels")]
        [SerializeField] private Transform yGridLabels;
        [SerializeField] private RotationType yRotationType;
        [SerializeField] private bool reverseYLabels = true;

        private void OnEnable() => gridRenderer.OnGridUpdated += FixLabels;

        private void OnDisable() => gridRenderer.OnGridUpdated -= FixLabels;

        private void OnValidate() => FixLabels();

        private void Start() => FixLabels();

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

    }
}