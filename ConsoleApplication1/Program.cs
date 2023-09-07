using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../1.txt";

            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "Hello\nYay");
                }
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
