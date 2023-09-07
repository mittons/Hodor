using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace ViewNavigation
{
    public class ViewControllerFactory
    {
        private static IViewController _ourInstance;

        public static IViewController GetViewControllerInstance()
        {
            if (_ourInstance == null)
            {
                _ourInstance = new ViewController();;
            }
            return _ourInstance;
        }
    }
}
