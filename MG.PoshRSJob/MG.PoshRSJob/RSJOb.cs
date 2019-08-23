using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace MG.Jobs
{
    public class RSJob
    {
        private List<object> _args;
        private ScriptDictionary _funcs;
        private bool _hasStarted = false;
        private bool _isComp = false;
        private VariableCollection _varCol;
        private ValueType _backingVt;

        public ICollection<object> Arguments => _args;
        public bool IsCompleted => _isComp;
        public bool IsStarted => _hasStarted;
        public ValueType JobId
        {
            get => _backingVt;
            set
            {
                if (value is Guid guid)
                    _backingVt = guid;

                else if (value is int int32)
                    _backingVt = int32;

                else if (value is long int64)
                    _backingVt = int64;

                else
                    throw new InvalidCastException("JobId must be a Guid, int32, or int64 valuetype.");
            }
        }
        public string JobName { get; set; }
        public ScriptDictionary Functions => _funcs;
        public object PSItem { get; set; }
        public JobResult Result { get; private set; }
        public ScriptBlock Script { get; set; }
        public VariableCollection Variables => _varCol;

        public RSJob()
        {
            _args = new List<object>();
            _funcs = new ScriptDictionary();
            _varCol = new VariableCollection();
        }
        public RSJob(ScriptBlock sb)
            :this() => this.Script = sb;
        public RSJob(ScriptBlock sb, object psItem)
            : this(sb) => this.PSItem = psItem;

        public RSJob(ScriptBlock sb, object psItem, IEnumerable<object> arguments)
            : this(sb, psItem) => _args.AddRange(arguments);

        public async Task InvokeAsync()
        {
            await Task.Run(() =>
            {
                _hasStarted = true;
                if (this.PSItem != null)
                {
                    _varCol.InsertAt(0, "_", this.PSItem);
                }

                ICollection<PSObject> objs = this.Script.InvokeWithContext(_funcs, _varCol, _args);
                this.Result = JobResult.FromPSObjectCollection(this.JobId, this.JobName, objs);
            });
        }
    }

    public class JobResult
    {
        public ValueType Id { get; private set; }
        public string JobName { get; set; }
        public ICollection<PSObject> Result { get; private set; }

        internal static JobResult FromPSObjectCollection(ValueType jobId, string jobName, ICollection<PSObject> psObjects)
        {
            return new JobResult
            {
                Id = jobId,
                JobName = jobName,
                Result = psObjects
            };
        }
    }
}
