using DataBaseManager.Common;
using Model.Common;
using Model.FindModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.FindDataBaseHelper
{
    public class FindDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;
        
        static FindDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};User=readonly;password=123456789", JavINIClass.IniReadValue("Scan", "server"), JavINIClass.IniReadValue("Scan", "db"));
            mycon = new SqlConnection(con);
        }

        public static List<Match> GetAllMovies()
        {
            var sql = @"SELECT * FROM Match";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>();
        }

        public static List<AVViewModel> GetAllViewModel(string where, int pageIndex, int pageSize, string order, ref PageModel total)
        {
            var sql = string.Format(@"SELECT * FROM (
                                        SELECT 
	                                    m.AvId, 
	                                    m.Name AS FileName, 
	                                    m.Location,
	                                    a.AvLength,
	                                    a.Category,
	                                    a.Actress,
	                                    a.Company,
	                                    a.Director,
	                                    a.Name,
	                                    a.Publisher,
	                                    a.ReleaseDate,
	                                    a.URL,
                                        ROW_NUMBER() OVER (ORDER BY a.ReleaseDate {3}, a.AvId ASC, m.Name) AS RANK
                                    FROM 
	                                    [ScanAllAv].[dbo].[Match] m
	                                    LEFT JOIN [JavLibraryDownload].[dbo].[AV] a on m.AvID = a.ID AND m.AvName = a.Name
                                    WHERE 1=1 {0}
				            ) AS t WHERE t.RANK BETWEEN {1} AND {2}", where, pageIndex * pageSize, (pageIndex + 1) * pageSize, order);

            var sqlCount = string.Format(@"SELECT count(1) AS Total FROM [ScanAllAv].[dbo].[Match] m LEFT JOIN[JavLibraryDownload].[dbo].[AV] a on m.AvID = a.ID  WHERE 1=1 {0}", where);

            total = SqlHelper.ExecuteDataTable(con, CommandType.Text, sqlCount).ToModel<PageModel>() ?? new PageModel();
            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<AVViewModel>();
        }
    }
}
