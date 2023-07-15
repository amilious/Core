
using System;
using UnityEngine;
using Amilious.Core.UI.Cursors;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI.Component.States {
    
    public class Idle : UIState {

        private Vector2 _pointerDown;
        
        public Idle(UIComponent component) : base(component, UIStatesType.Idle) { }

        public override void OnExitState(UIStatesType nextState) { }

        public override void OnEnterState(UIStatesType previousState, PointerEventData pointerData) { }

        public override void OnPointerEnter(PointerEventData eventData, bool softEnter = false) {
            CursorController.SetCursor(DefaultCursors.Hand);
        }

        public override void OnPointerExit(PointerEventData eventData, bool softExit = false) {
            CursorController.ReturnToDefault();
        }

        public override void OnBeginDrag(PointerEventData eventData) {
            eventData.position = _pointerDown; //used the mouse down position
            //check if on edge
            if(Component.Resizable && Component.GetEdge(eventData.position)!= ComponentEdge.None) {
                //switch to resizing
                Component.SetState(UIStatesType.Resizing, eventData);
            } else if(Component.Movable) 
                Component.SetState(UIStatesType.Moving,eventData);
            eventData.dragging = false;
        }

        public override void OnPointerDown(PointerEventData eventData) {
            _pointerDown = eventData.position; //store the mouse down position.
        }

        public override void OnPointerMove(PointerEventData eventData, bool overChild) {
            if(overChild) return;
            var edge = Component.GetEdge(eventData.position);
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
            else CursorController.SetCursor(DefaultCursors.Hand);
        }
    }
    
}