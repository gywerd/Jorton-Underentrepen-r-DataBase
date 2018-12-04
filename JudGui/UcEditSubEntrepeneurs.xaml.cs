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

        public Bizz Bizz;
        public UserControl UcRight;
        public List<IndexedContact> IndexableContacts = new List<IndexedContact>();
        public List<Enterprise> IndexableEnterpriseList = new List<Enterprise>();
        public List<IndexedLegalEntity> IndexableLegalEntities = new List<IndexedLegalEntity>();
        public List<IndexedSubEntrepeneur> IndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();

        #endregion

        #region Constructors
        public UcEditSubEntrepeneurs(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            ComboBoxCaseId.ItemsSource = Bizz.IndexedActiveProjects;
            ComboBoxRequest.ItemsSource = Bizz.RequestStatusList;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke redigering af Underentrepenører?", "Luk Rediger Underentrepenør", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
                    break;
                }
            }
            OverrideControl = true;
            TextBoxName.Text = Bizz.TempProject.Name;
            IndexableEnterpriseList = GetIndexableEnterpriseList();
            ComboBoxEnterprise.ItemsSource = IndexableEnterpriseList;
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
                IndexedContact contact = IndexableContacts[selectedIndex];
                if (Bizz.TempSubEntrepeneur.Contact.Id != contact.Id)
                {
                    Bizz.TempSubEntrepeneur.Contact.SetId(contact.Id);
                    Changed = true;
                }
                if (Changed)
                {
                    DbStatus = Bizz.UpdateInDb(Bizz.TempSubEntrepeneur);
                    if (DbStatus)
                    {
                        MessageBox.Show("Kontaktpersonen blev opdateret", "Opdater kontakt", MessageBoxButton.OK, MessageBoxImage.Information);
                        Bizz.RefreshList("SubEntrepeneurs");
                    }
                }
                if (Changed && !DbStatus)
                {
                    MessageBox.Show("Databasen meldte en fejl. Kontaktpersonen blev ikke opdateret", "Opdater kontakt", MessageBoxButton.OK, MessageBoxImage.Information);
                    Bizz.TempSubEntrepeneur.Contact = new Contact();
                    ComboBoxContact.SelectedIndex = 0;
                }
                DbStatus = false;
                Changed = false;
            }
            else
            {
                Bizz.TempContact = new Contact();
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
                foreach (IndexedEnterprise temp in IndexableEnterpriseList)
                {
                    if (temp.Index == selectedIndex)
                    {
                        Bizz.TempEnterprise = new Enterprise(temp.Id, temp.Project, temp.Name, temp.Elaboration, temp.OfferList, temp.CraftGroup1, temp.CraftGroup2, temp.CraftGroup3, temp.CraftGroup4);
                        break;
                    }
                }
                IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
                OverrideControl = true;
                ListBoxSubEntrepeneurs.UnselectAll();
                ListBoxSubEntrepeneurs.ItemsSource = null;
                OverrideControl = false;
                if (IndexableSubEntrepeneurs.Count != 0)
                {
                    ListBoxSubEntrepeneurs.ItemsSource = IndexableSubEntrepeneurs;
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

                        UpdateRequestStatusInDb(selectedIndex, Bizz.TempSubEntrepeneur.Request);
                    }
                    Bizz.RefreshList("SubEntrepeneurs");
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
                    Bizz.TempRequest = new Request();
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
                    Bizz.TempSubEntrepeneur = new SubEntrepeneur();
                    TextBoxEntrepeneur.Text = "";
                    TextBoxOfferPrice.Text = "";
                    ResetComboBoxes();
                    ResetRadioButtons();
                }
                else if (selectedIndex < IndexableSubEntrepeneurs.Count && selectedIndex >= 0)
                {
                    OverrideControl = true;
                    TextBoxEntrepeneur.Text = "";
                    TextBoxOfferPrice.Text = "";
                    ResetComboBoxes();
                    ResetRadioButtons();
                    OverrideControl = false;
                    Bizz.TempSubEntrepeneur = new SubEntrepeneur();
                    IndexableSubEntrepeneurs.Clear();
                    IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
                    //ListBoxSubEntrepeneurs.ItemsSource = null;
                    //ListBoxSubEntrepeneurs.ItemsSource = IndexableSubEntrepeneurs;
                    SetBizzTempSubEntrepeneur(selectedIndex);
                    TextBoxEntrepeneur.Text = Bizz.TempSubEntrepeneur.Entrepeneur.Name;
                    Bizz.TempIttLetter = GetBizzTempIttLetter();
                    Bizz.TempOffer = GetBizzTempOffer();
                    Bizz.TempRequest = GetBizzTempRequest();
                    SetComboBoxes();
                    SetRadioButtons();
                    TextBoxOfferPrice.Text = Bizz.TempOffer.Price.ToString();
                }
            }
        }

        private void RadioButtonAgreementConcludedYes_Checked(object sender, RoutedEventArgs e)
        {
            if (Bizz != null && !OverrideControl)
            {
                bool status = CheckTempSubEntrepeneur();
                if (status)
                {
                    RadioButtonAgreementConcludedYes.IsChecked = true;
                    RadioButtonAgreementConcludedNo.IsChecked = false;
                    bool changed = false;
                    if (Bizz.TempSubEntrepeneur.AgreementConcluded == false)
                    {
                        Bizz.TempSubEntrepeneur.ToggleAgreementConcluded();
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
            if (Bizz != null && !OverrideControl)
            {
                bool status = CheckTempSubEntrepeneur();
                if (status)
                {
                    RadioButtonAgreementConcludedYes.IsChecked = false;
                    RadioButtonAgreementConcludedNo.IsChecked = true;
                    bool changed = false;
                    if (Bizz.TempSubEntrepeneur.AgreementConcluded == true)
                    {
                        Bizz.TempSubEntrepeneur.ToggleAgreementConcluded();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (Bizz.TempSubEntrepeneur.IttLetter.Id != Bizz.TempIttLetter.Id)
                        {
                            Bizz.TempIttLetter = GetBizzTempIttLetter();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (Bizz.TempSubEntrepeneur.IttLetter.Id != Bizz.TempIttLetter.Id)
                        {
                            Bizz.TempIttLetter = GetBizzTempIttLetter();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (Bizz.TempSubEntrepeneur.Offer.Id != Bizz.TempOffer.Id)
                        {
                            Bizz.TempOffer = GetBizzTempOffer();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (Bizz.TempSubEntrepeneur.Offer.Id != Bizz.TempOffer.Id)
                        {
                            Bizz.TempOffer = GetBizzTempOffer();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (Bizz.TempSubEntrepeneur.Offer.Id != Bizz.TempOffer.Id)
                        {
                            Bizz.TempOffer = GetBizzTempOffer();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        if (Bizz.TempSubEntrepeneur.Offer.Id != Bizz.TempOffer.Id)
                        {
                            Bizz.TempOffer = GetBizzTempOffer();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonReservationsYes.IsChecked = true;
                        RadioButtonReservationsNo.IsChecked = false;
                        Changed = false;
                        if (Bizz.TempSubEntrepeneur.Reservations == false)
                        {
                            Bizz.TempSubEntrepeneur.ToggleReservations();
                            Changed = true;
                        }
                        if (Changed)
                        {
                            ResetSubEntrepeneursRadioButtons("Forbeholdet");
                        }
                        if (Changed && !DbStatus)
                        {
                            if (Bizz.TempSubEntrepeneur.Reservations == true)
                            {
                                Bizz.TempSubEntrepeneur.ToggleReservations();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonReservationsYes.IsChecked = false;
                        RadioButtonReservationsNo.IsChecked = true;
                        Changed = false;
                        if (Bizz.TempSubEntrepeneur.Reservations == true)
                        {
                            Bizz.TempSubEntrepeneur.ToggleReservations();
                            Changed = true;
                        }
                        if (Changed)
                        {
                            ResetSubEntrepeneursRadioButtons("Forbeholdet");
                        }
                        if (Changed && !DbStatus)
                        {
                            if (Bizz.TempSubEntrepeneur.Reservations == true)
                            {
                                Bizz.TempSubEntrepeneur.ToggleReservations();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonUpholdYes.IsChecked = true;
                        RadioButtonUpholdNo.IsChecked = false;
                        bool changed = false;
                        if (Bizz.TempSubEntrepeneur.Uphold == false)
                        {
                            Bizz.TempSubEntrepeneur.ToggleUphold();
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
                if (Bizz != null)
                {
                    bool status = CheckTempSubEntrepeneur();
                    if (status)
                    {
                        RadioButtonUpholdYes.IsChecked = false;
                        RadioButtonUpholdNo.IsChecked = true;
                        bool changed = false;
                        if (Bizz.TempSubEntrepeneur.Uphold == true)
                        {
                            Bizz.TempSubEntrepeneur.ToggleUphold();
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
                if (Bizz != null)
                {
                    if (CheckTempSubEntrepeneur())
                    {
                        if (Bizz.TempSubEntrepeneur.Offer.Id != Bizz.TempOffer.Id)
                        {
                            Bizz.TempOffer = GetBizzTempOffer();
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
        /// Method that compares CraftGroups in LegalEntities and EnterpriseList
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>bool</returns>
        private bool CheckCraftGroups(LegalEntity entity)
        {
            if (entity.CraftGroup4.Id != 0)
            {
                if (entity.CraftGroup4 == Bizz.TempEnterprise.CraftGroup1 || entity.CraftGroup4 == Bizz.TempEnterprise.CraftGroup2 || entity.CraftGroup4 == Bizz.TempEnterprise.CraftGroup3 || entity.CraftGroup4 == Bizz.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            else if (entity.CraftGroup3.Id != 0)
            {
                if (entity.CraftGroup3 == Bizz.TempEnterprise.CraftGroup1 || entity.CraftGroup3 == Bizz.TempEnterprise.CraftGroup2 || entity.CraftGroup3 == Bizz.TempEnterprise.CraftGroup3 || entity.CraftGroup3 == Bizz.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            else if (entity.CraftGroup2.Id != 0)
            {
                if (entity.CraftGroup2 == Bizz.TempEnterprise.CraftGroup1 || entity.CraftGroup2 == Bizz.TempEnterprise.CraftGroup2 || entity.CraftGroup2 == Bizz.TempEnterprise.CraftGroup3 || entity.CraftGroup2 == Bizz.TempEnterprise.CraftGroup4)
                {
                    return true;
                }
            }
            else if (entity.CraftGroup1.Id != 0)
            {
                if (entity.CraftGroup1 == Bizz.TempEnterprise.CraftGroup1 || entity.CraftGroup1 == Bizz.TempEnterprise.CraftGroup2 || entity.CraftGroup1 == Bizz.TempEnterprise.CraftGroup3 || entity.CraftGroup1 == Bizz.TempEnterprise.CraftGroup4)
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
                        Bizz.TempIttLetter.SentDate = date;
                        result = true;
                        break;
                    case "JudBizz.Offer":
                        Bizz.TempOffer.SetReceivedDate(date);
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
            if (Bizz.TempIttLetter.Sent == true)
            {
                Bizz.TempIttLetter.ToggleSent();
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
            bool tempChanged = CheckBizzTempDate(Date, Bizz.TempIttLetter);
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
            if (Bizz.TempIttLetter.Sent == false)
            {
                Bizz.TempIttLetter.ToggleSent();
                result = true;
            }
            DateTime date = Convert.ToDateTime(Bizz.TempIttLetter.SentDate);
            if (DateIttLetter.Text != "" && DateIttLetter.Text != "1932-03-17")
            {
                if (DateIttLetter.Text == DateOffer.DisplayDate.ToShortDateString().Substring(0, 10))
                {
                    date = DateIttLetter.DisplayDate;
                    bool tempChanged = CheckBizzTempDate(date, Bizz.TempOffer);
                    if (!result)
                    {
                        result = tempChanged;
                    }
                }
                else
                {
                    date = Convert.ToDateTime(DateIttLetter.Text);
                    DateIttLetter.DisplayDate = date;
                    bool tempChanged = CheckBizzTempDate(date, Bizz.TempIttLetter);
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
                bool tempChanged = CheckBizzTempDate(date, Bizz.TempOffer);
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
            if (ParseOfferPrice(Bizz.TempOffer.Price.ToString()) != TextBoxOfferPrice.Text)
            {
                temp = Regex.Replace(temp, "[,]", ".");
                if (temp == "")
                {
                    TextBoxOfferPrice.Text = "0";
                }
                if (Bizz.TempOffer.Price != Convert.ToDouble(TextBoxOfferPrice.Text))
                {
                    Bizz.TempOffer.Price = Convert.ToDouble(TextBoxOfferPrice.Text);
                    result = true;
                }
            }
            if (Bizz.TempOffer.Received && DateOffer.Text.Substring(0, 10) == "31-12-2018")
            {
                DateTime tempDate = DateTime.Now;
                DateOffer.Text = tempDate.ToShortDateString();
                DateOffer.DisplayDate = tempDate;
                Bizz.TempOffer.SetReceivedDate(tempDate);
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
            if (Bizz.TempOffer.Chosen == true)
            {
                Bizz.TempOffer.ToggleChosen();
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
            if (Bizz.TempOffer.Chosen == false)
            {
                Bizz.TempOffer.ToggleChosen();
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
            if (Bizz.TempOffer.Received)
            {
                Bizz.TempOffer.ToggleReceived();
                result = true;
            }
            DateTime date = Convert.ToDateTime("1932-03-17");
            if (Bizz.TempOffer.ReceivedDate != date)
            {
                Bizz.TempOffer.ResetReceived();
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
            if (Bizz.TempOffer.Price != Convert.ToDouble(0))
            {
                Bizz.TempOffer.Price = Convert.ToDouble(0);
                result = true;
            }
            TextBoxOfferPrice.Text = Bizz.TempOffer.Price.ToString();
            return result;
        }

        /// <summary>
        /// Method, that checks the wether checking RadioButtonOfferReceivedYes results in changes, that needs to be written to Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckOfferReceivedYes()
        {
            bool result = false;
            if (Bizz.TempOffer.Received == false)
            {
                Bizz.TempOffer.ToggleReceived();
                result = true;
            }
            if (Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10) == "1932-03-17")
            {
                Bizz.TempOffer.SetReceivedDate(DateTime.Now);
                result = true;
            }
            if (DateOffer.Text == "" || DateOffer.Text.Substring(0, 10) == "1932-03-17")
            {
                DateOffer.Text = DateTime.Now.ToShortDateString();
            }
            if (DateOffer.Text.Substring(0, 10) != Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
            {
                Bizz.TempOffer.SetReceivedDate(DateOffer.DisplayDate);
                result = true;
            }
            if (DateOffer.Text != DateOffer.DisplayDate.ToShortDateString().Substring(0, 10))
            {
                DateOffer.DisplayDate = Convert.ToDateTime(DateOffer.Text);
            }
            if (TextBoxOfferPrice.Text == "")
            {
                TextBoxOfferPrice.Text = "0";
                Bizz.TempOffer.Price = Convert.ToDouble(TextBoxOfferPrice.Text);
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
            if (Bizz.TempRequest.Status.Id != index)
            {
                Bizz.TempRequest.Status = GetRequestStatus(index);
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
                if (Bizz.TempRequest.SentDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
                {
                    Bizz.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
            if (index >= 2)
            {
                if (Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
                {
                    Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
            }
            GetRequestDate(Bizz.TempRequest);
            switch (index)
            {
                case 0:
                    if (Date.ToShortDateString().Substring(0, 10) != Bizz.TempRequest.SentDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestDateNotSent();
                        Date = Bizz.TempRequest.SentDate;
                    }
                    break;
                case 1:
                    if (Date.ToShortDateString().Substring(0, 10) != Bizz.TempRequest.SentDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestSentDate();
                        Date = Bizz.TempRequest.SentDate;
                    }
                    break;
                case 2:
                    if (Date.ToShortDateString().Substring(0, 10) != Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestReceivedDate();
                        Date = Bizz.TempRequest.ReceivedDate;
                    }
                    break;
                case 3:
                    if (Date.ToShortDateString().Substring(0, 10) != Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10))
                    {
                        SetRequestDateCancelled();
                        Date = Bizz.TempRequest.ReceivedDate;
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
            if (Bizz.TempSubEntrepeneur != temp)
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
            Bizz.TempSubEntrepeneur = new SubEntrepeneur();
            Bizz.TempContact = new Contact();
            Bizz.TempRequest = new Request();
            Bizz.TempIttLetter = new IttLetter();
            Bizz.TempOffer = new Offer();
        }

        /// <summary>
        /// Method, that filters existing Legal Entities in SubEntrepeneurs from list of indexable Legal Entities
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<IndexedLegalEntity> FilterIndexableLegalEntities(List<IndexedLegalEntity> list)
        {
            List<IndexedLegalEntity> tempList = new List<IndexedLegalEntity>();
            List<IndexedLegalEntity> tempResult = new List<IndexedLegalEntity>();
            List<IndexedLegalEntity> result = new List<IndexedLegalEntity>();
            int i = 0;
            foreach (IndexedLegalEntity temp in list)
            {
                if (temp.Active.Equals(true))
                {
                    tempList.Add(temp);
                }
            }
            foreach (IndexedLegalEntity temp in tempList)
            {
                if (!IdExistsInSubEntrepeneurs(Bizz.TempEnterprise.Id, temp.Id))
                {
                    LegalEntity legalEntity = new LegalEntity(temp.Id, temp.Name, temp.Address, temp.ContactInfo, temp.Url, temp.CraftGroup1, temp.CraftGroup2, temp.CraftGroup3, temp.CraftGroup4, temp.Region, temp.CountryWide, temp.Cooperative, temp.Active);
                    IndexedLegalEntity entity = new IndexedLegalEntity(i, legalEntity);
                    tempResult.Add(entity);
                }
            }
            int regionId = tempResult[1].Region.Id;
            foreach (IndexedLegalEntity entity in tempResult)
            {
                if (entity.Region.Id == regionId)
                {
                    result.Add(entity);
                }
            }
            foreach (IndexedLegalEntity entity in tempResult)
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
            Bizz.RefreshList("IttLetters");
            foreach (IttLetter tempIttLetter in Bizz.IttLetters)
            {
                if (tempIttLetter.Id == Bizz.TempSubEntrepeneur.IttLetter.Id)
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
            Bizz.RefreshList("Offers");
            foreach (Offer tempOffer in Bizz.Offers)
            {
                if (tempOffer.Id == Bizz.TempSubEntrepeneur.Offer.Id)
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
            Bizz.RefreshList("Requests");
            foreach (Request tempRequest in Bizz.Requests)
            {
                if (tempRequest.Id == Bizz.TempSubEntrepeneur.Request.Id)
                {
                    result = tempRequest;
                    break;
                }
            }
            return result;
        }

        /// Methods, creates a list of indexable Contacts
        /// </summary>
        /// <returns>List<IndexableContact></returns>
        private List<IndexedContact> GetIndexableContacts()
        {
            List<IndexedContact> result = new List<IndexedContact>();
            string id = Bizz.TempSubEntrepeneur.Entrepeneur.Id;
            IndexedContact notSpecified = new IndexedContact(0, Bizz.Contacts[0]);
            result.Add(notSpecified);
            int i = 1;
            Bizz.RefreshList("Contacts");
            IndexableContacts.Clear();
            foreach (Contact contact in Bizz.Contacts)
            {
                if (contact.LegalEntity.Id == id)
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
        /// <returns>List<IndexableEnterprise></returns>
        private List<Enterprise> GetIndexableEnterpriseList()
        {
            List<Enterprise> result = new List<Enterprise>();
            Enterprise notSpecified = new IndexedEnterprise(0, Bizz.EnterpriseList[0]);
            result.Add(notSpecified);
            int i = 1;
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
            List<IndexedLegalEntity> result = new List<IndexedLegalEntity>();
            IndexedLegalEntity notSpecified = new IndexedLegalEntity(0, Bizz.LegalEntities[0]);
            result.Add(notSpecified);
            int i = 1;
            foreach (LegalEntity entity in Bizz.LegalEntities)
            {
                if (CheckCraftGroups(entity))
                {
                    IndexedLegalEntity temp = new IndexedLegalEntity(i, entity);
                    result.Add(temp);
                    i++;
                }
            }
            result = FilterIndexableLegalEntities(result);
            return result;
        }

        /// <summary>
        /// Method that creates a list of indexable SubEbtrepeneurs
        /// </summary>
        /// <returns>List<IndexableLegalEntity></returns>
        private List<IndexedSubEntrepeneur> GetIndexableSubEntrepeneurs()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            int i = 0;
            int id = Bizz.TempEnterprise.Id;
            foreach (SubEntrepeneur subEntrepeneur in Bizz.SubEntrepeneurs)
            {
                if (subEntrepeneur.EnterpriseList.Id == id)
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
            foreach (IndexedRequestStatus status in Bizz.IndexedRequestStatusList)
            {
                if (status.Index == index)
                {
                    result = status;
                }
            }

            return new RequestStatus(result.Description);
        }

        /// <summary>
        /// Method that returns index for selected Contact
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>int</returns>
        private int GetSelectedContact(int id)
        {
            int result = 0;
            foreach (IndexedContact temp in IndexableContacts)
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
            foreach (Request temp in Bizz.Requests)
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
        /// <param name="enterpriseList">int</param>
        /// <param name="entrepeneur">string</param>
        /// <returns>bool</returns>
        private bool IdExistsInSubEntrepeneurs(int enterpriseList, string entrepeneur)
        {

            foreach (SubEntrepeneur temp in Bizz.SubEntrepeneurs)
            {
                if (temp.Entrepeneur.Id == entrepeneur && temp.EnterpriseList.Id == enterpriseList)
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
            Bizz.TempContact = new Contact();
            ComboBoxRequest.SelectedIndex = -1;
            ComboBoxRequest.ItemsSource = Bizz.RequestStatusList;
            DateRequest.DisplayDate = DateTime.Now;
            DateRequest.Text = "";
            Bizz.TempRequest = new Request();
        }

        /// <summary>
        /// Method clears and updates IttLetters
        /// </summary>
        private void ResetIttLetters()
        {
            //SubEntrepeneur temp = Bizz.TempSubEntrepeneur;
            UpdateIttLetterSentInDb();
            Bizz.RefreshList("IttLetters");
        }

        /// <summary>
        /// Method clears and updates Offers
        /// </summary>
        private void ResetOffers()
        {
            Offer temp = Bizz.TempOffer;
            UpdateOfferReceivedInDb(temp);
            Bizz.Offers.Clear();
            Bizz.RefreshList("Offers");
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
            Bizz.TempIttLetter = new IttLetter();
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
            Bizz.TempOffer = new Offer();
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
            Bizz.TempSubEntrepeneur = new SubEntrepeneur();
            Bizz.RefreshList("SubEntrepeneurs");
            IndexableSubEntrepeneurs.Clear();
            IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
            ListBoxSubEntrepeneurs.ItemsSource = null;
            ListBoxSubEntrepeneurs.ItemsSource = IndexableSubEntrepeneurs;
        }

        /// <summary>
        /// Method, that clears and update SubEntrepeneurs
        /// </summary>
        private void ResetSubEntrepeneursRadioButtons(string sender)
        {
            DbStatus = Bizz.UpdateInDb(Bizz.TempSubEntrepeneur);
            Bizz.RefreshList("SubEntrepeneurs");
            IndexableSubEntrepeneurs.Clear();
            IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
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
                if (Bizz.TempIttLetter == tempIttLetter && Bizz.TempSubEntrepeneur.IttLetter.Id != 0)
                {
                    foreach (IttLetter temp in Bizz.IttLetters)
                    {
                        if (temp.Id == id)
                        {
                            Bizz.TempIttLetter = temp;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Bizz.TempIttLetter = new IttLetter();
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
                if (Bizz.TempOffer == tempOffer && Bizz.TempSubEntrepeneur.IttLetter.Id != 0)
                {
                    foreach (Offer temp in Bizz.Offers)
                    {
                        if (temp.Id == id)
                        {
                            Bizz.TempOffer = temp;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Bizz.TempOffer = new Offer();
            }
        }

        /// <summary>
        /// Method, that sets content of Bizz.TempSubEntrepeneur
        /// </summary>
        private void SetBizzTempSubEntrepeneur(int index)
        {
            IndexedSubEntrepeneur temp = IndexableSubEntrepeneurs[index];
            Bizz.TempSubEntrepeneur = new SubEntrepeneur(temp.Id, temp.EnterpriseList, temp.Entrepeneur, temp.Contact, temp.Request, temp.IttLetter, temp.Offer, temp.Reservations, temp.Uphold, temp.AgreementConcluded, temp.Active);
            if (!Bizz.TempSubEntrepeneur.Active)
            {
                Bizz.TempSubEntrepeneur.ToggleActive();
            }
        }

        /// <summary>
        /// Method, that populates ComboBoxes
        /// </summary>
        private void SetComboBoxes()
        {
            IndexableContacts = GetIndexableContacts();
            int contactIndex = GetSelectedContact(Bizz.TempSubEntrepeneur.Contact.Id);
            ComboBoxContact.ItemsSource = IndexableContacts;
            ComboBoxContact.SelectedIndex = contactIndex;
            Bizz.TempRequest = GetSelectedRequest(Bizz.TempSubEntrepeneur.Request.Id);
            GetRequestDate(Bizz.TempRequest);
            DateRequest.DisplayDate = Date;
            DateRequest.Text = Date.ToShortDateString();
            ComboBoxRequest.ItemsSource = Bizz.RequestStatusList;
            ComboBoxRequest.SelectedIndex = Bizz.TempRequest.Status.Id;
        }

        /// <summary>
        /// Method, that sets values for RadioButtons
        /// </summary>
        private void SetRadioButtons()
        {
            SetRadioButtonsIttLetterSent(Bizz.TempSubEntrepeneur.IttLetter.Id);
            SetRadioButtonsOfferReceived(Bizz.TempSubEntrepeneur.Offer.Id);
            SetRadioButtonsReservations(Bizz.TempSubEntrepeneur.Reservations);
            SetRadioButtonsUphold(Bizz.TempSubEntrepeneur.Uphold);
            SetRadioButtonsOfferChosen(Bizz.TempSubEntrepeneur.Offer.Id);
            SetRadioButtonsAgreementConcluded(Bizz.TempSubEntrepeneur.AgreementConcluded);
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
            if (Bizz.TempIttLetter.Sent)
            {
                if (Bizz.TempIttLetter.SentDate.ToShortDateString() == "")
                {
                    Bizz.TempIttLetter.SentDate = DateTime.Now;
                }
                if (DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10) != Bizz.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.DisplayDate = Bizz.TempIttLetter.SentDate;
                }
                if (DateIttLetter.Text == "" || DateIttLetter.Text.Substring(0, 10) != Bizz.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.Text = Bizz.TempIttLetter.SentDate.ToShortDateString();
                }
                RadioButtonIttLetterSentYes.IsChecked = true;
                RadioButtonIttLetterSentNo.IsChecked = false;
            }
            else
            {
                Date = Convert.ToDateTime("1932-03-17");
                if (Bizz.TempIttLetter.SentDate.ToShortDateString().Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
                {
                    Bizz.TempIttLetter.SentDate = Date;
                }
                if (DateIttLetter.DisplayDate.ToShortDateString().Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.DisplayDate = Bizz.TempIttLetter.SentDate;
                }
                if (DateIttLetter.Text == "" || DateIttLetter.Text.Substring(0, 10) != Date.ToShortDateString().Substring(0, 10))
                {
                    DateIttLetter.Text = Bizz.TempIttLetter.SentDate.ToShortDateString();
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
            if (Bizz.TempOffer.Chosen)
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
            if (Bizz.TempOffer.Received)
            {
                if (Bizz.TempOffer.ReceivedDate.ToShortDateString() == "" || Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10) == "1932-03-17")
                {
                    Bizz.TempOffer.AddReceived(DateTime.Now);
                }
                if (DateOffer.DisplayDate.ToShortDateString().Substring(0, 10) != Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.DisplayDate = Bizz.TempOffer.ReceivedDate;
                }
                if (DateOffer.Text == "" || DateOffer.Text.Substring(0, 10) != Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.Text = Bizz.TempOffer.ReceivedDate.ToShortDateString();
                }
                RadioButtonOfferReceivedYes.IsChecked = true;
                RadioButtonOfferReceivedNo.IsChecked = false;
            }
            else
            {
                Bizz.TempOffer.AddReceived(Convert.ToDateTime("1932-03-17"));
                if (DateOffer.DisplayDate.ToShortDateString().Substring(0, 10) != Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.DisplayDate = Bizz.TempOffer.ReceivedDate;
                }
                if (DateOffer.Text == "" || DateOffer.Text.Substring(0, 10) != Bizz.TempOffer.ReceivedDate.ToShortDateString().Substring(0, 10))
                {
                    DateOffer.Text = Bizz.TempOffer.ReceivedDate.ToShortDateString();
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
            if (Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    if (Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != DateRequest.Text.Substring(0, 10))
                    {
                        Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                        Date = Bizz.TempRequest.ReceivedDate;
                        if (!Changed)
                        {
                            Changed = true;
                        }
                        Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                        Date = Bizz.TempRequest.ReceivedDate;
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
                    Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                    Date = Bizz.TempRequest.ReceivedDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    Date = DateTime.Now;
                    Bizz.TempRequest.ReceivedDate = Date;
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
            if (Bizz.TempRequest.SentDate.ToShortDateString().Substring(0, 10) != "1932-03-17" || Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                Date = Convert.ToDateTime("1932-03-17");
                Bizz.TempRequest.ReceivedDate = Date;
                Bizz.TempRequest.SentDate = Date;
                Changed = true;
            }
        }

        /// <summary>
        /// Method, that coordinates Bizz.tempRequest.RecievedDate and DateRequest.Text
        /// </summary>
        private void SetRequestReceivedDate()
        {
            if (Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    if (DateRequest.Text.Substring(0, 10) != Bizz.TempRequest.ReceivedDate.ToShortDateString().Substring(0, 10))
                    {
                        Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                        Date = Bizz.TempRequest.ReceivedDate;
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
                    Bizz.TempRequest.ReceivedDate = Convert.ToDateTime(DateRequest.Text);
                    Date = Bizz.TempRequest.ReceivedDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    Date = DateTime.Now;
                    Bizz.TempRequest.ReceivedDate = Date;
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
            if (Bizz.TempRequest.SentDate.ToShortDateString().Substring(0, 10) != "1932-03-17")
            {
                Date = Bizz.TempRequest.SentDate;
                if (DateRequest.Text.Substring(0, 10) != "1932-03-17")
                {
                    Bizz.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
                    Date = Bizz.TempRequest.SentDate;
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
                    Bizz.TempRequest.SentDate = Convert.ToDateTime(DateRequest.Text);
                    Date = Bizz.TempRequest.SentDate;
                    if (!Changed)
                    {
                        Changed = true;
                    }
                }
                else
                {
                    Date = DateTime.Now;
                    Bizz.TempRequest.SentDate = Date;
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
            bool result = Bizz.UpdateInDb(Bizz.TempIttLetter);

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
            bool result = Bizz.UpdateInDb(offer);

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
            bool result = Bizz.UpdateInDb(request);

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

            foreach (RequestStatus status in Bizz.RequestStatusList)
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
