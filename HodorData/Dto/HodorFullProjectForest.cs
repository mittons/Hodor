using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts.DtoContracts;
using Contracts.ObserverPattern;

namespace HodorData.Dto
{
    public class HodorFullProjectForest : IHodorProjectForest
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public HodorProjectForestType Type { get; private set; }
        public List<int> ProjectTreeRootIds { get; private set; }

        private IHodorTaskService _taskService;
        private List<IProjectForestDataSetObserver> _projectForestDataSetObservers = new List<IProjectForestDataSetObserver>(); 


        public HodorFullProjectForest(int id, IHodorTaskService hodorTaskService)
        {
            Id = id;
            Title = "All Project Trees";
            Type = HodorProjectForestType.FullProjectForest;

            _taskService = hodorTaskService;
            ProjectTreeRootIds = _taskService.GetAllProjectTreeRootIds();
        }

        #region IProjectForestDataSetObservable
        public void NotfiyProjectForestDataSetChanged(object sender)
        {
            foreach (var projectForestDataSetObserver in _projectForestDataSetObservers)
            {
                projectForestDataSetObserver.OnProjectForestDataSetChanged(sender);
            }
        }

        public void AddProjectForestDataSetObserver(IProjectForestDataSetObserver observer)
        {
            if (!_projectForestDataSetObservers.Contains(observer))
            {
                _projectForestDataSetObservers.Add(observer);
            }
        }

        public void RemoveProjectForestDataSetObserver(IProjectForestDataSetObserver observer)
        {
            if (_projectForestDataSetObservers.Contains(observer))
            {
                _projectForestDataSetObservers.Remove(observer);
            }
        }
        #endregion

        #region ITaskServiceDataSetObservable

        public void OnTaskServiceDataSetChanged(object sender)
        {
            ProjectTreeRootIds = _taskService.GetAllProjectTreeRootIds();
            NotfiyProjectForestDataSetChanged(sender);
        }

        #endregion
    }
}
