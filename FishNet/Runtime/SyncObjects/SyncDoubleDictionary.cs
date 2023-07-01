using System.Collections;
using System.Collections.Generic;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;

namespace Amilious.Core.FishNet.SyncObjects {
    
    public class ISyncDoubleDictionary<TKey1,TKey2, TValue> : SyncBase, 
        IDictionary<TKey1, IDictionary<TKey2, TValue>>, 
        IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>> {
        private int _count;
        private ICollection<TKey1> _keys;
        private ICollection<IDictionary<TKey2, TValue>> _values;
        private int _count1;
        private IEnumerable<TKey1> _keys1;
        private IEnumerable<IReadOnlyDictionary<TKey2, TValue>> _values1;

        IEnumerator<KeyValuePair<TKey1, IReadOnlyDictionary<TKey2, TValue>>> IEnumerable<KeyValuePair<TKey1, IReadOnlyDictionary<TKey2, TValue>>>.GetEnumerator() {
            throw new System.NotImplementedException();
        }

        IEnumerator<KeyValuePair<TKey1, IDictionary<TKey2, TValue>>> IEnumerable<KeyValuePair<TKey1, IDictionary<TKey2, TValue>>>.GetEnumerator() {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new System.NotImplementedException();
        }

        public void Add(KeyValuePair<TKey1, IDictionary<TKey2, TValue>> item) {
            throw new System.NotImplementedException();
        }

        public void Clear() {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey1, IDictionary<TKey2, TValue>> item) {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey1, IDictionary<TKey2, TValue>>[] array, int arrayIndex) {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey1, IDictionary<TKey2, TValue>> item) {
            throw new System.NotImplementedException();
        }

        int ICollection<KeyValuePair<TKey1, IDictionary<TKey2, TValue>>>.Count => _count;

        public bool IsReadOnly { get; }
        public void Add(TKey1 key, IDictionary<TKey2, TValue> value) {
            throw new System.NotImplementedException();
        }

        bool IDictionary<TKey1, IDictionary<TKey2, TValue>>.ContainsKey(TKey1 key) {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(TKey1 key, out IReadOnlyDictionary<TKey2, TValue> value) {
            throw new System.NotImplementedException();
        }

        IReadOnlyDictionary<TKey2, TValue> IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>>.this[TKey1 key] => throw new System.NotImplementedException();

        IEnumerable<TKey1> IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>>.Keys => _keys1;

        IEnumerable<IReadOnlyDictionary<TKey2, TValue>> IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>>.Values => _values1;

        public bool Remove(TKey1 key) {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(TKey1 key, out IDictionary<TKey2, TValue> value) {
            throw new System.NotImplementedException();
        }

        bool IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>>.ContainsKey(TKey1 key) {
            throw new System.NotImplementedException();
        }

        public IDictionary<TKey2, TValue> this[TKey1 key] {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        ICollection<TKey1> IDictionary<TKey1, IDictionary<TKey2, TValue>>.Keys => _keys;

        ICollection<IDictionary<TKey2, TValue>> IDictionary<TKey1, IDictionary<TKey2, TValue>>.Values => _values;

        int IReadOnlyCollection<KeyValuePair<TKey1, IReadOnlyDictionary<TKey2, TValue>>>.Count => _count1;
    }
}
