using JudDataAccess;
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
        public static string strConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JortonSubEnt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public PdfLists PdfLists;
        private Executor executor = new Executor(strConnection);
        private MacAddress macAddress = new MacAddress();

        #region Lists
            public List<Project> ActiveProjects = new List<Project>();
            public List<Address> Addresses = new List<Address>();
            public List<Authentication> Authentications = new List<Authentication>();
            public List<Builder> Builders = new List<Builder>();
            public List<Bullet> Bullets = new List<Bullet>();
            public List<Category> Categories = new List<Category>();
            public List<Contact> Contacts = new List<Contact>();
            public List<ContactInfo> ContactInfoList = new List<ContactInfo>();
            public List<CraftGroup> CraftGroups = new List<CraftGroup>();
            public List<EnterpriseForm> EnterpriseForms = new List<EnterpriseForm>();
            public List<Enterprise> Enterprises = new List<Enterprise>();
            public List<Entrepeneur> Entrepeneurs = new List<Entrepeneur>();
            public List<Project> InactiveProjects = new List<Project>();
            public List<IttLetter> IttLetters = new List<IttLetter>();
            public List<JobDescription> JobDescriptions = new List<JobDescription>();
            public List<LegalEntity> LegalEntities = new List<LegalEntity>();
            public List<LetterData> LetterDataList = new List<LetterData>();
            public List<Offer> Offers = new List<Offer>();
            public List<Paragraph> Paragraphs = new List<Paragraph>();
            public List<Person> Persons = new List<Person>();
            public List<Project> Projects = new List<Project>();
            public List<ProjectStatus> ProjectStatuses = new List<ProjectStatus>();
            public List<Receiver> Receivers = new List<Receiver>();
            public List<Region> Regions = new List<Region>();
            public List<Request> Requests = new List<Request>();
            public List<RequestStatus> RequestStatuses = new List<RequestStatus>();
            public List<Shipping> Shippings = new List<Shipping>();
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
        public string ObtainMacAddress()
        {
            return macAddress.ToString();
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
        public bool ProcesSqlQuery(string strSql) => executor.WriteToDataBase(strSql);

            #region Create
            /// <summary>
            /// Method, that creates a new entity in Db
            /// Accepts the following entityTypes: Address, BluePrintContact, 
            /// ContactInfo, CraftGroup, Enterprise, IttLetter, PdfData, 
            /// Project, Request
            /// </summary>
            /// <param name="entity">Object</param>
            /// <returns>int</returns>
            public int CreateInDb(object entity)
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
                            RefreshList("Addresses");
                            count = Addresses.Count;
                            result = Addresses[count - 1].Id;
                            break;
                        case "Authentication":
                            RefreshList("Authentications");
                            count = Authentications.Count;
                            result = Authentications[count - 1].Id;
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
                        case "Paragraph":
                            RefreshList("Paragraphs");
                            count = Paragraphs.Count;
                            result = Paragraphs[count - 1].Id;
                            break;
                        case "Project":
                            RefreshList("Projects");
                            count = Projects.Count;
                            result = Projects[count - 1].Id;
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
                        case "RequestStatus":
                            RefreshList("RequestStatuses");
                            count = RequestStatuses.Count;
                            result = RequestStatuses[count - 1].Id;
                            break;
                        case "Shipping":
                            RefreshList("ShippingList");
                            count = Shippings.Count;
                            result = Shippings[count - 1].Id;
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
                    case "LegalEntity":
                        result = "Legal Person";
                        break;
                    case "LetterData":
                        result = "Udbudbrevs data";
                        break;
                    case "Offer":
                        result = "Tilbud";
                        break;
                    case "Paragraph":
                        result = "Afsnit";
                        break;
                    case "Person":
                        result = "Person";
                        break;
                    case "Project":
                        result = "Projekt";
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
                    case "RequestStatus":
                        result = "Forespørgselsstatus";
                        break;
                    case "Shipping":
                        result = "Forsendelse";
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
            private string GetListFromEntityType(string entityType)
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
                    case "Paragraph":
                        result = "Paragraphs";
                        break;
                    case "Person":
                        result = "Persons";
                        break;
                    case "Project":
                        result = "Projects";
                        break;
                    case "ProjectStatus":
                        result = "ProjectStatusList";
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
                    case "RequestStatus":
                        result = "RequestStatusList";
                        break;
                    case "Shipping":
                        result = "ShippingList";
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
                        result = "ZipTown";
                        break;
                }

                return result;
            }


            /// <summary>
            /// Method, that returns a SQL-query
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
                        result = @"INSERT INTO [dbo].[Addresses](Street, Place, Zip) VALUES('" + address.Street + @"', '" + address.Place + @"', " + address.ZipTown.Id + ")";
                        break;
                    case "Authentications":
                        Authentication authentication = new Authentication((Authentication)entity);
                        result = @"INSERT INTO [dbo].[Authentications](UserLevel, PassWord) VALUES('" + authentication.UserLevel.Id + @"', '" + authentication.PassWord + ")";
                        break;
                    case "Builders":
                        Builder builder = new Builder((Builder)entity);
                        result = @"INSERT INTO [dbo].[Builders]([Entity], [Active]) VALUES(" +  builder.Entity.Id +  @", '" + builder.Active.ToString() + @"')";
                        break;
                    case "Bullets":
                        Bullet bullet = new Bullet((Bullet)entity);
                        result = @"INSERT INTO [dbo].[Bullets]([Paragraph], [Text]) VALUES(" + bullet.Paragraph.Id + @", '" + bullet.Text + @"')";
                        break;
                    case "Categories":
                        Category category = new Category((Category)entity);
                        result = "INSERT INTO [dbo].[Categories]([Text]) VALUES('" + category.Text + "')";
                        break;
                    case "Contacts":
                        Contact contact = new Contact((Contact)entity);
                        result = "INSERT INTO [dbo].[Contacts]([Person], [Entrepeneur], [Area]) VALUES(" + contact.Person.Id + ", " + contact.Entity.Id + ", '" + contact.Area + "')";
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
                        result = "INSERT INTO[dbo].[IttLetters]([Sent], [SentDate]) VALUES('" + ittLetter.Sent + "', " + ittLetter.SentDate.ToShortDateString() + "')";
                        break;
                    case "LetterDataList":
                        LetterData letterData = new LetterData((LetterData)entity);
                        result = "INSERT INTO [dbo].[LetterDataList]([ProjectName], [Builder], [AnswerDate], [QuestionDate], [CorrectionDate], [ConditionDate], [MaterialUrl], [ConditionUrl], [TimeSpan], [Password]) VALUES(" + letterData.ProjectName + ", " + letterData.Builder + ", '" + letterData.AnswerDate + "', '" + letterData.QuestionDate + "', '" + letterData.CorrectionDate + "', '" + letterData.ConditionDate + "', '" + letterData.MaterialUrl + "', '" + letterData.ConditionUrl + "', '" + "', '" + letterData.TimeSpan + letterData.PassWord + "')";
                        break;
                    case "Offers":
                        Offer offer = new Offer((Offer)entity);
                        result = "INSERT INTO [dbo].[Offers]([Received], [ReceivedDate], [Price], [Chosen]) VALUES('" + offer.Received.ToString() + @"', '" + offer.ReceivedDate.ToShortDateString() + @"', " + offer.Price.ToString() + @", '" + offer.Chosen.ToString() + @"')";
                        break;
                    case "Paragraphs":
                        Paragraph paragraph = new Paragraph((Paragraph)entity);
                        result = @"INSERT INTO [dbo].[Paragraphs](Project, Name) VALUES(" + paragraph.Project.Id.ToString() + @", '" + paragraph.Text + @"')";
                        break;
                    case "Persons":
                        Person person = new Person((Person)entity);
                        result = "INSERT INTO [dbo].[Persons]([Name], [ContactInfo]) VALUES('" + person.Name + @"', " + person.Id + @")";
                        break;
                    case "Projects":
                        Project project = new Project((Project)entity);
                        result = @"INSERT INTO [dbo].[Projects](CaseId, Name, Builder, Status, TenderForm, EnterpriseForm, Executive, Enterprises, Copy) VALUES(" + project.Case + @", '" + project.Name + @"', " + project.Builder + @", " + project.Status + @", " + project.TenderForm + @", " + project.EnterpriseForm + @", " + project.Executive + @", '" + project.EnterprisesList.ToString() + "', '" + project.Copy.ToString() + "')";
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
                        result = "INSERT INTO [dbo].[Requests]([Status], [SentDate], [ReceivedDate]) VALUES(" + request.Status.Id + @", '" + request.SentDate.ToShortDateString() + @"', '" + request.ReceivedDate.ToShortDateString() + @"')";
                        break;
                    case "RequestStatuses":
                        RequestStatus requestStatus = new RequestStatus((RequestStatus)entity);
                        result = "INSERT INTO [dbo].[RequestStatuses]([Description]) VALUES('" + requestStatus.Text + @"')";
                        break;
                    case "Shippings":
                        Shipping shipping = new Shipping((Shipping)entity);
                        result = "INSERT INTO [dbo].[ShippingList]([Project], [CommonPdfPath], [PdfPath]) VALUES(" + shipping.Project.Id + ", '" + shipping.CommonPdfPath + "', '" + shipping.PdfPath + "')";
                        break;
                    case "SubEntrepeneurs":
                        SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)entity);
                        result = "INSERT INTO [dbo].[Shippings]([Enterprises], [Entrepeneur], [Contact], [Request], [IttLetter], [Offer], [Reservations], [Uphold], AgreementConcluded], [Active]) VALUES(" + subEntrepeneur.Enterprise.Id + @", '" + subEntrepeneur.Entrepeneur.Id + @"', " + subEntrepeneur.Contact.Id + @", " + subEntrepeneur.Request.Id + @", " + subEntrepeneur.IttLetter.Id + @", " + subEntrepeneur.Offer.Id + @", '" + subEntrepeneur.Reservations.ToString() + @"', '" + subEntrepeneur.Uphold.ToString() + @"', '" + subEntrepeneur.AgreementConcluded.ToString() + @"', '" + subEntrepeneur.Active.ToString() + @"')";
                        break;
                    case "TenderFormList":
                        TenderForm tenderForm = new TenderForm((TenderForm)entity);
                        result = "INSERT INTO [dbo].[TenderFormList]([Description]) VALUES('" + tenderForm + @"')";
                        break;
                    case "UserLevels":
                        UserLevel userLevel = new UserLevel((UserLevel)entity);
                        result = "INSERT INTO [dbo].[UserLevels](Text) VALUES('" + userLevel.Text + @"')";
                        break;
                    case "Users":
                        User user = new User((User)entity);
                        result = "INSERT INTO [dbo].[Users]([Person], [Initials], [JobDescription], [Authentication]) VALUES(" + user.Person.Id + @", '" + user.Initials + @"', " + user.JobDescription.Id + @", " + user.Authentication.Id + @")";
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
                    case "Adresses":
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
                    case "JobDescriptions":
                        result = 4;
                        break;
                    case "LegalEntities":
                        result = 6;
                        break;
                    case "LetterDataList":
                        result = 11;
                        break;
                    case "Offers":
                        result = 5;
                        break;
                    case "Paragraphs":
                        result = 3;
                        break;
                    case "Persons":
                        result = 3;
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
                    case "RequestStatuses":
                        result = 2;
                        break;
                    case "Receivers":
                        result = 8;
                        break;
                    case "Shippings":
                        result = 7;
                        break;
                    case "SubEntrepeneurs":
                        result = 11;
                        break;
                    case "TenderForms":
                        result = 2;
                        break;
                    case "Users":
                        result = 5;
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
                    case "ActiveProjects":
                        Project activeProjects = new Project(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), resultArray[2], new Builder((Builder)GetObject("Builders", Convert.ToInt32(resultArray[3]))), new ProjectStatus((ProjectStatus)GetObject("ProjectStatuses", Convert.ToInt32(resultArray[4]))), new TenderForm((TenderForm)GetObject("TenderForms", Convert.ToInt32(resultArray[5]))), new EnterpriseForm((EnterpriseForm)GetObject("EnterpriseForms", Convert.ToInt32(resultArray[6]))), new User((User)GetObject("Users", Convert.ToInt32(resultArray[7]))), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
                        result = activeProjects;
                        break;
                    case "Addresses":
                        Address address = new Address(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], new ZipTown((ZipTown)GetObject("ZipTowns", Convert.ToInt32(resultArray[3]))));
                        result = address;
                        break;
                    case "Authentications":
                        Authentication authentication = new Authentication(Convert.ToInt32(resultArray[0]), new UserLevel((UserLevel)GetObject("UserLevels", Convert.ToInt32(resultArray[1]))), resultArray[2]);
                        result = authentication;
                        break;
                    case "Builders":
                        Builder builder = new Builder(Convert.ToInt32(resultArray[0]), new LegalEntity((LegalEntity)GetObject("LegalEntities", Convert.ToInt32(resultArray[1]))), Convert.ToBoolean(resultArray[2]));
                        result = builder;
                        break;
                    case "Bullets":
                        Bullet bullet = new Bullet(Convert.ToInt32(resultArray[0]), new Paragraph((Paragraph)GetObject("Paragraphs", Convert.ToInt32(resultArray[1]))), resultArray[2]);
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
                        Contact contact = new Contact(Convert.ToInt32(resultArray[0]), new Person((Person)GetObject("Persons", Convert.ToInt32(resultArray[1]))), new Entrepeneur((Entrepeneur)GetObject("Entrepeneurs", Convert.ToInt32(resultArray[2]))), resultArray[3]);
                        result = contact;
                        break;
                    case "CraftGroups":
                        CraftGroup craftGroup = new CraftGroup(Convert.ToInt32(resultArray[0]), new Category((Category)GetObject("Categories", Convert.ToInt32(resultArray[1]))), resultArray[2], resultArray[3], Convert.ToBoolean(resultArray[4]));
                        result = craftGroup;
                        break;
                    case "EnterpriseForms":
                        EnterpriseForm enterpriseForm = new EnterpriseForm(Convert.ToInt32(resultArray[0]), resultArray[1]);
                        result = enterpriseForm;
                        break;
                    case "Enterprises":
                        Enterprise enterprise = new Enterprise(Convert.ToInt32(resultArray[0]), new Project((Project)GetObject("Projects", Convert.ToInt32(resultArray[1]))), resultArray[2], resultArray[3], resultArray[4], new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[5]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[6]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[7]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[8]))));
                        result = enterprise;
                        break;
                    case "Entrepeneurs":
                        Entrepeneur entrepeneur = new Entrepeneur(Convert.ToInt32(resultArray[0]), new LegalEntity((LegalEntity)GetObject("LegalEntities", Convert.ToInt32(resultArray[1]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[2]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[3]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[4]))), new CraftGroup((CraftGroup)GetObject("CraftGroups", Convert.ToInt32(resultArray[5]))), new Region((Region)GetObject("Regions", Convert.ToInt32(resultArray[6]))), Convert.ToBoolean(resultArray[7]), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
                        result = entrepeneur;
                        break;
                    case "InactiveProjects":
                        Project inactiveproject = new Project(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), resultArray[2], new Builder((Builder)GetObject("Builders", Convert.ToInt32(resultArray[3]))), new ProjectStatus((ProjectStatus)GetObject("ProjectStatuses", Convert.ToInt32(resultArray[4]))), new TenderForm((TenderForm)GetObject("TenderForms", Convert.ToInt32(resultArray[5]))), new EnterpriseForm((EnterpriseForm)GetObject("EnterpriseForms", Convert.ToInt32(resultArray[6]))), new User((User)GetObject("Users", Convert.ToInt32(resultArray[7]))), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
                        result = inactiveproject;
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
                        LegalEntity legalEntity = new LegalEntity(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], new Address((Address)GetObject("Addresses", Convert.ToInt32(resultArray[3]))), new ContactInfo((ContactInfo)GetObject("ContactInfoList", Convert.ToInt32(resultArray[4]))), resultArray[5]);
                        result = legalEntity;
                        break;
                    case "LetterDataList":
                        LetterData pdfData = new LetterData(Convert.ToInt32(resultArray[0]), resultArray[1], resultArray[2], resultArray[3], resultArray[4], resultArray[5], Convert.ToInt32(resultArray[6]), resultArray[7], resultArray[8], resultArray[9]);
                        result = pdfData;
                        break;
                    case "Offers":
                        Offer offer = new Offer(Convert.ToInt32(resultArray[0]), Convert.ToBoolean(resultArray[1]), Convert.ToDateTime(resultArray[2]), Convert.ToDouble(resultArray[3]), Convert.ToBoolean(resultArray[4]));
                        result = offer;
                        break;
                    case "Paragraphs":
                        Paragraph paragraph = new Paragraph(Convert.ToInt32(resultArray[0]), new Project((Project)GetObject("Projects", Convert.ToInt32(resultArray[1]))), resultArray[2]);
                        result = paragraph;
                        break;
                    case "Projects":
                        Project project = new Project(Convert.ToInt32(resultArray[0]), Convert.ToInt32(resultArray[1]), resultArray[2], new Builder((Builder)GetObject("Builders", Convert.ToInt32(resultArray[3]))), new ProjectStatus((ProjectStatus)GetObject("ProjectStatuses", Convert.ToInt32(resultArray[4]))), new TenderForm((TenderForm)GetObject("TenderForms", Convert.ToInt32(resultArray[5]))), new EnterpriseForm((EnterpriseForm)GetObject("EnterpriseForms", Convert.ToInt32(resultArray[6]))), new User((User)GetObject("Users", Convert.ToInt32(resultArray[7]))), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]));
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
                        Request request = new Request(Convert.ToInt32(resultArray[0]), new RequestStatus((RequestStatus)GetObject("RequestStatuses", Convert.ToInt32(resultArray[1]))), Convert.ToDateTime(resultArray[2]), Convert.ToDateTime(resultArray[3]));
                        result = request;
                        break;
                    case "RequestStatuses":
                        RequestStatus requestStatus = new RequestStatus(Convert.ToInt32(resultArray[0]), resultArray[1]);
                        result = requestStatus;
                        break;
                    case "Shippings":
                        Shipping shipping = new Shipping(Convert.ToInt32(resultArray[0]), new Project((Project)GetObject("Projects", Convert.ToInt32(resultArray[1]))), new Receiver((Receiver)GetObject("Receivers", Convert.ToInt32(resultArray[2]))), new SubEntrepeneur((SubEntrepeneur)GetObject("SubEntrepeneurs", Convert.ToInt32(resultArray[3]))), new LetterData((LetterData)GetObject("LetterDataList", Convert.ToInt32(resultArray[4]))), resultArray[5], resultArray[6]);
                        result = shipping;
                        break;
                    case "SubEntrepeneurs":
                        SubEntrepeneur subEntrepeneur = new SubEntrepeneur(Convert.ToInt32(resultArray[0]), new Entrepeneur((Entrepeneur)GetObject("Entrepeneurs", Convert.ToInt32(resultArray[2]))), new Enterprise((Enterprise)GetObject("Enterprises", Convert.ToInt32(resultArray[1]))), new Contact((Contact)GetObject("Contacts", Convert.ToInt32(resultArray[3]))), new Request((Request)GetObject("Requests", Convert.ToInt32(resultArray[4]))), new IttLetter((IttLetter)GetObject("IttLetters", Convert.ToInt32(resultArray[5]))), new Offer((Offer)GetObject("Offers", Convert.ToInt32(resultArray[6]))), Convert.ToBoolean(resultArray[7]), Convert.ToBoolean(resultArray[8]), Convert.ToBoolean(resultArray[9]), Convert.ToBoolean(resultArray[10]));
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
                        User user = new User(Convert.ToInt32(resultArray[0]), new Person((Person)GetObject("Persons", Convert.ToInt32(resultArray[1]))), resultArray[2], new JobDescription((JobDescription)GetObject("JobDescriptions", Convert.ToInt32(resultArray[3]))), new Authentication((Authentication)GetObject("Authentications", Convert.ToInt32(resultArray[4]))));
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

            /// <summary>
            /// Method, that reads a Project List from Db
            /// Accepts the following lists: Enterprises [projectId], Shippings [projectId] & SubEntrepeneurs [enterpriseId]
            /// </summary>
            /// <param name="list">string</param>
            /// <param name="id">int</param>
            /// <returns>List<object></returns>
            public List<object> ReadProjectListFromDb(string list, int id)
            {
                List<object> result = new List<object>();


                List<string> strResults;

                strResults = executor.ReadProjectListFromDataBase(list, id);

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
                        case "Address":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Addresses", new Address((Address)_object)));
                            break;
                        case "Authentication":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Authentications", new Authentication((Authentication)_object)));
                            break;
                        case "Builder":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Builders", new Builder((Builder)_object)));
                            break;
                        case "Bullet":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Bullets", new Bullet((Bullet)_object)));
                            break;
                        case "Category":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Categories", new Category((Category)_object)));
                            break;
                        case "Contact":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Contacts", new Contact((Contact)_object)));
                            break;
                        case "ContactInfo":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("ContactInfoList", new ContactInfo((ContactInfo)_object)));
                            break;
                        case "CraftGroup":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("CraftGroups", new CraftGroup((CraftGroup)_object)));
                            break;
                        case "Enterprise":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Enterprises", new Enterprise((Enterprise)_object)));
                            break;
                        case "EnterpriseForm":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("EnterpriseForms", new EnterpriseForm((EnterpriseForm)_object)));
                            break;
                        case "IttLetter":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("IttLetters", new Enterprise((Enterprise)_object)));
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
                        case "Paragraph":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Paragraphs", new Paragraph((Paragraph)_object)));
                            break;
                        case "Person":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Persons", new Person((Person)_object)));
                            break;
                        case "Shipping":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Shippings", new Shipping((Shipping)_object)));
                            break;
                        case "Project":
                            result = ProcesSqlQuery(GetSQLQueryUpdate("Projects", new Project((Project)_object)));
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
                    case "Addresses":
                        Address address = new Address((Address)_object);
                        result = @"UPDATE [dbo].[Addresses] SET [Street] = '" + address.Street + "', [Place] = '" + address.Place + "', [Zip] = " + address.ZipTown.Id + " WHERE [Id] = '" + address.Id + "';";
                        break;
                    case "Authentications":
                        Authentication authentication = new Authentication((Authentication)_object);
                        result = @"UPDATE [dbo].[Authentications] SET [UserLevel] = " + authentication.UserLevel.Id + ", [PassWord] = '" + authentication.PassWord + "' WHERE [Id] = '" + authentication.Id + "';";
                        break;
                    case "Builders":
                        Builder builder = new Builder((Builder)_object);
                        result = @"UPDATE[dbo].[Builders] SET [Entity] = " + builder.Entity.Id + @" [Active] = '" + builder.Active.ToString() + @"' WHERE [Id] = " + builder.Id;
                        break;
                    case "Bullets":
                        Bullet bullet = new Bullet((Bullet)_object);
                        result = @"UPDATE[dbo].[Bullets] SET [Paragraph] = " + bullet.Paragraph.Id + @", [Text] = '" + bullet.Text + @"' WHERE [Id] = " + bullet.Id;
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
                        result = @"UPDATE [dbo].[Contacts] SET [Person] = " + contact.Person.Id + ", [Entrepeneur] = " + contact.Entity.Id + ", [Area] = '" + contact.Area + "' WHERE [Id] = " + contact.Id;
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
                    case "JobDescriptions":
                        JobDescription jobDescription = new JobDescription((JobDescription)_object);
                        result = @"UPDATE [dbo].[Regions] SET [Occupation] = " + jobDescription.Occupation + ", [Area] = '" + jobDescription.Area + "', [Procuration] = '" + jobDescription.Procuration.ToString() + "' WHERE [Id] = " + jobDescription.Id.ToString();
                        break;
                    case "LegalEntities":
                        LegalEntity legalEntity = new LegalEntity((LegalEntity)_object);
                        result = @"UPDATE [dbo].[LegalEntities] SET [Cvr] = '" + legalEntity.Cvr + @"', [Name] = '" + legalEntity.Name + @"', [Address] = " + legalEntity.Address.Id + @", [ContactInfo] = " + legalEntity.ContactInfo.Id + @", [Url] = '" + legalEntity.Url + @"' WHERE [Id] = " + legalEntity.Id;
                        break;
                    case "LetterDataList":
                        LetterData letterData = new LetterData((LetterData)_object);
                        result = @"UPDATE [dbo].[LetterDataList] SET [ProjectName] = '" + letterData.ProjectName + @"', [Builder] = '" + letterData.Builder + @" [AnswerDate] = '" + letterData.AnswerDate + @"', [QuestionDate] = '" + letterData.QuestionDate + @"', [CorrectionDate] = '" + letterData.CorrectionDate + @"', [TimeSpan] = '" + letterData.TimeSpan + @"', [MaterialUrl] = '" + letterData.MaterialUrl + @"', [ConditionUrl] = '" + letterData.ConditionUrl + @"', [PassWord] = '" + letterData.PassWord + @"' WHERE [Id] = " + letterData.Id;
                        break;
                    case "Offers":
                        Offer offer = new Offer((Offer)_object);
                        result = @"UPDATE [dbo].[Offers] SET [Received] = " + offer.Received.ToString() + @", [ReceivedDate] = " + offer.ReceivedDate.ToShortDateString() + @", [Price] = " + offer.Price + @", [Chosen] = '" + offer.Chosen.ToString() + @"' WHERE [Id] = " + offer.Id;
                        break;
                    case "Paragraphs":
                        Paragraph paragraph = new Paragraph((Paragraph)_object);
                        result = @"UPDATE [dbo].[Paragraphs] SET [Project] = " + paragraph.Project.Id + @", [Name] = '" + paragraph.Text + @"' WHERE[Id] = " + paragraph.Id;
                        break;
                    case "Persons":
                        Person person = new Person((Person)_object);
                        result = @"UPDATE [dbo].[Persons] SET [Name] = '" + person.Name + @"', [ContactInfo] = " + person.ContactInfo.Id + @" WHERE[Id] = " + person.Id;
                        break;
                    case "Projects":
                        Project project = new Project((Project)_object);
                        result = @"UPDATE dbo.[Projects] SET [CaseId] = " + project.Case + ", [Name] = '" + project.Name + "', [Builder] = " + project.Builder.Id + ", [Status] = " + project.Status.Id + ", [TenderForm] = " + project.TenderForm.Id + ", [EnterpriseForm] = " + project.EnterpriseForm.Id + ", [Executive] = " + project.Executive.Id + ", [Enterprises] = '" + project.EnterprisesList.ToString() + "', [Copy] = '" + project.Copy.ToString() + "' WHERE [Id] = " + project.Id;
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
                        result = @"UPDATE [dbo].[Requests] SET [Status] = " + request.Status.Id + ", [SentDate] = '" + request.SentDate.ToShortDateString() + "', [ReceivedDate] = '" + request.ReceivedDate.ToShortDateString() + "' WHERE [Id] = " + request.Id.ToString();
                        break;
                    case "RequestStatusList":
                        RequestStatus requestStatus = new RequestStatus((RequestStatus)_object);
                        result = @"UPDATE [dbo].[RequestStatusList] SET [Description] = '" + "' WHERE [Id] = " + requestStatus.Id.ToString();
                        break;
                    case "Shippings":
                        Shipping shipping = new Shipping((Shipping)_object);
                        result = @"UPDATE [dbo].[ShippingList] SET [Project] = " + shipping.Project.Id + @", [CommonPdfPath] = '" + shipping.CommonPdfPath + @"', [PdfPath] = '" + shipping.PdfPath + @"' WHERE [Id] = " + shipping.Id;
                        break;
                    case "SubEntrepeneurs":
                        SubEntrepeneur subEntrepeneur = new SubEntrepeneur((SubEntrepeneur)_object);
                        result = @"UPDATE [dbo].[SubEntrepeneurs] SET [Enterprise] = " + subEntrepeneur.Enterprise.Id + ", [Entrepeneur] = '" + subEntrepeneur.Entrepeneur + "', [Contact] = " + subEntrepeneur.Contact.Id + ", [Request] = " + subEntrepeneur.Request.Id + "', [IttLetter] = " + subEntrepeneur.IttLetter.Id + ", [Offer] = " + subEntrepeneur.Offer.Id + ", [Reservations] = '" + subEntrepeneur.Reservations.ToString() + ", [Uphold] = '" + subEntrepeneur.Uphold.ToString() + ", [AgreementConcluded] = '" + subEntrepeneur.AgreementConcluded.ToString() + ", [Active] = '" + subEntrepeneur.Active.ToString() + "' WHERE [Id] = " + subEntrepeneur.Id;
                        break;
                    case "TenderForms":
                        TenderForm tenderForm = new TenderForm((TenderForm)_object);
                        result = @"UPDATE [dbo].[TenderForms] SET [description] = '" + tenderForm.Text + "' WHERE [Id] = " + tenderForm.Id;
                        break;
                    case "Users":
                        User user = new User((User)_object);
                        result = @"UPDATE [dbo].[Users] SET [Person] = " + user.Person.Id + ", [Initials] = '" + user.Initials + "', [JobDescription] = " + user.JobDescription.Id + ", [Authentication] = " + user.Authentication.Id + " WHERE [Id] = " + user.Id;
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

        #region Get Lists for a project
        /// <summary>
        /// Method, that refreshes Lists for Itt Letter
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<object></returns>
        public void RefreshIttLetterLists(int projectId)
        {
            PdfLists = new PdfLists(GetEnterprises(projectId), GetSubEntrepeneurs(projectId), GetShippings(projectId));
        }

        /// <summary>
        /// Method, that retrieves an Enterprise List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<Enterprise></returns>
        private List<Enterprise> GetEnterprises(int projectId)
        {
            List<Enterprise> result = new List<Enterprise>();
            List<object> objectList = ReadProjectListFromDb("Enterprises", projectId);

            foreach (object enterprise in objectList)
            {
                result.Add(new Enterprise((Enterprise)enterprise));
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an Shipping List for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<Shipping></returns>
        private List<Shipping> GetShippings(int projectId)
        {
            List<Shipping> result = new List<Shipping>();
            List<object> objectList = ReadProjectListFromDb("Shippings", projectId);

            foreach (object shipping in objectList)
            {
                result.Add(new Shipping((Shipping)shipping));
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves a SubEntrepeneur list for a Project
        /// </summary>
        /// <param name="projectId">int</param>
        /// <returns>List<SubEntrepeneur></returns>
        private List<SubEntrepeneur> GetSubEntrepeneurs(int projectId)
        {
            List<SubEntrepeneur> result = new List<SubEntrepeneur>();
            List<Enterprise> projectEnterprises = GetEnterprises(projectId);

            foreach (Enterprise enterprise in projectEnterprises)
            {
                List<object> objectList = ReadProjectListFromDb("Subentrepeneurs", enterprise.Id);

                foreach (object shipping in objectList)
                {
                    result.Add(new SubEntrepeneur((SubEntrepeneur)shipping));
                }
            }

            return result;
            
        }

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
            //Base Level Lists
            RefreshCategories(); //
            RefreshContactInfoList(); //
            RefreshEnterpriseForms(); //
            RefreshIttLetters(); //
            RefreshJobDescriptions(); //
            RefreshLetterDataList(); //
            RefreshOffers(); //
            RefreshProjectStatuses(); //
            RefreshReceivers(); //
            RefreshRegions(); //
            RefreshRequestStatuses(); //
            RefreshTenderforms(); //
            RefreshUserLevels(); //
            RefreshZipTowns(); //

            //Second level Lists
            RefreshAddresses(); //
            RefreshAuthentications(); //
            RefreshCraftGroups(); //
            RefreshPersons(); //
            RefreshRequests(); //

            //Third level list
            RefreshLegalEntities(); //
            RefreshUsers(); //

            //Fourth Level List
            RefreshBuilders(); //
            RefreshEntrepeneurs(); //

            //Fifth level Lists
            RefreshContacts(); //
            RefreshProjects(); //
            RefreshActiveProjects(); //
            RefreshInactiveProjects(); //

            //Sixth level Lists
            RefreshEnterprises();
            RefreshParagraphs();

            //Seventh level Lists
            RefreshBullets();
            RefreshSubEntrepeneurs();

            //Eightieth level Lists
            RefreshShippings();


        }

        /// <summary>
        /// Method, that refreshes a list
        /// </summary>
        /// <param name="list">string</param>
        public void RefreshList(string list)
        {
            switch (list)
            {
                case "ActiveProjects":
                    RefreshActiveProjects();
                    break;
                case "Addresses":
                    RefreshAddresses();
                    break;
                case "Authentications":
                    RefreshAuthentications();
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
                case "InactiveProjects":
                    RefreshInactiveProjects();
                    break;
                case "IttLetters":
                    RefreshIttLetters();
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
                case "Paragraphs":
                    RefreshParagraphs();
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
                case "RequestStatuses":
                    RefreshRequestStatuses();
                    break;
                case "Shippings":
                    RefreshShippings();
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
        /// Method, that refreshes the Projects list
        /// </summary>
        private void RefreshActiveProjects()
        {
            if (Projects != null)
            {
                ActiveProjects.Clear();
            }
            List<object> tempList = ReadListFromDb("ActiveProjects");

            foreach (object _object in tempList)
            {
                ActiveProjects.Add((Project)_object);
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
        /// Method, that refreshes the Authentications list
        /// </summary>
        private void RefreshAuthentications()
        {
            if (Authentications != null)
            {
                Authentications.Clear();
            }
            List<object> tempList = ReadListFromDb("Authentications");

            foreach (object obj in tempList)
            {
                Authentications.Add((Authentication)obj);
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
        /// Method, that refreshes the Bullets list
        /// </summary>
        private void RefreshBullets()
        {
            if (Bullets != null)
            {
                Bullets.Clear();
            }
            List<object> tempList = ReadListFromDb("BluePrints");

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
        /// Method, that refreshes the Entrepeneurs List
        /// </summary>
        private void RefreshEntrepeneurs()
        {
            if (Entrepeneurs != null)
            {
                Entrepeneurs.Clear();
            }
            List<object> tempList = ReadListFromDb("Entrepeneurs");

            foreach (object obj in tempList)
            {
                Entrepeneurs.Add((Entrepeneur)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Projects list
        /// </summary>
        private void RefreshInactiveProjects()
        {
            if (Projects != null)
            {
                InactiveProjects.Clear();
            }
            List<object> tempList = ReadListFromDb("InactiveProjects");

            foreach (object _object in tempList)
            {
                InactiveProjects.Add((Project)_object);
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
        /// Method, that refreshes the Paragraph List
        /// </summary>
        private void RefreshParagraphs()
        {
            if (Paragraphs != null)
            {
                Paragraphs.Clear();
            }
            List<object> tempList = ReadListFromDb("Paragraphs");

            foreach (object obj in tempList)
            {
                Paragraphs.Add((Paragraph)obj);
            }
        }

        /// <summary>
        /// Method, that refreshes the Persons list
        /// </summary>
        private void RefreshPersons()
        {
            if (Persons != null)
            {
                Persons.Clear();
            }
            List<object> tempList = ReadListFromDb("Persons");

            foreach (object obj in tempList)
            {
                Persons.Add((Person)obj);
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
        /// Method, that refreshes the Shipping List
        /// </summary>
        private void RefreshShippings()
        {
            if (Shippings != null)
            {
                Shippings.Clear();
            }
            List<object> tempList = ReadListFromDb("Shippings");

            foreach (object obj in tempList)
            {
                Shippings.Add((Shipping)obj);
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
        /// Method, that refreshes the Users lists
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
