using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudDataAccess
{
    public class Executor : DbConn
    {
        
        /// <summary>
        /// Constructoren Class_YourProjectname initierer class DbConn med passende parametre
        /// der fortæller hvilken server, og hvilken database der skal kopbles op med.
        /// Connection med PW: Data Source=YYY.205.XX.31,\ASPITSQLSERVER;Initial Catalog = JensVen; User ID = sa; Password=QQQQQQQQQQQQQQQQ;
        /// Connection trusted: Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
        /// Connection IP-address: Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password=myPassword;
        /// For yderligere info se: https://www.connectionstrings.com/sql-server/
        /// </summary>
        /// <param name="strCon"></param>
        public Executor(string strCon)
        {
            StrConnectionString = strCon;
        }

        public List<string> ReadListFromDataBase(string table, int id = -1)
        {
            List<string> listRes = new List<string>();
            DataTable dm;
            if (id != -1)
            {
                dm = DbReturnDataTable(@"SELECT * FROM " + table + @"WHERE [Id] = " + id);
            }
            else
            {
                dm = DbReturnDataTable("SELECT * FROM " + table);
            }

            foreach (DataRow row in dm.Rows)
            {
                string rowString = "";

                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    rowString += row[i] + ";";
                }
                rowString = rowString.Remove(rowString.Length - 1);
                listRes.Add(rowString);
            }
            return listRes;
        }

        public bool WriteToDataBase(string strSql)
        {
            try
            {
                FunctionExecuteNonQuery(strSql);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
