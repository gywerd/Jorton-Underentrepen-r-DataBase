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
        #region Constructors
        /// <summary>
        /// Constructoren Class_YourProjectname initierer class DbConn med passende parametre
        /// der fortæller hvilken server, og hvilken database der skal kopbles op med.
        /// Connection med PW: Data Source=YYY.205.XX.31,\ASPITSQLSERVER;Initial Catalog = JensVen; User ID = sa; Password=QQQQQQQQQQQQQQQQ;
        /// Connection trusted: Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
        /// Connection IP-address: Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password=myPassword;
        /// For yderligere info se: https://www.connectionstrings.com/sql-server/
        /// </summary>
        /// <param name="strCon">string</param>
        public Executor(string strCon)
        {
            StrConnectionString = strCon;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that adds a User to Db
        /// </summary>
        /// <param name="person">int</param>
        /// <param name="initials">string</param>
        /// <param name="passWord">string</param>
        /// <param name="jobDescription">int</param>
        /// <param name="userLevel">int</param>
        /// <returns>bool</returns>
        public bool AddUser(int person, string initials, string passWord, int jobDescription, int userLevel)
        {
            return Convert.ToBoolean(DbReturnString(@"usersAddUser pPerson = " + person + ", pInitials = " + initials + @", pPassword = '" + passWord + @"', pJobDescription = " + jobDescription + @", pUserLevel = " + userLevel + @", responseMessage=responseMessage OUTPUT"));
        }

        /// <summary>
        /// Method, that sets new password in Db, if old password is correct
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="oldPassWord">string</param>
        /// <param name="newPassWord">string</param>
        /// <returns></returns>
        public bool ChangePassword(int id, string oldPassWord, string newPassWord)
        {
            return Convert.ToBoolean(DbReturnString(@"usersUpdatePassword pId = " + id + @", pOldPassword = '" + oldPassWord + @"', pNewPassword = '" + newPassWord + @"', responseMessage=responseMessage OUTPUT"));
        }


        /// <summary>
        /// Method, that checks wether password is correct
        /// </summary>
        /// <param name="initials">string</param>
        /// <param name="passWord">string</param>
        /// <returns></returns>
        public bool CheckLogin(string initials, string passWord)
        {
            return Convert.ToBoolean(DbReturnString(@"usersLogin pInitials = '" + initials + @"', pOldPassword = '" + passWord + @"', responseMessage=responseMessage OUTPUT"));
        }

        /// <summary>
        /// Method, that retrieves a List from Db
        /// </summary>
        /// <param name="table">string</param>
        /// <param name="id">int</param>
        /// <returns>List<string></returns>
        public List<string> ReadListFromDataBase(string table)
        {
            List<string> listRes = new List<string>();
            DataTable dm = GetListDataTable(table);

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

        /// <summary>
        /// Method, that retrieves a DataTable containing a List
        /// </summary>
        /// <param name="table">string</param>
        /// <returns>DataTable</returns>
        private DataTable GetListDataTable(string table)
        {
            DataTable result = new DataTable();

            switch (table)
            {
                case "Entrepeneurs":
                    result = DbReturnDataTable(@"SELECT * FROM [Entrepeneurs] ORDER BY [Cooperative] DESC, [Id] ASC");
                    break;
                case "Projects":
                    result = DbReturnDataTable(@"SELECT * FROM [Projects] ORDER BY [Name] ASC");
                    break;
                case "Users":
                    result = DbReturnDataTable(@"SELECT Id, Person, Initials, JobDescription, UserLevel FROM [Users]");
                    break;
                default:
                    result = DbReturnDataTable("SELECT * FROM " + table);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a DataTable containing a row
        /// </summary>
        /// <param name="table">string</param>
        /// <param name="id">int</param>
        /// <returns>DataTable</returns>
        private DataTable GetPostDataTable(string table, int id)
        {
            DataTable result = new DataTable();

            switch (table)
            {
                case "ActiveProjects":
                    result = DbReturnDataTable("SELECT * FROM [LegalEntities] WHERE [Active] = 'true', [Id] = " + id + @" ORDER BY [Cooperative] DESC, [Name] ASC");
                    break;
                case "InactiveProjects":
                    result = DbReturnDataTable("SELECT * FROM [LegalEntities] WHERE [Active] = 'false', [Id] = " + id + @" ORDER BY [Name] ASC");
                    break;
                case "Projects":
                    result = DbReturnDataTable("SELECT * FROM [LegalEntities] WHERE [Id] = " + id + @"  ORDER BY [Name] ASC");
                    break;
                default:
                    result = DbReturnDataTable(@"SELECT * FROM " + table + @" WHERE [Id] = " + id);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a post from Db
        /// </summary>
        /// <param name="table">string</param>
        /// <param name="id">int</param>
        /// <returns>string</returns>
        public string ReadPostFromDataBase(string table, int id)
        {
            List<string> listResult = new List<string>();
            DataTable dm = GetPostDataTable(table, id);
            
            foreach (DataRow row in dm.Rows)
            {
                string rowString = "";

                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    rowString += row[i] + ";";
                }
                rowString = rowString.Remove(rowString.Length - 1);
                listResult.Add(rowString);
            }
            return listResult[0];
        }

        /// <summary>
        /// Method, that retrieves a Project List from Db
        /// Accepts the following tables: Enterprises [projectId], Shippings [projectId] & SubEntrepeneurs [enterpriseId]
        /// 
        /// </summary>
        /// <param name="table">string</param>
        /// <param name="id">int</param>
        /// <returns>List<string></returns>
        public List<string> ReadProjectListFromDataBase(string table, int id)
        {
            List<string> listResult = new List<string>();
            DataTable dm = GetProjectListDataTable(table, id);

            foreach (DataRow row in dm.Rows)
            {
                string rowString = "";

                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    rowString += row[i] + ";";
                }
                rowString = rowString.Remove(rowString.Length - 1);
                listResult.Add(rowString);
            }
            return listResult;
        }

        /// <summary>
        /// Method, that retrieves a DataTable containing a List
        /// Accepts the following tables: Enterprises, Shippings & SubEntrepeneurs
        /// </summary>
        /// <param name="table">string</param>
        /// <param name="projectId">int</param>
        /// <returns>DataTable</returns>
        private DataTable GetProjectListDataTable(string table, int projectId)
        {
            return DbReturnDataTable(@"SELECT * FROM [" + table + @"] WHERE [Project] = " + projectId + "");
        }

        /// <summary>
        /// Method, that sends an SQL-Query string to Db
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>bool</returns>
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

        #endregion

    }
}
