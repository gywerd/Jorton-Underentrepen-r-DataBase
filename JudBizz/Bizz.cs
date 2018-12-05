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
            public static MyEntityFrameWork MEFW = new MyEntityFrameWork();
            public static string macAddress;


            public User CurrentUser = new User();
            public Address TempAddress = new Address();
            public Builder TempBuilder = new Builder();
            public Contact TempContact = new Contact();
            public ContactInfo TempContactInfo = new ContactInfo();
            public Enterprise TempEnterprise = new Enterprise();
            public IttLetter TempIttLetter = new IttLetter();
            public IttLetterBullet TempIttLetterBullet = new IttLetterBullet();
            public IttLetterParagraph TempIttLetterParagraph = new IttLetterParagraph();
            public IttLetterPdfData TempIttLetterPdfData = new IttLetterPdfData();
            public IttLetterReceiver TempIttLetterReceiver = new IttLetterReceiver();
            public IttLetterShipping TempIttLetterShipping = new IttLetterShipping();
            public LegalEntity TempLegalEntity = new LegalEntity();
            public Offer TempOffer = new Offer();
            public Project TempProject = new Project();
            public Request TempRequest = new Request();
            public SubEntrepeneur TempSubEntrepeneur;
            public ZipTown TempZipTown = new ZipTown();
            public bool UcRightActive = false;

            #endregion

            #region Ordinary Lists
            public List<Address> Addresses = MEFW.Addresses;
            public List<BluePrint> BluePrints = MEFW.BluePrints;
            public List<Builder> Builders = MEFW.Builders;
            public List<Category> Categories = MEFW.Categories;
            public List<Contact> Contacts = MEFW.Contacts;
            public List<ContactInfo> ContactInfoList = MEFW.ContactInfoList;
            public List<CraftGroup> CraftGroups = MEFW.CraftGroups;
            public List<Description> Descriptions = MEFW.Descriptions;
            public List<Enterprise> EnterpriseList = MEFW.EnterpriseList;
            public List<EnterpriseForm> EnterpriseForms = MEFW.EnterpriseForms;
            public List<IttLetter> IttLetters = MEFW.IttLetters;
            public List<IttLetterBullet> IttLetterBullets = MEFW.IttLetterBullets;
            public List<IttLetterParagraph> IttLetterParagraphs = MEFW.IttLetterParagraphs;
            public List<IttLetterPdfData> IttLetterPdfDataList = MEFW.IttLetterPdfDataList;
            public List<IttLetterReceiver> IttLetterReceivers = MEFW.IttLetterReceivers;
            public List<IttLetterShipping> IttLetterShippingList = MEFW.IttLetterShippingList;
            public List<JobDescription> JobDescriptions = MEFW.JobDescriptions;
            public List<LegalEntity> LegalEntities = MEFW.LegalEntities;
            public List<Miscellaneous> MiscellaneousList = MEFW.MiscellaneousList;
            public List<Offer> Offers = MEFW.Offers;
            public List<Project> Projects = MEFW.Projects;
            public List<ProjectStatus> ProjectStatusList = MEFW.ProjectStatusList;
            public List<Region> Regions = MEFW.Regions;
            public List<Request> Requests = MEFW.Requests;
            public List<RequestStatus> RequestStatusList = MEFW.RequestStatusList;
            public List<SubEntrepeneur> SubEntrepeneurs = MEFW.SubEntrepeneurs;
            public List<TenderForm> TenderForms = MEFW.TenderForms;
            public List<TimeSchedule> TimeSchedules = MEFW.TimeSchedules;
            public List<User> Users = MEFW.Users;
            public List<ZipTown> ZipTownList = MEFW.ZipTownList;

            #endregion

            #region Indexable Lists
            public List<IndexedProject> IndexedActiveProjects = new List<IndexedProject>();
            public List<IndexedEnterpriseForm> IndexedEnterpriseForms = new List<IndexedEnterpriseForm>();
            public List<IndexedProject> IndexedProjects = new List<IndexedProject>();
            public List<IndexedProjectStatus> IndexedProjectStatusList = new List<IndexedProjectStatus>();
            public List<IndexedRequestStatus> IndexedRequestStatusList = new List<IndexedRequestStatus>();
            public List<IndexedTenderForm> IndexedTenderForms = new List<IndexedTenderForm>();
            public List<IndexedUser> IndexableUsers = new List<IndexedUser>();
            #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Bizz()
        {
            RefreshAllIndexedLists();
            macAddress = GetMacAddress();
        }

        #endregion

        #region Methods

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
                if (user.Initials == initials && user.PassWord == passWord)
                {
                    CurrentUser = user;
                    userName.Text = user.Name;
                    menuItemChangePassWord.IsEnabled = true;
                    menuItemLogOut.IsEnabled = true;
                    return true;
                }
            }

            return false;
        }

        #region Database

        #region Create

        public bool CreateInDbReturnBool(object entity)
        {
            return MEFW.CreateInDbReturnBool(entity);
        }

        public int CreateInDbReturnInt(object entity)
        {
            return MEFW.CreateInDbReturnInt(entity);
        }

        #endregion

        #region Read

        #endregion

        #region Update In Database
        public bool UpdateInDb(object entity)
        {
            return MEFW.UpdateInDb(entity);
        }

        #endregion

        #region Delete
        /// <summary>
        /// Method, that Deletes an entity from Db
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteFromDb(string list, string id)
        {
            return MEFW.DeleteFromDb(list, id);
        }
        #endregion


        #endregion

        /// <summary>
        /// Method, that fetches an entity from Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>object</returns>
        public object GetEntity(string entity, string id)
        {
            return MEFW.GetEntity(entity, id);
        }

        /// <summary>
        /// Method, that retrieves the MacAddress
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress()
        {
            return MEFW.ObtainMacAddress();
        }

        /// <summary>
        /// Method, that retrieves the MacAddress
        /// </summary>
        /// <returns></returns>
        public string GetStrConnection()
        {
            return MEFW.ObtainStrConnection();
        }

        #region Refresh Indexed Lists
        /// <summary>
        /// Method, that refreshes all Indexed lists
        /// </summary>
        public void RefreshAllIndexedLists()
        {
            RefreshIndexedActiveProjects();
            RefreshIndexedProjects();
            RefreshIndexedEnterpriseForms();
            RefreshIndexedProjectStatusList();
            RefreshIndexedRequestStatusList();
        }

        /// <summary>
        /// Method, that refreshes all Indexed lists
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
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Active Projects list
        /// </summary>
        private void RefreshIndexedActiveProjects()
        {
            IndexedActiveProjects.Clear();
            int i = 0;
            foreach (Project project in Projects)
            {
                if (project.Status.Id == 1)
                {
                    IndexedActiveProjects.Add(new IndexedProject(i, project));
                    i++;
                }
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
            IndexedProjectStatusList.Clear();
            int i = 0;
            foreach (ProjectStatus status in ProjectStatusList)
            {
                IndexedProjectStatusList.Add(new IndexedProjectStatus(i, status));
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
            foreach (Project project in Projects)
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
            IndexedRequestStatusList.Clear();
            int i = 0;
            foreach (RequestStatus status in RequestStatusList)
            {
                IndexedRequestStatusList.Add(new IndexedRequestStatus(i, status));
                i++;
            }
        }

        #endregion

        #region Refresh Ordinary Lists
        /// <summary>
        /// Method, that refreshes all Lists
        /// </summary>
        public void RefreshAllLists()
        {
            MEFW.RefreshAllLists();
        }

        /// <summary>
        /// Method, that refreshes IttLetters ist
        /// </summary>
        public void RefreshList(string list)
        {
            MEFW.RefreshList(list);
        }

        #endregion

        #endregion

        #region Properties
        public static string MacAdresss { get; }

        #endregion
    }
}
