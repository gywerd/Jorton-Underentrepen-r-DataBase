using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;

namespace JudBizz
{
    public class Bizz
    {
        #region Fields

            #region Ordinary Fields
            public MyEntityFrameWork MEFW = new MyEntityFrameWork();
            public MacAddress macAddress = new MacAddress();
            public bool UcRightActive = false;


            public User CurrentUser = new User();
            public Address TempAddress => new Address();
            public Builder TempBuilder = new Builder();
            public Bullet TempBullet = new Bullet();
            public Contact TempContact = new Contact();
            public ContactInfo TempContactInfo = new ContactInfo();
            public Enterprise TempEnterprise = new Enterprise();
            public Entrepeneur TempEntrepeneur = new Entrepeneur();
            public IttLetter TempIttLetter = new IttLetter();
            public LegalEntity TempLegalEntity = new LegalEntity();
            public LetterData TempLetterData = new LetterData();
            public Offer TempOffer = new Offer();
            public Paragraph TempParagraph = new Paragraph();
            public Project TempProject = new Project();
            public Receiver TempReceiver = new Receiver();
            public Request TempRequest = new Request();
            public Shipping TempShipping = new Shipping();
            public SubEntrepeneur TempSubEntrepeneur;
            public User TempUser = new User();
            public ZipTown TempZipTown = new ZipTown();

        #endregion

            #region Indexed Lists
            public List<IndexedProject> IndexedActiveProjects = new List<IndexedProject>();
            public List<IndexedEnterpriseForm> IndexedEnterpriseForms = new List<IndexedEnterpriseForm>();
            public List<IndexedEnterprise> IndexedEnterprises = new List<IndexedEnterprise>();
            public List<IndexedEntrepeneur> IndexedEntrepeneurs = new List<IndexedEntrepeneur>();
            public List<IndexedParagraph> IndexedParagraphs = new List<IndexedParagraph>();
            public List<IndexedProject> IndexedProjects = new List<IndexedProject>();
            public List<IndexedProjectStatus> IndexedProjectStatuses = new List<IndexedProjectStatus>();
            public List<IndexedRequest> IndexedRequests = new List<IndexedRequest>();
            public List<IndexedRequestStatus> IndexedRequestStatuses = new List<IndexedRequestStatus>();
            public List<SubEntrepeneur> IndexedSubEntrepeneurs = new List<SubEntrepeneur>();
            public List<IndexedTenderForm> IndexedTenderForms = new List<IndexedTenderForm>();
            public List<IndexedUser> IndexedUsers = new List<IndexedUser>();
            #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Bizz()
        {
            RefreshAllInitialIndexedLists();
        }

        #endregion

        #region Methods

        #region Credentials
        /// <summary>
        /// Method, that checks credentials
        /// </summary>
        /// <param name="userName">TextBlock</param>
        /// <param name="menuItemChangePassWord">RibbonApplicationMenuItem</param>
        /// <param name="menuItemLogOut">RibbonApplicationMenuItem</param>
        /// <param name="initials">string</param>
        /// <param name="passWord">string</param>
        /// <returns>bool</returns>
        public bool CheckCredentials(TextBlock userName, RibbonApplicationMenuItem menuItemChangePassWord, RibbonApplicationMenuItem menuItemLogOut, string initials, string passWord)
        {
            foreach (User user in Users)
            {
                if (user.Initials == initials && user.Authentication.PassWord == passWord)
                {
                    CurrentUser = user;
                    userName.Text = user.Person.Name;
                    menuItemChangePassWord.IsEnabled = true;
                    menuItemLogOut.IsEnabled = true;
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Database

        /// <summary>
        /// Method, that creates a new entity in Db
        /// Accepts the following entityTypes: Address, BluePrintContact, 
        /// ContactInfo, CraftGroup, Enterprise, IttLetter, PdfData, 
        /// Project, Request
        /// </summary>
        /// <param name="entity">Object</param>
        /// <returns>int</returns>
        public int CreateInDb(object entity) => MEFW.CreateInDb(entity);


        /// <summary>
        /// Method, that reads a List from Db
        /// </summary>
        /// <param name="list">string</param>
        /// <returns>List<object></returns>
        public List<object> ReadListFromDb(string list) => MEFW.ReadListFromDb(list);

        /// <summary>
        /// Method, that updates an entity in Db
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        public bool UpdateInDb(object entity) => MEFW.UpdateInDb(entity);

        /// <summary>
        /// Method, that Deletes an entity from Db
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteFromDb(string list, string id) => MEFW.DeleteFromDb(list, id);

        #endregion


        #region Get
        /// <summary>
        /// Method, that fetches an entity from Id
        /// </summary>
        /// <param name="list">string</param>
        /// <param name="id">int</param>
        /// <returns>object</returns>
        public object GetObject(string list, int id) => MEFW.GetObject(list, id);

        #region Get Indexed Entrepeneurs
        /// <summary>
        /// Method, that checks, whether a Entrepeneur exists in Receivers list
        /// </summary>
        /// <param name="entrepeneur">IndexedEntrepeneur</param>
        /// <returns>bool</returns>
        private bool CheckEntrepeneurReceivers(IndexedEntrepeneur entrepeneur)
        {
            bool result = false;
            foreach (Receiver receiver in Receivers)
            {
                if (receiver.Cvr == entrepeneur.Entity.Cvr)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that checks, whether a Entrepeneur exists in tempResult list
        /// </summary>
        /// <param name="tempEntrepeneur">LegalEntity</param>
        /// <param name="sub">SubEntrepeneur</param>
        /// <param name="List<LegalEntity>"></param>
        /// <returns></returns>
        private bool CheckEntrepeneurTempResult(IndexedEntrepeneur tempEntrepeneur, List<IndexedEntrepeneur> tempResult)
        {
            bool exist = false;
            foreach (IndexedEntrepeneur entrepeneur in tempResult)
            {
                if (entrepeneur.Id == tempEntrepeneur.Id)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        /// <summary>
        /// Method that creates a list of Indexed Entrepeneurs
        /// </summary>
        /// <returns>List<IndexedEntrepeneur></returns>
        public void GetIndexedEntrepeneurs()
        {
            List<IndexedEntrepeneur> tempResult = new List<IndexedEntrepeneur>();
            RefreshList("Receivers");

            foreach (SubEntrepeneur sub in MEFW.PdfLists.SubEntrepeneurs)
            {
                foreach (IndexedEntrepeneur tempEntrepeneur in IndexedEntrepeneurs)
                {
                    if (tempEntrepeneur.Id == sub.Entrepeneur.Id)
                    {
                        bool exist = CheckEntrepeneurTempResult(tempEntrepeneur, tempResult);
                        if (!exist)
                        {
                            tempResult.Add(tempEntrepeneur);
                        }
                    }
                }
            }

            int i = 0;
            IndexedEntrepeneurs.Clear();

            foreach (IndexedEntrepeneur entrepeneur in tempResult)
            {
                bool exist = CheckEntrepeneurReceivers(entrepeneur);
                if (!exist)
                {
                    IndexedEntrepeneurs.Add(new IndexedEntrepeneur(i, entrepeneur));
                    i++;
                }
            }
        }

        #endregion

        /// <summary>
        /// Method, that retrieves the MacAddress
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress() => MEFW.ObtainMacAddress();

        /// <summary>
        /// Method, that retrieves the MacAddress
        /// </summary>
        /// <returns></returns>
        public string GetStrConnection() => MEFW.ObtainStrConnection();

        #endregion

        #region Refresh Indexed Lists
        /// <summary>
        /// Method, that refreshes all Indexed lists
        /// </summary>
        public void RefreshAllInitialIndexedLists()
        {
            RefreshIndexedActiveProjects();
            RefreshIndexedProjects();
            RefreshIndexedEnterpriseForms();
            RefreshIndexedProjectStatusList();
            RefreshIndexedRequestStatusList();
            RefreshIndexedTenderForms();
        }

        /// <summary>
        /// Method, that refreshes an Indexed list
        /// </summary>
        public void RefreshIndexedList(string list)
        {
            switch (list)
            {
                case "IndexedActiveProjects":
                    RefreshIndexedActiveProjects();
                    break;
                case "IndexedEnterpriseForms":
                    RefreshIndexedEnterpriseForms();
                    break;
                case "IndexedProjects":
                    RefreshIndexedProjects();
                    break;
                case "IndexedProjectStatusList":
                    RefreshIndexedProjectStatusList();
                    break;
                case "IndexedRequestStatusList":
                    RefreshIndexedRequestStatusList();
                    break;
                case "IndexedTenderForms":
                    RefreshIndexedTenderForms();
                    break;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Active Projects list
        /// </summary>
        private void RefreshIndexedActiveProjects()
        {
            IndexedActiveProjects.Clear();
            RefreshList("ActiveProjects");

            int i = 0;
            foreach (Project project in ActiveProjects)
            {
                IndexedActiveProjects.Add(new IndexedProject(i, project));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Enterprise Forms list
        /// </summary>
        private void RefreshIndexedEnterpriseForms()
        {
            IndexedEnterpriseForms.Clear();
            int i = 0;
            foreach (EnterpriseForm form in EnterpriseForms)
            {
                IndexedEnterpriseForms.Add(new IndexedEnterpriseForm(i, form));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Project Status list
        /// </summary>
        private void RefreshIndexedProjectStatusList()
        {
            IndexedProjectStatuses.Clear();
            int i = 0;
            foreach (ProjectStatus status in ProjectStatuses)
            {
                IndexedProjectStatuses.Add(new IndexedProjectStatus(i, status));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Projects list
        /// </summary>
        private void RefreshIndexedProjects()
        {
            IndexedProjects.Clear();
            int i = 0;
            foreach (Project project in ActiveProjects)
            {
                IndexedProjects.Add(new IndexedProject(i, project));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Request Status list
        /// </summary>
        private void RefreshIndexedRequestStatusList()
        {
            IndexedRequestStatuses.Clear();
            int i = 0;
            foreach (RequestStatus status in RequestStatuses)
            {
                IndexedRequestStatuses.Add(new IndexedRequestStatus(i, status));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Tender Forms list
        /// </summary>
        private void RefreshIndexedTenderForms()
        {
            IndexedTenderForms.Clear();
            int i = 0;
            foreach (TenderForm status in TenderForms)
            {
                IndexedTenderForms.Add(new IndexedTenderForm(i, status));
                i++;
            }
        }

        #endregion

        #region Refresh Ordinary Lists
        /// <summary>
        /// Method, that refreshes all Lists
        /// </summary>
        public void RefreshAllLists() => MEFW.RefreshAllLists();

        /// <summary>
        /// Method, that refreshes IttLetters list
        /// </summary>
        public void RefreshList(string list) => MEFW.RefreshList(list);

        #endregion

        public void RefreshLetterLists()
        {
            MEFW.RefreshIttLetterLists(TempProject.Id);
        }

        #endregion

        #region Properties
        public List<Project> ActiveProjects => MEFW.ActiveProjects;
        public List<Address> Addresses => MEFW.Addresses;
        public List<Builder> Builders => MEFW.Builders;
        public List<Bullet> Bullets => MEFW.Bullets;
        public List<Category> Categories => MEFW.Categories;
        public List<Contact> Contacts => MEFW.Contacts;
        public List<ContactInfo> ContactInfoList => MEFW.ContactInfoList;
        public List<CraftGroup> CraftGroups => MEFW.CraftGroups;
        public List<EnterpriseForm> EnterpriseForms => MEFW.EnterpriseForms;
        public List<Enterprise> Enterprises => MEFW.Enterprises;
        public List<Entrepeneur> Entrepeneurs => MEFW.Entrepeneurs;
        public List<Project> InactiveProjects => MEFW.InactiveProjects;
        public List<IttLetter> IttLetters => MEFW.IttLetters;
        public List<JobDescription> JobDescriptions => MEFW.JobDescriptions;
        public List<LegalEntity> LegalEntities => MEFW.LegalEntities;
        public List<LetterData> LetterDataList => MEFW.LetterDataList;
        public List<Offer> Offers => MEFW.Offers;
        public List<Paragraph> Paragraphs => MEFW.Paragraphs;
        public List<Person> Persons => MEFW.Persons;
        public List<Project> Projects => MEFW.Projects;
        public List<ProjectStatus> ProjectStatuses => MEFW.ProjectStatuses;
        public List<Region> Regions => MEFW.Regions;
        public List<Request> Requests => MEFW.Requests;
        public List<RequestStatus> RequestStatuses => MEFW.RequestStatuses;
        public List<Receiver> Receivers => MEFW.Receivers;
        public List<Shipping> Shippings => MEFW.Shippings;
        public List<SubEntrepeneur> SubEntrepeneurs => MEFW.SubEntrepeneurs;
        public List<TenderForm> TenderForms => MEFW.TenderForms;
        public List<UserLevel> UserLevels => MEFW.UserLevels;
        public List<User> Users => MEFW.Users;
        public List<ZipTown> ZipTowns => MEFW.ZipTowns;


        #endregion
    }
}
