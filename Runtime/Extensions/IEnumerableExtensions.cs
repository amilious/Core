using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to IEnumerables.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly MethodInfo RawConvertMethod;
        
        private static readonly Dictionary<Type, MethodInfo> ConvertMethods = 
            new Dictionary<Type, MethodInfo>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        static IEnumerableExtensions() {
            RawConvertMethod = typeof(IEnumerableExtensions).GetMethod(nameof(ConvertToGeneric),
                BindingFlags.Static | BindingFlags.NonPublic);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to convert a collection of objects to a params array of the given type.
        /// </summary>
        /// <param name="collection">The collection of same type values.</param>
        /// <param name="type">The type of the values in the collection.</param>
        /// <returns>An object that is a converted array for the given type.</returns>
        public static object ConvertToParamsArray(this IEnumerable<object> collection, Type type) {
            if(ConvertMethods.TryGetValueFix(type, out var converter))
                return converter.Invoke(null, new object[] { collection });
            converter = RawConvertMethod.MakeGenericMethod(type);
            ConvertMethods[type] = converter;
            return converter.Invoke(null,new object[]{collection});
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the base method that will be made generic for the <see cref="ConvertToParamsArray"/> method.
        /// </summary>
        /// <param name="collection">The collection of same type objects that you want to convert into a params
        /// array.</param>
        /// <typeparam name="T">The type of the params array!</typeparam>
        /// <returns>The collection converted into a params array!</returns>
        private static T[] ConvertToGeneric<T>(IEnumerable<object> collection) => collection.Cast<T>().ToArray();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}