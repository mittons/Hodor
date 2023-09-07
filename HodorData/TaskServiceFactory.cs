using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace HodorData
{
    public class TaskServiceFactory
    {
        private static IHodorTaskService _ourInstance;

        public static IHodorTaskService GetTaskServiceInstance()
        {
            if (_ourInstance == null)
            {
                _ourInstance = new HodorTaskServiceStub();
            }
            return _ourInstance;
        }
    }
}
