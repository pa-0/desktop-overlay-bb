using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oos
{
    public class ApplicationClass
    {
        public string Title = "";
        public string ExecPath = "";
        public string ProcessName = "";

        public ApplicationClass(string _title, string _execPath, string _processName = "")
        {
            Title = _title;
            ExecPath = _execPath;
            //if _processName == empty, dan de processname van de _execPath parsen om de process naam te krijgen
            ProcessName = _processName;
        }
    }
}
