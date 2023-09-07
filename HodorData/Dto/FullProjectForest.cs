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
    public class FullProjectForest : IProjectForest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ProjectForestType Type { get; set; }
        public List<int> ProjectTreeRootIds { get; set; }

//        private IHodorTaskService _taskService;


        public FullProjectForest()//, IHodorTaskService hodorTaskService)
        {
            //Id = id;
            //Title = "All Project Trees";
            Type = ProjectForestType.FullProjectForest;

//            _taskService = hodorTaskService;
//            ProjectTreeRootIds = _taskService.GetAllProjectTreeRootIds();
        }

//        #region IProjectForestDataSetObservable
//        private List<IProjectForestDataSetObserver> _projectForestDataSetObservers = new List<IProjectForestDataSetObserver>(); 
//        public void NotfiyProjectForestDataSetChanged(object sender)
//        {
//            foreach (var projectForestDataSetObserver in _projectForestDataSetObservers)
//            {
//                projectForestDataSetObserver.OnProjectForestDataSetChanged(sender);
//            }
//        }
//
//        public void AddProjectForestDataSetObserver(IProjectForestDataSetObserver observer)
//        {
//            if (!_projectForestDataSetObservers.Contains(observer))
//            {
//                _projectForestDataSetObservers.Add(observer);
//            }
//        }
//
//        public void RemoveProjectForestDataSetObserver(IProjectForestDataSetObserver observer)
//        {
//            if (_projectForestDataSetObservers.Contains(observer))
//            {
//                _projectForestDataSetObservers.Remove(observer);
//            }
//        }
//        #endregion
//
//        #region ITaskServiceDataSetObservable
//
//        public void OnTaskServiceDataSetChanged(object sender)
//        {
//            ProjectTreeRootIds = _taskService.GetAllProjectTreeRootIds();
//            NotfiyProjectForestDataSetChanged(sender);
//        }
//
//        #endregion
    }
}
