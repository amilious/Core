using FishNet.Connection;
using FishNet.Object;

namespace Amilious.Core.FishNet.Users {
    
    public partial class FishNetUserIdentityManager {
        
        #region Client Rpcs ////////////////////////////////////////////////////////////////////////////////////////////
        
        [TargetRpc]
        private void Client_ReceiveFriends(NetworkConnection con, int[] friendIds) {
            _friends.Clear();
            foreach(var id in friendIds)_friends.Add(id);
            Client_OnFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceivePendingFriends(NetworkConnection con, int[] pendingFriends) {
            _pendingFriends.Clear();
            foreach(var id in pendingFriends) _pendingFriends.Add(id);
            Client_OnPendingFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceiveRequestingFriends(NetworkConnection con, int[] requestingFriends) {
            _requestingFriends.Clear();
            foreach(var id in requestingFriends) _requestingFriends.Add(id);
            Client_OnRequestingFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceiveBlockedUsers(NetworkConnection con, int[] blockedIds) {
            _blocked.Clear();
            foreach(var id in blockedIds)_blocked.Add(id);
            Client_OnBlockedListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceiveBlockUpdate(NetworkConnection con, int userId, bool isBlocked) {
            if(isBlocked && !_blocked.Contains(userId)) _blocked.Add(userId);
            if(!isBlocked && _blocked.Contains(userId)) _blocked.Remove(userId);
            Client_OnBlockedListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceiveFriendUpdate(NetworkConnection con, int userId, bool isFriend) {
            if(isFriend && !_friends.Contains(userId)) _friends.Add(userId);
            if(!isFriend && _friends.Contains(userId)) _friends.Remove(userId);
            Client_OnFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceivePendingUpdate(NetworkConnection con, int userId, bool isPending) {
            if(isPending && !_pendingFriends.Contains(userId)) _pendingFriends.Add(userId);
            if(!isPending && _pendingFriends.Contains(userId)) _pendingFriends.Remove(userId);
            Client_OnPendingFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        private void Client_ReceiveRequestingUpdate(NetworkConnection con, int userId, bool isRequesting) {
            if(isRequesting && !_requestingFriends.Contains(userId)) _requestingFriends.Add(userId);
            if(!isRequesting && _requestingFriends.Contains(userId)) _requestingFriends.Remove(userId);
            Client_OnRequestingFriendListUpdated?.Invoke();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}