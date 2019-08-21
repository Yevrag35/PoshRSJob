using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace MG.Jobs
{
    public class VariableCollection : ICollection<PSVariable>
    {
        private List<PSVariable> _list;

        public VariableCollection() => _list = new List<PSVariable>();
        public VariableCollection(int capacity) => _list = new List<PSVariable>(capacity);
        public VariableCollection(IEnumerable<PSVariable> variables)
        {
            var ieq = new VariableEquality();
            _list = new List<PSVariable>(variables.Distinct(ieq));
        }

        public int Count => _list.Count;
        bool ICollection<PSVariable>.IsReadOnly => false;

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
        public void Clear() => _list.Clear();
        bool ICollection<PSVariable>.Contains(PSVariable item) => _list.Contains(item);
        public bool Contains(string name) => _list.Exists(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        void ICollection<PSVariable>.CopyTo(PSVariable[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public IEnumerator<PSVariable> GetEnumerator() => _list.GetEnumerator();
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
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public static implicit operator List<PSVariable>(VariableCollection varCol)
        {
            return varCol._list;
        }

        private class VariableEquality : IEqualityComparer<PSVariable>
        {
            public bool Equals(PSVariable x, PSVariable y) => x.Name.Equals(y.Name, StringComparison.CurrentCultureIgnoreCase);
            public int GetHashCode(PSVariable obj) => obj.GetHashCode();
        }
    }
}
