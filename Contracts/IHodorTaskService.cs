using System.Collections.Generic;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.ObserverPattern;

namespace Contracts
{
    public interface IHodorTaskService : ITaskServiceDataSetObservable
    {
        //Create
        HodorTask CreateTask(string title, int parentId, HodorTaskStatus status, object sender);
        HodorTask CreateTask(string title, int parentId, HodorTaskStatus status, int ordinal, object sender);

        //Returns the root task of the new project tree
        HodorTask CreateProjectTree(string rootTaskTitle, HodorTaskStatus rootTaskStatus, object sender);

        //Returns the root task of the new project tree
        HodorTask CreateProjectTree(string rootTaskTitle, HodorTaskStatus rootTaskStatus, int projectTreeOrdinal, object sender);

        //Read
        HodorTask GetTask(int taskId);
        List<HodorTask> GetChildren(int taskId);
        
        //todo remove
        List<int> GetAllProjectTreeRootIds();
        IProjectForest GetProjectForest(int projectForestId);
        List<IProjectForest> GetCurrentlyDisplayedProjectForests(); 

        //Update
        bool UpdateTask(int taskId, string updatedTitle, HodorTaskStatus? updatedStatus, int? updatedTaskOrdinal, object sender);
        void AddProjectTreeToProjectForest(int projectTreeRootTaskId, int projectForestId, object sender);
        void UpdateTaskCompletionGoalForTreeInSession(int sessionProjectForestId, int projectTreeRootId,
            int updatedTaskCompletionGoal);

        //Delete
        void DeleteTaskAndSubTasks(int taskId, object sender);
    }
}
