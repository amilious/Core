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
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  inspiration taken from [Game Dev Guide] -> https://www.youtube.com/watch?v=CGsEJToeXmA&t=84s  //////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using UnityEngine;
using UnityEngine.UI;
using Amilious.Core.Attributes;

namespace Amilious.Core.UI.Layout {
    
    //TODO: add divide by zero checks
    
    public class FlexibleGridLayout : LayoutGroup {
        
        public enum FitType { Uniform, Width, Height, FixedRows, FixedColumns}

        public FitType fitType;
        public Vector2Int gridSize;
        [AmiliousDisable]
        public Vector2 cellSize;
        public Vector2 spacing;
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void CalculateLayoutInputHorizontal() {

            base.CalculateLayoutInputHorizontal();
            
            var sqrt = Mathf.Sqrt(transform.childCount);

            switch(fitType) {
                case FitType.Uniform:
                    gridSize.y = gridSize.x = Mathf.CeilToInt(sqrt); break;
                case FitType.Width: 
                    gridSize.x = Mathf.CeilToInt(sqrt);
                    gridSize.y = Mathf.CeilToInt(transform.childCount / (float)gridSize.x); break;
                case FitType.FixedColumns:
                    gridSize.y = Mathf.CeilToInt(transform.childCount / (float)gridSize.x); break;
                case FitType.Height:
                    gridSize.y = Mathf.CeilToInt(sqrt);
                    gridSize.x = Mathf.CeilToInt(transform.childCount / (float)gridSize.y); break;
                case FitType.FixedRows:
                    gridSize.x = Mathf.CeilToInt(transform.childCount / (float)gridSize.y); break;
                default: throw new ArgumentOutOfRangeException();
            }

            if(float.IsNaN(gridSize.x)) gridSize.x = 0;
            if(float.IsNaN(gridSize.y)) gridSize.y = 0;
            
            var parentHeight = rectTransform.rect.height;
            var parentWidth = rectTransform.rect.width;

            //   size                 totalSize      -         spacing             -           padding
            var cellWidth = parentWidth/gridSize.x - spacing.x/gridSize.x*(gridSize.x-1) - (padding.left+padding.right)/(float)gridSize.x;
            var cellHeight = parentHeight/gridSize.y - spacing.y/gridSize.y*(gridSize.y-1) - (padding.top+padding.bottom)/(float)gridSize.y;

            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            var columnCount = 0;
            var rowCount = 0;

            for(var i = 0; i < rectChildren.Count; i++) {
                rowCount = i / gridSize.x;
                columnCount = i % gridSize.x;
                var item = rectChildren[i];
                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
                
                SetChildAlongAxis(item,0,xPos,cellSize.x);
                SetChildAlongAxis(item,1,yPos,cellSize.y);
                
            }

        }

        /// <inheritdoc />
        public override void CalculateLayoutInputVertical() {}

        /// <inheritdoc />
        public override void SetLayoutHorizontal() {}

        /// <inheritdoc />
        public override void SetLayoutVertical() {}
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}