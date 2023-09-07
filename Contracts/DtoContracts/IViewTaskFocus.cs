using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DtoContracts
{
    public interface IViewTaskFocus
    {
        int FocusedTaskId { get; set; }
        int FocusedForestId { get; set; }
    }
}
