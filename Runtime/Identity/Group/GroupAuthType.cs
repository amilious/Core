using System;

namespace Amilious.Core.Identity.Group {
    
    /// <summary>
    /// This enum represents group authentication types.
    /// </summary>
    [Serializable]
    public enum GroupAuthType : byte {
        
        /// <summary>
        /// There is no authentication
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Password authentication.
        /// </summary>
        Password = 1,
        
        /// <summary>
        /// Only invited member's can join the group.
        /// </summary>
        InviteOnly = 2,
        
        /// <summary>
        /// Users can apply or be invited to the group.
        /// </summary>
        ApprovalOrInvite = 3
        
    }
    
}