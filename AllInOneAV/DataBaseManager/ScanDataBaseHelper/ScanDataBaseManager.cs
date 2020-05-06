using DataBaseManager.Common;
using Model.FindModels;
using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.ScanDataBaseHelper
{
    public class ScanDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static ScanDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};User=readonly;password=123456789", JavINIClass.IniReadValue("Scan", "server"), JavINIClass.IniReadValue("Scan", "db"));
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
            var sql = string.Format(@"INSERT INTO Match (AvID, Name, Location, CreateTime, AvName) VALUES ('{0}', N'{1}', N'{2}', GETDATE(), N'{3}')", match.AvID, FileUtility.ReplaceInvalidChar(match.Name), match.Location, FileUtility.ReplaceInvalidChar(match.AvName));

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
            var sql = "INSERT INTO MagUrl (AvId, MagUrl, MagTitle, IsFound) VALUES ('" + avid + "', '" + magUrl + "', '" + magTitle + "', " + isFound + ")";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<ScanResult> GetMatchScanResult()
        {
            var sql = @"SELECT m.Location, m.Name AS FileName, a.ID AS AvId, a.Company, a.Name AS AvName, a.Director, a.Publisher, a.Category, a.Actress, a.ReleaseDate FROM ScanAllAv.dbo.Match m LEFT JOIN JavLibraryDownload.dbo.AV a ON m.AvID = a.ID";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<ScanResult>();
        }

        public static int DeleteFaviScan()
        {
            var sql = "TRUNCATE TABLE FaviScan";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertFaviScan(FaviScan favi)
        {
            var sql = "INSERT INTO FaviScan (Category, Url, Name) VALUES ('" + favi.Category + "', '" + favi.Url + "', '" + favi.Name + "')";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<FaviScan> GetFaviScan()
        {
            var sql = "SELECT * FROM FaviScan ORDER BY Category";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<FaviScan>();
        }
    }
}
