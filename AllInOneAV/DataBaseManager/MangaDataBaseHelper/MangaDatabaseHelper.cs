using DataBaseManager.Common;
using Model.JavModels;
using Model.MangaModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.MangaDataBaseHelper
{
    public class MangaDatabaseHelper 
    {
        private static string con;
        private static SqlConnection mycon;

        static MangaDatabaseHelper()
        {
            con = string.Format("Server={0};Database={1};User=sa;password=19880118Qs123!", JavINIClass.IniReadValue("Manga", "server"), JavINIClass.IniReadValue("Manga", "db"));
            mycon = new SqlConnection(con);
        }

        public static int InsertMangaCategory(MangaCategory entity)
        {
            var sql = string.Format("INSERT INTO MangaCategory (SourceType, RootCategory, Category, Url) VALUES ({0}, '{1}', '{2}', '{3}' )", (int)entity.SourceType, entity.RootCategory, entity.Category, entity.Url );

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<MangaCategory> GetMangaCategoryByType(MangaCategorySourceType type)
        {
            var sql = string.Format("SELECT RootCategory, Category, Url FROM MangaCategory WHERE SourceType = {0}", (int)type);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<MangaCategory>();
        }
    }
}
