using Model.JavModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Model.Common
{
    public class MissingCheckModel
    {
        public AV Av { get; set; }
        public bool IsMatch { get; set; }
        public List<FileInfo> Fi { get; set; }
        public List<SeedMagnetSearchModel> Seeds { get; set; }
    }
}