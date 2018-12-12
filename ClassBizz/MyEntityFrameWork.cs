using JudDataAccess;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClassBizz
{
    public class MyEntityFrameWork
    {
        #region Fields
        public static string strConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JortonSubEnt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private Executor executor = new Executor(strConnection);
        private MacAddress macAddress = new MacAddress();

            #region Lists
            public List<Address> Addresses = new List<Address>();
            public List<BluePrint> BluePrints = new List<BluePrint>();
            public List<Builder> Builders = new List<Builder>();
            public List<Category> Categories = new List<Category>();
            public List<Contact> Contacts = new List<Contact>();
            public List<ContactInfo> ContactInfoList = new List<ContactInfo>();
            public List<CraftGroup> CraftGroups = new List<CraftGroup>();
            public List<Description> Descriptions = new List<Description>();
            public List<Enterprise> Enterprises = new List<Enterprise>();
            public List<EnterpriseForm> EnterpriseForms = new List<EnterpriseForm>();
            public List<IttLetter> IttLetters = new List<IttLetter>();
            public List<IttLetterBullet> IttLetterBullets = new List<IttLetterBullet>();
            public List<IttLetterParagraph> IttLetterParagraphs = new List<IttLetterParagraph>();
            public List<IttLetterPdfData> IttLetterPdfDataList = new List<IttLetterPdfData>();
            public List<IttLetterReceiver> IttLetterReceivers = new List<IttLetterReceiver>();
            public List<IttLetterShipping> IttLetterShippingList = new List<IttLetterShipping>();
            public List<JobDescription> JobDescriptions = new List<JobDescription>();
            public List<LegalEntity> LegalEntities = new List<LegalEntity>();
            public List<Miscellaneous> MiscellaneousList = new List<Miscellaneous>();
            public List<Offer> Offers = new List<Offer>();
            public List<Project> Projects = new List<Project>();
            public List<ProjectStatus> ProjectStatusList = new List<ProjectStatus>();
            public List<Region> Regions = new List<Region>();
            public List<Request> Requests = new List<Request>();
            public List<RequestStatus> RequestStatusList = new List<RequestStatus>();
            public List<SubEntrepeneur> SubEntrepeneurs = new List<SubEntrepeneur>();
            public List<TenderForm> TenderForms = new List<TenderForm>();
            public List<TimeSchedule> TimeSchedules = new List<TimeSchedule>();
            public List<User> Users = new List<User>();
            public List<ZipTown> ZipTownList = new List<ZipTown>();

            #endregion

        //Multi Level Lists
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

        /// <summary>
        /// Method, that retrieves a MacAddress
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            String result = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Method, that returns the MacAddress
        /// </summary>
        /// <returns></returns>
        public string ObtainStrConnection()
        {
            return strConnection.ToString();
        }

        #region DataBase

        /// <summary>
        /// Method, that processes an SQL-query
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool ProcesSqlQuery(string strSql)
        {
            bool result;
            result = executor.WriteToDataBase(strSql);
            return result;
        }

        #region Create
        /// <summary>
        /// Method, that creates a new entity in Db
        /// Accepts the following entityTypes: EnterpriseForm, ZipTown
        /// </summary>
        /// <param name="entityType">string</param>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        public bool CreateInDbReturnBool(object entity)
        {
            bool result = false;
            string entityType = entity.GetType().ToString();
            string list = GetListFromEntityType(entityType);

            result = ProcesSqlQuery(GetSQLQueryInsert(list, entity));

            return result;
        }

        /// <summary>
        /// Method, that creates a new entity in Db
        /// Accepts the following entityTypes: Address, BluePrintContact, 
        /// ContactInfo, CraftGroup, Enterprise, IttLetter, IttLetterPdfData, 
        /// Project, Request
        /// </summary>
        /// <param name="entityType">string</param>
        /// <param name="entity">Object</param>
        /// <returns>int</returns>
        public int CreateInDbReturnInt(object entity)
        {
            int result = 0;
            int count = 0;
            string entityTypeDk = "";
            bool dbAnswer = false;
            string entityType = entity.GetType().ToString();
            string list = GetListFromEntityType(entityType);
            dbAnswer = ProcesSqlQuery(GetSQLQueryInsert(list, entity));
            entityTypeDk = GetDanishEntityType(entityType);

            if (!dbAnswer)
            {
                MessageBox.Show("Databasen returnerede en fejl ved forsøg på at oprette ny " + entityTypeDk + ".", "Databasefejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                switch (entityType)
                {
                    case "Address":
                        RefreshAddresses();
                        count = Addresses.Count;
                        result = Addresses[count - 1].Id;
                        break;
                    case "BluePrint":
                        RefreshBluePrints();
                        count = BluePrints.Count;
                        result = BluePrints[count - 1].Id;
                        break;
                    case "Contact":
                        RefreshContacts();
                        count = Contacts.Count;
                        result = Contacts[count - 1].Id;
                        break;
                    case "ContactInfo":
                        RefreshContactInfoList();
                        count = ContactInfoList.Count;
                        result = ContactInfoList[count - 1].Id;
                        break;
                    case "CraftGroup":
                        RefreshCraftGroups();
                        count = CraftGroups.Count;
                        result = CraftGroups[count - 1].Id;
                        break;
                    case "Description":
                        RefreshDescriptions();
                        count = Descriptions.Count;
                        result = Descriptions[count - 1].Id;
                        break;
                    case "Enterprise":
                        RefreshEnterprises();
                        count = Enterprises.Count;
                        result = Enterprises[count - 1].Id;
                        break;
                    case "IttLetter":
                        RefreshIttLetters();
                        count = IttLetters.Count;
                        result = IttLetters[count - 1].Id;
                        break;
                    case "IttLetterReceiver":
                        RefreshIttLetterReceivers();
                        count = IttLetterReceivers.Count;
                        result = IttLetterReceivers[count - 1].Id;
                        break;
                    case "IttLetterPdfData":
                        RefreshIttLetterPdfDataList();
                        count = IttLetterPdfDataList.Count;
                        result = IttLetterPdfDataList[count - 1].Id;
                        break;
                    case "IttLetterShipping":
                        RefreshIttLetterShippingList();
                        count = IttLetterShippingList.Count;
                        result = IttLetterShippingList[count - 1].Id;
                        break;
                    case "Project":
                        RefreshProjects();
                        count = Projects.Count;
                        result = Projects[count - 1].Id;
                        break;
                    case "Request":
                        RefreshRequests();
                        count = Requests.Count;
                        result = Requests[count - 1].Id;
                        break;
                }
            }

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
                case "BluePrint":
                    result = "Arbejdstegning";
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
                case "Description":
                    result = "Beskrivelse";
                    break;
                case "Enterprise":
                    result = "Entreprise";
                    break;
                case "IttLetter":
                    result = "Udbudsbrev";
                    break;
                case "IttLetterPdfData":
                    result = "Udbudbrevs Pdf-data";
                    break;
                case "IttLetterReceiver":
                    result = "Udbudbrevs Pdf-data";
                    break;
                case "IttLetterShipping":
                    result = "Udbudbrevs forsendelsesoplysninger";
                    break;
                case "Project":
                    result = "Projekt";
                    break;
                case "Request":
                    result = "Foresørgsel";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a List Name from an entity Type
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private string GetListFromEntityType(string entityType)
        {
            string result = "";

            switch (entityType)
            {
                case "Address":
                    result = "Addresses";
                    break;
                case "BluePrint":
                    result = "BluePrints";
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
                case "Description":
                    result = "Descriptions";
                    break;
                case "Enterprise":
                    result = "Enterprises";
                    break;
                case "EnterpriseForm":
                    result = "EnterpriseForms";
                    break;
                case "IttLetter":
                    result = "IttLetters";
                    break;
                case "IttLetterBullet":
                    result = "IttLetterBullets";
                    break;
                case "IttLetterParagraph":
                    result = "IttLetterParagraphs";
                    break;
                case "IttLetterPdfData":
                    result = "IttLetterPdfDataList";
                    break;
                case "IttLetterReceiver":
                    result = "IttLetterReceivers";
                    break;
                case "IttLetterShipping":
                    result = "IttLetterShippingList";
                    break;
                case "JobDescription":
                    result = "JobDescriptions";
                    break;
                case "LegalEntity":
                    result = "LegalEntities";
                    break;
                case "Miscellaneous":
                    result = "MiscellaneousList";
                    break;
                case "Offer":
                    result = "Offers";
                    break;
                case "Project":
                    result = "Projects";
                    break;
                case "ProjectStatus":
                    result = "ProjectStatusList";
                    break;
                case "Region":
                    result = "Regions";
                    break;
                case "Request":
                    result = "Requests";
                    break;
                case "RequestStatus":
                    result = "RequestStatusList";
                    break;
                case "SubEntrepeneur":
                    result = "SubEntrepeneurs";
                    break;
                case "TenderForms":
                    result = "TenderForms";
                    break;
                case "TimeSchedule":
                    result = "TimeSchedules";
                    break;
                case "User":
                    result = "Users";
                    break;
                case "ZipTown":
                    result = "ZipTown";
                    break;
            }

            return result;
        }


        /// <summary>
        /// Method, that returns a SQL-query
        /// Accepted lists:  Addresses, BluePrints, Contacts, ContactInfoList, CraftGroups, EnterpriseForms, EnterprisList, IttLetters, IttLetterPdfDataList, Project, Ziptown
        /// </summary>
        /// <param name="list">string</param>
        /// <param name="entity">object</param>
        /// <returns></returns>
        private string GetSQLQueryInsert(string list, object entity)
        {
            string result = "";

            switch (list)
            {
                //INSERT INTO table_name (column1, column2, column3, ...) VALUES(value1, value2, value3, ...);
                case "Addresses":
                    Address address = new Address((Address)entity);
                    result = @"INSERT INTO dbo.Addresses(Street, Place, Zip) VALUES('" + address.Street + @"', '" + address.Place + @"', " + address.ZipTown.Id + ")";
                    break;
                case "BluePrints":
                    BluePrint bluePrint = new BluePrint((BluePrint)entity);
                    result = @"INSERT INTO[dbo].[BluePrints]([Project], [Name], [Description], [Url]) VALUES(" + bluePrint.Project.Id + @", '" + bluePrint.Name + @"', " + bluePrint.Description.Id + @", '" + bluePrint.Url + @"')";
                    break;
                case "Builders":
                    Builder builder = new Builder((Builder)entity);
                    result = @"INSERT INTO[dbo].[Builders]([Cvr], [Name], [ContactInfo], [Adresse], [Url]) VALUES('" + builder.Cvr + @"', '" + builder.Name + @"', " + builder.ContactInfo.Id + @", " + builder.Address.Id + @"', '" + builder.Url + @"')";
                    break;
                case "Categories":
                    Category category = new Category((Category)entity);
                    result = "INSERT INTO[dbo].[Categories]([Name]) VALUES('" + category.Name + "')";
                    break;
                case "Contacts":
                    Contact contact = new Contact((Contact)entity);
                    result = "INSERT INTO[dbo].[Contacts]([LegalEntity], [Name], [Area], [ContactInfo]) VALUES(" + contact.LegalEntity + ", '" + contact.Name + "', '" + contact.Area + "', " + contact.ContactInfo.Id + ")";
                    break;
                case "ContactInfoList":
                    ContactInfo info = new ContactInfo((ContactInfo)entity);
                    result = "INSERT INTO[dbo].[ContactInfoList]([Phone], [Fax], [Mobile], [Email]) VALUES(" + info.Phone + ", '" + info.Fax + "', '" + info.Mobile + "', '" + info.Email + "')";
                    break;
                case "CraftGroups":
                    CraftGroup craftGroup = new CraftGroup((CraftGroup)entity);
                    result = @"INSERT INTO dbo.CraftGroups(Category, Designation, Description, Active) VALUES('" + craftGroup.Category.Id + @", '" + craftGroup.Designation + @"', '" + craftGroup.Description + @"', '" + craftGroup.Active.ToString() + "')";
                    break;
                case "Descriptions":
                    Description description = new Description((Description)entity);
                    result = @"INSERT INTO[dbo].[Descriptions]([Project], [Enterprise], [Text]) VALUES(" + description.Project.Id + @", '" + description.Enterprise.Id + @", '" + description.Text + @"')";
                    break;
                case "EnterpriseForms":
                    EnterpriseForm form = new EnterpriseForm((EnterpriseForm)entity);
                    result = @"INSERT INTO dbo.EnterpriseForms(Id, Name) VALUES(" + form.Id + @", '" + form.Name + "')";
                    break;
                case "Enterprises":
                    Enterprise enterprise = new Enterprise((Enterprise)entity);
                    result = @"INSERT INTO dbo.EnterpriseForms(Project, Name, Elaboration, Offerlist, Craftgroup1, Craftgroup2, Craftgroup3, Craftgroup4) VALUES(" + enterprise.Project.Id + @", '" + enterprise.Name + @"', '" + enterprise.Elaboration + @"', '" + enterprise.OfferList + @"', '" + enterprise.CraftGroup1.Id + @"', '" + enterprise.CraftGroup2.Id + @"', '" + enterprise.CraftGroup3.Id + @"', '" + enterprise.CraftGroup4.Id + "')";
                    break;
                case "IttLetters":
                    IttLetter ittLetter = new IttLetter((IttLetter)entity);
                    result = "INSERT INTO[dbo].[IttLetters]([Sent], [SentDate]) VALUES('" + ittLetter.Sent + "', " + ittLetter.SentDate.ToShortDateString() + "')";
                    break;
                case "IttLetterBullets":
                    IttLetterBullet bullet = new IttLetterBullet((IttLetterBullet)entity);
                    result = @"INSERT INTO dbo.IttLetterBullets(IttLetterParagraph, Name) VALUES(" + bullet.Paragraph.Id.ToString() + @", '" + bullet.Text + @"')";
                    break;
                case "IttLetterParagraphs":
                    IttLetterParagraph paragraph = new IttLetterParagraph((IttLetterParagraph)entity);
                    result = @"INSERT INTO dbo.IttLetterParagraphs(Project, Name) VALUES(" + paragraph.Project.Id.ToString() + @", '" + paragraph.Name + @"')";
                    break;
                case "IttLetterPdfDataList":
                    IttLetterPdfData pdfData = new IttLetterPdfData((IttLetterPdfData)entity);
                    result = "INSERT INTO[dbo].[IttLetterPdfDataList]([Project], [Builder], [AnswerDate], [QuestionDate], [CorrectionSheetDate], [TimeSpan], [MaterialUrl], [ConditionUrl], [Password]) VALUES(" + pdfData.Project.Id + ", " + pdfData.Builder.Id + ", '" + pdfData.AnswerDate + "', '" + pdfData.QuestionDate + "', '" + pdfData.CorrectionSheetDate + "', '" + pdfData.TimeSpan + "', '" + pdfData.MaterialUrl + "', '" + pdfData.ConditionUrl + "', '" + pdfData.PassWord + "')";
                    break;
                case "IttLetterReceivers":
                    IttLetterReceiver receiver = new IttLetterReceiver((IttLetterReceiver)entity);
                    result = "INSERT INTO[dbo].[IttLetterReceivers]([Shipping], [Project], [CompanyId], [CompanyName], [Attention], [Street], [Place], [Zip], [Email]) VALUES(" + receiver.Shipping.Id + ", " + receiver.Project.Id + ", '" + receiver.CompanyId + "', '" + receiver.CompanyName + "', '" + receiver.Attention + "', '" + receiver.Street + "', '" + receiver.Place + "', '" + receiver.ZipTown + "', '" + receiver.Email + "')";
                    break;
                case "IttLetterShippingList":
                    IttLetterShipping shipping = new IttLetterShipping((IttLetterShipping)entity);
                    result = "INSERT INTO[dbo].[IttLetterShippingList]([Project], [CommonPdfPath], [PdfPath]) VALUES(" + shipping.Project.Id + ", '" + shipping.CommonPdfPath + "', '" + shipping.PdfPath + "')";
                    break;
                case "Projects":
                    Project project = new Project((Project)entity);
                    result = @"INSERT INTO dbo.Projects(CaseId, Name, Builder, Status, TenderForm, EnterpriseForm, Executive, Enterprises, Copy) VALUES(" + project.CaseId + @", '" + project.Name + @"', " + project.Builder + @", " + project.Status + @", " + project.TenderForm + @", " + project.EnterpriseForm + @", " + project.Executive + @", '" + project.EnterprisesList.ToString() + "', '" + project.Copy.ToString() + "')";
                    break;
                case "ProjectStatusList":
                    ProjectStatus projectStatus = new ProjectStatus((ProjectStatus)entity);
                    result = "INSERT INTO[dbo].[ProjectStatusList]([Description]) VALUES('" + projectStatus.Description + @"')";
                    break;
                case "Requests":
                    Request request = new Request((Request)entity);
                    result = "INSERT INTO[dbo].[Requests]([Status], [SentDate], [ReceivedDate]) VALUES(" + request.Status.Id + @", '" + request.SentDate.ToShortDateString() + @"', '" + request.ReceivedDate.ToShortDateString() + @"')";
                    break;
                case "RequestStatusList":
                    RequestStatus requestStatus = new RequestStatus((RequestStatus)entity);
                    result = "INSERT INTO[dbo].[RequestStatusList]([Description]) VALUES('" + requestStatus.Description + @"')";
                    break;
                case "SubEntrepeneurs":
                    SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)entity);
                    result = "INSERT INTO[dbo].[SubEntrepeneurs]([Enterprises], [Entrepeneur], [Contact], [Request], [IttLetter], [Offer], [Reservations], [Uphold], AgreementConcluded], [Active]) VALUES(" + subEntrepeneur.Enterprise.Id + @", '" + subEntrepeneur.Entrepeneur.Id + @"', " + subEntrepeneur.Contact.Id + @", " + subEntrepeneur.Request.Id + @", " + subEntrepeneur.IttLetter.Id + @", " + subEntrepeneur.Offer.Id + @", '" + subEntrepeneur.Reservations.ToString() + @"', '" + subEntrepeneur.Uphold.ToString() + @"', '" + subEntrepeneur.AgreementConcluded.ToString() + @"', '" + subEntrepeneur.Active.ToString() + @"')";
                    break;
                case "TenderFormList":
                    TenderForm tenderForm = new TenderForm((TenderForm)entity);
                    result = "INSERT INTO[dbo].[TenderFormList]([Description]) VALUES('" + tenderForm + @"')";
                    break;
                case "TimeSchedules":
                    TimeSchedule schedule = new TimeSchedule((TimeSchedule)entity);
                    result = "INSERT INTO[dbo].[TimeSchedules]([Project], [Text], [ReceivedDate]) VALUES(" + schedule.Project.Id + @", '" + schedule.Text + @"')";
                    break;
                case "Users":
                    User user = new User((User)entity);
                    result = "INSERT INTO[dbo].[Users]([Initials], [Name], [PassWord], [JobDescription], [ContactInfo], [Administrator]) VALUES('" + user.Initials + @"', '" + user.Name + @"', '" + user.PassWord + @"', " + user.JobDescription.Id + @", '" + user.ContactInfo.Id + @"', '" + user.Administrator.ToString() + @"')";
                    break;
                case "ZipTown":
                    ZipTown zipTown = new ZipTown((ZipTown)entity);
                    result = @"INSERT INTO dbo.ZipTown(Zip, Town) VALUES('" + zipTown.Zip + @"', '" + zipTown.Town + "')";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        #endregion

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
                case "Adresses":
                    result = 4;
                    break;
                case "BluePrints":
                    result = 5;
                    break;
                case "Builders":
                    result = 6;
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
                    result = 5;
                    break;
                case "Descriptions":
                    result = 4;
                    break;
                case "EnterpriseForms":
                    result = 2;
                    break;
                case "Enterprises":
                    result = 9;
                    break;
                case "IttLetterBullets":
                    result = 3;
                    break;
                case "IttLetterParagraphs":
                    result = 3;
                    break;
                case "IttLetterPdfDataList":
                    result = 10;
                    break;
                case "IttLetterReceivers":
                    result = 10;
                    break;
                case "IttLetters":
                    result = 3;
                    break;
                case "IttLetterShippingList":
                    result = 4;
                    break;
                case "JobDescriptions":
                    result = 4;
                    break;
                case "LegalEntities":
                    result = 13;
                    break;
                case "MiscellaneousList":
                    result = 3;
                    break;
                case "Offers":
                    result = 5;
                    break;
                case "Projects":
                    result = 10;
                    break;
                case "ProjectStatusList":
                    result = 2;
                    break;
                case "Regions":
                    result = 3;
                    break;
                case "Requests":
                    result = 4;
                    break;
                case "RequestStatusList":
                    result = 2;
                    break;
                case "SubEntrepeneurs":
                    result = 11;
                    break;
                case "TenderForms":
                    result = 2;
                    break;
                case "TimeSchedules":
                    result = 2;
                    break;
                case "Users":
                    result = 7;
                    break;
                case "ZipTownList":
                    result = 2;
                    break;
            }

            return result;
        }

        private object GetObject(string list, string[] resultArray)
        {
            object result = new object();

            switch (list)
            {
                case "Addresses":
                    Address address = new Address(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], GetZipTown(Convert.ToInt32(resultArray[3])));
                    result = address;
                    break;
                case "BluePrints":
                    BluePrint bluePrint = new BluePrint(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2], GetDescription(Convert.ToInt32(resultArray[3])), resultArray[4]);
                    result = bluePrint;
                    break;
                case "Builders":
                    Builder builder = new Builder(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], GetAddress(Convert.ToInt32(resultArray[3])), GetContactInfo(Convert.ToInt32(resultArray[4])), resultArray[5]);
                    result = builder;
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
                    Contact contact = new Contact(Convert.ToInt32(resultArray[0]), GetLegalEntity(resultArray[1]), resultArray[2], resultArray[3], GetContactInfo(Convert.ToInt32(resultArray[4])));
                    result = contact;
                    break;
                case "CraftGroups":
                    CraftGroup craftGroup = new CraftGroup(Convert.ToInt32(resultArray[0]), GetCategory(Convert.ToInt32(resultArray[1])), resultArray[2], resultArray[3], Convert.ToBoolean(resultArray[4]));
                    result = craftGroup;
                    break;
                case "Descriptions":
                    Description description = new Description(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), GetEnterprise(Convert.ToInt32(resultArray[2])), resultArray[3]);
                    result = description;
                    break;
                case "EnterpriseForms":
                    EnterpriseForm enterpriseForm = new EnterpriseForm(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = enterpriseForm;
                    break;
                case "Enterprises":
                    Enterprise enterprise = new Enterprise(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2], resultArray[3], resultArray[4], GetCraftGroup(Convert.ToInt32(resultArray[5])), GetCraftGroup(Convert.ToInt32(resultArray[6])), GetCraftGroup(Convert.ToInt32(resultArray[7])), GetCraftGroup(Convert.ToInt32(resultArray[8])));
                    result = enterprise;
                    break;
                case "IttLetterBullets":
                    IttLetterBullet bullet = new IttLetterBullet(Convert.ToInt32(resultArray[0]), GetIttLetterParagraph(Convert.ToInt32(resultArray[1])), resultArray[2]);
                    result = bullet;
                    break;
                case "IttLetterParagraphs":
                    IttLetterParagraph paragraph = new IttLetterParagraph(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2]);
                    result = paragraph;
                    break;
                case "IttLetterPdfDataList":
                    IttLetterPdfData pdfData = new IttLetterPdfData(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), GetBuilder(Convert.ToInt32(resultArray[2])), resultArray[3], resultArray[4], resultArray[5], Convert.ToInt32(resultArray[6]), resultArray[7], resultArray[8], resultArray[9]);
                    result = pdfData;
                    break;
                case "IttLetterReceivers":
                    IttLetterReceiver receiver = new IttLetterReceiver(Convert.ToInt32(resultArray[0]), GetIttLetterShipping(Convert.ToInt32(resultArray[1])), GetProject(Convert.ToInt32(resultArray[2])), resultArray[3], resultArray[4], resultArray[5], resultArray[6], resultArray[7], resultArray[8], resultArray[9]);
                    result = receiver;
                    break;
                case "IttLetters":
                    IttLetter ittLetter = new IttLetter(Convert.ToInt32(resultArray[0]), Convert.ToBoolean(resultArray[1]), Convert.ToDateTime(resultArray[2]));
                    result = ittLetter;
                    break;
                case "IttLetterShippingList":
                    IttLetterShipping shipping = new IttLetterShipping(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2], resultArray[3]);
                    result = shipping;
                    break;
                case "JobDescriptions":
                    JobDescription jobDescription = new JobDescription(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], Convert.ToBoolean(resultArray[3]));
                    result = jobDescription;
                    break;
                case "LegalEntities":
                    LegalEntity legalEntity = new LegalEntity(resultArray[0], resultArray[1], GetAddress(Convert.ToInt32(resultArray[2])), GetContactInfo(Convert.ToInt32(resultArray[3])), resultArray[4], GetCraftGroup(Convert.ToInt32(resultArray[5])), GetCraftGroup(Convert.ToInt32(resultArray[6])), GetCraftGroup(Convert.ToInt32(resultArray[7])), GetCraftGroup(Convert.ToInt32(resultArray[8])), GetRegion(Convert.ToInt32(resultArray[9])), Convert.ToBoolean(resultArray[10]), Convert.ToBoolean(resultArray[11]), Convert.ToBoolean(resultArray[12]));
                    result = legalEntity;
                    break;
                case "MiscellaneousList":
                    Miscellaneous miscellaneous = new Miscellaneous(Convert.ToInt32(resultArray[0]), GetProject(Convert.ToInt32(resultArray[1])), resultArray[2]);
                    result = miscellaneous;
                    break;
                case "Offers":
                    Offer offer = new Offer(Convert.ToInt32(resultArray[0]), Convert.ToBoolean(resultArray[1]), Convert.ToDateTime(resultArray[2]), Convert.ToDouble(resultArray[3]), Convert.ToBoolean(resultArray[4]));
                    result = offer;
                    break;
                case "Projects":
                    Project project = new Project(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), resultArray[2], GetBuilder(Convert.ToInt32(resultArray[3])), GetProjectStatus(Convert.ToInt32(resultArray[4])), GetTenderForm(Convert.ToInt32(resultArray[5])), GetEnterpriseForm(Convert.ToInt32(resultArray[6])), GetUser(Convert.ToInt32(resultArray[7])), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
                    result = project;
                    break;
                case "ProjectStatusList":
                    ProjectStatus projectStatus = new ProjectStatus(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = projectStatus;
                    break;
                case "Regions":
                    Region region = new Region(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2]);
                    result = region;
                    break;
                case "Requests":
                    Request request = new Request(Convert.ToInt32(resultArray[0]), GetRequestStatus(Convert.ToInt32(resultArray[1])), Convert.ToDateTime(resultArray[2]), Convert.ToDateTime(resultArray[3]));
                    result = request;
                    break;
                case "RequestStatusList":
                    RequestStatus requestStatus = new RequestStatus(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = requestStatus;
                    break;
                case "SubEntrepeneurs":
                    SubEntrepeneur subEntrepeneur = new SubEntrepeneur(Convert.ToInt32(resultArray[0]), GetEnterprise(Convert.ToInt32(resultArray[1])), GetLegalEntity(resultArray[2]), GetContact(Convert.ToInt32(resultArray[3])), GetRequest(Convert.ToInt32(resultArray[4])), GetIttLetter(Convert.ToInt32(resultArray[5])), GetOffer(Convert.ToInt32(resultArray[6])), Convert.ToBoolean(resultArray[7]), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]), Convert.ToBoolean(resultArray[10]));
                    result = subEntrepeneur;
                    break;
                case "TenderForms":
                    TenderForm tenderForm = new TenderForm(Convert.ToInt32(resultArray[0]), resultArray[1]);
                    result = tenderForm;
                    break;
                case "TimeSchedules":
                    TimeSchedule schedule = new TimeSchedule(GetProject(Convert.ToInt32(resultArray[0])), resultArray[1]);
                    result = schedule;
                    break;
                case "Users":
                    User user = new User(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], GetContactInfo(Convert.ToInt32(resultArray[4])), GetJobDescription(Convert.ToInt32(resultArray[5])), Convert.ToBoolean(resultArray[6]));
                    result = user;
                    break;
                case "ZipTownList":
                    ZipTown zipTown = new ZipTown(resultArray[0], resultArray[1]);
                    result = zipTown;
                    break;
            }

            return result;
        }

        public List<object> ReadListFromDb(string list)
        {
            List<object> result = new List<object>();

            List<string> results = ReadStringListFromDb(list);

            int fieldAmount = GetFieldAmount(list);

            foreach (string strResult in results)
            {
                string[] resultArray = new string[fieldAmount];
                resultArray = strResult.Split(';');
                object obj = GetObject(list, resultArray);
                result.Add(obj);
            }

            return result;
        }

        /// <summary>
        /// Method, that reads a string List from dB
        /// </summary>
        /// <param name="list">List<string></param>
        /// <returns></returns>
        private List<string> ReadStringListFromDb(string list)
        {
            List<string> results = executor.ReadListFromDataBase(list);

            return results;
        }

        #endregion

        #region Update
        /// <summary>
        /// Method, that updates an entity in Db
        /// </summary>
        /// <param name="entityType">string</param>
        /// <param name="entity">object</param>
        /// <returns>bool</returns>
        public bool UpdateInDb(object entity)
        {
            bool result = false;
            string entityType = entity.GetType().ToString();

            switch (entityType)
            {
                case "Address":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Addresses", new Address((Address)entity)));
                    break;
                case "BluePrint":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("BluePrints", new BluePrint((BluePrint)entity)));
                    break;
                case "Builder":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Builders", new Builder((Builder)entity)));
                    break;
                case "Category":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Categories", new Category((Category)entity)));
                    break;
                case "Contact":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Contacts", new Contact((Contact)entity)));
                    break;
                case "ContactInfo":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ContactInfoList", new ContactInfo((ContactInfo)entity)));
                    break;
                case "CraftGroup":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("CraftGroups", new CraftGroup((CraftGroup)entity)));
                    break;
                case "Description":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Descriptions", new Description((Description)entity)));
                    break;
                case "Enterprise":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Enterprises", new Enterprise((Enterprise)entity)));
                    break;
                case "EnterpriseForm":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("EnterpriseForms", new EnterpriseForm((EnterpriseForm)entity)));
                    break;
                case "IttLetter":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetters", new Enterprise((Enterprise)entity)));
                    break;
                case "IttLetterBullet":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetterBullets", new IttLetterBullet((IttLetterBullet)entity)));
                    break;
                case "IttLetterParagraph":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetterParagraphs", new IttLetterParagraph((IttLetterParagraph)entity)));
                    break;
                case "IttLetterPdfData":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetterPdfDataList", new IttLetterPdfData((IttLetterPdfData)entity)));
                    break;
                case "IttLetterShipping":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetterShippingList", new IttLetterShipping((IttLetterShipping)entity)));
                    break;
                case "JobDescription":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("JobDescriptions", new JobDescription((JobDescription)entity)));
                    break;
                case "LegalEntity":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("LegalEntities", new LegalEntity((LegalEntity)entity)));
                    break;
                case "Miscellaneous":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("MiscellaneousList", new Miscellaneous((Miscellaneous)entity)));
                    break;
                case "Offer":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Offers", new Offer((Offer)entity)));
                    break;
                case "Project":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Projects", new Project((Project)entity)));
                    break;
                case "ProjectStatus":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ProjectStatusList", new ProjectStatus((ProjectStatus)entity)));
                    break;
                case "Region":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Regions", new Region((Region)entity)));
                    break;
                case "Request":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Requests", new Request((Request)entity)));
                    break;
                case "RequestStatus":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("RequestStatusList", new RequestStatus((RequestStatus)entity)));
                    break;
                case "SubEntrepeneur":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("SubEntrepeneurs", new SubEntrepeneur((SubEntrepeneur)entity)));
                    break;
                case "TenderForm":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("TenderForms", new TenderForm((TenderForm)entity)));
                    break;
                case "TimeSchedule":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("TimeSchedules", new TimeSchedule((TimeSchedule)entity)));
                    break;
                case "User":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("Users", new User((User)entity)));
                    break;
                case "ZipTown":
                    result = ProcesSqlQuery(GetSQLQueryUpdate("ZipTown", new ZipTown((ZipTown)entity)));
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that returns a SQL-query
        /// Accepted lists:  Addresses, BluePrints, Contacts, ContactInfoList, CraftGroups, EnterpriseForms, EnterprisList, IttLetters, IttLetterPdfDataList, Project, Ziptown
        /// </summary>
        /// <param name="list">string</param>
        /// <param name="entity">object</param>
        /// <returns></returns>
        private string GetSQLQueryUpdate(string list, object entity)
        {
            string result = "";

            switch (list)
            {
                //UPDATE table_name SET column1 = value1, column2 = value2, ... WHERE condition;
                case "Addresses":
                    Address address = new Address((Address)entity);
                    result = @"UPDATE [dbo].[Addresses] SET [Street] = '" + address.Street + "', [Place] = '" + address.Place + "', [Zip] = " + address.ZipTown.Id + " WHERE [Id] = '" + address.Id + "';";
                    break;
                case "BluePrints":
                    BluePrint bluePrint = new BluePrint((BluePrint)entity);
                    result = @"UPDATE[dbo].[BluePrints] SET [Project] = " + bluePrint.Project.Id + @", [Name] = '" + bluePrint.Name + @"', [Description] = " + bluePrint.Description.Id + @", [Url] = '" + bluePrint.Url + @"' WHERE [Id] = " + bluePrint.Id;
                    break;
                case "Builders":
                    Builder builder = new Builder((Builder)entity);
                    result = @"UPDATE[dbo].[Builders] SET [Cvr] = " + builder.Cvr + @", [Name] = '" + builder.Name + @"', [Address] = '" + builder.Address.Id + @"', [ContactInfo] = '" + builder.ContactInfo.Id + @"', [Url] = '" + builder.Url + @"' WHERE [Id] = " + builder.Id;
                    break;
                case "Categories":
                    Category category = new Category((Category)entity);
                    result = @"UPDATE[dbo].[Categories] SET [Name] = '" + category.Name + @"' WHERE[Id] = " + category.Id;
                    break;
                case "Contacts":
                    Contact contact = new Contact((Contact)entity);
                    result = @"UPDATE [dbo].[Contacts] SET [LegalEntity] = '" + contact.LegalEntity.Id + "', [Name] = '" + contact.Name + "', [Area] = '" + contact.Area + "', [ContactInfo] = '" + contact.ContactInfo.Id + "' WHERE [Id] = " + contact.Id;
                    break;
                case "ContactInfoList":
                    ContactInfo info = new ContactInfo((ContactInfo)entity);
                    result = @"UPDATE [dbo].[Contacts] SET [Phone] = '" + info.Phone + "', [Fax] = '" + info.Fax + "', [Mobile] = '" + info.Mobile + "', [Email] = '" + info.Email + "' WHERE [Id] = " + info.Id;
                    break;
                case "CraftGroups":
                    CraftGroup craftGroup = new CraftGroup((CraftGroup)entity);
                    result = @"UPDATE [dbo].[CraftGroup] SET [Category] = '" + craftGroup.Category.Id + "', [Designation] = '" + craftGroup.Designation + "', [Description] = '" + craftGroup.Description + "', [Active] = '" + craftGroup.Active.ToString() + "' WHERE [Id] = " + craftGroup.Id;
                    break;
                case "Descriptions":
                    Description description = new Description((Description)entity);
                    result = "UPDATE[dbo].[Descriptions] SET [Project] = " + description.Project.Id + ", [Enterprise] = " + description.Enterprise.Id + ", [Text] = '" + description.Text + "' WHERE[Id] = " + description.Id;
                    break;
                case "EnterpriseForms":
                    EnterpriseForm form = new EnterpriseForm((EnterpriseForm)entity);
                    result = @"UPDATE [dbo].[EnterpriseForms] SET [Name] = '" + form.Name + @"' WHERE [Id] = " + form.Id;
                    break;
                case "Enterprises":
                    Enterprise enterprise = new Enterprise((Enterprise)entity);
                    result = @"UPDATE [dbo].[Enterprises] SET [Project] = " + enterprise.Project.Id.ToString() + @", [Name] = '" + enterprise.Name + @"', [Elaboration] = '" + enterprise.Elaboration + @"', [OfferList] = '" + enterprise.OfferList + @"', [CraftGroup1] = " + enterprise.CraftGroup1.Id.ToString() + @", [CraftGroup2] = " + enterprise.CraftGroup2.Id.ToString() + @", [CraftGroup3] = " + enterprise.CraftGroup3.Id.ToString() + @", [CraftGroup4] = " + enterprise.CraftGroup4.Id.ToString() + @" WHERE [Id] = " + enterprise.Id;
                    break;
                case "IttLetters":
                    IttLetter ittLetter = new IttLetter((IttLetter)entity);
                    result = @"UPDATE [dbo].[IttLetters] SET [Sent] = '" + ittLetter.Sent + @"', [SentDate] = " + ittLetter.SentDate + "' WHERE [Id] = " + ittLetter.Id;
                    break;
                case "IttLetterBullets":
                    IttLetterBullet bullet = new IttLetterBullet((IttLetterBullet)entity);
                    result = @"UPDATE dbo.IttLetterBullets SET [Paragraph] = " + bullet.Paragraph.Id.ToString() + @", [Name] = '" + bullet.Text + @" WHERE [Id] = " + bullet.Id;
                    break;
                case "IttLetterParagraphs":
                    IttLetterParagraph paragraph = new IttLetterParagraph((IttLetterParagraph)entity);
                    result = @"UPDATE [dbo].[IttLetterParagraphs] SET [Project] = " + paragraph.Project.Id + @", [Name] = '" + paragraph.Name + @"' WHERE[Id] = " + paragraph.Id;
                    break;
                case "IttLetterPdfDataList":
                    IttLetterPdfData pdfData = new IttLetterPdfData((IttLetterPdfData)entity);
                    result = @"UPDATE [dbo].[IttLetterPdfDataList] SET [Project] = " + pdfData.Project.Id + @", [Builder] = " + pdfData.Builder.Id + @", [AnswerDate] = '" + pdfData.AnswerDate + @"', [QuestionDate] = '" + pdfData.QuestionDate + @"', [CorrectionSheetDate] = '" + pdfData.CorrectionSheetDate + @"', [TimeSpan] = '" + pdfData.TimeSpan + @"', [MaterialUrl] = '" + pdfData.MaterialUrl + @"', [ConditionUrl] = '" + pdfData.ConditionUrl + @"', [PassWord] = '" + pdfData.PassWord + @"' WHERE [Id] = " + pdfData.Id;
                    break;
                case "IttLetterShippingList":
                    IttLetterShipping shipping = new IttLetterShipping((IttLetterShipping)entity);
                    result = @"UPDATE [dbo].[IttLetterShippingList] SET [Project] = " + shipping.Project.Id + @", [CommonPdfPath] = '" + shipping.CommonPdfPath + @"', [PdfPath] = '" + shipping.PdfPath + @"' WHERE [Id] = " + shipping.Id;
                    break;
                case "JobDescriptions":
                    JobDescription jobDescription = new JobDescription((JobDescription)entity);
                    result = @"UPDATE [dbo].[Regions] SET [Occupation] = " + jobDescription.Occupation + ", [Area] = '" + jobDescription.Area + "', [Procuration] = '" + jobDescription.Procuration.ToString() + "' WHERE [Id] = " + jobDescription.Id.ToString();
                    break;
                case "LegalEntities":
                    LegalEntity legalEntity = new LegalEntity((LegalEntity)entity);
                    result = @"UPDATE [dbo].[LegalEntities] SET [Name] = " + legalEntity.Name + @", [Address] = " + legalEntity.Address.Id + @", [ContactInfo] = " + legalEntity.ContactInfo.Id + @", [Url] = '" + legalEntity.Url + @", [CraftGroup1] = " + legalEntity.CraftGroup1.Id + @", [CraftGroup2] = " + legalEntity.CraftGroup2.Id + @", [CraftGroup3] = " + legalEntity.CraftGroup3.Id + @", [CraftGroup4] = " + legalEntity.CraftGroup4.Id + @", [Region] = " + legalEntity.Region.Id + @", [CountryWide] = '" + legalEntity.CountryWide.ToString() + @"', [Cooperative] = '" + legalEntity.Cooperative.ToString() + @"', [Active] = '" + legalEntity.Active.ToString() + @"' WHERE [Id] = " + legalEntity.Id;
                    break;
                case "MiscellaneousList":
                    Miscellaneous miscellaneous = new Miscellaneous((Miscellaneous)entity);
                    result = @"UPDATE [dbo].[MiscellaneousList] SET [Project] = " + miscellaneous.Project.Id + @", [Text] = '" + miscellaneous.Text + @"' WHERE [Id] = " + miscellaneous.Id;
                    break;
                case "Offers":
                    Offer offer = new Offer((Offer)entity);
                    result = @"UPDATE [dbo].[Offers] SET [Received] = " + offer.Received.ToString() + @", [ReceivedDate] = " + offer.ReceivedDate.ToShortDateString() + @", [Price] = " + offer.Price + @", [Chosen] = '" + offer.Chosen.ToString() + @"' WHERE [Id] = " + offer.Id;
                    break;
                case "Projects":
                    Project project = new Project((Project)entity);
                    result = @"UPDATE dbo.[Projects] SET [CaseId] = " + project.CaseId + ", [Name] = '" + project.Name + "', [Builder] = " + project.Builder.Id + ", [Status] = " + project.Status.Id + ", [TenderForm] = " + project.TenderForm.Id + ", [EnterpriseForm] = " + project.EnterpriseForm.Id + ", [Executive] = " + project.Executive.Id + ", [Enterprises] = '" + project.EnterprisesList.ToString() + "', [Copy] = '" + project.Copy.ToString() + "' WHERE [Id] = " + project.Id;
                    break;
                case "ProjectStatusList":
                    ProjectStatus projectStatus = new ProjectStatus((ProjectStatus)entity);
                    result = @"UPDATE dbo.[ProjectStatusList] SET [Description] = '" + projectStatus.Description + "' WHERE Id = " + projectStatus.Id;
                    break;
                case "Regions":
                    Region region = new Region((Region)entity);
                    result = @"UPDATE [dbo].[Regions] SET [RegionName] = '" + region.RegionName + ", [Zips] = '" + region.Zips + "' WHERE [Id] = " + region.Id;
                    break;
                case "Requests":
                    Request request = new Request((Request)entity);
                    result = @"UPDATE [dbo].[Requests] SET [Status] = " + request.Status.Id + ", [SentDate] = '" + request.SentDate.ToShortDateString() + "', [ReceivedDate] = '" + request.ReceivedDate.ToShortDateString() + "' WHERE [Id] = " + request.Id.ToString();
                    break;
                case "RequestStatusList":
                    RequestStatus requestStatus = new RequestStatus((RequestStatus)entity);
                    result = @"UPDATE [dbo].[RequestStatusList] SET [Description] = '" + "' WHERE [Id] = " + requestStatus.Id.ToString();
                    break;
                case "SubEntrepeneurs":
                    SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)entity);
                    result = @"UPDATE [dbo].[SubEntrepeneurs] SET [Enterprise] = " + subEntrepeneur.Enterprise.Id + ", [Entrepeneur] = '" + subEntrepeneur.Entrepeneur + "', [Contact] = " + subEntrepeneur.Contact.Id + ", [Request] = " + subEntrepeneur.Request.Id + "', [IttLetter] = " + subEntrepeneur.IttLetter.Id + ", [Offer] = " + subEntrepeneur.Offer.Id + ", [Reservations] = '" + subEntrepeneur.Reservations.ToString() + ", [Uphold] = '" + subEntrepeneur.Uphold.ToString() + ", [AgreementConcluded] = '" + subEntrepeneur.AgreementConcluded.ToString() + ", [Active] = '" + subEntrepeneur.Active.ToString() + "' WHERE [Id] = " + subEntrepeneur.Id;
                    break;
                case "TenderForms":
                    TenderForm tenderForm = new TenderForm((TenderForm)entity);
                    result = @"UPDATE [dbo].[TenderForms] SET [description] = '" + tenderForm.Description + "' WHERE [Id] = " + tenderForm.Id;
                    break;
                case "TimeSchedules":
                    TimeSchedule schedule = new TimeSchedule((TimeSchedule)entity);
                    result = @"UPDATE [dbo].[TimeSchedules] SET [Project] = " + schedule.Project.Id + ", [Text] = '" + schedule.Text + "' WHERE [Id] = " + schedule.Id;
                    break;
                case "Users":
                    User user = new User((User)entity);
                    result = @"UPDATE [dbo].[Users] SET [Initials] = '" + user.Initials + "', [Name] = '" + user.Name + "', [PassWord] = '" + user.PassWord + "', [ContactInfo] = " + user.ContactInfo.Id + ", [JobDescription] = " + user.JobDescription.Id + ", [Admiistrator] = '" + user.Administrator.ToString() + "' WHERE [Id] = " + user.Id;
                    break;
                case "ZipTown":
                    ZipTown zipTown = new ZipTown((ZipTown)entity);
                    result = @"UPDATE dbo.[ZipTown] SET [Town] = '" + zipTown.Town + "' WHERE [Zip] = '" + zipTown.Zip + "'";
                    break;
                default:
                    result = "";
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
            string result = "";
            //DELETE FROM table_name WHERE condition;
            switch (list)
            {
                case "EnterpriseForms":
                    result = @"DELETE FROM [dbo].[EnterpriseForms] WHERE [Abvreviation] = '" + id + "'";
                    break;
                case "ZipTown":
                    result = @"DELETE FROM [dbo].[ZipTown] WHERE [Zip] = '" + id + "'";
                    break;
                default:
                    result = @"DELETE FROM [dbo].[" + list + @"] WHERE [Id] = " + id + ";";
                    break;
            }
            return result;
        }

        #endregion

        #endregion

        #region Get List for a project
        /// Method, that retrieves an BluePrints list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<BluePrint></returns>
        public List<BluePrint> GetBluePrints(int projectId)
        {
            List<BluePrint> result = new List<BluePrint>();

            foreach (BluePrint bluePrint in BluePrints)
            {
                if (bluePrint.Project.Id == projectId)
                {
                    result.Add(bluePrint);
                }
            }
            return result;
        }


        /// <summary>
        /// Method, that retrieves an Descriptions list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<Description></returns>
        public List<Description> GetDescriptions(int projectId)
        {
            List<Description> result = new List<Description>();

            foreach (Description description in Descriptions)
            {
                if (description.Project.Id == projectId)
                {
                    result.Add(description);
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves an Enterprise List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<Enterprise></returns>
        public List<Enterprise> GetEnterprises(int projectId)
        {
            List<Enterprise> result = new List<Enterprise>();

            foreach (Enterprise enterprise in Enterprises)
            {
                if (enterprise.Project.Id == projectId)
                {
                    result.Add(enterprise);
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves an IttLetterReceivers list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<IttLetterReceiver></returns>
        public List<IttLetterReceiver> GetIttLetterReceivers(int projectId)
        {
            List<IttLetterReceiver> result = new List<IttLetterReceiver>();

            foreach (IttLetterReceiver receiver in IttLetterReceivers)
            {
                if (receiver.Project.Id == projectId)
                {
                    result.Add(receiver);
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves an IttLetterShipping List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<IttLetterShipping></returns>
        public List<IttLetterShipping> GetIttLetterShippingList(int projectId)
        {
            List<IttLetterShipping> result = new List<IttLetterShipping>();

            foreach (IttLetterShipping shipping in IttLetterShippingList)
            {
                if (shipping.Project.Id == projectId)
                {
                    result.Add(shipping);
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves a Miscellaneous List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<Miscellaneous></returns>
        public List<Miscellaneous> GetMiscellaneousList(int projectId)
        {
            List<Miscellaneous> result = new List<Miscellaneous>();

            foreach (Miscellaneous miscellaneous in MiscellaneousList)
            {
                if (miscellaneous.Project.Id == projectId)
                {
                    result.Add(miscellaneous);
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves a TimeSchedules list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<TimeSchedule></returns>
        public List<TimeSchedule> GetTimeSchedules(int projectId)
        {
            List<TimeSchedule> result = new List<TimeSchedule>();

            foreach (TimeSchedule schedule in TimeSchedules)
            {
                if (schedule.Project.Id == projectId)
                {
                    result.Add(schedule);
                }
            }
            return result;
        }

        #endregion

        #region Get an entity from a List
        /// <summary>
        /// Method, that fetches an entity from Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>object</returns>
        public object GetEntity(string entity, string id)
        {
            object result = new object();
            switch (entity)
            {
                case "Address":
                    result = GetAddress(Convert.ToInt32(id));
                    break;
                case "BluePrint":
                    result = GetBluePrint(Convert.ToInt32(id));
                    break;
                case "Builder":
                    result = GetBuilder(Convert.ToInt32(id));
                    break;
                case "Category":
                    result = GetCategory(Convert.ToInt32(id));
                    break;
                case "Contact":
                    result = GetContact(Convert.ToInt32(id));
                    break;
                case "ContactInfo":
                    result = GetCategory(Convert.ToInt32(id));
                    break;
                case "CraftGroup":
                    result = GetCraftGroup(Convert.ToInt32(id));
                    break;
                case "Description":
                    result = GetDescription(Convert.ToInt32(id));
                    break;
                case "Enterprise":
                    result = GetEnterprise(Convert.ToInt32(id));
                    break;
                case "EnterpriseForm":
                    result = GetEnterpriseForm(Convert.ToInt32(id));
                    break;
                case "IttLetter":
                    result = GetIttLetter(Convert.ToInt32(id));
                    break;
                case "IttLetterBullet":
                    result = GetIttLetterBullet(Convert.ToInt32(id));
                    break;
                case "IttLetterParagraph":
                    result = GetIttLetterParagraph(Convert.ToInt32(id));
                    break;
                case "IttLetterPdfData":
                    result = GetIttLetterPdfData(Convert.ToInt32(id));
                    break;
                case "IttLetterShipping":
                    result = GetIttLetterShipping(Convert.ToInt32(id));
                    break;
                case "JobDescription":
                    result = GetJobDescription(Convert.ToInt32(id));
                    break;
                case "LegalEntity":
                    result = GetLegalEntity(id);
                    break;
                case "Miscellaneous":
                    result = GetMiscellaneous(Convert.ToInt32(id));
                    break;
                case "Offer":
                    result = GetOffer(Convert.ToInt32(id));
                    break;
                case "Project":
                    result = GetEnterprise(Convert.ToInt32(id));
                    break;
                case "ProjectStatus":
                    result = GetProjectStatus(Convert.ToInt32(id));
                    break;
                case "Region":
                    result = GetRegion(Convert.ToInt32(id));
                    break;
                case "Request":
                    result = GetRequest(Convert.ToInt32(id));
                    break;
                case "RequestStatus":
                    result = GetRequestStatus(Convert.ToInt32(id));
                    break;
                case "SubEntrepeneur":
                    result = GetSubEntrepeneur(Convert.ToInt32(id));
                    break;
                case "TenderForm":
                    result = GetTenderForm(Convert.ToInt32(id));
                    break;
                case "TimeSchedule":
                    result = GetTimeSchedule(Convert.ToInt32(id));
                    break;
                case "User":
                    result = GetUser(Convert.ToInt32(id));
                    break;
                case "ZipTown":
                    result = GetZipTown(Convert.ToInt32(id));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that reretrieves an Address from Id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns>Address</returns>
        private Address GetAddress(int addressId)
        {
            Address result = new Address();

            foreach (Address address in Addresses)
            {
                if (address.Id == addressId)
                {
                    result = address;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that reretrieves a BluePrint from Id
        /// </summary>
        /// <param name="bluePrintId"></param>
        /// <returns>Address</returns>
        private BluePrint GetBluePrint(int bluePrintId)
        {
            BluePrint result = new BluePrint();

            foreach (BluePrint bluePrint in BluePrints)
            {
                if (bluePrint.Id == bluePrintId)
                {
                    result = bluePrint;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// <summary>
        /// Method, that reretrieves an Builder from Id
        /// </summary>
        /// <param name="builderId"></param>
        /// <returns>Builder</returns>
        private Builder GetBuilder(int builderId)
        {
            Builder result = new Builder();

            foreach (Builder builder in Builders)
            {
                if (builder.Id == builderId)
                {
                    result = builder;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that reretrieves a Category from Id
        /// </summary>
        /// <param name="categoryId">int</param>
        /// <returns>Category</returns>
        private Category GetCategory(int categoryId)
        {
            Category result = new Category();

            foreach (Category category in Categories)
            {
                if (category.Id == categoryId)
                {
                    result = category;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that reretrieves a CraftGroup  from Id
        /// </summary>
        /// <param name="craftGroupIid">int</param>
        /// <returns>CraftGroup</returns>
        private CraftGroup GetCraftGroup(int craftGroupIid)
        {
            CraftGroup result = new CraftGroup();
            foreach (CraftGroup craftGroup in CraftGroups)
            {
                if (craftGroup.Id == craftGroupIid)
                {
                    result = craftGroup;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a Contact from Id
        /// </summary>
        /// <param name="contactId">int</param>
        /// <returns>Contact</returns>
        private Contact GetContact(int contactId)
        {
            Contact result = new Contact();
            foreach (Contact contact in Contacts)
            {
                if (contact.Id == contactId)
                {
                    result = contact;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a ContactInfo from Id
        /// </summary>
        /// <param name="contactInfoId">int</param>
        /// <returns>ContactInfo</returns>
        private ContactInfo GetContactInfo(int contactInfoId)
        {
            ContactInfo result = new ContactInfo();
            foreach (ContactInfo contactInfo in ContactInfoList)
            {
                if (contactInfo.Id == contactInfoId)
                {
                    result = contactInfo;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a Description from Id
        /// </summary>
        /// <param name="descriptionId">int</param>
        /// <returns>Contact</returns>
        private Description GetDescription(int descriptionId)
        {
            Description result = new Description();
            foreach (Description description in Descriptions)
            {
                if (description.Id == descriptionId)
                {
                    result = description;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an Enterprise from Id
        /// </summary>
        /// <param name="enterpriseId">int</param>
        /// <returns>Enterprise</returns>
        private Enterprise GetEnterprise(int enterpriseId)
        {
            Enterprise result = new Enterprise();
            foreach (Enterprise enterprise in Enterprises)
            {
                if (enterprise.Id == enterpriseId)
                {
                    result = enterprise;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an Enterprise from Id
        /// </summary>
        /// <param name="abbreviation">string</param>
        /// <returns>EnterpriseForm</returns>
        private EnterpriseForm GetEnterpriseForm(int formId)
        {
            EnterpriseForm result = new EnterpriseForm();
            foreach (EnterpriseForm form in EnterpriseForms)
            {
                if (form.Id == formId)
                {
                    result = form;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an IttLetter from Id
        /// </summary>
        /// <param name="ittLetterId">int</param>
        /// <returns>IttLetter</returns>
        private IttLetter GetIttLetter(int ittLetterId)
        {
            IttLetter result = new IttLetter();

            foreach (IttLetter letter in IttLetters)
            {
                if (letter.Id == ittLetterId)
                {
                    result = letter;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an IttLetter Bullet from Id
        /// </summary>
        /// <param name="bulletId">int</param>
        /// <returns>IttLetterBullet</returns>
        private IttLetterBullet GetIttLetterBullet(int bulletId)
        {
            IttLetterBullet result = new IttLetterBullet();

            foreach (IttLetterBullet bullet in IttLetterBullets)
            {
                if (bullet.Id == bulletId)
                {
                    result = bullet;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an IttLetterParagraph from Id
        /// </summary>
        /// <param name="paragraphId">int</param>
        /// <returns>IttLetterParagraph</returns>
        private IttLetterParagraph GetIttLetterParagraph(int paragraphId)
        {
            IttLetterParagraph result = new IttLetterParagraph();

            foreach (IttLetterParagraph paragraph in IttLetterParagraphs)
            {
                if (paragraph.Id == paragraphId)
                {
                    result = paragraph;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an IttLetter PdfData from Id
        /// </summary>
        /// <param name="pdfDataId">int</param>
        /// <returns>IttLetterPdfData</returns>
        private IttLetterPdfData GetIttLetterPdfData(int pdfDataId)
        {
            IttLetterPdfData result = new IttLetterPdfData();

            foreach (IttLetterPdfData pdfData in IttLetterPdfDataList)
            {
                if (pdfData.Id == pdfDataId)
                {
                    result = pdfData;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an IttLetterShipping from Id
        /// </summary>
        /// <param name="shippingId">int</param>
        /// <returns>IttLetterShipping</returns>
        private IttLetterShipping GetIttLetterShipping(int shippingId)
        {
            IttLetterShipping result = new IttLetterShipping();

            foreach (IttLetterShipping shipping in IttLetterShippingList)
            {
                if (shipping.Id == shippingId)
                {
                    result = shipping;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a JobDescription from Id
        /// </summary>
        /// <param name="JobDescriptionId">int</param>
        /// <returns></returns>
        private JobDescription GetJobDescription(int JobDescriptionId)
        {
            JobDescription result = new JobDescription();

            foreach (JobDescription description in JobDescriptions)
            {
                if (description.Id == JobDescriptionId)
                {
                    result = description;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a LegalEntity from Id
        /// </summary>
        /// <param name="legalEntityId">string</param>
        /// <returns>LegalEntity</returns>
        private LegalEntity GetLegalEntity(string legalEntityId)
        {
            LegalEntity result = new LegalEntity();

            foreach (LegalEntity legalEntity in LegalEntities)
            {
                if (legalEntity.Id == legalEntityId)
                {
                    result = legalEntity;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a LegalEntity from Id
        /// </summary>
        /// <param name="miscellaneousId">string</param>
        /// <returns>Miscellaneous</returns>
        private Miscellaneous GetMiscellaneous(int miscellaneousId)
        {
            Miscellaneous result = new Miscellaneous();

            foreach (Miscellaneous miscellaneous in MiscellaneousList)
            {
                if (miscellaneous.Id == miscellaneousId)
                {
                    result = miscellaneous;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves an Offer from Id
        /// </summary>
        /// <param name="offerId">int</param>
        /// <returns>Offer</returns>
        private Offer GetOffer(int offerId)
        {
            Offer result = new Offer();

            foreach (Offer offer in Offers)
            {
                if (offer.Id == offerId)
                {
                    result = offer;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a Project from Db
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>Project</returns>
        private Project GetProject(int projectId)
        {
            Project result = new Project();

            foreach (Project project in Projects)
            {
                if (project.Id == projectId)
                {
                    result = project;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a Project Status from Id
        /// </summary>
        /// <param name="statusId">int</param>
        /// <returns>Project</returns>
        private ProjectStatus GetProjectStatus(int statusId)
        {
            ProjectStatus result = new ProjectStatus();

            foreach (ProjectStatus status in ProjectStatusList)
            {
                if (status.Id == statusId)
                {
                    result = status;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Region from Id
        /// </summary>
        /// <param name="regionId">int</param>
        /// <returns>Region</returns>
        private Region GetRegion(int regionId)
        {
            Region result = new Region();

            foreach (Region region in Regions)
            {
                if (region.Id == regionId)
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
        /// <param name="requestId">int</param>
        /// <returns>Request</returns>
        private Request GetRequest(int requestId)
        {
            Request result = new Request();

            foreach (Request request in Requests)
            {
                if (request.Id == requestId)
                {
                    result = request;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a Request Status from Id
        /// </summary>
        /// <param name="statusId">int</param>
        /// <returns>Request</returns>
        private RequestStatus GetRequestStatus(int statusId)
        {
            RequestStatus result = new RequestStatus();

            foreach (RequestStatus status in RequestStatusList)
            {
                if (status.Id == statusId)
                {
                    result = status;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a SubEntrepeneur from Id
        /// </summary>
        /// <param name="subId">int</param>
        /// <returns>SubEntrepeneur</returns>
        private SubEntrepeneur GetSubEntrepeneur(int subId)
        {
            SubEntrepeneur result = new SubEntrepeneur();

            foreach (SubEntrepeneur sub in SubEntrepeneurs)
            {
                if (sub.Id == subId)
                {
                    result = sub;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a Tender Form from Id
        /// </summary>
        /// <param name="formId">int</param>
        /// <returns>Project</returns>
        private TenderForm GetTenderForm(int formId)
        {
            TenderForm result = new TenderForm();

            foreach (TenderForm form in TenderForms)
            {
                if (form.Id == formId)
                {
                    result = form;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a Time Schedule from Id
        /// </summary>
        /// <param name="scheduleId">int</param>
        /// <returns>TimeSchedule</returns>
        private TimeSchedule GetTimeSchedule(int scheduleId)
        {
            TimeSchedule result = new TimeSchedule();

            foreach (TimeSchedule schedule in TimeSchedules)
            {
                if (schedule.Id == scheduleId)
                {
                    result = schedule;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a User from Id
        /// </summary>
        /// <param name="userId">int</param>
        /// <returns>List<User></returns>
        private User GetUser(int userId)
        {
            User result = new User();

            foreach (User user in Users)
            {
                if (user.Id == userId)
                {
                    result = user;
                    break;
                }
            }

            if (result == new User())
            {
                result = GetUser(0);
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a ZipTown from zip
        /// </summary>
        /// <param name="zip">string</param>
        /// <returns>ZipTown</returns>
        private ZipTown GetZipTown(int zipId)
        {
            ZipTown result = new ZipTown();

            foreach (ZipTown zipTown in ZipTownList)
            {
                if (zipTown.Id == zipId)
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
            RefreshBluePrints();
            RefreshCategories();
            RefreshContactInfoList();
            RefreshDescriptions();
            RefreshEnterpriseForms();
            RefreshIttLetters();
            RefreshJobDescriptions();
            RefreshMiscellaneousList();
            RefreshOffers();
            RefreshProjectStatusList();
            RefreshRegions();
            RefreshRequestStatusList();
            RefreshTenderforms();
            RefreshZipTownList();

            //Two level Lists
            RefreshAddresses();
            RefreshCraftGroups();
            RefreshRequests();
            RefreshUsers();

            //Three level list
            RefreshLegalEntities();
            RefreshProjects();
            RefreshBuilders();

            //Four Level List
            RefreshContacts();
            RefreshEnterprises();

            //Five level Lists
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
                case "Addresses":
                    RefreshAddresses();
                    break;
                case "BluePrints":
                    RefreshBluePrints();
                    break;
                case "Builders":
                    RefreshBuilders();
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
                case "Descriptions":
                    RefreshDescriptions();
                    break;
                case "EnterpriseForms":
                    RefreshEnterpriseForms();
                    break;
                case "Enterprises":
                    RefreshEnterprises();
                    break;
                case "IttLetterBulletList":
                    RefreshIttLetterBullets();
                    break;
                case "IttLetterParagraphs":
                    RefreshIttLetterParagraphs();
                    break;
                case "IttLetterPdfDataList":
                    RefreshIttLetterPdfDataList();
                    break;
                case "IttLetterReceivers":
                    RefreshIttLetterReceivers();
                    break;
                case "IttLetters":
                    RefreshIttLetters();
                    break;
                case "IttLetterShippingList":
                    RefreshIttLetterShippingList();
                    break;
                case "JobDescriptions":
                    RefreshJobDescriptions();
                    break;
                case "LegalEntities":
                    RefreshLegalEntities();
                    break;
                case "MiscellaneousList":
                    RefreshMiscellaneousList();
                    break;
                case "Offers":
                    RefreshOffers();
                    break;
                case "Projects":
                    RefreshProjects();
                    break;
                case "ProjectStatusList":
                    RefreshProjectStatusList();
                    break;
                case "Regions":
                    RefreshRegions();
                    break;
                case "Requests":
                    RefreshRequests();
                    break;
                case "RequestStatusList":
                    RefreshRequestStatusList();
                    break;
                case "SubEntrepeneurs":
                    RefreshSubEntrepeneurs();
                    break;
                case "TenderForms":
                    RefreshTenderforms();
                    break;
                case "TimeSchedules":
                    RefreshTimeSchedules();
                    break;
                case "Users":
                    RefreshUsers();
                    break;
                case "ZipTownList":
                    RefreshZipTownList();
                    break;
            }
        }

        /// <summary>
        /// Method, that refreshes the Addresses list
        /// </summary>
        private void RefreshAddresses()
        {
            if (Addresses != null)
            {
                Addresses.Clear();
            }
            List<object> tempList = ReadListFromDb("Addresses");

            foreach (object obj in tempList)
            {
                Addresses.Add((Address)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the BluePrints list
        /// </summary>
        private void RefreshBluePrints()
        {
            if (BluePrints != null)
            {
                BluePrints.Clear();
            }
            List<object> tempList = ReadListFromDb("BluePrints");

            foreach (object obj in tempList)
            {
                BluePrints.Add((BluePrint)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Builders list
        /// </summary>
        private void RefreshBuilders()
        {
            if (Builders != null)
            {
                Builders.Clear();
            }
            List<object> tempList = ReadListFromDb("Builders");

            foreach (object obj in tempList)
            {
                Builders.Add((Builder)obj);
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
        /// Method, that refreshes the CraftGroups list
        /// </summary>
        private void RefreshCraftGroups()
        {
            if (CraftGroups != null)
            {
                CraftGroups.Clear();
            }
            List<object> tempList = ReadListFromDb("CraftGroups");

            foreach (object obj in tempList)
            {
                CraftGroups.Add((CraftGroup)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Descriptions list
        /// </summary>
        private void RefreshDescriptions()
        {
            if (Descriptions != null)
            {
                Descriptions.Clear();
            }
            List<object> tempList = ReadListFromDb("Descriptions");

            foreach (object obj in tempList)
            {
                Descriptions.Add((Description)obj);
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
        private void RefreshEnterprises()
        {
            if (Enterprises != null)
            {
                Enterprises.Clear();
            }
            List<object> tempList = ReadListFromDb("Enterprises");

            foreach (object obj in tempList)
            {
                Enterprises.Add((Enterprise)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the IttLetter Bullet List
        /// </summary>
        private void RefreshIttLetterBullets()
        {
            if (IttLetterBullets != null)
            {
                IttLetterBullets.Clear();
            }
            List<object> tempList = ReadListFromDb("IttLetterBullets");

            foreach (object obj in tempList)
            {
                IttLetterBullets.Add((IttLetterBullet)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the IttLetter Paragraph List
        /// </summary>
        private void RefreshIttLetterParagraphs()
        {
            if (IttLetterParagraphs != null)
            {
                IttLetterParagraphs.Clear();
            }
            List<object> tempList = ReadListFromDb("IttLetterParagraphs");

            foreach (object obj in tempList)
            {
                IttLetterParagraphs.Add((IttLetterParagraph)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the IttLetter PdfData List
        /// </summary>
        private void RefreshIttLetterPdfDataList()
        {
            if (IttLetterPdfDataList != null)
            {
                IttLetterPdfDataList.Clear();
            }
            List<object> tempList = ReadListFromDb("IttLetterPdfDataList");

            foreach (object obj in tempList)
            {
                IttLetterPdfDataList.Add((IttLetterPdfData)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the IttLetter Receivers list
        /// </summary>
        private void RefreshIttLetterReceivers()
        {
            if (IttLetterReceivers != null)
            {
                IttLetterReceivers.Clear();
            }
            List<object> tempList = ReadListFromDb("IttLetterReceivers");

            foreach (object obj in tempList)
            {
                IttLetterReceivers.Add((IttLetterReceiver)obj);
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
        /// Method, that refreshes the IttLetter Shipping List
        /// </summary>
        private void RefreshIttLetterShippingList()
        {
            if (IttLetterShippingList != null)
            {
                IttLetterShippingList.Clear();
            }
            List<object> tempList = ReadListFromDb("IttLetterShippingList");

            foreach (object obj in tempList)
            {
                IttLetterShippingList.Add((IttLetterShipping)obj);
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
        /// Method, that refreshes the Miscellaneous List
        /// </summary>
        private void RefreshMiscellaneousList()
        {
            if (MiscellaneousList != null)
            {
                MiscellaneousList.Clear();
            }
            List<object> tempList = ReadListFromDb("MiscellaneousList");

            foreach (object obj in tempList)
            {
                MiscellaneousList.Add((Miscellaneous)obj);
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
        /// Method, that refreshes the Projects list
        /// </summary>
        private void RefreshProjects()
        {
            if (Projects != null)
            {
                Projects.Clear();
            }
            List<object> tempList = ReadListFromDb("Projects");

            foreach (object obj in tempList)
            {
                Projects.Add((Project)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the ProjectStatus List
        /// </summary>
        private void RefreshProjectStatusList()
        {
            if (ProjectStatusList != null)
            {
                ProjectStatusList.Clear();
            }
            List<object> tempList = ReadListFromDb("ProjectStatusList");

            foreach (object obj in tempList)
            {
                ProjectStatusList.Add((ProjectStatus)obj);
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
        private void RefreshRequests()
        {
            if (Requests != null)
            {
                Requests.Clear();
            }
            List<object> tempList = ReadListFromDb("Requests");

            foreach (object obj in tempList)
            {
                Requests.Add((Request)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Request Status List
        /// </summary>
        private void RefreshRequestStatusList()
        {
            if (RequestStatusList != null)
            {
                RequestStatusList.Clear();
            }
            List<object> tempList = ReadListFromDb("RequestStatusList");

            foreach (object obj in tempList)
            {
                RequestStatusList.Add((RequestStatus)obj);
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
        /// Method, that refreshes the TimeSchedules list
        /// </summary>
        private void RefreshTimeSchedules()
        {
            if (TimeSchedules != null)
            {
                TimeSchedules.Clear();
            }
            List<object> tempList = ReadListFromDb("TimeSchedules");

            foreach (object obj in tempList)
            {
                TimeSchedules.Add((TimeSchedule)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Users lists
        /// </summary>
        private void RefreshUsers()
        {
            if (Users != null)
            {
                Users.Clear();
            }
            List<object> tempList = ReadListFromDb("Users");

            foreach (object obj in tempList)
            {
                Users.Add((User)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the ZipTown List
        /// </summary>
        private void RefreshZipTownList()
        {
            if (ZipTownList != null)
            {
                ZipTownList.Clear();
            }
            List<object> tempList = ReadListFromDb("ZipTownList");

            foreach (object obj in tempList)
            {
                ZipTownList.Add((ZipTown)obj);
            }
        }

        #endregion

        #endregion

    }
}
