using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Windows;

namespace JudDataAccess
{
    public class DbConn
    {
        #region Fields
        private SqlConnection myConnection;
        private string strConnectionString;

        #endregion

        #region Constructors
        public DbConn()
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that opens the connection to the Db
        /// </summary>
        protected void OpenDB()
        {
            try
            {
                if(myConnection != null && myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                else
                {
                    if(myConnection == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        CloseDB();
                        OpenDB();
                    }
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method, that closes the connection to the Db
        /// </summary>
        protected void CloseDB()
        {
            try
            {
                myConnection.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method, that receives an SQL-query and returns the result as a DataTable
        /// </summary>
        /// <param name="sqlQuery">string with SQL-query</param>
        /// <returns>DataTable</returns>
        protected DataTable DbReturnDataTable(string sqlQuery)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                using (SqlCommand command = new SqlCommand(sqlQuery, myConnection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dtRes);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CloseDB();
            }

            return dtRes;
        }

        /// <summary>
        /// Method, that receives an SQL-command and returns the result as a DataTable
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <returns>DataTable</returns>
        protected DataTable DbReturnDataTable(SqlCommand cmd)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                using (cmd)
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dtRes);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CloseDB();
            }

            return dtRes;
        }

        /// <summary>
        /// Method, that receives an SQL-query and returns the result as bool
        /// </summary>
        /// <param name="sqlQuery">string with SQL-query</param>
        /// <param name="args">string[] - strengArray med parametre</param>
        /// <returns>bool</returns>
        protected bool DbReturnBool(string sqlQuery, string[] args)
        {
            bool result = false;
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            switch (sqlQuery)
            {
                case "usersAddUser":
                    cmd.Parameters.Add("@pPerson", SqlDbType.Int).Value = Convert.ToInt32(args[0]);
                    cmd.Parameters.Add("@pInitials", SqlDbType.NVarChar).Value = args[1];
                    cmd.Parameters.Add("@pPassword", SqlDbType.NVarChar).Value = args[2];
                    cmd.Parameters.Add("@pJobDescription", SqlDbType.Int).Value = Convert.ToInt32(args[3]);
                    cmd.Parameters.Add("@pUserLevel", SqlDbType.Int).Value = Convert.ToInt32(args[4]);
                    break;
                case "usersUpdatePassword":
                    cmd.Parameters.Add("@pId", SqlDbType.Int).Value = Convert.ToInt32(args[0]);
                    cmd.Parameters.Add("@pOldPassword", SqlDbType.NVarChar).Value = args[1];
                    cmd.Parameters.Add("@pNewPassword", SqlDbType.NVarChar).Value = args[2];
                    break;
                case "usersLogin":
                    cmd.Parameters.Add("@pInitials", SqlDbType.NVarChar).Value = args[0];
                    cmd.Parameters.Add("@pPassword", SqlDbType.NVarChar).Value = args[1];
                    break;
            }

            try
            {
                OpenDB();
                DataTable dtRes = DbReturnDataTable(cmd);

                if (dtRes.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRes.Rows)
                    {
                        result = Convert.ToBoolean(row[0].ToString());
                        break;
                    }

                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves one, long liste of strings with all data in Db.
        /// </summary>
        /// <param name="sqlQuery">string with SQL-query</param>
        /// <returns>list</returns>
        protected List<string> DbReturnListString(string sqlQuery)
        {
            MessageBox.Show(sqlQuery + ".\n" + myConnection.ConnectionString + ".", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
            List<string> listString = new List<string>();
            try
            {
                OpenDB();
                using (var cmd = myConnection.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                listString.Add(reader.GetValue(i).ToString());
                            }
                        }
                    }
                }

            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Fejl", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                CloseDB();
            }

            return listString;
        }

        /// <summary>
        /// Method, that retrieves one, long Array List of strings with all data in Db.
        /// </summary>
        /// <param name="sqlQuery">string with SQL-query</param>
        /// <returns>ArrayList</returns>
        protected ArrayList DbReturnArrayListString(string sqlQuery)
        {
            ArrayList arrayList = new ArrayList();
            try
            {
                OpenDB();
                using (var cmd = myConnection.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                arrayList.Add(reader.GetValue(i).ToString());
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }

            return arrayList;
        }

        /// <summary>
        /// Method, that sends an SQL-query to Db.
        /// </summary>
        /// <param name="sqlQuery">string with SQL-query</param>
        protected void FunctionExecuteNonQuery(string sqlQuery)
        {
            try
            {
                OpenDB();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, myConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
            finally
            {
                CloseDB();
            }
        }

        /// <summary>
        /// Method, that returns a boolean result from Db.
        /// </summary>
        /// <param name="sqlQuery">string with SQL-query</param>
        /// <returns>bool</returns>
        protected bool DbReturnBool(string sqlQuery)
        {
            bool bolRes = false;
            try
            {
                OpenDB();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, myConnection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bolRes = Convert.ToBoolean(reader.GetValue(0).ToString());
                    }
                }

            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }

            return bolRes;
        }

        #endregion

        #region Properties
        protected string StrConnectionString
        {
            get
            {
                return strConnectionString;
            }
            set
            {
                if (value != strConnectionString)
                {
                    strConnectionString = value;
                    myConnection = new SqlConnection(value);
                }
            }
        }

        #endregion

    }
}
