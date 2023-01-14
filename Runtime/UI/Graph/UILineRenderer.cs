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

        private float _width, _height;

        private Vector2 _unitSize;

        private float _scaledLineThickness;

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

            var minPoint = grid.Origin * _unitSize;
            var maxPoint = offset + (_unitSize * grid.Size);

            float angle = 0;
            for (var i = 0; i < points.Count - 1; i++) {

                var point = points[i];
                var point2 = points[i+1];

                if (i < points.Count - 1) {
                    angle = GetAngle(point, point2) + 90f;
                }

                DrawVerticesForPoint(point, point2, offset,angle, vh);
            }

            //draw lines
            for (var i = 0; i < points.Count - 1; i++) {
                var index = i * 4;
                //draw line
                vh.AddTriangle(index + 0, index + 1, index + 2);
                vh.AddTriangle(index + 1, index + 2, index + 3);
                if(i >= points.Count - 2) continue;
                //draw line connection
                vh.AddTriangle(index + 3, index + 2, index + 4);
                vh.AddTriangle(index + 5, index + 4, index + 3);
            }
        }

        private bool ShouldDraw(ref Vector2 point1, ref Vector2 point2, Vector2 min, Vector2 max, 
            out bool point1Modified, out bool point2Modified) {
            point1Modified = false;
            point2Modified = false;
            if (min == max || point1 == point2)return false;
            if(point1.x < min.x || point1.x > max.x || point1.y < min.y || point1.y > max.y || point2.x < min.x || 
                point2.x > max.x || point2.y < min.y || point2.y > max.y) return false;
            if (point2.x - point1.x == 0) {
                point1Modified = true;
                point2Modified = true;
                point1.x = point2.x;
                point1.y = min.y;
                point2.y = max.y;
            } else {
                var m = (point2.y - point1.y) / (point2.x - point1.x);
                var b = point1.y - m * point1.x;
                var x = (point1.x < min.x) ? min.x : (point1.x > max.x) ? max.x : point1.x;
                if(x != point1.x) point1Modified = true;
                point1.y = m * x + b;
                point1.x = x;
                x = (point2.x < min.x) ? min.x : (point2.x > max.x) ? max.x : point2.x;
                if(x != point2.x) point2Modified = true;
                point2.y = m * x + b;
                point2.x = x;
            }
            return true;
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