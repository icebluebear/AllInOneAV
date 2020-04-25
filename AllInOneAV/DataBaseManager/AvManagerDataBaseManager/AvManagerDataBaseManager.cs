using DataBaseManager.Common;
using DataBaseManager.LogHelper;
using Model.AvManager;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.AvManagerDataBaseManager
{
    public class AvManagerDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static AvManagerDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};User=sa;password=19880118Qs123!", JavINIClass.IniReadValue("AvManager", "server"), JavINIClass.IniReadValue("AvManager", "db"));
            mycon = new SqlConnection(con);
        }

        public static int InserMoveLog(string src, string des, DateTime time)
        {
            var dt = time.ToString("yyyy-MM-dd hh:mm:ss");
            var sql = string.Format("INSERT INTO MoveLog (Src, Des, CreateTime) VALUES ('{0}','{1}','{2}')", src, des, dt);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<MoveLog> GetMoveLog()
        {
            var sql = @"SELECT * FROM MoveLog";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<MoveLog>();
        }
    }
}
