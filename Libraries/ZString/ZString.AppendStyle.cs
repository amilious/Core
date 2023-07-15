using System;
using System.Runtime.CompilerServices;
using Amilious.Console;

namespace Cysharp.Text {
    public static partial class ZString {
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Style<T1>(StyleFormat style, T1 text) {
            var sb = new Utf16ValueStringBuilder(false);
            try {
                sb.Append(text);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1>(StyleFormat style, string format, T1 arg1) {
            var sb = new Utf16ValueStringBuilder(false);
            try {
                sb.AppendFormat(format,arg1);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2>(StyleFormat style, string format, T1 arg1, T2 arg2) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, 
            T2 arg2, T3 arg3) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4>(StyleFormat style, string format, T1 arg1, T2 arg2, 
            T3 arg3, T4 arg4) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3, arg4);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }
            finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4>(StyleFormat style, ReadOnlySpan<char> format, 
            T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3, arg4);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5>(StyleFormat style, string format, T1 arg1, T2 arg2, 
            T3 arg3, T4 arg4, T5 arg5) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3, arg4, arg5);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5>(StyleFormat style, ReadOnlySpan<char> format, 
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3, arg4, arg5);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6>(StyleFormat style, string format, T1 arg1, T2 arg2, 
            T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3, arg4, arg5, arg6);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6>(StyleFormat style, ReadOnlySpan<char> format, 
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1, arg2, arg3, arg4, arg5, arg6);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
            T11 arg11) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10,T11>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10, T11 arg11) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
            T11 arg11, T12 arg12) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10,T11,T12>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10, T11 arg11, T12 arg12) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
            T11 arg11, T12 arg12, T13 arg13) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10,T11,T12,T13>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
            T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13,arg14);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10,T11,T12,T13,T14>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13,arg14);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
            T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13,arg14,arg15);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13,arg14,arg15);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(StyleFormat style, 
            string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
            T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13,arg14,arg15,arg16);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }

        /// <summary>Replaces one or more format items in a string with the string representation of some specified objects.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StyleFormat<T1, T2, T3, T4, T5, T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(StyleFormat style, 
            ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, 
            T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
            var sb = new Utf16ValueStringBuilder(true);
            try {
                sb.AppendFormat(format,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10,arg11,arg12,arg13,arg14,arg15,arg16);
                sb.AddTagToAll(style.OpenStyleTag(),"</style>");
                return sb.ToString();
            }finally {
                sb.Dispose();
            }
        }
        
    }
}