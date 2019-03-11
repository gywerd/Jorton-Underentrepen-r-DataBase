﻿using JudBizz;
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
        public List<IndexedEntrepeneur> IndexedLegalEntities = new List<IndexedEntrepeneur>();

        #endregion

        #region Constructors
        public UcSubEntrepeneursChoose(Bizz bizz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = bizz;
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
                Contact tempContact = GetContact();
                CBZ.TempSubEntrepeneur.Contact = tempContact;

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
                TextBoxName.Text = "";

                //Update Enterprise List
                CBZ.RefreshList("SubEntrepeneurs");
                IndexedLegalEntities.Clear();
                IndexedLegalEntities = GetIndexedEntrepeneurs();
                ListBoxEntrepeneurs.ItemsSource = IndexedLegalEntities;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Underentrepenøren blev ikke føjet til Entrepriselisten. Prøv igen.", "Rediger Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke Vælg Underentrepenør?", "Luk Vælg Underentrepenør", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                }
            }
            TextBoxName.Text = CBZ.TempProject.Name;
            IndexedEnterprises = GetIndexedEnterprises();
            ComboBoxEnterprise.ItemsSource = IndexedEnterprises;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ComboBoxContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxEnterprise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxEnterprise.SelectedIndex;
            if (IndexedEnterprises.Count == 0)
            {
                IndexedEnterprises = GetIndexedEnterprises();
            }
            foreach (IndexedEnterprise temp in IndexedEnterprises)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempEnterprise = new Enterprise(temp);
                    break;
                }
            }
            CBZ.RefreshList("LegalEntities");
            IndexedLegalEntities.Clear();
            IndexedLegalEntities = GetIndexedEntrepeneurs();
            ListBoxEntrepeneurs.ItemsSource = IndexedLegalEntities;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxEntrepeneurs.SelectedItems.Count < 2)
            {
                Entrepeneur selected = new Entrepeneur((IndexedEntrepeneur)ListBoxEntrepeneurs.SelectedItem);
                foreach (Entrepeneur temp in CBZ.Entrepeneurs)
                {
                    if (temp.Id == selected.Id)
                    {
                        CBZ.TempEntrepeneur = temp;
                        CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                        CBZ.TempSubEntrepeneur.Enterprise = CBZ.TempEnterprise;
                        CBZ.TempSubEntrepeneur.Entrepeneur = new Entrepeneur(temp);
                        if (!CBZ.TempSubEntrepeneur.Active)
                        {
                            CBZ.TempSubEntrepeneur.ToggleActive();
                        }
                    }
                }
                CBZ.RefreshList("Contacts");
                IndexedContacts.Clear();
                IndexedContacts = GetIndexedContacts();
                //ListBoxEntrepeneurs.ItemsSource = IndexedLegalEntities;
                ComboBoxContact.ItemsSource = IndexedContacts;
                ComboBoxContact.SelectedIndex = 0;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
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
                CBZ.TempEntrepeneur = entrepeneur;
                CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                CBZ.TempSubEntrepeneur.Enterprise = CBZ.TempEnterprise;
                CBZ.TempSubEntrepeneur.Entrepeneur = entrepeneur;
                if (!CBZ.TempSubEntrepeneur.Active)
                {
                    CBZ.TempSubEntrepeneur.ToggleActive();
                }
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
            if (entrepeneur.CraftGroup1.Id != 0)
            {
                if (entrepeneur.CraftGroup1.Id == CBZ.TempEnterprise.CraftGroup1.Id || entrepeneur.CraftGroup1.Id == CBZ.TempEnterprise.CraftGroup2.Id || entrepeneur.CraftGroup1.Id == CBZ.TempEnterprise.CraftGroup3.Id || entrepeneur.CraftGroup1.Id == CBZ.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            if (entrepeneur.CraftGroup2.Id != 0)
            {
                if (entrepeneur.CraftGroup2.Id == CBZ.TempEnterprise.CraftGroup1.Id || entrepeneur.CraftGroup2.Id == CBZ.TempEnterprise.CraftGroup2.Id || entrepeneur.CraftGroup2.Id == CBZ.TempEnterprise.CraftGroup3.Id || entrepeneur.CraftGroup2.Id == CBZ.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            if (entrepeneur.CraftGroup3.Id != 0)
            {
                if (entrepeneur.CraftGroup3.Id == CBZ.TempEnterprise.CraftGroup1.Id || entrepeneur.CraftGroup3.Id == CBZ.TempEnterprise.CraftGroup2.Id || entrepeneur.CraftGroup3.Id == CBZ.TempEnterprise.CraftGroup3.Id || entrepeneur.CraftGroup3.Id == CBZ.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            if (entrepeneur.CraftGroup4.Id != 0)
            {
                if (entrepeneur.CraftGroup4.Id == CBZ.TempEnterprise.CraftGroup1.Id || entrepeneur.CraftGroup4.Id == CBZ.TempEnterprise.CraftGroup2.Id || entrepeneur.CraftGroup4.Id == CBZ.TempEnterprise.CraftGroup3.Id || entrepeneur.CraftGroup4.Id == CBZ.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            return false;
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
        /// <returns></returns>
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
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxContact
        /// </summary>
        private void GenerateContactItems()
        {
            ComboBoxContact.Items.Clear();
            IndexedContacts = GetIndexedContacts();
            foreach (IndexedContact temp in IndexedContacts)
            {
                    ComboBoxContact.Items.Add(temp);
            }
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxEnterprise
        /// </summary>
        private void GenerateEnterpriseItems()
        {
            ComboBoxEnterprise.Items.Clear();
            IndexedEnterprises = GetIndexedEnterprises();
            foreach (Enterprise temp in IndexedEnterprises)
            {
                    ComboBoxEnterprise.Items.Add(temp);
            }
        }

        /// <summary>
        /// Method, that finds a Contact
        /// </summary>
        /// <returns>Contact</returns>
        private Contact GetContact()
        {
            Contact result = new Contact();
            int index = ComboBoxContact.SelectedIndex;
            if (IndexedContacts.Count == 0)
            {
                GetIndexedContacts();
            }
            foreach (IndexedContact tempContact in IndexedContacts)
            {
                if (tempContact.Index == index)
                {
                    return tempContact;
                }
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method that creates a list of indexable Contacts
        /// </summary>
        /// <returns>List<IndexedSubEntrepeneur></returns>
        private List<IndexedContact> GetIndexedContacts()
        {
            List<IndexedContact> result = new List<IndexedContact>();
            IndexedContact iContact = new IndexedContact(0, CBZ.Contacts[0]);
            result.Add(iContact);
            int i = 1;
            foreach (Contact contact in CBZ.Contacts)
            {
                if (contact.Entrepeneur.Id == CBZ.TempLegalEntity.Id)
                {
                    IndexedContact temp = new IndexedContact(i, contact);
                    result.Add(temp);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that creates an indexable Enterprises
        /// </summary>
        /// <returns>List<IndexedEnterprise></returns>
        private List<Enterprise> GetIndexedEnterprises()
        {
            List<Enterprise> result = new List<Enterprise>();
            int i = 0;
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    IndexedEnterprise temp = new IndexedEnterprise(i, enterprise);
                    result.Add(temp);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Method that creates a list of indexable Legal Entities
        /// </summary>
        /// <returns>List<IndexedEntrepeneur></returns>
        private List<IndexedEntrepeneur> GetIndexedEntrepeneurs()
        {
            List<Entrepeneur> tempResult = new List<Entrepeneur>();
            List<IndexedEntrepeneur> result = new List<IndexedEntrepeneur>();
            IndexedEntrepeneur temp = new IndexedEntrepeneur(0, CBZ.Entrepeneurs[0]);
            result.Add(temp);
            int i = 1;
            foreach (Entrepeneur entrepeneur in CBZ.Entrepeneurs)
            {
                if (CheckCraftGroups(entrepeneur))
                {
                    tempResult.Add(entrepeneur);
                    i++;
                }
            }
            result = FilterIndexedEntrepeneur(tempResult);
            return result;
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