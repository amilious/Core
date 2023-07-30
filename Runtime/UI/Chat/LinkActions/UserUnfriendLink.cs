using Amilious.Core.Attributes;
using Amilious.Core.Identity.User;
using UnityEngine;

namespace Amilious.Core.UI.Chat.LinkActions {
    
    [AmiHelpBox(MSG_BOX,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/UI/Links/User Unfriend Link")]
    public class UserUnfriendLink : AbstractUserLink {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        private const string MSG_BOX = "This component is responsible for adding and handling user unfriend links.";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Remove Friend";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatBox chatBox, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not unfriend yourself.
            if(!user.IsFriend) return false; //the user is already a friend.
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatBox chatBox, UserIdentity user) {
            UserManager.Client_RemoveFriend(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}