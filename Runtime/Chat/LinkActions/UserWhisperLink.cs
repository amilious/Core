using Amilious.Core.Identity.User;

namespace Amilious.Core.Chat.LinkActions {
    public class UserWhisperLink : AbstractUserLink {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Whisper";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatDisplay chatDisplay, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not whisper to yourself.
            if(user.IsBlocked) return false; //the user is blocked
            if(!user.IsOnline) return false; //the user is not online
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatDisplay chatDisplay, UserIdentity user) {
            chatDisplay.SetChatChannel(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}