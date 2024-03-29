﻿
using UnityEngine;
using FishNet.Managing;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using Amilious.Core.Identity.Group.Data;

namespace Amilious.Core.FishNet.Groups {
    
    [RequireComponent(typeof(NetworkManager))]
    [AmiHelpBox(HELP_BOX_TEXT,HelpBoxType.Info)]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet Group Data Manager")]
    public class FishNetGroupDataManager : DefaultGroupDataManager {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private NetworkManager _networkManager;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the NetworkManager.
        /// </summary>
        public NetworkManager NetworkManager => this.GetCacheComponent(ref _networkManager);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is used to register the instance.
        /// </summary>
        private void Awake() {
            Debug.Log("Group manager registered.");
            NetworkManager.TryRegisterInstance<FishNetGroupDataManager>(this);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}