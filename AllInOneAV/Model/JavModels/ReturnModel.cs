using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Model.JavModels
{
    public class HtmlResponse
    {
        public bool Success { get; set; }
        public string Content { get; set; }
    }

    public class ReturnModel
    {
        public HtmlResponse Content { get; set; }
        public CookieContainer CC { get; set; }
    }

    public class NeedToUpdate
    {
        public bool Need{ get; set; }
        public HtmlResponse Content { get; set; }
    }
}