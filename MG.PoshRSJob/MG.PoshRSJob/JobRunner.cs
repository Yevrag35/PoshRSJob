using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Threading.Tasks;

namespace MG.Jobs
{
    public class JobRunner
    {
        private List<RSJob> _jobs;

        public IEnumerable<RSJob> Jobs => _jobs;


    }
}
