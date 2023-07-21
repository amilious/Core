using Amilious.Core.Attributes;
using Amilious.Core.Editor.Editors.Tabs;
using Amilious.Core.UI.Graph;
using Amilious.Core.UI.Layout;
using UnityEditor;

namespace Amilious.Core.Editor.Editors {
    
    /// <summary>
    /// This editor is used for displaying the <see cref="FlexibleGridLayout"/> in the inspector.
    /// </summary>
    [CustomEditor(typeof(UILineRenderer),true, isFallback = true)]
    public class UILineRendererEditor : AmiliousEditor {
        
        #region Override Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc/>
        protected override void Initialize() {
            TabController.AddToGroup(new TabProperty(new AmiTabAttribute("Graphic"), serializedObject.FindProperty("m_Material")));
            TabController.AddToGroup(new TabProperty(new AmiTabAttribute("Graphic"), serializedObject.FindProperty("m_Color")));
            TabController.AddToGroup(new TabProperty(new AmiTabAttribute("Graphic"), serializedObject.FindProperty("m_RaycastTarget")));
            TabController.AddToGroup(new TabProperty(new AmiTabAttribute("Graphic"), serializedObject.FindProperty("m_RaycastPadding")));
            base.Initialize();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}