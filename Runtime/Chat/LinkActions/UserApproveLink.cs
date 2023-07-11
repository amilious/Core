﻿using Amilious.Core.Identity.User;

namespace Amilious.Core.Chat.LinkActions {
    public class UserApproveLink : AbstractUserLink {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Approve Friend";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatDisplay chatDisplay, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not approve yourself.
            if(user.IsFriend) return false; //the user is already a friend.
            if(user.IsBlocked) return false; //the user must be unblocked first.
            if(user.IsPendingFriendship) return false; //a friendship request has already been sent.
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatDisplay chatDisplay, UserIdentity user) {
            UserManager.Client_AddFriend(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}