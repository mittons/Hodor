using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.MediatorPattern;
using Contracts.ObserverPattern;

namespace Contracts
{
    public interface IViewController : ICurrentViewDataObservable, ITextEditorDisplayMediator, ITaskServiceDataSetObserver
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
        IProjectForest GetProjectForest(int projectForestId);
        ISessionProjectForest GetCurrentSessionProjectForest();


        //Update
        void UpdateTask(int taskId, object sender, string updatedTitle = null, int? updatedTaskOrdinal = null, HodorTaskStatus? updatedStatus = null);
        void AddProjectTreeToProjectForest(int projectTreeRootTaskId, int projectForestId, object sender);
        void UpdateTaskCompletionGoalForTreeInSession(int sessionProjectForestId, int projectTreeRootId,
            int updatedTaskCompletionGoal);

        //Delete
        void DeleteTaskAndSubTasks(int taskId, object sender);

    }
}
