using System;
using Amilious.Console;

namespace Cysharp.Text {

    public partial struct Utf16ValueStringBuilder {
        public void AppendStyle<T>(StyleFormat format, T text, bool endSpace = false) {
            AppendFormat(format.FormatString(),text);
            if(endSpace)Append(SBHelp.SPACE);
        }

        public void AppendStyleFormat<T1, T2, T3, T4, T5>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4,arg5));
        }
        
        public void AppendStyleFormat<T1, T2, T3, T4>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4));
        }
        
        public void AppendStyleFormat<T1, T2, T3>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3));
        }
        
        public void AppendStyleFormat<T1, T2>(StyleFormat style, string format, T1 arg1, T2 arg2) {
            AppendStyle(style,ZString.Format(format,arg1,arg2));
        }
        
        public void AppendStyleFormat<T1>(StyleFormat style, string format, T1 arg1) {
            AppendStyle(style,ZString.Format(format,arg1));
        }

        public void AppendStyleFormat<T1, T2, T3, T4, T5>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4,arg5));
        }
        
        public void AppendStyleFormat<T1, T2, T3, T4>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4));
        }
        
        public void AppendStyleFormat<T1, T2, T3>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3));
        }
        
        public void AppendStyleFormat<T1, T2>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2) {
            AppendStyle(style,ZString.Format(format,arg1,arg2));
        }
        
        public void AppendStyleFormat<T1>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1) {
            AppendStyle(style,ZString.Format(format,arg1));
        }
        
    }
    
    public partial struct Utf8ValueStringBuilder {
        
        public void AppendStyle<T>(StyleFormat format, T text, bool endSpace = false) {
            AppendFormat(format.FormatString(),text);
            if(endSpace)Append(SBHelp.SPACE);
        }
        
        public void AppendStyleFormat<T1, T2, T3, T4, T5>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4,arg5));
        }
        
        public void AppendStyleFormat<T1, T2, T3, T4>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4));
        }
        
        public void AppendStyleFormat<T1, T2, T3>(StyleFormat style, string format, T1 arg1, T2 arg2, T3 arg3) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3));
        }
        
        public void AppendStyleFormat<T1, T2>(StyleFormat style, string format, T1 arg1, T2 arg2) {
            AppendStyle(style,ZString.Format(format,arg1,arg2));
        }
        
        public void AppendStyleFormat<T1>(StyleFormat style, string format, T1 arg1) {
            AppendStyle(style,ZString.Format(format,arg1));
        }

        public void AppendStyleFormat<T1, T2, T3, T4, T5>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4,arg5));
        }
        
        public void AppendStyleFormat<T1, T2, T3, T4>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3,arg4));
        }
        
        public void AppendStyleFormat<T1, T2, T3>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2, T3 arg3) {
            AppendStyle(style,ZString.Format(format,arg1,arg2,arg3));
        }
        
        public void AppendStyleFormat<T1, T2>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1, T2 arg2) {
            AppendStyle(style,ZString.Format(format,arg1,arg2));
        }
        
        public void AppendStyleFormat<T1>(StyleFormat style, ReadOnlySpan<char> format, T1 arg1) {
            AppendStyle(style,ZString.Format(format,arg1));
        }
        
    }


    
}