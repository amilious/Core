namespace Amilious.Core.Authentication {
    
    public abstract class PasswordRequestProvider : AmiliousBehavior {

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used to request a new password.
        /// </summary>
        public delegate void PasswordCreatedCallback(string newPassword);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        public abstract void RequestNewPassword(PasswordCreatedCallback generateNewPassword);

    }
    
}