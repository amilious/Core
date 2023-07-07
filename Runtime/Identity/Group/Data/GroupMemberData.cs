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
        private const ushort VERSION = 2;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        [DataMember, SerializeField] private uint _groupId;
        [DataMember, SerializeField] private uint _userId;
        [DataMember, SerializeField] private short _rank;
        [DataMember, SerializeField] private uint? _invitedBy;
        [DataMember, SerializeField] private uint? _approvedBy;
        [DataMember, SerializeField] private string _applicationRequest;
        [DataMember, SerializeField] private MemberStatus _memberStatus;
        [DataMember, SerializeField] private DateTime? _inviteDate;
        [DataMember, SerializeField] private DateTime? _appliedDate;
        [DataMember, SerializeField] private DateTime? _joinedDate;
        [DataMember, SerializeField] private DateTime? _leaveDate;
        [DataMember, SerializeField] private DateTime _rankDate;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the manager for the data.
        /// </summary>
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
        /// This property contains the user's group rank.  Setting this property will automatically update the rank date.
        /// </summary>
        public short Rank {
            get => _rank;
            set {
                if(_rank == value) return;
                _rank = value;
                _rankDate = DateTime.UtcNow;
                Updated();
            }
        }

        public string ApplicationRequest {
            get => _applicationRequest;
            set {
                if(_applicationRequest == value) return;
                _applicationRequest = value;
                _memberStatus = MemberStatus.Applying;
                _appliedDate = DateTime.UtcNow;
                Updated();
            }
        }

        /// <summary>
        /// This property contains the id of the member who invited the user to the group, or null.  Setting this
        /// property will automatically update the status and the invited date.
        /// </summary>
        public uint? InvitedBy {
            get => _invitedBy;
            set {
                if(_invitedBy == value || Status == MemberStatus.Member) return;
                _invitedBy = value;
                if(_invitedBy == null) {
                    if(Status == MemberStatus.Invited) {
                        _memberStatus = MemberStatus.None;
                        _inviteDate = null;
                    }
                }
                else {
                    _memberStatus = MemberStatus.Invited;
                    _inviteDate = DateTime.UtcNow;
                }
                Updated();
            }
        }

        /// <summary>
        /// This property contains the id of the member that approved the user's join request, or null.  Setting this
        /// property will automatically update the status and the joined date.
        /// </summary>
        public uint? ApprovedBy {
            get => _approvedBy;
            set {
                if(_approvedBy == value) return;
                _approvedBy = value;
                if(_approvedBy != null && Status != MemberStatus.Member) {
                    _memberStatus = MemberStatus.Member;
                    _joinedDate = DateTime.UtcNow;
                }
                Updated();
            }
        }

        /// <summary>
        /// This property contains the users group status.  Setting this property will automatically update the
        /// date and times.
        /// </summary>
        public MemberStatus Status {
            get => _memberStatus;
            set {
                if(_memberStatus == value) return;
                _memberStatus = value;
                switch(value) {
                    case MemberStatus.None:
                        _appliedDate = null;
                        _inviteDate = null;
                        _joinedDate = null;
                        _leaveDate = null;
                        _approvedBy = null;
                        _invitedBy = null;
                        break;
                    case MemberStatus.Member:
                        _joinedDate = DateTime.UtcNow;
                        break;
                    case MemberStatus.Invited:
                        _inviteDate = DateTime.UtcNow;
                        break;
                    case MemberStatus.Applying:
                        _appliedDate = DateTime.UtcNow;
                        break;
                    case MemberStatus.Kicked:
                        _leaveDate = DateTime.UtcNow;
                        break;
                    case MemberStatus.Left:
                        _leaveDate = DateTime.UtcNow;
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
                Updated();
            }
        }

        /// <summary>
        /// This property contains the local date and time for when the user was invited to the group.
        /// </summary>
        public DateTime? InviteDate => _inviteDate?.ToLocalTime();

        /// <summary>
        /// This property contains the local date and time for when the user joined the group.
        /// </summary>
        public DateTime? JoinDate => _joinedDate?.ToLocalTime();

        /// <summary>
        /// This property contains the local date and time for when the user applied to the group.
        /// </summary>
        public DateTime? AppliedDate => _appliedDate?.ToLocalTime();

        /// <summary>
        /// This property contains the local date and time for when the user left the group.
        /// </summary>
        public DateTime? LeaveDate => _leaveDate?.ToLocalTime();

        public DateTime RankDate => _rankDate.ToLocalTime();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constructor is used to create a new group member info.
        /// </summary>
        /// <param name="groupId">The id of the group.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="status">The membership status of the user.</param>
        /// <param name="rank">The rank of the user.</param>
        /// <param name="invitedBy">The id of the user's invitor.</param>
        /// <param name="approvedBy">The id of the user's approver.</param>
        /// <param name="rankDate">The date that the last rank was changed.</param>
        /// <param name="inviteDate">The Utc date time of when the user was invited.</param>
        /// <param name="appliedDate">The Utc date time of when the user applied.</param>
        /// <param name="joinedDate">The Utc date time of when the user joined.</param>
        /// <param name="leaveDate">The Utc date time of when the user joined.</param>
        /// <param name="applicationRequest">The user's application request.</param>
        public GroupMemberData(uint groupId, uint userId, MemberStatus status = MemberStatus.None, 
            short rank = 0, uint? invitedBy = null, uint? approvedBy = null, DateTime? rankDate = null, 
            DateTime? inviteDate = null, DateTime? appliedDate = null, DateTime? joinedDate = null, 
            DateTime? leaveDate =null, string applicationRequest = null) {

            switch(status) { // add missing dates
                case MemberStatus.Member: joinedDate??= DateTime.UtcNow; break;
                case MemberStatus.Invited: inviteDate??=DateTime.UtcNow; break;
                case MemberStatus.Applying: appliedDate??=DateTime.UtcNow; break;
                case MemberStatus.Kicked: leaveDate ??= DateTime.UtcNow; break;
                case MemberStatus.Left: leaveDate ??= DateTime.UtcNow; break;
            }
            
            _groupId = groupId;
            _userId = userId;
            _memberStatus = status;
            _rank = rank;
            _invitedBy = invitedBy;
            _approvedBy = approvedBy;
            //added in version 2
            _inviteDate = inviteDate;
            _appliedDate = appliedDate;
            _joinedDate = joinedDate;
            _leaveDate = leaveDate;
            _rankDate = rankDate ?? DateTime.UtcNow;
            _applicationRequest = applicationRequest;
        }
        
        /// <summary>
        /// This constructor is used when deserializing the object.
        /// </summary>
        /// <param name="info">The serialized info.</param>
        /// <param name="context">The streaming context.</param>
        /// <exception cref="InvalidDataException">Thrown if the data is invalid.</exception>
        protected GroupMemberData(SerializationInfo info, StreamingContext context) {
            var version = info.GetUInt16(nameof(VERSION));
            if(version > VERSION) throw new InvalidDataException("The read data is not formatted correctly!");
            _groupId = info.GetUInt32(nameof(GroupId));
            _userId = info.GetUInt32(nameof(UserId));
            _rank = info.GetInt16(nameof(Rank));
            _invitedBy = (uint?)info.GetValue(nameof(InvitedBy), typeof(int?));
            _approvedBy = (uint?)info.GetValue(nameof(ApprovedBy), typeof(int?));
            _memberStatus = (MemberStatus)info.GetValue(nameof(Status), typeof(MemberStatus));
            if(version < 2) return; //added in version 2
            _inviteDate = (DateTime?)info.GetValue(nameof(InviteDate), typeof(DateTime?));
            _appliedDate = (DateTime?)info.GetValue(nameof(AppliedDate), typeof(DateTime?));
            _joinedDate = (DateTime?)info.GetValue(nameof(JoinDate), typeof(DateTime?));
            _leaveDate = (DateTime?)info.GetValue(nameof(LeaveDate), typeof(DateTime?));
            _rankDate = info.GetDateTime(nameof(RankDate));
            _applicationRequest = info.GetString(nameof(ApplicationRequest));
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
            info.AddValue(nameof(GroupId), _groupId);
            info.AddValue(nameof(UserId), _userId);
            info.AddValue(nameof(Rank), _rank);
            info.AddValue(nameof(InvitedBy), _invitedBy);
            info.AddValue(nameof(ApprovedBy), _approvedBy);
            info.AddValue(nameof(Status), _memberStatus);
            //added in version 2
            info.AddValue(nameof(InviteDate), _inviteDate);
            info.AddValue(nameof(AppliedDate), _appliedDate);
            info.AddValue(nameof(JoinDate), _joinedDate);
            info.AddValue(nameof(LeaveDate), _leaveDate);
            info.AddValue(nameof(RankDate), _rankDate);
            info.AddValue(nameof(ApplicationRequest), _applicationRequest);
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