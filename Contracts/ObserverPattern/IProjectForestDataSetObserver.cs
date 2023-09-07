using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ObserverPattern
{
    public interface IProjectForestDataSetObserver
    {
        void OnProjectForestDataSetChanged(object sender);
    }
}
