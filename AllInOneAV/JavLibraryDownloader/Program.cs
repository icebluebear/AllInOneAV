using Model.JavModels;

namespace JavLibraryDownloader
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Process.Process.Start(RunType.Both);
            }

            if (args.Length == 1 && args[0] == "scan")
            {
                Process.Process.Start(RunType.Scan);
            }

            if (args.Length == 1 && args[0] == "download")
            {
                Process.Process.Start(RunType.Download);
            }

            if (args.Length == 1 && args[0] == "update")
            {
                Process.Process.Start(RunType.Update);
            }

            if (args.Length == 2 && args[0] == "skip")
            {
                Process.Process.Start(RunType.Skip, args[1]);
            }
        }
    }
}
