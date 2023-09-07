using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DtoContracts;

namespace HodorData.Dto
{
    public class SessionProjectForest : ISessionProjectForest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ProjectForestType Type { get; set; }
        public List<int> ProjectTreeRootIds { get; set; }
        public Dictionary<int, int> ProjectTreeTaskCompletionGoals { get; set; }

        public SessionProjectForest()
        {
            Type = ProjectForestType.SessionProjectForest;
        }
    }
}
