using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access
{
    internal class DBHelper
    {
        //数据库链接字符串
        private static string ConnString = "server=.;database=DB_OAwork;uid=sa;pwd=123456";
        //数据库命名对象
        private static SqlConnection conn = new SqlConnection(ConnString);
        //数据库SQL执行命令
        private static SqlCommand comm = null;
        //数据库适配器
        private static SqlDataAdapter ad = null;

        //查询DataTable
        public static DataTable GetDTBySQL(string sql)
        {
            //如果是datatable则不用打开关闭链接，是持续性链接的
            ad = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }
        //增删改返回受影响的行数
        public static int ExecuteBySQL(string sql)
        {
            comm = new SqlCommand(sql, conn);
            conn.Open();
            int num = comm.ExecuteNonQuery();
            comm.Dispose();
            conn.Close();
            return num;
        }

        //存储过程查询方法 ADO.NET执行存储过程返回查询DataTable
        public static DataTable GetDTByPROC(string proName, SqlParameter[] parameter)
        {
            comm = new SqlCommand(proName, conn);
            //创建SqlCommand并设置其类型为StoredProcedure
            comm.CommandType = CommandType.StoredProcedure;
            //参数循环添加到Parameter数组中
            foreach (SqlParameter p in parameter)
            {
                comm.Parameters.Add(p);
            }
            //ad执行comm填充到datatable
            ad = new SqlDataAdapter(comm);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }
        //增删改存储过程方法调用
        public static int ExecuteByPROC(string proName, SqlParameter[] parameter)
        {
            conn.Open();
            comm = new SqlCommand(proName, conn);
            comm.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter p in parameter)
            {
                comm.Parameters.Add(p);
            }
            //执行存储过程返回数据库值
            int num = comm.ExecuteNonQuery();
            conn.Close();
            return num;
        }
    }
}
