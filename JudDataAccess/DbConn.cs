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
        /// Funktionen DbReturnDataTable, skal returnere
        /// datatypen DataTable og modtage et SQL-Udtryk i vores strSql string.
        /// Vi opretter en ny instans af DataTable, navngiver den dtRes og åbner forbindelsen til DB.
        /// Derefter fortæller vi vores DB hvilke informationer den skal hive ud, via strSQL og
        /// indsætter resultatsættet i dtRes
        /// Til slut lukker vi DB igen.
        /// </summary>
        /// <param name="strSql">Tekststreng med SQL-Udtrykket</param>
        /// <returns>DataTable med det samlede resultat</returns>
        protected DataTable DbReturnDataTable(string strSql)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                using (SqlCommand command = new SqlCommand(strSql, myConnection))
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

            return dtRes;
        }

        /// <summary>
        /// Denne funktion returnerer en string, 
        /// der modsvarer strSql forespørgslen.
        /// Til slut lukker vi DB igen.
        /// </summary>
        /// <param name="strSQL">Tekststreng med SQL-Udtrykket</param>
        /// <returns>String</returns>
        protected string DbReturnString(string strSQL)
        {
            string strRes = "";

            try
            {
                OpenDB();
                using(SqlCommand cmd = new SqlCommand(strSQL, myConnection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        strRes = reader.GetValue(0).ToString();
                    }
                }


                CloseDB();

            }
            catch(SqlException ex)
            {
                throw ex;
            }


            return strRes;
        }

        /// <summary>
        /// Funktion der henter én, lang liste af strings med alle data i DB.
        /// Til slut lukker vi DB igen.
        /// </summary>
        /// <param name="strSql">Tekststreng med SQL-Udtrykket</param>
        /// <returns>list</returns>
        protected List<string> DbReturnListString(string strSql)
        {
            MessageBox.Show(strSql + ".\n" + myConnection.ConnectionString + ".", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
            List<string> listString = new List<string>();
            try
            {
                OpenDB();
                using (var cmd = myConnection.CreateCommand())
                {
                    cmd.CommandText = strSql;
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

                CloseDB();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Fejl", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return listString;
        }

        /// <summary>
        /// Funktion der henter én, lang ArrayListe af strings med alle data i DB.
        /// Til slut lukker vi DB igen.
        /// </summary>
        /// <param name="strSql">Tekststreng med SQL-Udtrykket</param>
        /// <returns>ArrayList</returns>
        protected ArrayList DbReturnArrayListString(string strSql)
        {
            ArrayList arrayList = new ArrayList();
            try
            {
                OpenDB();
                using (var cmd = myConnection.CreateCommand())
                {
                    cmd.CommandText = strSql;
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

                CloseDB();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return arrayList;
        }

        /// <summary>
        /// Funktion, der skriver i DB.
        /// Til slut lukker vi DB igen.
        /// </summary>
        /// <param name="strSql">Tekststreng med SQL-Udtrykket</param>
        protected void FunctionExecuteNonQuery(string strSql)
        {
            try
            {
                OpenDB();
                using (SqlCommand cmd = new SqlCommand(strSql, myConnection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                CloseDB();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// Funktion, der returnerer en boolsk værdi fra DB.
        /// Til slut lukker vi DB igen.
        /// </summary>
        /// <param name="strSql">Tekststreng med SQL-Udtrykket</param>
        /// <returns>bool</returns>
        protected bool DbReturnBool(string strSql)
        {
            bool bolRes = false;
            try
            {
                OpenDB();
                using (SqlCommand cmd = new SqlCommand(strSql, myConnection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bolRes = Convert.ToBoolean(reader.GetValue(0).ToString());
                    }
                }

                CloseDB();
            }
            catch(SqlException ex)
            {
                throw ex;
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
