using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void Completed(string key);

    public class FileSystemWather
    {
        private readonly FileSystemWatcher _fsWather;

        private readonly Hashtable _hstbWather;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="path">要监控的路径</param>
        /// <param name="filter">要监控的文件</param>
        /// <param name="includeSubdirectories">是否包含子文件夹</param>
        public FileSystemWather(string path, string filter, bool includeSubdirectories)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("找不到路径：" + path);
            }

            _hstbWather = new Hashtable();

            _fsWather = new FileSystemWatcher(path) { IncludeSubdirectories = includeSubdirectories, Filter = filter };
            _fsWather.Renamed += fsWather_Renamed;
            _fsWather.Changed += fsWather_Changed;
            _fsWather.Created += fsWather_Created;
            _fsWather.Deleted += fsWather_Deleted;
        }

        public event RenamedEventHandler OnRenamed;
        public event FileSystemEventHandler OnChanged;
        public event FileSystemEventHandler OnCreated;
        public event FileSystemEventHandler OnDeleted;

        /// <summary>
        ///     开始监控
        /// </summary>
        public void Start()
        {
            _fsWather.EnableRaisingEvents = true;
        }

        /// <summary>
        ///     停止监控
        /// </summary>
        public void Stop()
        {
            _fsWather.EnableRaisingEvents = false;
        }

        /// <summary>
        ///     filesystemWatcher 本身的事件通知处理过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fsWather_Renamed(object sender, RenamedEventArgs e)
        {
            lock (_hstbWather)
            {
                if (!_hstbWather.ContainsKey(e.FullPath))
                {
                    _hstbWather.Add(e.FullPath, e);
                }
            }
            var watcherProcess = new WatcherProcess(sender, e);
            watcherProcess.OnRenamed += OnRenamed; //传递事件
            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;
            var thread = new Thread(watcherProcess.Process);
            thread.Start();
        }


        private void fsWather_Created(object sender, FileSystemEventArgs e)
        {
            lock (_hstbWather)
            {
                if (!_hstbWather.ContainsKey(e.FullPath))
                {
                    _hstbWather.Add(e.FullPath, e);
                }
            }
            var watcherProcess = new WatcherProcess(sender, e);
            watcherProcess.OnCreated += OnCreated; //传递事件
            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;
            var threadDeal = new Thread(watcherProcess.Process);
            threadDeal.Start();
        }

        private void fsWather_Deleted(object sender, FileSystemEventArgs e)
        {
            lock (_hstbWather)
            {
                if (!_hstbWather.ContainsKey(e.FullPath))
                {
                    _hstbWather.Add(e.FullPath, e);
                }
            }
            var watcherProcess = new WatcherProcess(sender, e);
            watcherProcess.OnDeleted += OnDeleted; //传递事件
            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;
            var tdDeal = new Thread(watcherProcess.Process);
            tdDeal.Start();
        }

        private void fsWather_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (_hstbWather.ContainsKey(e.FullPath))
                {
                    WatcherChangeTypes oldType = ((FileSystemEventArgs)_hstbWather[e.FullPath]).ChangeType;
                    if (oldType == WatcherChangeTypes.Created || oldType == WatcherChangeTypes.Changed)
                    {
                        return;
                    }
                }
            }

            lock (_hstbWather)
            {
                if (!_hstbWather.ContainsKey(e.FullPath))
                {
                    _hstbWather.Add(e.FullPath, e);
                }
            }
            var watcherProcess = new WatcherProcess(sender, e);
            watcherProcess.OnChanged += OnChanged; //传递事件
            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;
            var thread = new Thread(watcherProcess.Process);
            thread.Start();
        }

        /// <summary>
        ///     使用了线程安全的Hashtable来处理一次改变触发两次事件的问题，
        ///     要注意的是在实际项目使用中，在通过监控文件事情触发时开一个线程WatcherProcess去处理自己业务逻辑的时候，
        ///     不管业务逻辑成功或者失败（例如有异常抛出一定要try一下）一定要让WatcherProcess的Completed
        ///     也就是MyFileSystemWather的WatcherProcess_OnCompleted执行去移除对应变化文件的Hashtable的key，
        ///     不然下次此文件改变时是无法触发你的业务逻辑的。
        /// </summary>
        /// <param name="key"></param>
        public void WatcherProcess_OnCompleted(string key)
        {
            lock (_hstbWather)
            {
                _hstbWather.Remove(key);
            }
        }
    }

    public class WatcherProcess
    {
        private readonly object _eParam;
        private readonly object _sender;

        public WatcherProcess(object sender, object eParam)
        {
            _sender = sender;
            _eParam = eParam;
        }

        public event RenamedEventHandler OnRenamed;
        public event FileSystemEventHandler OnChanged;
        public event FileSystemEventHandler OnCreated;
        public event FileSystemEventHandler OnDeleted;
        public event Completed OnCompleted;

        public void Process()
        {
            if (_eParam.GetType() == typeof(RenamedEventArgs))
            {
                OnRenamed(_sender, (RenamedEventArgs)_eParam);
                OnCompleted(((RenamedEventArgs)_eParam).FullPath);
            }
            else
            {
                var e = (FileSystemEventArgs)_eParam;
                if (e.ChangeType == WatcherChangeTypes.Created)
                {
                    OnCreated(_sender, e);
                    OnCompleted(e.FullPath);
                }
                else if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    OnChanged(_sender, e);
                    OnCompleted(e.FullPath);
                }
                else if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    OnDeleted(_sender, e);
                    OnCompleted(e.FullPath);
                }
                else
                {
                    OnCompleted(e.FullPath);
                }
            }
        }
    }
}
