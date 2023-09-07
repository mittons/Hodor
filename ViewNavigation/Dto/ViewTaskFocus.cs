using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DtoContracts;

namespace ViewNavigation.Dto
{
    public class ViewTaskFocus : IViewTaskFocus
    {
        public int FocusedTaskId { get; set; }
        public int FocusedForestId { get; set; }
    }
}
