using JudBizz;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for UcRequests.xaml
    /// </summary>
    public partial class UcRequests : UserControl
    {
        #region Fields
        public Bizz CBZ = new Bizz();
        public bool result;
        public UserControl UcMain;
        public static string macAddress;
        public PdfCreator PdfCreator;

        public IttLetterShipping IttLetterShipping = new IttLetterShipping();
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<IttLetterShipping> ProjectShippingList = new List<IttLetterShipping>();
        public List<Receiver> TempReceivers = new List<Receiver>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        #region Constructors
        public UcRequests(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            PdfCreator = new PdfCreator(CBZ.StrConnection);
            CBZ.TempProject = new Project();
            CBZ.TempRequestShipping = new RequestShipping();
            RefreshIndexedSubEntrepeneurs();
            macAddress = CBZ.MacAddress;
            this.UcMain = ucMain;
            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
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
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du lukke Vælg Modtagere?", "Luk Vælg Modtagere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            bool receivers = CheckReceiversExist();

            switch (receivers)
            {
                case false:
                    //Show Confirmation
                    MessageBox.Show("Du har ikke valgt nogen modtagere. Der blev ikke føjet modtagere til modtagerlisten.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case true:
                    RefreshReceivers();
                    result = true;
                    break;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Modtageren/-erne blev føjet til modtagerlisten.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);

                //Code to Send Requests
                try
                {
                    //Make som code, that sends emails
                    foreach (object item in ListBoxEntrepeneurs.SelectedItems)
                    {
                        IndexedSubEntrepeneur subEntrepeneur = new IndexedSubEntrepeneur((IndexedSubEntrepeneur)item);
                        CBZ.TempRequestShipping.SubEntrepeneur = subEntrepeneur;
                        CBZ.TempRequestShipping.RequestPdfPath = PdfCreator.GenerateRequestPdf(CBZ, CBZ.TempRequestShipping);
                        string[] fileNames = new string[] { CBZ.TempRequestShipping.RequestPdfPath };
                        Email email = new Email(CBZ, "PRØVE: Forespørgsel om underentreprise på " + CBZ.TempProject.Details.Name, subEntrepeneur.Contact.Person.ContactInfo.Email, CBZ.CurrentUser.Person.ContactInfo.Email, "Dette er en prøve", fileNames);
                        subEntrepeneur.Request.Status = new RequestStatus((RequestStatus)CBZ.GetRequestStatus(1));
                        subEntrepeneur.Request.SentDate = DateTime.Now;
                        CBZ.CreateInDb(CBZ.TempRequestShipping);
                        CBZ.UpdateInDb(subEntrepeneur.Request);
                        CBZ.UpdateInDb(subEntrepeneur);
                    }
                    MessageBox.Show("Forespørgslen/-erne blev sendt.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Reset Boxes
                    ComboBoxCaseId.SelectedIndex = -1;
                    ListBoxEntrepeneurs.SelectedIndex = -1;
                    ListBoxEntrepeneurs.ItemsSource = "";
                    TextBoxName.Text = "";
                    TextBoxProjectDescription.Text = "";
                    TextBoxPeriod.Text = "";
                    TextBoxAnswerDate.Text = "";
                    CBZ.RefreshList("SubEntrepeneurs");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Forespørgslen/-erne blev ikke sendt.\n" + ex.ToString(), "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            else
            {
                //Show error
                MessageBox.Show("Modtageren/-erne blev ikke føjet til modtagerlisten. Prøv igen.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        #endregion

        #region Events
        private void CheckBoxShowSent_ToggleChecked(object sender, RoutedEventArgs e)
        {
            CBZ.TempRequestShipping = new RequestShipping();
            RefreshIndexedSubEntrepeneurs();
            ListBoxEntrepeneurs.SelectedIndex = -1;
            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
            TextBoxAnswerDate.Text = "";
            TextBoxPeriod.Text = "";
            TextBoxProjectDescription.Text = "";

        }

        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);
                TextBoxName.Text = CBZ.TempProject.Details.Name;
                CBZ.TempRequestShipping = new RequestShipping();
                RefreshIndexedSubEntrepeneurs();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
                ListBoxEntrepeneurs.SelectedIndex = -1;
            }
            else
            {
                TextBoxName.Text = "";
                CBZ.TempProject = new Project();
                CBZ.TempRequestShipping = new RequestShipping();
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterprises.Clear();
                CBZ.IndexedSubEntrepeneurs.Clear();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.SelectedIndex = -1;
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

        private void TextBoxProjectDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CBZ.TempRequestShipping.SubEntrepeneur.Enterprise.Project.Details.Description != TextBoxProjectDescription.Text)
            {
                CBZ.TempRequestShipping.SubEntrepeneur.Enterprise.Project.Details.Description = TextBoxProjectDescription.Text;
            }
        }

        private void TextBoxPeriod_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempRequestShipping.SubEntrepeneur.Enterprise.Project.Details.Period = TextBoxPeriod.Text;
        }

        private void TextBoxAnswerDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempRequestShipping.SubEntrepeneur.Enterprise.Project.Details.AnswerDate = TextBoxAnswerDate.Text;
        }

        #endregion

        #region Methods
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
        /// Method, that checks, wether a request has already been sent to SubEntrepeneur
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <returns>bool</returns>
        private bool CheckRequestSent(SubEntrepeneur subEntrepeneur)
        {
            bool result = false;

            if (subEntrepeneur.Request.Status.Id >= 1)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that creates a Shipping
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        private void CreateShipping(SubEntrepeneur subEntrepeneur, Receiver receiver)
        {
            IttLetterShipping = new IttLetterShipping(subEntrepeneur, receiver, new LetterData(), @"PDF_Documents\", macAddress);
            try
            {
                int id = CBZ.CreateInDb(IttLetterShipping);
                IttLetterShipping.SetId(id);
                IttLetterShipping.PersonalPdfPath = @"PDF_Documents\";
                CBZ.UpdateInDb(IttLetterShipping);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Databasen returnerede en fejl. Forsendelsen blev ikke opdateret.\n" + ex, "Opdater forsendelse", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        /// Method, that generates a receiver from Indexed SubEntrepeneur
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        private Receiver GenerateReceiver(object selectedItem)
        {
            Receiver result = new Receiver();

            IndexedSubEntrepeneur subEntrepeneur = new IndexedSubEntrepeneur((IndexedSubEntrepeneur)selectedItem);
            result.Cvr = subEntrepeneur.Entrepeneur.Entity.Cvr;
            result.Name = subEntrepeneur.Entrepeneur.Entity.Name;
            result.Attention = subEntrepeneur.Contact.Person.Name;
            result.Street = subEntrepeneur.Entrepeneur.Entity.Address.Street;
            result.Place = subEntrepeneur.Entrepeneur.Entity.Address.Place;
            result.ZipTown = subEntrepeneur.Entrepeneur.Entity.Address.ZipTown.ToString();
            result.Email = subEntrepeneur.Contact.Person.ContactInfo.Email;

            return result;
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
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private void GetProjectSubEntrepeneurs()
        {
            ProjectEnterprises.Clear();
            ProjectSubEntrepeneurs.Clear();
            CBZ.RefreshList("Enterprises");
            CBZ.RefreshList("SubEntrepeneurs");
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    foreach (SubEntrepeneur sub in CBZ.SubEntrepeneurs)
                    {
                        if (sub.Enterprise.Id == enterprise.Id)
                        {
                            ProjectSubEntrepeneurs.Add(sub);
                        }
                    }

                    ProjectEnterprises.Add(enterprise);
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

        /// <summary>
        /// Method that creates a list of indexable Legal Entities
        /// </summary>
        private void RefreshIndexedSubEntrepeneurs()
        {
            GetProjectSubEntrepeneurs();
            CBZ.IndexedSubEntrepeneurs.Clear();

            int i = 0;

            foreach (SubEntrepeneur subEntrepeneur in ProjectSubEntrepeneurs)
            {
                bool requestSent = false;

                if (CheckBoxShowSent.IsChecked == false)
                {
                    requestSent = CheckRequestSent(subEntrepeneur);
                }

                if (!requestSent)
                {
                    CBZ.IndexedSubEntrepeneurs.Add(new IndexedSubEntrepeneur(i, subEntrepeneur));
                    i++;
                }
            }

            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
        }

        /// <summary>
        /// Method, that refreshed the Receivers list
        /// </summary>
        private void RefreshReceivers()
        {
            TempReceivers.Clear();

            switch (ListBoxEntrepeneurs.SelectedItems.Count)
            {
                case 0:
                    MessageBox.Show("Forespørgslen kan ikke sendes, da du ikke har valgt nogen modtagere.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case 1:
                    TempReceivers.Add(GenerateReceiver(ListBoxEntrepeneurs.SelectedItem));
                    break;
                default:
                    foreach (object subEntrepeneur in ListBoxEntrepeneurs.SelectedItems)
                    {
                        TempReceivers.Add(GenerateReceiver(subEntrepeneur));
                    }
                    break;
            }

        }

        #endregion

    }


}
