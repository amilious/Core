using System;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.UI.Cursors {
    
    [CreateAssetMenu(menuName = "Amilious/UI/Cursor Info", order = -1)]
    public class CursorInfo : ScriptableObject {

        [SerializeField] private Texture2D texture;
        [SerializeField] private Vector2 hotSpot;

        public Texture2D Texture => texture;

        public Vector2 HotSpot => hotSpot;


        public static CursorInfo CreateInstance(Texture2D texture, Vector2 hotSpot) {
            var instance = CreateInstance<CursorInfo>();
            instance.texture = texture;
            instance.hotSpot = hotSpot;
            return instance;
        }
        

        #if UNITY_EDITOR 
        
        private void OnValidate() {
            if(!Application.isPlaying) return;
            if(CursorController.Instance == null) return;
            if(CursorController.CurrentCursor==this)
                CursorController.SetCursor(this,true);
        }
        
        #endif
        
    }
    
}