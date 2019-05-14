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

        public Shipping Shipping = new Shipping();
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<Shipping> ProjectShippingList = new List<Shipping>();
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
            CBZ.TempShipping = new Shipping();
            RefreshIndexedSubEntrepeneurs();
            macAddress = CBZ.MacAddress;
            this.UcMain = ucMain;
            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonClearAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxSubEntrepeneurs.UnselectAll();
        }

        private void ButtonChoseAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxSubEntrepeneurs.SelectAll();
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
            bool details = Convert.ToBoolean(CheckBoxDetails.IsChecked);

            if (details)
            {
                switch (receivers)
                {
                    case false:
                        //Show Confirmation
                        MessageBox.Show("Du har ikke valgt nogen modtagere. Der blev ikke føjet modtagere til modtagerlisten.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case true:
                        result = true;
                        break;
                }

                if (result)
                {
                    //Code to Send Requests
                    try
                    {
                        //Make som code, that sends emails
                        foreach (object item in ListBoxSubEntrepeneurs.SelectedItems)
                        {
                            IndexedSubEntrepeneur subEntrepeneur = new IndexedSubEntrepeneur((IndexedSubEntrepeneur)item);
                            CBZ.TempShipping.SubEntrepeneur = subEntrepeneur;
                            CBZ.TempShipping.Receiver = new Receiver(CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Cvr, CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Name, @"Att. " + CBZ.TempShipping.SubEntrepeneur.Contact.Person.Name, CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Address.Street, CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Address.ZipTown.ToString(), CBZ.TempShipping.SubEntrepeneur.Contact.Person.ContactInfo.Email, CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Address.Place);
                            CBZ.TempShipping.RequestPdfPath = PdfCreator.GenerateRequestPdf(CBZ, CBZ.TempShipping);
                            string[] fileNames = new string[] { CBZ.TempShipping.RequestPdfPath };
                            Email email = new Email(CBZ, "Forespørgsel om underentreprise på " + CBZ.TempShipping.Receiver.Name, CBZ.TempShipping.Receiver.Email, CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email, "Dette er en prøve", fileNames);
                            CBZ.TempShipping.SubEntrepeneur.Request.Status = new RequestStatus((RequestStatus)CBZ.GetRequestStatus(1));
                            CBZ.TempShipping.SubEntrepeneur.Request.SentDate = DateTime.Now;
                            CBZ.CreateInDb(CBZ.TempShipping);
                            //CBZ.UpdateInDb(subEntrepeneur.Request);
                            //CBZ.UpdateInDb(subEntrepeneur);
                        }
                        MessageBox.Show("Forespørgslen/-erne blev sendt.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);

                        //Reset Boxes
                        ComboBoxCaseId.SelectedIndex = -1;
                        ListBoxSubEntrepeneurs.SelectedIndex = -1;
                        ListBoxSubEntrepeneurs.ItemsSource = "";
                        TextBoxName.Text = "";
                        CheckBoxDetails.IsChecked = false;
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
            else
            {
                //Show error
                MessageBox.Show("Projektet mangler detaljer. Tilføj disse under 'Rediger Projekt' og prøv igen.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        #endregion

        #region Events
        private void CheckBoxShowSent_ToggleChecked(object sender, RoutedEventArgs e)
        {
            CBZ.TempShipping = new Shipping();
            RefreshIndexedSubEntrepeneurs();
            ListBoxSubEntrepeneurs.SelectedIndex = -1;
            ListBoxSubEntrepeneurs.ItemsSource = "";
            ListBoxSubEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
            SetCheckboxes();

        }

        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);
                TextBoxName.Text = CBZ.TempProject.Details.Name;
                CBZ.TempShipping = new Shipping();
                RefreshIndexedSubEntrepeneurs();
                ListBoxSubEntrepeneurs.ItemsSource = "";
                ListBoxSubEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
                ListBoxSubEntrepeneurs.SelectedIndex = -1;
                SetCheckboxes();

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
            else
            {
                TextBoxName.Text = "";
                CBZ.TempProject = new Project();
                CBZ.TempShipping = new Shipping();
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterprises.Clear();
                CBZ.IndexedSubEntrepeneurs.Clear();
                ListBoxSubEntrepeneurs.ItemsSource = "";
                ListBoxSubEntrepeneurs.SelectedIndex = -1;
                CheckBoxDetails.IsChecked = false;

                //Set CBZ.UcMainEdited
                if (CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = false;
                }

            }

        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItemsCount = ListBoxSubEntrepeneurs.SelectedItems.Count;
            switch (selectedItemsCount)
            {
                case 1:
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur((IndexedSubEntrepeneur)ListBoxSubEntrepeneurs.SelectedItem);
                    if (!CBZ.TempSubEntrepeneur.Active)
                    {
                        CBZ.TempSubEntrepeneur.ToggleActive();
                    }
                    break;
                default:
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
        /// Method, that checks if Receivers have been selected in ListBoxEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private bool CheckReceiversExist()
        {
            bool result = false;

            if (ListBoxSubEntrepeneurs.SelectedItems.Count >= 1)
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
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that generates a receiver from Indexed SubEntrepeneur
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns>Receiver</returns>
        private Receiver GenerateReceiver(SubEntrepeneur subEntrepeneur)
        {
            Receiver result = new Receiver();

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
        /// Method that creates a list of Indexed SubEntrepeneurs
        /// </summary>
        private void RefreshIndexedSubEntrepeneurs()
        {
            CBZ.RefreshProjectList("SubEntrepeneurs", CBZ.TempProject.Id);
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

            ListBoxSubEntrepeneurs.ItemsSource = "";
            ListBoxSubEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
        }

        /// <summary>
        /// Method, that refreshed the Receivers list
        /// </summary>
        private void RefreshReceivers()
        {
            TempReceivers.Clear();

            switch (ListBoxSubEntrepeneurs.SelectedItems.Count)
            {
                case 0:
                    MessageBox.Show("Forespørgslen kan ikke sendes, da du ikke har valgt nogen modtagere.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case 1:
                    TempReceivers.Add(GenerateReceiver(new SubEntrepeneur((IndexedSubEntrepeneur)ListBoxSubEntrepeneurs.SelectedItem)));
                    break;
                default:
                    foreach (object obj in ListBoxSubEntrepeneurs.SelectedItems)
                    {
                        TempReceivers.Add(GenerateReceiver(new SubEntrepeneur((IndexedSubEntrepeneur)obj)));
                    }
                    break;
            }

        }

        /// <summary>
        /// Method, that sets IsChecked for CheckBoxes
        /// </summary>
        private void SetCheckboxes()
        {
            if (CBZ.TempProject.Details.Description.Length >= 1 && CBZ.TempProject.Details.Period.Length >= 1 && CBZ.TempProject.Details.AnswerDate.Length >= 1)
            {
                CheckBoxDetails.IsChecked = true;
            }
            else
            {
                CheckBoxDetails.IsChecked = false;
            }

        }

        #endregion

    }


}
