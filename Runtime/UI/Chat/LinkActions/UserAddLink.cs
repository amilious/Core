using Amilious.Core.Attributes;
using Amilious.Core.Identity.User;
using UnityEngine;

namespace Amilious.Core.UI.Chat.LinkActions {
    
    [AmiHelpBox(MSG_BOX,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/UI/Links/User Add Friend Link")]
    public class UserAddLink : AbstractUserLink {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        private const string MSG_BOX = "This component is responsible for adding and handling user add friend links.";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override string ActionName => "Add Friend";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override bool ShowUserMenuItem(IChatBox chatBox, UserIdentity user) {
            if(user.IsLocalUser) return false; //you can not add yourself.
            if(user.IsFriend) return false; //the user is already a friend.
            if(user.IsBlocked) return false; //the user must be unblocked first.
            if(user.IsRequestingFriendship) return false; //the user is requesting friendship so show approve instead.
            if(user.IsPendingFriendship) return false; //a friendship request has already been sent.
            return true;
        }

        /// <inheritdoc />
        protected override void UserAction(IChatBox chatBox, UserIdentity user) {
            UserManager.Client_AddFriend(user);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}