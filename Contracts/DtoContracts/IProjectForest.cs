using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Contracts.ObserverPattern;

namespace Contracts.DtoContracts
{
    public interface IProjectForest// : IProjectForestDataSetObservable, ITaskServiceDataSetObserver
    {
        int Id { get; }
        string Title { get; }
        ProjectForestType Type { get; }
        List<int> ProjectTreeRootIds { get; }
    }

    public enum ProjectForestType
    {
        FullProjectForest,
        SessionProjectForest
    }
}
