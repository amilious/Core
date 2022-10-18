using Amilious.Core.Editor.Editors;
using Amilious.Core.FishNet.Authentication;
using UnityEditor;

namespace Amilious.Core.FishNet.Editor {
    
    [CustomEditor(typeof(AmiliousAuthenticator),editorForChildClasses:true,isFallback = true)]
    public class AmiliousAuthenticatorEditor : AmiliousEditor { }
    
}