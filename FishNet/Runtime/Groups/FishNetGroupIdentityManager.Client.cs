using System;
using Amilious.Core.Identity.Group;

namespace Amilious.Core.FishNet.Groups {
    
    public partial class FishNetGroupIdentityManager {
        
        #region Client Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool RequestCreate(string name, GroupType type, GroupAuthType authType = GroupAuthType.None, 
            string password = null) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestOwnerChange(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestApply(int group, string request = null, string password = null) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestInvite(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestApproveApplication(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestRankChange(int group, int user, short rank) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestLeave(int group) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestKick(int group, int user) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RequestDisband(int group) {
            throw new NotImplementedException();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}