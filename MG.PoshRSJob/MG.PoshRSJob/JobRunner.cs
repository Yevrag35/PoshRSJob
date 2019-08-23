using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MG.Jobs
{
    public class JobRunner
    {
        #region PRIVATE FIELDS/CONSTANTS
        private List<RSJob> _jobs;
        private ReadOnlyCollection<JobResult> _results;

        #endregion

        #region PROPERTIES

        public IReadOnlyCollection<JobResult> AllResults => _results;
        public ICollection<RSJob> Jobs => _jobs;
        public int JobsLeft => _jobs != null && _jobs.Count > 0
            ? _jobs.Count(x => !x.IsCompleted)
            : 0;

        #endregion

        #region CONSTRUCTORS

        public JobRunner() => _jobs = new List<RSJob>();
        public JobRunner(int jobCapacity) => _jobs = new List<RSJob>(jobCapacity);
        public JobRunner(IEnumerable<RSJob> jobs) => _jobs = new List<RSJob>(jobs);

        #endregion

        public static JobRunner Create(ScriptBlock sb, IEnumerable<object> varyingItems)
        {
            object[] objArr = varyingItems.ToArray();

            var runner = new JobRunner();
            
            for (int i = 1; i <= objArr.Length; i++)
            {
                object obj = objArr[i-1];
                runner.Jobs.Add(new RSJob(sb, obj)
                {
                    JobId = i
                });
            }
            return runner;
        }

        public void Execute()
        {
            var taskList = new List<Task>(_jobs.Count);
            for (int i = 0; i < _jobs.Count; i++)
            {
                Task task = _jobs[i].InvokeAsync();
                taskList.Add(task);
            }

            while (taskList.Count > 0)
            {
                for (int t = taskList.Count - 1; t >= 0; t--)
                {
                    Task task = taskList[t];
                    if (task.IsCompleted)
                    {
                        taskList.Remove(task);
                    }
                }
                Thread.Sleep(200);
            }

            _results = new ReadOnlyCollection<JobResult>(_jobs.Select(x => x.Result).ToList());
        }
    }
}
