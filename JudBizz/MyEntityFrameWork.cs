﻿using JudDataAccess;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JudBizz
{
    public class MyEntityFrameWork
    {
        #region Fields
        private static string strConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JortonSubEnt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public ProjectLists ProjectLists;
        private Executor executor = new Executor(strConnection);
        private MacAddress macAddress = new MacAddress();
        private string appPath = "";
        public DateTime oldDate = Convert.ToDateTime("1932-03-17");


        public User CurrentUser = new User();
        public Address TempAddress = new Address();
        public Builder TempBuilder = new Builder();
        public Bullet TempBullet = new Bullet();
        public Category TempCategory = new Category();
        public CraftGroup TempCraftGroup = new CraftGroup();
        public Contact TempContact = new Contact();
        public ContactInfo TempContactInfo = new ContactInfo();
        public Enterprise TempEnterprise = new Enterprise();
        public EnterpriseForm TempEnterpriseForm = new EnterpriseForm();
        public Entrepeneur TempEntrepeneur = new Entrepeneur();
        public IttLetter TempIttLetter = new IttLetter();
        public IttLetterShipping TempIttLetterShipping = new IttLetterShipping();
        public JobDescription TempJobDescription = new JobDescription();
        public LegalEntity TempLegalEntity = new LegalEntity();
        public LetterData TempLetterData = new LetterData();
        public Offer TempOffer = new Offer();
        public Paragraf TempParagraf = new Paragraf();
        public string TempPassWord = "";
        public Project TempProject = new Project();
        public ProjectDetail TempProjectDetail = new ProjectDetail();
        public ProjectStatus TempProjectStatus = new ProjectStatus();
        public RequestShipping TempRequestShipping = new RequestShipping();
        public Receiver TempReceiver = new Receiver();
        public Region TempRegion = new Region();
        public Request TempRequest = new Request();
        public SubEntrepeneur TempSubEntrepeneur;
        public TenderForm TempTenderForm = new TenderForm();
        public User TempUser = new User();
        public ZipTown TempZipTown = new ZipTown();



            #region Lists
            public List<Builder> ActiveBuilders = new List<Builder>();
            public List<Entrepeneur> ActiveEntrepeneurs = new List<Entrepeneur>();
            public List<Project> ActiveProjects = new List<Project>();
            public List<User> ActiveUsers = new List<User>();
            public List<Address> Addresses = new List<Address>();
            public List<Builder> Builders = new List<Builder>();
            public List<Bullet> Bullets = new List<Bullet>();
            public List<Category> Categories = new List<Category>();
            public List<Contact> Contacts = new List<Contact>();
            public List<ContactInfo> ContactInfoList = new List<ContactInfo>();
            public List<CraftGroup> CraftGroups = new List<CraftGroup>();
            public List<EnterpriseForm> EnterpriseForms = new List<EnterpriseForm>();
            public List<Enterprise> Enterprises = new List<Enterprise>();
            public List<Entrepeneur> Entrepeneurs = new List<Entrepeneur>();
            public List<Builder> InactiveBuilders = new List<Builder>();
            public List<Entrepeneur> InactiveEntrepeneurs = new List<Entrepeneur>();
            public List<Project> InactiveProjects = new List<Project>();
            public List<User> InactiveUsers = new List<User>();
            public List<IttLetter> IttLetters = new List<IttLetter>();
            public List<IttLetterShipping> IttLetterShippings = new List<IttLetterShipping>();
            public List<JobDescription> JobDescriptions = new List<JobDescription>();
            public List<LegalEntity> LegalEntities = new List<LegalEntity>();
            public List<LetterData> LetterDataList = new List<LetterData>();
            public List<Offer> Offers = new List<Offer>();
            public List<Paragraf> Paragrafs = new List<Paragraf>();
            public List<Person> Persons = new List<Person>();
            public List<ProjectDetail> ProjectDetails = new List<ProjectDetail>();
            public List<Project> Projects = new List<Project>();
            public List<ProjectStatus> ProjectStatuses = new List<ProjectStatus>();
            public List<Receiver> Receivers = new List<Receiver>();
            public List<Region> Regions = new List<Region>();
            public List<Request> Requests = new List<Request>();
            public List<RequestShipping> RequestShippings = new List<RequestShipping>();
            public List<RequestStatus> RequestStatuses = new List<RequestStatus>();
            public List<SubEntrepeneur> SubEntrepeneurs = new List<SubEntrepeneur>();
            public List<TenderForm> TenderForms = new List<TenderForm>();
            public List<UserLevel> UserLevels = new List<UserLevel>();
            public List<User> Users = new List<User>();
            public List<ZipTown> ZipTowns = new List<ZipTown>();

            #endregion

        #endregion

        #region Constructors
        public MyEntityFrameWork()
        {
            appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            appPath = appPath.Remove(appPath.Count() - 11, 11);

            RefreshAllLists();
        }

        #endregion

        #region Properties
        public string AppPath
        {
            get => appPath;
            set
            {
                try
                {
                    appPath = value;
                }
                catch (Exception)
                {
                    appPath = "";
                }
            }
        }

        public string MacAddress { get => macAddress.ToString(); }

        public string StrConnection { get => strConnection.ToString(); }

        #endregion

        #region Methods

        #region DataBase

        /// <summary>
        /// Method, that processes an SQL-query
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool ProcesSqlQuery(string strSql) => executor.WriteToDataBase(strSql);

        #region Create
        /// <summary>
        /// Method, that adds a User to Db
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="passWord">string</param>
        /// <returns>bool</returns>
        private bool AddUser(User user, string passWord)
        {
            return executor.AddUser(user.Person.Id, user.Initials, user.Department.Id, passWord, user.JobDescription.Id, user.UserLevel.Id);
        }

        /// <summary>
        /// Method, that checks wether an Address exists in Addresses
        /// </summary>
        /// <param name="tempAddress">Address</param>
        /// <returns>bool</returns>
        private bool CheckAddressExist(Address tempAddress)
        {
            bool result = false;

            RefreshAddresses();

            foreach (Address address in Addresses)
            {
                if (address.Id == tempAddress.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        /// <summary>
        /// Method, that checks wether an Builder exists in Builders
        /// </summary>
        /// <param name="tempBuilder">Builder</param>
        /// <returns>bool</returns>
        private bool CheckBuilderExist(Builder tempBuilder)
        {
            bool result = false;

            foreach (Builder builder in Builders)
            {
                if (builder.Id == tempBuilder.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        /// <summary>
        /// Method, that checks wether a ContactInfo exists in ContactInfoList
        /// </summary>
        /// <param name="tempContactInfo">ContactInfo</param>
        /// <returns>bool</returns>
        private bool CheckContactInfoExist(ContactInfo tempContactInfo)
        {
            bool result = false;

            foreach (ContactInfo info in ContactInfoList)
            {
                if (info.Id == tempContactInfo.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a Entrepeneur exists in Entrepeneurs
        /// </summary>
        /// <param name="tempPerson">Person</param>
        /// <returns>bool</returns>
        private bool CheckEntrepeneurExist(Entrepeneur tempEntrepeneur)
        {
            bool result = false;

            foreach (Entrepeneur entrepeneur in Entrepeneurs)
            {
                if (entrepeneur.Id == tempEntrepeneur.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        /// <summary>
        /// Method, that checks wether a LegalEntity exists in LegalEntities
        /// </summary>
        /// <param name="tempEntity">LegalEntity</param>
        /// <returns>bool</returns>
        private bool CheckLegalEntityExist(LegalEntity tempEntity)
        {
            bool result = false;

            foreach (LegalEntity entity in LegalEntities)
            {
                if (entity.Id == tempEntity.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a LetterData exists in LetterDataList
        /// </summary>
        /// <param name="tempEntity">LetterData</param>
        /// <returns>bool</returns>
        private bool CheckLetterDataExist(LetterData tempData)
        {
            bool result = false;

            foreach (LetterData data in LetterDataList)
            {
                if (data.Id == tempData.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a Paragraf exists in Paragrafs
        /// </summary>
        /// <param name="tempParagraf">Paragraf</param>
        /// <returns>bool</returns>
        private bool CheckParagrafExist(Paragraf tempParagraf)
        {
            bool result = false;

            foreach (Paragraf paragraf in Paragrafs)
            {
                if (paragraf.Id == tempParagraf.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a Person exists in Persons
        /// </summary>
        /// <param name="tempPerson">Person</param>
        /// <returns>bool</returns>
        private bool CheckPersonExist(Person tempPerson)
        {
            bool result = false;

            foreach (Person person in Persons)
            {
                if (person.Id == tempPerson.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a Project exists in Projects
        /// </summary>
        /// <param name="tempProject">Project</param>
        /// <returns>bool</returns>
        private bool CheckProjectExist(Project tempProject)
        {
            bool result = false;

            foreach (Project project in Projects)
            {
                if (project.Id == tempProject.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a ProjectDetail exists in ProjectDetails
        /// </summary>
        /// <param name="tempDetail">Person</param>
        /// <returns>bool</returns>
        private bool CheckProjectDetailExist(ProjectDetail tempDetail)
        {
            bool result = false;

            foreach (ProjectDetail detail in ProjectDetails)
            {
                if (detail.Id == tempDetail.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a Receiver exists in Receivers
        /// </summary>
        /// <param name="tempReceiver">Receiver</param>
        /// <returns>bool</returns>
        private bool CheckReceiverExist(Receiver tempReceiver)
        {
            bool result = false;

            foreach (Receiver receiver in Receivers)
            {
                if (receiver.Id == tempReceiver.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a SubEntrepeneur exists in SubEntrepeneurs
        /// </summary>
        /// <param name="tempSubEntrepeneur">SubEntrepeneur</param>
        /// <returns>bool</returns>
        private bool CheckSubEntrepeneurExist(SubEntrepeneur tempSubEntrepeneur)
        {
            bool result = false;

            foreach (SubEntrepeneur subEntrepeneur in SubEntrepeneurs)
            {
                if (subEntrepeneur.Id == tempSubEntrepeneur.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks wether a User exists in Users
        /// </summary>
        /// <param name="tempUser">User</param>
        /// <returns>bool</returns>
        private bool CheckUserExist(User tempUser)
        {
            bool result = false;

            foreach (User user in Users)
            {
                if (user.Id == tempUser.Id)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that creates a Bullet
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateBullet(object entity)
        {
            bool result = false;

            Bullet tempBullet = new Bullet((Bullet)entity);

            if (tempBullet.Paragraf.Id != 0 && CheckParagrafExist(tempBullet.Paragraf))
            {
                UpdateInDb(tempBullet.Paragraf);
            }
            else
            {
                tempBullet.Paragraf.SetId(CreateInDb(tempBullet.Paragraf));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Builders", tempBullet));

            return result;
        }

        /// <summary>
        /// Method, that creates a Builder
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateBuilder(object entity)
        {
            bool result = false;

            Builder tempBuilder = new Builder((Builder)entity);

            if (tempBuilder.Entity.Id != 0 && CheckLegalEntityExist(tempBuilder.Entity))
            {
                UpdateInDb(tempBuilder.Entity);
            }
            else
            {
                tempBuilder.Entity.SetId(CreateInDb(tempBuilder.Entity));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Builders", tempBuilder));

            return result;
        }

        /// <summary>
        /// Method, that creates a Contact
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateContact(object entity)
        {
            bool result = false;

            Contact tempContact = new Contact((Contact)entity);

            if (tempContact.Person.Id != 0 && CheckPersonExist(tempContact.Person))
            {
                UpdateInDb(tempContact.Person);
            }
            else
            {
                tempContact.Person.SetId(CreateInDb(tempContact.Person));
            }

            if (tempContact.Entrepeneur.Id != 0 && CheckEntrepeneurExist(tempContact.Entrepeneur))
            {
                UpdateInDb(tempContact.Entrepeneur);
            }
            else
            {
                tempContact.Entrepeneur.SetId(CreateInDb(tempContact.Entrepeneur));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Contacts", tempContact));

            return result;
        }

        /// <summary>
        /// Method, that creates an Enterprise
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateEnterprise(object entity)
        {
            bool result = false;

            Enterprise tempEnterprise = new Enterprise((Enterprise)entity);

            if (tempEnterprise.Project.Id != 0 && CheckProjectExist(tempEnterprise.Project))
            {
                UpdateInDb(tempEnterprise.Project);
            }
            else
            {
                tempEnterprise.Project.SetId(CreateInDb(tempEnterprise.Project));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Enterprises", tempEnterprise));

            return result;
        }

        /// <summary>
        /// Method, that creates an Entrepeneur
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateEntrepeneur(object entity)
        {
            bool result = false;

            Entrepeneur tempEntrepeneur = new Entrepeneur((Entrepeneur)entity);

            if (tempEntrepeneur.Entity.Id != 0 && CheckLegalEntityExist(tempEntrepeneur.Entity))
            {
                UpdateInDb(tempEntrepeneur.Entity);
            }
            else
            {
                tempEntrepeneur.Entity.SetId(CreateInDb(tempEntrepeneur.Entity));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Entrepeneurs", tempEntrepeneur));

            return result;
        }

        /// <summary>
        /// Method, that creates an IttLetter Shipping
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateIttLetterShipping(object entity)
        {
            bool result = false;

            IttLetterShipping tempShipping = new IttLetterShipping((IttLetterShipping)entity);

            if (tempShipping.SubEntrepeneur.Id != 0 && CheckSubEntrepeneurExist(tempShipping.SubEntrepeneur))
            {
                UpdateInDb(tempShipping.SubEntrepeneur);
            }
            else
            {
                tempShipping.SubEntrepeneur.SetId(CreateInDb(tempShipping.SubEntrepeneur));
            }

            if (tempShipping.Receiver.Id != 0 && CheckReceiverExist(tempShipping.Receiver))
            {
                UpdateInDb(tempShipping.Receiver);
            }
            else
            {
                tempShipping.Receiver.SetId(CreateInDb(tempShipping.Receiver));
            }

            if (tempShipping.LetterData.Id != 0 && CheckLetterDataExist(tempShipping.LetterData))
            {
                UpdateInDb(tempShipping.LetterData);
            }
            else
            {
                tempShipping.LetterData.SetId(CreateInDb(tempShipping.LetterData));
            }

            ProcesSqlQuery(GetSQLQueryInsert("IttLetterShippings", tempShipping));

            return result;
        }

        /// <summary>
        /// Method, that creates a new entity in Db
        /// </summary>
        /// <param name="entity">Object</param>
        /// <returns>int</returns>
        public int CreateInDb(object entity)
        {
            int result = 0;
            int count = 0;
            string entityTypeDk = "";
            bool dbAnswer = false;
            string entityType = entity.GetType().ToString().Remove(0, 14);
            string list = GetListNameFromEntityType(entityType);

            switch (list)
            {
                case "Bullets":
                    dbAnswer = CreateBullet(entity);
                    break;
                case "Builders":
                    dbAnswer = CreateBuilder(entity);
                    break;
                case "Contacts":
                    dbAnswer = CreateContact(entity);
                    break;
                case "Enterprises":
                    dbAnswer = CreateEnterprise(entity);
                    break;
                case "Entrepeneurs":
                    dbAnswer = CreateEntrepeneur(entity);
                    break;
                case "IttLetterShippings":
                    dbAnswer = CreateIttLetterShipping(entity);
                    break;
                case "LegalEntities":
                    dbAnswer = CreateLegalEntity(entity);
                    break;
                case "Paragrafs":
                    dbAnswer = CreateParagraf(entity);
                    break;
                case "Persons":
                    dbAnswer = CreatePerson(entity);
                    break;
                case "Projects":
                    dbAnswer = CreateProject(entity);
                    break;
                case "RequestShippings":
                    dbAnswer = CreateRequestShipping(entity);
                    break;
                case "SubEntrepeneurs":
                    dbAnswer = CreateSubEntrepeneur(entity);
                    break;
                case "Users":
                    dbAnswer = CreateUser(entity);
                    break;
                default:
                    dbAnswer = ProcesSqlQuery(GetSQLQueryInsert(list, entity));
                    break;
            }

            entityTypeDk = GetDanishEntityType(entityType);

            if (!dbAnswer)
            {
                MessageBox.Show("Databasen returnerede en fejl ved forsøg på at oprette ny " + entityTypeDk + ".", "Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                switch (entityType)
                {
                    case "Address":
                        RefreshList("Addresses");
                        count = Addresses.Count;
                        result = Addresses[count - 1].Id;
                        break;
                    case "Builder":
                        RefreshList("Builders");
                        count = Builders.Count;
                        result = Builders[count - 1].Id;
                        break;
                    case "Bullet":
                        RefreshList("Bullets");
                        count = Bullets.Count;
                        result = Bullets[count - 1].Id;
                        break;
                    case "Category":
                        RefreshList("Categories");
                        count = Categories.Count;
                        result = Categories[count - 1].Id;
                        break;
                    case "Contact":
                        RefreshList("Contacts");
                        count = Contacts.Count;
                        result = Contacts[count - 1].Id;
                        break;
                    case "ContactInfo":
                        RefreshList("Contactinfolist");
                        count = ContactInfoList.Count;
                        result = ContactInfoList[count - 1].Id;
                        break;
                    case "CraftGroup":
                        RefreshList("CraftGroups");
                        count = CraftGroups.Count;
                        result = CraftGroups[count - 1].Id;
                        break;
                    case "Enterprise":
                        RefreshList("Enterprises");
                        count = Enterprises.Count;
                        result = Enterprises[count - 1].Id;
                        break;
                    case "EnterpriseForm":
                        RefreshList("EnterpriseForms");
                        count = EnterpriseForms.Count;
                        result = EnterpriseForms[count - 1].Id;
                        break;
                    case "Entrepeneur":
                        RefreshList("Entrepeneurs");
                        count = Entrepeneurs.Count;
                        result = Entrepeneurs[count - 1].Id;
                        break;
                    case "IttLetter":
                        RefreshList("IttLetters");
                        count = IttLetters.Count;
                        result = IttLetters[count - 1].Id;
                        break;
                    case "IttLetterShipping":
                        RefreshList("IttLetterShippings");
                        count = IttLetterShippings.Count;
                        result = IttLetterShippings[count - 1].Id;
                        break;
                    case "JobDescription":
                        RefreshList("JobDescriptions");
                        count = JobDescriptions.Count;
                        result = JobDescriptions[count - 1].Id;
                        break;
                    case "LegalEntity":
                        RefreshList("LegalEntities");
                        count = LegalEntities.Count;
                        result = LegalEntities[count - 1].Id;
                        break;
                    case "LetterData":
                        RefreshList("LetterDataList");
                        count = LetterDataList.Count;
                        result = LetterDataList[count - 1].Id;
                        break;
                    case "Offer":
                        RefreshList("Offers");
                        count = Offers.Count;
                        result = Offers[count - 1].Id;
                        break;
                    case "Paragraf":
                        RefreshList("Paragrafs");
                        count = Paragrafs.Count;
                        result = Paragrafs[count - 1].Id;
                        break;
                    case "Project":
                        RefreshList("Projects");
                        count = Projects.Count;
                        result = Projects[count - 1].Id;
                        break;
                    case "ProjectDetail":
                        RefreshList("ProjectDetails");
                        count = ProjectDetails.Count;
                        result = ProjectDetails[count - 1].Id;
                        break;
                    case "ProjectStatus":
                        RefreshList("ProjectStatuses");
                        count = ProjectStatuses.Count;
                        result = ProjectStatuses[count - 1].Id;
                        break;
                    case "Receiver":
                        RefreshList("Receivers");
                        count = Receivers.Count;
                        result = Receivers[count - 1].Id;
                        break;
                    case "Region":
                        RefreshList("Regions");
                        count = Regions.Count;
                        result = Regions[count - 1].Id;
                        break;
                    case "Request":
                        RefreshList("Requests");
                        count = Requests.Count;
                        result = Requests[count - 1].Id;
                        break;
                    case "RequestShipping":
                        RefreshList("RequestShippings");
                        count = RequestShippings.Count;
                        result = RequestShippings[count - 1].Id;
                        break;
                    case "RequestStatus":
                        RefreshList("RequestStatuses");
                        count = RequestStatuses.Count;
                        result = RequestStatuses[count - 1].Id;
                        break;
                    case "SubEntrepeneur":
                        RefreshList("SubEntrepeneurs");
                        count = SubEntrepeneurs.Count;
                        result = SubEntrepeneurs[count - 1].Id;
                        break;
                    case "TenderForm":
                        RefreshList("TenderForms");
                        count = TenderForms.Count;
                        result = TenderForms[count - 1].Id;
                        break;
                    case "User":
                        RefreshList("Users");
                        count = Users.Count;
                        result = Users[count - 1].Id;
                        break;
                    case "UserLevel":
                        RefreshList("UserLevels");
                        count = UserLevels.Count;
                        result = UserLevels[count - 1].Id;
                        break;
                    case "ZipTown":
                        RefreshList("ZipTowns");
                        count = ZipTowns.Count;
                        result = ZipTowns[count - 1].Id;
                        break;
                }

            }

            return result;
        }

        /// <summary>
        /// Method, that creates a Contact
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateLegalEntity(object entity)
        {
            bool result = false;

            LegalEntity tempLegalEntity = new LegalEntity((LegalEntity)entity);

            if (tempLegalEntity.Address.Id != 0 && CheckAddressExist(tempLegalEntity.Address))
            {
                UpdateInDb(tempLegalEntity.Address);
            }
            else
            {
                tempLegalEntity.Address.SetId(CreateInDb(tempLegalEntity.Address));
            }

            if (tempLegalEntity.ContactInfo.Id != 0 && CheckContactInfoExist(tempLegalEntity.ContactInfo))
            {
                UpdateInDb(tempLegalEntity.ContactInfo);
            }
            else
            {
                tempLegalEntity.ContactInfo.SetId(CreateInDb(tempLegalEntity.ContactInfo));
            }

            ProcesSqlQuery(GetSQLQueryInsert("LegalEntities", tempLegalEntity));

            return result;
        }

        /// <summary>
        /// Method, that creates a Paragraf
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateParagraf(object entity)
        {
            bool result = false;

            Paragraf tempParagraf = new Paragraf((Paragraf)entity);

            if (tempParagraf.Project.Id != 0 && CheckProjectExist(tempParagraf.Project))
            {
                UpdateInDb(tempParagraf.Project);
            }
            else
            {
                tempParagraf.Project.SetId(CreateInDb(tempParagraf.Project));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Paragrafs", tempParagraf));

            return result;
        }

        /// <summary>
        /// Method, that creates a Person
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreatePerson(object entity)
        {
            bool result = false;

            Person tempPerson = new Person((Person)entity);

            if (tempPerson.ContactInfo.Id != 0 && CheckContactInfoExist(tempPerson.ContactInfo))
            {
                UpdateInDb(tempPerson.ContactInfo);
            }
            else
            {
                tempPerson.ContactInfo.SetId(CreateInDb(tempPerson.ContactInfo));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Persons", tempPerson));

            return result;
        }

        /// <summary>
        /// Method, that creates a Project
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateProject(object entity)
        {
            bool result = false;

            Project tempProject = new Project((Project)entity);

            if (tempProject.Builder.Id != 0 && CheckBuilderExist(tempProject.Builder))
            {
                UpdateInDb(tempProject.Builder);
            }
            else
            {
                tempProject.Builder.SetId(CreateInDb(tempProject.Builder));
            }

            if (tempProject.Executive.Id != 0 && CheckUserExist(tempProject.Executive))
            {
                UpdateInDb(tempProject.Executive);
            }
            else
            {
                tempProject.Executive.SetId(CreateInDb(tempProject.Executive));
            }

            if (tempProject.Details.Id != 0 && CheckProjectDetailExist(tempProject.Details))
            {
                UpdateInDb(tempProject.Details);
            }
            else
            {
                tempProject.Details.SetId(CreateInDb(tempProject.Details));
            }

            ProcesSqlQuery(GetSQLQueryInsert("Projects", tempProject));

            return result;
        }

        /// <summary>
        /// Method, that creates an Request Shipping
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateRequestShipping(object entity)
        {
            bool result = false;

            RequestShipping tempShipping = new RequestShipping((RequestShipping)entity);

            if (tempShipping.SubEntrepeneur.Id != 0 && CheckSubEntrepeneurExist(tempShipping.SubEntrepeneur))
            {
                UpdateInDb(tempShipping.SubEntrepeneur);
            }
            else
            {
                tempShipping.SubEntrepeneur.SetId(CreateInDb(tempShipping.SubEntrepeneur));
            }

            if (tempShipping.Receiver.Id != 0 && CheckReceiverExist(tempShipping.Receiver))
            {
                UpdateInDb(tempShipping.Receiver);
            }
            else
            {
                tempShipping.Receiver.SetId(CreateInDb(tempShipping.Receiver));
            }

            ProcesSqlQuery(GetSQLQueryInsert("RequestShippings", tempShipping));

            return result;
        }

        /// <summary>
        /// Method, that creates a SubEntrepeneur
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateSubEntrepeneur(object entity)
        {
            bool result = false;

            SubEntrepeneur tempSubEntrepeneur = new SubEntrepeneur((SubEntrepeneur)entity);

            if (tempSubEntrepeneur.Entrepeneur.Id != 0 && CheckEntrepeneurExist(tempSubEntrepeneur.Entrepeneur))
            {
                UpdateInDb(tempSubEntrepeneur.Entrepeneur);
            }
            else
            {
                tempSubEntrepeneur.Entrepeneur.SetId(CreateInDb(tempSubEntrepeneur.Entrepeneur));
            }

            ProcesSqlQuery(GetSQLQueryInsert("SubEntrepeneurs", tempSubEntrepeneur));

            return result;
        }

        /// <summary>
        /// Method, that creates a User
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        private bool CreateUser(object entity)
        {
            bool result = false;
            User tempUser = new User((User)entity);

            if (tempUser.Department.Id != 0 && CheckLegalEntityExist(tempUser.Department))
            {
                UpdateInDb(tempUser.Department);
            }
            else
            {
                tempUser.Department.SetId(CreateInDb(tempUser.Department));
            }

            result = AddUser(tempUser, TempPassWord);

            return result;
        }

        /// <summary>
        /// Method, that provide a danish translation of an entity type
        /// </summary>
        /// <param name="entityType">string</param>
        /// <returns>string</returns>
        private string GetDanishEntityType(string entityType)
        {
            string result = "";

            switch (entityType)
            {
                case "Address":
                    result = "Adresse";
                    break;
                case "Authentication":
                    result = "Adgang";
                    break;
                case "Builder":
                    result = "Bygherre";
                    break;
                case "Bullet":
                    result = "Punkt";
                    break;
                case "Contact":
                    result = "Kontakt";
                    break;
                case "ContactInfo":
                    result = "Kontaktinfo";
                    break;
                case "CraftGroup":
                    result = "Branche";
                    break;
                case "Enterprise":
                    result = "Entreprise";
                    break;
                case "EnterpriseForm":
                    result = "EntrepriseForm";
                    break;
                case "Entrepeneur":
                    result = "Entrepenør";
                    break;
                case "IttLetter":
                    result = "Udbudsbrev";
                    break;
                case "IttLetterShipping":
                    result = "Forsendelse af Udbudsbrev";
                    break;
                case "LegalEntity":
                    result = "Legal Person";
                    break;
                case "LetterData":
                    result = "Udbudbrevsdata";
                    break;
                case "Offer":
                    result = "Tilbud";
                    break;
                case "Paragraf":
                    result = "Afsnit";
                    break;
                case "Person":
                    result = "Person";
                    break;
                case "Project":
                    result = "Projekt";
                    break;
                case "ProjectDetail":
                    result = "Projektdetalje";
                    break;
                case "ProjectStatus":
                    result = "Projektstatus";
                    break;
                case "Receiver":
                    result = "Modtager";
                    break;
                case "Region":
                    result = "Landsdel";
                    break;
                case "Request":
                    result = "Forespørgsel";
                    break;
                case "RequestShipping":
                    result = "Forsendelse af Forespørgsel";
                    break;
                case "RequestStatus":
                    result = "Forespørgselsstatus";
                    break;
                case "SubEntrepeneur":
                    result = "Underentrepenør";
                    break;
                case "TenderForm":
                    result = "Udbudsform";
                    break;
                case "User":
                    result = "Bruger";
                    break;
                case "UserLevel":
                    result = "Brugerniveau";
                    break;
                case "ZipTown":
                    result = "Postnr. & By";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a List Name from an entity Type
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private string GetListNameFromEntityType(string entityType)
        {
            string result = "";

            switch (entityType)
            {
                case "Address":
                    result = "Addresses";
                    break;
                case "Authentication":
                    result = "Authentications";
                    break;
                case "Builder":
                    result = "Builders";
                    break;
                case "Bullet":
                    result = "Bullets";
                    break;
                case "Category":
                    result = "Categories";
                    break;
                case "Contact":
                    result = "Contacts";
                    break;
                case "ContactInfo":
                    result = "ContaktInfoList";
                    break;
                case "CraftGroup":
                    result = "CraftGroups";
                    break;
                case "Enterprise":
                    result = "Enterprises";
                    break;
                case "EnterpriseForm":
                    result = "EnterpriseForms";
                    break;
                case "Entrepeneur":
                    result = "Entrepeneurs";
                    break;
                case "IttLetter":
                    result = "IttLetters";
                    break;
                case "IttLetterShipping":
                    result = "IttLetterShippings";
                    break;
                case "JobDescription":
                    result = "JobDescriptions";
                    break;
                case "LegalEntity":
                    result = "LegalEntities";
                    break;
                case "LetterData":
                    result = "LetterDataList";
                    break;
                case "Offer":
                    result = "Offers";
                    break;
                case "Paragraf":
                    result = "Paragrafs";
                    break;
                case "Person":
                    result = "Persons";
                    break;
                case "Project":
                    result = "Projects";
                    break;
                case "ProjectStatus":
                    result = "ProjectStatuses";
                    break;
                case "Receiver":
                    result = "Receivers";
                    break;
                case "Region":
                    result = "Regions";
                    break;
                case "Request":
                    result = "Requests";
                    break;
                case "RequestData":
                    result = "RequestShippings";
                    break;
                case "RequestStatus":
                    result = "RequestStatuses";
                    break;
                case "SubEntrepeneur":
                    result = "SubEntrepeneurs";
                    break;
                case "TenderForms":
                    result = "TenderForms";
                    break;
                case "User":
                    result = "Users";
                    break;
                case "UserLevel":
                    result = "UserLevels";
                    break;
                case "ZipTown":
                    result = "ZipTowns";
                    break;
            }

            return result;
        }


        /// <summary>
        /// Method, that returns a SQL-query
        /// </summary>
        /// <param name="list">string</param>
        /// <param name="entity">object</param>
        /// <returns>string</returns>
        private string GetSQLQueryInsert(string list, object entity)
        {
            string result = "";

            switch (list)
            {
                //INSERT INTO table_name (column1, column2, column3, ...) VALUES(value1, value2, value3, ...);
                case "Addresses":
                    Address address = new Address((Address)entity);
                    result = @"INSERT INTO [dbo].[Addresses](Street, Place, Zip) VALUES('" + address.Street + @"', '" + address.Place + @"', " + address.ZipTown.Id + ")";
                    break;
                case "Builders":
                    Builder builder = new Builder((Builder)entity);
                    result = @"INSERT INTO [dbo].[Builders]([Entity], [Active]) VALUES(" + builder.Entity.Id + @", '" + builder.Active.ToString() + @"')";
                    break;
                case "Bullets":
                    Bullet bullet = new Bullet((Bullet)entity);
                    result = @"INSERT INTO [dbo].[Bullets]([Paragraf], [Text]) VALUES(" + bullet.Paragraf.Id + @", '" + bullet.Text + @"')";
                    break;
                case "Categories":
                    Category category = new Category((Category)entity);
                    result = "INSERT INTO [dbo].[Categories]([Text]) VALUES('" + category.Text + "')";
                    break;
                case "Contacts":
                    Contact contact = new Contact((Contact)entity);
                    result = "INSERT INTO [dbo].[Contacts]([Person], [Entrepeneur], [Area]) VALUES(" + contact.Person.Id + ", " + contact.Entrepeneur.Id + ", '" + contact.Area + "')";
                    break;
                case "ContactInfoList":
                    ContactInfo info = new ContactInfo((ContactInfo)entity);
                    result = "INSERT INTO [dbo].[ContactInfoList]([Phone], [Fax], [Mobile], [Email]) VALUES(" + info.Phone + ", '" + info.Fax + "', '" + info.Mobile + "', '" + info.Email + "')";
                    break;
                case "CraftGroups":
                    CraftGroup craftGroup = new CraftGroup((CraftGroup)entity);
                    result = @"INSERT INTO [dbo].[CraftGroups](Category, Designation, Description, Active) VALUES('" + craftGroup.Category.Id + @", '" + craftGroup.Designation + @"', '" + craftGroup.Description + @"', '" + craftGroup.Active.ToString() + "')";
                    break;
                case "EnterpriseForms":
                    EnterpriseForm form = new EnterpriseForm((EnterpriseForm)entity);
                    result = @"INSERT INTO [dbo].[EnterpriseForms](Id, Name) VALUES(" + form.Id + @", '" + form.Text + "')";
                    break;
                case "Enterprises":
                    Enterprise enterprise = new Enterprise((Enterprise)entity);
                    result = @"INSERT INTO [dbo].[Enterprises](Project, Name, Elaboration, Offerlist, Craftgroup1, Craftgroup2, Craftgroup3, Craftgroup4) VALUES(" + enterprise.Project.Id + @", '" + enterprise.Name + @"', '" + enterprise.Elaboration + @"', '" + enterprise.OfferList + @"', '" + enterprise.CraftGroup1.Id + @"', '" + enterprise.CraftGroup2.Id + @"', '" + enterprise.CraftGroup3.Id + @"', '" + enterprise.CraftGroup4.Id + "')";
                    break;
                case "Entrepeneurs":
                    Entrepeneur entrepeneur = new Entrepeneur((Entrepeneur)entity);
                    result = @"INSERT INTO [dbo].[Entrepeneurs](Entity, Craftgroup1, Craftgroup2, Craftgroup3, Craftgroup4, Region, CountryWide, Cooperative, Active) VALUES(" + entrepeneur.Entity.Id + @", " + entrepeneur.CraftGroup1.Id + @", " + entrepeneur.CraftGroup2.Id + @", " + entrepeneur.CraftGroup3.Id + @", " + entrepeneur.CraftGroup4.Id + @", " + entrepeneur.Region.Id + @", '" + entrepeneur.CountryWide.ToString() + @"', '" + entrepeneur.Cooperative.ToString() + @"', '" + entrepeneur.Active.ToString() + "')";
                    break;
                case "IttLetters":
                    IttLetter ittLetter = new IttLetter((IttLetter)entity);
                    result = "INSERT INTO[dbo].[IttLetters]([Sent], [SentDate]) VALUES('" + ittLetter.Sent.ToString() + "', '" + ittLetter.SentDate.ToString("yyyy-MM-dd") + "')";
                    break;
                case "IttLetterShippings":
                    IttLetterShipping shipping = new IttLetterShipping((IttLetterShipping)entity);
                    result = "INSERT INTO [dbo].[IttLetterShippings]([SubEntrepeneur], [Receiver, [LetterData], [CommonPdfPath], [PersonalPdfPath]) VALUES(" + shipping.SubEntrepeneur.Id + ", " + shipping.Receiver.Id + ", " + shipping.LetterData.Id + ", '" + shipping.CommonPdfPath + "', '" + shipping.PersonalPdfPath + "')";
                    break;
                case "LegalEntities":
                    LegalEntity legalEntity = new LegalEntity((LegalEntity)entity);
                    result = "INSERT INTO [dbo].[LegalEntities]([Cvr], [Name], [CoName], [Address], [ContactInfo], [Url]) VALUES(" + legalEntity.Cvr + ", " + legalEntity.Name + ", '" + legalEntity.CoName + "', " + legalEntity.Address.Id + ", " + legalEntity.ContactInfo.Id + ", '" + legalEntity.Url + "')";
                    break;
                case "LetterDataList":
                    LetterData letterData = new LetterData((LetterData)entity);
                    result = "INSERT INTO [dbo].[LetterDataList]([ProjectName], [Builder], [AnswerDate], [QuestionDate], [CorrectionDate], [ConditionDate], [MaterialUrl], [ConditionUrl], [TimeSpan], [Password]) VALUES(" + letterData.ProjectName + ", " + letterData.Builder + ", '" + letterData.AnswerDate + "', '" + letterData.QuestionDate + "', '" + letterData.CorrectionDate + "', '" + letterData.ConditionDate + "', '" + letterData.MaterialUrl + "', '" + letterData.ConditionUrl + "', '" + letterData.TimeSpan + letterData.PassWord + "')";
                    break;
                case "Offers":
                    Offer offer = new Offer((Offer)entity);
                    result = "INSERT INTO [dbo].[Offers]([Received], [ReceivedDate], [Price], [Chosen]) VALUES('" + offer.Received.ToString() + @"', '" + offer.ReceivedDate.ToString("yyyy-MM-dd") + @"', " + offer.Price.ToString() + @", '" + offer.Chosen.ToString() + @"')";
                    break;
                case "Paragrafs":
                    Paragraf paragraph = new Paragraf((Paragraf)entity);
                    result = @"INSERT INTO [dbo].[Paragrafs](Project, Name) VALUES(" + paragraph.Project.Id.ToString() + @", '" + paragraph.Text + @"')";
                    break;
                case "Persons":
                    Person person = new Person((Person)entity);
                    result = "INSERT INTO [dbo].[Persons]([Name], [ContactInfo]) VALUES('" + person.Name + @"', " + person.Id + @")";
                    break;
                case "ProjectDetails":
                    ProjectDetail projectDetail = new ProjectDetail((ProjectDetail)entity);
                    //result = "INSERT INTO [dbo].[ProjectDetails]([Name], [Description], [Period], [AnswerDate]) VALUES(< Name, nvarchar(50),>, < Description, nvarchar(max),>, < Period, nvarchar(50),>, < AnswerDate, nvarchar(50),>)";
                    result = "INSERT INTO [dbo].[ProjectDetails]([Name], [Description], [Period], [AnswerDate]) VALUES('" + projectDetail.Name + "', '" + projectDetail.Description + "', '" + projectDetail.Period + "', '" + projectDetail.AnswerDate + "')";
                    break;
                case "Projects":
                    Project project = new Project((Project)entity);
                    //result = @"INSERT INTO [dbo].[Projects]([Case], [Builder], [Status], [TenderForm], [EnterpriseForm], [Executive], [Detail], [EnterpriseList], [Copy]) VALUES(<Case, int,>, <Name, nvarchar(250),>, <Builder, int,>, <Status, int,>, <TenderForm, int,>, <EnterpriseForm, int,>, <Executive, int,>, <Description, nvarchar(250),>, <EnterpriseList, bit,>, <Copy, bit,>)"
                    result = @"INSERT INTO [dbo].[Projects]([Case], [Builder], [Status], [TenderForm], [EnterpriseForm], [Executive], [Detail], [EnterpriseList], [Copy]) VALUES(" + project.Case + @", " + project.Builder.Id + @", " + project.Status.Id + @", " + project.TenderForm.Id + @", " + project.EnterpriseForm.Id + @", " + project.Executive.Id + @", '" + project.Details + @"', '" + project.EnterpriseList.ToString() + "', '" + project.Copy.ToString() + "')";
                    break;
                case "ProjectStatuses":
                    ProjectStatus projectStatus = new ProjectStatus((ProjectStatus)entity);
                    result = "INSERT INTO [dbo].[ProjectStatuses]([Description]) VALUES('" + projectStatus.Text + @"')";
                    break;
                case "Receivers":
                    Receiver receiver = new Receiver((Receiver)entity);
                    result = "INSERT INTO [dbo].[Receivers]([Cvr], [Name], [Attention], [Street], [Place], [Zip], [Email]) VALUES('" + receiver.Cvr + "', '" + receiver.Name + "', '" + receiver.Attention + "', '" + receiver.Street + "', '" + receiver.Place + "', '" + receiver.ZipTown + "', '" + receiver.Email + "')";
                    break;
                case "Regions":
                    Region region = new Region((Region)entity);
                    result = "INSERT INTO [dbo].[Regions]([Text], [Zips]) VALUES('" + region.Text + "', '" + region.Zips + "')";
                    break;
                case "Requests":
                    Request request = new Request((Request)entity);
                    result = "INSERT INTO [dbo].[Requests]([Status], [SentDate], [ReceivedDate]) VALUES(" + request.Status.Id + @", '" + request.SentDate.ToString("yyyy-MM-dd") + @"', '" + request.ReceivedDate.ToString("yyyy-MM-dd") + @"')";
                    break;
                case "RequestShippings":
                    RequestShipping requestShipping = new RequestShipping((RequestShipping)entity);
                    //result = "INSERT INTO [dbo].[RequestShippings]([SubEntrepeneur], [AcceptUrl], [DeclineUrl], [Period], [AnswerDate], [RequestPdfPath]) VALUES(< SubEntrepeneur, int,>, < AcceptUrl, nvarchar(50),>, < DeclineUrl, nvarchar(50),>, < RequestPdfPath, nvarchar(50),>)"
                    result = @"INSERT INTO [dbo].[RequestShippings]([SubEntrepeneur], [AcceptUrl], [DeclineUrl], [Period], [AnswerDate], [RequestPdfPath]) VALUES(" + requestShipping.SubEntrepeneur.Id + @", '" + requestShipping.AcceptUrl + @"', '" + requestShipping.DeclineUrl + @"', '" + requestShipping.RequestPdfPath + @"')";
                    break;
                case "RequestStatuses":
                    RequestStatus requestStatus = new RequestStatus((RequestStatus)entity);
                    result = "INSERT INTO [dbo].[RequestStatuses]([Description]) VALUES('" + requestStatus.Text + @"')";
                    break;
                case "SubEntrepeneurs":
                    SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)entity);
                    //result = "INSERT INTO [dbo].[SubEntrepeneurs]([Entrepeneur], [Enterprise], [Contact], [Request], [IttLetter], [Offer], [Reservations], [Uphold], [AgreementConcluded], [Active]) VALUES(< Entrepeneur, int,>, < Enterprise, int,>, < Contact, int,>, < Request, int,>, < IttLetter, int,>, < Offer, int,>, < Reservations, bit,>, < Uphold, bit,>, < AgreementConcluded, bit,>, < Active, bit,>)"
                    result = "INSERT INTO [dbo].[SubEntrepeneurs]([Entrepeneur], [Enterprise], [Contact], [Request], [IttLetter], [Offer], [Reservations], [Uphold], [AgreementConcluded], [Active]) VALUES(" + subEntrepeneur.Entrepeneur.Id + @", " + subEntrepeneur.Enterprise.Id + @", " + subEntrepeneur.Contact.Id + @", " + subEntrepeneur.Request.Id + @", " + subEntrepeneur.IttLetter.Id + @", " + subEntrepeneur.Offer.Id + @", '" + subEntrepeneur.Reservations.ToString() + @"', '" + subEntrepeneur.Uphold.ToString() + @"', '" + subEntrepeneur.AgreementConcluded.ToString() + @"', '" + subEntrepeneur.Active.ToString() + @"')";
                    break;
                case "TenderFormList":
                    TenderForm tenderForm = new TenderForm((TenderForm)entity);
                    result = "INSERT INTO [dbo].[TenderFormList]([Description]) VALUES('" + tenderForm + @"')";
                    break;
                case "UserLevels":
                    UserLevel userLevel = new UserLevel((UserLevel)entity);
                    result = "INSERT INTO [dbo].[UserLevels](Text) VALUES('" + userLevel.Text + @"')";
                    break;
                case "ZipTown":
                    ZipTown zipTown = new ZipTown((ZipTown)entity);
                    result = @"INSERT INTO [dbo].[ZipTown](Zip, Town) VALUES('" + zipTown.Zip + @"', '" + zipTown.Town + "')";
                    break;
            }

            return result;
        }

        #endregion

        #region Read
        /// <summary>
        /// Method, that checks wether password is correct
        /// </summary>
        /// <param name="initials">string</param>
        /// <param name="passWord">string</param>
        /// <returns></returns>
        public bool CheckLogin(string initials, string passWord)
        {
            return executor.CheckLogin(initials, passWord);
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
                case "Addresses":
                    Address address = new Address(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], GetZipTown(Convert.ToInt32(resultArray[3])));
                    result = address;
                    break;
                case "Builders":
                    Builder builder = new Builder(Convert.ToInt32(resultArray[0]), GetLegalEntity(Convert.ToInt32(resultArray[1])), Convert.ToBoolean(resultArray[2]));
                    result = builder;
                    break;
                case "Bullets":
                    Bullet bullet = new Bullet(Convert.ToInt32(resultArray[0]), GetParagraf(Convert.ToInt32(resultArray[1])), resultArray[2]);
                    result = bullet;
                    break;
                case "Categories":
                    Category cat = new Category(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = cat;
                    break;
                case "ContactInfoList":
                    ContactInfo contactInfo = new ContactInfo(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], resultArray[4]);
                    result = contactInfo;
                    break;
                case "Contacts":
                    Contact contact = new Contact(Convert.ToInt32(resultArray[0]), GetPerson(Convert.ToInt32(resultArray[1])), GetEntrepeneur(Convert.ToInt32(resultArray[2])), resultArray[3]);
                    result = contact;
                    break;
                case "CraftGroups":
                    CraftGroup craftGroup = new CraftGroup(Convert.ToInt32(resultArray[0]), GetCategory(Convert.ToInt32(resultArray[1])), resultArray[2], resultArray[3], Convert.ToBoolean(resultArray[4]));
                    result = craftGroup;
                    break;
                case "EnterpriseForms":
                    EnterpriseForm enterpriseForm = new EnterpriseForm(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = enterpriseForm;
                    break;
                case "Enterprises":
                    Enterprise enterprise = new Enterprise(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2], resultArray[3], resultArray[4], GetCraftGroup(Convert.ToInt32(resultArray[5])), GetCraftGroup(Convert.ToInt32(resultArray[6])), GetCraftGroup(Convert.ToInt32(resultArray[7])), GetCraftGroup(Convert.ToInt32(resultArray[8])));
                    result = enterprise;
                    break;
                case "Entrepeneurs":
                    Entrepeneur entrepeneur = new Entrepeneur(Convert.ToInt32(resultArray[0]), GetLegalEntity(Convert.ToInt32(resultArray[1])), GetCraftGroup(Convert.ToInt32(resultArray[2])), GetCraftGroup(Convert.ToInt32(resultArray[3])), GetCraftGroup(Convert.ToInt32(resultArray[4])), GetCraftGroup(Convert.ToInt32(resultArray[5])), GetRegion(Convert.ToInt32(resultArray[6])), Convert.ToBoolean(resultArray[7]), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
                    result = entrepeneur;
                    break;
                case "IttLetters":
                    IttLetter ittLetter = new IttLetter(Convert.ToInt32(resultArray[0]), Convert.ToBoolean(resultArray[1]), Convert.ToDateTime(resultArray[2]));
                    result = ittLetter;
                    break;
                case "JobDescriptions":
                    JobDescription jobDescription = new JobDescription(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], Convert.ToBoolean(resultArray[3]));
                    result = jobDescription;
                    break;
                case "LegalEntities":
                    LegalEntity legalEntity = new LegalEntity(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], GetAddress(Convert.ToInt32(resultArray[4])), GetContactInfo(Convert.ToInt32(resultArray[5])), resultArray[6]);
                    result = legalEntity;
                    break;
                case "LetterDataList":
                    LetterData letterData = new LetterData(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], resultArray[4], resultArray[5], resultArray[6], resultArray[7], resultArray[8], Convert.ToInt32(resultArray[9]), resultArray[10]);
                    result = letterData;
                    break;
                case "Offers":
                    Offer offer = new Offer(Convert.ToInt32(resultArray[0]), Convert.ToBoolean(resultArray[1]), Convert.ToDateTime(resultArray[2]), Convert.ToDouble(resultArray[3]), Convert.ToBoolean(resultArray[4]));
                    result = offer;
                    break;
                case "Paragraphs":
                    Paragraf paragraph = new Paragraf(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2]);
                    result = paragraph;
                    break;
                case "Persons":
                    Person person = new Person(Convert.ToInt32(resultArray[0]), resultArray[1], GetContactInfo(Convert.ToInt32(resultArray[2])));
                    result = person;
                    break;
                case "ProjectDetails":
                    ProjectDetail projectDetail = new ProjectDetail(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], resultArray[4]);
                    result = projectDetail;
                    break;
                case "Projects":
                    Project project = new Project(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), GetBuilder(Convert.ToInt32(resultArray[2])), GetProjectStatus(Convert.ToInt32(resultArray[3])), GetTenderForm(Convert.ToInt32(resultArray[4])), GetEnterpriseForm(Convert.ToInt32(resultArray[5])), GetUser(Convert.ToInt32(resultArray[6])), GetProjectDetail(Convert.ToInt32(resultArray[7])), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
                    result = project;
                    break;
                case "ProjectStatuses":
                    ProjectStatus projectStatus = new ProjectStatus(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = projectStatus;
                    break;
                case "Receivers":
                    Receiver receiver = new Receiver(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], resultArray[4], resultArray[5], resultArray[6], resultArray[7]);
                    result = receiver;
                    break;
                case "Regions":
                    Region region = new Region(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2]);
                    result = region;
                    break;
                case "Requests":
                    Request request = new Request(Convert.ToInt32(resultArray[0]), GetRequestStatus(Convert.ToInt32(resultArray[1])), Convert.ToDateTime(resultArray[2]), Convert.ToDateTime(resultArray[3]));
                    result = request;
                    break;
                case "RequestShippings":
                    RequestShipping requestData = new RequestShipping(Convert.ToInt32(resultArray[0]), GetSubEntrepeneur(Convert.ToInt32(resultArray[1])), GetReceiver(Convert.ToInt32(resultArray[2])), resultArray[3], resultArray[4], resultArray[5]);
                    result = requestData;
                    break;
                case "RequestStatuses":
                    RequestStatus requestStatus = new RequestStatus(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = requestStatus;
                    break;
                case "IttLetterShippings":
                    IttLetterShipping shipping = new IttLetterShipping(Convert.ToInt32(resultArray[0]), GetSubEntrepeneur(Convert.ToInt32(resultArray[1])), GetReceiver(Convert.ToInt32(resultArray[2])), GetLetterData(Convert.ToInt32(resultArray[3])), resultArray[4], resultArray[5]);
                    result = shipping;
                    break;
                case "SubEntrepeneurs":
                    SubEntrepeneur subEntrepeneur = new SubEntrepeneur(Convert.ToInt32(resultArray[0]), GetEntrepeneur(Convert.ToInt32(resultArray[1])), GetEnterprise(Convert.ToInt32(resultArray[2])), GetContact(Convert.ToInt32(resultArray[3])), GetRequest(Convert.ToInt32(resultArray[4])), GetIttLetter(Convert.ToInt32(resultArray[5])), GetOffer(Convert.ToInt32(resultArray[6])), Convert.ToBoolean(resultArray[7]), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]), Convert.ToBoolean(resultArray[10]));
                    result = subEntrepeneur;
                    break;
                case "TenderForms":
                    TenderForm tenderForm = new TenderForm(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = tenderForm;
                    break;
                case "UserLevels":
                    UserLevel userLevel = new UserLevel(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = userLevel;
                    break;
                case "Users":
                    User user = new User(Convert.ToInt32(resultArray[0]), GetPerson(Convert.ToInt32(resultArray[1])), resultArray[2], GetLegalEntity(Convert.ToInt32(resultArray[3])), GetJobDescription(Convert.ToInt32(resultArray[4])), GetUserLevel(Convert.ToInt32(resultArray[5])));
                    result = user;
                    break;
                case "ZipTowns":
                    ZipTown zipTown = new ZipTown(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2]);
                    result = zipTown;
                    break;
            }

            return result;
        }

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
                case "ActiveProjects":
                    result = 10;
                    break;
                case "Addresses":
                    result = 4;
                    break;
                case "Authentications":
                    result = 3;
                    break;
                case "Builders":
                    result = 3;
                    break;
                case "Bullets":
                    result = 3;
                    break;
                case "Categories":
                    result = 2;
                    break;
                case "CraftGroups":
                    result = 5;
                    break;
                case "ContactInfoList":
                    result = 5;
                    break;
                case "Contacts":
                    result = 4;
                    break;
                case "EnterpriseForms":
                    result = 2;
                    break;
                case "Enterprises":
                    result = 9;
                    break;
                case "Entrepeneurs":
                    result = 10;
                    break;
                case "InactiveProjects":
                    result = 10;
                    break;
                case "IttLetters":
                    result = 3;
                    break;
                case "IttLetterShippings":
                    result = 6;
                    break;
                case "JobDescriptions":
                    result = 4;
                    break;
                case "LegalEntities":
                    result = 7;
                    break;
                case "LetterDataList":
                    result = 11;
                    break;
                case "Offers":
                    result = 5;
                    break;
                case "Paragrafs":
                    result = 3;
                    break;
                case "Persons":
                    result = 3;
                    break;
                case "ProjectDetails":
                    result = 5;
                    break;
                case "Projects":
                    result = 10;
                    break;
                case "ProjectStatuses":
                    result = 2;
                    break;
                case "Regions":
                    result = 3;
                    break;
                case "Requests":
                    result = 4;
                    break;
                case "RequestShippings":
                    result = 6;
                    break;
                case "RequestStatuses":
                    result = 2;
                    break;
                case "Receivers":
                    result = 8;
                    break;
                case "SubEntrepeneurs":
                    result = 11;
                    break;
                case "TenderForms":
                    result = 2;
                    break;
                case "Users":
                    result = 6;
                    break;
                case "UserLevels":
                    result = 2;
                    break;
                case "ZipTowns":
                    result = 3;
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
        /// Method, that sets new password in Db, if old password is correct
        /// </summary>
        /// <param name="oldPassWord">string</param>
        /// <param name="newPassWord">string</param>
        /// <returns></returns>
        public bool ChangePassword(string oldPassWord, string newPassWord)
        {
            bool result = false;
            result = executor.ChangePassword(CurrentUser.Id, oldPassWord, newPassWord);
            return result;
        }

        /// <summary>
        /// Method, that returns a SQL-query
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
                case "Addresses":
                    Address address = new Address((Address)_object);
                    result = @"UPDATE [dbo].[Addresses] SET [Street] = '" + address.Street + "', [Place] = '" + address.Place + "', [Zip] = " + address.ZipTown.Id + " WHERE [Id] = '" + address.Id + "';";
                    break;
                case "Builders":
                    Builder builder = new Builder((Builder)_object);
                    result = @"UPDATE[dbo].[Builders] SET [Entity] = " + builder.Entity.Id + @" [Active] = '" + builder.Active.ToString() + @"' WHERE [Id] = " + builder.Id;
                    break;
                case "Bullets":
                    Bullet bullet = new Bullet((Bullet)_object);
                    result = @"UPDATE[dbo].[Bullets] SET [Paragraf] = " + bullet.Paragraf.Id + @", [Text] = '" + bullet.Text + @"' WHERE [Id] = " + bullet.Id;
                    break;
                case "Categories":
                    Category category = new Category((Category)_object);
                    result = @"UPDATE[dbo].[Categories] SET [Text] = '" + category.Text + @"' WHERE[Id] = " + category.Id;
                    break;
                case "ContactInfoList":
                    ContactInfo info = new ContactInfo((ContactInfo)_object);
                    result = @"UPDATE [dbo].[ContactInfoList] SET [Phone] = '" + info.Phone + "', [Fax] = '" + info.Fax + "', [Mobile] = '" + info.Mobile + "', [Email] = '" + info.Email + "' WHERE [Id] = " + info.Id;
                    break;
                case "Contacts":
                    Contact contact = new Contact((Contact)_object);
                    result = @"UPDATE [dbo].[Contacts] SET [Person] = " + contact.Person.Id + ", [Entrepeneur] = " + contact.Entrepeneur.Id + ", [Area] = '" + contact.Area + "' WHERE [Id] = " + contact.Id;
                    break;
                case "CraftGroups":
                    CraftGroup craftGroup = new CraftGroup((CraftGroup)_object);
                    result = @"UPDATE [dbo].[CraftGroup] SET [Category] = '" + craftGroup.Category.Id + "', [Designation] = '" + craftGroup.Designation + "', [Description] = '" + craftGroup.Description + "', [Active] = '" + craftGroup.Active.ToString() + "' WHERE [Id] = " + craftGroup.Id;
                    break;
                case "EnterpriseForms":
                    EnterpriseForm form = new EnterpriseForm((EnterpriseForm)_object);
                    result = @"UPDATE [dbo].[EnterpriseForms] SET [Text] = '" + form.Text + @"' WHERE [Id] = " + form.Id;
                    break;
                case "Enterprises":
                    Enterprise enterprise = new Enterprise((Enterprise)_object);
                    result = @"UPDATE [dbo].[Enterprises] SET [Project] = " + enterprise.Project.Id.ToString() + @", [Name] = '" + enterprise.Name + @"', [Elaboration] = '" + enterprise.Elaboration + @"', [OfferList] = '" + enterprise.OfferList + @"', [CraftGroup1] = " + enterprise.CraftGroup1.Id.ToString() + @", [CraftGroup2] = " + enterprise.CraftGroup2.Id.ToString() + @", [CraftGroup3] = " + enterprise.CraftGroup3.Id.ToString() + @", [CraftGroup4] = " + enterprise.CraftGroup4.Id.ToString() + @" WHERE [Id] = " + enterprise.Id;
                    break;
                case "Entrepeneurs":
                    Entrepeneur entrepeneur = new Entrepeneur((Entrepeneur)_object);
                    result = @"UPDATE [dbo].[Entrepeneurs] SET [Entity] = " + entrepeneur.Entity.Id.ToString() + @", [CraftGroup1] = " + entrepeneur.CraftGroup1.Id.ToString() + @", [CraftGroup2] = " + entrepeneur.CraftGroup2.Id.ToString() + @", [CraftGroup3] = " + entrepeneur.CraftGroup3.Id.ToString() + @", [CraftGroup4] = " + entrepeneur.CraftGroup4.Id.ToString() + @", [Region] = " + entrepeneur.Region.Id.ToString() + @", [CountryWide] = '" + entrepeneur.CountryWide.ToString() + @"', [Cooperative] = '" + entrepeneur.Cooperative.ToString() + @"', [Active] = '" + entrepeneur.Active.ToString() + @"' WHERE [Id] = " + entrepeneur.Id;
                    break;
                case "IttLetters":
                    IttLetter ittLetter = new IttLetter((IttLetter)_object);
                    result = @"UPDATE [dbo].[IttLetters] SET [Sent] = '" + ittLetter.Sent + @"', [SentDate] = " + ittLetter.SentDate + "' WHERE [Id] = " + ittLetter.Id;
                    break;
                case "IttLetterShippings":
                    IttLetterShipping shipping = new IttLetterShipping((IttLetterShipping)_object);
                    result = @"UPDATE [dbo].[IttLetterShippings] SET [SubEntrepeneur] = " + shipping.SubEntrepeneur.Id + @", [Receiver] = " + shipping.Receiver.Id + @", [LetterData] = " + shipping.LetterData.Id + @", [CommonPdfPath] = '" + shipping.CommonPdfPath + @"', [PersonalPdfPath] = '" + shipping.PersonalPdfPath + @"' WHERE [Id] = " + shipping.Id;
                    break;
                case "JobDescriptions":
                    JobDescription jobDescription = new JobDescription((JobDescription)_object);
                    result = @"UPDATE [dbo].[Regions] SET [Occupation] = " + jobDescription.Occupation + ", [Area] = '" + jobDescription.Area + "', [Procuration] = '" + jobDescription.Procuration.ToString() + "' WHERE [Id] = " + jobDescription.Id.ToString();
                    break;
                case "LegalEntities":
                    LegalEntity legalEntity = new LegalEntity((LegalEntity)_object);
                    result = @"UPDATE [dbo].[LegalEntities] SET [Cvr] = '" + legalEntity.Cvr + @"', [Name] = '" + legalEntity.Name + @"', [CoName] = '" + legalEntity.CoName + @"', [Address] = " + legalEntity.Address.Id + @", [ContactInfo] = " + legalEntity.ContactInfo.Id + @", [Url] = '" + legalEntity.Url + @"' WHERE [Id] = " + legalEntity.Id;
                    break;
                case "LetterDataList":
                    LetterData letterData = new LetterData((LetterData)_object);
                    result = @"UPDATE [dbo].[LetterDataList] SET [ProjectName] = '" + letterData.ProjectName + @"', [Builder] = '" + letterData.Builder + @" [AnswerDate] = '" + letterData.AnswerDate + @"', [QuestionDate] = '" + letterData.QuestionDate + @"', [CorrectionDate] = '" + letterData.CorrectionDate + @"', [TimeSpan] = '" + letterData.TimeSpan + @"', [MaterialUrl] = '" + letterData.MaterialUrl + @"', [ConditionUrl] = '" + letterData.ConditionUrl + @"', [PassWord] = '" + letterData.PassWord + @"' WHERE [Id] = " + letterData.Id;
                    break;
                case "Offers":
                    Offer offer = new Offer((Offer)_object);
                    result = @"UPDATE [dbo].[Offers] SET [Received] = " + offer.Received.ToString() + @", [ReceivedDate] = " + offer.ReceivedDate.ToString("yyyy-MM-dd") + @", [Price] = " + offer.Price + @", [Chosen] = '" + offer.Chosen.ToString() + @"' WHERE [Id] = " + offer.Id;
                    break;
                case "Paragrafs":
                    Paragraf paragraph = new Paragraf((Paragraf)_object);
                    result = @"UPDATE [dbo].[Paragrafs] SET [Project] = " + paragraph.Project.Id + @", [Name] = '" + paragraph.Text + @"' WHERE[Id] = " + paragraph.Id;
                    break;
                case "Persons":
                    Person person = new Person((Person)_object);
                    result = @"UPDATE [dbo].[Persons] SET [Name] = '" + person.Name + @"', [ContactInfo] = " + person.ContactInfo.Id + @" WHERE[Id] = " + person.Id;
                    break;
                case "ProjectDetails":
                    ProjectDetail projectDetail = new ProjectDetail((ProjectDetail)_object);
                    //result = @"UPDATE [dbo].[ProjectDetails] SET [Name] = < Case, int,>, [Description] = <Description, nvarchar(max),>, [Period] = <Period, nvarchar(50),>, [AnswerDate] = <AnswerDate, nvarchar(50),> WHERE[Id] = <Id, int,>"
                    result = @"UPDATE dbo.[ProjectDetails] SET [Name] = '" + projectDetail.Name + "', [Description] = '" + projectDetail.Description + "', [Period] = " + projectDetail.Period + "', [AnswerDate] = " + projectDetail.AnswerDate + "' WHERE [Id] = " + projectDetail.Id;
                    break;
                case "Projects":
                    Project project = new Project((Project)_object);
                    //result = @"UPDATE [dbo].[Projects] SET[Case] = < Case, int,>, [Name] = <Name, nvarchar(250),>, [Builder] = <Builder, int,>, [Status] = <Status, int,>, [TenderForm] = <TenderForm, int,>, [EnterpriseForm] = <EnterpriseForm, int,>, [Executive] = <Executive, int,>, [Details] = <Details, int,>, [EnterpriseList] = <EnterpriseList, bit,>, [Copy] = <Copy, bit,> WHERE[Id] = <Id, int,>"
                    result = @"UPDATE dbo.[Projects] SET [Case] = '" + project.Case + "', [Builder] = " + project.Builder.Id + ", [Status] = " + project.Status.Id + ", [TenderForm] = " + project.TenderForm.Id + ", [EnterpriseForm] = " + project.EnterpriseForm.Id + ", [Executive] = " + project.Executive.Id + ", [Details] = " + project.Details.Id + ", [EnterpriseList] = '" + project.EnterpriseList.ToString() + "', [Copy] = '" + project.Copy.ToString() + "' WHERE [Id] = " + project.Id;
                    break;
                case "ProjectStatuses":
                    ProjectStatus projectStatus = new ProjectStatus((ProjectStatus)_object);
                    result = @"UPDATE dbo.[ProjectStatuses] SET [Description] = '" + projectStatus.Text + "' WHERE Id = " + projectStatus.Id;
                    break;
                case "Regions":
                    Region region = new Region((Region)_object);
                    result = @"UPDATE [dbo].[Regions] SET [Text] = '" + region.Text + ", [Zips] = '" + region.Zips + "' WHERE [Id] = " + region.Id;
                    break;
                case "Requests":
                    Request request = new Request((Request)_object);
                    result = @"UPDATE [dbo].[Requests] SET [Status] = " + request.Status.Id + ", [SentDate] = '" + request.SentDate.ToString("yyyy-MM-dd") + "', [ReceivedDate] = '" + request.ReceivedDate.ToString("yyyy-MM-dd") + "' WHERE [Id] = " + request.Id.ToString();
                    break;
                case "RequestShippings":
                    RequestShipping requestData = new RequestShipping((RequestShipping)_object);
                    //result = "UPDATE [dbo].[RequestShippings] SET [SubEntrepeneur] = <SubEntrepeneur, int,>, [AcceptUrl] = <AcceptUrl, nvarchar(50),>, [DeclineUrl] = <DeclineUrl, nvarchar(50),>, [RequestPdfPath] = <RequestPdfPath, nvarchar(50),> WHERE [Id] = <Id, int,>"
                    result = @"UPDATE [dbo].[RequestShippings] SET [SubEntrepeneur] = " + requestData.SubEntrepeneur.Id + @", [AcceptUrl] = '" + requestData.AcceptUrl + @"', [DeclineUrl] = '" + requestData.DeclineUrl + @"', [RequestPdfPath] = '" + requestData.RequestPdfPath + "' WHERE [Id] = " + requestData.Id;
                    break;
                case "RequestStatuses":
                    RequestStatus requestStatus = new RequestStatus((RequestStatus)_object);
                    result = @"UPDATE [dbo].[RequestStatuses] SET [Text] = '" + requestStatus.Text + "' WHERE [Id] = " + requestStatus.Id.ToString();
                    break;
                case "SubEntrepeneurs":
                    SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)_object);
                    result = @"UPDATE [dbo].[SubEntrepeneurs] SET [Enterprise] = " + subEntrepeneur.Enterprise.Id + ", [Entrepeneur] = " + subEntrepeneur.Entrepeneur.Id + ", [Contact] = " + subEntrepeneur.Contact.Id + ", [Request] = " + subEntrepeneur.Request.Id + ", [IttLetter] = " + subEntrepeneur.IttLetter.Id + ", [Offer] = " + subEntrepeneur.Offer.Id + ", [Reservations] = '" + subEntrepeneur.Reservations.ToString() + "', [Uphold] = '" + subEntrepeneur.Uphold.ToString() + "', [AgreementConcluded] = '" + subEntrepeneur.AgreementConcluded.ToString() + "', [Active] = '" + subEntrepeneur.Active.ToString() + "' WHERE [Id] = " + subEntrepeneur.Id;
                    break;
                case "TenderForms":
                    TenderForm tenderForm = new TenderForm((TenderForm)_object);
                    result = @"UPDATE [dbo].[TenderForms] SET [description] = '" + tenderForm.Text + "' WHERE [Id] = " + tenderForm.Id;
                    break;
                case "Users":
                    User user = new User((User)_object);
                    result = @"UPDATE [dbo].[Users] SET [Person] = " + user.Person.Id + ", [Initials] = '" + user.Initials + ", [Department] = '" + user.Department.Id + "', [JobDescription] = " + user.JobDescription.Id + ", [UserLevel] = " + user.UserLevel.Id + " WHERE [Id] = " + user.Id;
                    break;
                case "UserLevels":
                    UserLevel userLevel = new UserLevel((UserLevel)_object);
                    result = @"UPDATE [dbo].[UserLevels] SET [Text] = '" + userLevel.Text + "' WHERE [Id] = " + userLevel.Id;
                    break;
                case "ZipTown":
                    ZipTown zipTown = new ZipTown((ZipTown)_object);
                    result = @"UPDATE dbo.[ZipTown] SET [Town] = '" + zipTown.Town + "' WHERE [Zip] = '" + zipTown.Zip + "'";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that updates an entity in Db
        /// </summary>
        /// <param name="_object">object</param>
        /// <returns>bool</returns>
        public bool UpdateInDb(object _object)
        {
            bool result = false;
            string entityType = _object.GetType().ToString().Remove(0, 14);

            switch (entityType)
            {
                case "Address":
                    Address address = new Address((Address)_object);
                    UpdateInDb(address.ZipTown);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Addresses", address));
                    break;
                case "Builder":
                    Builder builder = new Builder((Builder)_object);
                    UpdateInDb(builder.Entity);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Builders", builder));
                    break;
                case "Bullet":
                    Bullet bullet = new Bullet((Bullet)_object);
                    UpdateInDb(bullet.Paragraf);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Bullets", bullet));
                    break;
                case "Category":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Categories", new Category((Category)_object)));
                    break;
                case "Contact":
                    Contact contact = new Contact((Contact)_object);
                    UpdateInDb(contact.Person);
                    UpdateInDb(contact.Entrepeneur);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Contacts", contact));
                    break;
                case "ContactInfo":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ContactInfoList", new ContactInfo((ContactInfo)_object)));
                    break;
                case "CraftGroup":
                    CraftGroup craftGroup = new CraftGroup((CraftGroup)_object);
                    UpdateInDb(craftGroup.Category);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("CraftGroups", craftGroup));
                    break;
                case "Enterprise":
                    Enterprise enterprise = new Enterprise((Enterprise)_object);
                    UpdateInDb(enterprise.Project);
                    UpdateInDb(enterprise.CraftGroup1);
                    UpdateInDb(enterprise.CraftGroup2);
                    UpdateInDb(enterprise.CraftGroup3);
                    UpdateInDb(enterprise.CraftGroup4);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Enterprises", enterprise));
                    break;
                case "EnterpriseForm":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("EnterpriseForms", new EnterpriseForm((EnterpriseForm)_object)));
                    break;
                case "IttLetter":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetters", new Enterprise((Enterprise)_object)));
                    break;
                case "IttLetterShipping":
                    IttLetterShipping ittLetterShipping = new IttLetterShipping((IttLetterShipping)_object);
                    UpdateInDb(ittLetterShipping.SubEntrepeneur);
                    UpdateInDb(ittLetterShipping.Receiver);
                    UpdateInDb(ittLetterShipping.LetterData);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetterShippings", ittLetterShipping));
                    break;
                case "JobDescription":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("JobDescriptions", new JobDescription((JobDescription)_object)));
                    break;
                case "LegalEntity":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("LegalEntities", new LegalEntity((LegalEntity)_object)));
                    break;
                case "LetterData":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("LetterDataList", new LetterData((LetterData)_object)));
                    break;
                case "Offer":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Offers", new Offer((Offer)_object)));
                    break;
                case "Paragraf":
                    Paragraf paragraf = new Paragraf((Paragraf)_object);
                    UpdateInDb(paragraf.Project);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Paragrafs", paragraf));
                    break;
                case "Person":
                    Person person = new Person((Person)_object);
                    UpdateInDb(person.ContactInfo);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Persons", person));
                    break;
                case "Project":
                    Project project = new Project((Project)_object);
                    UpdateInDb(project.Builder);
                    UpdateInDb(project.Status);
                    UpdateInDb(project.TenderForm);
                    UpdateInDb(project.EnterpriseForm);
                    UpdateInDb(project.Executive);
                    UpdateInDb(project.Details);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Projects", project));
                    break;
                case "ProjectDetail":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ProjectDetails", new ProjectDetail((ProjectDetail)_object)));
                    break;
                case "ProjectStatus":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ProjectStatuses", new ProjectStatus((ProjectStatus)_object)));
                    break;
                case "Region":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Regions", new Region((Region)_object)));
                    break;
                case "Request":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Requests", new Request((Request)_object)));
                    break;
                case "RequestShipping":
                    RequestShipping requestShipping = new RequestShipping((RequestShipping)_object);
                    UpdateInDb(requestShipping.SubEntrepeneur);
                    UpdateInDb(requestShipping);
                    result = ProcesSqlQuery(GetSQLQueryUpdate("RequestShippings", requestShipping));
                    break;
                case "RequestStatus":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("RequestStatuses", new RequestStatus((RequestStatus)_object)));
                    break;
                case "SubEntrepeneur":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("SubEntrepeneurs", new SubEntrepeneur((SubEntrepeneur)_object)));
                    break;
                case "TenderForm":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("TenderForms", new TenderForm((TenderForm)_object)));
                    break;
                case "User":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Users", new User((User)_object)));
                    break;
                case "UserLevel":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("UserLevels", new UserLevel((UserLevel)_object)));
                    break;
                case "ZipTown":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ZipTowns", new ZipTown((ZipTown)_object)));
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Method, that deletes an entry from Db
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteFromDb(string list, string id)
        {
            return ProcesSqlQuery(CreateSqlQueryDeleteFromList(list, id.ToString()));
        }

        /// <summary>
        /// Method, that Create a Delete From SQL-Query
        /// </summary>
        /// <param name="list">string</param>
        /// <param name="id">string</param>
        /// <returns>string</returns>
        private string CreateSqlQueryDeleteFromList(string list, string id)
        {
            //DELETE FROM table_name WHERE condition;
            return @"DELETE FROM [dbo].[" + list + @"] WHERE [Id] = " + id + ";";
        }

        #endregion

        #endregion

        #region Refresh Project Lists
        /// <summary>
        /// Method, that refreshes all Project Lists
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshAllProjectLists(int projectId)
        {
            RefreshProjectBullets(projectId);
            RefreshProjectEnterprises(projectId);
            RefreshProjectIttLetterShippings(projectId);
            RefreshProjectParagrafs(projectId);
            RefreshProjectSubEntrepeneurs(projectId);
            RefreshProjectRequestShippings(projectId);

        }

        /// <summary>
        /// Method, that retrieves an Bullet List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshProjectBullets(int projectId)
        {
            ProjectLists.Bullets.Clear();

            RefreshBullets();

            foreach (Bullet bullet in Bullets)
            {
                if (bullet.Paragraf.Project.Id == projectId)
                {
                    ProjectLists.Bullets.Add(new Bullet((Bullet)bullet));
                }
            }

        }

        /// <summary>
        /// Method, that retrieves an Enterprise List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshProjectEnterprises(int projectId)
        {
            ProjectLists.Enterprises.Clear();

            RefreshEnterprises();

            foreach (Enterprise enterprise in Enterprises)
            {
                if (enterprise.Project.Id == projectId)
                {
                    ProjectLists.Enterprises.Add(new Enterprise((Enterprise)enterprise));
                }
            }

        }

        /// <summary>
        /// Method, that refreshes a single Project List
        /// Accepts the following lists: All, Bullets, Enterprises, Paragrafs, RequestShippings, IttLetterShippings & SubEntrepeneurs
        /// </summary>
        /// <param name="list">string</param>
        /// <param name="projectId">int</param>
        public void RefreshProjectList(string list, int projectId)
        {
            switch (list)
            {
                case "All":
                    RefreshAllProjectLists(projectId);
                    break;
                case "Bullets":
                    RefreshProjectBullets(projectId);
                    break;
                case "Enterprises":
                    RefreshProjectEnterprises(projectId);
                    break;
                case "Paragrafs":
                    RefreshProjectParagrafs(projectId);
                    break;
                case "RequestShippings":
                    RefreshProjectRequestShippings(projectId);
                    break;
                case "IttLetterShippings":
                    RefreshProjectIttLetterShippings(projectId);
                    break;
                case "SubEntrepeneurs":
                    RefreshProjectSubEntrepeneurs(projectId);
                    break;
            }

        }

        /// <summary>
        /// Method, that retrieves an Enterprise List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshProjectParagrafs(int projectId)
        {
            ProjectLists.Paragrafs.Clear();

            RefreshParagrafs();

            foreach (Paragraf paragraf in Paragrafs)
            {
                if (paragraf.Project.Id == projectId)
                {
                    ProjectLists.Paragrafs.Add(new Paragraf((Paragraf)paragraf));
                }
            }

        }

        /// <summary>
        /// Method, that retrieves an Request Shippings list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshProjectRequestShippings(int projectId)
        {
            ProjectLists.RequestShippings.Clear();

            RefreshRequestShippings();

            foreach (RequestShipping requestData in RequestShippings)
            {
                if (requestData.SubEntrepeneur.Enterprise.Project.Id == projectId)
                {
                    ProjectLists.RequestShippings.Add(new RequestShipping((RequestShipping)requestData));
                }
            }

        }

        /// <summary>
        /// Method, that retrieves an IttLetter Shippings list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshProjectIttLetterShippings(int projectId)
        {
            ProjectLists.IttLetterShippings.Clear();

            RefreshIttLetterShippings();

            foreach (IttLetterShipping shipping in IttLetterShippings)
            {
                if (shipping.SubEntrepeneur.Enterprise.Project.Id == projectId)
                {
                    ProjectLists.IttLetterShippings.Add(new IttLetterShipping((IttLetterShipping)shipping));
                }
            }

        }

        /// <summary>
        /// Method that retrieves a SubEntrepeneur list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        private void RefreshProjectSubEntrepeneurs(int projectId)
        {
            ProjectLists.SubEntrepeneurs.Clear();

            RefreshSubEntrepeneurs();

            foreach (SubEntrepeneur subEntrepeneur in SubEntrepeneurs)
            {
                if (subEntrepeneur.Enterprise.Project.Id == projectId)
                {
                    ProjectLists.SubEntrepeneurs.Add(subEntrepeneur);
                }
            }

        }

        #endregion

        #region Get Object
        /// <summary>
        /// Method, that retrieves an Address from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Address</returns>
        public Address GetAddress(int id)
        {
            Address result = new Address();

            foreach (Address address in Addresses)
            {
                if (address.Id == id)
                {
                    result = address;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Builder from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Builder</returns>
        public Builder GetBuilder(int id)
        {
            Builder result = new Builder();

            foreach (Builder builder in Builders)
            {
                if (builder.Id == id)
                {
                    result = builder;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Bullet from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Bullet</returns>
        public Bullet GetBullet(int id)
        {
            Bullet result = new Bullet();

            foreach (Bullet bullet in Bullets)
            {
                if (bullet.Id == id)
                {
                    result = bullet;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Category from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Category</returns>
        public Category GetCategory(int id)
        {
            Category result = new Category();

            foreach (Category category in Categories)
            {
                if (category.Id == id)
                {
                    result = category;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an Address from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Address</returns>
        public Contact GetContact(int id)
        {
            Contact result = new Contact();

            foreach (Contact contact in Contacts)
            {
                if (contact.Id == id)
                {
                    result = contact;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves ContactInfo from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ContactInfo</returns>
        public ContactInfo GetContactInfo(int id)
        {
            ContactInfo result = new ContactInfo();

            foreach (ContactInfo info in ContactInfoList)
            {
                if (info.Id == id)
                {
                    result = info;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a CraftGroup from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>CraftGroup</returns>
        public CraftGroup GetCraftGroup(int id)
        {
            CraftGroup result = new CraftGroup();

            foreach (CraftGroup craftGroup in CraftGroups)
            {
                if (craftGroup.Id == id)
                {
                    result = craftGroup;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an Enterprise from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Enterprise</returns>
        public Enterprise GetEnterprise(int id)
        {
            Enterprise result = new Enterprise();

            foreach (Enterprise enterprise in Enterprises)
            {
                if (enterprise.Id == id)
                {
                    result = enterprise;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an EnterpriseForm from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>EnterpriseForm</returns>
        public EnterpriseForm GetEnterpriseForm(int id)
        {
            EnterpriseForm result = new EnterpriseForm();

            foreach (EnterpriseForm form in EnterpriseForms)
            {
                if (form.Id == id)
                {
                    result = form;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an Entrepeneur from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Entrepeneur</returns>
        public Entrepeneur GetEntrepeneur(int id)
        {
            Entrepeneur result = new Entrepeneur();

            foreach (Entrepeneur entrepeneur in Entrepeneurs)
            {
                if (entrepeneur.Id == id)
                {
                    result = entrepeneur;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an IttLetter from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>IttLetter</returns>
        public IttLetter GetIttLetter(int id)
        {
            IttLetter result = new IttLetter();

            foreach (IttLetter letter in IttLetters)
            {
                if (letter.Id == id)
                {
                    result = letter;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an IttLetterShipping from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>IttLetterShipping</returns>
        public IttLetterShipping GetIttLetterShipping(int id)
        {
            IttLetterShipping result = new IttLetterShipping();

            foreach (IttLetterShipping shipping in IttLetterShippings)
            {
                if (shipping.Id == id)
                {
                    result = shipping;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a JobDescription from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>JobDescription</returns>
        public JobDescription GetJobDescription(int id)
        {
            JobDescription result = new JobDescription();

            foreach (JobDescription description in JobDescriptions)
            {
                if (description.Id == id)
                {
                    result = description;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a LegalEntity from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>LegalEntity</returns>
        public LegalEntity GetLegalEntity(int id)
        {
            LegalEntity result = new LegalEntity();

            foreach (LegalEntity entity in LegalEntities)
            {
                if (entity.Id == id)
                {
                    result = entity;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves LetterData from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>LetterData</returns>
        public LetterData GetLetterData(int id)
        {
            LetterData result = new LetterData();

            foreach (LetterData letterData in LetterDataList)
            {
                if (letterData.Id == id)
                {
                    result = letterData;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an Offer from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Offer</returns>
        public Offer GetOffer(int id)
        {
            Offer result = new Offer();

            foreach (Offer offer in Offers)
            {
                if (offer.Id == id)
                {
                    result = offer;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Paragraf from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Paragraf</returns>
        public Paragraf GetParagraf(int id)
        {
            Paragraf result = new Paragraf();

            foreach (Paragraf paragraph in Paragrafs)
            {
                if (paragraph.Id == id)
                {
                    result = paragraph;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Person from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Person</returns>
        public Person GetPerson(int id)
        {
            Person result = new Person();

            foreach (Person person in Persons)
            {
                if (person.Id == id)
                {
                    result = person;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Project from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Project</returns>
        public Project GetProject(int id)
        {
            Project result = new Project();

            foreach (Project project in Projects)
            {
                if (project.Id == id)
                {
                    result = project;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a ProjectDetail from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ProjectDetail</returns>
        public ProjectDetail GetProjectDetail(int id)
        {
            ProjectDetail result = new ProjectDetail();

            foreach (ProjectDetail projectDetail in ProjectDetails)
            {
                if (projectDetail.Id == id)
                {
                    result = projectDetail;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a ProjectStatus from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ProjectStatus</returns>
        public ProjectStatus GetProjectStatus(int id)
        {
            ProjectStatus result = new ProjectStatus();

            foreach (ProjectStatus status in ProjectStatuses)
            {
                if (status.Id == id)
                {
                    result = status;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Receiver from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Receiver</returns>
        public Receiver GetReceiver(int id)
        {
            Receiver result = new Receiver();

            foreach (Receiver receiver in Receivers)
            {
                if (receiver.Id == id)
                {
                    result = receiver;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Region from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Region</returns>
        public Region GetRegion(int id)
        {
            Region result = new Region();

            foreach (Region region in Regions)
            {
                if (region.Id == id)
                {
                    result = region;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Request from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Request</returns>
        public Request GetRequest(int id)
        {
            Request result = new Request();

            foreach (Request request in Requests)
            {
                if (request.Id == id)
                {
                    result = request;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a RequestShipping from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>RequestShipping</returns>
        public RequestShipping GetRequestShipping(int id)
        {
            RequestShipping result = new RequestShipping();

            foreach (RequestShipping requestShipping in RequestShippings)
            {
                if (requestShipping.Id == id)
                {
                    result = requestShipping;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a RequestStatus from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>RequestStatus</returns>
        public RequestStatus GetRequestStatus(int id)
        {
            RequestStatus result = new RequestStatus();

            foreach (RequestStatus status in RequestStatuses)
            {
                if (status.Id == id)
                {
                    result = status;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a SubEntrepeneur from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>SubEntrepeneur</returns>
        public SubEntrepeneur GetSubEntrepeneur(int id)
        {
            SubEntrepeneur result = new SubEntrepeneur();

            foreach (SubEntrepeneur subEntrepeneur in SubEntrepeneurs)
            {
                if (subEntrepeneur.Id == id)
                {
                    result = subEntrepeneur;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a TenderForm from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>TenderForm</returns>
        public TenderForm GetTenderForm(int id)
        {
            TenderForm result = new TenderForm();

            foreach (TenderForm form in TenderForms)
            {
                if (form.Id == id)
                {
                    result = form;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a User from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>User</returns>
        public User GetUser(int id)
        {
            User result = new User();

            foreach (User user in Users)
            {
                if (user.Id == id)
                {
                    result = user;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a UserLevel from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>UserLevel</returns>
        public UserLevel GetUserLevel(int id)
        {
            UserLevel result = new UserLevel();

            foreach (UserLevel userLevel in UserLevels)
            {
                if (userLevel.Id == id)
                {
                    result = userLevel;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a ZipTown from Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ZipTown</returns>
        public ZipTown GetZipTown(int id)
        {
            ZipTown result = new ZipTown();

            foreach (ZipTown zipTown in ZipTowns)
            {
                if (zipTown.Id == id)
                {
                    result = zipTown;
                    break;
                }
            }

            return result;
        }

        #endregion

        #region Refresh Lists
        /// <summary>
        /// Method, that refreshes all Lists
        /// </summary>
        public void RefreshAllLists()
        {
            //Base Level Lists
            RefreshCategories(); //
            RefreshContactInfoList(); //
            RefreshEnterpriseForms(); //
            RefreshIttLetters(); //
            RefreshJobDescriptions(); //
            RefreshLetterDataList(); //
            RefreshOffers(); //
            RefreshProjectDetails(); //
            RefreshProjectStatuses(); //
            RefreshReceivers(); //
            RefreshRegions(); //
            RefreshRequestStatuses(); //
            RefreshTenderforms(); //
            RefreshUserLevels(); //
            RefreshZipTowns(); //

            //Second level Lists
            RefreshAddresses(true); //
            RefreshCraftGroups(true); //
            RefreshPersons(true); //
            RefreshRequests(true); //

            //Third level list
            RefreshLegalEntities(true); //
            RefreshUsers(true); //

            //Fourth Level List
            RefreshBuilders(true); //
            RefreshEntrepeneurs(true); //

            //Fifth level Lists
            RefreshContacts(true); //
            RefreshProjects(true); //

            //Sixth level Lists
            RefreshEnterprises(true);
            RefreshParagrafs(true);
            RefreshContacts(true); //

            //Seventh level Lists
            RefreshBullets(true);
            RefreshSubEntrepeneurs(true);

            //Eightieth level Lists
            RefreshIttLetterShippings(true);
            RefreshRequestShippings(true); //


        }

        /// <summary>
        /// Method, that refreshes the Addresses list
        /// </summary>
        private void RefreshAddresses(bool allLists = false)
        {
            if (Addresses != null)
            {
                Addresses.Clear();
            }

            if (!allLists)
            {
                RefreshZipTowns();
            }

            List<object> tempList = ReadListFromDb("Addresses");

            foreach (object obj in tempList)
            {
                Addresses.Add((Address)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Builders lists
        /// </summary>
        private void RefreshBuilders(bool allLists = false)
        {
            if (Builders != null)
            {
                Builders.Clear();
            }
            if (ActiveBuilders != null)
            {
                ActiveBuilders.Clear();
            }
            if (InactiveBuilders != null)
            {
                InactiveBuilders.Clear();
            }

            if (!allLists)
            {
                RefreshLegalEntities();
            }

            List<object> objectList = ReadListFromDb("Builders");

            foreach (object obj in objectList)
            {
                Builder builder = new Builder((Builder)obj);
                Builders.Add(builder);
                switch (builder.Active.ToString())
                {
                    case "True":
                        ActiveBuilders.Add(builder);
                        break;
                    case "False":
                        InactiveBuilders.Add(builder);
                        break;
                }
            }

        }

        /// <summary>
        /// Method, that refreshes the Bullets list
        /// </summary>
        private void RefreshBullets(bool allLists = false)
        {
            if (Bullets != null)
            {
                Bullets.Clear();
            }

            if (!allLists)
            {
                RefreshParagrafs();
            }

            List<object> tempList = ReadListFromDb("Bullets");

            foreach (object obj in tempList)
            {
                Bullets.Add((Bullet)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Categories list
        /// </summary>
        private void RefreshCategories()
        {
            if (Categories != null)
            {
                Categories.Clear();
            }
            else
            {
                Categories = new List<Category>();
            }

            List<object> tempList = ReadListFromDb("Categories");

            foreach (object obj in tempList)
            {
                Categories.Add(new Category((Category)obj));
            }
        }

        /// <summary>
        /// Method, that refreshes the ContactInfo List
        /// </summary>
        private void RefreshContactInfoList()
        {
            if (ContactInfoList != null)
            {
                ContactInfoList.Clear();
            }

            List<object> tempList = ReadListFromDb("ContactInfoList");

            foreach (object obj in tempList)
            {
                ContactInfoList.Add((ContactInfo)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Contacts list
        /// </summary>
        private void RefreshContacts(bool allLists = false)
        {
            if (Contacts != null)
            {
                Contacts.Clear();
            }

            if (!allLists)
            {
                RefreshPersons();
                RefreshEntrepeneurs();
            }

            List<object> tempContacts = ReadListFromDb("Contacts");

            foreach (object obj in tempContacts)
            {
                Contacts.Add(new Contact((Contact)obj));
            }
        }

        /// <summary>
        /// Method, that refreshes the CraftGroups list
        /// </summary>
        private void RefreshCraftGroups(bool allLists = false)
        {
            if (CraftGroups != null)
            {
                CraftGroups.Clear();
            }

            if (!allLists)
            {
                RefreshCategories();
            }

            List<object> tempList = ReadListFromDb("CraftGroups");

            foreach (object obj in tempList)
            {
                CraftGroups.Add((CraftGroup)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the EnterpriseForms list
        /// </summary>
        private void RefreshEnterpriseForms()
        {
            if (EnterpriseForms != null)
            {
                EnterpriseForms.Clear();
            }

            List<object> tempList = ReadListFromDb("EnterpriseForms");

            foreach (object obj in tempList)
            {
                EnterpriseForms.Add((EnterpriseForm)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Enterprise List
        /// </summary>
        private void RefreshEnterprises(bool allLists = false)
        {
            if (Enterprises != null)
            {
                Enterprises.Clear();
            }

            if (!allLists)
            {
                RefreshProjects();
                RefreshCraftGroups();
            }

            List<object> tempList = ReadListFromDb("Enterprises");

            foreach (object obj in tempList)
            {
                Enterprises.Add((Enterprise)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Entrepeneurs lists
        /// </summary>
        private void RefreshEntrepeneurs(bool allLists = false)
        {
            if (Entrepeneurs != null)
            {
                Entrepeneurs.Clear();
            }
            if (ActiveEntrepeneurs != null)
            {
                ActiveEntrepeneurs.Clear();
            }
            if (InactiveEntrepeneurs != null)
            {
                InactiveEntrepeneurs.Clear();
            }

            if (!allLists)
            {
                RefreshRegions();
                RefreshLegalEntities();
                RefreshCraftGroups();
            }

            List<object> tempList = ReadListFromDb("Entrepeneurs");

            foreach (object obj in tempList)
            {
                Entrepeneur entrepeneur = new Entrepeneur((Entrepeneur)obj);
                Entrepeneurs.Add(entrepeneur);

                switch (entrepeneur.Active.ToString())
                {
                    case "True":
                        ActiveEntrepeneurs.Add(entrepeneur);
                        break;
                    default:
                        InactiveEntrepeneurs.Add(entrepeneur);
                        break;
                }
            }

        }

        /// <summary>
        /// Method, that refreshes the IttLetters list
        /// </summary>
        private void RefreshIttLetters()
        {
            if (IttLetters != null)
            {
                IttLetters.Clear();
            }

            List<object> tempList = ReadListFromDb("IttLetters");

            foreach (object obj in tempList)
            {
                IttLetters.Add((IttLetter)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the IttLetter Shippings List
        /// </summary>
        private void RefreshIttLetterShippings(bool allLists = false)
        {
            if (IttLetterShippings != null)
            {
                IttLetterShippings.Clear();
            }

            if (!allLists)
            {
                RefreshLetterDataList();
                RefreshReceivers();
                RefreshSubEntrepeneurs();
            }

            List<object> tempList = ReadListFromDb("IttLetterShippings");

            foreach (object obj in tempList)
            {
                IttLetterShippings.Add((IttLetterShipping)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the JobDescriptions list
        /// </summary>
        private void RefreshJobDescriptions()
        {
            if (JobDescriptions != null)
            {
                JobDescriptions.Clear();
            }

            List<object> tempList = ReadListFromDb("JobDescriptions");

            foreach (object obj in tempList)
            {
                JobDescriptions.Add((JobDescription)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the LegalEntities list
        /// </summary>
        private void RefreshLegalEntities(bool allLists = false)
        {
            if (LegalEntities != null)
            {
                LegalEntities.Clear();
            }

            if (!allLists)
            {
                RefreshAddresses();
                RefreshContactInfoList();
            }

            List<object> tempList = ReadListFromDb("LegalEntities");

            foreach (object obj in tempList)
            {
                LegalEntities.Add((LegalEntity)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the LetterData List
        /// </summary>
        private void RefreshLetterDataList()
        {
            if (LetterDataList != null)
            {
                LetterDataList.Clear();
            }

            List<object> tempList = ReadListFromDb("LetterDataList");

            foreach (object obj in tempList)
            {
                LetterDataList.Add((LetterData)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes a list
        /// </summary>
        /// <param name="list">string</param>
        public void RefreshList(string list)
        {
            switch (list)
            {
                case "Addresses":
                    RefreshAddresses();
                    break;
                case "Builders":
                    RefreshBuilders();
                    break;
                case "Bullets":
                    RefreshBullets();
                    break;
                case "Categories":
                    RefreshCategories();
                    break;
                case "ContactInfoList":
                    RefreshContactInfoList();
                    break;
                case "Contacts":
                    RefreshContacts();
                    break;
                case "CraftGroups":
                    RefreshCraftGroups();
                    break;
                case "EnterpriseForms":
                    RefreshEnterpriseForms();
                    break;
                case "Enterprises":
                    RefreshEnterprises();
                    break;
                case "Entrepeneurs":
                    RefreshEntrepeneurs();
                    break;
                case "IttLetters":
                    RefreshIttLetters();
                    break;
                case "IttLetterShippings":
                    RefreshIttLetterShippings();
                    break;
                case "JobDescriptions":
                    RefreshJobDescriptions();
                    break;
                case "LegalEntities":
                    RefreshLegalEntities();
                    break;
                case "LetterDataList":
                    RefreshLetterDataList();
                    break;
                case "Offers":
                    RefreshOffers();
                    break;
                case "Paragrafs":
                    RefreshParagrafs();
                    break;
                case "Persons":
                    RefreshPersons();
                    break;
                case "Projects":
                    RefreshProjects();
                    break;
                case "ProjectStatuses":
                    RefreshProjectStatuses();
                    break;
                case "Receivers":
                    RefreshReceivers();
                    break;
                case "Regions":
                    RefreshRegions();
                    break;
                case "Requests":
                    RefreshRequests();
                    break;
                case "RequestShippings":
                    RefreshRequestShippings();
                    break;
                case "RequestStatuses":
                    RefreshRequestStatuses();
                    break;
                case "SubEntrepeneurs":
                    RefreshSubEntrepeneurs();
                    break;
                case "TenderForms":
                    RefreshTenderforms();
                    break;
                case "Users":
                    RefreshUsers();
                    break;
                case "UserLevels":
                    RefreshUserLevels();
                    break;
                case "ZipTowns":
                    RefreshZipTowns();
                    break;
            }
        }

        /// <summary>
        /// Method, that refreshes the Offers list
        /// </summary>
        private void RefreshOffers()
        {
            if (Offers != null)
            {
                Offers.Clear();
            }

            List<object> tempList = ReadListFromDb("Offers");

            foreach (object obj in tempList)
            {
                Offers.Add((Offer)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Paragraf List
        /// </summary>
        private void RefreshParagrafs(bool allLists = false)
        {
            if (Paragrafs != null)
            {
                Paragrafs.Clear();
            }

            if (!allLists)
            {
                RefreshProjects();
            }

            List<object> tempList = ReadListFromDb("Paragraphs");

            foreach (object obj in tempList)
            {
                Paragrafs.Add(new Paragraf((Paragraf)obj));
            }
        }

        /// <summary>
        /// Method, that refreshes the Persons list
        /// </summary>
        private void RefreshPersons(bool allLists = false)
        {
            if (Persons != null)
            {
                Persons.Clear();
            }

            if (!allLists)
            {
                RefreshContactInfoList();
            }

            List<object> tempList = ReadListFromDb("Persons");

            foreach (object obj in tempList)
            {
                Person person = new Person((Person)obj);
                Persons.Add(person);
            }
        }

        /// <summary>
        /// Method, that refreshes the Project Details list
        /// </summary>
        private void RefreshProjectDetails()
        {
            if (ProjectDetails != null)
            {
                ProjectDetails.Clear();
            }

            List<object> tempList = ReadListFromDb("ProjectDetails");

            foreach (ProjectDetail obj in tempList)
            {
                ProjectDetails.Add(new ProjectDetail((ProjectDetail)obj));
            }
        }

        /// <summary>
        /// Method, that refreshes the Projects lists
        /// </summary>
        private void RefreshProjects(bool allLists = false)
        {
            if (Projects != null)
            {
                Projects.Clear();
            }
            if (ActiveProjects != null)
            {
                ActiveProjects.Clear();
            }
            if (InactiveProjects != null)
            {
                InactiveProjects.Clear();
            }

            if (!allLists)
            {
                RefreshProjectStatuses();
                RefreshProjectDetails();
                RefreshTenderforms();
                RefreshEnterpriseForms();
                RefreshUsers();
                RefreshBuilders();
            }

            List<object> tempList = ReadListFromDb("Projects");

            foreach (object obj in tempList)
            {
                Project project = new Project((Project)obj);
                Projects.Add(project);

                switch (project.Status.Id)
                {
                    case 1:
                        ActiveProjects.Add(project);
                        break;
                    default:
                        InactiveProjects.Add(project);
                        break;
                }
            }

        }

        /// <summary>
        /// Method, that refreshes the ProjectStatuses list
        /// </summary>
        private void RefreshProjectStatuses()
        {
            if (ProjectStatuses != null)
            {
                ProjectStatuses.Clear();
            }

            List<object> tempList = ReadListFromDb("ProjectStatuses");

            foreach (object obj in tempList)
            {
                ProjectStatuses.Add((ProjectStatus)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Receivers list
        /// </summary>
        private void RefreshReceivers()
        {
            if (Receivers != null)
            {
                Receivers.Clear();
            }

            List<object> tempList = ReadListFromDb("Receivers");

            foreach (object obj in tempList)
            {
                Receivers.Add((Receiver)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Regions list
        /// </summary>
        private void RefreshRegions()
        {
            if (Regions != null)
            {
                Regions.Clear();
            }

            List<object> tempList = ReadListFromDb("Regions");

            foreach (object obj in tempList)
            {
                Regions.Add((Region)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Requests list
        /// </summary>
        private void RefreshRequests(bool allLists = false)
        {
            if (Requests != null)
            {
                Requests.Clear();
            }

            if (allLists)
            {
                RefreshRequestStatuses();
            }

            List<object> tempList = ReadListFromDb("Requests");

            foreach (object obj in tempList)
            {
                Requests.Add((Request)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Request Shippings list
        /// </summary>
        private void RefreshRequestShippings(bool allLists = false)
        {
            if (RequestShippings != null)
            {
                RequestShippings.Clear();
            }

            if (!allLists)
            {
                RefreshSubEntrepeneurs();
            }

            List<object> tempList = ReadListFromDb("RequestShippings");

            foreach (object obj in tempList)
            {
                RequestShippings.Add((RequestShipping)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Request Status List
        /// </summary>
        private void RefreshRequestStatuses()
        {
            if (RequestStatuses != null)
            {
                RequestStatuses.Clear();
            }

            List<object> tempList = ReadListFromDb("RequestStatuses");

            foreach (object obj in tempList)
            {
                RequestStatuses.Add((RequestStatus)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the SubEntrepeneurs list
        /// </summary>
        private void RefreshSubEntrepeneurs(bool allLists = false)
        {
            if (SubEntrepeneurs != null)
            {
                SubEntrepeneurs.Clear();
            }

            if (!allLists)
            {
                RefreshEntrepeneurs();
                RefreshEnterprises();
                RefreshContacts();
                RefreshRequests();
                RefreshIttLetters();
                RefreshOffers();
            }

            List<object> tempList = ReadListFromDb("SubEntrepeneurs");

            foreach (object obj in tempList)
            {
                SubEntrepeneurs.Add(new SubEntrepeneur((SubEntrepeneur)obj));
            }
        }

        /// <summary>
        /// Method, that refreshes the TenderForms list
        /// </summary>
        private void RefreshTenderforms()
        {
            if (TenderForms != null)
            {
                TenderForms.Clear();
            }

            List<object> tempList = ReadListFromDb("TenderForms");

            foreach (object obj in tempList)
            {
                TenderForms.Add((TenderForm)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Users lists
        /// </summary>
        private void RefreshUsers(bool allLists = false)
        {
            if (Users != null)
            {
                Users.Clear();
            }
            if (ActiveUsers != null)
            {
                ActiveUsers.Clear();
            }
            if (InactiveUsers != null)
            {
                InactiveUsers.Clear();
            }

            if (!allLists)
            {
                RefreshJobDescriptions();
                RefreshUserLevels();
                RefreshPersons();
                RefreshLegalEntities();
            }

            List<object> tempList = ReadListFromDb("Users");

            foreach (object obj in tempList)
            {
                User user = new User((User)obj);
                Users.Add(user);

                switch (user.UserLevel.Id)
                {
                    case 0:
                        InactiveUsers.Add(user);
                        break;
                    default:
                        ActiveUsers.Add(user);
                        break;
                }
            }

        }

        /// <summary>
        /// Method, that refreshes the User Level list
        /// </summary>
        private void RefreshUserLevels()
        {
            if (UserLevels != null)
            {
                UserLevels.Clear();
            }

            List<object> tempList = ReadListFromDb("UserLevels");

            foreach (object obj in tempList)
            {
                UserLevels.Add((UserLevel)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the ZipTown List
        /// </summary>
        private void RefreshZipTowns()
        {
            if (ZipTowns != null)
            {
                ZipTowns.Clear();
            }

            List<object> tempList = ReadListFromDb("ZipTowns");

            foreach (object obj in tempList)
            {
                ZipTowns.Add((ZipTown)obj);
            }
        }

        #endregion

        #endregion

    }
}
