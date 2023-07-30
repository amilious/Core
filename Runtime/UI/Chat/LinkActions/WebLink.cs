using Amilious.Core.Attributes;
using UnityEngine;

namespace Amilious.Core.UI.Chat.LinkActions {
    
    [AmiHelpBox(MSG_BOX,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/UI/Links/Web Link")]
    public class WebLink : LinkAction {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        private const string MSG_BOX = "This component is responsible for handling website links.";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public override bool IsMenuItem => false;
        public override string LinkType => "web";
        public override string ActionName => null;
        public override bool ShowMenuItem(IChatBox chatBox, string text, string[] linkArgs) => false;

        public override void Action(IChatBox chatBox, string text, string[] linkArgs) {
            Application.OpenURL(text);
        }
    }
}