using System;
using UnityEngine;
using Cysharp.Text;
using System.Collections.Generic;

namespace Amilious.Console {
    
    /// <summary>
    /// This enum is used to represent the styles that can be applied to text.
    /// </summary>
    public enum StyleFormat {
        Normal, Command, Group, Parameter, Description, Type, Value,
        Bullet, Selector, Selected, Suggestion, DebugLog, DebugAssert,
        DebugWarning, DebugError, DebugException, StackTrace, Invalid,
        Title, UserEnteredCommand, UserEnteredText, Server, TimeString,
        User,Friend,PrivateSentText, PrivateReceivedText
    }

    /// <summary>
    /// This extension adds a format cache that contains the format strings for
    /// all the style format.
    /// </summary>
    public static class StyleFormatExtension {
        
        #region Instance Variables /////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly Dictionary<StyleFormat, string> FormatCache = new Dictionary<StyleFormat, string>();

        private static readonly Dictionary<StyleFormat, string>
            StartingTagCache = new Dictionary<StyleFormat, string>();
        
        #endregion
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This static constructor will be called once when the object is first created.
        /// </summary>
        static StyleFormatExtension() {
            //build the cache
            foreach(StyleFormat val in Enum.GetValues(typeof(StyleFormat))) {
                FormatCache[val] = ZString.Concat("<style=",Enum.GetName(typeof(StyleFormat), val),">{0}</style>");
                StartingTagCache[val] = ZString.Concat("<style=",Enum.GetName(typeof(StyleFormat), val),'>');
            }
        }
        
        #endregion

        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the style format associated with a log type.
        /// </summary>
        /// <param name="logType">The log type that you want to get the style format for.</param>
        /// <returns>The style format of the give log type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if unknown log type.</exception>
        public static StyleFormat StyleFormat(this LogType logType) {
            switch(logType) {
                case LogType.Error: return Console.StyleFormat.DebugError;
                case LogType.Assert: return Console.StyleFormat.DebugAssert;
                case LogType.Warning: return Console.StyleFormat.DebugWarning;
                case LogType.Log: return Console.StyleFormat.DebugLog;
                case LogType.Exception: return Console.StyleFormat.DebugException;
                default: throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        /// <summary>
        /// This method is used to get the closing tag for the style.
        /// </summary>
        /// <param name="format">The style format that you want to use.</param>
        /// <returns>The closing tag for the given style format.</returns>
        public static string FormatString(this StyleFormat format) => FormatCache[format];
        
        /// <summary>
        /// This method is used to get the opening tag for the style.
        /// </summary>
        /// <param name="format">The style format that you want to use.</param>
        /// <returns>The opening tag for the given style format.</returns>
        public static string OpenStyleTag(this StyleFormat format) => StartingTagCache[format];

        #endregion

    }
}