using System;
using System.Reflection;

namespace Amilious.Core.Extensions {
    public static class ParameterInfoExtensions {

        /// <summary>
        /// This method is used to check if a method parameter is a params array.
        /// </summary>
        /// <param name="paramInfo">The parameter info.</param>
        /// <returns>True if the parameter is params, otherwise false.</returns>
        public static bool IsParams(this ParameterInfo paramInfo) {
            return paramInfo.IsDefined(typeof(ParamArrayAttribute), false);
        }
        
    }
}