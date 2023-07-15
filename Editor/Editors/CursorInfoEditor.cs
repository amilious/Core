
using Amilious.Core.Extensions;
using UnityEditor;
using UnityEngine;
using Amilious.Core.UI.Cursors;
using UnityEngine.SceneManagement;

namespace Amilious.Core.Editor.Editors {
    
    [CustomEditor(typeof(CursorInfo), true)]
    public class CursorInfoEditor : AmiliousEditor {

        private CursorInfo _item;

        private CursorInfo Item {
            get {
                if(_item != null) return _item;
                _item = target as CursorInfo;
                return _item;
            }
        }

        protected override void BeforeDefaultDraw() {
            base.BeforeDefaultDraw();
            GUILayout.Label(Item.Texture);
        }

        protected override void AfterDefaultDraw() {
            base.AfterDefaultDraw();
            if(Application.isPlaying&&CursorController.Instance != null&&CursorController.Instance) {
                if(GUILayout.Button("Set As Active")) CursorController.SetCursor(Item,true);
            }
        }

        /// <inheritdoc />
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
            if(Item == null||Item.Texture==null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
            return Item.Texture == null ? base.RenderStaticPreview(assetPath, subAssets, width, height) :  
                Item.Texture.ScaleTexture(width,height);
        }
        
    }
    
}