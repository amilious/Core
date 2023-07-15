using System.Collections.Concurrent;
using UnityEngine;

namespace Cysharp.Text {

	// ReSharper disable InconsistentNaming
	// ReSharper disable IdentifierTypo
	
    public static class SBHelp {

	    public const string APPEND_B_FORMAT = "<b>{0}</b>";
        public const string APPEND_I_FORMAT = "<i>{0}</i>";
        public const string APPEND_S_FORMAT = "<S>{0}</S>";
        public const string APPEND_U_FORMAT = "<u>{0}</u>";
        public const string APPEND_BI_FORMAT = "<b><i>{0}</i></b>";
        public const string APPEND_BS_FORMAT = "<b><S>{0}</S></b>";
        public const string APPEND_BU_FORMAT = "<b><u>{0}</u></b>";
        public const string APPEND_IS_FORMAT = "<i><S>{0}</S></i>";
        public const string APPEND_IU_FORMAT = "<i><u>{0}</u></i>";
        public const string APPEND_US_FORMAT = "<u><S>{0}</S></u>";
        public const string APPEND_C_FORMAT = "<color=#{0}>{1}</color>";
        public const string APPEND_BIS_FORMAT = "<b><i><S>{0}</S></i></b>";
        public const string APPEND_BIU_FORMAT = "<b><i><u>{0}</u></i></b>";
        public const string APPEND_BUS_FORMAT = "<b><u><S>{0}</S></u></b>";
        public const string APPEND_IUS_FORMAT = "<i><u><S>{0}</S></u></i>";
        public const string APPEND_L_FORMAT = "<link id=\"{0}\">{1}</link>";
        public const string APPEND_CB_FORMAT = "<color=#{0}><b>{1}</b></color>";
        public const string APPEND_CI_FORMAT = "<color=#{0}><i>{1}</i></color>";
        public const string APPEND_CS_FORMAT = "<color=#{0}><S>{1}</S></color>";
        public const string APPEND_CU_FORMAT = "<color=#{0}><u>{1}</u></color>";
        public const string APPEND_BIUS_FORMAT = "<b><i><u><S>{0}</S></u></i></b>";
        public const string APPEND_LB_FORMAT = "<link id=\"{0}\"><b>{1}</b></link>";
        public const string APPEND_LI_FORMAT = "<link id=\"{0}\"><i>{1}</i></link>";
        public const string APPEND_LS_FORMAT = "<link id=\"{0}\"><S>{1}</S></link>";
        public const string APPEND_LU_FORMAT = "<link id=\"{0}\"><u>{1}</u></link>";
        public const string APPEND_CBI_FORMAT = "<color=#{0}><b><i>{1}</i></b></color>";
        public const string APPEND_CBS_FORMAT = "<color=#{0}><b><S>{1}</S></b></color>";
        public const string APPEND_CBU_FORMAT = "<color=#{0}><b><u>{1}</u></b></color>";
        public const string APPEND_CIS_FORMAT = "<color=#{0}><i><S>{1}</S></i></color>";
        public const string APPEND_CIU_FORMAT = "<color=#{0}><i><u>{1}</u></i></color>";
        public const string APPEND_CUS_FORMAT = "<color=#{0}><u><S>{1}</S></u></color>";
        public const string APPEND_LBI_FORMAT = "<link id=\"{0}\"><b><i>{1}</i></b></link>";
        public const string APPEND_LBS_FORMAT = "<link id=\"{0}\"><b><S>{1}</S></b></link>";
        public const string APPEND_LBU_FORMAT = "<link id=\"{0}\"><b><u>{1}</u></b></link>";
        public const string APPEND_LIS_FORMAT = "<link id=\"{0}\"><i><S>{1}</S></i></link>";
        public const string APPEND_LIU_FORMAT = "<link id=\"{0}\"><i><u>{1}</u></i></link>";
        public const string APPEND_LUS_FORMAT = "<link id=\"{0}\"><u><S>{1}</S></u></link>";
        public const string APPEND_CBIS_FORMAT = "<color=#{0}><b><i><S>{1}</S></i></b></color>";
        public const string APPEND_CBIU_FORMAT = "<color=#{0}><b><i><u>{1}</u></i></b></color>";
        public const string APPEND_CBUS_FORMAT = "<color=#{0}><b><u><S>{1}</S></u></b></color>";
        public const string APPEND_CIUS_FORMAT = "<color=#{0}><i><u><S>{1}</S></u></i></color>";
        public const string APPEND_LC_FORMAT = "<link id=\"{0}\"><color=#{1}>{2}</color></link>";
        public const string APPEND_LBIS_FORMAT = "<link id=\"{0}\"><b><i><S>{1}</S></i></b></link>";
        public const string APPEND_LBIU_FORMAT = "<link id=\"{0}\"><b><i><u>{1}</u></i></b></link>";
        public const string APPEND_LBUS_FORMAT = "<link id=\"{0}\"><b><u><S>{1}</S></u></b></link>";
        public const string APPEND_LIUS_FORMAT = "<link id=\"{0}\"><i><u><S>{1}</S></u></i></link>";
        public const string APPEND_CBIUS_FORMAT = "<color=#{0}><b><i><u><S>{1}</S></u></i></b></color>";
        public const string APPEND_LCB_FORMAT = "<link id=\"{0}\"><color=#{1}><b>{2}</b></color></link>";
        public const string APPEND_LCI_FORMAT = "<link id=\"{0}\"><color=#{1}><i>{2}</i></color></link>";
        public const string APPEND_LCS_FORMAT = "<link id=\"{0}\"><color=#{1}><S>{2}</S></color></link>";
        public const string APPEND_LCU_FORMAT = "<link id=\"{0}\"><color=#{1}><u>{2}</u></color></link>";
        public const string APPEND_LBIUS_FORMAT = "<link id=\"{0}\"><b><i><u><S>{1}</S></u></i></b></link>";
        public const string APPEND_LCBI_FORMAT = "<link id=\"{0}\"><color=#{1}><b><i>{2}</i></b></color></link>";
        public const string APPEND_LCBS_FORMAT = "<link id=\"{0}\"><color=#{1}><b><S>{2}</S></b></color></link>";
        public const string APPEND_LCBU_FORMAT = "<link id=\"{0}\"><color=#{1}><b><u>{2}</u></b></color></link>";
        public const string APPEND_LCIS_FORMAT = "<link id=\"{0}\"><color=#{1}><i><S>{2}</S></i></color></link>";
        public const string APPEND_LCIU_FORMAT = "<link id=\"{0}\"><color=#{1}><i><u>{2}</u></i></color></link>";
        public const string APPEND_LCUS_FORMAT = "<link id=\"{0}\"><color=#{1}><u><S>{2}</S></u></color></link>";
        public const string APPEND_LCBIS_FORMAT = "<link id=\"{0}\"><color=#{1}><b><i><S>{2}</S></i></b></color></link>";
        public const string APPEND_LCBIU_FORMAT = "<link id=\"{0}\"><color=#{1}><b><i><u>{2}</u></i></b></color></link>";
        public const string APPEND_LCBUS_FORMAT = "<link id=\"{0}\"><color=#{1}><b><u><S>{2}</S></u></b></color></link>";
        public const string APPEND_LCIUS_FORMAT = "<link id=\"{0}\"><color=#{1}><i><u><S>{2}</S></u></i></color></link>";
        public const string APPEND_LCBIUS_FORMAT = "<link id=\"{0}\"><color=#{1}><b><i><u><S>{2}</S></u></i></b></color></link>";

        public const string START_COLOR = "<color=#{0}>";
        public const string START_LINK = "<link=\"{0}\">";
        public const string START_MARK = "<mark=#{0}>";
        public const string START_NO_PARSE = "<noparse>";
        public const string START_BOLD = "<b>";
        public const string START_ITALIC = "<i>";
        public const string START_UNDERLINE = "<u>";
        public const string START_STRIKETHROUGH = "<s>";
        public const string END_COLOR = "</color>";
        public const string END_LINK = "</link>";
        public const string END_MARK = "</mark>";
        public const string END_NO_PARSE = "</parse>";
        public const string END_BOLD = "</b>";
        public const string END_ITALIC = "</i>";
        public const string END_UNDERLINE = "</u>";
        public const string END_STRIKETHROUGH = "</s>";

        public const string SUPERSCRIPT_FORMAT = "<sup>{0}</sup>";
        public const string SUBSCRIPT_FORMAT = "<sub>{0}</sub>";
        public const string NO_PARSE_FORMAT = "<noparse>{0}</noparse>";
        public const string MARK_FORMAT = "<mark=#{0}>{1}</mark>";
        public const string ALIGN_RIGHT = "<align=\"right\">";
        public const string ALIGN_LEFT = "<align=\"left\">";
        public const string ALIGN_CENTER = "<align=\"center\">";
        public const string RESET_ALIGN = "</align>";

        public const string INDENT_FORMAT = "<indent={0}{1}>";
        public const string INDENT_END = "</indent>";
        public const string LINE_INDENT_FORMAT = "<line-indent={0}{1}>";
        public const string END_LINE_INDENT = "</line-indent>";
        public const string POSITION_FORMAT = "<pos={0}{1}> ";
        public const string MARGIN_FORMAT = "<margin={0}{1}>";
        public const string MARGIN_LEFT_FORMAT = "<margin-left={0}{1}>";
        public const string MARGIN_RIGHT_FORMAT = "<margin-right={0}{1}>";
        public const string END_MARGIN = "</margin>";
        public const string SPRITE_INDEX_FORMAT = "<sprite index={0}>";
        public const string SPRITE_NAME_FORMAT = "<sprite name=\"{0}\">";
        
        
        public const char SPACE = ' ';
        public const char NL = '\n';
        public const char TAB = '\t';
        
        private static readonly ConcurrentDictionary<Color, string> CachedColors = 
            new ConcurrentDictionary<Color, string>();
        
        public static string GetCachedColor(Color color) {
            //try get cashed color
            if(CachedColors.TryGetValue(color, out var colorCode)) return colorCode;
            //generate and cache the color code
            colorCode = ColorUtility.ToHtmlStringRGBA(color);
            CachedColors.TryAdd(color, colorCode);
            return colorCode;
        }
        
    }

    public enum Align{Left,Right,Center, Reset}
    public enum SizeType { Pixel, FontUnits, Percentage }
    public enum MarginType {Left, Right, Both}

    public static class SBEnumExtensions {
	    public static char SizeChar(this SizeType sizeType) {
		    switch(sizeType) {
			    case SizeType.Pixel: return 'p';
			    case SizeType.FontUnits: return 'e';
			    case SizeType.Percentage: return '%';
			    default: return 'p';
		    }
	    }
    }
    
    
    public partial struct Utf16ValueStringBuilder {
        
	    public static readonly Utf16PreparedFormat<float, char> IndentFormatter =
		    new Utf16PreparedFormat<float, char>(SBHelp.INDENT_FORMAT);
	    public static readonly Utf16PreparedFormat<float, char> LineIndentFormatter =
		    new Utf16PreparedFormat<float, char>(SBHelp.LINE_INDENT_FORMAT);
	    public static readonly Utf16PreparedFormat<float, char> PositionFormatter =
		    new Utf16PreparedFormat<float, char>(SBHelp.POSITION_FORMAT);
	    public static readonly Utf16PreparedFormat<float, char> MarginFormatter =
		    new Utf16PreparedFormat<float, char>(SBHelp.MARGIN_FORMAT);
	    public static readonly Utf16PreparedFormat<float, char> MarginLeftFormatter =
		    new Utf16PreparedFormat<float, char>(SBHelp.MARGIN_LEFT_FORMAT);
	    public static readonly Utf16PreparedFormat<float, char> MarginRightFormatter =
		    new Utf16PreparedFormat<float, char>(SBHelp.MARGIN_RIGHT_FORMAT);
	    public static readonly Utf16PreparedFormat<int> SpriteIndexFormatter =
		    new Utf16PreparedFormat<int>(SBHelp.SPRITE_INDEX_FORMAT);
	    
	    /// <summary>
	    /// This method is used to add a color to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want to add to the builder's contents.</param>
	    /// <seealso cref="AddColorToAll(string)"/>
	    public void AddColorToAll(Color color) =>
		    AddTagToAll(ZString.Format(SBHelp.START_COLOR,SBHelp.GetCachedColor(color)),SBHelp.END_COLOR);

	    /// <summary>
	    /// This method is used to add a color to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want to add to the builder's contents.</param>
	    /// <seealso cref="AddColorToAll(UnityEngine.Color)"/>
	    public void AddColorToAll(string color) => 
		    AddTagToAll(ZString.Format(SBHelp.START_COLOR,color),SBHelp.END_COLOR);

	    /// <summary>
	    /// This method is used to add a starting color tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartColor(string)"/>
	    public void StartColor(Color color) =>
		    AppendFormat(SBHelp.START_COLOR, SBHelp.GetCachedColor(color));

	    /// <summary>
	    /// This method is used to add a starting color tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartColor(string)"/>
	    public void StartColor(string color)  =>
		    AppendFormat(SBHelp.START_COLOR, color);

	    /// <summary>
	    /// This method is used to add a ending color tag for TextMeshPro.
	    /// </summary>
	    public void EndColor() => Append(SBHelp.END_COLOR);

	    /// <summary>
	    /// This method is used to add a starting link tag for TextMeshPro.
	    /// </summary>
	    /// <param name="id">The id you want the link tag to use.</param>
		public void StartLink(params string[] id) {
		    if(id==null||id.Length==0) Append("<link>");
		    else if(id.Length==1) AppendFormat(SBHelp.START_LINK, id);
		    else {
			    Append("<link=\"");
			    Append(id[0]);
			    for(var i = 1; i < id.Length; i++) {
				    Append('|');
				    Append(id[i]);
			    }
			    Append("\">");
		    }
	    }

	    /// <summary>
	    /// This method is used to add a ending link tag for TextMeshPro.
	    /// </summary>
	    public void EndLink() => Append(SBHelp.END_LINK);

	    /// <summary>
	    /// This method is used to add a background color to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The background color you want to add to the builder's contents.</param>
	    /// <seealso cref="AddMarkToAll(string)"/>
	    public void AddMarkToAll(Color color) =>
		    AddTagToAll(ZString.Format(SBHelp.START_MARK, SBHelp.GetCachedColor(color)), SBHelp.END_MARK);

	    /// <summary>
	    /// This method is used to add a background color to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The background color you want to add to the builder's contents.</param>
	    /// <seealso cref="AddMarkToAll(UnityEngine.Color)"/>
	    public void AddMarkToAll(string color)  =>
		    AddTagToAll(ZString.Format(SBHelp.START_MARK, color), SBHelp.END_MARK);

	    /// <summary>
	    /// This method is used to add a starting mark tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartMark(string)"/>
	    public void StartMark(Color color) =>
		    AppendFormat(SBHelp.START_MARK, SBHelp.GetCachedColor(color));

	    /// <summary>
	    /// This method is used to add a starting mark color tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartMark(string)"/>
	    public void StartMark(string color) =>
		    AppendFormat(SBHelp.START_MARK, color);

	    /// <summary>
	    /// This method is used to add a ending mark tag for TextMeshPro.
	    /// </summary>
	    public void EndMark() => Append(SBHelp.END_MARK);

	    /// <summary>
	    /// This method is used to add bold formatting to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    public void AddBoldToAll() => AddTagToAll(SBHelp.START_BOLD, SBHelp.END_BOLD);
	    
	    /// <summary>
	    /// This method is used to add a starting bold tag for TextMeshPro.
	    /// </summary>
	    public void StartBold() => Append(SBHelp.START_BOLD);

	    /// <summary>
	    /// This method is used to add a ending bold tag for TextMeshPro.
	    /// </summary>
	    public void EndBold() => Append(SBHelp.END_BOLD);
	    
	    /// <summary>
	    /// This method is used to add italics formatting to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    public void AddItalicToAll() => AddTagToAll(SBHelp.START_ITALIC, SBHelp.END_ITALIC);
	    
	    /// <summary>
	    /// This method is used to add a starting italic tag for TextMeshPro.
	    /// </summary>
	    public void StartItalic() => Append(SBHelp.START_ITALIC);

	    /// <summary>
	    /// This method is used to add a ending italic tag for TextMeshPro.
	    /// </summary>
	    public void EndItalic() => Append(SBHelp.END_ITALIC);
	    
	    /// <summary>
	    /// This method is used to add underline formatting to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    public void AddUnderlineToAll() => AddTagToAll(SBHelp.START_UNDERLINE, SBHelp.END_UNDERLINE);
	    
	    /// <summary>
	    /// This method is used to add a starting underline tag for TextMeshPro.
	    /// </summary>
	    public void StartUnderline() => Append(SBHelp.START_UNDERLINE);

	    /// <summary>
	    /// This method is used to add a ending strikethrough tag for TextMeshPro.
	    /// </summary>
	    public void EndUnderline() => Append(SBHelp.END_UNDERLINE);
	    
	    /// <summary>
	    /// This method is used to add strikethrough formatting to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    public void AddStrikethroughToAll() => AddTagToAll(SBHelp.START_STRIKETHROUGH, SBHelp.END_STRIKETHROUGH);
	    
	    /// <summary>
	    /// This method is used to add a starting strikethrough tag for TextMeshPro.
	    /// </summary>
	    public void StartStrikethrough() => Append(SBHelp.START_STRIKETHROUGH);

	    /// <summary>
	    /// This method is used to add a ending strikethrough tag for TextMeshPro.
	    /// </summary>
	    public void EndStrikethrough() => Append(SBHelp.END_STRIKETHROUGH);
	    
	    /// <summary>
	    /// This method is used to add no parse formatting to all the string builder's current contents for TextMeshPro.
	    /// </summary>
	    public void AddNoParseToAll() => AddTagToAll(SBHelp.START_NO_PARSE, SBHelp.END_NO_PARSE);
	    
	    /// <summary>
	    /// This method is used to add a starting no parse tag for TextMeshPro.
	    /// </summary>
	    public void StartNoParse() => Append(SBHelp.START_NO_PARSE);

	    /// <summary>
	    /// This method is used to add a ending no parse tag for TextMeshPro.
	    /// </summary>
	    public void EndNoParse() => Append(SBHelp.END_NO_PARSE);

	    /// <summary>
	    /// This method will write to both the beginning and ending of the string builder.
	    /// </summary>
	    /// <param name="start">The text that you want to add to the start of the string builder.</param>
	    /// <param name="end">The text that you want to add to the end of the string builder.</param>
	    public void AddTagToAll(string start, string end) {
		    Insert(0,start);
		    Append(end);
	    }
	    
        /// <summary>
		/// This method is used to append text that is formatted as bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendB<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_B_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendI<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_I_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_U_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_S_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBI<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BI_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BU_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendIU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_IU_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendIS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_IS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_US_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBIU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BIU_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBIS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BIS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BUS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendIUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_IUS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBIUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BIUS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendL<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_L_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLB<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LB_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLI<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LI_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBI<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBI_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLIU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LIU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLIS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LIS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBIU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBIU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBIS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBIS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLIUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LIUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBIUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBIUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendC{T}(T,string,bool)"/>
		public void AppendC<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_C_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendC{T}(T,UnityEngine.Color,bool)"/>
		public void AppendC<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_C_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCB{T}(T,string,bool)"/>
		public void AppendCB<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CB_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCI{T}(T,string,bool)"/>
		public void AppendCI<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CI_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCU{T}(T,string,bool)"/>
		public void AppendCU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCS{T}(T,string,bool)"/>
		public void AppendCS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCB{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCB<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CB_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBI{T}(T,string,bool)"/>
		public void AppendCBI<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBI_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBU{T}(T,string,bool)"/>
		public void AppendCBU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCI{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCI<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CI_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBS{T}(T,string,bool)"/>
		public void AppendCBS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIU{T}(T,string,bool)"/>
		public void AppendCIU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIS{T}(T,string,bool)"/>
		public void AppendCIS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCUS{T}(T,string,bool)"/>
		public void AppendCUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBI{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBI<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBI_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIU{T}(T,string,bool)"/>
		public void AppendCBIU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCIU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIS{T}(T,string,bool)"/>
		public void AppendCBIS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBUS{T}(T,string,bool)"/>
		public void AppendCBUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCIS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIUS{T}(T,string,bool)"/>
		public void AppendCIUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBIU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBIS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIUS{T}(T,string,bool)"/>
		public void AppendCBIUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCIUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBIUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLC{T}(T,string,string,bool)"/>
		public void AppendLC<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LC_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCB{T}(T,string,string,bool)"/>
		public void AppendLCB<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCB_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLC{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLC<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LC_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCI{T}(T,string,string,bool)"/>
		public void AppendLCI<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCI_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCU{T}(T,string,string,bool)"/>
		public void AppendLCU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCS{T}(T,string,string,bool)"/>
		public void AppendLCS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCB{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCB<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCB_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBI{T}(T,string,string,bool)"/>
		public void AppendLCBI<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBI_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBU{T}(T,string,string,bool)"/>
		public void AppendLCBU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCI{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCI<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCI_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBS{T}(T,string,string,bool)"/>
		public void AppendLCBS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIU{T}(T,string,string,bool)"/>
		public void AppendLCIU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIS{T}(T,string,string,bool)"/>
		public void AppendLCIS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCUS{T}(T,string,string,bool)"/>
		public void AppendLCUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBI{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBI<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBI_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIU{T}(T,string,string,bool)"/>
		public void AppendLCBIU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCIU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIS{T}(T,string,string,bool)"/>
		public void AppendLCBIS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBUS{T}(T,string,string,bool)"/>
		public void AppendLCBUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCIS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIUS{T}(T,string,string,bool)"/>
		public void AppendLCIUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBIU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBIS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIUS{T}(T,string,string,bool)"/>
		public void AppendLCBIUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCIUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBIUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append a space the given amount of times.
		/// </summary>
		/// <param name="times">The number of times that you want to append the space.</param>
        public void Space(int times = 1) {
            Append(SBHelp.SPACE, times);
        }
        
		/// <summary>
		/// This method is used to append a tab character the given amount of times.
		/// </summary>
		/// <param name="times">The number of times that you want to append the tab character.</param>
        public void Tab(int times = 1) {
            Append(SBHelp.TAB, times);
        }
        
		/// <summary>
		/// This method is sued to append a newline character the given amount of times.
		/// </summary>
		/// <param name="times">The number of times that you want to append the newline character.</param>
        public void NewLine(int times = 1) {
            Append(SBHelp.NL, times);
        }

		/// <summary>
		/// This method is used to append an alignment tag for TextMeshPro.
		/// </summary>
		/// <param name="align">The type of alignment tag you want to add.</param>
        public void AppendAlign(Align align) {
	        string alignment;
	        switch(align) {
		        case Align.Center: alignment = SBHelp.ALIGN_CENTER; break;
		        case Align.Left: alignment = SBHelp.ALIGN_LEFT; break;
		        case Align.Right: alignment = SBHelp.ALIGN_RIGHT; break;
		        case Align.Reset: alignment = SBHelp.RESET_ALIGN; break;
		        default: alignment = SBHelp.ALIGN_LEFT; break;
	        }

	        Append(alignment);
        }
        
		/// <summary>
		/// This method is used to append a superscript for TextMeshPro.
		/// </summary>
		/// <param name="text">The text that you want to display as a superscript.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
        public void AppendSuperscript<T>(T text, bool endSpace = false) {
	        AppendFormat(SBHelp.SUPERSCRIPT_FORMAT, text);
	        if(endSpace)Append(SBHelp.SPACE);
        }
        
		/// <summary>
		/// This method is used to append a subscript for TextMeshPro.
		/// </summary>
		/// <param name="text">The text that you want to display as a subscript.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
        public void AppendSubscript<T>(T text, bool endSpace = false) {
	        AppendFormat(SBHelp.SUBSCRIPT_FORMAT, text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to append text that will ignore TextMeshPro's formatting.
		/// </summary>
		/// <param name="text">The raw text that you want to display.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
        public void AppendNoparse<T>(T text, bool endSpace = false) {
	        AppendFormat(SBHelp.NO_PARSE_FORMAT,text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to add marked color to the given text.
		/// </summary>
		/// <param name="text">The text that you want to add a marked color to.</param>
		/// <param name="color">The color you want to mark the text as.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendMark{T}(T,string,bool)"/>
        public void AppendMark<T>(T text, string color, bool endSpace = false) {
	        AppendFormat(SBHelp.MARK_FORMAT, color,text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to add marked color to the given text.
		/// </summary>
		/// <param name="text">The text that you want to add a marked color to.</param>
		/// <param name="color">The color you want to mark the text as.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendMark{T}(T,string,bool)"/>
        public void AppendMark<T>(T text, Color color, bool endSpace) {
	        AppendFormat(SBHelp.MARK_FORMAT, SBHelp.GetCachedColor(color),text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to add an indent to TextMeshPro.  This indent will be applied
		/// to every line and wrap-around until ended.
		/// </summary>
		/// <param name="size">The size of the indent.</param>
		/// <param name="sizeType">The size type.</param> 
		/// <param name="endPrevious">If true the previous indent will be ended first.</param>
		/// <seealso cref="EndIndent"/>
		public void StartIndent(float size, SizeType sizeType, bool endPrevious = false) {
			if(endPrevious) EndIndent();
			IndentFormatter.FormatTo(ref this,size,sizeType.SizeChar());
		}

		/// <summary>
		/// This method is used to end an indent for TextMeshPro.
		/// </summary>
		/// <seealso cref="StartIndent"/>
		public void EndIndent() => Append(SBHelp.INDENT_END);

		/// <summary>
		/// This method is used to add a line indent to TextMeshPro.  This indent will be
		/// applied to every new line until ended.
		/// </summary>
		/// <param name="size">The size of the line indent.</param>
		/// <param name="sizeType">The size type.</param>
		/// <seealso cref="EndLineIndent"/>
		public void StartLineIndent(float size, SizeType sizeType) {
			LineIndentFormatter.FormatTo(ref this,size,sizeType.SizeChar());
		}

		/// <summary>
		/// This method is sued to end a line indent for TextMeshPro.
		/// </summary>
		/// <seealso cref="StartLineIndent"/>
		public void EndLineIndent() => Append(SBHelp.END_LINE_INDENT);

		/// <summary>
		/// This method is used to append a starting horizontal position in TextMeshPro.
		/// </summary>
		/// <param name="size">The size or location of the position.</param>
		/// <param name="sizeType">The size type.</param>
		public void SetPosition(float size, SizeType sizeType) {
			PositionFormatter.FormatTo(ref this,size,sizeType.SizeChar());
		}

		/// <summary>
		/// This method is used to add or adjust a margin in TextMeshPro.
		/// </summary>
		/// <param name="marginType">The type of margin.</param>
		/// <param name="size">The size of the margin.</param>
		/// <param name="sizeType">The size type.</param>
		/// <seealso cref="AppendEndMargin"/>
		public void AppendMargin(MarginType marginType, float size, SizeType sizeType) {
			switch(marginType) {
				case MarginType.Left: 
					MarginLeftFormatter.FormatTo(ref this, size, sizeType.SizeChar());
					break;
				case MarginType.Right:
					MarginRightFormatter.FormatTo(ref this, size, sizeType.SizeChar());
					break;
				default:
					MarginFormatter.FormatTo(ref this, size, sizeType.SizeChar());
					break;
			}
		}

		/// <summary>
		/// This method is used to end all margins in TextMeshPro.
		/// </summary>
		/// <seealso cref="AppendMargin"/>
		public void AppendEndMargin() => Append(SBHelp.END_MARGIN);

		/// <summary>
		/// This method is used to append a TextMeshPro sprite using its index.
		/// </summary>
		/// <param name="index">The index of the sprite.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <seealso cref="AppendSprite(string,bool)"/>
		public void AppendSprite(int index, bool endSpace = false) {
			SpriteIndexFormatter.FormatTo(ref this,index);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append a TextMeshPro sprite using its name.
		/// </summary>
		/// <param name="name">The name of the sprite.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <seealso cref="AppendSprite(int,bool)"/>
		public void AppendSprite(string name, bool endSpace = false) {
			AppendFormat(SBHelp.SPRITE_NAME_FORMAT,name);
			if(endSpace)Append(SBHelp.SPACE);
		}
        
    }
    
    public partial struct Utf8ValueStringBuilder {
        
	    public static readonly Utf8PreparedFormat<float, char> IndentFormatter =
		    new Utf8PreparedFormat<float, char>(SBHelp.INDENT_FORMAT);
	    public static readonly Utf8PreparedFormat<float, char> LineIndentFormatter =
		    new Utf8PreparedFormat<float, char>(SBHelp.LINE_INDENT_FORMAT);
	    public static readonly Utf8PreparedFormat<float, char> PositionFormatter =
		    new Utf8PreparedFormat<float, char>(SBHelp.POSITION_FORMAT);
	    public static readonly Utf8PreparedFormat<float, char> MarginFormatter =
		    new Utf8PreparedFormat<float, char>(SBHelp.MARGIN_FORMAT);
	    public static readonly Utf8PreparedFormat<float, char> MarginLeftFormatter =
		    new Utf8PreparedFormat<float, char>(SBHelp.MARGIN_LEFT_FORMAT);
	    public static readonly Utf8PreparedFormat<float, char> MarginRightFormatter =
		    new Utf8PreparedFormat<float, char>(SBHelp.MARGIN_RIGHT_FORMAT);
	    public static readonly Utf8PreparedFormat<int> SpriteIndexFormatter =
		    new Utf8PreparedFormat<int>(SBHelp.SPRITE_INDEX_FORMAT);
	    
	    /// <summary>
	    /// This method is used to add a starting color tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartColor(string)"/>
	    public void StartColor(Color color) =>
		    AppendFormat(SBHelp.START_COLOR, SBHelp.GetCachedColor(color));

	    /// <summary>
	    /// This method is used to add a starting color tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartColor(string)"/>
	    public void StartColor(string color)  =>
		    AppendFormat(SBHelp.START_COLOR, color);

	    /// <summary>
	    /// This method is used to add a ending color tag for TextMeshPro.
	    /// </summary>
	    public void EndColor() => Append(SBHelp.END_COLOR);

	    /// <summary>
	    /// This method is used to add a starting link tag for TextMeshPro.
	    /// </summary>
	    /// <param name="linkId">The id you want the link tag to use.</param>
		public void StartLink(string linkId) => AppendFormat(SBHelp.START_LINK, linkId);

	    /// <summary>
	    /// This method is used to add a ending link tag for TextMeshPro.
	    /// </summary>
	    public void EndLink() => Append(SBHelp.END_LINK);

	    /// <summary>
	    /// This method is used to add a starting mark tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartMark(string)"/>
	    public void StartMark(Color color) =>
		    AppendFormat(SBHelp.START_MARK, SBHelp.GetCachedColor(color));

	    /// <summary>
	    /// This method is used to add a starting mark color tag for TextMeshPro.
	    /// </summary>
	    /// <param name="color">The color you want the tag to use.</param>
	    /// <seealso cref="StartMark(string)"/>
	    public void StartMark(string color) =>
		    AppendFormat(SBHelp.START_MARK, color);

	    /// <summary>
	    /// This method is used to add a ending mark tag for TextMeshPro.
	    /// </summary>
	    public void EndMark() => Append(SBHelp.END_MARK);
	    
	    /// <summary>
	    /// This method is used to add a starting bold tag for TextMeshPro.
	    /// </summary>
	    public void StartBold() => Append(SBHelp.START_BOLD);

	    /// <summary>
	    /// This method is used to add a ending bold tag for TextMeshPro.
	    /// </summary>
	    public void EndBold() => Append(SBHelp.END_BOLD);
	    
	    /// <summary>
	    /// This method is used to add a starting italic tag for TextMeshPro.
	    /// </summary>
	    public void StartItalic() => Append(SBHelp.START_ITALIC);

	    /// <summary>
	    /// This method is used to add a ending italic tag for TextMeshPro.
	    /// </summary>
	    public void EndItalic() => Append(SBHelp.END_ITALIC);
	    
	    /// <summary>
	    /// This method is used to add a starting underline tag for TextMeshPro.
	    /// </summary>
	    public void StartUnderline() => Append(SBHelp.START_UNDERLINE);

	    /// <summary>
	    /// This method is used to add a ending strikethrough tag for TextMeshPro.
	    /// </summary>
	    public void EndUnderline() => Append(SBHelp.END_UNDERLINE);
	    
	    /// <summary>
	    /// This method is used to add a starting strikethrough tag for TextMeshPro.
	    /// </summary>
	    public void StartStrikethrough() => Append(SBHelp.START_STRIKETHROUGH);

	    /// <summary>
	    /// This method is used to add a ending strikethrough tag for TextMeshPro.
	    /// </summary>
	    public void EndStrikethrough() => Append(SBHelp.END_STRIKETHROUGH);
	    
	    /// <summary>
	    /// This method is used to add a starting no parse tag for TextMeshPro.
	    /// </summary>
	    public void StartNoParse() => Append(SBHelp.START_NO_PARSE);

	    /// <summary>
	    /// This method is used to add a ending no parse tag for TextMeshPro.
	    /// </summary>
	    public void EndNoParse() => Append(SBHelp.END_NO_PARSE);

        /// <summary>
		/// This method is used to append text that is formatted as bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendB<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_B_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendI<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_I_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_U_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_S_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBI<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BI_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BU_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendIU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_IU_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendIS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_IS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_US_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBIU<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BIU_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBIS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BIS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BUS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendIUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_IUS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendBIUS<T>(T text, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_BIUS_FORMAT, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendL<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_L_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLB<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LB_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLI<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LI_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBI<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBI_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLIU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LIU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLIS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LIS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBIU<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBIU_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBIS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBIS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLIUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LIUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		public void AppendLBIUS<T>(T text, string linkId, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LBIUS_FORMAT, linkId, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendC{T}(T,string,bool)"/>
		public void AppendC<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_C_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendC{T}(T,UnityEngine.Color,bool)"/>
		public void AppendC<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_C_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCB{T}(T,string,bool)"/>
		public void AppendCB<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CB_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCI{T}(T,string,bool)"/>
		public void AppendCI<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CI_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCU{T}(T,string,bool)"/>
		public void AppendCU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCS{T}(T,string,bool)"/>
		public void AppendCS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCB{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCB<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CB_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBI{T}(T,string,bool)"/>
		public void AppendCBI<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBI_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBU{T}(T,string,bool)"/>
		public void AppendCBU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCI{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCI<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CI_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBS{T}(T,string,bool)"/>
		public void AppendCBS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIU{T}(T,string,bool)"/>
		public void AppendCIU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIS{T}(T,string,bool)"/>
		public void AppendCIS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCUS{T}(T,string,bool)"/>
		public void AppendCUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBI{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBI<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBI_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIU{T}(T,string,bool)"/>
		public void AppendCBIU<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIU_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCIU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIS{T}(T,string,bool)"/>
		public void AppendCBIS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBUS{T}(T,string,bool)"/>
		public void AppendCBUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCIS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIUS{T}(T,string,bool)"/>
		public void AppendCIUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIU{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBIU<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIU_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBIS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIUS{T}(T,string,bool)"/>
		public void AppendCBIUS<T>(T text, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIUS_FORMAT, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCIUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCIUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CIUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendCBIUS{T}(T,UnityEngine.Color,bool)"/>
		public void AppendCBIUS<T>(T text, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_CBIUS_FORMAT, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLC{T}(T,string,string,bool)"/>
		public void AppendLC<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LC_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCB{T}(T,string,string,bool)"/>
		public void AppendLCB<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCB_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, and colored for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLC{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLC<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LC_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCI{T}(T,string,string,bool)"/>
		public void AppendLCI<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCI_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCU{T}(T,string,string,bool)"/>
		public void AppendLCU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCS{T}(T,string,string,bool)"/>
		public void AppendLCS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and bolded for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCB{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCB<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCB_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBI{T}(T,string,string,bool)"/>
		public void AppendLCBI<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBI_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBU{T}(T,string,string,bool)"/>
		public void AppendLCBU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCI{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCI<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCI_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBS{T}(T,string,string,bool)"/>
		public void AppendLCBS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIU{T}(T,string,string,bool)"/>
		public void AppendLCIU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIS{T}(T,string,string,bool)"/>
		public void AppendLCIS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCUS{T}(T,string,string,bool)"/>
		public void AppendLCUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and italicised for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBI{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBI<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBI_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIU{T}(T,string,string,bool)"/>
		public void AppendLCBIU<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIU_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCIU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIS{T}(T,string,string,bool)"/>
		public void AppendLCBIS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBUS{T}(T,string,string,bool)"/>
		public void AppendLCBUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCIS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIUS{T}(T,string,string,bool)"/>
		public void AppendLCIUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and underlined for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIU{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBIU<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIU_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBIS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIUS{T}(T,string,string,bool)"/>
		public void AppendLCBIUS<T>(T text, string linkId, Color color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIUS_FORMAT, linkId, SBHelp.GetCachedColor(color), text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCIUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCIUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCIUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append text that is formatted as a link, colored, bolded, italicised, underlined, and struckthrough for TextMeshPro.
		/// </summary>
		/// <param name="text">The text or object that you want to format.</param>
		/// <param name="linkId">The links id that will be used for a callback.</param>
		/// <param name="color">The html color code of the color that you want the text to be.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendLCBIUS{T}(T,string,UnityEngine.Color,bool)"/>
		public void AppendLCBIUS<T>(T text, string linkId, string color, bool endSpace = false) {
			AppendFormat(SBHelp.APPEND_LCBIUS_FORMAT, linkId, color, text);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append a space the given amount of times.
		/// </summary>
		/// <param name="times">The number of times that you want to append the space.</param>
        public void Space(int times = 1) {
            Append(SBHelp.SPACE, times);
        }
        
		/// <summary>
		/// This method is used to append a tab character the given amount of times.
		/// </summary>
		/// <param name="times">The number of times that you want to append the tab character.</param>
        public void Tab(int times = 1) {
            Append(SBHelp.TAB, times);
        }
        
		/// <summary>
		/// This method is sued to append a newline character the given amount of times.
		/// </summary>
		/// <param name="times">The number of times that you want to append the newline character.</param>
        public void NewLine(int times = 1) {
            Append(SBHelp.NL, times);
        }

		/// <summary>
		/// This method is used to append an alignment tag for TextMeshPro.
		/// </summary>
		/// <param name="align">The type of alignment tag you want to add.</param>
        public void AppendAlign(Align align) {
	        string alignment;
	        switch(align) {
		        case Align.Center: alignment = SBHelp.ALIGN_CENTER; break;
		        case Align.Left: alignment = SBHelp.ALIGN_LEFT; break;
		        case Align.Right: alignment = SBHelp.ALIGN_RIGHT; break;
		        case Align.Reset: alignment = SBHelp.RESET_ALIGN; break;
		        default: alignment = SBHelp.ALIGN_LEFT; break;
	        }
	        Append(alignment);
        }
        
		/// <summary>
		/// This method is used to append a superscript for TextMeshPro.
		/// </summary>
		/// <param name="text">The text that you want to display as a superscript.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
        public void AppendSuperscript<T>(T text, bool endSpace = false) {
	        AppendFormat(SBHelp.SUPERSCRIPT_FORMAT, text);
	        if(endSpace)Append(SBHelp.SPACE);
        }
        
		/// <summary>
		/// This method is used to append a subscript for TextMeshPro.
		/// </summary>
		/// <param name="text">The text that you want to display as a subscript.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
        public void AppendSubscript<T>(T text, bool endSpace = false) {
	        AppendFormat(SBHelp.SUBSCRIPT_FORMAT, text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to append text that will ignore TextMeshPro's formatting.
		/// </summary>
		/// <param name="text">The raw text that you want to display.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
        public void AppendNoparse<T>(T text, bool endSpace = false) {
	        AppendFormat(SBHelp.NO_PARSE_FORMAT,text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to add marked color to the given text.
		/// </summary>
		/// <param name="text">The text that you want to add a marked color to.</param>
		/// <param name="color">The color you want to mark the text as.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendMark{T}(T,string,bool)"/>
        public void AppendMark<T>(T text, string color, bool endSpace = false) {
	        AppendFormat(SBHelp.MARK_FORMAT, color,text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to add marked color to the given text.
		/// </summary>
		/// <param name="text">The text that you want to add a marked color to.</param>
		/// <param name="color">The color you want to mark the text as.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <typeparam name="T">The type of the object that you want to display as text.</typeparam>
		/// <seealso cref="AppendMark{T}(T,string,bool)"/>
        public void AppendMark<T>(T text, Color color, bool endSpace) {
	        AppendFormat(SBHelp.MARK_FORMAT, SBHelp.GetCachedColor(color),text);
	        if(endSpace)Append(SBHelp.SPACE);
        }

		/// <summary>
		/// This method is used to add an indent to TextMeshPro.  This indent will be applied
		/// to every line and wrap-around until ended.
		/// </summary>
		/// <param name="size">The size of the indent.</param>
		/// <param name="sizeType">The size type.</param>
		/// <param name="endPrevious">If true the previous indent will be ended first.</param>
		/// <seealso cref="EndIndent"/>
		public void StartIndent(float size, SizeType sizeType, bool endPrevious = false) {
			if(endPrevious) EndIndent();
			IndentFormatter.FormatTo(ref this,size,sizeType.SizeChar());
		}

		/// <summary>
		/// This method is used to end an indent for TextMeshPro.
		/// </summary>
		/// <seealso cref="StartIndent"/>
		public void EndIndent() => Append(SBHelp.INDENT_END);

		/// <summary>
		/// This method is used to add a line indent to TextMeshPro.  This indent will be
		/// applied to every new line until ended.
		/// </summary>
		/// <param name="size">The size of the line indent.</param>
		/// <param name="sizeType">The size type.</param>
		/// <seealso cref="EndLineIndent"/>
		public void StartLineIndent(float size, SizeType sizeType) {
			LineIndentFormatter.FormatTo(ref this,size,sizeType.SizeChar());
		}

		/// <summary>
		/// This method is sued to end a line indent for TextMeshPro.
		/// </summary>
		/// <seealso cref="StartLineIndent"/>
		public void EndLineIndent() => Append(SBHelp.END_LINE_INDENT);

		/// <summary>
		/// This method is used to append a starting horizontal position in TextMeshPro.
		/// </summary>
		/// <param name="size">The size or location of the position.</param>
		/// <param name="sizeType">The size type.</param>
		public void SetPosition(float size, SizeType sizeType) {
			PositionFormatter.FormatTo(ref this,size,sizeType.SizeChar());
		}

		/// <summary>
		/// This method is used to add or adjust a margin in TextMeshPro.
		/// </summary>
		/// <param name="marginType">The type of margin.</param>
		/// <param name="size">The size of the margin.</param>
		/// <param name="sizeType">The size type.</param>
		/// <seealso cref="AppendEndMargin"/>
		public void AppendMargin(MarginType marginType, float size, SizeType sizeType) {
			switch(marginType) {
				case MarginType.Left: 
					MarginLeftFormatter.FormatTo(ref this, size, sizeType.SizeChar());
					break;
				case MarginType.Right:
					MarginRightFormatter.FormatTo(ref this, size, sizeType.SizeChar());
					break;
				default:
					MarginFormatter.FormatTo(ref this, size, sizeType.SizeChar());
					break;
			}
		}

		/// <summary>
		/// This method is used to end all margins in TextMeshPro.
		/// </summary>
		/// <seealso cref="AppendMargin"/>
		public void AppendEndMargin() => Append(SBHelp.END_MARGIN);

		/// <summary>
		/// This method is used to append a TextMeshPro sprite using its index.
		/// </summary>
		/// <param name="index">The index of the sprite.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <seealso cref="AppendSprite(string,bool)"/>
		public void AppendSprite(int index, bool endSpace = false) {
			SpriteIndexFormatter.FormatTo(ref this,index);
			if(endSpace)Append(SBHelp.SPACE);
		}

		/// <summary>
		/// This method is used to append a TextMeshPro sprite using its name.
		/// </summary>
		/// <param name="name">The name of the sprite.</param>
		/// <param name="endSpace">If true a space will be appended after the text.</param>
		/// <seealso cref="AppendSprite(int,bool)"/>
		public void AppendSprite(string name, bool endSpace = false) {
			AppendFormat(SBHelp.SPRITE_NAME_FORMAT,name);
			if(endSpace)Append(SBHelp.SPACE);
		}
        
    }
    
}