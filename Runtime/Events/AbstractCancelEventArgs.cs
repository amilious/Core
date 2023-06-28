using System;

namespace Amilious.Core.Events {
    
    /// <summary>
    /// This is a cancelable event args
    /// </summary>
    public class AbstractCancelEventArgs : EventArgs {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the operation was canceled.
        /// </summary>
        public bool Canceled { get; private set; }
        
        /// <summary>
        /// This property contains the cancel reason if the operation was canceled.
        /// </summary>
        public string CancelReason { get; private set; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to cancel the operation.
        /// </summary>
        /// <param name="reason">The reason that you are canceling.</param>
        public void Cancel(string reason) {
            if(Canceled) return;
            Canceled = true;
            CancelReason = reason;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}