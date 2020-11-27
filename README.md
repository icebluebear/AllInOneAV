# AllInOneAV
One place to manage AVs

自用AV整理，存储，播放及下载的全自动及半自动程序集，主要功能已Winform为主，为了方便开发了一些Web功能。仅适合自用，非大牛部分代码也没有经过优化，前段只有凑合能用的水平，只是做到思路启发的用途。

主要功能，

1 3种模式抓取Javlibray(启用强CloudFlare验证时，启动Chrome浏览器手动完成验证获取Cookie，弱CloudFlare验证时，用ChromeDrive半自动完成获取Cookie，没有启用CloudFlare时直接抓取，分别对应setting中database.ini的CookieMode Prcosee, Chrome, None 3种配置)
2 文件重命名 番号+名称，并支持查找重复(搜索每个盘符下面的fin文件夹中av文件，不能有子文件夹)
3 根据fin文件夹中的av文件做到匹配及搜索以及报表功能
4 合并多个av文件，例如abc-001A, abc-001B到abc-001，大部分可以自动完成，少数需要手动识别
5 根据筛选条件搜索Javlibray中的av条目并在Sukebei中搜索种子，有Winform端及Web端，Web短实现机制为根据筛选条件在数据库中插入一条任务，等Windows定时任务去数据库中抓取一条任务解析筛选条件并执行然后回写执行状态。Winform端及Web端均支持一键加入115下载（需要115客户端及刷新115Cookie的定时任务支持），Web端更好用，具体交互方式请自行查看代码。
6 其他还有一些小功能 ，比如批量转码h265，文件分享，去重等自己实验。

大概运行基础如下
1 运行Script中的脚本在本地Sqlserver数据库中建立库及表

2 导入Script的dataBak中的数据（JavLibrary中数据的快照，后续所有功能的依赖数据）

3 Setting复制到C:\根目录下

4 编译整个解决方案并按照ScheduleTask中的截图建立定时任务（具体路径按照自己环境更换）