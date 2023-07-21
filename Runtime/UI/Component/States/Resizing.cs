using System;
using Amilious.Core.Extensions;
using Amilious.Core.UI.Cursors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI.Component.States {

    public class Resizing : UIState {
        
        #region Constructor ////////////////////////////////////////////////////////////////////////////////////////////

        private Vector3 _position;
        private Vector2 _offset;
        private ComponentEdge _edge;
        //private bool _resizing;
        private Vector2 _size;
        
        public Resizing(UIComponent component) : base(component, UIStatesType.Resizing) { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region State Changed ////////////////////////////////////////////////////////////////////////////////////////// 

        public override void OnEnterState(UIStatesType previousState, PointerEventData pointerData) {
            var edge = Component.GetEdge(pointerData.position);
            if(Component.Resizable && edge != ComponentEdge.None) {
                var cursor = edge switch {
                    ComponentEdge.TopLeft => DefaultCursors.ResizeUpLeft,
                    ComponentEdge.Top => DefaultCursors.ResizeVertical,
                    ComponentEdge.TopRight => DefaultCursors.ResizeUpRight,
                    ComponentEdge.Left => DefaultCursors.ResizeHorizontal,
                    ComponentEdge.Right => DefaultCursors.ResizeHorizontal,
                    ComponentEdge.BottomLeft => DefaultCursors.ResizeUpRight,
                    ComponentEdge.Bottom => DefaultCursors.ResizeVertical,
                    ComponentEdge.BottomRight => DefaultCursors.ResizeUpLeft,
                    _ => throw new ArgumentOutOfRangeException()
                };
                CursorController.SetCursor(cursor);
            }
            if(pointerData!=null)OnBeginDrag(pointerData);
        }
        
        public override void OnExitState(UIStatesType nextState) {
            _position = Vector3.zero;
            _edge = ComponentEdge.None;
            _offset = Vector2.zero;
            _size = Vector2.zero;
        }

        public override void OnBeginDrag(PointerEventData eventData) {
            _edge = Component.GetEdge(eventData.position);
            if(_edge == ComponentEdge.None) {
                Component.SetState(UIStatesType.Idle,null);
                return;
            }
            _offset = eventData.position;
            _position = _edge.GetOpposite().GetPosition(RectTransform);
            _size = RectTransform.sizeDelta;
            eventData.dragging = true;
        }

        public override void OnDrag(PointerEventData eventData) {
            //bind the position to the screen;
            var pos = eventData.position;
            pos = UIController.RectTransform.ClampToRect(pos);
            //calculate the new size
            var off = (pos - _offset) / UIController.Canvas.scaleFactor;
            //fix the offset
            if(_edge.HasBottom()) off.y *= -1;
            if(_edge.HasLeft()) off.x *= -1;
            var newSize = _size + off;
            newSize = new Vector2(Mathf.Max(newSize.x, Component.MinSize.x),
                Mathf.Max(newSize.y, Component.MinSize.y));
            //fix single access resize
            if(_edge is ComponentEdge.Top or ComponentEdge.Bottom) newSize.x = _size.x;
            if(_edge is ComponentEdge.Left or ComponentEdge.Right) newSize.y = _size.y;
            //set size and restore position;
            RectTransform.sizeDelta = newSize;
            _edge.GetOpposite().SetPosition(RectTransform,_position);
        }

        public override void OnPointerDown(PointerEventData eventData) {
            //reset the resize
            if(eventData.button == PointerEventData.InputButton.Right) {
                RectTransform.sizeDelta = _size;
                _edge.GetOpposite().SetPosition(RectTransform,_position);
            }
            Component.SetState(UIStatesType.Idle, null);
        }

        public override void OnEndDrag(PointerEventData eventData) {
            Component.SetState(UIStatesType.Idle,null);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        
    }
    
}
