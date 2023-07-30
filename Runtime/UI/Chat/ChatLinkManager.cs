
using System.Linq;
using UnityEngine;
using Amilious.Core.UI.Text;
using Amilious.Core.UI.Cursors;
using UnityEngine.EventSystems;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Attributes;
using Amilious.Core.UI.Chat.LinkActions;

namespace Amilious.Core.UI.Chat {
    
    [AmiHelpBox(MSG_BOX,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/UI/Chat/Chat Link Manager")]
    [HelpURL("https://amilious.gitbook.io/core/runtime/ui/chatbox/chat-link-manager")]
    [DisallowMultipleComponent, RequireComponent(typeof(ChatBox))]
    public class ChatLinkManager : AmiliousBehavior {

        private const string MSG_BOX = "This component is used to handle chat links and menus.";
        
        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [SerializeField] private LinkActionMenu linkActionMenu;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private ChatBox _chatBox;
        private readonly Dictionary<string, List<LinkAction>> _linkActions = new Dictionary<string, List<LinkAction>>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public ChatBox ChatBox => this.GetCacheComponent(ref _chatBox);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Monobehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            //load link actions
            foreach(var component in GetComponents<LinkAction>()) AddLinkAction(component);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a link action.
        /// </summary>
        /// <param name="action">The action that you want to add.</param>
        public void AddLinkAction(LinkAction action) {
            if(!_linkActions.ContainsKey(action.LinkType)) _linkActions[action.LinkType] = new List<LinkAction>();
            _linkActions[action.LinkType].Add(action);
        }

        public void Register(TMProLinkHandler linkListener) {
            linkListener.OnLinkClickLocal += OnLinkClicked;
            linkListener.OnLinkEnterLocal += OnLinkEnter;
            linkListener.OnLinkExitLocal += OnLinkExit;
        }

        public void Unregister(TMProLinkHandler linkListener) {
            linkListener.OnLinkClickLocal -= OnLinkClicked;
            linkListener.OnLinkEnterLocal -= OnLinkEnter;
            linkListener.OnLinkExitLocal -= OnLinkExit;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        private void OnLinkExit(string text, string[] id) { CursorController.ReturnToPrevious(); }

        private void OnLinkEnter(string text, string[] id) {CursorController.SetCursor(DefaultCursors.Finger);}

        private void OnLinkClicked(PointerEventData clickEvent, string text, string[] args) {
            if(linkActionMenu == null) {
                Debug.LogWarning("Unable to show link menu because it is missing!");
                return;
            }
            if(args.Length < 1) return;
            if(!_linkActions.TryGetValueFix(args[0], out var actions)) return;
            if(actions.Count == 0) return;
            var clickAction = actions.FirstOrDefault(a => !a.IsMenuItem);
            if(clickAction != null) {
                clickAction.Action(ChatBox, text, args);
                return;
            }
            //linkActionMenu.Show(clickEvent.pressPosition);
            var hasMenu = false;
            foreach(var action in actions) {
                if(!action.ShowMenuItem(ChatBox, text, args)) continue;
                linkActionMenu.AddAction(action.ActionName,()=>action.Action(ChatBox, text, args));
                hasMenu = true;
            }
            if(hasMenu) linkActionMenu.Show(clickEvent.pressPosition);
            //if(!hasMenu) linkActionMenu.Hide();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}