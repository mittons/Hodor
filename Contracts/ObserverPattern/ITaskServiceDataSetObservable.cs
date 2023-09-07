using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ObserverPattern
{
    public interface ITaskServiceDataSetObservable
    {
        void NotifyTaskServiceDataSetChanged(object sender);
        void AddTaskServiceDataSetChangedObserver(ITaskServiceDataSetObserver observer);
        void RemoveTaskServiceDataSetChangedObserver(ITaskServiceDataSetObserver observer);

        //MAKE SURE WE ALWAYS CALL FinishBatchDataSetUpdate AFTER THIS
        void StartBatchDataSetUpdate();
        void FinishBatchDataSetUpdate(object sender);
    }
}
