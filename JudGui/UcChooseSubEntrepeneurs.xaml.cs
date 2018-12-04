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
    /// Interaction logic for UcChooseSubEntrepeneurs.xaml
    /// </summary>
    public partial class UcChooseSubEntrepeneurs : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        public List<IndexedContact> IndexableContacts = new List<IndexedContact>();
        public List<Enterprise> IndexableEnterpriseList = new List<Enterprise>();
        public List<IndexedLegalEntity> IndexableLegalEntities = new List<IndexedLegalEntity>();

        #endregion

        #region Constructors
        public UcChooseSubEntrepeneurs(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            GenerateComboBoxCaseIdItems();
            ComboBoxArea.ItemsSource = Bizz.Regions;
        }

        #endregion

        #region Buttons
        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (ListBoxLegalEntities.SelectedItems.Count == 0)
            {
                //Show Confirmation
                MessageBox.Show("Du har ikke valgt nogen underentrepenører.", "Vælg Underentrepenør", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ListBoxLegalEntities.SelectedItems.Count == 1)
            {
                //Code that adds IttLetter, Offer and Request to Bizz.TempSubEntrepeneur
                CreateIttLetter();
                CreateOffer();
                CreateRequest();
                Contact tempContact = GetContact();
                Bizz.TempSubEntrepeneur.Contact = tempContact;

                //Code that adds a SubEntrepeneur to Enterprise List
                result = Bizz.CreateInDbReturnBool(Bizz.TempSubEntrepeneur);
            }
            else
            {
                result = AddMultipleSubentrepeneurs();
            }
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Underentrepenøre(r)n(e) blev føjet til Entrepriselisten. Ved flere underentrepenører, er der ikke valgt kontaktperson. Ret dette under 'Rediger Underentrepenør'", "Vælg Underentrepenør", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxName.Text = "";

                //Update Enterprise List
                Bizz.RefreshList("SubEntrepeneurs");
                IndexableLegalEntities.Clear();
                IndexableLegalEntities = GetIndexableLegalEntities();
                ListBoxLegalEntities.ItemsSource = IndexableLegalEntities;
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
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            TextBoxName.Text = Bizz.TempProject.Name;
            IndexableEnterpriseList = GetIndexableEnterpriseList();
            ComboBoxEnterprise.ItemsSource = IndexableEnterpriseList;
        }

        private void ComboBoxContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxEnterprise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxEnterprise.SelectedIndex;
            if (IndexableEnterpriseList.Count == 0)
            {
                IndexableEnterpriseList = GetIndexableEnterpriseList();
            }
            foreach (IndexedEnterprise temp in IndexableEnterpriseList)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempEnterprise = new Enterprise(temp);
                    break;
                }
            }
            Bizz.RefreshList("LegalEntities");
            IndexableLegalEntities.Clear();
            IndexableLegalEntities = GetIndexableLegalEntities();
            ListBoxLegalEntities.ItemsSource = IndexableLegalEntities;
        }

        private void ListBoxLegalEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxLegalEntities.SelectedItems.Count < 2)
            {
                int selectedIndex = ListBoxLegalEntities.SelectedIndex;
                foreach (IndexedLegalEntity temp in IndexableLegalEntities)
                {
                    if (temp.Index == selectedIndex)
                    {
                        Bizz.TempLegalEntity = temp;
                        Bizz.TempSubEntrepeneur = new SubEntrepeneur();
                        Bizz.TempSubEntrepeneur.EnterpriseList = Bizz.TempEnterprise;
                        Bizz.TempSubEntrepeneur.Entrepeneur = new LegalEntity(temp);
                        if (!Bizz.TempSubEntrepeneur.Active)
                        {
                            Bizz.TempSubEntrepeneur.ToggleActive();
                        }
                    }
                }
                Bizz.RefreshList("Contacts");
                IndexableContacts.Clear();
                IndexableContacts = GetIndexableContacts();
                //ListBoxLegalEntities.ItemsSource = IndexableLegalEntities;
                ComboBoxContact.ItemsSource = IndexableContacts;
                ComboBoxContact.SelectedIndex = 0;
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
            List<IndexedLegalEntity> tempList = new List<IndexedLegalEntity>();
            foreach (IndexedLegalEntity entity in ListBoxLegalEntities.SelectedItems)
            {
                Bizz.TempLegalEntity = entity;
                Bizz.TempSubEntrepeneur = new SubEntrepeneur();
                Bizz.TempSubEntrepeneur.EnterpriseList = Bizz.TempEnterprise;
                Bizz.TempSubEntrepeneur.Entrepeneur = entity;
                if (!Bizz.TempSubEntrepeneur.Active)
                {
                    Bizz.TempSubEntrepeneur.ToggleActive();
                }
                Bizz.TempSubEntrepeneur.Contact = new Contact();
                CreateIttLetter();
                CreateOffer();
                CreateRequest();

                //Code that ads a enterprise to Enterprise List
                bool tempResult = Bizz.CreateInDbReturnBool(Bizz.TempSubEntrepeneur);

                //Code, that checks result
                if (!result)
                {
                    result = tempResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that compares CraftGroups in LegalEntities and EnterpriseList
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool CheckCraftGroups(LegalEntity entity)
        {
            if (entity.CraftGroup1.Id != 0)
            {
                if (entity.CraftGroup1.Id == Bizz.TempEnterprise.CraftGroup1.Id || entity.CraftGroup1.Id == Bizz.TempEnterprise.CraftGroup2.Id || entity.CraftGroup1.Id == Bizz.TempEnterprise.CraftGroup3.Id || entity.CraftGroup1.Id == Bizz.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            if (entity.CraftGroup2.Id != 0)
            {
                if (entity.CraftGroup2.Id == Bizz.TempEnterprise.CraftGroup1.Id || entity.CraftGroup2.Id == Bizz.TempEnterprise.CraftGroup2.Id || entity.CraftGroup2.Id == Bizz.TempEnterprise.CraftGroup3.Id || entity.CraftGroup2.Id == Bizz.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            if (entity.CraftGroup3.Id != 0)
            {
                if (entity.CraftGroup3.Id == Bizz.TempEnterprise.CraftGroup1.Id || entity.CraftGroup3.Id == Bizz.TempEnterprise.CraftGroup2.Id || entity.CraftGroup3.Id == Bizz.TempEnterprise.CraftGroup3.Id || entity.CraftGroup3.Id == Bizz.TempEnterprise.CraftGroup4.Id)
                {
                    return true;
                }
            }
            if (entity.CraftGroup4.Id != 0)
            {
                if (entity.CraftGroup4.Id == Bizz.TempEnterprise.CraftGroup1.Id || entity.CraftGroup4.Id == Bizz.TempEnterprise.CraftGroup2.Id || entity.CraftGroup4.Id == Bizz.TempEnterprise.CraftGroup3.Id || entity.CraftGroup4.Id == Bizz.TempEnterprise.CraftGroup4.Id)
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
            Bizz.TempIttLetter = new IttLetter();
            int id = Bizz.CreateInDbReturnInt(Bizz.TempIttLetter);
            Bizz.TempIttLetter.SetId(id);
            Bizz.TempSubEntrepeneur.IttLetter = Bizz.TempIttLetter;
        }

        /// <summary>
        /// Method, that inserts an Offer to Db, and register it in Bizz.tempSubentrepeneur
        /// </summary>
        private void CreateOffer()
        {
            Bizz.TempOffer = new Offer();
            int id = Bizz.CreateInDbReturnInt(Bizz.TempOffer);
            Bizz.TempOffer.SetId(id);
            Bizz.TempSubEntrepeneur.Offer = Bizz.TempOffer;
        }

        /// <summary>
        /// Method, that inserts a Request to Db, and register it in Bizz.tempSubentrepeneur
        /// </summary>
        private void CreateRequest()
        {
            Bizz.TempRequest = new Request();
            int id = Bizz.CreateInDbReturnInt(Bizz.TempRequest);
            Bizz.TempRequest.SetId(id);
            Bizz.TempSubEntrepeneur.Request = Bizz.TempRequest;
        }

        /// <summary>
        /// Method, that filters existing Legal Entities in SubEntrepeneurs from list of indexable Legal Entities
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<IndexedLegalEntity> FilterIndexableLegalEntities(List<LegalEntity> list)
        {
            List<LegalEntity> tempResult = new List<LegalEntity>();
            List<IndexedLegalEntity> result = new List<IndexedLegalEntity>();
            LegalEntity tempEntity = new LegalEntity();
            foreach (LegalEntity entity in list)
            {
                if (entity.Region.Id == ComboBoxArea.SelectedIndex)
                {
                    tempResult.Add(entity);
                }
            }
            foreach (LegalEntity entity in tempResult)
            {
                if (entity.Region.Id != ComboBoxArea.SelectedIndex && entity.CountryWide.Equals(true))
                {
                    tempResult.Add(entity);
                }
            }
            int i = 0;
            foreach (LegalEntity temp in tempResult)
            {
                if (!IdExistsInSubEntrepeneurs(Bizz.TempEnterprise.Id, temp.Id))
                {
                    IndexedLegalEntity entity = new IndexedLegalEntity(i, temp);
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
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
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
            IndexableContacts = GetIndexableContacts();
            foreach (IndexedContact temp in IndexableContacts)
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
            IndexableEnterpriseList = GetIndexableEnterpriseList();
            foreach (Enterprise temp in IndexableEnterpriseList)
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
            if (IndexableContacts.Count == 0)
            {
                GetIndexableContacts();
            }
            foreach (IndexedContact tempContact in IndexableContacts)
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
        /// <returns>List<IndexableSubEntrepeneur></returns>
        private List<IndexedContact> GetIndexableContacts()
        {
            List<IndexedContact> result = new List<IndexedContact>();
            IndexedContact iContact = new IndexedContact(0, Bizz.Contacts[0]);
            result.Add(iContact);
            int i = 1;
            foreach (Contact contact in Bizz.Contacts)
            {
                if (contact.LegalEntity.Id == Bizz.TempLegalEntity.Id)
                {
                    IndexedContact temp = new IndexedContact(i, contact);
                    result.Add(temp);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that creates an indexable EnterpriseList
        /// </summary>
        /// <returns>List<IndexableEnterprise></returns>
        private List<Enterprise> GetIndexableEnterpriseList()
        {
            List<Enterprise> result = new List<Enterprise>();
            int i = 0;
            foreach (Enterprise enterprise in Bizz.EnterpriseList)
            {
                if (enterprise.Project.Id == Bizz.TempProject.Id)
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
        /// <returns>List<IndexableLegalEntity></returns>
        private List<IndexedLegalEntity> GetIndexableLegalEntities()
        {
            List<LegalEntity> tempResult = new List<LegalEntity>();
            List<IndexedLegalEntity> result = new List<IndexedLegalEntity>();
            IndexedLegalEntity temp = new IndexedLegalEntity(0, Bizz.LegalEntities[0]);
            result.Add(temp);
            int i = 1;
            foreach (LegalEntity entity in Bizz.LegalEntities)
            {
                if (CheckCraftGroups(entity))
                {
                    tempResult.Add(entity);
                    i++;
                }
            }
            result = FilterIndexableLegalEntities(tempResult);
            return result;
        }

        /// <summary>
        /// <summary>
        /// Method, that checks if a legal entity is already added to SubEntrepeneurs
        /// </summary>
        /// <param name="enterpriseId">int</param>
        /// <param name="entrepeneur">string</param>
        /// <returns>bool</returns>
        private bool IdExistsInSubEntrepeneurs(int enterpriseId, string entrepeneurId)
        {

            foreach (SubEntrepeneur temp in Bizz.SubEntrepeneurs)
            {
                if (temp.Entrepeneur.Id == entrepeneurId && temp.EnterpriseList.Id == enterpriseId)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
