using UnityEngine;

namespace Amilious.Core.Chat.LinkActions {
    public class WebLink : LinkAction {
        public override bool IsMenuItem => false;
        public override string LinkType => "web";
        public override string ActionName => null;
        public override bool ShowMenuItem(IChatDisplay chatDisplay, string text, string[] linkArgs) => false;

        public override void Action(IChatDisplay chatDisplay, string text, string[] linkArgs) {
            Application.OpenURL(text);
        }
    }
}