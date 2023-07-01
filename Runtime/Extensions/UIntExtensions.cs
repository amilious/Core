namespace Amilious.Core.Extensions {
    public static class UIntExtensions {
        
        /// <summary>
        /// This method is used to pack two int values into a long.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The two values packed as a long.</returns>
        public static ulong PackWith(this uint value1, uint value2) => ((ulong)value1) << 32 | ((ulong)value2 & 0xFFFFFFFF);
        
    }
}