using Amilious.Core.Attributes;
using Amilious.Core.Identity.User;
using UnityEngine;

namespace Amilious.Core.UI.Chat.LinkActions {
    
    [AmiHelpBox(MSG_BOX,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/UI/Links/User Unblock Link")]
    public class UserUnblockLink : AbstractUserLink {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        private const string MSG_BOX = "This component is responsible for adding and handling user unblock links.";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Unblock User";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatBox chatBox, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not unblock yourself.
            if(!user.IsBlocked) return false; //the user must be unblocked first.
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatBox chatBox, UserIdentity user) {
            UserManager.Client_UnblockUser(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}