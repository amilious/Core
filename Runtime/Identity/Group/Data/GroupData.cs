using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;

// ReSharper disable InconsistentNaming
namespace Amilious.Core.Identity.Group.Data {
    
    [DataContract, Serializable]
    public class GroupData : ISerializable {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const ushort VERSION = 2;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        [DataMember, SerializeField] private uint _id;
        [DataMember, SerializeField] private string _name;
        [DataMember, SerializeField] private uint _owner;
        [DataMember, SerializeField] private uint _creator;
        [DataMember, SerializeField] private DateTime _created;
        [DataMember, SerializeField] private GroupAuthType _authType;
        [DataMember, SerializeField] private GroupType _groupType;
        [DataMember, SerializeField] private string _password;
        [DataMember, SerializeField] private string _salt;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public IGroupDataManager Manager { get; private set; }
        
        public uint Id => _id;

        public string Name {
            get => _name;
            set {
                if(_name == value) return;
                _name = value;
                Update();
            }
        }

        public uint Owner {
            get => _owner;
            set {
                if(_owner == value) return;
                _owner = value;
                Update();
            }
        }

        public uint Creator => _creator;

        public DateTime Created {
            get => _created;
            set {
                if(_created == value) return;
                _created = value;
                Update();
            }
        }

        public GroupAuthType AuthType {
            get => _authType;
            set {
                if(_authType == value) return;
                _authType = value;
                Update();
            }
        }

        public GroupType GroupType {
            get => _groupType;
            set {
                if(_groupType == value) return;
                _groupType = value;
                Update();
            }
        }

        public string Password {
            get => _password;
            set {
                if(_password == value) return;
                _password = value;
                Update();
            }
        }
        
        public string Salt { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        public GroupData(uint id, string name, GroupType groupType, uint owner, uint creator, DateTime created,
            GroupAuthType authType = GroupAuthType.None, string password = null, string salt = null) {
            _id = id;
            _name = name;
            _groupType = groupType;
            _owner = owner;
            _creator = creator;
            _created = created;
            _authType = authType;
            _password = password;
            _salt = salt;
        }
        
        protected GroupData(SerializationInfo info, StreamingContext context) {
            var version = info.GetUInt16(nameof(VERSION));
            Debug.Log(Application.persistentDataPath);
            if(version != VERSION) throw new InvalidDataException("The read data is not formatted correctly!");
            _id = info.GetUInt32(nameof(Id));
            _name = info.GetString(nameof(Name));
            _owner = info.GetUInt32(nameof(Owner));
            _creator = info.GetUInt32(nameof(Creator));
            _created = info.GetDateTime(nameof(Created));
            _authType = (GroupAuthType)info.GetValue(nameof(AuthType),typeof(GroupAuthType));
            _groupType = (GroupType)info.GetValue(nameof(GroupType), typeof(GroupType));
            _password = info.GetString(nameof(Password));
            if(version < 2) return; //value added in version 2
            _salt = info.GetString(nameof(Salt));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private void Update() {
            if(Manager is null) return;
            Manager.OnGroupDataUpdated(this);
        }

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(VERSION),VERSION);
            info.AddValue(nameof(Id), Id);
            info.AddValue(nameof(Name), Name);
            info.AddValue(nameof(Owner), Owner);
            info.AddValue(nameof(Creator), Creator);
            info.AddValue(nameof(Created), Created);
            info.AddValue(nameof(AuthType), AuthType);
            info.AddValue(nameof(GroupType), GroupType);
            info.AddValue(nameof(Password), Password);
            //added in version 2
            info.AddValue(nameof(Salt), Password);
        }
        
        public void RegisterDataManager(IGroupDataManager manager) => Manager = manager;

        /// <summary>
        /// This method is used to get the group identity from the group data.
        /// </summary>
        /// <param name="manager">The group identity manager.</param>
        /// <returns>The group identity for the group data.</returns>
        public GroupIdentity GetGroupIdentity(IGroupIdentityManager manager) =>
            new GroupIdentity(manager, Id, Name, GroupType, Owner, AuthType, Salt);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}