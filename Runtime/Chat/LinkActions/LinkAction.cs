using System;

namespace Amilious.Core.Chat.LinkActions {
    
    public abstract class LinkAction : AmiliousBehavior {

        public abstract bool IsMenuItem { get; }
        
        public abstract string LinkType { get; }

        public abstract string ActionName { get; }
        
        public bool IsForLinkType(string linkType) => LinkType.Equals(linkType, StringComparison.CurrentCultureIgnoreCase);

        public abstract bool ShowMenuItem(IChatDisplay chatDisplay, string text, string[] linkArgs);

        public abstract void Action(IChatDisplay chatDisplay, string text, string[] linkArgs);


    }
}