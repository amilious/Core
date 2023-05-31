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
using Amilious.Core.Extensions;
using Amilious.Core.Attributes;

namespace Amilious.Core.UI.Graph {

    /// <summary>
    /// This class is used to draw a grid.
    /// </summary>
    [ExecuteAlways, RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu(AmiliousCore.GRAPH_CONTEXT_MENU+"UI Grid Renderer")]
    public class UIGridRenderer : Graphic {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [Tooltip("The number of rows and columns that the grid should contain.")]
        [AmiVector(VLayout.SingleLine,"Columns","Rows")]
        [SerializeField] private Vector2Int size = new Vector2Int(10, 10);
        [Tooltip("The display origin of the grid.")]
        [AmiVector(VLayout.SingleLine, "First-X", "First-Y")]
        [SerializeField] private Vector2Int origin = new Vector2Int(0, 0);
        [Tooltip("The number of units per grid line.")]
        [AmiVector(VLayout.SingleLine,"X-Axis", "Y-Axis")]
        [SerializeField]private Vector2Int scale = new Vector2Int(1, 1);
        [Tooltip("The thickness of the grid lines.")]
        [SerializeField] private float lineThickness = 2f;
        [Tooltip("If true the last row grid line will be drawn, otherwise it will not be drawn.")]
        [SerializeField] private bool drawLastRow;
        [Tooltip("If true the last column grid line will be drawn, otherwise it will not be drawn.")]
        [SerializeField] private bool drawLastColumn;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private bool _updated = true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when the grid is redrawn.
        /// </summary>
        public event Action OnGridUpdated;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set whether the last column should be drawn or not.
        /// </summary>
        public bool DrawLastColumn {
            get => drawLastColumn;
            set {
                drawLastColumn = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set whether the last row should be drawn or not.
        /// </summary>
        public bool DrawLastRow {
            get => drawLastRow;
            set {
                drawLastRow = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set the grid line thickness.
        /// </summary>
        public float LineThickness {
            get => lineThickness;
            set {
                lineThickness = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set the grid scale.
        /// </summary>
        public Vector2Int Scale {
            get => scale;
            set {
                scale = value; 
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set the grid size.
        /// </summary>
        public Vector2Int Size {
            get => size;
            set {
                size = value;
                SetAllDirty();
            }
        }

        /// <summary>
        /// This property is used to get or set the grid origin point.
        /// </summary>
        public Vector2Int Origin => origin;

        /// <summary>
        /// This property is used to get the pixel offset.
        /// </summary>
        public Vector2 PixelOffset => new Vector2(lineThickness / 2f, lineThickness / 2f);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Unity MonoBehavior Methods /////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called every frame.
        /// </summary>
        private void Update() {
            // this is to avoid an "already in update loop error"
            if(!_updated) return;
            _updated = false;
            OnGridUpdated?.Invoke();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
                  
        /// <summary>
        /// This method is used to redraw the graph.
        /// </summary>
        [ContextMenu("Redraw")]
        public void Redraw() => SetAllDirty();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void OnPopulateMesh(VertexHelper vh) {
            
            vh.Clear();

            var rect = rectTransform.rect;
            rect.GetSizes(out var width, out var height);
            var offset = rectTransform.rect.min;

            var cellSize = new Vector2((width - lineThickness) / size.x, (height-lineThickness) / size.y);

            var vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            var xPos = offset.x;
            var yPos = 0f;
            var vertexOffset = 0;

            var draw = drawLastRow ? size.y + 1 : size.y;
            
            for(var y = 0; y < draw; y++) {
                
                //calculate y position
                yPos = offset.y + (y * cellSize.y);
                
                //add vertex
                vh.AddVertex(ref vertex,xPos, yPos);
                vh.AddVertex(ref vertex,xPos, yPos+lineThickness);
                vh.AddVertex(ref vertex,xPos+width, yPos+lineThickness);
                vh.AddVertex(ref vertex,xPos+width, yPos);
                
                //add triangles
                vh.AddTriangle(vertexOffset+0,vertexOffset+1,vertexOffset+3);
                vh.AddTriangle(vertexOffset+1,vertexOffset+2,vertexOffset+3);
                
                //update offset
                vertexOffset += 4;
            }

            draw = drawLastColumn ? size.x + 1 : size.x;
            yPos = offset.y;
            for(var x = 0; x < draw; x++) {
                //calculate x position
                xPos = offset.x + (x * cellSize.x);
                
                //add vertices
                vh.AddVertex(ref vertex,xPos, yPos);
                vh.AddVertex(ref vertex,xPos, yPos+height);
                vh.AddVertex(ref vertex,xPos+lineThickness, yPos+height);
                vh.AddVertex(ref vertex,xPos+lineThickness, yPos);
                
                //add triangles
                vh.AddTriangle(vertexOffset+0,vertexOffset+1,vertexOffset+3);
                vh.AddTriangle(vertexOffset+1,vertexOffset+2,vertexOffset+3);
                
                //update offset
                vertexOffset += 4;
            }

            //we update this way to prevent an "already in loop error"
            _updated = true;

        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}