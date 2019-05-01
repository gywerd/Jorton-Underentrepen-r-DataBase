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
        public UserControl UcMain;
        public static string macAddress;

        public IttLetterShipping Shipping = new IttLetterShipping();
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<IndexedEntrepeneur> IndexedEntrepeneurs;
        public List<IttLetterShipping> ProjectShippingList = new List<IttLetterShipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        #region Constructors
        public UcIttLettersChooseReceivers(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            macAddress = CBZ.MacAddress;
            this.UcMain = ucMain;
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
                    MessageBox.Show("Du har ikke valgt nogen modtagere. Der blev ikke føjet modtagere til modtagerlisten.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case true:
                    AddReceivers();
                    break;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Modtager(e)n(ne) blev føjet til modtagerlisten.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ComboBoxCaseId.SelectedIndex = -1;

                //Update lists and fields
                CBZ.RefreshList("Receivers");
                CBZ.RefreshList("ShippingList");
                Shipping = new IttLetterShipping();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Modtager(e)n(ne) blev ikke føjet til modtagerlisten. Prøv igen.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Vælg Modtagere?", "Udbudsbreve", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            if (selectedIndex >= 0)
            {
                CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

                TextBoxName.Text = CBZ.TempProject.Details.Name;
                GetProjectSubEntrepeneurs();
                CBZ.RefreshProjectList("All", CBZ.TempProject.Id);
                GetIndexedEntrepeneurs();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = IndexedEntrepeneurs;
            }
            else
            {
                TextBoxName.Text = "";
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterprises.Clear();
                IndexedEntrepeneurs.Clear();
                ListBoxEntrepeneurs.ItemsSource = "";
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
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

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
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
            Shipping = new IttLetterShipping(subEntrepeneur, receiver, new LetterData(), @"PDF_Documents\", macAddress);
            try
            {
                int id = CBZ.CreateInDb(Shipping);
                Shipping.SetId(id);
                Shipping.PersonalPdfPath = "";
                CBZ.UpdateInDb(Shipping);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Databasen returnerede en fejl. Forsendelsen blev ikke opdateret.\n" + ex, "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
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
            CBZ.RefreshIndexedList("ActiveEntrepeneursFromProjectSubEntrepeneurs");
            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = IndexedEntrepeneurs;
        }

        /// <summary>
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private void GetProjectSubEntrepeneurs()
        {

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
