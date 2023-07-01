
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
            return new GroupMemberData(group, user, status, rank, invitedBy, approvedBy);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}