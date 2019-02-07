using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class MyEntityFrameWork
    {
        #region Fields
        public static string strConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LegalEntitySort;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private Executor executor = new Executor(strConnection);

        #region Lists
            public List<Contact> Contacts = new List<Contact>();
            public List<LegalEntity> LegalEntities = new List<LegalEntity>();
            public List<SubEntrepeneur> SubEntrepeneurs = new List<SubEntrepeneur>();

            #endregion

        #endregion

        #region Constructors
        public MyEntityFrameWork()
        {
            RefreshAllLists();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region DataBase

        /// <summary>
        /// Method, that processes an SQL-query
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool ProcesSqlQuery(string strSql) => executor.WriteToDataBase(strSql);

            #region Read
            /// <summary>
            /// Method, that return the amount of fields
            /// </summary>
            /// <param name="list"></param>
            /// <returns></returns>
            private int GetFieldAmount(string list)
            {
                int result = 0;

            switch (list)
                {
                    case "Contacts":
                        result = 4;
                        break;
                    case "LegalEntities":
                        result = 6;
                        break;
                    case "SubEntrepeneurs":
                        result = 11;
                        break;
                }

                return result;
            }

            /// <summary>
            /// Method, that converts an string array into an object of the correct type
            /// </summary>
            /// <param name="list"></param>
            /// <param name="resultArray"></param>
            /// <returns></returns>
            private object ConvertObject(string list, string[] resultArray)
            {
                object result = new object();

                switch (list)
                {
                    case "Contacts":
                        Contact contact = new Contact(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), Convert.ToInt32(resultArray[2]), resultArray[3]);
                        result = contact;
                        break;
                    case "LegalEntities":
                        LegalEntity legalEntity = new LegalEntity(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], Convert.ToInt32(resultArray[3]), Convert.ToInt32(resultArray[4]), resultArray[5]);
                        result = legalEntity;
                        break;
                    case "SubEntrepeneurs":
                        SubEntrepeneur subEntrepeneur = new SubEntrepeneur(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), Convert.ToInt32(resultArray[2]), Convert.ToInt32(resultArray[3]), Convert.ToInt32(resultArray[4]), Convert.ToInt32(resultArray[5]), Convert.ToInt32(resultArray[6]), Convert.ToBoolean(resultArray[7]), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]), Convert.ToBoolean(resultArray[10]));
                        result = subEntrepeneur;
                        break;
                }

                return result;
            }

            /// <summary>
            /// Method, that reads a List from Db
            /// </summary>
            /// <param name="list">string</param>
            /// <returns>List<object></returns>
            public List<object> ReadListFromDb(string list)
            {
                List<object> result = new List<object>();


                List<string> strResults;

                strResults = executor.ReadListFromDataBase(list);

                int fieldAmount = GetFieldAmount(list);

                foreach (string strResult in strResults)
                {
                    string[] resultArray = new string[fieldAmount];
                    resultArray = strResult.Split(';');
                    object obj = ConvertObject(list, resultArray);
                    result.Add(obj);
                }

                return result;
            }

            /// <summary>
            /// Method, that reads a List from Db
            /// </summary>
            /// <param name="list">string</param>
            /// <returns>List<object></returns>
            public object ReadPostFromDb(string list, int id)
        {
            object result = new object();
            string strResult;
            strResult = executor.ReadPostFromDataBase(list, id);

            string[] resultArray = new string[GetFieldAmount(list)];
            resultArray = strResult.Split(';');
            object obj = ConvertObject(list, resultArray);
            result = (obj);

            return result;
        }

            #endregion

            #region Update
            /// <summary>
            /// Method, that updates an entity in Db
            /// </summary>
            /// <param name="_object">object</param>
            /// <returns>bool</returns>
            public bool UpdateInDb(object _object)
                {
                    bool result = false;
                    string entityType = _object.GetType().ToString();

                    switch (entityType)
                    {
                        case "Contact":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Contacts", new Contact((Contact)_object)));
                            break;
                        case "SubEntrepeneur":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("SubEntrepeneurs", new SubEntrepeneur((SubEntrepeneur)_object)));
                            break;
                        default:
                            break;
                    }

                    return result;
                }

            /// <summary>
            /// Method, that returns a SQL-query
            /// Accepted lists:  Addresses, BluePrints, Contacts, ContactInfoList, CraftGroups, EnterpriseForms, EnterprisList, IttLetters, PdfDataList, Project, Ziptown
            /// </summary>
            /// <param name="list">string</param>
            /// <param name="Object">object</param>
            /// <returns></returns>
            private string GetSQLQueryUpdate(string list, object _object)
            {
                string result = "";

                switch (list)
                {
                    //UPDATE table_name SET column1 = value1, column2 = value2, ... WHERE condition;
                    case "Contacts":
                        Contact contact = new Contact((Contact)_object);
                        result = @"UPDATE [dbo].[Contacts] SET [Entity] = " + contact.Entity + " WHERE [Id] = " + contact.Id;
                        break;
                    case "SubEntrepeneurs":
                        SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)_object);
                        result = @"UPDATE [dbo].[SubEntrepeneurs] SET [Entrepeneur] = " + subEntrepeneur.Entrepeneur + " WHERE [Id] = " + subEntrepeneur.Id;
                        break;
                }

                return result;
            }

            #endregion

        #endregion

        /// <summary>
        /// Method, that fetches an entity from Id
        /// </summary>
        /// <param name="list"></param>
        /// <returns>object</returns>
        public object GetObject(string list, int id)
        {
            return ReadPostFromDb(list, id);
        }


        #region Refresh Lists
        /// <summary>
        /// Method, that refreshes all Lists
        /// </summary>
        public void RefreshAllLists()
        {
            RefreshLegalEntities();
            RefreshContacts();
            RefreshSubEntrepeneurs();

        }

        /// <summary>
        /// Method, that refreshes a list
        /// </summary>
        /// <param name="list">string</param>
        public void RefreshList(string list)
        {
            switch (list)
            {
                case "Contacts":
                    RefreshContacts();
                    break;
                case "LegalEntities":
                    RefreshLegalEntities();
                    break;
                case "SubEntrepeneurs":
                    RefreshSubEntrepeneurs();
                    break;
            }
        }

        /// <summary>
        /// Method, that refreshes the Contacts list
        /// </summary>
        private void RefreshContacts()
        {
            if (Contacts != null)
            {
                Contacts.Clear();
            }
            List<object> tempBluePrints = ReadListFromDb("Contacts");

            foreach (object obj in tempBluePrints)
            {
                Contacts.Add((Contact)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the LegalEntities list
        /// </summary>
        private void RefreshLegalEntities()
        {
            if (LegalEntities != null)
            {
                LegalEntities.Clear();
            }
            List<object> tempList = ReadListFromDb("LegalEntities");

            foreach (object obj in tempList)
            {
                LegalEntities.Add((LegalEntity)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the SubEntrepeneurs list
        /// </summary>
        private void RefreshSubEntrepeneurs()
        {
            if (SubEntrepeneurs != null)
            {
                SubEntrepeneurs.Clear();
            }
            List<object> tempList = ReadListFromDb("SubEntrepeneurs");

            foreach (object obj in tempList)
            {
                SubEntrepeneurs.Add((SubEntrepeneur)obj);
            }
        }

        #endregion

        #endregion

    }
}
