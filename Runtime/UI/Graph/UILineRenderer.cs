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

using UnityEngine;
using UnityEngine.UI;
using Amilious.Core.Extensions;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Amilious.Core.UI.Graph {
    
    [ExecuteAlways, RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic {

        [SerializeField] private UIGridRenderer grid;
        [SerializeField] private Vector2Int gridSize;
        [FormerlySerializedAs("thickness")] 
        [SerializeField] private float lineThickness = 10f;
        [SerializeField] private List<Vector2> points;
        [SerializeField] private bool cropToGrid;
        
        private float _width, _height;

        private Vector2 _unitSize;

        private float _scaledLineThickness;

        private readonly List<Vector2Int> _connectingPoints = new List<Vector2Int>();
        private readonly List<int> _skippedLines = new List<int>();
        private readonly List<int> _skippedConnections = new List<int>();

        public UIGridRenderer Grid {
            get => grid;
            set {
                if(value == grid) return;
                if(grid != null) grid.OnGridUpdated -= SetAllDirty;
                grid = value;
                gridSize = grid.Size;
                if(grid != null) grid.OnGridUpdated += SetAllDirty;
                SetAllDirty();
            }
        }

        public bool AddPoint(Vector2 point) {
            if(points.Contains(point)) return false;
            points.Add(point);
            SetAllDirty();
            return true;
        }

        public void AddPoints(IEnumerable<Vector2> points) {
            foreach(var point in points) {
                if(this.points.Contains(point)) continue;
                this.points.Add(point);
            }
            SetAllDirty();
        }

        public void ClearPoints() {
            points.Clear();
            SetAllDirty();
        }
        
        protected override void OnPopulateMesh(VertexHelper vh) {
            
            vh.Clear();
            rectTransform.rect.GetSizes(out _width, out _height);
            var offset = rectTransform.rect.min;
            _unitSize.x = _width / (float)gridSize.x;
            _unitSize.y = _height / (float)gridSize.y;
            _scaledLineThickness = lineThickness;
            
            if(grid != null) {
                _unitSize.x  /= grid.Scale.x;
                _unitSize.y /= grid.Scale.y;
                offset += grid.PixelOffset - (grid.Origin * _unitSize);
                var scale = grid.Scale.x + grid.Scale.y / 2f;
                if(scale != 0) _scaledLineThickness = lineThickness / scale;
                _scaledLineThickness = Mathf.Max(1f, _scaledLineThickness);
            }
            if(points.Count<2) return;

            var minPoint = grid.Origin * gridSize;
            var maxPoint = (grid.Origin + gridSize)* grid.Scale;
            _connectingPoints.Clear();
            _skippedLines.Clear();
            _skippedConnections.Clear();

            float angle = 0;
            for (var i = 0; i < points.Count - 1; i++) {

                var point = points[i];
                var point2 = points[i+1];

                if(cropToGrid) {
                    if(!ShouldDraw(ref point, ref point2, minPoint, maxPoint, out var point1Modified,
                        out bool point2Modified)) {
                        _skippedLines.Add(i);
                        continue; //do not add verticies or draw lines
                    }

                    if(point1Modified) _skippedConnections.Add(i);
                }

                angle = GetAngle(point, point2) + 90f;
                DrawVerticesForPoint(point, point2, offset,angle, vh);
            }

            //draw lines
            for (var i = 0; i < points.Count - 1; i++) {
                if(_skippedLines.Contains(i)) continue;
                var index = i * 4;
                //draw line
                vh.AddTriangle(index + 0, index + 1, index + 2);
                vh.AddTriangle(index + 1, index + 2, index + 3);
                if(i >= points.Count - 2 || _skippedConnections.Contains(i)) continue;
                //draw line connection
                vh.AddTriangle(index + 3, index + 2, index + 4);
                vh.AddTriangle(index + 5, index + 4, index + 3);
            }
        }

        private bool ShouldDraw(ref Vector2 point1, ref Vector2 point2, Vector2 minPoint, Vector2 maxPoint, out bool point1Modified, out bool point2Modified)
{
    point1Modified = false;
    point2Modified = false;

    // Check if point1 is within the grid
    if (point1.x < minPoint.x || point1.x > maxPoint.x || point1.y < minPoint.y || point1.y > maxPoint.y)
    {
        // Check if the line between point1 and point2 intersects the left or bottom side of the grid
        if (point1.x < minPoint.x && point2.x > minPoint.x)
        {
            float slope = (point2.y - point1.y) / (point2.x - point1.x);
            point1.y = slope * (minPoint.x - point1.x) + point1.y;
            point1.x = minPoint.x;
            point1Modified = true;
        }
        else if (point1.y < minPoint.y && point2.y > minPoint.y)
        {
            float slope = (point2.x - point1.x) / (point2.y - point1.y);
            point1.x = slope * (minPoint.y - point1.y) + point1.x;
            point1.y = minPoint.y;
            point1Modified = true;
        }

        // Check if the line between point1 and point2 intersects the right or top side of the grid
        if (point1.x > maxPoint.x && point2.x < maxPoint.x)
        {
            float slope = (point2.y - point1.y) / (point2.x - point1.x);
            point1.y = slope * (maxPoint.x - point1.x) + point1.y;
            point1.x = maxPoint.x;
            point1Modified = true;
        }
        else if (point1.y > maxPoint.y && point2.y < maxPoint.y)
        {
            float slope = (point2.x - point1.x) / (point2.y - point1.y);
            point1.x = slope * (maxPoint.y - point1.y) + point1.x;
            point1.y = maxPoint.y;
            point1Modified = true;
        }
    }

    // Check if point2 is within the grid
    if (point2.x < minPoint.x || point2.x > maxPoint.x || point2.y < minPoint.y || point2.y > maxPoint.y)
    {
        // Check if the line between point1 and point2 intersects the left or bottom side of the grid
        if (point2.x < minPoint.x && point1.x > minPoint.x)
        {
            float slope = (point2.y - point1.y) / (point2.x - point1.x);
            point2.y = slope * (minPoint.x - point1.x) + point1.y;
            point2.x = minPoint.x;
            point2Modified = true;
        }
        else if (point2.y < minPoint.y && point1.y > minPoint.y)
        {
            float slope = (point2.x - point1.x) / (point2.y - point1.y);
            point2.x = slope * (minPoint.y - point1.y) + point1.x;
            point2.y = minPoint.y;
            point2Modified = true;
        }

        // Check if the line between point1 and point2 intersects the right or top side of the grid
        if (point2.x > maxPoint.x && point1.x < maxPoint.x)
        {
            float slope = (point2.y - point1.y) / (point2.x - point1.x);
            point2.y = slope * (maxPoint.x - point1.x)+point1.y;
            point2.x = maxPoint.x;
            point2Modified = true;
        }
        else if (point2.y > maxPoint.y && point1.y < maxPoint.y)
        {
            float slope = (point2.x - point1.x) / (point2.y - point1.y);
            point2.x = slope * (maxPoint.y - point1.y) + point1.x;
            point2.y = maxPoint.y;
            point2Modified = true;
        }
    }

// Return true if the line between point1 and point2 intersects the grid
    return point1Modified || point2Modified || (point1.x >= minPoint.x && point1.x <= maxPoint.x && point1.y >= minPoint.y && point1.y <= maxPoint.y) || (point2.x >= minPoint.x && point2.x <= maxPoint.x && point2.y >= minPoint.y && point2.y <= maxPoint.y);
}
                
                
                
                
                
                
                
                
                
                
                
                

        private void Update() {
            if(grid) gridSize = grid.Size;
        }

        protected override void OnEnable() {
            base.OnEnable();
            if(grid == null) return;
            grid.OnGridUpdated += SetAllDirty;
        }

        protected override void OnDisable() {
            base.OnDisable();
            if(grid == null) return;
            grid.OnGridUpdated -= SetAllDirty;
        }

        private float GetAngle(Vector2 me, Vector2 target) {
            return (float)(Mathf.Atan2(_height * (target.y - me.y), _width * (target.x - me.x)) * (180 / Mathf.PI));
        }

        private void DrawVerticesForPoint(Vector2 point, Vector2 point2, Vector2 offset, float angle, VertexHelper vh) {
            
            var vertex = UIVertex.simpleVert;
            vertex.color = color;

            vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-_scaledLineThickness / 2, 0);
            vertex.position += new Vector3(offset.x+(_unitSize.x * point.x), offset.y+(_unitSize.y * point.y));
            vh.AddVert(vertex);

            vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(_scaledLineThickness / 2, 0);
            vertex.position += new Vector3(offset.x+(_unitSize.x * point.x), offset.y+(_unitSize.y * point.y));
            vh.AddVert(vertex);

            vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-_scaledLineThickness / 2, 0);
            vertex.position += new Vector3(offset.x+(_unitSize.x * point2.x), offset.y+(_unitSize.y * point2.y));
            vh.AddVert(vertex);

            vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(_scaledLineThickness / 2, 0);
            vertex.position += new Vector3(offset.x+(_unitSize.x * point2.x), offset.y+(_unitSize.y * point2.y));
            vh.AddVert(vertex);
        }
    }
       
}