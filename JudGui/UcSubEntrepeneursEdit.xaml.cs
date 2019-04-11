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
        public bool Changed = false;
        public bool ChangeSentDate;
        public bool DbStatus = false;
        public bool OverrideControl = false;

        public Bizz CBZ;
        public UserControl UcMain;

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
                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxSubEntrepeneurs.SelectedItems.Count > 0)
            {
                bool result = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur);

                if (result)
                {
                    MessageBox.Show("Underentrepenøren blev opdateret?", "Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Information);

                    ComboBoxCaseId.SelectedIndex = -1;
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                    ResetComboBoxes();
                    ResetRadioButtons();

                    CBZ.UcMainEdited = false;
                }
                else
                {
                    MessageBox.Show("Der opstod en fejl, ved opdatering af Underentrepenøren. Prøv Igen.", "Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex > -1)
            {
                TextBoxName.Text = CBZ.TempProject.Name;
                GetIndexedEnterprises();
                ComboBoxEnterprise.ItemsSource = "";
                ComboBoxEnterprise.ItemsSource = CBZ.IndexedEnterprises;
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
                CBZ.TempSubEntrepeneur.Contact = ((Contact)ComboBoxContact.SelectedItem);

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
            if (ComboBoxEnterprise.SelectedIndex > 0)
            {
                TextBoxEntrepeneur.Text = "";
                TextBoxOfferPrice.Text = "";
                CBZ.TempEnterprise = new Enterprise((Enterprise)ComboBoxEnterprise.SelectedItem);
                ResetComboBoxes();
                ResetRadioButtons();
                ListBoxSubEntrepeneurs.UnselectAll();
                ListBoxSubEntrepeneurs.ItemsSource = "";
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
            if (ComboBoxRequest.SelectedIndex > 0)
            {
                if (CBZ.TempSubEntrepeneur != new SubEntrepeneur())
                {
                    CBZ.TempSubEntrepeneur.Request = new Request((IndexedRequest)ComboBoxRequest.SelectedItem);
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
            if (CheckTempSubEntrepeneur())
            {
                CBZ.TempSubEntrepeneur.IttLetter.SentDate = DateIttLetter.DisplayDate;

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

        private void DateOffer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckTempSubEntrepeneur())
            {
                CBZ.TempSubEntrepeneur.Offer.SetReceived(DateOffer.DisplayDate);

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

        private void DateRequestReceived_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckTempSubEntrepeneur())
            {
                CBZ.TempSubEntrepeneur.Request.ReceivedDate = (DateOffer.DisplayDate);

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

        private void DateRequestSent_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckTempSubEntrepeneur())
            {
                CBZ.TempSubEntrepeneur.Request.SentDate = (DateOffer.DisplayDate);


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
                RadioButtonOfferReceivedYes.IsChecked = true;
                RadioButtonOfferReceivedNo.IsChecked = false;
                if (!CBZ.TempSubEntrepeneur.AgreementConcluded)
                {
                    CBZ.TempSubEntrepeneur.ToggleAgreementConcluded();
                }
            }
            else
            {
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedYes.IsChecked = false;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited && CheckTempSubEntrepeneur())
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void RadioButtonAgreementConcludedNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                RadioButtonAgreementConcludedYes.IsChecked = false;
                RadioButtonAgreementConcludedNo.IsChecked = true;
                if (CBZ.TempSubEntrepeneur.AgreementConcluded == true)
                {
                    CBZ.TempSubEntrepeneur.ToggleAgreementConcluded();
                }
            }
            else
            {
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedYes.IsChecked = false;
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

        private void RadioButtonIttLetterSentYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && CheckTempSubEntrepeneur())
            {
                if (!CBZ.TempSubEntrepeneur.IttLetter.Sent)
                {
                    CBZ.TempSubEntrepeneur.IttLetter.ToggleSent();
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
                    CBZ.TempSubEntrepeneur.IttLetter.ToggleSent();
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
                    CBZ.TempSubEntrepeneur.Offer.ToggleReceived();
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
                    CBZ.TempSubEntrepeneur.Offer.ToggleReceived();
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
        /// Methods, creates a list of indexable Enterprises
        /// </summary>
        private void GetIndexedEnterprises()
        {
            CBZ.IndexedEnterprises.Clear();
            CBZ.RefreshList("Enterprises");
            CBZ.IndexedEnterprises.Add(new IndexedEnterprise(0, CBZ.Enterprises[0]));
            int i = 1;
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    CBZ.IndexedEnterprises.Add(new IndexedEnterprise(i, enterprise));
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
            ComboBoxRequest.SelectedIndex = CBZ.TempRequest.Status.Id;
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


        #endregion

    }
}
