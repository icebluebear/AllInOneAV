using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AVWeb.Models
{
    public class VMResult
    {
        public string Msg { get; set; }
        public VMCode MsgCode { get; set; }
    }

    public enum VMCode
    { 
        Success = 1,
        Error = 2,
        Exception = 3
    }
}