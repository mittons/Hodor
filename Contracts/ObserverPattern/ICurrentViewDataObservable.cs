using System.Collections;
using System.Collections.Generic;
using Contracts.DtoContracts;

namespace Contracts.ObserverPattern
{
    public interface ICurrentViewDataObservable
    {
        IViewTaskFocus GetViewTaskFocus();
        void SetFocusedTaskGivenFocusedForest(int focusedTaskId, object sender);
        void SetFocusedTaskAndForest(int focusedTaskIdInForest, int focusedProjectForestId, object sender);

        IEnumerable<IProjectForest> GetDisplayedprojectForests();

        void NotifyViewTaskFocusChanged(object sender);
        void NotifyViewDataSetChanged(object sender);

        void AddCurrentViewDataObserver(ICurrentViewDataObserver observer);
        void RemoveViewDisplayDataObserver(ICurrentViewDataObserver observer);

        //MAKE SURE WE ALWAYS CALL FinishBatchDataSetUpdate AFTER THIS
        void StartBatchDataSetUpdate();
        void FinishBatchDataSetUpdate(object sender);
    }
}
