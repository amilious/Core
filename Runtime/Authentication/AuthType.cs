namespace Amilious.Core.Authentication {
    
    /// <summary>
    /// This enum is used to represent an authentication type.
    /// </summary>
    public enum AuthType : byte {
        
        /// <summary>
        /// This type is used when no authentication is needed. 
        /// </summary>
        None = 0,
        
        /// <summary>
        /// This type is used when a password is required.
        /// </summary>
        Password = 1,
        
        /// <summary>
        /// This type is used when approval is required.
        /// </summary>
        Approval = 2,
        
    }
}