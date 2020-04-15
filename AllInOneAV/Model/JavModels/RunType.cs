using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JavModels
{
    [Flags]
    public enum RunType
    {
        Both = 1,
        Scan = 2,
        Download = 4,
        SecondTry = 8,
        Update = 16,
        Skip = 32
    }
}
