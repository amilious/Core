using UnityEngine;

namespace Amilious.Core.UI.Chat.LinkActions {
    public class WebLink : LinkAction {
        public override bool IsMenuItem => false;
        public override string LinkType => "web";
        public override string ActionName => null;
        public override bool ShowMenuItem(IChatBox chatBox, string text, string[] linkArgs) => false;

        public override void Action(IChatBox chatBox, string text, string[] linkArgs) {
            Application.OpenURL(text);
        }
    }
}