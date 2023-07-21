
using System;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Drawing;

namespace Amilious.Core.UI.Cursors {
    
    [DisallowMultipleComponent, AddComponentMenu("Amilious/UI/Cursor Controller")]
    public class CursorController : AmiliousBehavior {

        private const string OPTIONS = "Options";
        private const string DEFAULT_CURSORS = "Default Cursors";

        [SerializeField, AmiTab(OPTIONS)] private CursorMode cursorMode = CursorMode.Auto;
        [SerializeField, AmiTab(OPTIONS), AmiBool(true)]
        [AmiShowIf(nameof(cursorMode), CursorMode.ForceSoftware)] private bool forceAutoInEditor = true;
        [SerializeField, AmiTab(OPTIONS), Range(.1f,2f),  AmiShowIf(nameof(cursorMode),CursorMode.ForceSoftware)] 
        private float cursorScale = 1;
        [SerializeField, AmiTab((OPTIONS))] private CursorInfo defaultCursor;

        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo arrow;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo hand;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo finger;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo drag;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo arrowHourglass;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo arrowClock;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo clock;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo hourglass;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo resizeUpRight;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo resizeUpLeft;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo moveArrow;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo arrowPlus;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo arrowMinus;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo zoomIn;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo zoomOut;
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo resizeVertical; 
        [SerializeField, AmiTab(DEFAULT_CURSORS)] private CursorInfo resizeHorizontal; 

        private static readonly Dictionary<CursorInfo, Texture2D> TextureCache = new();

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public static CursorInfo PreviousCursor { get; private set; }
        
        public static CursorInfo CurrentCursor { get; private set; }

        public static CursorController Instance { get; private set; }

        public static CursorMode CursorMode {
            get => Instance.cursorMode;
            set {
                if(Instance.cursorMode == value) return;
                Instance.cursorMode = value;
                SetCursor(CurrentCursor,true);
            }
        }
        
        public static CursorLockMode LockMode {
            get => Cursor.lockState;
            set => Cursor.lockState = value;
        }
        
        public static bool IsVisible {
            get => Cursor.visible;
            set => Cursor.visible = value;
        }

        public static float CursorScale {
            get => Instance.cursorScale;
            set {
                var tmp = Mathf.Clamp(value, 0.1f, 2f);
                if(Math.Abs(Instance.cursorScale - tmp) < .001) return;
                Instance.cursorScale = tmp;
                TextureCache.Clear();
                SetCursor(CurrentCursor,true);
            }
        }

        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            Debug.Log($"Screens {Display.displays.Length}");
            if(Instance != null && Instance != this) {
                Destroy(this);
                return;
            }
            Instance = this;
            //load missing cursors
            if(arrow == null) arrow = Resources.Load<CursorInfo>("Cursors/Arrow");
            if(hand == null) hand = Resources.Load<CursorInfo>("Cursors/Hand");
            if(finger == null) finger = Resources.Load<CursorInfo>("Cursors/Finger");
            if(drag == null) drag = Resources.Load<CursorInfo>("Cursors/Drag");
            if(arrowHourglass == null) arrowHourglass = Resources.Load<CursorInfo>("Cursors/ArrowHourglass");
            if(arrowClock == null) arrowClock = Resources.Load<CursorInfo>("Cursors/ArrowClock");
            if(clock == null) clock = Resources.Load<CursorInfo>("Cursors/Clock");
            if(hourglass == null) hourglass = Resources.Load<CursorInfo>("Cursors/Hourglass");
            if(resizeUpRight == null) resizeUpRight = Resources.Load<CursorInfo>("Cursors/ResizeUpRight");
            if(resizeUpLeft == null) resizeUpLeft = Resources.Load<CursorInfo>("Cursors/ResizeUpLeft");
            if(moveArrow == null) moveArrow = Resources.Load<CursorInfo>("Cursors/MoveArrow");
            if(arrowPlus == null) arrowPlus = Resources.Load<CursorInfo>("Cursors/ArrowPlus");
            if(arrowMinus == null) arrowMinus = Resources.Load<CursorInfo>("Cursors/ArrowMinus");
            if(zoomIn == null) zoomIn = Resources.Load<CursorInfo>("Cursors/ZoomIn");
            if(zoomOut == null) zoomOut = Resources.Load<CursorInfo>("Cursors/ZoomOut");
            if(resizeVertical == null) resizeVertical = Resources.Load<CursorInfo>("Cursors/ResizeVertical");
            if(resizeHorizontal == null) resizeHorizontal = Resources.Load<CursorInfo>("Cursors/ResizeHorizontal");
            if(defaultCursor == null) defaultCursor = arrow;
            SetCursor(defaultCursor);
        }

        public static void SetCursor(CursorInfo cursorInfo, bool force=false) {
            if(Instance == null) return;
            var mode = Instance.forceAutoInEditor && Application.isEditor ? CursorMode.Auto : CursorMode;
            if(cursorInfo == CurrentCursor && !force) return;
            if(cursorInfo != CurrentCursor) PreviousCursor = CurrentCursor;
            var texture = cursorInfo.Texture;
            var hotSpot = cursorInfo.HotSpot;
            if(Math.Abs(CursorScale - 1) > .001f&&mode == CursorMode.ForceSoftware){
                if(!TextureCache.TryGetValue(cursorInfo, out texture)) {
                    texture = cursorInfo.Texture.ScaleTexture(CursorScale);
                    TextureCache.Add(cursorInfo, texture);
                }
                hotSpot *= CursorScale;
            }
            Cursor.SetCursor(texture, hotSpot, mode);
            CurrentCursor = cursorInfo;
        }

        public static void SetCursor(Texture2D texture, Vector2 hotSpot) {
            if(Instance == null) return;
            var cursorInfo = CursorInfo.CreateInstance(texture, hotSpot);
            SetCursor(cursorInfo);
        }
        public static void SetCursor(DefaultCursors cursor) {
            if(Instance == null) return;
            switch(cursor) {
                case DefaultCursors.Arrow: SetCursor(Instance.arrow); break;
                case DefaultCursors.Hand: SetCursor(Instance.hand); break;
                case DefaultCursors.Finger: SetCursor(Instance.finger); break;
                case DefaultCursors.Drag: SetCursor(Instance.drag); break;
                case DefaultCursors.ArrowHourglass: SetCursor(Instance.arrowHourglass); break;
                case DefaultCursors.ArrowClock: SetCursor(Instance.arrowClock); break;
                case DefaultCursors.Clock: SetCursor(Instance.clock); break;
                case DefaultCursors.Hourglass: SetCursor(Instance.hourglass); break;
                case DefaultCursors.ResizeUpRight: SetCursor(Instance.resizeUpRight); break;
                case DefaultCursors.ResizeUpLeft: SetCursor(Instance.resizeUpLeft); break;
                case DefaultCursors.MoveArrow: SetCursor(Instance.moveArrow); break;
                case DefaultCursors.ArrowPlus: SetCursor(Instance.arrowPlus); break;
                case DefaultCursors.ArrowMinus: SetCursor(Instance.arrowMinus); break;
                case DefaultCursors.ZoomIn: SetCursor(Instance.zoomIn); break;
                case DefaultCursors.ZoomOut: SetCursor(Instance.zoomOut); break;
                case DefaultCursors.ResizeVertical: SetCursor(Instance.resizeVertical); break;
                case DefaultCursors.ResizeHorizontal: SetCursor(Instance.resizeHorizontal); break;
                default:  SetCursor(Instance.arrow); break;
            }
        }

        public static void ReturnToDefault() {
            if(Instance == null) return;
            SetCursor(Instance.defaultCursor);
        }

        public static void ReturnToPrevious() {
            if(Instance == null) return;
            SetCursor(PreviousCursor!=null?PreviousCursor:Instance.defaultCursor);
            PreviousCursor = null;
        }
        
    }
    
    
}