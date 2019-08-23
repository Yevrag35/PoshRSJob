using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace MG.Jobs
{
    public class VariableCollection : ICollection, ICollection<PSVariable>
    {
        private List<PSVariable> _list;

        public VariableCollection() => _list = new List<PSVariable>();
        public VariableCollection(int capacity) => _list = new List<PSVariable>(capacity);
        private VariableCollection(IEnumerable<PSVariable> variables) => _list = new List<PSVariable>(variables);

        public int Count => _list.Count;
        bool ICollection<PSVariable>.IsReadOnly => false;
        public object SyncRoot => ((ICollection)this._list).SyncRoot;
        public bool IsSynchronized => ((ICollection)this._list).IsSynchronized;

        #region METHODS
        void ICollection<PSVariable>.Add(PSVariable item)
        {
            if (!_list.Exists(x => x.Name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)))
                _list.Add(item);
        }
        public bool Add(string variableName, object value)
        {
            bool result = false;
            if (!_list.Exists(x => x.Name.Equals(variableName, StringComparison.CurrentCultureIgnoreCase)))
            {
                _list.Add(new PSVariable(variableName, value));
                result = true;
            }
            return result;
        }
        public bool AddRange(IDictionary hashtable)
        {
            bool result = true;
            foreach (DictionaryEntry de in hashtable)
            {
                result = this.Add(Convert.ToString(de.Key), de.Value);
                if (!result)
                    break;
            }
            return result;
        }
        public void AddRange(IEnumerable<PSVariable> variables)
        {
            foreach (PSVariable variable in variables)
            {
                ((ICollection<PSVariable>)this).Add(variable);
            }
        }
        public ReadOnlyCollection<PSVariable> AsReadOnly() => _list.AsReadOnly();
        public void Clear() => _list.Clear();
        bool ICollection<PSVariable>.Contains(PSVariable item) => _list.Contains(item);
        public bool Contains(string name) => _list.Exists(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        void ICollection<PSVariable>.CopyTo(PSVariable[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        void ICollection.CopyTo(Array array, int index) => ((ICollection)this._list).CopyTo(array, index);
        public bool Exists(Predicate<PSVariable> match) => _list.Exists(match);
        public PSVariable Find(Predicate<PSVariable> match) => _list.Find(match);
        public VariableCollection FindAll(Predicate<PSVariable> match) => new VariableCollection(_list.FindAll(match));
        public int FindIndex(Predicate<PSVariable> match) => _list.FindIndex(match);
        public PSVariable FindLast(Predicate<PSVariable> match) => _list.FindLast(match);
        public int FindLastIndex(Predicate<PSVariable> match) => _list.FindLastIndex(match);
        public IEnumerator<PSVariable> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
        public int IndexOf(PSVariable item) => _list.IndexOf(item);
        public bool InsertAt(int index, string name, object value)
        {
            bool result = false;
            if (!_list.Exists(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
            {
                _list.Insert(index, new PSVariable(name, value));
                result = true;
            }
            return result;
        }
        bool ICollection<PSVariable>.Remove(PSVariable item) => _list.Remove(item);
        public void Remove(string name) => _list.RemoveAll(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        public void RemoveAll(Predicate<PSVariable> match) => _list.RemoveAll(match);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public void RemoveRange(int index, int count) => _list.RemoveRange(index, count);
        public PSVariable[] ToArray() => _list.ToArray();
        public bool TrueForAll(Predicate<PSVariable> match) => _list.TrueForAll(match);

        #endregion

        #region OPERATORS/CASTS

        public static implicit operator List<PSVariable>(VariableCollection varCol) => varCol._list;
        public static implicit operator VariableCollection(Hashtable ht)
        {
            var varCol = new VariableCollection(ht.Count);
            varCol.AddRange(ht);
            return varCol;
        }

        #endregion

        private class VariableEquality : IEqualityComparer<PSVariable>
        {
            public bool Equals(PSVariable x, PSVariable y) => x.Name.Equals(y.Name, StringComparison.CurrentCultureIgnoreCase);
            public int GetHashCode(PSVariable obj) => obj.GetHashCode();
        }
    }
}
