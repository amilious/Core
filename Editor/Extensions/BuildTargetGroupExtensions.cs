using System;
using UnityEditor;

namespace Amilious.Core.Editor.Extensions {
    
    public static class BuildTargetGroupExtensions {

        /// <summary>
        /// This field is used to cache the build targets.
        /// </summary>
        private static BuildTarget[] _buildTargets;

        /// <summary>
        /// This method is used to check if the provided group is currently supported.
        /// </summary>
        /// <param name="targetGroup">The group that you want to check.</param>
        /// <returns>True if the given group is supported, otherwise false.</returns>
        public static bool IsSupported(this BuildTargetGroup targetGroup) {
            _buildTargets ??= (BuildTarget[])Enum.GetValues(typeof(BuildTarget));
            foreach (var target in _buildTargets)
                if (BuildPipeline.IsBuildTargetSupported(targetGroup, target)) return true;
            return false;
        }
        
    }
}