using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DtoContracts
{
    public interface ISessionProjectForest : IProjectForest
    {
        Dictionary<int, int> ProjectTreeTaskCompletionGoals { get; set; }
    }
}
