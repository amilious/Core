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
using System;
using UnityEngine;
using UnityEngine.UI;
using Amilious.Core.Attributes;
using System.Collections.Generic;

namespace Amilious.Core.UI.Layout {

    /// <summary>
    /// This class is used for the type of rotation.
    /// </summary>
    [Serializable]
    public enum RotationType {
        
        /// <summary>
        /// Do not rotate the child text objects.
        /// </summary>
        None, 
        
        /// <summary>
        /// Rotate the child text objects 90 degrees clockwise.
        /// </summary>
        Clockwise, 
        
        /// <summary>
        /// Rotate the child text objects 180 degrees.
        /// </summary>
        UpsideDown, 
        
        /// <summary>
        /// Rotate the child text objects 90 degrees counter-clockwise.
        /// </summary>
        CounterClockwise
        
    }
    
    /// <summary>
    /// The class is used to rotate a <see cref="TMP_Text"/> within another gameobject.
    /// </summary>
    public class LayoutTMProRotator : LayoutGroup {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [Header("Text")]
        [Tooltip("The rotation that you want to be applied to TextMeshPro text objects.")]
        [SerializeField] private RotationType rotationType = RotationType.None;
        [Tooltip("The text objects that you want this layout to effect.")]
        [SerializeField] private List<TMP_Text> texts;
        
        [Header("Alignment")]
        [Tooltip("If true the alignment of TextMeshPro text objects will be edited.")]
        [SerializeField, AmiliousBool(true)] private bool applyAlignment;
        [SerializeField, ShowIf(nameof(applyAlignment))]
        [Tooltip("The alignment to apply to the TextMeshPro text if the rotation type is set to none.")]
        private TextAlignmentOptions noRotationAlignment = TextAlignmentOptions.BottomRight;
        [SerializeField, ShowIf(nameof(applyAlignment))] 
        [Tooltip("The alignment to apply to the TextMeshPro text if the rotation type is set to clockwise.")]
        private TextAlignmentOptions clockwiseAlignment = TextAlignmentOptions.BottomRight;
        [SerializeField, ShowIf(nameof(applyAlignment))] 
        [Tooltip("The alignment to apply to the TextMeshPro text if the rotation type is set to counter clockwise.")]
        private TextAlignmentOptions counterClockwiseAlignment = TextAlignmentOptions.BottomLeft;
        [SerializeField, ShowIf(nameof(applyAlignment))] 
        [Tooltip("The alignment to apply to the TextMeshPro text if the rotation type is set to upside down.")]
        private TextAlignmentOptions upsideDownAlignment = TextAlignmentOptions.BottomLeft;

        [Header("Margins")]
        [Tooltip("If true the text padding of TextMeshPro text objects will be edited.")]
        [SerializeField, AmiliousBool(true)] private bool applyMargins;
        [SerializeField, ShowIf(nameof(applyMargins))] 
        [Tooltip("The margins to apply to the text if there is no rotation.")]
        [AmiliousVector(VLayout.TripleLine,"Left","Top","Right","Bottom")] 
        private Vector4 noRotationMargins;
        [SerializeField, ShowIf(nameof(applyMargins))]
        [Tooltip("The margins to apply to the text if there is clockwise rotation.")]
        [AmiliousVector(VLayout.TripleLine,"Left","Top","Right","Bottom")] 
        private Vector4 clockwiseMargins;
        [SerializeField, ShowIf(nameof(applyMargins))] 
        [Tooltip("The margins to apply to the text if there is counter-clockwise rotation.")]
        [AmiliousVector(VLayout.TripleLine,"Left","Top","Right","Bottom")] 
        private Vector4 counterClockwiseMargins;
        [SerializeField, ShowIf(nameof(applyMargins))] 
        [Tooltip("The margins to apply to the text if it is upside down.")]
        [AmiliousVector(VLayout.TripleLine,"Left","Top","Right","Bottom")] 
        private Vector4 upsideDownMargins;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or modify the rotation type of the text.
        /// </summary>
        public RotationType RotationType {
            get => rotationType;
            set {
                if(rotationType == value) return;
                rotationType = value;
                SetDirty();
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void CalculateLayoutInputVertical() { }

        /// <inheritdoc />
        public override void SetLayoutHorizontal() { }

        /// <inheritdoc />
        public override void SetLayoutVertical() { }
        
        /// <inheritdoc />
        public override void CalculateLayoutInputHorizontal() {
            
            base.CalculateLayoutInputHorizontal();
            
            var parentSize = rectTransform.rect.size;
            var position = new Vector2(padding.left,padding.top);
            var size = new Vector2(parentSize.x-padding.left-padding.right,parentSize.y-padding.top-padding.bottom);
            var pivot = Vector2.zero;
            var rotation = Quaternion.identity;
            var alignment = noRotationAlignment;
            var margins = noRotationMargins;

            switch(rotationType) {
                case RotationType.Clockwise: 
                    Clockwise(parentSize,out position,out size,out pivot,out rotation);
                    alignment = clockwiseAlignment;
                    margins = clockwiseMargins;
                    break;
                case RotationType.UpsideDown: 
                    UpsideDown(parentSize,out position,out size,out pivot,out rotation);
                    alignment = upsideDownAlignment;
                    margins = upsideDownMargins;
                    break;
                case RotationType.CounterClockwise:
                    CounterClockwise(parentSize,out position,out size,out pivot,out rotation);
                    alignment = counterClockwiseAlignment;
                    margins = counterClockwiseMargins;
                    break;
                case RotationType.None: default: break;
            }

            foreach(var child in texts) {
                if(child==null) continue;
                if(applyAlignment)child.alignment = alignment;
                if(applyMargins) child.margin = margins;
                var childRect = child.rectTransform;
                childRect.pivot = pivot;
                childRect.rotation = rotation;
                SetChildAlongAxis(childRect,0,position.x,size.x);
                SetChildAlongAxis(childRect,1,position.y,size.y);
            }

        }
        
        /// <summary>
        /// This method is used to set the text of all the child text mesh pro objects.
        /// </summary>
        /// <param name="text">The text that you want to set.</param>
        public void SetText(string text) {
            foreach(var tmp in texts) {
                if(tmp == null) continue;
                tmp.text = text;
                tmp.SetAllDirty();
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to calculate values for a counter-clockwise rotation.
        /// </summary>
        private void CounterClockwise(Vector2 parentSize, out Vector2 position, out Vector2 size, out Vector2 pivot, 
            out Quaternion rotation) {
            size.x = parentSize.y - padding.left - padding.right;
            position.x = padding.left;
            size.y = parentSize.x - padding.top - padding.bottom;
            position.y = parentSize.y+padding.top;
            pivot.x = 0f;
            pivot.y = 1f;
            rotation = Quaternion.Euler(0,0,90);
        }
        
        /// <summary>
        /// This method is used to calculate values for a clockwise rotation.
        /// </summary>
        private void Clockwise(Vector2 parentSize, out Vector2 position, out Vector2 size, out Vector2 pivot, 
            out Quaternion rotation) {
            size.x = parentSize.y - padding.left - padding.right;
            position.x = padding.left;
            size.y = parentSize.x - padding.top - padding.bottom;
            position.y = -parentSize.x + padding.top;
            pivot = Vector2.zero;
            rotation = Quaternion.Euler(0,0,-90);
        }
        
        /// <summary>
        /// This method is used to calculate values for an upsidedown rotation.
        /// </summary>
        private void UpsideDown(Vector2 parentSize, out Vector2 position, out Vector2 size, out Vector2 pivot, 
            out Quaternion rotation) {
            size.x = parentSize.x - padding.left - padding.right;
            position.x = padding.left;
            size.y = parentSize.y - padding.top - padding.bottom;
            position.y = padding.top;
            //rotate from the center
            pivot = Vector2.one * .5f;
            rotation = Quaternion.Euler(0,0,180);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
    
}