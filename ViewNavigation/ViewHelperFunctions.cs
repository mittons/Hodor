using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts.Dto;

namespace ViewNavigation
{
    public class ViewHelperFunctions
    {
        public static List<HodorTask> GetImpendingTaskTreeTaskList(int projectTreeRootId, IViewController vc)
        {
            var impTaskTree = GetImpendingTaskTree(projectTreeRootId, vc);
            return impTaskTree != null ? ((ImpendingTaskTreeStruct)impTaskTree).ToList() : new List<HodorTask>();
        }
        
        public static ImpendingTaskTreeStruct? GetImpendingTaskTree(int projectTreeRootId, IViewController vc)
        {
            return CreateImpendingTaskTreeRecursive(vc.GetTask(projectTreeRootId), vc);
        }

        private static ImpendingTaskTreeStruct? CreateImpendingTaskTreeRecursive(HodorTask task, IViewController vc)
        {
            var children = vc.GetChildren(task.Id);
            if (children.Count == 0)
            {
                if (task.CurrentTaskStatus != HodorTaskStatus.Impending) return null;
                
                return new ImpendingTaskTreeStruct
                {
                    ImpendingTaskTreeTask = task,
                    ImpendingTaskTreeTaskChildren = new List<ImpendingTaskTreeStruct>()
                };
            }

            var childReturns =
                children.Select(childTask => CreateImpendingTaskTreeRecursive(childTask, vc))
                    .OfType<ImpendingTaskTreeStruct>().ToList();

            if (childReturns.Count == 0) return null;

            return new ImpendingTaskTreeStruct
            {
                ImpendingTaskTreeTask = task,
                ImpendingTaskTreeTaskChildren = childReturns
            };
        }
    }

    public struct ImpendingTaskTreeStruct
    {
        public HodorTask ImpendingTaskTreeTask { get; set; }
        public List<ImpendingTaskTreeStruct> ImpendingTaskTreeTaskChildren { get; set; }

        public List<HodorTask> ToList()
        {
            var impendingTaskTreeTaskList = new List<HodorTask> {ImpendingTaskTreeTask};
            foreach (var ch in ImpendingTaskTreeTaskChildren)
            {
                impendingTaskTreeTaskList.AddRange(ch.ToList());
            }
            return impendingTaskTreeTaskList;
        }
    }
}
