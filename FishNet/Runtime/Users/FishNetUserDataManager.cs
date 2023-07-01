
using UnityEngine;
using FishNet.Managing;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using Amilious.Core.Identity.User;
using FishNet;

namespace Amilious.Core.FishNet.Users {
    
    [AmiHelpBox( HELP_BOX_TEXT,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet User Data Manager")]
    [RequireComponent(typeof(NetworkManager))]
    public class FishNetUserDataManager : AbstractUserDataManager {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private NetworkManager _networkManager;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This can be used to get the instance of the first <see cref="NetworkManager"/>, otherwise you can get the
        /// instance from the <see cref="NetworkManager"/>.
        /// </summary>
        public static FishNetUserDataManager Instance => InstanceFinder.GetInstance<FishNetUserDataManager>();
        
        /// <summary>
        /// This property contains the NetworkManager.
        /// </summary>
        public NetworkManager NetworkManager => this.GetCacheComponent(ref _networkManager);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is used to register the instance.
        /// </summary>
        private void Awake() => NetworkManager.TryRegisterInstance<FishNetUserDataManager>(this);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}