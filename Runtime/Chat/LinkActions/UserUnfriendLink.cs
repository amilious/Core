﻿using Amilious.Core.Identity.User;

namespace Amilious.Core.Chat.LinkActions {
    public class UserUnfriendLink : AbstractUserLink {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Remove Friend";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatDisplay chatDisplay, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not unfriend yourself.
            if(!user.IsFriend) return false; //the user is already a friend.
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatDisplay chatDisplay, UserIdentity user) {
            UserManager.Client_RemoveFriend(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}