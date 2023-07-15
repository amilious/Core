using UnityEditor;
using Amilious.Core.Editor.Editors;

namespace Amilious.Core.FishNet.Editor {
    
    /// <summary>
    /// This class is used as the default editor for an <see cref="AmiliousNetworkBehavior"/>.
    /// </summary>
    [CustomEditor(typeof(AmiliousNetworkBehavior), true, isFallback = true)]
    public class AmiliousNetworkBehaviorEditor : AmiliousEditor { }
        
}