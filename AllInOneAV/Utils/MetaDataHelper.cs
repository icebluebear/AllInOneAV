using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class MetaDataHelper
    {
        private static void ReadVideoData(string fileName)
        {
            string file = "d:\\test.mp4";
            var so = ShellObject.FromParsingName(file);

            var title = so.Properties.DefaultPropertyCollection;

            var dir = so.Properties.GetProperty(SystemProperties.System.Video.Director);
            var writer = so.Properties.GetPropertyWriter();
            writer.WriteProperty(SystemProperties.System.Video.Director, "Author"
                , true);
            writer.Close();
        }
    }
}
