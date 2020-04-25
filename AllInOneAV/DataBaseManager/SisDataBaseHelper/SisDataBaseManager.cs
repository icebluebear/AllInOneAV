using DataBaseManager.Common;
using Model.SisModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.SisDataBaseHelper
{
    public class SisDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static SisDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};User=sa;password=19880118Qs123!", JavINIClass.IniReadValue("Sis", "server"), JavINIClass.IniReadValue("Sis", "db"));
            mycon = new SqlConnection(con);
        }

        public static DateTime GetLastOperationEndDate()
        {
            var sql = @"SELECT Top 1 LastOperationEndDate AS LastOperationDate from LastOperationEndDate ORDER BY LastOperationEndDateID DESC";
            var model = SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<LastOperationEndDate>();

            return  model == null ? new LastOperationEndDate().LastOperationDate : model.LastOperationDate;
        }

        public static bool IsExistScanThread(ScanThread st)
        {
            var sql = @"SELECT TOP 1 * FROM AlreadyScaned WHERE Url = '" + st.Url + "'";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ScanThread>() == null ? false : true;
        }

        public static int InsertScanThread(ScanThread st)
        {
            var sql = @"INSERT INTO AlreadyScaned (Channel, Url, Name, ScannedDate, IsDownloaded) 
                        VALUES (@channel, @url, @name, @scannedDate, @isDownloaded)";

            SqlParameter[] paras = {
                new SqlParameter("@channel",SqlDbType.NVarChar,100),
                new SqlParameter("@url",SqlDbType.NVarChar,100),
                new SqlParameter("@name",SqlDbType.NVarChar,500),
                new SqlParameter("@scannedDate",SqlDbType.DateTime),
                new SqlParameter("@isDownloaded",SqlDbType.Int)
            };

            paras[0].Value = st.Channel;
            paras[1].Value = st.Url;
            paras[2].Value = st.Name;
            paras[3].Value = st.ScannedDate;
            paras[4].Value = st.IsDownloaded;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
        }

        public static List<ScanThread> GetScanThread()
        {
            var sql = @"SELECT * FROM AlreadyScaned";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<ScanThread>();
        }

        public static int UpdateDownload(string url)
        {
            var sql = @"UPDATE AlreadyScaned set IsDownloaded = 1 WHERE URL = '" + url + "'";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertLastOperationEndDate(DateTime endDate)
        {
            var sql = @"INSERT INTO LastOperationEndDate (LastOperationEndDate) VALUES (@lastOperationEndDate)";

            SqlParameter[] paras = {
                new SqlParameter("@lastOperationEndDate",SqlDbType.DateTime)
            };

            paras[0].Value = endDate;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
        }

        public static string GetURLFromName(string name)
        {
            var sql = @"SELECT TOP 1 URL FROM AlreadyScaned WHERE Name = '" + name + "'";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ScanThread>().Url;
        }
    }
}
