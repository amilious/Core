
using FishNet;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing;
using System.Collections.Generic;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.FishNet.Groups {
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(short.MinValue+99)]
    [AddComponentMenu("Amilious/Networking/FishNet/FishNet Group Identity Manager")]
    public class FishNetGroupIdentityManager : NetworkBehaviour, IGroupIdentityManager {

        private FishNetGroupDataManager _groupDataManager;
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This can be used to get the instance of the first <see cref="NetworkManager"/>, otherwise you can get the
        /// instance from the <see cref="NetworkManager"/>.
        /// </summary>
        public static FishNetGroupIdentityManager Instance => InstanceFinder.GetInstance<FishNetGroupIdentityManager>();
        
        public AbstractGroupDataManager GroupIdDataManager {
            get {
                if(_groupDataManager != null) return _groupDataManager;
                _groupDataManager = NetworkManager.GetGroupDataManager();
                return _groupDataManager;
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client & Server Properties /////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public GroupIdentity this[int userId] {
            get {
                TryGetGroupIdentity(userId, out var groupIdentity);
                return groupIdentity;
            }
        }

        /// <inheritdoc />
        public IEnumerable<GroupIdentity> Identities { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            /*if(!NetworkManager.TryRegisterInstance(this)) {
                AmiliousCore.RemoveDuplicateMessage(this);
                Destroy(this);
                return;
            }*/
            GroupIdentity.SetIdentityManager(this);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        public override void OnStartNetwork() {
            base.OnStartNetwork();
            NetworkManager.TryRegisterInstance(this);
        }

        /// <inheritdoc />
        public virtual bool IsMember(int groupId, int identityId) {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryGetMemberIds(int groupId, out int[] members) {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryGetGroupIdentity(int groupId, out GroupIdentity groupIdentity) {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool CreateGroup(string name, int identityId, GroupType groupType, out int groupId, 
            string password = null) {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryLeaveGroup(int groupId, int identityId) {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TryJoinGroup(int groupId, int identityId, string password = null) {
            throw new System.NotImplementedException();
        }
        
    }
}