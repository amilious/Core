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

using System;
using UnityEngine;
using UnityEngine.UI;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using Amilious.Core.MathHelpers;
using System.Collections.Generic;

namespace Amilious.Core.UI.Graph {

    /// <summary>
    /// This class is used to render a line in ui
    /// </summary>
    [ExecuteAlways, RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu(AmiliousCore.GRAPH_CONTEXT_MENU+"UI Line Renderer")]
    public class UILineRenderer : Graphic {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [AmiTab("Grid")][Tooltip("If true line will be cropped to the grid area.")]
        [SerializeField] private bool cropToGrid = true;
        [AmiTab("Grid")][Tooltip("(Optional) The grid that you want the lines drawn on.")]
        [SerializeField] private UIGridRenderer grid;
        [AmiTab("Grid")][Tooltip("The size of the grid.")]
        [SerializeField, AmiVector(VLayout.DoubleLine,"Columns", "Rows")] private Vector2Int gridSize;
        
        [AmiTab("Line")][Tooltip("The thickness of the drawn line.")]
        [SerializeField] private float lineThickness = 10f;
        [AmiTab("Line")][Tooltip("The points of the line.")]
        [SerializeField] private List<Vector2> points;
        
        [SerializeField]
        [AmiColor(useHDR:false, showAlpha: true)]
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private Vector2 _unitSize, _minPoint, _maxPoint, _offset;
        private float _scaledLineThickness,_width, _height;
        private readonly List<int> _skippedLines = new List<int>();
        private readonly List<int> _skippedConnections = new List<int>();
        private bool _updatedGrid = true;
        private bool _needsRedraw = true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set the grid that the line is drawn on.
        /// </summary>
        public UIGridRenderer Grid {
            get => grid;
            set {
                if(value == grid) return;
                if(grid != null) grid.OnGridUpdated -= SetAllDirty;
                grid = value;
                _updatedGrid = true;
                gridSize = grid.Size;
                if(grid != null) { grid.OnGridUpdated += SetAllDirty; }
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set whether or not the line should be cropped to the associated grids
        /// dementions.
        /// </summary>
        public bool CropToGrid {
            get => cropToGrid;
            set {
                if(value == cropToGrid) return;
                cropToGrid = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set the grid size associated with the drawn line.
        /// </summary>
        public Vector2Int GridSize {
            get => gridSize;
            set {
                if(value == gridSize) return;
                gridSize = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set the line thickness.
        /// </summary>
        public float LineThickness {
            get => lineThickness;
            set {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if(value == lineThickness) return;
                lineThickness = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get the number of points within the line.
        /// </summary>
        public int PointCount => points.Count;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when the object is first loaded.
        /// </summary>
        protected override void Awake() {
            _updatedGrid = true;
            base.Awake();
        }

        /// <summary>
        /// This method is called when inspector values are changed in the editor.
        /// </summary>
        protected override void OnValidate() {
            _updatedGrid = true;
            base.OnValidate();
        }

        /// <summary>
        /// This method is called on every frame.
        /// </summary>
        private void Update() {
            if(_needsRedraw) {
                _needsRedraw = false;
                SetAllDirty();
            }
            if(grid) gridSize = grid.Size;
            StretchToParent();
        }

        /// <summary>
        /// This method is called when the game object is enabled.
        /// </summary>
        protected override void OnEnable() {
            base.OnEnable();
            if(grid == null) return;
            grid.OnGridUpdated += SetAllDirty;
        }

        /// <summary>
        /// This method is called when the game object is disabled.
        /// </summary>
        protected override void OnDisable() {
            base.OnDisable();
            if(grid == null) return;
            grid.OnGridUpdated -= SetAllDirty;
        }

        protected void OnDrawGizmosSelected() {
            /*Gizmos.color = Color.red;
            for (int i = 0; i < points.Count; i++) {
                Vector2 relativePoint = points[i];
                Vector3 point = transform.position + (new Vector3(relativePoint.x*_unitSize.x, relativePoint.y*_unitSize.y, 0)) + (Vector3)_offset;
                Gizmos.DrawSphere(point, 0.1f);
                points[i] = ((Vector2)(Handles.PositionHandle(point, Quaternion.identity) - transform.position) - _offset) / _unitSize;
            }*/
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to add a point to the line. It will be added to the end of the existing
        /// points.
        /// </summary>
        /// <param name="point">The point that you want to add.</param>
        public void AddPoint(Vector2 point) {
            points.Add(point);
            SetAllDirty();
        }

        /// <summary>
        /// This method is used to add the given points to the line.  They will be added to the end of the existing
        /// points in the order that they are given.
        /// </summary>
        /// <param name="pointsToAdd">The points that you want to add.</param>
        public void AddPoints(IEnumerable<Vector2>pointsToAdd) {
            foreach(var point in pointsToAdd) {
                if(this.points.Contains(point)) continue;
                this.points.Add(point);
            }
            SetAllDirty();
        }

        /// <summary>
        /// This method is used to get the point with the given index.
        /// </summary>
        /// <param name="index">The index of the point that you want to get.</param>
        /// <returns>The point at the given index, otherwise 0,0 if the index is out of range.</returns>
        public Vector2 GetPoint(int index) {
            if(index < 0 || index >= points.Count) return Vector2.zero;
            return points[index];
        }

        /// <summary>
        /// This method is used to remove the point at the given index.
        /// </summary>
        /// <param name="index">The index of the point that you want to remove.</param>
        /// <returns>True if the index was in range and was removed, otherwise false.</returns>
        public bool RemovePoint(int index) {
            if(index < 0 || index >= points.Count) return false;
            points.RemoveAt(index);
            SetAllDirty();
            return true;
        }

        /// <summary>
        /// This method is used to clear all of the existing points in the line.
        /// </summary>
        public void ClearPoints() {
            points.Clear();
            SetAllDirty();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to stretch the transform to the parents size.
        /// </summary>
        private void StretchToParent() {
            if(!_updatedGrid) return;
            _updatedGrid = false;
            if(grid == null) return;
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            SetAllDirty();
        }
        
        /// <inheritdoc />
        protected override void OnPopulateMesh(VertexHelper vh) {
            try {
                vh.Clear();
                rectTransform.rect.GetSizes(out _width, out _height);
                if(grid != null) {
                    _width -= grid.DrawLastColumn ? grid.LineThickness : grid.LineThickness / 2f;
                    _height -= grid.DrawLastRow ? grid.LineThickness : grid.LineThickness / 2f;
                }

                _offset = rectTransform.rect.min;
                _unitSize.x = _width / (float)gridSize.x;
                _unitSize.y = _height / (float)gridSize.y;
                _scaledLineThickness = lineThickness;

                if(grid != null) {
                    _unitSize.x /= grid.Scale.x;
                    _unitSize.y /= grid.Scale.y;
                    _offset += grid.PixelOffset - (grid.Origin * _unitSize);
                    var scale = grid.Scale.x + grid.Scale.y / 2f;
                    if(scale != 0) _scaledLineThickness = lineThickness / scale;
                    _scaledLineThickness = Mathf.Max(1f, _scaledLineThickness);
                }

                if(points.Count < 2) return;

                _minPoint = grid.Origin * gridSize;
                _maxPoint = (grid.Origin + gridSize) * grid.Scale;
                _skippedLines.Clear();
                _skippedConnections.Clear();

                float angle = 0;
                for(var i = 0; i < points.Count - 1; i++) {

                    var point = points[i];
                    var point2 = points[i + 1];

                    if(cropToGrid) {
                        if(!ShouldDraw(ref point, ref point2, out var point1Modified, out bool point2Modified)) {
                            _skippedLines.Add(i);
                            continue; //do not add verticies or draw lines
                        }

                        if(point1Modified) _skippedConnections.Add(i);
                        if(i == points.Count - 2 && point2Modified) _skippedConnections.Add(i + 1);
                    }

                    angle = GetAngle(point, point2);
                    DrawVerticesForPoint(point, point2, _offset, angle, vh);
                }

                var vOffset = 0;
                //draw lines
                for(var i = 0; i < points.Count - 1; i++) {
                    if(_skippedLines.Contains(i)) continue;
                    //draw line
                    vh.AddTriangle(vOffset + 0, vOffset + 1, vOffset + 2);
                    vh.AddTriangle(vOffset + 0, vOffset + 2, vOffset + 3);
                    vOffset += 4;
                    if(i >= points.Count - 2 || _skippedConnections.Contains(i)) continue;
                    //draw line connection
                    vh.AddTriangle(vOffset + -1, vOffset + -2, vOffset);
                    vh.AddTriangle(vOffset + -1, vOffset + -2, vOffset + 1);
                    vh.AddTriangle(vOffset + 1, vOffset, vOffset + -1);
                    vh.AddTriangle(vOffset + 1, vOffset, vOffset + -2);
                }

                //connect last and first point if the same
                // ReSharper disable once UseIndexFromEndExpression
                if(points[0] == points[points.Count - 1]) {
                    if(_skippedLines.Contains(0) || _skippedLines.Contains(points.Count - 1)) return;
                    if(_skippedConnections.Contains(points.Count - 1)) return;
                    var last = vOffset - 4;

                    //connect first and last lines
                    vh.AddTriangle(last + 3, last + 2, 0);
                    vh.AddTriangle(last + 3, last + 2, 1);
                    vh.AddTriangle(1, 0, last + 3);
                    vh.AddTriangle(1, 0, last + 2);
                }
                //prevent error when removing the grid
            }
            catch(NullReferenceException) { _needsRedraw = true; }
        }

        /// <summary>
        /// This method is used to modify points and crop lines outside the graph
        /// </summary>
        private bool ShouldDraw(ref Vector2 point1, ref Vector2 point2, out bool point1Modified, out bool point2Modified) {
            point1Modified = false;
            point2Modified = false;

            var equation = new LinearEquation(point1, point2);
            
            // Check if point1 is within the grid
            if (point1.x < _minPoint.x || point1.x > _maxPoint.x || point1.y < _minPoint.y || point1.y > _maxPoint.y) {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if(point1.x == point2.x) point1.y = point1.y>_maxPoint.y?_maxPoint.y:_minPoint.y;
                else {
                    if(point1.x < _minPoint.x && point2.x > _minPoint.x) { //the line intersects the left
                        equation.FindPointFromX(_minPoint.x, out point1); point1Modified = true;
                    }
                    else if(point1.y < _minPoint.y && point2.y > _minPoint.y) { //the line intersects the bottom
                        equation.FindPointFromY(_minPoint.y, out point1); point1Modified = true;
                    }

                    if(point1.x > _maxPoint.x && point2.x < _maxPoint.x) { //the line intersects the right
                        equation.FindPointFromX(_maxPoint.x, out point1); point1Modified = true;
                    }
                    else if(point1.y > _maxPoint.y && point2.y < _maxPoint.y) { //the line intersects the left
                        equation.FindPointFromY(_maxPoint.y, out point1); point1Modified = true;
                    }
                }
            }

            // Check if point2 is within the grid
            if (point2.x < _minPoint.x || point2.x > _maxPoint.x || point2.y < _minPoint.y || point2.y > _maxPoint.y) {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if(point1.x == point2.x) point2.y = point2.y>_maxPoint.y?_maxPoint.y:_minPoint.y;
                else {
                    if(point2.x < _minPoint.x && point1.x > _minPoint.x) { //the line intersects the left
                        equation.FindPointFromX(_minPoint.x, out point2); point2Modified = true;
                    }
                    else if(point2.y < _minPoint.y && point1.y > _minPoint.y) { //the line intersects the bottom
                        equation.FindPointFromY(_minPoint.y, out point2); point2Modified = true;
                    }
                    if(point2.x > _maxPoint.x && point1.x < _maxPoint.x) { //the line intersects the right
                        equation.FindPointFromX(_maxPoint.x, out point2); point2Modified = true;
                    }
                    else if(point2.y > _maxPoint.y && point1.y < _maxPoint.y) { //the line intersects the left
                        equation.FindPointFromY(_maxPoint.y, out point2); point2Modified = true;
                    }
                }
            }

            // Return true if the line between point1 and point2 intersects the grid
            return point1Modified || point2Modified || (point1.x >= _minPoint.x && point1.x <= _maxPoint.x && 
                point1.y >= _minPoint.y && point1.y <= _maxPoint.y) || (point2.x >= _minPoint.x && 
                point2.x <= _maxPoint.x && point2.y >= _minPoint.y && point2.y <= _maxPoint.y);
        }

        /// <summary>
        /// This method is used to get the angle of the line between the two given points.
        /// </summary>
        private float GetAngle(Vector2 me, Vector2 target) =>
            Mathf.Atan2(_height * (target.y - me.y), _width * (target.x - me.x)) * (180 / Mathf.PI)+90;

        /// <summary>
        /// This method is used to draw vertices for the line made between the given points
        /// </summary>
        private void DrawVerticesForPoint(Vector2 point, Vector2 point2, Vector2 offset, float angle, VertexHelper vh) {
            
            var vertex = UIVertex.simpleVert;
            vertex.color = color;
            var angleQuaternion = Quaternion.Euler(0, 0, angle);
            var halfLine = -_scaledLineThickness / 2f;
            
            var firstPoint = new Vector3(offset.x+(_unitSize.x * point.x), offset.y+(_unitSize.y * point.y));
            var secondPoint = new Vector3(offset.x+(_unitSize.x * point2.x), offset.y+(_unitSize.y * point2.y));
            var rotation = new Vector3(-halfLine, 0);

            var topLeft = angleQuaternion * rotation+firstPoint;
            var bottomLeft = angleQuaternion * -rotation + firstPoint;
            var topRight = angleQuaternion * rotation + secondPoint;
            var bottomRight = angleQuaternion * -rotation + secondPoint;
            var max = offset + new Vector2(_width, _height);
            var topEq = new LinearEquation(topLeft, topRight);
            var bottomEq = new LinearEquation(bottomLeft, bottomRight);
            
            //fix x positions if not a horizontal line
            if(!topEq.IsHorizontal && !bottomEq.IsHorizontal) {
                if(topLeft.x < offset.x || bottomLeft.x < offset.x) {
                    topEq.FindPointFromX(offset.x, out topLeft);
                    bottomEq.FindPointFromX(offset.x, out bottomLeft);
                }
                if(topLeft.x > max.x || bottomLeft.x > max.x) {
                    topEq.FindPointFromX(max.x, out topLeft);
                    bottomEq.FindPointFromX(max.x, out bottomLeft);
                }
                if(topRight.x < offset.x || bottomRight.x < offset.x) {
                    topEq.FindPointFromX(offset.x, out topRight);
                    bottomEq.FindPointFromX(offset.x, out bottomRight);
                }
                if(topRight.x > max.x || bottomRight.x > max.x) {
                    topEq.FindPointFromX(max.x, out topRight);
                    bottomEq.FindPointFromX(max.x, out bottomRight);
                }
            }

            //fix y positions if not a vertical line
            if(!topEq.IsVertical && !bottomEq.IsVertical) {
                if(topLeft.y < offset.y || bottomLeft.y < offset.y) {
                    topEq.FindPointFromY(offset.y, out topLeft);
                    bottomEq.FindPointFromY(offset.y, out bottomLeft);
                }
                if(topLeft.y > max.y || bottomLeft.y > max.y) {
                    topEq.FindPointFromY(max.y, out topLeft);
                    bottomEq.FindPointFromY(max.y, out bottomLeft);
                }
                if(topRight.y < offset.y || bottomRight.y < offset.y) {
                    topEq.FindPointFromY(offset.y, out topRight);
                    bottomEq.FindPointFromY(offset.y, out bottomRight);
                }
                if(topRight.y > max.y || bottomRight.y > max.y) {
                    topEq.FindPointFromY(max.y, out topRight);
                    bottomEq.FindPointFromY(max.y, out bottomRight);
                }
            }

            //add the vertices
            vh.AddVertex(ref vertex,topLeft);
            vh.AddVertex(ref vertex,bottomLeft);
            vh.AddVertex(ref vertex,bottomRight);
            vh.AddVertex(ref vertex,topRight);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
       
}