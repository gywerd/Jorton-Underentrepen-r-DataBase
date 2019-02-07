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
    /// Interaction logic for UcEditSubEntrepeneurs.xaml
    /// </summary>
    public partial class UcEditSubEntrepeneurs : UserControl
    {
        #region Fields
        public DateTime Date = new DateTime();
        public bool Changed = false;
        public bool ChangeSentDate;
        public bool DbStatus = false;
        public bool OverrideControl = false;

        public Bizz CBZ;
        public UserControl UcRight;
        public List<IndexedContact> IndexedContacts = new List<IndexedContact>();
        public List<Enterprise> IndexedEnterprises = new List<Enterprise>();
        public List<IndexedEntrepeneur> IndexedLegalEntities = new List<IndexedEntrepeneur>();
        public List<IndexedSubEntrepeneur> IndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();

        #endregion

        #region Constructors
        public UcEditSubEntrepeneurs(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = bizz;
            this.UcRight = ucRight;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
            ComboBoxRequest.ItemsSource = CBZ.RequestStatuses;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke redigering af Underentrepenører?", "Luk Rediger Underentrepenør", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                    break;
                }
            }
            OverrideControl = true;
            TextBoxName.Text = CBZ.TempProject.Name;
            IndexedEnterprises = GetIndexedEnterprises();
            ComboBoxEnterprise.ItemsSource = IndexedEnterprises;
            ComboBoxEnterprise.SelectedIndex = 0;
            ListBoxSubEntrepeneurs.ItemsSource = "";
            ListBoxSubEntrepeneurs.SelectedIndex = -1;
            ClearTempEntities();
            TextBoxEntrepeneur.Text = "";
            TextBoxOfferPrice.Text = "";
            ResetComboBoxes();
            ResetRadioButtons();
            OverrideControl = false;
        }

        private void ComboBoxContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxContact.SelectedIndex;
            if (selectedIndex >= 0)
            {
                Changed = false;
                IndexedContact contact = IndexedContacts[selectedIndex];
                if (CBZ.TempSubEntrepeneur.Contact.Id != contact.Id)
                {
                    CBZ.TempSubEntrepeneur.Contact.SetId(contact.Id);
                    Changed = true;
                }
                if (Changed)
                {
                    DbStatus = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur);
                    if (DbStatus)
                    {
                        MessageBox.Show("Kontaktpersonen blev opdateret", "Opdater kontakt", MessageBoxButton.OK, MessageBoxImage.Information);
                        CBZ.RefreshList("SubEntrepeneurs");
                    }
                }
                if (Changed && !DbStatus)
                {
                    MessageBox.Show("Databasen meldte en fejl. Kontaktpersonen blev ikke opdateret", "Opdater kontakt", MessageBoxButton.OK, MessageBoxImage.Information);
                    CBZ.TempSubEntrepeneur.Contact = new Contact();
                    ComboBoxContact.SelectedIndex = 0;
                }
                DbStatus = false;
                Changed = false;
            }
            else
            {
                CBZ.TempContact = new Contact();
            }
        }

        private void ComboBoxEnterprise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!OverrideControl)
            {
                OverrideControl = true;
                ClearTempEntities();
                TextBoxEntrepeneur.Text = "";
                TextBoxOfferPrice.Text = "";
                ResetComboBoxes();
                ResetRadioButtons();
                OverrideControl = false;
                int selectedIndex = ComboBoxEnterprise.SelectedIndex;
                foreach (IndexedEnterprise temp in IndexedEnterprises)
                {
                    if (temp.Index == selectedIndex)
                    {
                        CBZ.TempEnterprise = new Enterprise(temp.Id, temp.Project, temp.Name, temp.Elaboration, temp.OfferList, temp.CraftGroup1, temp.CraftGroup2, temp.CraftGroup3, temp.CraftGroup4);
                        break;
                    }
                }
                IndexedSubEntrepeneurs = GetIndexedSubEntrepeneurs();
                OverrideControl = true;
                ListBoxSubEntrepeneurs.UnselectAll();
                ListBoxSubEntrepeneurs.ItemsSource = null;
                OverrideControl = false;
                if (IndexedSubEntrepeneurs.Count != 0)
                {
                    ListBoxSubEntrepeneurs.ItemsSource = IndexedSubEntrepeneurs;
                }
            }
        }

        private void ComboBoxRequest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!OverrideControl)
            {
                int selectedIndex = ComboBoxRequest.SelectedIndex;
                if (selectedIndex > 0)
                {
                    CheckRequest(selectedIndex);
                    if (Changed)
                    {

                        UpdateRequestStatusInDb(selectedIndex, CBZ.TempSubEntrepeneur.Request);
                    }
                    CBZ.RefreshList("SubEntrepeneurs");
                    Changed = false;
                    Date = new DateTime();
                }
                else if (selectedIndex == 0)
                {
                    Changed = false;
                    Date = new DateTime();
                }
                else
                {
                    ComboBoxRequest.SelectedIndex = -1;
                    CBZ.TempRequest = new Request();
                    DateTime date = DateTime.Now;
                    DateRequest.DisplayDate = date;
                    DateRequest.Text = "";
                }
            }
        }

        private void DateIttLetter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!OverrideControl)
            //{
            //    if (Bizz != null)
            //    {
            //        bool status = CheckTempSubEntrepeneur();
            //        if (status)
            //        {
            //            if (Bizz.TempSubEntrepeneur.IttLetter.Id != Bizz.TempIttLetter.Id)
            //            {
            //                Bizz.TempIttLetter = GetBizzTempIttLetter();
            //            }
            //            if (DateIttLetter.Text != "")
            //            {
            //                if (DateIttLetter.Text.Substring(0, 10) != Bizz.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10))
            //                {
            //                    Bizz.TempIttLetter.SentDate = Convert.ToDateTime(DateIttLetter.Text);
            //                    Changed = true;
            //                }
            //                if (DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10) != DateIttLetter.Text.Substring(0, 10))
            //                {
            //                    OverrideControl = true;
            //                    DateIttLetter.DisplayDate = Bizz.TempIttLetter.SentDate;
            //                    OverrideControl = false;
            //                }
            //            }
            //            else
            //            {
            //                DateIttLetter.DisplayDate = DateTime.Now;
            //            }
            //            if (Changed)
            //            {
            //                ResetIttLetters();
            //                Changed = false;
            //            }
            //        }
            //    }
            //}
        }

        private void DateOffer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!OverrideControl)
            //{
            //    if (Bizz != null)
            //    {
            //        if (Bizz.TempSubEntrepeneur.Offer.Id != Bizz.TempOffer.Id)
            //        {
            //            Bizz.TempOffer = GetBizzTempOffer();
            //        }
            //        bool status = CheckTempSubEntrepeneur();
            //        if (status)
            //        {
            //            if (DateOffer.Text != "")
            //            {
            //                Date = Bizz.TempOffer.ReceivedDate;
            //                if (DateOffer.Text.Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
            //                {
            //                    Bizz.TempOffer.SetReceivedDate(Convert.ToDateTime(DateOffer.Text));
            //                    Changed = true;
            //                }
            //                if (DateOffer.DisplayDate.ToShortDateString().Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
            //                {
            //                    DateOffer.DisplayDate = Bizz.TempOffer.ReceivedDate;
            //                }
            //            }
            //            else
            //            {
            //                DateOffer.DisplayDate = DateTime.Now;
            //            }
            //            if (Changed)
            //            {
            //                ResetOffers();
            //                Changed = false;
            //            }
            //        }
            //    }
            //}
        }

        private void DateRequest_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!OverrideControl)
            //{
            //    if (Bizz != null)
            //    {
            //        Changed = false;
            //        if (Bizz.TempSubEntrepeneur.Request.Id != Bizz.TempRequest.Id)
            //        {
            //            Bizz.TempRequest = GetBizzTempRequest();
            //        }
            //        int index = Bizz.TempRequest.Status.Id;
            //        bool status = CheckTempSubEntrepeneur();
            //        if (status)
            //        {
            //            int selectedIndex = ComboBoxRequest.SelectedIndex;
            //            ChangeSentDate = false;
            //            if (DateRequest.Text != "" && selectedIndex > 0)
            //            {
            //                if (DateRequest.Text.Substring(0, 10) != Bizz.TempRequest.SentDate.ToShortDateString().Substring(0, 10) && Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "17-13-1932")
            //                {
            //                    if (Bizz.TempRequest.Status.Id == 1)
            //                    {
            //                        if (MessageBox.Show("Vi du ændre afsendelsesesdatoen til " + DateRequest.Text + "?", "Forespørgeselsdato", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //                        {
            //                            Bizz.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
            //                            ChangeSentDate = true;
            //                            Changed = true;
            //                            index = 1;
            //                        }
            //                    }
            //                }
            //            }
            //            if (!ChangeSentDate && selectedIndex > 0)
            //            {
            //                if (DateRequest.Text.Substring(0, 10) != Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) && Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "17-13-1932")
            //                {
            //                    if (Bizz.TempRequest.Status.Id == 2)
            //                    {
            //                        if (MessageBox.Show("Vi du ændre modtagelsesdatoen til " + DateRequest.Text + "?", "Forespørgeselsdato", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //                        {
            //                            Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
            //                            Changed = true;
            //                            index = 2;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        if (Changed || ChangeSentDate)
            //        {
            //            UpdateRequestStatusInDb(index, Bizz.TempSubEntrepeneur.Request);
            //            Changed = false;
            //            ChangeSentDate = false;
            //            GetRequestDate(Bizz.TempRequest);
            //            OverrideControl = true;
            //            DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
            //            DateRequest.DisplayDate = Convert.ToDateTime(DateRequest.Text);
            //            OverrideControl = false;
            //        }
            //        else
            //        {
            //            if (DateRequest.Text != "")
            //            {
            //                if (DateRequest.DisplayDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
            //                {
            //                    OverrideControl = true;
            //                    DateRequest.DisplayDate = Convert.ToDateTime(DateRequest.Text);
            //                    OverrideControl = false;
            //                }
            //            }
            //            else
            //            {
            //                OverrideControl = true;
            //                DateRequest.DisplayDate = DateTime.Now;
            //                OverrideControl = false;
            //            }
            //        }
            //    }
            //}
        }

        private void ListBoxSubEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!OverrideControl)
            {
                int selectedIndex = ListBoxSubEntrepeneurs.SelectedIndex;
                if (selectedIndex == -1)
                {
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                    TextBoxEntrepeneur.Text = "";
                    TextBoxOfferPrice.Text = "";
                    ResetComboBoxes();
                    ResetRadioButtons();
                }
                else if (selectedIndex < IndexedSubEntrepeneurs.Count && selectedIndex >= 0)
                {
                    OverrideControl = true;
                    TextBoxEntrepeneur.Text = "";
                    TextBoxOfferPrice.Text = "";
                    ResetComboBoxes();
                    ResetRadioButtons();
                    OverrideControl = false;
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                    IndexedSubEntrepeneurs.Clear();
                    IndexedSubEntrepeneurs = GetIndexedSubEntrepeneurs();
                    //ListBoxSubEntrepeneurs.ItemsSource = null;
                    //ListBoxSubEntrepeneurs.ItemsSource = IndexedSubEntrepeneurs;
                    SetBizzTempSubEntrepeneur(selectedIndex);
                    TextBoxEntrepeneur.Text = CBZ.TempSubEntrepeneur.Entrepeneur.Entity.Name;
                    CBZ.TempIttLetter = GetBizzTempIttLetter();
                    CBZ.TempOffer = GetBizzTempOffer();
                    CBZ.TempRequest = GetBizzTempRequest();
                    SetComboBoxes();
                    SetRadioButtons();
                    TextBoxOfferPrice.Text = CBZ.TempOffer.Price.ToString();
                }
            }
        }

        private void RadioButtonAgreementConcludedYes_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && !OverrideControl)
            {
                bool status = CheckTempSubEntrepeneur();
                if (status)
                {
                    RadioButtonAgreementConcludedYes.IsChecked = true;
                    RadioButtonAgreementConcludedNo.IsChecked = false;
                    bool changed = false;
                    if (CBZ.TempSubEntrepeneur.AgreementConcluded == false)
                    {
                        CBZ.TempSubEntrepeneur.ToggleAgreementConcluded();
                        changed = true;
                    }
                    if (changed)
                    {
                        ResetSubEntrepeneursRadioButtons("Aftalen");
                    }
                }
                else
                {
                    RadioButtonAgreementConcludedYes.IsChecked = false;
                    RadioButtonAgreementConcludedNo.IsChecked = false;
                }
            }
        }

        private void RadioButtonAgreementConcludedNo_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ != null && !OverrideControl)
            {
                bool status = CheckTempSubEntrepeneur();
                if (status)
                {
                    RadioButtonAgreementConcludedYes.IsChecked = false;
                    RadioButtonAgreementConcludedNo.IsChecked = true;
                    bool changed = false;
                    if (CBZ.TempSubEntrepeneur.AgreementConcluded == true)
                    {
                        CBZ.TempSubEntrepeneur.ToggleAgreementConcluded();
                        changed = true;
                    }
                    if (changed)
                    {
                        ResetSubEntrepeneursRadioButtons("Aftalen");
                    }
                }
                else
                {
                    RadioButtonAgreementConcludedYes.IsChecked = false;
                    RadioButtonAgreementConcludedNo.IsChecked = false;
                }
            }
        }

        private void RadioButtonIttLetterSentYes_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (CBZ.TempSubEntrepeneur.IttLetter.Id != CBZ.TempIttLetter.Id)
                        {
                            CBZ.TempIttLetter = GetBizzTempIttLetter();
                        }
                        RadioButtonIttLetterSentYes.IsChecked = true;
                        RadioButtonIttLetterSentNo.IsChecked = false;
                        bool tempChanged = CheckIttLetterSentYes();
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        if (Changed)
                        {
                            ResetIttLetters();
                            Changed = false;
                        }
                    }
                    else
                    {
                        RadioButtonIttLetterSentYes.IsChecked = false;
                        RadioButtonIttLetterSentNo.IsChecked = false;
                    }
                }
            }
        }

        private void RadioButtonIttLetterSentNo_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (CBZ.TempSubEntrepeneur.IttLetter.Id != CBZ.TempIttLetter.Id)
                        {
                            CBZ.TempIttLetter = GetBizzTempIttLetter();
                        }
                        RadioButtonIttLetterSentYes.IsChecked = false;
                        RadioButtonIttLetterSentNo.IsChecked = true;
                        bool tempChanged = CheckIttLetterSentNo();
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        if (Changed)
                        {
                            ResetIttLetters();
                        }
                    }
                    else
                    {
                        RadioButtonIttLetterSentYes.IsChecked = false;
                        RadioButtonIttLetterSentNo.IsChecked = false;
                        DateIttLetter.DisplayDate = DateTime.Now;
                        DateIttLetter.Text = "";
                        TextBoxOfferPrice.Text = "";
                    }
                }
            }
        }

        private void RadioButtonOfferChosenYes_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (CBZ.TempSubEntrepeneur.Offer.Id != CBZ.TempOffer.Id)
                        {
                            CBZ.TempOffer = GetBizzTempOffer();
                        }
                        RadioButtonOfferChosenYes.IsChecked = true;
                        RadioButtonOfferChosenNo.IsChecked = false;
                        bool tempChanged = CheckOfferChosenYes();
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        if (Changed)
                        {
                            ResetOffers();
                        }
                    }
                    else
                    {
                        RadioButtonOfferChosenYes.IsChecked = false;
                        RadioButtonOfferChosenNo.IsChecked = false;
                    }
                }
            }
        }

        private void RadioButtonOfferChosenNo_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (CBZ.TempSubEntrepeneur.Offer.Id != CBZ.TempOffer.Id)
                        {
                            CBZ.TempOffer = GetBizzTempOffer();
                        }
                        RadioButtonOfferChosenYes.IsChecked = false;
                        RadioButtonOfferChosenNo.IsChecked = true;
                        bool tempChanged = CheckOfferChosenNo();
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        if (Changed)
                        {
                            ResetOffers();
                        }
                    }
                    else
                    {
                        RadioButtonOfferChosenYes.IsChecked = false;
                        RadioButtonOfferChosenNo.IsChecked = false;
                    }
                }
            }
        }

        private void RadioButtonOfferReceivedYes_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (CBZ.TempSubEntrepeneur.Offer.Id != CBZ.TempOffer.Id)
                        {
                            CBZ.TempOffer = GetBizzTempOffer();
                        }
                        RadioButtonOfferReceivedYes.IsChecked = true;
                        RadioButtonOfferReceivedNo.IsChecked = false;
                        bool tempChanged = CheckOfferReceivedYes();
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        if (Changed)
                        {
                            ResetOffers();
                        }
                    }
                    else
                    {
                        RadioButtonOfferReceivedYes.IsChecked = false;
                        RadioButtonOfferReceivedYes.IsChecked = false;
                        DateTime date = DateTime.Now;
                        DateOffer.DisplayDate = date;
                        DateOffer.Text = "";
                        TextBoxOfferPrice.Text = "";
                    }
                }
            }
        }

        private void RadioButtonOfferReceivedNo_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (CBZ.TempSubEntrepeneur.Offer.Id != CBZ.TempOffer.Id)
                        {
                            CBZ.TempOffer = GetBizzTempOffer();
                        }
                        bool tempChanged = CheckOfferReceivedNo();
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        RadioButtonOfferReceivedYes.IsChecked = false;
                        RadioButtonOfferReceivedNo.IsChecked = true;
                        if (Changed)
                        {
                            ResetOffers();
                        }
                    }
                    else
                    {
                        RadioButtonOfferReceivedYes.IsChecked = false;
                        RadioButtonOfferReceivedNo.IsChecked = false;
                        DateTime date = DateTime.Now;
                        DateOffer.DisplayDate = date;
                        DateOffer.Text = "";
                        TextBoxOfferPrice.Text = "0,00";
                    }
                }
            }
        }

        private void RadioButtonReservationsYes_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonReservationsYes.IsChecked = true;
                        RadioButtonReservationsNo.IsChecked = false;
                        Changed = false;
                        if (CBZ.TempSubEntrepeneur.Reservations == false)
                        {
                            CBZ.TempSubEntrepeneur.ToggleReservations();
                            Changed = true;
                        }
                        if (Changed)
                        {
                            ResetSubEntrepeneursRadioButtons("Forbeholdet");
                        }
                        if (Changed && !DbStatus)
                        {
                            if (CBZ.TempSubEntrepeneur.Reservations == true)
                            {
                                CBZ.TempSubEntrepeneur.ToggleReservations();
                            }
                            RadioButtonReservationsYes.IsChecked = false;
                            RadioButtonReservationsNo.IsChecked = true;
                        }
                        Changed = false;
                        DbStatus = false;
                    }
                    else
                    {
                        RadioButtonReservationsYes.IsChecked = false;
                        RadioButtonReservationsNo.IsChecked = false;
                    }
                }
            }
        }

        private void RadioButtonReservationsNo_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonReservationsYes.IsChecked = false;
                        RadioButtonReservationsNo.IsChecked = true;
                        Changed = false;
                        if (CBZ.TempSubEntrepeneur.Reservations == true)
                        {
                            CBZ.TempSubEntrepeneur.ToggleReservations();
                            Changed = true;
                        }
                        if (Changed)
                        {
                            ResetSubEntrepeneursRadioButtons("Forbeholdet");
                        }
                        if (Changed && !DbStatus)
                        {
                            if (CBZ.TempSubEntrepeneur.Reservations == true)
                            {
                                CBZ.TempSubEntrepeneur.ToggleReservations();
                            }
                            RadioButtonReservationsYes.IsChecked = false;
                            RadioButtonReservationsNo.IsChecked = true;
                        }
                        Changed = false;
                        DbStatus = false;
                    }
                    else
                    {
                        RadioButtonReservationsYes.IsChecked = false;
                        RadioButtonReservationsNo.IsChecked = false;
                    }
                }
            }
        }

        private void RadioButtonUpholdYes_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonUpholdYes.IsChecked = true;
                        RadioButtonUpholdNo.IsChecked = false;
                        bool changed = false;
                        if (CBZ.TempSubEntrepeneur.Uphold == false)
                        {
                            CBZ.TempSubEntrepeneur.ToggleUphold();
                            changed = true;
                        }
                        if (changed)
                        {
                            ResetSubEntrepeneursRadioButtons("Vedståelsen");
                        }
                    }
                    else
                    {
                        RadioButtonUpholdYes.IsChecked = false;
                        RadioButtonUpholdNo.IsChecked = false;
                    }
                }
            }
        }

        private void RadioButtonUpholdNo_Checked(object sender, RoutedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonUpholdYes.IsChecked = false;
                        RadioButtonUpholdNo.IsChecked = true;
                        bool changed = false;
                        if (CBZ.TempSubEntrepeneur.Uphold == true)
                        {
                            CBZ.TempSubEntrepeneur.ToggleUphold();
                            changed = true;
                        }
                        if (changed)
                        {
                            ResetSubEntrepeneursRadioButtons("Vedståelsen");
                        }
                    }
                    else
                    {
                        RadioButtonUpholdYes.IsChecked = false;
                        RadioButtonUpholdNo.IsChecked = false;
                    }
                }
            }
        }

        private void TextBoxOfferPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!OverrideControl)
            {
                if (CBZ != null)
                {
                    if (CheckTempSubEntrepeneur())
                    {
                        if (CBZ.TempSubEntrepeneur.Offer.Id != CBZ.TempOffer.Id)
                        {
                            CBZ.TempOffer = GetBizzTempOffer();
                        }
                        string temp = TextBoxOfferPrice.Text;
                        temp = ParseOfferPrice(temp);
                        temp = ParseOfferPriceComma(temp);
                        TextBoxOfferPrice.Text = temp;
                        bool tempChanged = CheckOfferPriceForChanges(temp);
                        if (!Changed)
                        {
                            Changed = tempChanged;
                        }
                        if (Changed)
                        {
                            ResetOffers();
                            Changed = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method that compares CraftGroups in LegalEntities and Enterprises
        /// </summary>
        /// <param name="entrepeneur"></param>
        /// <returns>bool</returns>
        private bool CheckCraftGroups(Entrepeneur entrepeneur)
        {
            if (entrepeneur.CraftGroup4.Id != 0)
            {
                if (entrepeneur.CraftGroup4 == CBZ.TempEnterprise.CraftGroup1 || entrepeneur.CraftGroup4 == CBZ.TempEnterprise.CraftGroup2 || entrepeneur.CraftGroup4 == CBZ.TempEnterprise.CraftGroup3 || entrepeneur.CraftGroup4 == CBZ.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            else if (entrepeneur.CraftGroup3.Id != 0)
            {
                if (entrepeneur.CraftGroup3 == CBZ.TempEnterprise.CraftGroup1 || entrepeneur.CraftGroup3 == CBZ.TempEnterprise.CraftGroup2 || entrepeneur.CraftGroup3 == CBZ.TempEnterprise.CraftGroup3 || entrepeneur.CraftGroup3 == CBZ.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            else if (entrepeneur.CraftGroup2.Id != 0)
            {
                if (entrepeneur.CraftGroup2 == CBZ.TempEnterprise.CraftGroup1 || entrepeneur.CraftGroup2 == CBZ.TempEnterprise.CraftGroup2 || entrepeneur.CraftGroup2 == CBZ.TempEnterprise.CraftGroup3 || entrepeneur.CraftGroup2 == CBZ.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            else if (entrepeneur.CraftGroup1.Id != 0)
            {
                if (entrepeneur.CraftGroup1 == CBZ.TempEnterprise.CraftGroup1 || entrepeneur.CraftGroup1 == CBZ.TempEnterprise.CraftGroup2 || entrepeneur.CraftGroup1 == CBZ.TempEnterprise.CraftGroup3 || entrepeneur.CraftGroup1 == CBZ.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method, that checks content wether Bizz.TempIttLetter.SentDate has been changed
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckBizzTempDate(DateTime date, object temp)
        {
            bool result = false;
            if (temp == null)
            {
                return result;
            }

            DateTime tempDate = new DateTime();
            string type = temp.GetType().ToString();
            switch (type)
            {
                case "JudBizz.IttLetter":
                    IttLetter tempIttLetter = new IttLetter((IttLetter)temp);
                    tempDate = Convert.ToDateTime(tempIttLetter.SentDate);
                    break;
                case "JudBizz.Offer":
                    Offer tempOffer = new Offer((Offer)temp);
                    tempDate = Convert.ToDateTime(tempOffer.ReceivedDate);
                    break;
            }

            if (tempDate.ToShortDateString().Substring(0,10) != DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10))
            {
                switch (type)
                {
                    case "JudBizz.IttLetter":
                        CBZ.TempIttLetter.SentDate = date;
                        result = true;
                        break;
                    case "JudBizz.Offer":
                        CBZ.TempOffer.SetReceived(date);
                        result = true;
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonIttLetterSentNo results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckIttLetterSentNo()
        {
            bool result = false;
            if (CBZ.TempIttLetter.Sent == true)
            {
                CBZ.TempIttLetter.ToggleSent();
                result = true;
            }
            Date = Convert.ToDateTime("1932-03-17");
            if (DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
            {
                DateIttLetter.DisplayDate = Date;
            }
            if (DateIttLetter.Text.Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
            {
                DateIttLetter.Text = Date.ToShortDateString();
            }
            bool tempChanged = CheckBizzTempDate(Date, CBZ.TempIttLetter);
            if (!result)
            {
                result = tempChanged;
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonIttLetterSentYes results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckIttLetterSentYes()
        {
            bool result = false;
            if (CBZ.TempIttLetter.Sent == false)
            {
                CBZ.TempIttLetter.ToggleSent();
                result = true;
            }
            DateTime date = Convert.ToDateTime(CBZ.TempIttLetter.SentDate);
            if (DateIttLetter.Text != "" && DateIttLetter.Text != "1932-03-17")
            {
                if (DateIttLetter.Text == DateOffer.DisplayDate.ToShortDateString().Substring(0, 10))
                {
                    date = DateIttLetter.DisplayDate;
                    bool tempChanged = CheckBizzTempDate(date, CBZ.TempOffer);
                    if (!result)
                    {
                        result = tempChanged;
                    }
                }
                else
                {
                    date = Convert.ToDateTime(DateIttLetter.Text);
                    DateIttLetter.DisplayDate = date;
                    bool tempChanged = CheckBizzTempDate(date, CBZ.TempIttLetter);
                    if (!result)
                    {
                        result = tempChanged;
                    }
                }
            }
            if (DateIttLetter.Text == "")
            {
                date = DateTime.Now;
                DateIttLetter.DisplayDate = date;
                DateIttLetter.Text = date.ToShortDateString();
                bool tempChanged = CheckBizzTempDate(date, CBZ.TempOffer);
                if (!result)
                {
                    result = tempChanged;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the price for changes, that needs to be written to Db
        /// </summary>
        /// <param name="temp">string</param>
        /// <returns>bool</returns>
        private bool CheckOfferPriceForChanges(string temp)
        {
            bool result = false;
            if (ParseOfferPrice(CBZ.TempOffer.Price.ToString()) != TextBoxOfferPrice.Text)
            {
                temp = Regex.Replace(temp, "[,]", ".");
                if (temp == "")
                {
                    TextBoxOfferPrice.Text = "0";
                }
                if (CBZ.TempOffer.Price != Convert.ToDouble(TextBoxOfferPrice.Text))
                {
                    CBZ.TempOffer.Price = Convert.ToDouble(TextBoxOfferPrice.Text);
                    result = true;
                }
            }
            if (CBZ.TempOffer.Received && DateOffer.Text.Substring(0, 10) == "31-12-2018")
            {
                DateTime tempDate = DateTime.Now;
                DateOffer.Text = tempDate.ToShortDateString();
                DateOffer.DisplayDate = tempDate;
                CBZ.TempOffer.SetReceived(tempDate);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonOfferChosenNo results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckOfferChosenNo()
        {
            bool result = false;
            if (CBZ.TempOffer.Chosen == true)
            {
                CBZ.TempOffer.ToggleChosen();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonOfferChosenYes results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckOfferChosenYes()
        {
            bool result = false;
            if (CBZ.TempOffer.Chosen == false)
            {
                CBZ.TempOffer.ToggleChosen();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonOfferReceivedNo results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckOfferReceivedNo()
        {
            bool result = false;
            if (CBZ.TempOffer.Received)
            {
                CBZ.TempOffer.ToggleReceived();
                result = true;
            }
            DateTime date = Convert.ToDateTime("1932-03-17");
            if (CBZ.TempOffer.ReceivedDate != date)
            {
                CBZ.TempOffer.ResetReceived();
                result = true;
            }
            if (DateOffer.DisplayDate.ToShortDateString().Substring(0, 10) != date.ToShortDateString().Substring(0, 10))
            {
                DateOffer.DisplayDate = date;
            }
            if (DateOffer.Text.Substring(0, 10) != date.ToShortDateString().Substring(0, 10))
            {
                DateOffer.Text = date.ToShortDateString();
            }
            if (CBZ.TempOffer.Price != Convert.ToDouble(0))
            {
                CBZ.TempOffer.Price = Convert.ToDouble(0);
                result = true;
            }
            TextBoxOfferPrice.Text = CBZ.TempOffer.Price.ToString();
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonOfferReceivedYes results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckOfferReceivedYes()
        {
            bool result = false;
            if (CBZ.TempOffer.Received == false)
            {
                CBZ.TempOffer.ToggleReceived();
                result = true;
            }
            if (CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10) == "1932-03-17")
            {
                CBZ.TempOffer.SetReceived(DateTime.Now);
                result = true;
            }
            if (DateOffer.Text == "" || DateOffer.Text.Substring(0, 10) == "1932-03-17")
            {
                DateOffer.Text = DateTime.Now.ToShortDateString();
            }
            if (DateOffer.Text.Substring(0, 10) != CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
            {
                CBZ.TempOffer.SetReceived(DateOffer.DisplayDate);
                result = true;
            }
            if (DateOffer.Text != DateOffer.DisplayDate.ToShortDateString().Substring(0, 10))
            {
                DateOffer.DisplayDate = Convert.ToDateTime(DateOffer.Text);
            }
            if (TextBoxOfferPrice.Text == "")
            {
                TextBoxOfferPrice.Text = "0";
                CBZ.TempOffer.Price = Convert.ToDouble(TextBoxOfferPrice.Text);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method, that checks the wether changed ComboBoxRequest selection results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private void CheckRequest(int index)
        {
            ComboBoxRequest.SelectedIndex = index;
            if (CBZ.TempRequest.Status.Id != index)
            {
                CBZ.TempRequest.Status = GetRequestStatus(index);
                if (!Changed)
                {
                    Changed = true;
                }
            }
            if (DateRequest.Text == "" || DateRequest.Text.Substring(0, 10) == "17-03-1932")
            {
                DateRequest.Text = DateTime.Now.ToShortDateString().Substring(0, 10);
            }
            if (index == 0 || index == 1)
            {
                if (CBZ.TempRequest.SentDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
                {
                    CBZ.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
            if (index >= 2)
            {
                if (CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
                {
                    CBZ.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
            GetRequestDate(CBZ.TempRequest);
            switch (index)
            {
                case 0:
                    if (Date.ToShortDateString().Substring(0, 10) != CBZ.TempRequest.SentDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestDateNotSent();
                        Date = CBZ.TempRequest.SentDate;
                    }
                    break;
                case 1:
                    if (Date.ToShortDateString().Substring(0, 10) != CBZ.TempRequest.SentDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestSentDate();
                        Date = CBZ.TempRequest.SentDate;
                    }
                    break;
                case 2:
                    if (Date.ToShortDateString().Substring(0, 10) != CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestReceivedDate();
                        Date = CBZ.TempRequest.ReceivedDate;
                    }
                    break;
                case 3:
                    if (Date.ToShortDateString().Substring(0, 10) != CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestDateCancelled();
                        Date = CBZ.TempRequest.ReceivedDate;
                    }
                    break;
            }
            if (DateRequest.DisplayDate.ToShortDateString().Substring(0,10) != Date.ToShortDateString().Substring(0, 10))
            {
                DateRequest.DisplayDate = Date;
            }
            if (DateRequest.Text.Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
            {
                DateRequest.Text = Date.ToShortDateString();
            }
        }

        /// <summary>
        /// Method, that checks content oftempSubEntrepeneur
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckTempSubEntrepeneur()
        {
            bool result = false;
            SubEntrepeneur temp = new SubEntrepeneur();
            if (CBZ.TempSubEntrepeneur != temp)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method, that clears tempSubEntrepeneur, tempContact, tempRequest, tempIttLetter & tempOffer
        /// </summary>
        private void ClearTempEntities()
        {
            CBZ.TempSubEntrepeneur = new SubEntrepeneur();
            CBZ.TempContact = new Contact();
            CBZ.TempRequest = new Request();
            CBZ.TempIttLetter = new IttLetter();
            CBZ.TempOffer = new Offer();
        }

        /// <summary>
        /// Method, that filters existing Legal Entities in SubEntrepeneurs from list of indexable Legal Entities
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<IndexedEntrepeneur> FilterIndexedLegalEntities(List<IndexedEntrepeneur> list)
        {
            List<IndexedEntrepeneur> tempList = new List<IndexedEntrepeneur>();
            List<IndexedEntrepeneur> tempResult = new List<IndexedEntrepeneur>();
            List<IndexedEntrepeneur> result = new List<IndexedEntrepeneur>();
            int i = 0;
            foreach (IndexedEntrepeneur temp in list)
            {
                if (temp.Active.Equals(true))
                {
                    tempList.Add(temp);
                }
            }
            foreach (IndexedEntrepeneur temp in tempList)
            {
                if (!IdExistsInSubEntrepeneurs(CBZ.TempEnterprise.Id, temp.Id))
                {
                    Entrepeneur entrepeneur = new Entrepeneur(temp);
                    IndexedEntrepeneur indexedEntrepeneur = new IndexedEntrepeneur(i, entrepeneur);
                    tempResult.Add(indexedEntrepeneur);
                }
            }
            int regionId = tempResult[1].Region.Id;
            foreach (IndexedEntrepeneur entity in tempResult)
            {
                if (entity.Region.Id == regionId)
                {
                    result.Add(entity);
                }
            }
            foreach (IndexedEntrepeneur entity in tempResult)
            {
                if (entity.Region.Id != regionId && entity.CountryWide.Equals(true))
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        private IttLetter GetBizzTempIttLetter()
        {
            IttLetter result = new IttLetter();
            CBZ.RefreshList("IttLetters");
            foreach (IttLetter tempIttLetter in CBZ.IttLetters)
            {
                if (tempIttLetter.Id == CBZ.TempSubEntrepeneur.IttLetter.Id)
                {
                    result = tempIttLetter;
                    break;
                }
            }
            return result;
        }

        private Offer GetBizzTempOffer()
        {
            Offer result = new Offer();
            CBZ.RefreshList("Offers");
            foreach (Offer tempOffer in CBZ.Offers)
            {
                if (tempOffer.Id == CBZ.TempSubEntrepeneur.Offer.Id)
                {
                    result = tempOffer;
                    break;
                }
            }
            return result;
        }

        private Request GetBizzTempRequest()
        {
            Request result = new Request();
            CBZ.RefreshList("Requests");
            foreach (Request tempRequest in CBZ.Requests)
            {
                if (tempRequest.Id == CBZ.TempSubEntrepeneur.Request.Id)
                {
                    result = tempRequest;
                    break;
                }
            }
            return result;
        }

        /// Methods, creates a list of indexable Contacts
        /// </summary>
        /// <returns>List<IndexedContact></returns>
        private List<IndexedContact> GetIndexedContacts()
        {
            List<IndexedContact> result = new List<IndexedContact>();
            int id = CBZ.TempSubEntrepeneur.Entrepeneur.Id;
            IndexedContact notSpecified = new IndexedContact(0, CBZ.Contacts[0]);
            result.Add(notSpecified);
            int i = 1;
            CBZ.RefreshList("Contacts");
            IndexedContacts.Clear();
            foreach (Contact contact in CBZ.Contacts)
            {
                if (contact.Entity.Id == id)
                {
                    IndexedContact temp = new IndexedContact(i, contact);
                    result.Add(temp);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Methods, creates a list of indexable Enterprises
        /// </summary>
        /// <returns>List<IndexedEnterprise></returns>
        private List<Enterprise> GetIndexedEnterprises()
        {
            List<Enterprise> result = new List<Enterprise>();
            Enterprise notSpecified = new IndexedEnterprise(0, CBZ.Enterprises[0]);
            result.Add(notSpecified);
            int i = 1;
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
        /// <returns>List<IndexedLegalEntity></returns>
        private List<IndexedEntrepeneur> GetIndexedLegalEntities()
        {
            List<IndexedEntrepeneur> result = new List<IndexedEntrepeneur>();
            IndexedEntrepeneur notSpecified = new IndexedEntrepeneur(0, CBZ.Entrepeneurs[0]);
            result.Add(notSpecified);
            int i = 1;
            foreach (Entrepeneur entrepeneur in CBZ.Entrepeneurs)
            {
                if (CheckCraftGroups(entrepeneur))
                {
                    IndexedEntrepeneur temp = new IndexedEntrepeneur(i, entrepeneur);
                    result.Add(temp);
                    i++;
                }
            }
            result = FilterIndexedLegalEntities(result);
            return result;
        }

        /// <summary>
        /// Method that creates a list of indexable SubEbtrepeneurs
        /// </summary>
        /// <returns>List<IndexedLegalEntity></returns>
        private List<IndexedSubEntrepeneur> GetIndexedSubEntrepeneurs()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            int i = 0;
            int id = CBZ.TempEnterprise.Id;
            foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
            {
                if (subEntrepeneur.Enterprise.Id == id)
                {
                    IndexedSubEntrepeneur temp = new IndexedSubEntrepeneur(i, subEntrepeneur);
                    result.Add(temp);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that checks request status and return SentDate or ReceivedDate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private void GetRequestDate(Request request)
        {
            Date = Convert.ToDateTime("1932-03-17");

            switch (request.Status.Id)
            {
                case 0:
                    break;
                case 1:
                    Date = request.SentDate;
                    break;
                case 2:
                    Date = request.ReceivedDate;
                    break;
                case 3:
                    Date = request.ReceivedDate;
                    break;
                case 4:
                    Date = request.ReceivedDate;
                    break;
                case 5:
                    Date = request.ReceivedDate;
                    break;
                case 6:
                    Date = request.ReceivedDate;
                    break;
                case 7:
                    Date = request.ReceivedDate;
                    break;
                case 8:
                    Date = request.ReceivedDate;
                    break;
                case 9:
                    Date = request.ReceivedDate;
                    break;
                case 10:
                    Date = request.ReceivedDate;
                    break;
            }
        }

        private RequestStatus GetRequestStatus(int index)
        {
            IndexedRequestStatus result = new IndexedRequestStatus();
            foreach (IndexedRequestStatus status in CBZ.IndexedRequestStatuses)
            {
                if (status.Index == index)
                {
                    result = status;
                }
            }

            return new RequestStatus(result.Text);
        }

        /// <summary>
        /// Method that returns index for selected Contact
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>int</returns>
        private int GetSelectedContact(int id)
        {
            int result = 0;
            foreach (IndexedContact temp in IndexedContacts)
            {
                try
                {
                    if (temp.Id == id)
                    {
                        result = temp.Index;
                        break;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Den valgte kontakt kunne ikke findes", "Find Valgt Kontakt", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return result;
        }

        /// <summary>
        /// Method that returns index for selected RequestStatus
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Request</returns>
        private Request GetSelectedRequest(int id)
        {
            Request result = new Request();
            foreach (Request temp in CBZ.Requests)
            {
                try
                {
                    if (temp.Id == id)
                    {
                        result = temp;
                    }
                }
                catch (Exception)
                {
                    return result;
                }
            }
            return result;
        }

        /// Method, that checks if a legal entity is already added to SubEntrepeneurs
        /// </summary>
        /// <param name="enterprise">int</param>
        /// <param name="entrepeneur">string</param>
        /// <returns>bool</returns>
        private bool IdExistsInSubEntrepeneurs(int enterprise, int entrepeneur)
        {

            foreach (SubEntrepeneur temp in CBZ.SubEntrepeneurs)
            {
                if (temp.Entrepeneur.Id == entrepeneur && temp.Enterprise.Id == enterprise)
                {
                    return true;
                }
            }

            return false;
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
            ComboBoxRequest.ItemsSource = CBZ.RequestStatuses;
            DateRequest.DisplayDate = DateTime.Now;
            DateRequest.Text = "";
            CBZ.TempRequest = new Request();
        }

        /// <summary>
        /// Method clears and updates IttLetters
        /// </summary>
        private void ResetIttLetters()
        {
            //SubEntrepeneur temp = Bizz.TempSubEntrepeneur;
            UpdateIttLetterSentInDb();
            CBZ.RefreshList("IttLetters");
        }

        /// <summary>
        /// Method clears and updates Offers
        /// </summary>
        private void ResetOffers()
        {
            Offer temp = CBZ.TempOffer;
            UpdateOfferReceivedInDb(temp);
            CBZ.Offers.Clear();
            CBZ.RefreshList("Offers");
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
            CBZ.TempIttLetter = new IttLetter();
            DateIttLetter.DisplayDate = DateTime.Now;
            DateIttLetter.Text = "";
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
            CBZ.TempOffer = new Offer();
            DateOffer.DisplayDate = DateTime.Now;
            DateOffer.Text = "";
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
        /// Method, that clears and update SubEntrepeneurs
        /// </summary>
        private void ResetSubEntrepeneurs()
        {
            CBZ.TempSubEntrepeneur = new SubEntrepeneur();
            CBZ.RefreshList("SubEntrepeneurs");
            IndexedSubEntrepeneurs.Clear();
            IndexedSubEntrepeneurs = GetIndexedSubEntrepeneurs();
            ListBoxSubEntrepeneurs.ItemsSource = null;
            ListBoxSubEntrepeneurs.ItemsSource = IndexedSubEntrepeneurs;
        }

        /// <summary>
        /// Method, that clears and update SubEntrepeneurs
        /// </summary>
        private void ResetSubEntrepeneursRadioButtons(string sender)
        {
            DbStatus = CBZ.UpdateInDb(CBZ.TempSubEntrepeneur);
            CBZ.RefreshList("SubEntrepeneurs");
            IndexedSubEntrepeneurs.Clear();
            IndexedSubEntrepeneurs = GetIndexedSubEntrepeneurs();
            if (!DbStatus)
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. " + sender + " blev ikke rettet. Prøv igen.", "Ret Underentrepenør", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //Show confirmation
                MessageBox.Show(sender + " blev rettet.", "Ret Underentrepenør", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        /// <summary>
        /// Method, that sets content of Bizz.TempIttLetter
        /// </summary>
        /// <param name="id">int</param>
        private void SetBizzTempIttLetter(int id)
        {
            try
            {
                IttLetter tempIttLetter = new IttLetter();
                if (CBZ.TempIttLetter == tempIttLetter && CBZ.TempSubEntrepeneur.IttLetter.Id != 0)
                {
                    foreach (IttLetter temp in CBZ.IttLetters)
                    {
                        if (temp.Id == id)
                        {
                            CBZ.TempIttLetter = temp;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                CBZ.TempIttLetter = new IttLetter();
            }
        }

        /// <summary>
        /// Method, that sets content of Bizz.TempIttLetter
        /// </summary>
        /// <param name="id">int</param>
        private void SetBizzTempOffer(int id)
        {
            try
            {
                Offer tempOffer = new Offer();
                if (CBZ.TempOffer == tempOffer && CBZ.TempSubEntrepeneur.IttLetter.Id != 0)
                {
                    foreach (Offer temp in CBZ.Offers)
                    {
                        if (temp.Id == id)
                        {
                            CBZ.TempOffer = temp;
                        }
                    }
                }
            }
            catch (Exception)
            {
                CBZ.TempOffer = new Offer();
            }
        }

        /// <summary>
        /// Method, that sets content of Bizz.TempSubEntrepeneur
        /// </summary>
        private void SetBizzTempSubEntrepeneur(int index)
        {
            IndexedSubEntrepeneur temp = IndexedSubEntrepeneurs[index];
            CBZ.TempSubEntrepeneur = new SubEntrepeneur(temp.Id, temp.Entrepeneur, temp.Enterprise, temp.Contact, temp.Request, temp.IttLetter, temp.Offer, temp.Reservations, temp.Uphold, temp.AgreementConcluded, temp.Active);
            if (!CBZ.TempSubEntrepeneur.Active)
            {
                CBZ.TempSubEntrepeneur.ToggleActive();
            }
        }

        /// <summary>
        /// Method, that populates ComboBoxes
        /// </summary>
        private void SetComboBoxes()
        {
            IndexedContacts = GetIndexedContacts();
            int contactIndex = GetSelectedContact(CBZ.TempSubEntrepeneur.Contact.Id);
            ComboBoxContact.ItemsSource = IndexedContacts;
            ComboBoxContact.SelectedIndex = contactIndex;
            CBZ.TempRequest = GetSelectedRequest(CBZ.TempSubEntrepeneur.Request.Id);
            GetRequestDate(CBZ.TempRequest);
            DateRequest.DisplayDate = Date;
            DateRequest.Text = Date.ToShortDateString();
            ComboBoxRequest.ItemsSource = CBZ.RequestStatuses;
            ComboBoxRequest.SelectedIndex = CBZ.TempRequest.Status.Id;
        }

        /// <summary>
        /// Method, that sets values for RadioButtons
        /// </summary>
        private void SetRadioButtons()
        {
            SetRadioButtonsIttLetterSent(CBZ.TempSubEntrepeneur.IttLetter.Id);
            SetRadioButtonsOfferReceived(CBZ.TempSubEntrepeneur.Offer.Id);
            SetRadioButtonsReservations(CBZ.TempSubEntrepeneur.Reservations);
            SetRadioButtonsUphold(CBZ.TempSubEntrepeneur.Uphold);
            SetRadioButtonsOfferChosen(CBZ.TempSubEntrepeneur.Offer.Id);
            SetRadioButtonsAgreementConcluded(CBZ.TempSubEntrepeneur.AgreementConcluded);
        }

        /// <summary>
        /// Method, that sets RadioButtonAgreementConcludedYes and RadioButtonAgreementConcludedNo
        /// </summary>
        /// <param name="agreementConcluded">bool</param>
        private void SetRadioButtonsAgreementConcluded(bool agreementConcluded)
        {
            if (agreementConcluded)
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
        /// <param name="ittLetter">int</param>
        private void SetRadioButtonsIttLetterSent(int ittLetter)
        {
            SetBizzTempIttLetter(ittLetter);
            if (CBZ.TempIttLetter.Sent)
            {
                if (CBZ.TempIttLetter.SentDate.ToShortDateString() == "")
                {
                    CBZ.TempIttLetter.SentDate = DateTime.Now;
                }
                if (DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10) != CBZ.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.DisplayDate = CBZ.TempIttLetter.SentDate;
                }
                if (DateIttLetter.Text == "" || DateIttLetter.Text.Substring(0, 10) != CBZ.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.Text = CBZ.TempIttLetter.SentDate.ToShortDateString();
                }
                RadioButtonIttLetterSentYes.IsChecked = true;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }
            else
            {
                Date = Convert.ToDateTime("1932-03-17");
                if (CBZ.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
                {
                    CBZ.TempIttLetter.SentDate = Date;
                }
                if (DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.DisplayDate = CBZ.TempIttLetter.SentDate;
                }
                if (DateIttLetter.Text == "" || DateIttLetter.Text.Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.Text = CBZ.TempIttLetter.SentDate.ToShortDateString();
                }
                RadioButtonIttLetterSentYes.IsChecked = false;
                RadioButtonIttLetterSentNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonOfferChosenYes and RadioButtonOfferChosenNo
        /// </summary>
        /// <param name="offer">int</param>
        private void SetRadioButtonsOfferChosen(int offer)
        {
            SetBizzTempOffer(offer);
            if (CBZ.TempOffer.Chosen)
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
        /// <param name="offer">int</param>
        private void SetRadioButtonsOfferReceived(int offer)
        {
            SetBizzTempOffer(offer);
            if (CBZ.TempOffer.Received)
            {
                if (CBZ.TempOffer.ReceivedDate.ToShortDateString() == "" || CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10) == "1932-03-17")
                {
                    CBZ.TempOffer.SetReceived(DateTime.Now);
                }
                if (DateOffer.DisplayDate.ToShortDateString().Substring(0, 10) != CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.DisplayDate = CBZ.TempOffer.ReceivedDate;
                }
                if (DateOffer.Text == "" || DateOffer.Text.Substring(0, 10) != CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.Text = CBZ.TempOffer.ReceivedDate.ToShortDateString();
                }
                RadioButtonOfferReceivedYes.IsChecked = true;
                RadioButtonOfferReceivedNo.IsChecked = false;
            }
            else
            {
                CBZ.TempOffer.SetReceived(Convert.ToDateTime("1932-03-17"));
                if (DateOffer.DisplayDate.ToShortDateString().Substring(0, 10) != CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.DisplayDate = CBZ.TempOffer.ReceivedDate;
                }
                if (DateOffer.Text == "" || DateOffer.Text.Substring(0, 10) != CBZ.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.Text = CBZ.TempOffer.ReceivedDate.ToShortDateString();
                }
                RadioButtonOfferReceivedYes.IsChecked = false;
                RadioButtonOfferReceivedNo.IsChecked = true;
            }
        }

        /// <summary>
        /// Method, that sets RadioButtonReservationsYes and RadioButtonReservationsNo
        /// </summary>
        /// <param name="reservations">bool</param>
        private void SetRadioButtonsReservations(bool reservations)
        {
            if (reservations)
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
        /// <param name="uphold">bool</param>
        private void SetRadioButtonsUphold(bool uphold)
        {
            if (uphold)
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
        /// Method, that coordinates Bizz.tempRequest.RecievedDate and DateRequest.Text when cancelled
        /// </summary>
        private void SetRequestDateCancelled()
        {
            if (CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    if (CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
                    {
                        CBZ.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                        Date = CBZ.TempRequest.ReceivedDate;
                        if (!Changed)
                        {
                            Changed = true;
                        }
                        CBZ.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                        Date = CBZ.TempRequest.ReceivedDate;
                        if (!Changed)
                        {
                            Changed = true;
                        }
                    }
                }
                else
                {
                    DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
                }
            }
            else
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    CBZ.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                    Date = CBZ.TempRequest.ReceivedDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    Date = DateTime.Now;
                    CBZ.TempRequest.ReceivedDate = Date;
                    DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method, that sets Bizz.tempRequest.SentDate and DateRequest.Text to 1932-03-17
        /// </summary>
        private void SetRequestDateNotSent()
        {
            if (CBZ.TempRequest.SentDate.ToShortDateString().Substring(0, 10) != "1932-03-17" || CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                Date = Convert.ToDateTime("1932-03-17");
                CBZ.TempRequest.ReceivedDate = Date;
                CBZ.TempRequest.SentDate = Date;
                Changed = true;
            }
        }

        /// <summary>
        /// Method, that coordinates Bizz.tempRequest.RecievedDate and DateRequest.Text
        /// </summary>
        private void SetRequestReceivedDate()
        {
            if (CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    if (DateRequest.Text.Substring(0, 10) != CBZ.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10))
                    {
                        CBZ.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                        Date = CBZ.TempRequest.ReceivedDate;
                        if (!Changed)
                        {
                            Changed = true;
                        }
                    }
                }
                else
                {
                    DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
                }
            }
            else
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    CBZ.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                    Date = CBZ.TempRequest.ReceivedDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    Date = DateTime.Now;
                    CBZ.TempRequest.ReceivedDate = Date;
                    DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method, that coordinates Bizz.tempRequest.SentDate and DateRequest.Text
        /// </summary>
        private void SetRequestSentDate()
        {
            if (CBZ.TempRequest.SentDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                Date = CBZ.TempRequest.SentDate;
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    CBZ.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
                    Date = CBZ.TempRequest.SentDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
                }
            }
            else
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    CBZ.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
                    Date = CBZ.TempRequest.SentDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    Date = DateTime.Now;
                    CBZ.TempRequest.SentDate = Date;
                    DateRequest.Text = Date.ToShortDateString().Substring(0, 10);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method, that update sent status on an IttLetter in Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="sent">bool</param>
        private void UpdateIttLetterSentInDb()
        {
            string date = DateIttLetter.DisplayDate.Year + "-" + DateIttLetter.DisplayDate.Month + "-" + DateIttLetter.DisplayDate.Day;
            // Code that save changes to the project
            bool result = CBZ.UpdateInDb(CBZ.TempIttLetter);

            if (result)
            {
                //Show confirmation
                MessageBox.Show("Udbudsbrevets status blev rettet.", "Ret Udbudsbrevsstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Udbudsbrevets status blev ikke rettet. Prøv igen.", "Ret Udbudsbrevsstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Method, that update received status on an Offer in Db
        /// </summary>
        /// <param name="offer">int</param>
        /// <param name="received">bool</param>
        private void UpdateOfferReceivedInDb(Offer offer)
        {
            //string strDate = DateOffer.DisplayDate.Year + "-" + DateOffer.DisplayDate.Month + "-" + DateOffer.DisplayDate.Day;
            string strDate = offer.ReceivedDate.Year + "-" + offer.ReceivedDate.Month + "-" + offer.ReceivedDate.Day;
            // Code that save changes to the project
            bool result = CBZ.UpdateInDb(offer);

            if (result)
            {
                //Show confirmation
                MessageBox.Show("Tilbuddets status blev rettet.", "Ret Tilbudsstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Tilbuddets status blev ikke rettet. Prøv igen.", "Ret Tilbudsstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Method, that update an Request status in Db
        /// </summary>
        /// <param name="request">Request</param>
        private void UpdateRequest(Request request)
        {
            // Code that save changes to the project
            bool result = CBZ.UpdateInDb(request);

            if (!result)
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Forespørgslens status blev ikke rettet. Prøv igen.", "Ret Forespørgselsstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //Show confirmation
                MessageBox.Show("Forespørgslens status blev rettet.", "Ret Forespørgselsstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateRequestStatusInDb(int selectedIndex, Request request)
        {
            request.Status = GetStatus(selectedIndex);
            UpdateRequest(request);
        }

        private RequestStatus GetStatus(int requestStatusId)
        {
            RequestStatus result = new RequestStatus();

            foreach (RequestStatus status in CBZ.RequestStatuses)
            {
                if (status.Id == requestStatusId)
                {
                    result = status;
                    break;
                }
            }

            return result;
        }

        #endregion

    }
}
