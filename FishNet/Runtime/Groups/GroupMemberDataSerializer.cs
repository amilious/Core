
using System;
using FishNet.Serializing;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.Group.Data;

namespace Amilious.Core.FishNet.Groups {
    
    public static class GroupMemberDataSerializer {
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to write a group identity's values.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="memberData">The group member data that you want to write.</param>
        public static void WriteGroupMemberData(this Writer writer, GroupMemberData memberData) {
            writer.WriteUInt32(memberData.GroupId, AutoPackType.Unpacked);
            writer.WriteUInt32(memberData.UserId, AutoPackType.Unpacked);
            writer.WriteByte((byte)memberData.Status);
            writer.WriteInt16(memberData.Rank);
            writer.WriteBoolean(memberData.InvitedBy.HasValue);
            if(memberData.InvitedBy.HasValue) writer.WriteUInt32(memberData.InvitedBy.Value, AutoPackType.Unpacked);
            writer.WriteBoolean(memberData.ApprovedBy.HasValue);
            if(memberData.ApprovedBy.HasValue) writer.WriteUInt32(memberData.ApprovedBy.Value, AutoPackType.Unpacked);
            //added in version 2
            writer.WriteDateTime(memberData.RankDate);
            writer.WriteBoolean(memberData.InviteDate.HasValue);
            if(memberData.InviteDate.HasValue) writer.WriteDateTime(memberData.InviteDate.Value);
            writer.WriteBoolean(memberData.AppliedDate.HasValue);
            if(memberData.AppliedDate.HasValue) writer.WriteDateTime(memberData.AppliedDate.Value);
            writer.WriteBoolean(memberData.JoinDate.HasValue);
            if(memberData.JoinDate.HasValue) writer.WriteDateTime(memberData.JoinDate.Value);
            writer.WriteBoolean(memberData.LeaveDate.HasValue);
            if(memberData.LeaveDate.HasValue) writer.WriteDateTime(memberData.LeaveDate.Value);
            writer.WriteString(memberData.ApplicationRequest);
        }

        /// <summary>
        /// This method is used to read a group member data's values.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Group member data build from the reader's values.</returns>
        public static GroupMemberData ReadGroupMemberData(this Reader reader) {
            var group = reader.ReadUInt32(AutoPackType.Unpacked);
            var user = reader.ReadUInt32(AutoPackType.Unpacked);
            var status = (MemberStatus)reader.ReadByte();
            var rank = reader.ReadInt16();
            uint? invitedBy = reader.ReadBoolean() ? reader.ReadUInt32(AutoPackType.Unpacked) : null;
            uint? approvedBy = reader.ReadBoolean() ? reader.ReadUInt32(AutoPackType.Unpacked) : null;
            //added in version 2
            var rankDate = reader.ReadDateTime();
            DateTime? inviteDate = reader.ReadBoolean() ? reader.ReadDateTime() : null;
            DateTime? appliedDate = reader.ReadBoolean() ? reader.ReadDateTime() : null;
            DateTime? joinedDate = reader.ReadBoolean() ? reader.ReadDateTime() : null;
            DateTime? leaveDate = reader.ReadBoolean() ? reader.ReadDateTime() : null;
            var applicationRequest = reader.ReadString();
            //create the object
            return new GroupMemberData(group, user, status, rank, invitedBy, approvedBy, rankDate, inviteDate, 
                appliedDate, joinedDate, leaveDate, applicationRequest);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}