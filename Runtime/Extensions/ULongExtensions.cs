namespace Amilious.Core.Extensions {
    public static class ULongExtensions {
        
        /// <summary>
        /// This method is used to get two int values that are packed into a long.
        /// </summary>
        /// <param name="packedLong">The packed ulong.</param>
        /// <param name="value1">The first uint value.</param>
        /// <param name="value2">The second uint value.</param>
        public static void UnpackUInts(this ulong packedLong, out uint value1, out uint value2) {
            value1 = (uint)(packedLong >> 32);
            value2 = (uint)(packedLong & 0xFFFFFFFF);
        }
        
    }
}