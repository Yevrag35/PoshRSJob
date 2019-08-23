using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MG.Jobs
{
    public class JobResultCollection : IReadOnlyCollection<JobResult>, ICollection
    {
        #region FIELDS/CONSTANTS
        private List<JobResult> _list;

        #endregion

        #region PROPERTIES
        public int Count => _list.Count;
        public object SyncRoot => ((ICollection)this._list).SyncRoot;
        public bool IsSynchronized => false;

        #endregion

        #region CONSTRUCTORS
        internal JobResultCollection() => _list = new List<JobResult>();
        internal JobResultCollection(int capacity) => _list = new List<JobResult>(capacity);
        internal JobResultCollection(IEnumerable<JobResult> results) => _list = new List<JobResult>(results);

        #endregion

        #region PUBLIC METHODS
        void ICollection.CopyTo(Array array, int index) => ((ICollection)this._list).CopyTo(array, index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        IEnumerator<JobResult> IEnumerable<JobResult>.GetEnumerator() => _list.GetEnumerator();

        #endregion

        #region PRIVATE METHODS


        #endregion
    }
}