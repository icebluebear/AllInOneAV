USE [master]
GO
/****** Object:  Database [ScanAllAv]    Script Date: 11/27/2020 5:29:14 PM ******/
CREATE DATABASE [ScanAllAv]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ScanAllAv', FILENAME = N'G:\DB\ScanAllAv.mdf' , SIZE = 10485760KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ScanAllAv_log', FILENAME = N'G:\DB\ScanAllAv_log.ldf' , SIZE = 3480448KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ScanAllAv] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ScanAllAv].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ScanAllAv] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ScanAllAv] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ScanAllAv] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ScanAllAv] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ScanAllAv] SET ARITHABORT OFF 
GO
ALTER DATABASE [ScanAllAv] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ScanAllAv] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ScanAllAv] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ScanAllAv] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ScanAllAv] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ScanAllAv] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ScanAllAv] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ScanAllAv] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ScanAllAv] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ScanAllAv] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ScanAllAv] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ScanAllAv] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ScanAllAv] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ScanAllAv] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ScanAllAv] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ScanAllAv] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ScanAllAv] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ScanAllAv] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ScanAllAv] SET  MULTI_USER 
GO
ALTER DATABASE [ScanAllAv] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ScanAllAv] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ScanAllAv] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ScanAllAv] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ScanAllAv] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ScanAllAv] SET QUERY_STORE = OFF
GO
USE [ScanAllAv]
GO
/****** Object:  User [readonly]    Script Date: 11/27/2020 5:29:14 PM ******/
CREATE USER [readonly] FOR LOGIN [readonly] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [readonly]
GO
/****** Object:  Table [dbo].[FaviScan]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FaviScan](
	[FaviScanId] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FaviScan] PRIMARY KEY CLUSTERED 
(
	[FaviScanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MagUrl]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MagUrl](
	[MagUrlId] [int] IDENTITY(1,1) NOT NULL,
	[AvId] [nvarchar](50) NOT NULL,
	[MagUrl] [text] NULL,
	[IsFound] [bit] NOT NULL,
	[MagTitle] [nvarchar](500) NOT NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_MagUrl] PRIMARY KEY CLUSTERED 
(
	[MagUrlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Match]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Match](
	[MatchID] [int] IDENTITY(1,1) NOT NULL,
	[AvID] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Location] [nvarchar](1000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[AvName] [nvarchar](500) NOT NULL,
	[MatchAVId] [int] NOT NULL,
 CONSTRAINT [PK_Match] PRIMARY KEY CLUSTERED 
(
	[MatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MatchMap]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchMap](
	[MatchMapId] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](50) NOT NULL,
	[FilePath] [nchar](4000) NOT NULL,
	[AVID] [int] NOT NULL,
 CONSTRAINT [PK_MatchMap] PRIMARY KEY CLUSTERED 
(
	[MatchMapId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OneOneFiveCookie]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OneOneFiveCookie](
	[OneOneFiveCookieId] [int] IDENTITY(1,1) NOT NULL,
	[OneOneFiveCookie] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OneOneFiveCookie] PRIMARY KEY CLUSTERED 
(
	[OneOneFiveCookieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prefix]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prefix](
	[Prefix] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteScanMag]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteScanMag](
	[RemoteScanMagId] [int] IDENTITY(1,1) NOT NULL,
	[AvId] [nvarchar](1000) NOT NULL,
	[AvUrl] [nvarchar](1000) NOT NULL,
	[AvName] [nvarchar](3000) NOT NULL,
	[MagTitle] [nvarchar](3000) NOT NULL,
	[MagUrl] [nvarchar](3000) NOT NULL,
	[MagSize] [numeric](18, 0) NOT NULL,
	[MagDate] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[SearchStatus] [int] NULL,
	[MatchFile] [nvarchar](500) NULL,
	[ScanJobId] [int] NOT NULL,
 CONSTRAINT [PK_RemoteScanMag] PRIMARY KEY CLUSTERED 
(
	[RemoteScanMagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[ReportDate] [datetime] NOT NULL,
	[TotalCount] [int] NOT NULL,
	[TotalExist] [int] NOT NULL,
	[TotalExistSize] [decimal](18, 0) NOT NULL,
	[LessThenOneGiga] [int] NOT NULL,
	[OneGigaToTwo] [int] NOT NULL,
	[TwoGigaToFour] [int] NOT NULL,
	[FourGigaToSix] [int] NOT NULL,
	[GreaterThenSixGiga] [int] NOT NULL,
	[Extension] [nvarchar](max) NOT NULL,
	[H265Count] [int] NOT NULL,
	[ChineseCount] [int] NOT NULL,
	[IsFinish] [int] NOT NULL,
	[EndDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportItem]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportItem](
	[ReportItemId] [int] IDENTITY(1,1) NOT NULL,
	[ReportType] [int] NOT NULL,
	[ItemName] [nvarchar](70) NOT NULL,
	[ExistCount] [int] NOT NULL,
	[TotalCount] [int] NOT NULL,
	[TotalSize] [decimal](18, 0) NOT NULL,
	[ReportId] [int] NOT NULL,
 CONSTRAINT [PK_ReportItem] PRIMARY KEY CLUSTERED 
(
	[ReportItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScanJob]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScanJob](
	[ScanJobId] [int] IDENTITY(1,1) NOT NULL,
	[ScanJobName] [nvarchar](1000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[IsFinish] [int] NOT NULL,
	[ScanParameter] [nvarchar](max) NOT NULL,
	[totalitem] [int] NOT NULL,
 CONSTRAINT [PK_ScanJob] PRIMARY KEY CLUSTERED 
(
	[ScanJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchHistory]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchHistory](
	[SearchHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_SearchHistory] PRIMARY KEY CLUSTERED 
(
	[SearchHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[UserInfoId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](500) NOT NULL,
	[UserPassword] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[UserInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ViewHistory]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ViewHistory](
	[FileName] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebViewLog]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebViewLog](
	[WebViewLogId] [int] IDENTITY(1,1) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[Controller] [nvarchar](500) NOT NULL,
	[Parameter] [nvarchar](max) NOT NULL,
	[UserAgent] [nvarchar](max) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsLogin] [int] NOT NULL,
	[Action] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_WebViewLog] PRIMARY KEY CLUSTERED 
(
	[WebViewLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WishList]    Script Date: 11/27/2020 5:29:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WishList](
	[WishListId] [int] IDENTITY(1,1) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[Id] [int] NOT NULL,
	[AvId] [nvarchar](500) NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_WishList] PRIMARY KEY CLUSTERED 
(
	[WishListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20201121-120527]    Script Date: 11/27/2020 5:29:14 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20201121-120527] ON [dbo].[ReportItem]
(
	[ItemName] ASC,
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Match] ADD  DEFAULT ((0)) FOR [MatchAVId]
GO
ALTER TABLE [dbo].[RemoteScanMag] ADD  DEFAULT ((0)) FOR [ScanJobId]
GO
ALTER TABLE [dbo].[ReportItem] ADD  CONSTRAINT [DF__ReportIte__Repor__42ACE4D4]  DEFAULT ((0)) FOR [ReportId]
GO
ALTER TABLE [dbo].[ScanJob] ADD  DEFAULT ('') FOR [ScanParameter]
GO
ALTER TABLE [dbo].[ScanJob] ADD  DEFAULT ((0)) FOR [totalitem]
GO
USE [master]
GO
ALTER DATABASE [ScanAllAv] SET  READ_WRITE 
GO
