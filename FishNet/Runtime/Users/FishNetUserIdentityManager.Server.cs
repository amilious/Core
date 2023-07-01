using FishNet.Connection;
using FishNet.Object;

namespace Amilious.Core.FishNet.Users {
    public partial class FishNetUserIdentityManager {
        
        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveUniqueDataRequest(NetworkConnection con = null) {
            if(con == null) con = LocalConnection;
            if(!con.TryGetUserId(out var userId)) return;
            if(FriendsNeedAcceptance) {
                var approved = UserIdDataManager.Server_GetApprovedFriends(userId).ToArray();
                var notApproved = UserIdDataManager.Server_GetNotApprovedFriends(userId).ToArray();
                var requesting = UserIdDataManager.Server_GetRequestingFriends(userId).ToArray();
                Client_ReceiveFriends(con,approved);
                Client_ReceivePendingFriends(con,notApproved);
                Client_ReceiveRequestingFriends(con,requesting);
            }else {
                var friends = UserIdDataManager.Server_GetFriends(userId).ToArray();
                //send friends
                Client_ReceiveFriends(con,friends);
            }
            //send blocked
            Client_ReceiveBlockedUsers(con,UserIdDataManager.Server_GetBlockedUsers(userId).ToArray());
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveFriendUpdate(uint userId, bool isFriend, NetworkConnection con = null) {
            if(!con.TryGetUserId(out var id)) return;
            if(!UserIdDataManager.Server_IsUserIdValid(userId)) return;
            if(isFriend == UserIdDataManager.Server_HasFriended(id, userId)) return;
            //unblock if blocked and adding friend
            if(isFriend && UserIdDataManager.Server_HasBlocked(id, userId)) {
                UserIdDataManager.Server_UnblockUser(id,userId);
                Client_ReceiveBlockUpdate(con,userId,false);
            }
            //add friend
            if(isFriend)UserIdDataManager.Server_FriendUser(id,userId);
            else UserIdDataManager.Server_UnfriendUser(id,userId);
            if(!friendshipsRequireAcceptance) {
                Client_ReceiveFriendUpdate(con,userId,isFriend);
                return;
            }
            //acceptance is required
            TryGetConnection(userId, out var friendCon);
            //request is pending
            if(isFriend && UserIdDataManager.Server_HasApprovedFriendship(id, userId)) {
                //update friends info
                if(friendCon != null&&friendCon.IsActive) {
                    Client_ReceivePendingUpdate(friendCon, id, false);
                    Client_ReceiveFriendUpdate(friendCon, id, true);
                }
                //update your info
                Client_ReceiveFriendUpdate(con,userId, true);
                Client_ReceiveRequestingUpdate(con,userId,false);
                return;
            }
            if(!isFriend) {
                if(friendCon != null && friendCon.IsActive) {
                    Client_ReceivePendingUpdate(friendCon, id, true);
                    Client_ReceiveFriendUpdate(friendCon, id, false);
                }
                Client_ReceiveFriendUpdate(con,userId, false);
                Client_ReceiveRequestingUpdate(con,userId,true);
            }
            //no pending request
            if(friendCon != null && friendCon.IsActive)
                Client_ReceiveRequestingUpdate(friendCon,id,true);
            Client_ReceivePendingUpdate(con,userId,true);
        }

        [ServerRpc(RequireOwnership = false)]
        private void Server_ReceiveBlockedUpdate(uint userId, bool isBlocked, NetworkConnection con = null) {
            if(!con.TryGetUserId(out var id)) return;
            if(!UserIdDataManager.Server_IsUserIdValid(userId)) return;
            if(isBlocked == UserIdDataManager.Server_HasBlocked(id, userId)) return;
            if(isBlocked&&!friendshipsRequireAcceptance && UserIdDataManager.Server_HasFriended(id, userId)) {
                UserIdDataManager.Server_UnfriendUser(id,userId);
                Client_ReceiveFriendUpdate(con,userId,false);
            }
            if(isBlocked&&friendshipsRequireAcceptance && UserIdDataManager.Server_HasFriended(id, userId)) {
                TryGetConnection(userId, out var friendCon);
                UserIdDataManager.Server_UnfriendUser(id,userId);
                if(friendCon != null && friendCon.IsActive) {
                    Client_ReceiveFriendUpdate(friendCon, id, false);
                    Client_ReceivePendingUpdate(friendCon, id, true);
                }
                Client_ReceiveFriendUpdate(con,userId,false);
            }
            if(isBlocked) UserIdDataManager.Server_BlockUser(id,userId);
            else UserIdDataManager.Server_UnblockUser(id,userId);
            Client_ReceiveBlockUpdate(con,userId,isBlocked);
        }
        
    }
}