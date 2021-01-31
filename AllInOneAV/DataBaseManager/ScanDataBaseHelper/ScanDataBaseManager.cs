using DataBaseManager.Common;
using Model.FindModels;
using Model.JavModels;
using Model.ScanModels;
using Model.WebModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utils;

namespace DataBaseManager.ScanDataBaseHelper
{
    public class ScanDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static ScanDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};User=sa;password=19880118Qs123!", JavINIClass.IniReadValue("Scan", "server"), JavINIClass.IniReadValue("Scan", "db"));
            mycon = new SqlConnection(con);
        }

        public static int ClearMatch()
        {
            var sql = @"TRUNCATE TABLE Match";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteMatch(string location, string name)
        {
            name = FileUtility.ReplaceInvalidChar(name);
            location = FileUtility.ReplaceInvalidChar(location);

            var sql = @"DELETE FROM Match WHERE Location = '" + location + "' and Name = '" + name + "'";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteMatch(int matchid)
        {
            var sql = @"DELETE FROM Match WHERE matchid = " + matchid;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<Match> GetAllMatch()
        {
            var sql = @"SELECT * FROM Match";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>();
        }

        public static List<Match> GetAllMatch(string id)
        {
            var sql = @"SELECT * FROM Match WHERE AvId = '" + id + "'";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>();
        }

        public static bool HasMatch(string id)
        {
            var sql = string.Format("SELECT * FROM Match WHERE AvID = '{0}'", id);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>().Count > 0 ? true : false;
        }

        public static int SaveMatch(Match match)
        {
            var sql = string.Format(@"INSERT INTO Match (AvID, MatchAVId, Name, Location, CreateTime, AvName) VALUES ('{0}', {4}, N'{1}', N'{2}', GETDATE(), N'{3}')", match.AvID, FileUtility.ReplaceInvalidChar(match.Name), match.Location, FileUtility.ReplaceInvalidChar(match.AvName), match.MatchAVId);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteFinish()
        {
            var sql = @"DELETE FROM Finish";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertFinish()
        {
            var sql = @"INSERT INTO Finish (IsFinish) VALUES (1)";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static bool IsFinish()
        {
            var sql = @"SELECT * FROM Finish";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<Finish>().IsFinish == 0 ? false : true;
        }

        public static int InsertViewHistory(string file)
        {
            var sql = string.Format("INSERT INTO ViewHistory (FileName) VALUES ('{0}')", FileUtility.ReplaceInvalidChar(file));

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int RemoveViewHistory(string file)
        {
            var sql = string.Format("DELETE FROM ViewHistory WHERE FileName = '{0}'", FileUtility.ReplaceInvalidChar(file));

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertSearchHistory(string content)
        {
            var sql = string.Format("IF NOT EXISTS (SELECT 1 FROM SearchHistory WHERE Content = '{0}') INSERT INTO SearchHistory(Content) VALUES('{0}')", FileUtility.ReplaceInvalidChar(content));

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<SearchHistory> GetSearchHistory()
        {
            var sql = "SELECT Content From SearchHistory";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<SearchHistory>();
        }

        public static bool ViewedFile(string file)
        {
            var sql = string.Format("SELECT * FROM ViewHistory WHERE FileName = '{0}'", file);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ViewHistory>() == null ? false : true;
        }

        public static int InsertPrefix(string pre)
        {
            var sql = "INSERT INTO Prefix (Prefix) VALUES ('" + pre + "')";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<PrefixModel> GetPrefix()
        {
            var sql = "SELECT * FROM Prefix";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<PrefixModel>();
        }

        public static int DeleteMatchMap()
        {
            var sql = "TRUNCATE TABLE MatchMap";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertMatchMap(string file, string id, int avid)
        {
            var sql = string.Format("INSERT INTO MatchMap (FilePath, ID, AVID) VALUES ('{0}', '{1}', {2})", file, id, avid);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static MatchMap GetMatchMapByAvId(int avId)
        {
            var sql = "SELECT m.FilePath, a.* FROM [ScanAllAv].[dbo].MatchMap m JOIN [JavLibraryDownload].[dbo].AV a ON m.AVID = a.AVID WHERE a.AvId = " + avId;

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<MatchMap>();
        }

        public static List<MatchMap> GetMatchMap(string orderStr, string where, string pageStr)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT m.FilePath, a.*, ROW_NUMBER() OVER (ORDER BY {0}) AS OnePage FROM [ScanAllAv].[dbo].MatchMap m JOIN [JavLibraryDownload].[dbo].AV a ON m.AVID = a.AVID WHERE {1}) AS t WHERE 1 = 1 {2}", orderStr, where, pageStr);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<MatchMap>();
        }

        public static List<MatchMap> GetMatchMapByFile(string file)
        {
            var sql = string.Format(@"SELECT * FROM MatchMap WHERE FilePath = '{0}'", file);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<MatchMap>();
        }

        public static List<MagUrlModel> GetAllMagUrl()
        {
            var sql = "SELECT * FROM MagUrl";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<MagUrlModel>();
        }

        public static List<MagUrlModel> GetAllMagUrlById(string id)
        {
            var sql = "SELECT * FROM MagUrl WHERE Avid = '" + id + "' AND IsFound = 1";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<MagUrlModel>();
        }

        public static int DeleteMagUrlById(string avid)
        {
            var sql = "DELETE FROM MagUrl WHERE AvId = '" + avid + "'";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertMagUrl(string avid, string magUrl, string magTitle, int isFound)
        {
            var sql = "INSERT INTO MagUrl (AvId, MagUrl, MagTitle, IsFound, CreateTime) VALUES ('" + avid + "', '" + magUrl + "', '" + magTitle + "', " + isFound + ", GETDATE())";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<ScanResult> GetMatchScanResult()
        {
            var sql = @"SELECT m.MatchId AS Id, CASE WHEN m.Location IS NULL THEN a.AVID ELSE m.MatchAVId END AS MatchAvId, m.Location, m.Name AS FileName, a.PictureURL AS PicUrl, a.ID AS AvId, a.Company, a.Name AS AvName, a.Director, a.Publisher, a.Category, a.Actress, a.ReleaseDate FROM ScanAllAv.dbo.Match m RIGHT JOIN JavLibraryDownload.dbo.AV a ON m.AvID = a.ID";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<ScanResult>();
        }

        public static List<ScanResult> GetAllAvFromJav()
        {
            var sql = @"SELECT AvId AS MatchAVId, '' AS Location, '' AS FileName, PictureURL AS PicUrl, ID AS AvId, Company, Name AS AvName, Director, Publisher, Category, Actress, ReleaseDate FROM JavLibraryDownload.dbo.AV";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<ScanResult>();
        }

        public static ScanResult GetMatchScanResult(int avId)
        {
            var sql = @"SELECT TOP 1 m.MatchId AS Id, m.Location, m.Name AS FileName, a.PictureURL AS PicUrl, a.ID AS AvId, a.Company, a.Name AS AvName, a.Director, a.Publisher, a.Category, a.Actress, a.ReleaseDate FROM ScanAllAv.dbo.Match m LEFT JOIN JavLibraryDownload.dbo.AV a ON m.AvID = a.ID WHERE m.MatchAvID=" + avId;

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ScanResult>();
        }

        public static int DeleteFaviScan()
        {
            var sql = "TRUNCATE TABLE FaviScan";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertFaviScan(FaviScan favi)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM FaviScan WHERE [Url] = '" + favi.Url + @"')
                            INSERT INTO FaviScan (Category, Url, Name) VALUES ('" + favi.Category + "', '" + favi.Url + "', '" + favi.Name + "')";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<FaviScan> GetFaviScan()
        {
            var sql = "SELECT * FROM FaviScan ORDER BY Category";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<FaviScan>();
        }

        public static AV GetMatchedAv(int id)
        {
            var sql = "SELECT TOP 1 av.* FROM JavLibraryDownload.dbo.AV av JOIN ScanAllAv.dbo.[Match] m ON av.AVID = m.MatchAVId WHERE m.MatchID = " + id;

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<AV>();
        }

        public static int InsertRemoteScanMag(RemoteScanMag entity)
        {
            var sql = string.Format("INSERT INTO RemoteScanMag (AvId, AvUrl, AvName, MagTitle, MagUrl, MagSize, SearchStatus, MatchFile, CreateTime, MagDate, ScanJobId) VALUES ('{0}', '{1}', N'{2}', '{3}', '{4}', {5}, {6}, N'{7}', GETDATE(), GETDATE(), {8})", entity.AvId, entity.AvUrl, entity.AvName, entity.MagTitle, entity.MagUrl, entity.MagSize, entity.SearchStatus, entity.MatchFile, entity.JobId);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertScanJob(string scanJobName, string scanParameter)
        {
            var sql = string.Format("INSERT INTO ScanJob (ScanJobName, ScanParameter, CreateTime, EndTime, IsFinish) VALUES ('{0}', '{1}', GETDATE(), GETDATE(), 0) SELECT @@IDENTITY", scanJobName, scanParameter);

            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
        }

        public static int DeleteRemoteScanMag()
        {
            var sql = "DELETE FROM RemoteScanMag WHERE ScanJobId = 0";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static ScanJob GetFirstScanJob()
        {
            var sql = "SELECT TOP 1 * FROM ScanJob WHERE IsFinish = 0 ORDER BY CreateTime ASC";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ScanJob>();
        }

        public static ScanJob GetFirstScanJobTest()
        {
            var sql = "SELECT TOP 1 * FROM ScanJob ORDER BY CreateTime ASC";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ScanJob>();
        }

        public static List<ScanJob> GetScanJob(int count)
        {
            var sql = "SELECT TOP " + count + " * FROM ScanJob ORDER BY EndTime DESC";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<ScanJob>();
        }

        public static int GetScanJobItem(int scanJobId)
        {
            var sql = "SELECT COUNT(DISTINCT(AvId)) FROM RemoteScanMag WHERE ScanJobID = " + scanJobId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
        }

        public static int DeleteScanJob(int jobId)
        {
            var sql = "DELETE FROM ScanJob WHERE ScanJobId=" + jobId;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteRemoteMagScan(int jobId)
        {
            var sql = "DELETE FROM RemoteScanMag WHERE ScanJobId=" + jobId;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int SetScanJobFinish(int scanJobId, int status, int totalItem = 0)
        {
            var sql = "UPDATE ScanJob SET IsFinish = " + status + ", TotalItem = " + totalItem + ", EndTime = GETDATE() WHERE ScanJobId = " + scanJobId;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<RemoteScanMag> GetAllMag()
        {
            var sql = "SELECT * FROM RemoteScanMag";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<RemoteScanMag>();
        }

        public static List<RemoteScanMag> GetAllMagByJob(int jobId)
        {
            var sql = "SELECT * FROM RemoteScanMag WHERE ScanJobId="+jobId;

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<RemoteScanMag>();
        }

        public static int InsertOneOneFiveCookie(OneOneFiveCookieModel entity)
        {
            var sql = string.Format("INSERT INTO OneOneFiveCookie (OneOneFiveCookie) VALUES ('{0}')", entity.OneOneFiveCookie);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static OneOneFiveCookieModel GetOneOneFiveCookie()
        {
            var sql = "SELECT TOP 1 * FROM OneOneFiveCookie ORDER BY OneOneFiveCookieId DESC";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<OneOneFiveCookieModel>();
        }

        public static bool IsUser(string name, string pass)
        {
            var sql = string.Format("SELECT * FROM UserInfo WHERE UserName = '{0}' AND UserPassword = '{1}'", name, pass);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<UserInfo>() == null ? false : true;
        }

        public static int DeleteReportItem()
        {
            var sql = "TRUNCATE TABLE ReportItem";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertOrUpdateReportItem(ReportType reportType, string itemName, int exist, double existSize, int reportId)
        {
            var sql = string.Format(@"IF EXISTS (SELECT 1 FROM ReportItem WHERE ReportType = {0} AND ItemName = '{1}' AND ReportId = {4})
                                            UPDATE ReportItem SET ExistCount = ExistCount + {2}, TotalCount = TotalCount + 1, TotalSize = TotalSize + {3} WHERE ReportType = {0} AND ItemName = '{1}' AND ReportId = {4}
                                        ELSE
                                            INSERT INTO ReportItem (ReportType, ItemName, ExistCount, TotalCount, TotalSize, ReportId) VALUES ({0}, '{1}', {2}, 1, {3}, {4})", (int)reportType, itemName, exist, existSize, reportId);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int BatchInserReportItem(List<ReportItem> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ReportItemId", typeof(int));
            dt.Columns.Add("ReportType", typeof(int));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ExistCount", typeof(int));
            dt.Columns.Add("TotalCount", typeof(int));
            dt.Columns.Add("TotalSize", typeof(double));
            dt.Columns.Add("ReportId", typeof(int));

            foreach(var item in items)
            {
                dt.Rows.Add(null, (int)item.ReportType, item.ItemName, item.ExistCount, item.TotalCount, item.TotalSize, item.ReportId);
            }

            using (SqlConnection conn = new SqlConnection(con))
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
                bulkCopy.DestinationTableName = "ReportItem";
                bulkCopy.BatchSize = dt.Rows.Count;
                conn.Open();
                if (dt != null && dt.Rows.Count != 0)
                {
                    bulkCopy.WriteToServer(dt);
                }
            }

            return items.Count;
        }

        public static int InsertReportItem(ReportType reportType, string itemName, int exist, double existSize, int reportId)
        {
            var sql = string.Format(@"INSERT INTO ReportItem (ReportType, ItemName, ExistCount, TotalCount, TotalSize, ReportId) VALUES ({0}, '{1}', {2}, 1, {3}, {4})", (int)reportType, itemName, exist, existSize, reportId);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertReport(Report entity)
        {
            var sql = string.Format(@"INSERT INTO Report (ReportDate,TotalCount,TotalExist,TotalExistSize,LessThenOneGiga,OneGigaToTwo,TwoGigaToFour,FourGigaToSix,GreaterThenSixGiga,Extension,H265Count,ChineseCount,IsFinish,EndDate) VALUES (GETDATE(), {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}', {9}, {10}, {11}, GETDATE()) SELECT @@IDENTITY", entity.TotalCount, entity.TotalExist, entity.TotalExistSize, entity.LessThenOneGiga, entity.OneGigaToTwo, entity.TwoGigaToFour, entity.FourGigaToSix, entity.GreaterThenSixGiga, entity.Extension, entity.H265Count, entity.ChineseCount, 0);

            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
        }

        public static int UpdateReport(Report entity)
        {
            var sql = string.Format("UPDATE Report SET TotalExist = {0}, TotalExistSize = {1}, LessThenOneGiga = {2}, OneGigaToTwo = {3}, TwoGigaToFour = {4}, FourGigaToSix = {5}, GreaterThenSixGiga = {6}, Extension = '{7}', H265Count = {8}, ChineseCount = {9} WHERE ReportId = " + entity.ReportId, entity.TotalExist, entity.TotalExistSize, entity.LessThenOneGiga, entity.OneGigaToTwo, entity.TwoGigaToFour, entity.FourGigaToSix, entity.GreaterThenSixGiga, JsonConvert.SerializeObject(entity.ExtensionModel), entity.H265Count, entity.ChineseCount);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int UpdateReportFinish(int id)
        {
            var sql = "UPDATE Report SET IsFinish = 1, EndDate = GETDATE() WHERE ReportID = " + id;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static Report GetReport()
        {
            var sql = "SELECT TOP 1 * FROM Report WHERE IsFinish = 1 ORDER BY EndDate DESC";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<Report>();
        }

        public static List<ReportItem> ReportItem(int reportId)
        {
            var sql = "SELECT * FROM ReportItem WHERE ReportId=" + reportId;

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<ReportItem>();
        }

        public static int InsertWishList(WishList entity)
        {
            var sql = string.Format(@"IF NOT EXISTS (SELECT * FROM WishList WHERE IPAddress = '{0}' AND FilePath = '{3}')
                                        INSERT INTO WishList (IPAddress, Id, AvId, FilePath) VALUES ('{0}', {1}, '{2}', '{3}')", entity.IPAddress, entity.Id, entity.AvId, entity.FilePath);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InserWebViewLog(WebViewLog entity)
        {
            var sql = string.Format(@"INSERT INTO WebViewLog (IPAddress, Controller, [Action], Parameter, UserAgent, IsLogin, CreateTime) VALUES ('{0}', '{1}', '{5}', '{2}', '{3}', {4}, GETDATE())", entity.IPAddress, entity.Controller, entity.Parameter, entity.UserAgent, entity.IsLogin, entity.Action);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<WishList> GetWishList(string where)
        {
            var sql = string.Format("SELECT * FROM WishList WHERE 1=1");

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<WishList>();
        }

        public static List<WebViewLog> GetWebViewLog(string where)
        {
            var sql = string.Format("SELECT * FROM WebViewLog WHERE 1=1");

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<WebViewLog>();
        }

        public static AvAndShaMapping GetPossibleMaping(string filePath, double fileSize)
        {
            var sql = string.Format(@"SELECT TOP 1 * FROM AvAndShaMapping WHERE FilePath = N'{0}' AND FileSize = {1}", filePath, fileSize);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<AvAndShaMapping>();
        }

        public static bool IsExistShaMapping(string filePath, double fileSize, string sha1)
        {
            var sql = string.Format(@"SELECT TOP 1 * FROM AvAndShaMapping WHERE FilePath = N'{0}' AND FileSize = {1} AND Sha1 = N'{2}'", filePath, fileSize, sha1);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<AvAndShaMapping>() == null ? false : true;
        }

        public static int InsertShaMapping(AvAndShaMapping entity)
        {
            var sql = string.Format("INSERT INTO AvAndShaMapping (FilePath, FileSize, IsExist, Sha1, UpdateTime) VALUES (N'{0}', {1}, {2}, N'{3}', GETDATE())", entity.FilePath, entity.FileSize, entity.IsExist, entity.Sha1);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteShaMapping(string sha1)
        {
            var sql = string.Format("DELETE FROM AvAndShaMapping WHERE Sha1 = N'{0}'", sha1);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int UpdateShaMapping(int id, int status)
        {
            var sql = "UPDATE AvAndShaMapping SET IsExist = " + status + " WHERE AvAndShaMappingId = " + id;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static TokenModel GetToken()
        {
            var sql = "SELECT * FROM Token";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<TokenModel>();
        }

        public static int UpdateFaviAvator(string name, string avator)
        {
            var sql = "UPDATE FaviScan SET Avator = '" + avator + "' WHERE category = 'actress' AND Name = '" + name + "'";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }
    }
}
