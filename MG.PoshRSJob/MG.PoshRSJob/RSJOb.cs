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
        private VariableCollection _varCol;

        public ICollection<object> Arguments => _args;
        public object PSItem { get; set; }
        public ScriptDictionary Functions => _funcs;
        public VariableCollection Variables => _varCol;
        public ScriptBlock Script { get; set; }

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

        public async Task<Collection<PSObject>> InvokeAsync()
        {
            if (this.PSItem != null)
            {
                _args.Insert(0, this.PSItem);
            }

            return await Task.Run(() =>
            {
                return this.Script.InvokeWithContext(_funcs, _varCol, _args);
            });
        }

    }
}
