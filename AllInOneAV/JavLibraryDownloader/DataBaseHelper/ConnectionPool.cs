using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.DataBaseHelper
{
    using System.Data.SqlClient;
    using System.Collections;
    /// <summary>
    /// ConnectionPool 的摘要说明
    /// </summary>
    public class ConnectionPool
    {
        private static ConnectionPool cpool = null;//池管理对象
        private static Object objlock = typeof(ConnectionPool);//池管理对象实例
        private static int size = 10;//池中连接数
        private int useCount = 0;//已经使用的连接数
        private static ArrayList pool = null;//连接保存的集合
        private static String ConnectionStr = "";//连接字符串

        static ConnectionPool()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnectionStr = string.Format("Server={0};Database={1};Trusted_Connection=SSPI", INIClass.IniReadValue("Jav", "server"), INIClass.IniReadValue("Jav", "db"));
            size = 100;
            pool = new ArrayList();
        }

        //创建获取连接池对象
        public static ConnectionPool getPool()
        {
            lock (objlock)
            {
                if (cpool == null)
                {
                    cpool = new ConnectionPool();
                }
                return cpool;
            }
        }
        //获取池中的连接
        public SqlConnection getConnection()
        {
            lock (pool)
            {
                SqlConnection tmp = null;
                if (pool.Count > 0)
                {
                    tmp = (SqlConnection)pool[0];
                    pool.RemoveAt(0);
                    //不成功
                    if (!isUserful(tmp))
                    {
                        //可用的连接数据已去掉一个
                        useCount--;
                        tmp = getConnection();
                    }
                }
                else
                {
                    //可使用的连接小于连接数量
                    if (useCount < size)
                    {
                        try

                        {
                            //创建连接
                            SqlConnection conn = new SqlConnection(ConnectionStr);
                            conn.Open();
                            useCount++;
                            tmp = conn;
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                return tmp;
            }
        }
        //关闭连接,加连接回到池中
        public void closeConnection(SqlConnection con)
        {
            lock (pool)
            {
                if (con != null)
                {
                    pool.Add(con);
                }
            }
        }
        //目的保证所创连接成功,测试池中连接
        private bool isUserful(SqlConnection con)
        {
            //主要用于不同用户
            bool result = true;
            if (con != null)
            {
                string sql = "select 1";//随便执行对数据库操作
                SqlCommand cmd = new SqlCommand(sql, con);
                try
                {

                    cmd.ExecuteScalar().ToString();
                }
                catch
                {
                    result = false;
                }

            }
            return result;
        }


    }


}
