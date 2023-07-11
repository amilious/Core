using Amilious.Core.Identity.User;

namespace Amilious.Core.Chat.LinkActions {
    public class UserUnblockLink : AbstractUserLink {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Unblock User";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatDisplay chatDisplay, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not unblock yourself.
            if(!user.IsBlocked) return false; //the user must be unblocked first.
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatDisplay chatDisplay, UserIdentity user) {
            UserManager.Client_UnblockUser(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}