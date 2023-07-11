using System;
using Amilious.Core;
using Amilious.Core.Chat;

namespace Amilious.Console.Display.LinkActions {
    
    public abstract class LinkAction : AmiliousBehavior {

        public abstract bool IsMenuItem { get; }
        
        public abstract string LinkType { get; }

        public abstract string ActionName { get; }
        
        public bool IsForLinkType(string linkType) => LinkType.Equals(linkType, StringComparison.CurrentCultureIgnoreCase);

        public abstract bool ShowMenuItem(IChatDisplay chatDisplay, string[] linkArgs);

        public abstract void Action(IChatDisplay chatDisplay, string[] linkArgs);


    }
}