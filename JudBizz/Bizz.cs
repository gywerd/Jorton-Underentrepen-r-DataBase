using CvrApiServices;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using UkZipApi;

namespace JudBizz
{
    public class Bizz : MyEntityFrameWork
    {
        #region Fields

        public CvrAPI CvrApi;
        public UkZipApi.UkZipApi UkZipApi;
        public bool UcMainEdited = false;
        private bool danishZip = false;
        private bool faroeseZip = false;
        private bool foreignZip = false;
        private bool kaalaalitZip = false;
        private bool validZip = false;
        private int intZip = 0;
        private string tempZip = "";
        public ImageBrush button = new ImageBrush();
        public ImageBrush smallButton = new ImageBrush();
        public ImageBrush broadButton = new ImageBrush();
        public ImageBrush greenIndicator = new ImageBrush();
        public ImageBrush redIndicator = new ImageBrush();


        public List<IndexedBuilder> IndexedActiveBuilders = new List<IndexedBuilder>();
        public List<IndexedEntrepeneur> IndexedActiveEntrepeneurs = new List<IndexedEntrepeneur>();
        public List<IndexedProject> IndexedActiveProjects = new List<IndexedProject>();
        public List<IndexedUser> IndexedActiveUsers = new List<IndexedUser>();
        public List<IndexedBullet> IndexedBullets = new List<IndexedBullet>();
        public List<IndexedBuilder> IndexedBuilders = new List<IndexedBuilder>();
        public List<IndexedCategory> IndexedCategories = new List<IndexedCategory>();
        public List<IndexedCraftGroup> IndexedCraftGroups = new List<IndexedCraftGroup>();
        public List<IndexedContact> IndexedContacts = new List<IndexedContact>();
        public List<IndexedEnterpriseForm> IndexedEnterpriseForms = new List<IndexedEnterpriseForm>();
        public List<IndexedEnterprise> IndexedEnterprises = new List<IndexedEnterprise>();
        public List<IndexedEntrepeneur> IndexedEntrepeneurs = new List<IndexedEntrepeneur>();
        public List<IndexedBuilder> IndexedInactiveBuilders = new List<IndexedBuilder>();
        public List<IndexedEntrepeneur> IndexedInactiveEntrepeneurs = new List<IndexedEntrepeneur>();
        public List<IndexedProject> IndexedInactiveProjects = new List<IndexedProject>();
        public List<IndexedUser> IndexedInactiveUsers = new List<IndexedUser>();
        public List<IndexedJobDescription> IndexedJobDescriptions = new List<IndexedJobDescription>();
        public List<IndexedParagraf> IndexedParagrafs = new List<IndexedParagraf>();
        public List<IndexedProject> IndexedProjects = new List<IndexedProject>();
        public List<IndexedProjectStatus> IndexedProjectStatuses = new List<IndexedProjectStatus>();
        public List<IndexedRegion> IndexedRegions = new List<IndexedRegion>();
        public List<IndexedRequest> IndexedRequests = new List<IndexedRequest>();
        public List<IndexedRequestStatus> IndexedRequestStatuses = new List<IndexedRequestStatus>();
        public List<IndexedSubEntrepeneur> IndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedTenderForm> IndexedTenderForms = new List<IndexedTenderForm>();
        public List<IndexedUser> IndexedUsers = new List<IndexedUser>();
        public List<IndexedUserLevel> IndexedUserLevels = new List<IndexedUserLevel>();
        public List<IndexedZipTown> IndexedZipTowns = new List<IndexedZipTown>();

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Bizz()
        {
            RefreshAllInitialIndexedLists();
            CvrApi = new CvrAPI(ZipTowns);
            UkZipApi = new UkZipApi.UkZipApi();

            SetIndicators();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Method, that checks, wether a string can be converted to integer
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public bool CanConvertStringToInt(string zip)
        {
            intZip = 0;
            return Int32.TryParse(zip, out intZip);
        }

        /// <summary>
        /// Method, that checks credentials
        /// </summary>
        /// <param name="userName">TextBlock</param>
        /// <param name="menuItemChangePassWord">RibbonApplicationMenuItem</param>
        /// <param name="menuItemLogOut">RibbonApplicationMenuItem</param>
        /// <param name="initials">string</param>
        /// <param name="passWord">string</param>
        /// <returns>bool</returns>
        public bool CheckCredentials(Label userName, RibbonApplicationMenuItem menuItemChangePassWord, RibbonApplicationMenuItem menuItemLogOut, string initials, string passWord)
        {
            bool result = false;
            RefreshList("Users");
            if (CheckLogin(initials, passWord))
            {
                foreach (User user in Users)
                {
                    if (user.Initials == initials && user.UserLevel.Id >= 1)
                    {
                        CurrentUser = user;
                        userName.Content = user.Person.Name;
                        menuItemChangePassWord.IsEnabled = true;
                        menuItemLogOut.IsEnabled = true;
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that clears the UcMain UserControl in MainWindow
        /// </summary>
        /// <param name="ucMain"></param>
        public void CloseUcMain(UserControl ucMain)
        {
            UcMainEdited = false;
            ucMain.Content = new UserControl();
        }

        #region Refresh Indexed Lists
        /// <summary>
        /// Method, that refreshes all Indexed lists
        /// </summary>
        public void RefreshAllInitialIndexedLists()
        {
            RefreshIndexedProjects();
            RefreshIndexedCategories();
            RefreshIndexedCraftGroups();
            RefreshIndexedProjects();
            RefreshIndexedEnterpriseForms();
            RefreshIndexedProjectStatuses();
            RefreshIndexedRegions();
            RefreshIndexedRequestStatuses();
            RefreshIndexedTenderForms();
            RefreshIndexedZipTowns();
        }

        /// <summary>
        /// Method, that refreshes Indexed  Builders list
        /// </summary>
        private void RefreshIndexedBuilders()
        {
            RefreshList("Builders");
            IndexedBuilders.Clear();

            int i = 0;
            int j = 0;
            int k = 0;

            foreach (Builder builder in Builders)
            {
                IndexedBuilders.Add(new IndexedBuilder(i, builder));
                i++;

                if (builder.Active)
                {
                    IndexedActiveBuilders.Add(new IndexedBuilder(j, builder));
                    j++;
                }
                else
                {
                    IndexedInactiveBuilders.Add(new IndexedBuilder(k, builder));
                    k++;
                }
            }

        }

        /// <summary>
        /// Method that refreshes a list of Indexed Categories
        /// </summary>
        private void RefreshIndexedCategories()
        {
            IndexedCategories.Clear();
            RefreshList("Categories");

            int i = 0;

            foreach (Category contact in Categories)
            {
                IndexedCategories.Add(new IndexedCategory(i, contact));
                i++;
            }
        }

        /// <summary>
        /// <summary>
        /// Method that refreshes a list of Indexed Contacts
        /// </summary>
        private void RefreshIndexedContacts()
        {
            RefreshList("Contacts");
            RefreshIndexedEntrepeneurs();
            IndexedContacts.Clear();

            int i = 0;

            foreach (Contact contact in Contacts)
            {
                IndexedContacts.Add(new IndexedContact(i, contact));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed CraftGroups
        /// </summary>
        private void RefreshIndexedCraftGroups()
        {
            RefreshList("CraftGroups");
            RefreshIndexedCategories();
            IndexedCraftGroups.Clear();

            int i = 0;

            foreach (CraftGroup craftGroup in CraftGroups)
            {
                IndexedCraftGroups.Add(new IndexedCraftGroup(i, craftGroup));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Enterprise Forms list
        /// </summary>
        private void RefreshIndexedEnterpriseForms()
        {
            RefreshList("EnterpriseForms");
            IndexedEnterpriseForms.Clear();

            int i = 0;

            foreach (EnterpriseForm form in EnterpriseForms)
            {
                IndexedEnterpriseForms.Add(new IndexedEnterpriseForm(i, form));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Enterprises list
        /// </summary>
        private void RefreshIndexedEnterprises()
        {
            RefreshList("Enterprises");
            RefreshIndexedCraftGroups();
            RefreshIndexedProjects();
            IndexedEnterprises.Clear();

            int i = 0;

            foreach (Enterprise enterprise in Enterprises)
            {
                IndexedEnterprises.Add(new IndexedEnterprise(i, enterprise));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed Entrepeneurs
        /// </summary>
        /// <returns>List<IndexedEntrepeneur></returns>
        private void RefreshIndexedEntrepeneurs()
        {
            RefreshList("Entrepeneurs");
            RefreshIndexedCraftGroups();

            int i = 0;
            int j = 0;
            int k = 0;

            IndexedEntrepeneurs.Clear();

            foreach (Entrepeneur entrepeneur in Entrepeneurs)
            {
                IndexedEntrepeneurs.Add(new IndexedEntrepeneur(i, entrepeneur));
                i++;

                if (entrepeneur.Active)
                {
                    IndexedActiveEntrepeneurs.Add(new IndexedEntrepeneur(j, entrepeneur));
                    j++;
                }
                else
                {
                    IndexedInactiveEntrepeneurs.Add(new IndexedEntrepeneur(k, entrepeneur));
                    k++;
                }
            }

        }

        /// <summary>
        /// Method that refreshes a list of Indexed Entrepeneurs
        /// </summary>
        /// <returns>List<IndexedEntrepeneur></returns>
        private void RefreshIndexedSubEntrepeneursFromProjectSubEntrepeneurs()
        {
            RefreshProjectList("SubEntrepeneurs", TempProject.Id);
            IndexedSubEntrepeneurs.Clear();

            int i = 0;

            //Fill Indexed Active Entrepeneur list
            foreach (SubEntrepeneur subEntrepeneur in ProjectLists.SubEntrepeneurs)
            {
                IndexedSubEntrepeneurs.Add(new IndexedSubEntrepeneur(i, subEntrepeneur));
                i++;
            }

        }

        /// <summary>
        /// Method, that refreshes an Indexed list
        /// </summary>
        public void RefreshIndexedList(string list)
        {
            switch (list)
            {
                case "SubEntrepeneursFromProjectSubEntrepeneurs":
                    RefreshIndexedSubEntrepeneursFromProjectSubEntrepeneurs();
                    break;
                case "Builders":
                    RefreshIndexedBuilders();
                    break;
                case "Categories":
                    RefreshIndexedCategories();
                    break;
                case "Contacts":
                    RefreshIndexedContacts();
                    break;
                case "CraftGroups":
                    RefreshIndexedCraftGroups();
                    break;
                case "EnterpriseForms":
                    RefreshIndexedEnterpriseForms();
                    break;
                case "Entrepeneurs":
                    RefreshIndexedEntrepeneurs();
                    break;
                case "JobDescriptions":
                    RefreshIndexedJobDescriptions();
                    break;
                case "Projects":
                    RefreshIndexedProjects();
                    break;
                case "ProjectStatuses":
                    RefreshIndexedProjectStatuses();
                    break;
                case "Regions":
                    RefreshIndexedRegions();
                    break;
                case "RequestStatuses":
                    RefreshIndexedRequestStatuses();
                    break;
                case "TenderForms":
                    RefreshIndexedTenderForms();
                    break;
                case "Users":
                    RefreshIndexedUsers();
                    break;
                case "UserLevels":
                    RefreshIndexedUserLevels();
                    break;
                case "ZipTowns":
                    RefreshIndexedZipTowns();
                    break;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed Job Descriptions
        /// </summary>
        private void RefreshIndexedJobDescriptions()
        {
            RefreshList("JobDescriptions");
            IndexedJobDescriptions.Clear();

            int i = 0;

            foreach (JobDescription description in JobDescriptions)
            {
                IndexedJobDescriptions.Add(new IndexedJobDescription(i, description));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Projects list
        /// </summary>
        private void RefreshIndexedProjects()
        {
            RefreshList("Projects");
            RefreshIndexedProjectStatuses();
            RefreshIndexedEnterpriseForms();
            RefreshIndexedUsers();
            RefreshIndexedBuilders();
            IndexedProjects.Clear();

            int i = 0;
            int j = 0;
            int k = 0;

            foreach (Project project in Projects)
            {
                IndexedProjects.Add(new IndexedProject(i, project));
                i++;

                if (project.Status.Id == 1)
                {
                    IndexedActiveProjects.Add(new IndexedProject(j, project));
                    j++;
                }
                else
                {
                    IndexedInactiveProjects.Add(new IndexedProject(k, project));
                    k++;
                }
            }

        }

        /// <summary>
        /// Method, that refreshes Indexed Project Status list
        /// </summary>
        private void RefreshIndexedProjectStatuses()
        {
            RefreshList("ProjectStatuses");
            IndexedProjectStatuses.Clear();

            int i = 0;

            foreach (ProjectStatus status in ProjectStatuses)
            {
                IndexedProjectStatuses.Add(new IndexedProjectStatus(i, status));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed Regions list
        /// </summary>
        private void RefreshIndexedRegions()
        {
            RefreshList("Regions");
            IndexedRegions.Clear();

            int i = 0;

            foreach (Region region in Regions)
            {
                IndexedRegions.Add(new IndexedRegion(i, region));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Request Statuses list
        /// </summary>
        private void RefreshIndexedRequestStatuses()
        {
            RefreshList("RequestStatuses");
            IndexedRequestStatuses.Clear();

            int i = 0;

            foreach (RequestStatus status in RequestStatuses)
            {
                IndexedRequestStatuses.Add(new IndexedRequestStatus(i, status));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed SubEntrepeneurs list
        /// </summary>
        private void RefreshIndexedSubEntrepeneurs()
        {
            RefreshList("SubEntrepeneurs");
            RefreshIndexedEntrepeneurs();
            RefreshIndexedEnterprises();
            RefreshIndexedContacts();

            int i = 0;

            foreach (Project project in Projects)
            {
                IndexedProjects.Add(new IndexedProject(i, project));
                i++;
            }

        }


        /// <summary>
        /// Method, that refreshes Indexed Tender Forms list
        /// </summary>
        private void RefreshIndexedTenderForms()
        {
            RefreshList("TenderForms");
            IndexedTenderForms.Clear();

            int i = 0;

            foreach (TenderForm status in TenderForms)
            {
                IndexedTenderForms.Add(new IndexedTenderForm(i, status));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed Users
        /// </summary>
        private void RefreshIndexedUsers()
        {
            RefreshList("Users");
            RefreshIndexedJobDescriptions();
            RefreshIndexedUserLevels();
            IndexedUsers.Clear();

            int i = 0;
            int j = 0;
            int k = 0;

            foreach (User user in Users)
            {
                IndexedUsers.Add(new IndexedUser(i, user));
                i++;

                switch (user.UserLevel.Id)
                {
                    case 0:
                        IndexedInactiveUsers.Add(new IndexedUser(k, user));
                        k++;
                        break;
                    default:
                        IndexedActiveUsers.Add(new IndexedUser(j, user));
                        j++;
                        break;
                }
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed User Levels
        /// </summary>
        private void RefreshIndexedUserLevels()
        {
            RefreshList("UserLevels");
            IndexedUserLevels.Clear();

            int i = 0;

            foreach (UserLevel userLevel in UserLevels)
            {
                IndexedUserLevels.Add(new IndexedUserLevel(i, userLevel));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed ZipTowns
        /// </summary>
        private void RefreshIndexedZipTowns()
        {
            RefreshList("ZipTowns");
            IndexedZipTowns.Clear();

            int i = 0;

            foreach (ZipTown zipTown in ZipTowns)
            {
                IndexedZipTowns.Add(new IndexedZipTown(i, zipTown));
                i++;
            }
        }

        #endregion

        /// <summary>
        /// Method, that loads images for buttons
        /// </summary>
        private void SetIndicators()
        {
            Uri buttonUri = new Uri(@"Images\button.png", UriKind.Relative);
            Uri smallButtonUri = new Uri(@"Images\button-small.png", UriKind.Relative);
            Uri broadButtonUri = new Uri(@"Images\button-small-broad.png", UriKind.Relative);
            Uri greenUri = new Uri(@"Images\green-indicator.png", UriKind.Relative);
            Uri redUri = new Uri(@"Images\red-indicator.png", UriKind.Relative);

            StreamResourceInfo buttonStreamInfo = Application.GetResourceStream(buttonUri);
            StreamResourceInfo smallButtonStreamInfo = Application.GetResourceStream(smallButtonUri);
            StreamResourceInfo broadButtonStreamInfo = Application.GetResourceStream(broadButtonUri);
            StreamResourceInfo greenStreamInfo = Application.GetResourceStream(greenUri);
            StreamResourceInfo redStreamInfo = Application.GetResourceStream(redUri);

            BitmapFrame buttonFrame = BitmapFrame.Create(buttonStreamInfo.Stream);
            BitmapFrame smallButtonFrame = BitmapFrame.Create(smallButtonStreamInfo.Stream);
            BitmapFrame broadButtonFrame = BitmapFrame.Create(broadButtonStreamInfo.Stream);
            BitmapFrame greenFrame = BitmapFrame.Create(greenStreamInfo.Stream);
            BitmapFrame redFrame = BitmapFrame.Create(redStreamInfo.Stream);

            button.ImageSource = buttonFrame;
            smallButton.ImageSource = smallButtonFrame;
            broadButton.ImageSource = broadButtonFrame;
            greenIndicator.ImageSource = greenFrame;
            redIndicator.ImageSource = redFrame;

        }

        #region Zips
        /// <summary>
        /// Method, that detects obsolete faroese zips
        /// - obsolete faroese zips are zips within the range [3800;3899] written without prefix
        /// </summary>
        /// <param name="zip">string</param>
        private void DetectObsoleteFaroeseZip()
        {
            if (tempZip.Length == 4)
            {
                if (CanConvertStringToInt(tempZip))
                {
                    if (intZip >= 3800 && intZip <= 3899)
                    {
                            tempZip = RetrieveNewFaroeseZip(tempZip);
                    }

                }

            }

        }

        /// <summary>
        /// Method, that exchanges obsolete faroese zip to valid zip
        /// </summary>
        /// <param name="zip">string</param>
        /// <returns>string</returns>
        private string RetrieveNewFaroeseZip(string zip)
        {
            string result = "FO-999";

            switch (zip)
            {
                case "3800":
                    result = "FO-100"; //Tórshavn
                    break;
                case "3813":
                    result = "FO-530"; //Fuglafjørdur 
                    break;
                case "3820":
                    result = "FO-240"; //Skopun
                    break;
                case "3821":
                    result = "FO-210"; //Sandur
                    break;
                case "3822":
                    result = "FO-220"; //Skálavík
                    break;
                case "3823":
                    result = "FO-235"; //Dalur
                    break;
                case "3826":
                    result = "FO-260"; //Skúvoy
                    break;
                case "3827":
                    result = "FO-270"; //Nólsoy
                    break;
                case "3828":
                    result = "FO-280"; //Hestur
                    break;
                case "3829":
                    result = "FO-285"; //Koltur
                    break;
                case "3830":
                    result = "FO-466"; //Ljósá
                    break;
                case "3831":
                    result = "FO-478"; //Elduvik
                    break;
                case "3832":
                    result = "FO-475"; //Funningur
                    break;
                case "3833":
                    result = "FO-476"; //Gjógv
                    break;
                case "3834":
                    result = "FO-470"; //Eiði
                    break;
                case "3835":
                    result = "FO-430"; //Hvalvik
                    break;
                case "3836":
                    result = "FO-420"; //Hósvík
                    break;
                case "3837":
                    result = "FO-440"; //Haldarsvik
                    break;
                case "3840":
                    result = "FO-530"; //Fuglafjørdur
                    break;
                case "3842":
                    result = "FO-F20"; //Leirvik
                    break;
                case "3843":
                    result = "FO-510"; //Gøta
                    break;
                case "3845":
                    result = "FO-485"; //Skálafjørður
                    break;
                case "3846":
                    result = "FO-495"; //Kolbanargjógv
                    break;
                case "3847":
                    result = "FO-496"; //Morskranes
                    break;
                case "3850":
                    result = "FO-600"; //Saltangará
                    break;
                case "3854":
                    result = "FO-640"; //Rituvík
                    break;
                case "3855":
                    result = "FO-655"; //Nes, Eysturoy
                    break;
                case "3856":
                    result = "FO-660"; //Søldarfjørður
                    break;
                case "3857":
                    result = "FO-666"; //Gøtueiði
                    break;
                case "3859":
                    result = "FO-695"; //Hellur
                    break;
                case "3860":
                    result = "FO-350"; //Vestmanna
                    break;
                case "3862":
                    result = "FO-410"; //Kollafjørður
                    break;
                case "3863":
                    result = "FO-335"; //Leynar
                    break;
                case "3864":
                    result = "FO-340"; //Kvívík
                    break;
                case "3865":
                    result = "FO-360"; //Sandavágur
                    break;
                case "3866":
                    result = "FO-370"; //Miðvágur
                    break;
                case "3867":
                    result = "FO-386"; //Bøur
                    break;
                case "3868":
                    result = "FO-385"; //Vatnsoyrar
                    break;
                case "3869":
                    result = "FO-388"; //Mykines
                    break;
                case "3870":
                    result = "FO-700"; //Klaksvík
                    break;
                case "3874":
                    result = "FO-726"; //Ánir
                    break;
                case "3875":
                    result = "FO-750"; //Viðareiði
                    break;
                case "3876":
                    result = "FO-765"; //Svínoy
                    break;
                case "3877":
                    result = "FO-767"; //Hattarvík
                    break;
                case "3878":
                    result = "FO-785"; //Haraldssund
                    break;
                case "3879":
                    result = "FO-796"; //Húsar
                    break;
                case "3880":
                    result = "FO-800"; //Tvøroyri
                    break;
                case "3885":
                    result = "FO-850"; //Hvalba
                    break;
                case "3886":
                    result = "FO-860"; //Sandvík
                    break;
                case "3887":
                    result = "FO-870"; //Fámjin
                    break;
                case "3890":
                    result = "FO-900"; //Vágur
                    break;
                case "3895":
                    result = "FO-950"; //Porkeri
                    break;
                case "3896":
                    result = "FO-960"; //Hov
                    break;
                case "3897":
                    result = "FO-970"; //Sumba
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a town based on the zip
        /// </summary>
        /// <param name="zip">string</param>
        /// <returns>string</returns>
        public string RetrieveTownFromZip(string zip)
        {
            RetrieveTempZipTown(zip);
            return TempZipTown.Town;
        }

        /// <summary>
        /// Method, that retrieves TempZipTown based on the zip
        /// </summary>
        /// <param name="zip">string</param>
        public void RetrieveTempZipTown(string zip)
        {
            TempZipTown = new ZipTown();
            bool zipFound = false;
            tempZip = zip;

            //Analyse zip
            if (zip.Length >= 3)
            {
                //Validate Zips
                ValidateZip();

                //Search valid in ZipTown list
                if (validZip)
                {
                    foreach (ZipTown zipTown in ZipTowns)
                    {
                        if (zipTown.Zip == zip)
                        {
                            TempZipTown = zipTown;
                            zipFound = true;
                            break;
                        }
                    }

                }
            }

            if (!zipFound)
            {
                TempZipTown = GetZipTown(1100);
            }

        }

        /// <summary>
        /// Method, that validates danish zips
        /// . Danish zips are 4 digits written witout prefix
        /// ; within the ranges
        /// : [0100;2411], [2413;3799] and [4000;9999] 
        /// </summary>
        /// <param name="zip">string</param>
        private void ValidateDanishZip()
        {
            bool canConvertTempZipToInt = CanConvertStringToInt(tempZip);

            if (tempZip.Length == 4)
            {
                intZip = 0;

                if (CanConvertStringToInt(tempZip))
                {
                    if (tempZip.Remove(1) == "0")
                    {
                        danishZip = true;
                    }
                    else if (intZip >= 1000 && intZip <= 2411 || intZip >= 2413 && intZip <= 3799 || intZip >= 4000 && intZip <= 9999)
                    {
                        danishZip = true;
                    }
                }

            }
            else
            {
                danishZip = false;
            }

        }

        /// <summary>
        /// Method, that validates faroese zips
        /// . Faroese zips are 3 digits
        /// , and always written with prefix in Denmark
        /// ; e.g. FO-100 Tórshavn
        /// </summary>
        private void ValidateFaroeseZip()
        {
            DetectObsoleteFaroeseZip();

            switch (tempZip.Length)
            {
                case 3:
                    intZip = 0;

                    if (CanConvertStringToInt(tempZip))
                    {
                        if (intZip >= 100 && intZip <= 999)
                        {
                            faroeseZip = true;
                            tempZip = "FO-" + tempZip;
                        }
                        else
                        {
                            faroeseZip = false;
                        }
                    }
                    break;
                case 6:
                    if (tempZip.Remove(2) == "Fo" || tempZip.Remove(2) == "fo")
                    {
                        faroeseZip = true;
                        tempZip = "FO" + tempZip.Remove(2, 5);
                    }
                    else if (tempZip.Remove(2) == "FO")
                    {
                        faroeseZip = true;
                    }
                    else
                    {
                        faroeseZip = false;
                    }
                    break;
                default:
                    faroeseZip = false;
                    break;
            }
        }

        /// <summary>
        /// Method, that validates foreign zips 
        /// . Foreign zips differ in format 
        /// . In Denmark they are mostly written with prefix 
        /// ; e.g. D-24955 or NL-7891 WS
        /// . Excludes danish, faroese and kaalaalit zips
        /// . Specifically validate british postcodes
        /// ; e.g. EC1A 1BB or W1A 0AX
        /// </summary>
        private void ValidateForeignZip()
        {
            if (tempZip.Length >= 3 && !danishZip && !faroeseZip && !kaalaalitZip)
            {
                if (UkZipApi.ValidateUkZip(tempZip))
                {
                    foreignZip = true;
                }
                else if (!CanConvertStringToInt(tempZip.Remove(1)))
                {
                    foreignZip = true;
                }
            }
            else
            {
                foreignZip = false;
            }
        }

        /// <summary>
        /// Method, that validates kaalaalit zips
        /// . Kaalaalit zips are 4 digits 
        /// , and usually written with prefix in Denmark 
        /// ; e.g. GL-3900 Nuuk
        /// . An ordinary valid zip is within the range [3900;3999] 
        /// . 2412 is an exclusive zip for Juullip Inua 
        /// (Santa Claus - December 24th is Christmas Eve in Kingdom of Denmark)
        /// </summary>
        private void ValidateKaalaalitZip()
        {
            switch (tempZip.Length)
            {
                case 4:
                    intZip = 0;

                    if (CanConvertStringToInt(tempZip))
                    {
                        if (intZip >= 3900 && intZip <= 3999)
                        {
                            kaalaalitZip = true;
                            tempZip = "GL-" + tempZip;
                        }
                        else if (intZip == 2412)
                        {
                            kaalaalitZip = true;
                            tempZip = "GL-2412";
                        }
                        else
                        {
                            kaalaalitZip = false;
                        }
                    }
                    else
                    {
                        kaalaalitZip = false;
                    }

                    break;
                case 7:
                    if (tempZip.Remove(2) == "Fo" || tempZip.Remove(2) == "fo")
                    {
                        faroeseZip = true;
                        tempZip = "FO" + tempZip.Remove(2, 6);
                    }
                    else if (tempZip.Remove(2) == "FO")
                    {
                        faroeseZip = true;
                    }
                    else
                    {
                        faroeseZip = false;
                    }
                    break;
                default:
                    kaalaalitZip = false;
                    break;
            }
            if (tempZip.Length == 4)
            {

            }
            else
            {
                kaalaalitZip = false;
            }

        }

        /// <summary>
        /// Method, that validates zips
        /// . A valid zip is at least 3 CHARs
        /// ; is written with or without prefix in Denmark.
        /// ; e.g. 6230 Rødekro, FO-100 Torshavn, GL-3900 Nuuk or D-24955 Harrislee
        /// </summary>
        /// <param name="zip">string</param>
        private void ValidateZip()
        {
            if (tempZip.Length > 2)
            {
                ValidateDanishZip();

                if (!danishZip)
                {
                    ValidateFaroeseZip();

                    if (!faroeseZip)
                    {
                        ValidateKaalaalitZip();

                        if (!kaalaalitZip)
                        {
                            ValidateForeignZip();
                        }
                    }
                }

                if (danishZip || faroeseZip || foreignZip || kaalaalitZip)
                {
                    validZip = true;
                }
            }
            else
            {
                validZip = false;
            }

        }

        #endregion

        #endregion

    }
}
