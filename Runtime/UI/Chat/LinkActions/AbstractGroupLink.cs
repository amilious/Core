using Amilious.Core.Identity.Group;

namespace Amilious.Core.UI.Chat.LinkActions {
    
    /// <summary>
    /// This class can be extended to add actions to the group menu.
    /// </summary>
    public abstract class AbstractGroupLink : AbstractNetworkLink {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsMenuItem => true;
        
        /// <inheritdoc />
        public override string LinkType => "group";

        /// <summary>
        /// This property is used to get the group manager.
        /// </summary>
        public IGroupIdentityManager GroupManager => NetworkManagers.GroupManager;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public sealed override bool ShowMenuItem(IChatBox chatBox, string text, string[] linkArgs) {
            return TryGetGroup(linkArgs, out var group) && ShowMenuItem(chatBox, group);
        }

        /// <inheritdoc />
        public sealed override void Action(IChatBox chatBox, string text, string[] linkArgs) {
            if(TryGetGroup(linkArgs, out var group)) Action(chatBox,group);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Abstract Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called to check if the option should be shown for the menu.
        /// </summary>
        /// <param name="chatBox">The chat display.</param>
        /// <param name="group">The group that the menu is for.</param>
        /// <returns>True if the item should be shown on the group's menu, otherwise false.</returns>
        protected abstract bool ShowMenuItem(IChatBox chatBox, GroupIdentity group);

        /// <summary>
        /// This method will be called if the menu item is clicked.
        /// </summary>
        /// <param name="chatBox">The chat display.</param>
        /// <param name="group">The group that the menu is for.</param>
        protected abstract void Action(IChatBox chatBox, GroupIdentity group);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the group from the link args.
        /// </summary>
        /// <param name="linkArgs">The link args.</param>
        /// <param name="group">The group.</param>
        /// <returns>True if able to get the group, otherwise false.</returns>
        private bool TryGetGroup(string[] linkArgs, out GroupIdentity group) {
            group = GroupIdentity.Default;
            if(linkArgs.Length < 2) return false;
            if(!IsForLinkType(linkArgs[0])) return false;
            if(NetworkManagers.GroupManager == null) return false;
            if(!NetworkManagers.IsServer && !NetworkManagers.IsClient) return false;
            if(!uint.TryParse(linkArgs[1], out var groupId)) return false;
            if(!NetworkManagers.GroupManager.TryGetGroup(groupId,out group)) return false;
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}