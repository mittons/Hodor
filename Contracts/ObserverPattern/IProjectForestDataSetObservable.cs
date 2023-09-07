using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ObserverPattern
{
    public interface IProjectForestDataSetObservable
    {
        void NotfiyProjectForestDataSetChanged(object sender);
        void AddProjectForestDataSetObserver(IProjectForestDataSetObserver observer);
        void RemoveProjectForestDataSetObserver(IProjectForestDataSetObserver observer);
    }
}
