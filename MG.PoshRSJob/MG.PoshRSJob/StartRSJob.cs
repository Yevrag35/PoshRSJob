using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Threading.Tasks;

namespace MG.PoshRSJob
{
    [Cmdlet(VerbsLifecycle.Start, "RSJob", ConfirmImpact = ConfirmImpact.None)]
    public class StartRSJob : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public ScriptBlock ScriptBlock { get; set; }

        [Parameter(Mandatory = )]
    }
}
