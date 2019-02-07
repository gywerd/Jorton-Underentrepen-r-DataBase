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
    /// Interaction logic for UcIttLettersChooseReceivers.xaml
    /// </summary>
    public partial class UcIttLettersChooseReceivers : UserControl
    {
        #region Fields
        public Bizz CBZ = new Bizz();
        public bool result;
        public UserControl UcRight;
        public static string macAddress;

        public Shipping Shipping = new Shipping();
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<IndexedEntrepeneur> IndexedLegalEntities;
        public List<Shipping> ProjectShippingList = new List<Shipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        #region Constructors
        public UcIttLettersChooseReceivers(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            IndexedLegalEntities = cbz.IndexedEntrepeneurs;
            macAddress = GetMacAddress();
            this.UcRight = ucRight;
            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            bool receivers = CheckReceiversExist();

            switch (receivers)
            {
                case false:
                    //Show Confirmation
                    MessageBox.Show("Du har ikke valgt nogen modtagere. Der blev ikke føjet modtagere til modtagerlisten.", "Tilføj Modtager(e)", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case true:
                    AddReceivers();
                    break;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Modtager(e)n(ne) blev føjet til modtagerlisten.", "Tilføj Modtager(e)", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ComboBoxCaseId.SelectedIndex = -1;

                //Update lists and fields
                CBZ.RefreshList("Receivers");
                CBZ.RefreshList("ShippingList");
                Shipping = new Shipping();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Modtager(e)n(ne) blev ikke føjet til modtagerlisten. Prøv igen.", "Tilføj Modtager(e)", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonClearAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxEntrepeneurs.UnselectAll();
        }

        private void ButtonChoseAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxEntrepeneurs.SelectAll();
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke Vælg Modtagere?", "Luk Vælg Modtagere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                CBZ.UcRightActive = false;
                UcRight.Content = new UserControl();
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            if (selectedIndex >= 0)
            {
                foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
                {
                    if (temp.Index == selectedIndex)
                    {
                        CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                        break;
                    }
                }
                TextBoxName.Text = CBZ.TempProject.Name;
                GetProjectSubEntrepeneurs();
                CBZ.RefreshLetterLists();
                GetIndexedEntrepeneurs();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = IndexedLegalEntities;
            }
            else
            {
                TextBoxName.Text = "";
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterprises.Clear();
                IndexedLegalEntities.Clear();
                ListBoxEntrepeneurs.ItemsSource = "";
            }
        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItemsCount = ListBoxEntrepeneurs.SelectedItems.Count;
            switch (selectedItemsCount)
            {
                case 1:
                    int selectedIndex = ListBoxEntrepeneurs.SelectedIndex;
                    foreach (IndexedEntrepeneur temp in CBZ.IndexedEntrepeneurs)
                    {
                        if (temp.Index == selectedIndex)
                        {
                            CBZ.TempEntrepeneur = temp;
                            CBZ.TempSubEntrepeneur = GetSubEntrepeneur(CBZ.TempEntrepeneur.Id);
                            if (!CBZ.TempSubEntrepeneur.Active)
                            {
                                CBZ.TempSubEntrepeneur.ToggleActive();
                            }
                        }
                    }
                    break;
                default:
                    CBZ.TempLegalEntity = null;
                    CBZ.TempSubEntrepeneur = null;
                    break;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that adds multiple Receivers to Db
        /// </summary>
        /// <returns></returns>
        private void AddReceivers()
        {
            foreach (IndexedEntrepeneur entrepeneur in ListBoxEntrepeneurs.SelectedItems)
            {
                CBZ.TempReceiver = FillReceiver(CBZ.TempProject, entrepeneur);
                //Code that ads a enterprise to Enterprise List
                int tempResult = CBZ.CreateInDb(CBZ.TempReceiver);

                //Code, that checks result
                if (!result)
                {
                    if (tempResult < 1)
                    {
                        result = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method, that checks if Receivers have been selected in ListBoxEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private bool CheckReceiversExist()
        {
            bool result = false;

            if (ListBoxEntrepeneurs.SelectedItems.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that creates a Shipping
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="letterData">PdfData</param>
        private void CreateShipping(Project project, Receiver receiver, SubEntrepeneur subEntrepeneur)
        {
            Shipping = new Shipping(project, receiver, subEntrepeneur, new LetterData(), @"PDF_Documents\", macAddress);
            try
            {
                int id = CBZ.CreateInDb(Shipping);
                Shipping.SetId(id);
                Shipping.PdfPath = "";
                CBZ.UpdateInDb(Shipping);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Databasen returnerede en fejl. Forsendelsen blev ikke opdateret.\n" + ex, "Opdater forsendelse", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method, that adds data to an ITT-letter receiver
        /// </summary>
        /// <param name="entrepeneur">IndexedLegalEntity</param>
        /// <returns>Receiver</returns>
        private Receiver FillReceiver(Project project, IndexedEntrepeneur entrepeneur)
        {
            string cvr = entrepeneur.Entity.Cvr;
            string companyName = entrepeneur.Entity.Name;
            Contact contact = GetContact(entrepeneur.Id);
            string attention = contact.Person.Name;
            Address address = entrepeneur.Entity.Address;
            string street = address.Street;
            string place = address.Place;
            ZipTown zipTown = address.ZipTown;
            string zip = zipTown.ToString();
            string email = contact.Person.ContactInfo.Email;

            Receiver result = new Receiver(cvr, companyName, attention, street, zip, email, place);
            CreateShipping(project, result, CBZ.TempSubEntrepeneur);
            ProjectShippingList.Add(Shipping);

            return result;

        }

 
        /// <summary>
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that returns a Contact
        /// </summary>
        /// <param name="entrepeneur">string</param>
        /// <returns>Contact</returns>
        private Contact GetContact(int entrepeneur)
        {
            SubEntrepeneur sub = GetSubEntrepeneur(entrepeneur);
            Contact contact = sub.Contact;
            return contact;
        }

        /// <summary>
        /// Method, that creates an indexable Enterprises List
        /// </summary>
        /// <returns>List<IndexedEnterprise></returns>
        private List<IndexedEnterprise> GetIndexedEnterprises()
        {
            List<IndexedEnterprise> result = new List<IndexedEnterprise>();
            int i = 0;
            foreach (Enterprise enterprise in ProjectEnterprises)
            {
                IndexedEnterprise temp = new IndexedEnterprise(i, enterprise);
                result.Add(temp);
                i++;
            }
            return result;
        }

        /// <summary>
        /// Method that creates a list of indexable Legal Entities
        /// </summary>
        /// <returns>List<IndexedLegalEntity></returns>
        private void GetIndexedEntrepeneurs()
        {
            CBZ.GetIndexedEntrepeneurs();
            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = IndexedLegalEntities;
        }

        private string GetMacAddress()
        {
            return CBZ.GetMacAddress();
        }

        /// <summary>
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private void GetProjectSubEntrepeneurs()
        {
            ProjectEnterprises.Clear();
            ProjectSubEntrepeneurs.Clear();
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectEnterprises.Add(enterprise);
                    foreach (SubEntrepeneur sub in CBZ.SubEntrepeneurs)
                    {
                        if (sub.Enterprise.Id == enterprise.Id)
                        {
                            ProjectSubEntrepeneurs.Add(sub);
                        }
                    }
                }
            }
        }

        private SubEntrepeneur GetSubEntrepeneur(int entrepeneur)
        {
            SubEntrepeneur tempSub = new SubEntrepeneur();
            foreach (SubEntrepeneur sub in ProjectSubEntrepeneurs)
            {
                if (sub.Entrepeneur.Id == entrepeneur)
                {
                    tempSub = sub;
                }
            }
            return tempSub;
        }

        #endregion

    }
}
