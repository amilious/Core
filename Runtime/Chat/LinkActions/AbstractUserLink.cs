using Amilious.Core.Identity.User;

namespace Amilious.Core.Chat.LinkActions {
    
    /// <summary>
    /// This class can be extended to add actions to the user menu.
    /// </summary>
    public abstract class AbstractUserLink : AbstractNetworkLink {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsMenuItem => true;
        
        /// <inheritdoc />
        public override string LinkType => "user";

        /// <summary>
        /// This property is used to get access to the user manager.
        /// </summary>
        public IUserIdentityManager UserManager => NetworkManagers.UserManager;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public sealed override bool ShowMenuItem(IChatDisplay chatDisplay, string text, string[] linkArgs) {
            return TryGetUser(linkArgs, out var user) && ShowUserMenuItem(chatDisplay, user);
        }

        /// <inheritdoc />
        public sealed override void Action(IChatDisplay chatDisplay, string text, string[] linkArgs) {
            if(TryGetUser(linkArgs, out var user)) UserAction(chatDisplay,user);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Abstract Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called to check if the option should be shown for the menu.
        /// </summary>
        /// <param name="chatDisplay">The chat display.</param>
        /// <param name="user">The user that the menu is for.</param>
        /// <returns>True if the item should be shown on the user's menu, otherwise false.</returns>
        protected abstract bool ShowUserMenuItem(IChatDisplay chatDisplay, UserIdentity user);

        /// <summary>
        /// This method will be called if the menu item is clicked.
        /// </summary>
        /// <param name="chatDisplay">The chat display.</param>
        /// <param name="user">The user that the menu is for.</param>
        protected abstract void UserAction(IChatDisplay chatDisplay, UserIdentity user);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
                                 
        /// <summary>
        /// This method is used to get the user from the link args.
        /// </summary>
        /// <param name="linkArgs">The link args.</param>
        /// <param name="user">The user.</param>
        /// <returns>True if able to get the user, otherwise false.</returns>
        private bool TryGetUser(string[] linkArgs, out UserIdentity user) {
            user = UserIdentity.DefaultUser;
            if(linkArgs.Length < 2) return false;
            if(!IsForLinkType(linkArgs[0])) return false;
            if(NetworkManagers.UserManager == null) return false;
            if(!NetworkManagers.IsServer && !NetworkManagers.IsClient) return false;
            if(!uint.TryParse(linkArgs[1], out var userId)) return false;
            if(!NetworkManagers.UserManager.TryGetIdentity(userId,out user)) return false;
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}