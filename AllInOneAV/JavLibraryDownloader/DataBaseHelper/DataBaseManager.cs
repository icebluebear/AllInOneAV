using JavLibraryDownloader.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.DataBaseHelper
{
    public class DataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static DataBaseManager()
        {
            con = string.Format("Server={0};Database={1};Trusted_Connection=SSPI", INIClass.IniReadValue("Jav", "server"), INIClass.IniReadValue("Jav", "db"));
            mycon = new SqlConnection(con);
        }

        private static SqlDataReader GetReader(string sql)
        {
            SqlCommand commond = new SqlCommand(sql, mycon);
            var reader = commond.ExecuteReader();

            return reader;
        }

        private static void ExecuteQuery(string sql)
        {
            SqlCommand commond = new SqlCommand(sql, mycon);
            commond.ExecuteNonQuery();
        }

        public static void WriteLog(JavLibraryLog log)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("Insert into JavLibraryLog values ('{0}', '{1}', '{2}', {3}, '{4}', {5})", log.Logger.Trim(), log.URL.Trim(), log.Content.Trim(), log.IsException == true ? 1 : 0, DateTime.Now, log.BatchID);
                ExecuteQuery(sql);

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
        }

        public static int GetLastBatchID()
        {
            int res = 0;
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = "select top 1 BatchID from Batch Order By CreateTime desc";

                var reader = GetReader(sql);

                while (reader.Read())
                {
                    res = int.Parse(reader[0].ToString());
                }

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }

            return res;
        }

        public static bool HasSameCategory(Category category)
        {
            bool res = false;
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("Select count(name) from category where name = '{0}' and url = '{1}'", category.Name.Trim(), category.Url);
                var reader = GetReader(sql);

                while (reader.Read())
                {
                    res = int.Parse(reader[0].ToString()) > 0 ? true : false;
                }

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }

            return res;
        }

        public static void SaveCategory(Category category)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("Insert into category values ('{0}', '{1}', GETDATE())", category.Name.Trim(), category.Url);

                ExecuteQuery(sql);

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
        }

        public static List<Category> GetAllCategories()
        {
            List<Category> res = new List<Category>();
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = "Select Name, Url from category";
                var reader = GetReader(sql);

                while (reader.Read())
                {
                    Category temp = new Models.Category();
                    temp.Name = reader[0].ToString();
                    temp.Url = reader[1].ToString();

                    res.Add(temp);
                }

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
            return res;
        }

        public static bool HasScanUrl(ScanURL scanUrl)
        {
            bool res = false;
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("Select count(url) from ScanURL where url = '{0}'", scanUrl.URL);
                var reader = GetReader(sql);

                while (reader.Read())
                {
                    res = int.Parse(reader[0].ToString()) > 0 ? true : false;
                }

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }

            return res;
        }

        public static void SaveScanUrl(ScanURL scanUrl, string page, int totalPage, int currentPage, int total, JavLibraryLog _logger)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("Insert into ScanURL values ('{0}', '{1}', '{2}', '{3}', {4}, '{5}')", scanUrl.Category.Trim(), scanUrl.URL.Trim(), scanUrl.ID.Trim(), scanUrl.Title.Trim(), scanUrl.IsDownload == true ? 1 : 0, scanUrl.CreateTime);
                ExecuteQuery(sql);

                pool.closeConnection(mycon);

                Console.WriteLine("Processing {0} - {1} - {2}/{3} - {4}/{5} success", scanUrl.Title, scanUrl.Category, page, totalPage, currentPage, total);
                _logger.WriteLog(scanUrl.URL, string.Format("Save ScanURL {0} success", scanUrl.Title));
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);

                var ts = e.ToString();

                if (e.ToString().Contains("Error Number:2601"))
                {
                    Console.WriteLine("Processing {0} - {1} - {2}/{3} - {4}/{5} successfailed duplicated", scanUrl.Title, scanUrl.Category, page, totalPage, currentPage, total);
                    _logger.WriteExceptionLog(scanUrl.URL, string.Format("Save ScanURL {0} failed duplicated", scanUrl.Title));
                }
                else
                {
                    Console.WriteLine(e.ToString());
                    _logger.WriteExceptionLog(scanUrl.URL, string.Format("Save ScanURL {0} falied {1}", scanUrl.Title, e.ToString()));
                }
            }
        }

        public static List<ScanURL> GetAllScanUrl()
        {
            List<ScanURL> res = new List<ScanURL>();

            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("select category, url, id, title, isdownload, createtime from scanurl");
                var reader = GetReader(sql);

                while (reader.Read())
                {
                    ScanURL temp = new ScanURL();
                    temp.Category = reader[0].ToString();
                    temp.URL = reader[1].ToString();
                    temp.ID = reader[2].ToString();
                    temp.Title = reader[3].ToString();
                    temp.IsDownload = reader[4].ToString() == "1" ? true : false;
                    temp.CreateTime = DateTime.Parse(reader[5].ToString());

                    res.Add(temp);
                }

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }


            return res;
        }

        public static void SaveComapny(string name)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("insert into company values ('{0}', '', GETDATE())", name.Trim());
                ExecuteQuery(sql);

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
        }

        public static void SavePublisher(string name)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("insert into publisher values ('{0}', '', GETDATE())", name.Trim());
                ExecuteQuery(sql);

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
        }

        public static void SaveDirector(string name)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("insert into director values ('{0}', '', GETDATE())", name.Trim());
                ExecuteQuery(sql);

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
        }

        public static void SaveActress(string name)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("insert into actress values ('{0}', '', GETDATE())", name.Trim());
                ExecuteQuery(sql);

                pool.closeConnection(mycon);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);
            }
        }

        public static void SaveAV(AV av, JavLibraryLog _logger)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("insert into av values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}', '{11}')", av.ID.Trim(), av.Name.Trim(), av.Company.Trim(), av.Directory.Trim(), av.Publisher.Trim(), av.Category.Trim(), av.Actress.Trim(), av.ReleaseDate, av.AvLength, av.CreateTime, av.PictureURL, av.URL);
                ExecuteQuery(sql);

                pool.closeConnection(mycon);

                Console.WriteLine(string.Format("Save AV {0} success", av.ID));
                _logger.WriteLog("download", string.Format("Save AV {0} success", av.ID));

                UpdateScanURL(av.URL, _logger);
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);

                _logger.WriteExceptionLog("", string.Format("Save AV {0} failed, {1}", av.ID, e.ToString()));
                Console.WriteLine(string.Format("Save AV {0} failed, {1}", av.ID, e.ToString()));
            }
        }

        public static void UpdateScanURL(string url, JavLibraryLog _logger)
        {
            var pool = ConnectionPool.getPool();

            try
            {
                mycon = pool.getConnection();

                var sql = string.Format("update scanurl set isdownload = 1 where url = '{0}'", url);
                ExecuteQuery(sql);

                pool.closeConnection(mycon);

                Console.WriteLine(string.Format("Update ScanUrl {0} success", url));
                _logger.WriteLog("update", string.Format("Update ScanUrl {0} success", url));
            }
            catch (Exception e)
            {
                pool.closeConnection(mycon);

                _logger.WriteExceptionLog("", string.Format("Update ScanUrl {0} failed, {1}", url, e.ToString()));
                Console.WriteLine(string.Format("Update ScanUrl {0} failed, {1}", url, e.ToString()));
            }
        }
    }
}
