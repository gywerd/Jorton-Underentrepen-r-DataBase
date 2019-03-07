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

namespace JudBizz
{
    public class Bizz : MyEntityFrameWork
    {
        #region Fields

        public CvrAPI CvrApi;
        public bool UcMainEdited = false;

        public List<IndexedProject> IndexedActiveProjects = new List<IndexedProject>();
        public List<IndexedBuilder> IndexedBuilders = new List<IndexedBuilder>();
        public List<IndexedCraftGroup> IndexedCraftGroups = new List<IndexedCraftGroup>();
        public List<IndexedContact> IndexedContacts = new List<IndexedContact>();
        public List<IndexedEnterpriseForm> IndexedEnterpriseForms = new List<IndexedEnterpriseForm>();
        public List<IndexedEnterprise> IndexedEnterprises = new List<IndexedEnterprise>();
        public List<IndexedEntrepeneur> IndexedEntrepeneurs = new List<IndexedEntrepeneur>();
        public List<IndexedParagraph> IndexedParagraphs = new List<IndexedParagraph>();
        public List<IndexedProject> IndexedProjects = new List<IndexedProject>();
        public List<IndexedProjectStatus> IndexedProjectStatuses = new List<IndexedProjectStatus>();
        public List<IndexedRegion> IndexedRegions = new List<IndexedRegion>();
        public List<IndexedRequest> IndexedRequests = new List<IndexedRequest>();
        public List<IndexedRequestStatus> IndexedRequestStatuses = new List<IndexedRequestStatus>();
        public List<SubEntrepeneur> IndexedSubEntrepeneurs = new List<SubEntrepeneur>();
        public List<IndexedTenderForm> IndexedTenderForms = new List<IndexedTenderForm>();
        public List<IndexedUser> IndexedUsers = new List<IndexedUser>();

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Bizz()
        {
            RefreshAllInitialIndexedLists();
            CvrApi = new CvrAPI(ZipTowns);
        }

        #endregion

        #region Properties
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
                if (user.Initials == initials && user.Authentication.PassWord == passWord && user.Authentication.UserLevel.Id >= 1)
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

        #region Refresh Indexed Lists
        /// <summary>
        /// Method, that checks, whether a Entrepeneur exists in Receivers list
        /// </summary>
        /// <param name="entrepeneur">IndexedEntrepeneur</param>
        /// <returns>bool</returns>
        private bool CheckEntrepeneurReceivers(Entrepeneur entrepeneur)
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
        private bool CheckEntrepeneurTempResult(Entrepeneur tempEntrepeneur, List<Entrepeneur> tempResult)
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
        /// Method, that refreshes all Indexed lists
        /// </summary>
        public void RefreshAllInitialIndexedLists()
        {
            RefreshIndexedActiveProjects();
            RefreshIndexedCraftGroups();
            RefreshIndexedProjects();
            RefreshIndexedEnterpriseForms();
            RefreshIndexedProjectStatuses();
            RefreshIndexedRegions();
            RefreshIndexedRequestStatuses();
            RefreshIndexedTenderForms();
        }

        /// <summary>
        /// Method, that refreshes active Indexed  Builders list
        /// </summary>
        private void RefreshIndexedActiveBuilders()
        {
            IndexedBuilders.Clear();
            RefreshList("Builders");

            int i = 0;
            foreach (Builder builder in Builders)
            {
                if (builder.Active)
                {
                    IndexedBuilders.Add(new IndexedBuilder(i, builder));
                    i++;
                }
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
        /// Method, that refreshes Indexed  Builders list
        /// </summary>
        private void RefreshIndexedBuilders()
        {
            IndexedBuilders.Clear();
            RefreshList("Builders");

            int i = 0;
            foreach (Builder builder in Builders)
            {
                IndexedBuilders.Add(new IndexedBuilder(i, builder));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed CraftGroups
        /// </summary>
        private void RefreshIndexedContacts()
        {
            RefreshList("Contacts");

            int i = 0;
            IndexedContacts.Clear();

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

            int i = 0;
            IndexedCraftGroups.Clear();

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
            IndexedEnterpriseForms.Clear();
            int i = 0;
            foreach (EnterpriseForm form in EnterpriseForms)
            {
                IndexedEnterpriseForms.Add(new IndexedEnterpriseForm(i, form));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed Entrepeneurs
        /// </summary>
        /// <returns>List<IndexedEntrepeneur></returns>
        private void RefreshIndexedEntrepeneurs()
        {
            RefreshList("ActiveEntrepeneurs");

            int i = 0;
            IndexedEntrepeneurs.Clear();

            foreach (Entrepeneur entrepeneur in ActiveEntrepeneurs)
            {
                IndexedEntrepeneurs.Add(new IndexedEntrepeneur(i, entrepeneur));
                i++;
            }
        }

        /// <summary>
        /// Method that refreshes a list of Indexed Entrepeneurs
        /// </summary>
        /// <returns>List<IndexedEntrepeneur></returns>
        private void RefreshIndexedEntrepeneursFromSubEntrepeneurs()
        {
            List<Entrepeneur> tempResult = new List<Entrepeneur>();
            RefreshList("ActiveEntrepeneurs");

            //Fill temporary Entrepeneur list
            foreach (SubEntrepeneur sub in PdfLists.SubEntrepeneurs)
            {
                foreach (Entrepeneur tempEntrepeneur in ActiveEntrepeneurs)
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

            //Refresh Indexed Entrepeneurs
            int i = 0;
            IndexedEntrepeneurs.Clear();

            foreach (Entrepeneur entrepeneur in tempResult)
            {
                bool exist = CheckEntrepeneurReceivers(entrepeneur);
                if (!exist)
                {
                    IndexedEntrepeneurs.Add(new IndexedEntrepeneur(i, entrepeneur));
                    i++;
                }
            }
        }

        /// <summary>
        /// Method, that refreshes an Indexed list
        /// </summary>
        public void RefreshIndexedList(string list)
        {
            switch (list)
            {
                case "IndexedActiveBuilders":
                    RefreshIndexedActiveBuilders();
                    break;
                case "IndexedActiveProjects":
                    RefreshIndexedActiveProjects();
                    break;
                case "IndexedBuilders":
                    RefreshIndexedBuilders();
                    break;
                case "IndexedContacts":
                    RefreshIndexedContacts();
                    break;
                case "IndexedCraftGroups":
                    RefreshIndexedCraftGroups();
                    break;
                case "IndexedEnterpriseForms":
                    RefreshIndexedEnterpriseForms();
                    break;
                case "IndexedEntrepeneurs":
                    RefreshIndexedEntrepeneurs();
                    break;
                case "IndexedEntrepeneursFromSubEntrepeneurs":
                    RefreshIndexedEntrepeneursFromSubEntrepeneurs();
                    break;
                case "IndexedProjects":
                    RefreshIndexedProjects();
                    break;
                case "IndexedProjectStatuses":
                    RefreshIndexedProjectStatuses();
                    break;
                case "IndexedRegions":
                    RefreshIndexedRegions();
                    break;
                case "IndexedRequestStatuses":
                    RefreshIndexedRequestStatuses();
                    break;
                case "IndexedTenderForms":
                    RefreshIndexedTenderForms();
                    break;
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
        /// Method, that refreshes Indexed Project Status list
        /// </summary>
        private void RefreshIndexedProjectStatuses()
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
        /// Method that refreshes a list of Indexed Regions
        /// </summary>
        private void RefreshIndexedRegions()
        {
            RefreshList("Regions");

            int i = 0;
            IndexedRegions.Clear();

            foreach (Region region in Regions)
            {
                IndexedRegions.Add(new IndexedRegion(i, region));
                i++;
            }
        }

        /// <summary>
        /// Method, that refreshes Indexed Request Status list
        /// </summary>
        private void RefreshIndexedRequestStatuses()
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

        public string RetrieveTownFromZip(string zip)
        {
            RetrieveZipTownFromZip(zip);
            return TempZipTown.Town;
        }

        public void RetrieveZipTownFromZip(string zip)
        {
            TempZipTown = new ZipTown();
            int intZip;
            bool zipFound = false;
            try
            {
                intZip = Convert.ToInt32(zip);
            }
            catch (Exception)
            {
                intZip = -1;
            }

            if (intZip > 0 && intZip < 900)
            {
                TempZipTown = new ZipTown((ZipTown)GetObject("ZipTowns", 1100));
                zipFound = true;
            }
            else if (intZip <= 0 || intZip >= 900)
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

            if (!zipFound)
            {
                TempZipTown = ZipTowns[0];
            }

        }

        #endregion

    }
}
