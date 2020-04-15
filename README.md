# AllInOneAV
One place to manage AVs

Mulitple subtools to manage Janpanese AV(s).

All functions are base on configfile in setting folder, create a folder 'setting' under your c:\ drive, put all config files in it.

All AV metadata come from Javlibray.com, use BatchJavScanerAndMacthMagUrl to download whole Javlibray.com first, commond line -full is fully update(single thread, mutilple threads mode has bugs), -update for daily update.

1 AvManager is a GUI project to play local av, provided serach function. Need download Javlibrary.com first and use CombineEpisode to rename your local av file and ScanAllAndMatch to map your file with database.

2 AvPlay likes AvManager

3 CombineEpisode is a GUI project to combine muti-episode AV to one, and do other funtions like rename, remove duplicate etc,.

4 AvWeb ia a web base project can serach and play, more like a demo project.

5 FindMovie is a GUI poeject to find all 'matched' local AV, i'm using Everything instead rightnow.

6 ManagaDownloaderGUI can donwnload Comics from only 2 sources for now.

7 PlayRecnet is a GUI project to play recent AV from certain folder, most use for just downloaded AVs.

8 SisDownload can download .torrent from Sis.net
