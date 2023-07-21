using System;

namespace Amilious.Core.UI.Chat.LinkActions {
    
    public abstract class LinkAction : AmiliousBehavior {

        public abstract bool IsMenuItem { get; }
        
        public abstract string LinkType { get; }

        public abstract string ActionName { get; }
        
        public bool IsForLinkType(string linkType) => LinkType.Equals(linkType, StringComparison.CurrentCultureIgnoreCase);

        public abstract bool ShowMenuItem(IChatBox chatBox, string text, string[] linkArgs);

        public abstract void Action(IChatBox chatBox, string text, string[] linkArgs);


    }
}