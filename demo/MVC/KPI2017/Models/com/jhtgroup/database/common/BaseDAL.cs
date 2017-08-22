using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace com.jhtgroup.database.common
{
    // All of connection string 
    public enum ConnectionString
    {
        Manufacture,
        CPI,
        IDES,
        Agentflow,
        Sales,
        CRM
    }


    public class BaseDAL<T> where T : new()
    {
        private string _connStr = ConfigurationManager.ConnectionStrings["CPI"].ConnectionString; // default connection string 

        private List<T> _list = null; //將回傳的資料集合
        private Dictionary<string, object> _parameters; //呼叫所用的參數


        /// <summary>
        /// 新增參數
        /// </summary>
        /// <param name="name">參數名</param>
        /// <param name="value">參數值</param>
        public void addParameter(string name, object value)
        {
            if (_parameters == null)
                resetParameter();

            if (!_parameters.ContainsKey(name))
                _parameters.Add(name, value);
        }

        /// <summary>
        /// 執行 store procedure (5m timeout)
        /// </summary>
        /// <param name="procedureName">Store procedure name</param>
        public void execProcedure(string procedureName)
        {
            if (procedureName == null || procedureName.Length == 0 || _parameters == null)
                return;

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 300;

                foreach (KeyValuePair<string, object> item in _parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 執行 store procedure (5m timeout)，並回傳結果
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns>List of T</returns>
        public List<T> execProcedureReturnData(string procedureName)
        {
            if (procedureName == null || procedureName.Length == 0)
                return null;

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 300;

                if (_parameters != null)
                {
                    foreach (KeyValuePair<string, object> item in _parameters)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        _list = DataTableExten.ToList<T>(dt).ToList();
                    }
                }
                return _list;
            }
        }

        /// <summary>
        /// 執行sql (30m timeout)
        /// </summary>
        /// <param name="sql">SQL Statement</param>
        /// <returns>executed count</returns>
        public int execute(string sql)
        {
            if (sql == null || sql.Length == 0)
                return 0;

            resetParameter();
            return executeWithParameters(sql);
        }

        /// <summary>
        /// 執行sql (30m timeout)
        /// </summary>
        /// <param name="sql">SQL Statement</param>
        /// <returns>executed count</returns>
        public int executeWithParameters(string sql)
        {
            if (sql == null || sql.Length == 0)
                return 0;
            if (_parameters == null)
                throw new ArgumentNullException("parameters required !");

            int execCnt = 0;
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1800;
                if (_parameters != null && _parameters.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in _parameters)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                execCnt = cmd.ExecuteNonQuery();
            }

            return execCnt;
        }

        /// <summary>
        /// 執行sql (30m timeout)
        /// </summary>
        /// <param name="sql">SQL Statement</param>
        /// <returns>Guid</returns>
        public Guid executeWithParametersReturn(string sql)
        {
            if (sql == null || sql.Length == 0)
                return Guid.Empty;
            if (_parameters == null)
                throw new ArgumentNullException("parameters required !");

            Guid uid;
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1800;
                if (_parameters != null && _parameters.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in _parameters)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                uid = (Guid)cmd.ExecuteScalar();
            }

            return uid;
        }

        /// <summary>
        /// 依sql進行查詢 (30m timeout)
        /// </summary>
        /// <param name="sql">SQL Statement</param>
        /// <returns>List of T</returns>
        public List<T> query(string sql)
        {
            if (sql == null || sql.Length == 0)
                return null;

            resetParameter();
            _list = queryWithParameters(sql);

            return _list;
        }

        /// <summary>
        /// 依sql進行查詢 (30m timeout)
        /// </summary>
        /// <param name="sql">SQL Statement</param>
        /// <returns>List of T</returns>
        public List<T> queryWithParameters(string sql)
        {
            if (sql == null || sql.Length == 0)
                return null;
            if (_parameters == null)
                throw new ArgumentNullException("parameters required !");

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1800;
                if (_parameters != null && _parameters.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in _parameters)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                if (dt != null && dt.Rows.Count > 0)
                {
                    _list = DataTableExten.ToList<T>(dt).ToList();
                }
            }

            return _list;
        }

        /// <summary>
        /// 重設所有參數
        /// </summary>
        public void resetParameter()
        {
            _parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 組出同一欄位多個like的查詢語句
        /// </summary>
        /// <param name="columnName">Table欄名</param>
        /// <param name="strArray">資料集合(aa,bb,cc,...)</param>
        /// <returns>SQL statement: and (columnName like 'aa%' or columnName like 'bb%'...)</returns>
        protected string generateMultiAndConditions(string columnName, string strArray)
        {
            string sql = "";
            if (strArray == null || strArray.Equals(""))
                return "";

            if (strArray.IndexOf(",") > 0)
            {
                sql += " and (";
                string[] ary = strArray.Split(',');
                for (int i = 0; i < ary.Length; i++)
                {
                    sql += columnName + " like '" + ary[i] + "%'";
                    if (i + 1 < ary.Length)
                        sql += " or ";
                }
                sql += ") ";
            }
            else
            {
                sql += " and " + columnName + " like '" + strArray + "%' ";
            }
            return sql;
        }

        /// <summary>
        /// 執行SQL前，設定新的連線字串
        /// </summary>
        /// <param name="enumConn">ConnectionString Enum</param>
        protected void setConnectionstring(ConnectionString enumConn)
        {
            this._connStr = getConnectionStr(enumConn);
        }

        /// <summary>
        /// 取DB連線字串
        /// </summary>
        /// <param name="enumConn">ConnectionString Enum</param>
        /// <returns>connection string</returns>
        private string getConnectionStr(ConnectionString enumConn)
        {
            switch (enumConn)
            {
                case ConnectionString.Manufacture:
                    return ConfigurationManager.ConnectionStrings["Manufacture"].ConnectionString;
                case ConnectionString.CPI:
                    return ConfigurationManager.ConnectionStrings["CPI"].ConnectionString;
                case ConnectionString.IDES:
                    return ConfigurationManager.ConnectionStrings["IDES"].ConnectionString;
                case ConnectionString.Agentflow:
                    return ConfigurationManager.ConnectionStrings["agentflow"].ConnectionString;
                case ConnectionString.Sales:
                    return ConfigurationManager.ConnectionStrings["SALES"].ConnectionString;
                case ConnectionString.CRM:
                    return ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;
                default:
                    return ConfigurationManager.ConnectionStrings["Manufacture"].ConnectionString;
            }
        }
    }
}