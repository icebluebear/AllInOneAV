using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class TaskModel
    {
    }

    public class NextRunModel
    { 
        public DateTime NextRunTime { get; set; }
        public int NextRunCountMinutes { get; set; }
    }

    public class TaskCommonModel
    { 
        public string Message { get; set; }
    }
}
