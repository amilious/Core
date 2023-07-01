using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;

// ReSharper disable InconsistentNaming
namespace Amilious.Core.Identity.Group.Data {
    
    [DataContract, Serializable]
    public class GroupMemberData : ISerializable {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is used to allow us to change the serialization format.
        /// </summary>
        private const ushort VERSION = 1;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        [DataMember, SerializeField] private uint _groupId;
        [DataMember, SerializeField] private uint _userId;
        [DataMember, SerializeField] private short _rank;
        [DataMember, SerializeField] private uint? _invitedBy;
        [DataMember, SerializeField] private uint? _approvedBy;
        [DataMember, SerializeField] private MemberStatus _memberStatus;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public IGroupDataManager Manager { get; private set; }
        
        /// <summary>
        /// This property contains the group id.
        /// </summary>
        public uint GroupId => _groupId;

        /// <summary>
        /// This property contains the user id.
        /// </summary>
        public uint UserId => _userId;

        /// <summary>
        /// This property contains the user's group rank.
        /// </summary>
        public short Rank {
            get => _rank;
            set {
                if(_rank == value) return;
                _rank = value;
                Updated();
            }
        }

        /// <summary>
        /// This property contains the id of the member who invited the user to the group, or null.
        /// </summary>
        public uint? InvitedBy {
            get => _invitedBy;
            set {
                if(_invitedBy == value) return;
                _invitedBy = value;
                Updated();
            }
        }

        /// <summary>
        /// This property contains the id of the member that approved the user's join request, or null.
        /// </summary>
        public uint? ApprovedBy {
            get => _approvedBy;
            set {
                if(_approvedBy == value) return;
                _approvedBy = value;
                Updated();
            }
        }

        /// <summary>
        /// This property contains the users group status.
        /// </summary>
        public MemberStatus Status {
            get => _memberStatus;
            set {
                if(_memberStatus == value) return;
                _memberStatus = value;
                Updated();
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        public GroupMemberData(uint groupId, uint userId, MemberStatus status = MemberStatus.None, 
            short rank = 0, uint? invitedBy = null, uint? approvedBy = null ) {
            _groupId = groupId;
            _userId = userId;
            _memberStatus = status;
            _rank = rank;
            _invitedBy = invitedBy;
            _approvedBy = approvedBy;
        }
        
        protected GroupMemberData(SerializationInfo info, StreamingContext context) {
            var version = info.GetUInt16(nameof(VERSION));
            if(version != VERSION) throw new InvalidDataException("The read data is not formatted correctly!");
            _groupId = info.GetUInt32(nameof(GroupId));
            _userId = info.GetUInt32(nameof(UserId));
            _rank = info.GetInt16(nameof(Rank));
            _invitedBy = (uint?)info.GetValue(nameof(InvitedBy), typeof(int?));
            _approvedBy = (uint?)info.GetValue(nameof(ApprovedBy), typeof(int?));
            _memberStatus = (MemberStatus)info.GetValue(nameof(Status), typeof(MemberStatus));
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to notify listeners of an update.
        /// </summary>
        private void Updated() {
            if(Manager == null) return;
            Manager.OnMemberDataUpdated(this);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(VERSION),VERSION);
            info.AddValue(nameof(GroupId), GroupId);
            info.AddValue(nameof(UserId), UserId);
            info.AddValue(nameof(Rank), Rank);
            info.AddValue(nameof(InvitedBy), InvitedBy);
            info.AddValue(nameof(ApprovedBy), ApprovedBy);
            info.AddValue(nameof(Status), Status);
        }

        /// <summary>
        /// Register the data manager that is responsible for managing the group data.
        /// </summary>
        /// <param name="manager">The manager that you want to register to the data.</param>
        /// <remarks>Do not register the data manager on the client <b>ONLY ON THE SERVER</b></remarks>
        public void RegisterDataManager(IGroupDataManager manager) => Manager = manager;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}