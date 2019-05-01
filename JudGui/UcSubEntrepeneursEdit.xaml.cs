using JudBizz;
using JudRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UcSubEntrepeneursEdit.xaml
    /// </summary>
    public partial class UcSubEntrepeneursEdit : UserControl
    {
        #region Fields
        public DateTime Date = new DateTime();
        public bool ChangedRequestStatus = false;
        public bool ChangedIttLetterSent = false;
        public bool ChangedOfferReceived = false;
        public bool requestStatusUpTodate = false;
        public bool ittLetterSentUptoDate = false;
        public bool offerReceivedUpToDate = false;
        public bool result = false;
        public bool DbStatus = false;
        public bool OverrideControl = false;

        public Bizz CBZ;
        public UserControl UcMain;

        List<IndexedEnterprise> ProjectEnterprises = new List<IndexedEnterprise>();

        #endregion

        #region Constructors
        public UcSubEntrepeneursEdit(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
            ComboBoxRequest.ItemsSource = CBZ.IndexedRequestStatuses;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke redigering af Underentrepenører?", "Underentrepenører", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxSubEntrepeneurs.SelectedItems.Count > 0)
            {
                //Assume, that Request Status, IttLetter sent & Offer Received is up to date, 
                //while TempSupEntrepeneur is not yet updated
                requestStatusUpTodate = true;
                ittLetterSentUptoDate = true;
                offerReceivedUpToDate = true;
                result = false;

                //Validate, requestStatus that Request Status is up to date
                if (ChangedRequestStatus)
                {
                    requestStatusUpTodate = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur.Request);
                }

                //Validate, wether IttLetter sent is up to date
                if (ChangedIttLetterSent)
                {
                    ittLetterSentUptoDate = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur.IttLetter);
                }

                //Validate, wether Offer received is up to date
                if (ChangedOfferReceived)
                {
                    offerReceivedUpToDate = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur.Offer);
                }

                //Update SubEntrepeneur
                if (requestStatusUpTodate && ittLetterSentUptoDate && offerReceivedUpToDate)
                {
                    result = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur);
                }

                //Display result in messagebox
                if (result)
                {
                    MessageBox.Show("Underentrepenøren blev opdateret.", "Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Information);

                    ComboBoxCaseId.SelectedIndex = -1;
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                    ComboBoxEnterprise.SelectedIndex = -1;
                    ComboBoxEnterprise.ItemsSource = "";
                    ListBoxSubEntrepeneurs.SelectedIndex = -1;
                    ListBoxSubEntrepeneurs.ItemsSource = "";
                    ResetComboBoxes();
                    ResetRadioButtons();

                    CBZ.UcMainEdited = false;
                }
                else
                {
                    MessageBox.Show("Der opstod en fejl under opdatering af Underentrepenøren. Prøv Igen.", "Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Du har ikke valgt en Underentrepenøren at opdatere.", "Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex > -1)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);

                TextBoxName.Text = CBZ.TempProject.Details.Name;
                ComboBoxEnterprise.ItemsSource = "";
                GetProjectEnterprises();
                ComboBoxEnterprise.ItemsSource = ProjectEnterprises;
                ComboBoxEnterprise.SelectedIndex = 0;
                ListBoxSubEntrepeneurs.ItemsSource = "";
                ListBoxSubEntrepeneurs.SelectedIndex = -1;
                CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                TextBoxEntrepeneur.Text = "";
                TextBoxOfferPrice.Text = "";
                ResetComboBoxes();
                ResetRadioButtons();

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
                if (CBZ.TempSubEntrepeneur.Contact.Id != new Contact((Contact)ComboBoxContact.SelectedItem).Id)
                {
                    CBZ.TempSubEntrepeneur.Contact = ((Contact)ComboBoxContact.SelectedItem);
                }

                //Set CBZ.UcMainEdited
                if (CBZ != null)
                {
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }

            }

        }

        private void ComboBoxEnterprise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxEntrepeneur.Text = "";
            TextBoxOfferPrice.Text = "";
            ResetComboBoxes();
            ResetRadioButtons();
            ListBoxSubEntrepeneurs.UnselectAll();
            ListBoxSubEntrepeneurs.ItemsSource = "";
            if (ComboBoxEnterprise.SelectedIndex == -1 || ComboBoxEnterprise.SelectedIndex == 0)
            {
                CBZ.TempEnterprise = new Enterprise((Enterprise)ComboBoxEnterprise.SelectedItem);
                ListBoxSubEntrepeneurs.ItemsSource = "";

            }
            else if (ComboBoxEnterprise.SelectedIndex >= 1)
            {
                CBZ.TempEnterprise = new Enterprise((Enterprise)ComboBoxEnterprise.SelectedItem);
                if (CBZ.TempEnterprise.Id != 0)
                {
                    GetIndexedSubEntrepeneurs();
                    ListBoxSubEntrepeneurs.ItemsSource = "";
                    ListBoxSubEntrepeneurs.ItemsSource = CBZ.IndexedSubEntrepeneurs;
                }

            }
        }

        private void ComboBoxRequest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxRequest.SelectedIndex > -1)
            {
                if (CBZ.TempSubEntrepeneur != new SubEntrepeneur())
                {
                    if (CBZ.TempSubEntrepeneur.Request.Status != new RequestStatus((IndexedRequestStatus)ComboBoxRequest.SelectedItem))
                    {
                        RequestStatus requestStatus = new RequestStatus((IndexedRequestStatus)ComboBoxRequest.SelectedItem);

                        //Validate Dates
                        switch (requestStatus.Id)
                        {
                            case 0:
                                ValidateRequestDatesNotSent();
                                break;
                            case 1:
                                ValidateRequestDatesSent();
                                break;
                            case 2:
                                ValidateRequestDatesReceived();
                                break;
                            case 3:
                                ValidateRequestDatesCancelled();
                                break;
                            default:
                                ValidateRequestDatesNotSent();
                                break;
                        }

                        //Update CBZ.TempSubEntrepeneur Request Status
                        CBZ.TempSubEntrepeneur.Request.Status = requestStatus;
                        ChangedRequestStatus = true;
                    }
                }

                //Set CBZ.UcMainEdited
                if (CBZ != null)
                {
                    if (!CBZ.UcMainEdited && CheckTempSubEntrepeneur())
                    {
                        CBZ.UcMainEdited = true;
                    }
                }

            }
        }

        private void DateIttLetter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Validating IttLetter Sent Date when written/selected corrupts the code
            //Instead, validate date when changing status
        }

        private void DateOffer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Validating Offer Received Date when written/selected corrupts the code
            //Instead, validate Offer Received Date when changing status
        }

        private void DateRequestReceived_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Validating Request Received Date when written/selected corrupts the code
            //Instead, validate Request Received Date when changing status
        }

        private void DateRequestSent_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Validating Request Sent Date when written/selected corrupts the code
            //Instead, validate Sent Received Date when changing status
        }

        private void ListBoxSubEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxSubEntrepeneurs.SelectedIndex > -1)
            {
                CBZ.TempSubEntrepeneur = new SubEntrepeneur((IndexedSubEntrepeneur)ListBoxSubEntrepeneurs.SelectedItem);
                TextBoxEntrepeneur.Text = CBZ.TempSubEntrepeneur.Entrepeneur.Entity.Name;
                SetComboBoxes();
                SetRadioButtons();


                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
            else
            {
                CBZ.TempSubEntrepeneur = new SubEntrepeneur();
            }

        }

        private void RadioButtonAgreementConcludedYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (!CBZ.TempSubEntrepeneur.AgreementConcluded)
                {
                    CBZ.TempSubEntrepeneur.ToggleAgreementConcluded();
                }
                RadioButtonOfferReceivedYes.IsChecked = true;
                RadioButtonOfferReceivedNo.IsChecked = false;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited && CheckTempSubEntrepeneur())
                {
                    CBZ.UcMainEdited = true;
                }

            }
            else
            {
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedYes.IsChecked = false;
            }

        }

        private void RadioButtonAgreementConcludedNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (CBZ.TempSubEntrepeneur.AgreementConcluded)
                {
                    CBZ.TempSubEntrepeneur.ToggleAgreementConcluded();
                }
                RadioButtonAgreementConcludedYes.IsChecked = false;
                RadioButtonAgreementConcludedNo.IsChecked = true;

            }
            else
            {
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedYes.IsChecked = false;
            }

        }

        private void RadioButtonIttLetterSentYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (!CBZ.TempSubEntrepeneur.IttLetter.Sent)
                {
                    ValidateIttLetterSentDateSent();
                    CBZ.TempSubEntrepeneur.IttLetter.ToggleSent();
                    ChangedIttLetterSent = true;
                }
                RadioButtonIttLetterSentYes.IsChecked = true;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonIttLetterSentNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (CBZ.TempSubEntrepeneur.IttLetter.Sent)
                {
                    ValidateIttLetterSentDateNotSent();
                    CBZ.TempSubEntrepeneur.IttLetter.ToggleSent();
                    ChangedIttLetterSent = true;
                }
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = true;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonOfferChosenYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (!CBZ.TempSubEntrepeneur.Offer.Chosen)
                {
                    CBZ.TempSubEntrepeneur.Offer.ToggleChosen();
                }
                RadioButtonOfferChosenYes.IsChecked = true;
                RadioButtonOfferChosenNo.IsChecked = false;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonOfferChosenNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (CBZ.TempSubEntrepeneur.Offer.Chosen)
                {
                    CBZ.TempSubEntrepeneur.Offer.ToggleChosen();
                }
                RadioButtonOfferChosenYes.IsChecked = false;
                RadioButtonOfferChosenNo.IsChecked = true;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

            if (CBZ != null)
            {
                if (!CBZ.UcMainEdited && CheckTempSubEntrepeneur())
                {
                    CBZ.UcMainEdited = true;
                }
            }

        }

        private void RadioButtonOfferReceivedYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (!CBZ.TempSubEntrepeneur.Offer.Received)
                {
                    ValidateOfferReceivedDateReceived();
                    CBZ.TempSubEntrepeneur.Offer.ToggleReceived();
                    ChangedOfferReceived = true;
                }
                RadioButtonOfferReceivedYes.IsChecked = true;
                RadioButtonOfferReceivedNo.IsChecked = false;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonOfferReceivedNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (CBZ.TempSubEntrepeneur.Offer.Received)
                {
                    ValidateOfferReceivedDateNotReceived();
                    CBZ.TempSubEntrepeneur.Offer.ToggleReceived();
                    ChangedOfferReceived = true;
                }
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedNo.IsChecked = true;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonReservationsYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                RadioButtonReservationsYes.IsChecked = true;
                RadioButtonReservationsNo.IsChecked = false;
                if (!CBZ.TempSubEntrepeneur.Reservations)
                {
                    CBZ.TempSubEntrepeneur.ToggleReservations();
                }
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonReservationsNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (CBZ.TempSubEntrepeneur.Reservations)
                {
                    CBZ.TempSubEntrepeneur.ToggleReservations();
                }
                RadioButtonReservationsYes.IsChecked = false;
                RadioButtonReservationsNo.IsChecked = true;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonUpholdYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (!CBZ.TempSubEntrepeneur.Uphold)
                {
                    CBZ.TempSubEntrepeneur.ToggleUphold();
                }
                RadioButtonUpholdYes.IsChecked = true;
                RadioButtonUpholdNo.IsChecked = false;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void RadioButtonUpholdNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (CBZ.TempSubEntrepeneur.Uphold)
                {
                    CBZ.TempSubEntrepeneur.ToggleUphold();
                }
                RadioButtonUpholdYes.IsChecked = false;
                RadioButtonUpholdNo.IsChecked = true;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }

        }

        private void TextBoxOfferPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                string temp = TextBoxOfferPrice.Text;
                if (temp != "0,00")
                {
                    temp = ParseOfferPrice(temp);
                    temp = ParseOfferPriceComma(temp);
                }
                TextBoxOfferPrice.Text = temp;

                if (CBZ.TempSubEntrepeneur == null)
                {
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                }

                CBZ.TempSubEntrepeneur.Offer.Price = Convert.ToDouble(TextBoxOfferPrice.Text);

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited && CheckTempSubEntrepeneur())
                {
                    CBZ.UcMainEdited = true;
                }
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that checks content of CBZ.TempSubEntrepeneur
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckTempSubEntrepeneur()
        {
            bool result = false;

            if (CBZ.TempSubEntrepeneur != new SubEntrepeneur())
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves the index for a Contact
        /// </summary>
        /// <returns>int</returns>
        private int GetContactIndex()
        {
            int result = -1;

            foreach (IndexedContact contact in CBZ.IndexedContacts)
            {
                if (contact.Id == CBZ.TempSubEntrepeneur.Contact.Id)
                {
                    result = contact.Index;
                }
            }

            return result;
        }

        /// <summary>
        /// Methods, creates a list of indexable Contacts
        /// </summary>
        private void GetIndexedContacts()
        {
            CBZ.IndexedContacts.Clear();
            CBZ.RefreshList("Contacts");
            CBZ.IndexedContacts.Add(new IndexedContact(0, CBZ.Contacts[0]));
            int i = 1;
            foreach (Contact contact in CBZ.Contacts)
            {
                if (contact.Entrepeneur.Id == CBZ.TempSubEntrepeneur.Entrepeneur.Id)
                {
                    CBZ.IndexedContacts.Add(new IndexedContact(i, contact));
                    i++;
                }
            }
        }

        /// <summary>
        /// <summary>
        /// Method that creates a list of indexable SubEbtrepeneurs
        /// </summary>
        private void GetIndexedSubEntrepeneurs()
        {
            CBZ.IndexedSubEntrepeneurs.Clear();
            int i = 0;
            CBZ.RefreshList("SubEntrepeneurs");
            foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
            {
                if (subEntrepeneur.Enterprise.Id == CBZ.TempEnterprise.Id)
                {
                    CBZ.IndexedSubEntrepeneurs.Add(new IndexedSubEntrepeneur(i, subEntrepeneur));
                    i++;
                }
            }
        }

        /// <summary>
        /// Methods, creates a list of indexable Enterprises
        /// </summary>
        private void GetProjectEnterprises()
        {
            ProjectEnterprises.Clear();

            CBZ.RefreshList("Enterprises");

            ProjectEnterprises.Add(new IndexedEnterprise(0, CBZ.Enterprises[0]));

            int i = 1;

            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectEnterprises.Add(new IndexedEnterprise(i, enterprise));
                    i++;
                }
            }

        }

        /// <summary>
        /// Method, that retrieves the index for a Request
        /// </summary>
        /// <returns>int</returns>
        private int GetRequestIndex()
        {
            int result = -1;

            foreach (IndexedRequest request in CBZ.IndexedRequests)
            {
                if (request.Id == CBZ.TempSubEntrepeneur.Request.Status.Id)
                {
                    result = request.Index;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that removes letters from string and adds to digit if necessary
        /// </summary>
        /// <param name="result">string</param>
        /// <returns>string</returns>
        private string ParseOfferPrice(string price)
        {
            string result = Regex.Replace(price, "[A-Za-z ]", "");
            result = Regex.Replace(result, "[.]", ",");
            return result;
        }

        /// <summary>
        /// Method, that checks comma from string and adds to digit if necessary
        /// </summary>
        /// <param name="result">string</param>
        /// <returns>string</returns>
        private string ParseOfferPriceComma(string price)
        {
            bool comma = false;
            string result = price;
            int i = 0;
            int j = 0;
            foreach (char c in result)
            {
                string cc = c.ToString();
                if (cc == ",")
                {
                    comma = true;
                    j++;
                }
                if (j > 1)
                {
                    result = result.Substring(0, i);
                    result += "00";
                    break;
                }
                i++;
            }
            if (!comma)
            {
                result += ",00";
            }
            if (result == ",00")
            {
                result = "0,00";
            }
            return result;
        }

        /// <summary>
        /// Method, that's resets Combocoxes
        /// </summary>
        private void ResetComboBoxes()
        {
            ComboBoxContact.SelectedIndex = -1;
            ComboBoxContact.ItemsSource = null;
            CBZ.TempContact = new Contact();
            ComboBoxRequest.SelectedIndex = -1;
            DateRequestSent.DisplayDate = DateTime.Now;
            DateRequestSent.Text = "";
            DateRequestReceived.DisplayDate = DateTime.Now;
            DateRequestReceived.Text = "";
        }

        /// <summary>
        /// Method, that reset's RadioButtons
        /// </summary>
        private void ResetRadioButtons()
        {
            ResetRadioButtonsIttLetterSent();
            ResetRadioButtonsOfferReceived();
            ResetRadioButtonsReservations();
            ResetRadioButtonsUphold();
            ResetRadioButtonsOfferChosen();
            ResetRadioButtonsAgreementConcluded();
        }

        /// <summary>
        /// Method, that resets RadioButtonAgreementConcludedYes and RadioButtonAgreementConcludedNo
        /// </summary>
        private void ResetRadioButtonsAgreementConcluded()
        {
            RadioButtonAgreementConcludedYes.IsChecked = false;
            RadioButtonAgreementConcludedNo.IsChecked = false;
        }

        /// <summary>
        /// Method, that resets RadioButtonIttLetterYes and RadioButtonIttLetterNo
        /// </summary>
        private void ResetRadioButtonsIttLetterSent()
        {
            RadioButtonIttLetterSentYes.IsChecked = false;
            RadioButtonIttLetterSentNo.IsChecked = false;
        }

        /// <summary>
        /// Method, that resets RadioButtonOfferChosenYes and RadioButtonOfferChosenNo
        /// </summary>
        private void ResetRadioButtonsOfferChosen()
        {
            RadioButtonOfferChosenYes.IsChecked = false;
            RadioButtonOfferChosenNo.IsChecked = false;
        }

        /// <summary>
        /// Method, that resets RadioButtonOfferReceivedYes and RadioButtonOfferReceivedNo
        /// </summary>
        private void ResetRadioButtonsOfferReceived()
        {
            RadioButtonOfferReceivedYes.IsChecked = false;
            RadioButtonOfferReceivedNo.IsChecked = false;
        }

        /// <summary>
        /// Method, that resets RadioButtonReservationsYes and RadioButtonReservationsNo
        /// </summary>
        private void ResetRadioButtonsReservations()
        {
            RadioButtonReservationsYes.IsChecked = false;
            RadioButtonReservationsNo.IsChecked = false;
        }

        /// <summary>
        /// Method, that sets RadioButtonUpholdYes and RadioButtonUpholdNo
        /// </summary>
        private void ResetRadioButtonsUphold()
        {
            RadioButtonUpholdYes.IsChecked = false;
            RadioButtonUpholdNo.IsChecked = false;
        }

        /// <summary>
        /// Method, that populates ComboBoxes
        /// </summary>
        private void SetComboBoxes()
        {
            GetIndexedContacts();
            ComboBoxContact.ItemsSource = "";
            ComboBoxContact.ItemsSource = CBZ.IndexedContacts;
            ComboBoxContact.SelectedIndex = GetContactIndex();
            DateRequestSent.DisplayDate = CBZ.TempSubEntrepeneur.Request.SentDate;
            DateRequestSent.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToShortDateString();
            DateRequestReceived.DisplayDate = CBZ.TempSubEntrepeneur.Request.ReceivedDate;
            DateRequestReceived.Text = CBZ.TempSubEntrepeneur.Request.ReceivedDate.ToShortDateString();
            ComboBoxRequest.SelectedIndex = CBZ.TempSubEntrepeneur.Request.Status.Id;
        }

        /// <summary>
        /// Method, that sets values for RadioButtons
        /// </summary>
        private void SetRadioButtons()
        {
            SetRadioButtonsIttLetterSent();
            SetRadioButtonsOfferReceived();
            SetRadioButtonsReservations();
            SetRadioButtonsUphold();
            SetRadioButtonsOfferChosen();
            SetRadioButtonsAgreementConcluded();
        }

        /// <summary>
        /// Method, that sets RadioButtonAgreementConcludedYes and RadioButtonAgreementConcludedNo
        /// </summary>
        private void SetRadioButtonsAgreementConcluded()
        {
            if (CBZ.TempSubEntrepeneur.AgreementConcluded)
            {
                RadioButtonAgreementConcludedYes.IsChecked = true;
                RadioButtonAgreementConcludedNo.IsChecked = false;
            }
            else
            {
                RadioButtonAgreementConcludedYes.IsChecked = false;
                RadioButtonAgreementConcludedNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonIttLetterYes and RadioButtonIttLetterNo
        /// </summary>
        private void SetRadioButtonsIttLetterSent()
        {
            if (CBZ.TempSubEntrepeneur.IttLetter.Sent)
            {
                RadioButtonIttLetterSentYes.IsChecked = true;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }
            else
            {
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonOfferChosenYes and RadioButtonOfferChosenNo
        /// </summary>
        private void SetRadioButtonsOfferChosen()
        {
            if (CBZ.TempSubEntrepeneur.Offer.Chosen)
            {
                RadioButtonOfferChosenYes.IsChecked = true;
                RadioButtonOfferChosenNo.IsChecked = false;
            }
            else
            {
                RadioButtonOfferChosenYes.IsChecked = false;
                RadioButtonOfferChosenNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonOfferReceivedYes and RadioButtonOfferReceivedNo
        /// </summary>
        private void SetRadioButtonsOfferReceived()
        {
            if (CBZ.TempSubEntrepeneur.Offer.Received)
            {
                RadioButtonOfferReceivedYes.IsChecked = true;
                RadioButtonOfferReceivedNo.IsChecked = false;
            }
            else
            {
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonReservationsYes and RadioButtonReservationsNo
        /// </summary>
        private void SetRadioButtonsReservations()
        {
            if (CBZ.TempSubEntrepeneur.Reservations)
            {
                RadioButtonReservationsYes.IsChecked = true;
                RadioButtonReservationsNo.IsChecked = false;
            }
            else
            {
                RadioButtonReservationsYes.IsChecked = false;
                RadioButtonReservationsNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonUpholdYes and RadioButtonUpholdNo
        /// </summary>
        private void SetRadioButtonsUphold()
        {
            if (CBZ.TempSubEntrepeneur.Uphold)
            {
                RadioButtonUpholdYes.IsChecked = true;
                RadioButtonUpholdNo.IsChecked = false;
            }
            else
            {
                RadioButtonUpholdYes.IsChecked = false;
                RadioButtonUpholdNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that resets IttLetter Sent Date, when status is 'False'
        /// Logic: As the IttLetter is not sent, the IttLetter Sent Date must be reset
        /// </summary>
        private void ValidateIttLetterSentDateNotSent()
        {
            if (DateIttLetter.DisplayDate.ToShortDateString() != CBZ.oldDate.ToShortDateString() || CBZ.TempSubEntrepeneur.IttLetter.SentDate.ToShortDateString() != CBZ.oldDate.ToShortDateString())
            {
                CBZ.TempSubEntrepeneur.IttLetter.SentDate = DateIttLetter.DisplayDate = CBZ.oldDate;
                DateIttLetter.Text = CBZ.TempSubEntrepeneur.IttLetter.SentDate.ToString("dd-MM-yyyy");
            }
        }

        /// <summary>
        /// Method, that validates, wether IttLetter Sent Date is less than ten years old, when status is 'True'
        /// Logic: Adjusting IttLetter Sent Date is pointless for very old cases
        /// </summary>
        private void ValidateIttLetterSentDateSent()
        {
            if (DateIttLetter.DisplayDate.Year < (DateTime.Now.Year - 10))
            {
                CBZ.TempSubEntrepeneur.IttLetter.SentDate = DateIttLetter.DisplayDate = DateTime.Now;
                DateIttLetter.Text = CBZ.TempSubEntrepeneur.IttLetter.SentDate.ToString("dd-MM-yyyy");
            }
            else
            {
                CBZ.TempSubEntrepeneur.IttLetter.SentDate = DateIttLetter.DisplayDate;
            }
        }

        /// <summary>
        /// Method, that resets Offer Received Date, when status is 'False'
        /// Logic: As the Offer is not received, the Offer Received Date must be reset
        /// </summary>
        private void ValidateOfferReceivedDateNotReceived()
        {
            if (DateOffer.DisplayDate.ToShortDateString() != CBZ.oldDate.ToShortDateString() || CBZ.TempSubEntrepeneur.Offer.ReceivedDate.ToShortDateString() != CBZ.oldDate.ToShortDateString())
            {
                CBZ.TempSubEntrepeneur.Offer.SetReceived(CBZ.oldDate);
                DateOffer.DisplayDate = CBZ.TempSubEntrepeneur.IttLetter.SentDate;
                DateOffer.Text = CBZ.TempSubEntrepeneur.IttLetter.SentDate.ToString("dd-MM-yyyy");
            }
        }

        /// <summary>
        /// Method, that validates, wether Offer Received Date is less than ten years old, when status is 'True'
        /// Logic: Adjusting Offer Received Date is pointless for very old cases
        /// </summary>
        private void ValidateOfferReceivedDateReceived()
        {
            if (DateIttLetter.DisplayDate.Year < (DateTime.Now.Year - 10))
            {
                CBZ.TempSubEntrepeneur.Offer.SetReceived(DateTime.Now);
                DateOffer.DisplayDate = CBZ.TempSubEntrepeneur.Offer.ReceivedDate;
                DateOffer.Text = CBZ.TempSubEntrepeneur.Offer.ReceivedDate.ToString("dd-MM-yyyy");
            }
            else
            {
                CBZ.TempSubEntrepeneur.Offer.SetReceived(DateIttLetter.DisplayDate);
            }
        }

        /// <summary>
        /// Method, that resets Request dates, when status is '3 Cancellation'
        /// </summary>
        private void ValidateRequestDatesCancelled()
        {
            ValidateRequestReceivedDateReceived();
            ValidateRequestSentDateWhenReceived();
        }

        /// <summary>
        /// Method, that resets Request dates, when status is '0 Not Sent'
        /// </summary>
        private void ValidateRequestDatesNotSent()
        {
            ValidateRequestSentDateNotSent();
            ValidateRequestReceivedDateNotReceived();
        }

        /// <summary>
        /// Method, that resets Request dates, when status is '2 Receiced'
        /// </summary>
        private void ValidateRequestDatesReceived()
        {
            ValidateRequestReceivedDateReceived();
            ValidateRequestSentDateWhenReceived();
        }

        /// <summary>
        /// Method, that validates Request dates, when status is '1 Sent'
        /// </summary>
        private void ValidateRequestDatesSent()
        {
            ValidateRequestSentDateSent();
            ValidateRequestReceivedDateNotReceived();
        }

        /// <summary>
        /// Method, that resets Request Received Date when status is '0 Not Sent' or '1 Sent'
        /// Logic: As the Request was sent recently or never sent, the Request Received Date must be reset, if no Request received
        /// </summary>
        private void ValidateRequestReceivedDateNotReceived()
        {
            CBZ.TempSubEntrepeneur.Request.ReceivedDate = DateRequestReceived.DisplayDate = CBZ.oldDate;
            DateRequestReceived.Text = CBZ.TempSubEntrepeneur.Request.ReceivedDate.ToString("dd-MM-yyy");
        }

        /// <summary>
        /// Method, that validates, wether Request Received Date is less than ten years old, when status is '2 Received' or '3 Cancellation'
        /// Logic: Adjusting Request Received Date is pointless for very old cases
        /// </summary>
        private void ValidateRequestReceivedDateReceived()
        {
            if (DateRequestReceived.DisplayDate.Year <= (DateTime.Now.Year - 10))
            {
                CBZ.TempSubEntrepeneur.Request.ReceivedDate = DateRequestReceived.DisplayDate = DateTime.Now;
                DateRequestReceived.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToString("dd-MM-yyy");
            }
            else
            {
                CBZ.TempSubEntrepeneur.Request.ReceivedDate = DateRequestReceived.DisplayDate;
            }
        }

        /// <summary>
        /// Method, that resets Request Sent Date, when status is '0 Not Sent'
        /// Logic: As the Request has not been sent, Request Sent Date must be reset
        /// </summary>
        private void ValidateRequestSentDateNotSent()
        {
            CBZ.TempSubEntrepeneur.Request.SentDate = DateRequestSent.DisplayDate = CBZ.oldDate;
            DateRequestSent.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToString("dd-MM-yyy");
        }

        /// <summary>
        /// Method, that validates Request Sent Date is less than ten years old, when status is '1 Sent'
        /// Logic: Adjusting Request Sent Date is pointless for very old cases
        /// </summary>
        private void ValidateRequestSentDateSent()
        {
            if (DateRequestSent.DisplayDate.Year <= (DateTime.Now.Year - 10))
            {
                CBZ.TempSubEntrepeneur.Request.SentDate = DateRequestSent.DisplayDate = DateTime.Now;
                DateRequestSent.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToString("dd-MM-yyy");
            }
            else
            {
                CBZ.TempSubEntrepeneur.Request.SentDate = DateRequestSent.DisplayDate;
            }
        }

        /// <summary>
        /// Method, that validates Request Sent Date, when status i '2 Received' or '3 Cancellation' - Used after ValidateRequestReceivedDateReceived().
        /// Logic: Request Sent Date cannot be newer, than Request Received Date
        /// </summary>
        private void ValidateRequestSentDateWhenReceived()
        {
            //Compare year
            if (CBZ.TempSubEntrepeneur.Request.SentDate.Year > DateRequestReceived.DisplayDate.Year)
            {
                CBZ.TempSubEntrepeneur.Request.SentDate = DateRequestSent.DisplayDate = DateRequestReceived.DisplayDate;
                DateRequestSent.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToString("dd-MM-yyy");
            }

            //Compare year and month
            else if (CBZ.TempSubEntrepeneur.Request.SentDate.Year == DateRequestReceived.DisplayDate.Year)
            {
                if (CBZ.TempSubEntrepeneur.Request.SentDate.Month > DateRequestReceived.DisplayDate.Month)
                {
                    CBZ.TempSubEntrepeneur.Request.SentDate = DateRequestSent.DisplayDate = DateRequestReceived.DisplayDate;
                    DateRequestSent.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToString("dd-MM-yyy");
                }
            }

            //compares full date
            else if (CBZ.TempSubEntrepeneur.Request.SentDate.Year == DateRequestReceived.DisplayDate.Year)
            {
                if (CBZ.TempSubEntrepeneur.Request.SentDate.Month == DateRequestReceived.DisplayDate.Month)
                {
                    if (CBZ.TempSubEntrepeneur.Request.SentDate.Day > DateRequestReceived.DisplayDate.Day)
                    {
                        CBZ.TempSubEntrepeneur.Request.SentDate = DateRequestSent.DisplayDate = DateRequestReceived.DisplayDate;
                        DateRequestSent.Text = CBZ.TempSubEntrepeneur.Request.SentDate.ToString("dd-MM-yyy");
                    }
                }
            }
        }


        #endregion

    }
}
