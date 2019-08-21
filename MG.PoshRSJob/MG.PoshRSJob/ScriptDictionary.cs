using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace MG.Jobs
{
    public class ScriptDictionary : IDictionary<string, ScriptBlock>, IDictionary
    {
        private Dictionary<string, ScriptBlock> _dict;

        public ScriptDictionary() => _dict = new Dictionary<string, ScriptBlock>();
        public ScriptDictionary(int capacity) => _dict = new Dictionary<string, ScriptBlock>(capacity);

        public ScriptBlock this[string key]
        {
            get => _dict[key];
            set => _dict[key] = value;
        }

        public ICollection<string> Keys => _dict.Keys;
        public ICollection<ScriptBlock> Values => _dict.Values;
        public int Count => _dict.Count;
        bool ICollection<KeyValuePair<string, ScriptBlock>>.IsReadOnly => false;

        ICollection IDictionary.Keys => ((IDictionary)_dict).Keys;

        ICollection IDictionary.Values => ((IDictionary)_dict).Values;

        bool IDictionary.IsReadOnly => false;

        bool IDictionary.IsFixedSize => ((IDictionary)_dict).IsFixedSize;

        object ICollection.SyncRoot => ((IDictionary)_dict).SyncRoot;

        bool ICollection.IsSynchronized => ((IDictionary)_dict).IsSynchronized;

        public object this[object key] { get => ((IDictionary)_dict)[key]; set => ((IDictionary)_dict)[key] = value; }

        public void Add(string key, ScriptBlock value) => _dict.Add(key, value);

        public void AddMultiple(IDictionary functions)
        {
            foreach (DictionaryEntry de in functions)
            {
                string key = Convert.ToString(de.Key);
                if (de.Value is ScriptBlock sb)
                {
                    _dict.Add(key, sb);
                }
            }
        }

        void ICollection<KeyValuePair<string, ScriptBlock>>.Add(KeyValuePair<string, ScriptBlock> item) => 
            ((IDictionary<string, ScriptBlock>)_dict).Add(item);
        public void Clear() => _dict.Clear();
        bool ICollection<KeyValuePair<string, ScriptBlock>>.Contains(KeyValuePair<string, ScriptBlock> item) => _dict.Contains(item);
        public bool ContainsKey(string key) => _dict.ContainsKey(key);
        void ICollection<KeyValuePair<string, ScriptBlock>>.CopyTo(KeyValuePair<string, ScriptBlock>[] array, int arrayIndex) =>
            ((IDictionary<string, ScriptBlock>)_dict).CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<string, ScriptBlock>> GetEnumerator() => _dict.GetEnumerator();
        public bool Remove(string key) => _dict.Remove(key);
        bool ICollection<KeyValuePair<string, ScriptBlock>>.Remove(KeyValuePair<string, ScriptBlock> item) =>
            ((IDictionary<string, ScriptBlock>)_dict).Remove(item);
        public bool TryGetValue(string key, out ScriptBlock value) => _dict.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
        public bool Contains(object key) => ((IDictionary)_dict).Contains(key);
        void IDictionary.Add(object key, object value) => ((IDictionary)_dict).Add(key, value);
        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)_dict).GetEnumerator();
        void IDictionary.Remove(object key) => ((IDictionary)_dict).Remove(key);
        void ICollection.CopyTo(Array array, int index) => ((IDictionary)_dict).CopyTo(array, index);

        public static implicit operator ScriptDictionary(Hashtable hashtable)
        {
            var dict = new ScriptDictionary(hashtable.Count);
            foreach (DictionaryEntry de in hashtable)
            {
                string key = Convert.ToString(de.Key);
                if (de.Value is ScriptBlock sb)
                {
                    dict.Add(key, sb);
                }
            }
            return dict;
        }
    }
}
