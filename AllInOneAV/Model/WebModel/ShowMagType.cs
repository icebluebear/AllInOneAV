using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WebModel
{
    [Flags]
    public enum ShowMagType
    {
        All = 1,
        OnlyExist = 2,
        OnlyNotExist = 4,
        GreaterThenExist = 8,
        HasMagSize = 16,
        GreaterThenNotExist = 32,
    }

    public class ShowMagKey
    { 
        public string Key { get; set; }
        public ShowMagType Type { get; set; }
    }
}
