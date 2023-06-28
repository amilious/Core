using System;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.User;

namespace Amilious.Core.FishNet.Groups {
    
    public partial class FishNetGroupIdentityManager {
        
        #region Server Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        public bool Create(string name, GroupType type, UserIdentity owner, out GroupIdentity group, 
            GroupAuthType authType = GroupAuthType.None, string password = null) {
            throw new NotImplementedException();
        }

        public bool ChangeOwner(GroupIdentity group, UserIdentity owner) {
            throw new NotImplementedException();
        }

        public bool AddMember(GroupIdentity group, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Apply(GroupIdentity group, UserIdentity user, string request = null, string password = null) {
            throw new NotImplementedException();
        }

        public bool ApproveApplication(GroupIdentity group, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Invite(GroupIdentity group, UserIdentity inviter, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool RankChange(GroupIdentity group, UserIdentity user, short rank) {
            throw new NotImplementedException();
        }

        public bool Remove(GroupIdentity group, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Kick(GroupIdentity group, UserIdentity kicker, UserIdentity user) {
            throw new NotImplementedException();
        }

        public bool Disband(GroupIdentity group) {
            throw new NotImplementedException();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}