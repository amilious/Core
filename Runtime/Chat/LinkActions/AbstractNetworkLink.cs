using Amilious.Core.Extensions;
using Amilious.Core.Networking;
using UnityEngine;

namespace Amilious.Core.Chat.LinkActions {
    
    [RequireComponent(typeof(AbstractNetworkManagers))]
    public abstract class AbstractNetworkLink : LinkAction {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private AbstractNetworkManagers _networkManagers;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains a links to the network managers.
        /// </summary>
        public AbstractNetworkManagers NetworkManagers => this.GetCacheComponent(ref _networkManagers);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
       
    }
}