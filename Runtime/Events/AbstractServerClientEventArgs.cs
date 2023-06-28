namespace Amilious.Core.Events {
    
    public class AbstractServerClientEventArgs : AbstractCancelEventArgs {
    
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the event is executing for the server.
        /// </summary>
        public bool ExecutingForServer { get; }

        /// <summary>
        /// This property is true if the event is executing for the server.
        /// </summary>
        public bool ExecutingForClient { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructor ////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new <see cref="AbstractServerClientEventArgs"/>.
        /// </summary>
        /// <param name="server">True if executing for the server, otherwise false for the client.</param>
        protected AbstractServerClientEventArgs(bool server) {
            ExecutingForServer = server;
            ExecutingForClient = !server;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}