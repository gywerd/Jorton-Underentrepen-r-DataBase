using JudBizz;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JudGui
{
    /// <summary>
    /// Interaction logic for UcSubEntrepeneursChoose.xaml
    /// </summary>
    public partial class UcSubEntrepeneursChoose : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public List<IndexedContact> IndexedContacts = new List<IndexedContact>();
        public List<Enterprise> IndexedEnterprises = new List<Enterprise>();

        public List<IndexedEntrepeneur> FilteredEntrepeneurs = new List<IndexedEntrepeneur>();

        #endregion

        #region Constructors
        public UcSubEntrepeneursChoose(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            GenerateComboBoxCaseIdItems();
            ComboBoxArea.ItemsSource = CBZ.Regions;
        }

        #endregion

        #region Buttons
        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (ListBoxEntrepeneurs.SelectedItems.Count == 0)
            {
                //Show Confirmation
                MessageBox.Show("Du har ikke valgt nogen underentrepenører.", "Vælg Underentrepenør", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ListBoxEntrepeneurs.SelectedItems.Count == 1)
            {
                int id = 0;

                //Code that adds IttLetter, Offer and Request to Bizz.TempSubEntrepeneur
                CreateIttLetter();
                CreateOffer();
                CreateRequest();

                //Code that adds a SubEntrepeneur to Enterprise List
                id = CBZ.CreateInDb(CBZ.TempSubEntrepeneur);
                if (id >= 1)
                {
                    result = true;
                }
            }
            else
            {
                result = AddMultipleSubentrepeneurs();
            }
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Underentrepenøren/-ne blev føjet til Entrepriselisten. Ved flere underentrepenører, er der ikke valgt kontaktperson. Ret dette under 'Rediger Underentrepenør'", "Vælg Underentrepenør", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ComboBoxCaseId.SelectedIndex = -1;
                TextBoxName.Text = "";
                ListBoxEntrepeneurs.SelectedIndex = -1;
                ListBoxEntrepeneurs.ItemsSource = "";
                ComboBoxContact.SelectedIndex = -1;
                ComboBoxContact.ItemsSource = "";


                //Update SubEntrepeneurs List
                CBZ.RefreshList("SubEntrepeneurs");
                CBZ.UcMainEdited = false;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Underentrepenøren blev ikke føjet til Entrepriselisten. Prøv igen.", "Rediger Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Vælg Underentrepenør?", "Luk Vælg Underentrepenør", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);
                TextBoxName.Text = CBZ.TempProject.Details.Name;

                RefreshIndexedEnterprises();
                ComboBoxEnterprise.ItemsSource = "";
                ComboBoxEnterprise.ItemsSource = CBZ.IndexedEnterprises;

                CBZ.TempSubEntrepeneur = new SubEntrepeneur();

                if (!CBZ.TempSubEntrepeneur.Active)
                {
                    CBZ.TempSubEntrepeneur.ToggleActive();
                }

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
        }

        private void ComboBoxContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxContact.SelectedIndex > 0)
            {
                CBZ.TempSubEntrepeneur.Contact = new Contact((Contact)ComboBoxContact.SelectedItem);

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
        }

        private void ComboBoxEnterprise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEnterprise.SelectedIndex > -1)
            {
                CBZ.TempSubEntrepeneur.Enterprise = new Enterprise((Enterprise)ComboBoxEnterprise.SelectedItem);
                CBZ.RefreshList("SubEntrepeneurs");
                GetFilteredEntrepeneurs();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = FilteredEntrepeneurs;
                ComboBoxContact.SelectedIndex = -1;
                ComboBoxContact.ItemsSource = "";

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }

        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxEntrepeneurs.SelectedItems.Count == 1)
            {
                CBZ.TempSubEntrepeneur.Entrepeneur = new Entrepeneur((IndexedEntrepeneur)ListBoxEntrepeneurs.SelectedItem);
                CBZ.RefreshIndexedList("Contacts");
                ComboBoxContact.ItemsSource = "";
                ComboBoxContact.ItemsSource = CBZ.IndexedContacts;
                ComboBoxContact.SelectedIndex = 0;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredEntrepeneurs();
            ListBoxEntrepeneurs.SelectedIndex = -1;
            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = this.FilteredEntrepeneurs;

        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that adds multiple SubEntrepeneurs to Db
        /// </summary>
        /// <returns></returns>
        private bool AddMultipleSubentrepeneurs()
        {
            bool result = false;
            List<IndexedEntrepeneur> tempList = new List<IndexedEntrepeneur>();
            foreach (IndexedEntrepeneur entrepeneur in ListBoxEntrepeneurs.SelectedItems)
            {
                CBZ.TempSubEntrepeneur.Entrepeneur = entrepeneur;
                CBZ.TempSubEntrepeneur.Contact = new Contact();
                CreateIttLetter();
                CreateOffer();
                CreateRequest();

                //Code that adds a enterprise to Enterprise List
                int id = 0;
                bool tempResult = false;
                id = CBZ.CreateInDb(CBZ.TempSubEntrepeneur);
                if (id >= 1)
                {
                    tempResult = true;
                }

                //Code, that checks result
                if (!result)
                {
                    result = tempResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that compares CraftGroups in LegalEntities and Enterprises
        /// </summary>
        /// <param name="entrepeneur"></param>
        /// <returns></returns>
        private bool CheckCraftGroups(Entrepeneur entrepeneur)
        {
            bool result = false;
            if (entrepeneur.CraftGroup1.Id != 0)
            {
                if (entrepeneur.CraftGroup1.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup1.Id || entrepeneur.CraftGroup1.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup2.Id || entrepeneur.CraftGroup1.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup3.Id || entrepeneur.CraftGroup1.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup4.Id)
                {
                    result = true;
                }
            }
            if (entrepeneur.CraftGroup2.Id != 0)
            {
                if (entrepeneur.CraftGroup2.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup1.Id || entrepeneur.CraftGroup2.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup2.Id || entrepeneur.CraftGroup2.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup3.Id || entrepeneur.CraftGroup2.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup4.Id)
                {
                    result = true;
                }
            }
            if (entrepeneur.CraftGroup3.Id != 0)
            {
                if (entrepeneur.CraftGroup3.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup1.Id || entrepeneur.CraftGroup3.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup2.Id || entrepeneur.CraftGroup3.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup3.Id || entrepeneur.CraftGroup3.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup4.Id)
                {
                    result = true;
                }
            }
            if (entrepeneur.CraftGroup4.Id != 0)
            {
                if (entrepeneur.CraftGroup4.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup1.Id || entrepeneur.CraftGroup4.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup2.Id || entrepeneur.CraftGroup4.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup3.Id || entrepeneur.CraftGroup4.Id == CBZ.TempSubEntrepeneur.Enterprise.CraftGroup4.Id)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that inserts an Offer to Db, and register it in Bizz.tempSubentrepeneur
        /// </summary>
        private void CreateIttLetter()
        {
            CBZ.TempIttLetter = new IttLetter();
            int id = CBZ.CreateInDb(CBZ.TempIttLetter);
            CBZ.TempIttLetter.SetId(id);
            CBZ.TempSubEntrepeneur.IttLetter = CBZ.TempIttLetter;
        }

        /// <summary>
        /// Method, that inserts an Offer to Db, and register it in Bizz.tempSubentrepeneur
        /// </summary>
        private void CreateOffer()
        {
            CBZ.TempOffer = new Offer();
            int id = CBZ.CreateInDb(CBZ.TempOffer);
            CBZ.TempOffer.SetId(id);
            CBZ.TempSubEntrepeneur.Offer = CBZ.TempOffer;
        }

        /// <summary>
        /// Method, that inserts a Request to Db, and register it in Bizz.tempSubentrepeneur
        /// </summary>
        private void CreateRequest()
        {
            CBZ.TempRequest = new Request();
            int id = CBZ.CreateInDb(CBZ.TempRequest);
            CBZ.TempRequest.SetId(id);
            CBZ.TempSubEntrepeneur.Request = CBZ.TempRequest;
        }

        /// <summary>
        /// Method, that filters existing Legal Entities in SubEntrepeneurs from list of indexable Legal Entities
        /// </summary>
        /// <param name="result"></param>
        /// <returns>List<IndexedEntrepeneur></returns>
        private List<IndexedEntrepeneur> FilterIndexedEntrepeneur(List<Entrepeneur> list)
        {
            List<Entrepeneur> tempResult = new List<Entrepeneur>();
            List<IndexedEntrepeneur> result = new List<IndexedEntrepeneur>();
            Entrepeneur tempEntrepeneur = new Entrepeneur();
            foreach (Entrepeneur entrepeneur in list)
            {
                if (entrepeneur.Region.Id == ComboBoxArea.SelectedIndex)
                {
                    tempResult.Add(entrepeneur);
                }
            }
            foreach (Entrepeneur entrepeneur in tempResult)
            {
                if (entrepeneur.Region.Id != ComboBoxArea.SelectedIndex && entrepeneur.CountryWide.Equals(true))
                {
                    tempResult.Add(entrepeneur);
                }
            }
            int i = 0;
            foreach (Entrepeneur temp in tempResult)
            {
                if (!IdExistsInSubEntrepeneurs(CBZ.TempEnterprise.Id, temp.Id))
                {
                    IndexedEntrepeneur entity = new IndexedEntrepeneur(i, temp);
                    result.Add(entity);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxContact
        /// </summary>
        private void GenerateContactItems()
        {
            ComboBoxContact.ItemsSource = "";
            ComboBoxContact.ItemsSource = CBZ.IndexedContacts;
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxEnterprise
        /// </summary>
        private void GenerateEnterpriseItems()
        {
            ComboBoxEnterprise.ItemsSource = "";
            ComboBoxEnterprise.ItemsSource = CBZ.IndexedEnterprises;
        }

        /// <summary>
        /// Method, that retrieves a list of filtered Entrepeneurs for ListBoxEntrepeneurs
        /// </summary>
        private void GetFilteredEntrepeneurs()
        {
            List<Entrepeneur> tempResult = new List<Entrepeneur>();
            List<IndexedEntrepeneur> result = new List<IndexedEntrepeneur>();
            this.FilteredEntrepeneurs.Clear();
            CBZ.RefreshList("Entrepeneurs");

            int length = TextBoxSearch.Text.Length;

            if (length >= 1)
            {

                foreach (Entrepeneur entrepeneur in CBZ.ActiveEntrepeneurs)
                {
                    if (entrepeneur.Entity.Name.Length >= length)
                    {
                        if (entrepeneur.Entity.Name.Remove(length).ToLower() == TextBoxSearch.Text.ToLower())
                        {
                            if (CheckCraftGroups(entrepeneur))
                            {
                                tempResult.Add(entrepeneur);
                            }
                        }
                    }
                }

            }
            else
            {
                foreach (Entrepeneur entrepeneur in CBZ.ActiveEntrepeneurs)
                {
                    if (CheckCraftGroups(entrepeneur))
                    {
                        tempResult.Add(entrepeneur);
                    }
                }

            }

            result = FilterIndexedEntrepeneur(tempResult);

            int i = 0;

            foreach (Entrepeneur entrepeneur in result)
            {
                this.FilteredEntrepeneurs.Add(new IndexedEntrepeneur(i, entrepeneur));
                i++;
            }
        }

        /// <summary>
        /// Method, that finds a Contact
        /// </summary>
        /// <returns>Contact</returns>
        private Contact GetContact()
        {
            return new Contact((Contact)ComboBoxContact.SelectedItem);
        }

        /// <summary>
        /// Method that creates a list of indexable Contacts
        /// </summary>
        private void RefreshIndexedContacts() => CBZ.RefreshIndexedList("Contacts");

        /// <summary>
        /// Method, that creates an indexable Enterprises
        /// </summary>
        private void RefreshIndexedEnterprises()
        {
            CBZ.RefreshProjectList("All", CBZ.TempProject.Id);
            CBZ.IndexedEnterprises.Clear();

            int i = 0;

            foreach (Enterprise enterprise in CBZ.ProjectLists.Enterprises)
            {
                CBZ.IndexedEnterprises.Add(new IndexedEnterprise(i, enterprise));
                i++;
            }
        }

        /// <summary>
        /// <summary>
        /// Method, that checks if a legal entity is already added to SubEntrepeneurs
        /// </summary>
        /// <param name="enterpriseId">int</param>
        /// <param name="entrepeneur">string</param>
        /// <returns>bool</returns>
        private bool IdExistsInSubEntrepeneurs(int enterpriseId, int entrepeneurId)
        {
            bool result = false;

            foreach (SubEntrepeneur temp in CBZ.SubEntrepeneurs)
            {
                if (temp.Entrepeneur.Id == entrepeneurId && temp.Enterprise.Id == enterpriseId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        #endregion

    }
}
