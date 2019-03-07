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
    /// Interaction logic for UcContactsDelete.xaml
    /// </summary>
    public partial class UcContactsDelete : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<IndexedContact> FilteredContacts = new List<IndexedContact>();

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor with GUI for deleting contacts
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <param name="ucRight">UserControl</param>
        public UcContactsDelete(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucRight;

            CBZ.TempContact = new Contact();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempContact != new Contact())
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke 'Slet Kontakt'?", "Kontakter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Refresh Contacts list
                    CBZ.RefreshList("Contacts");
                    CBZ.TempContact = new Contact();

                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Refresh Contacts list
                CBZ.RefreshList("Contacts");
                CBZ.TempContact = new Contact();

                //Close main UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;


            //Warning about lost changes before closing
            if (MessageBox.Show("Vil slette de(n) valgte Kontakt(er)'? De kan ikke gendannes", "Kontakter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (ListBoxContacts.SelectedItems.Count >= 2)
                {
                    result = DeleteContactsFromDb();
                }
                else if (ListBoxContacts.SelectedItems.Count == 1)
                {
                    result = DeleteContactFromDb();
                }
                //Refresh Contacts list
                CBZ.RefreshList("Contacts");
                CBZ.TempContact = new Contact();

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Kontakten blev opdateret", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxContacts.SelectedIndex = -1;
                ListBoxContacts.ItemsSource = "";
                TextBoxContactSearch.Text = "";

                //Refresh Contacts list
                CBZ.RefreshList("Contacts");
                CBZ.TempContact = new Contact();
                CBZ.TempEntrepeneur = new Entrepeneur();

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Kontakten blev ikke opdateret. Prøv igen.", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region Events
        private void TextBoxContactSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxContactSearch.Text.Length >= 3)
            {
                GetFilteredContacts();
                ListBoxContacts.SelectedIndex = -1;
                ListBoxContacts.ItemsSource = "";
                ListBoxContacts.ItemsSource = FilteredContacts;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ListBoxContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxContacts.SelectedItems.Count == 1)
            {
                CBZ.TempContact = new Contact((Contact)ListBoxContacts.SelectedItem);
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
        /// Method, that deletes the selected Contact from Db
        /// </summary>
        /// <returns>bool</returns>
        private bool DeleteContactFromDb()
        {
            bool result = false;

            try
            {
                bool tempResult = CBZ.DeleteFromDb("Contacts", CBZ.TempContact.Id.ToString());

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(CBZ.TempContact.Person.Name + " kunne ikke slettes, da den er tilknyttet en Entrepenør." + ex, "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// <summary>
        /// Method, that deletes the selected Contacts from Db
        /// </summary>
        /// <returns>bool</returns>
        private bool DeleteContactsFromDb()
        {
            bool allDeleted = false;
            bool result = false;
            bool oneDeleted = false;
            bool someDeleted = false;
            int countDeleted = 0;
            int maxCount = ListBoxContacts.SelectedItems.Count;

            foreach (var selectedItem in ListBoxContacts.SelectedItems)
            {
                CBZ.TempContact = new Contact((Contact)selectedItem);

                bool tempResult = DeleteContactFromDb();

                if (tempResult)
                {
                    countDeleted++;
                    string Switch = GetSwitch(countDeleted, maxCount);

                    switch (Switch)
                    {
                        case "MAX":
                            allDeleted = true;
                            break;
                        case "SOME":
                            someDeleted = true;
                            break;
                        case "ONE":
                            oneDeleted = true;
                            break;
                        default:
                            break;
                    }

                }

                if (allDeleted)
                {
                    MessageBox.Show("Kontakterne blev slettet.", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                }
                else if (!allDeleted && someDeleted)
                {
                    MessageBox.Show("Nogle kontakter blev slettet. Resten er tilknyttet Underentrepenør(er).", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                }
                else if (!allDeleted && !someDeleted && oneDeleted)
                {
                    MessageBox.Show("Kun én kontakt blev slettet. Resten er tilknyttet Underentrepenør(er).", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                }
                else
                {
                    MessageBox.Show("Ingen kontakter blev slettet! Alle er tilknyttet Underentrepenører!", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            return result;

        }

        /// <summary>
        /// Method, that retrieves a list of filtered Entrepeneurs for ListBoxEntrepeneurs
        /// </summary>
        private void GetFilteredContacts()
        {
            CBZ.RefreshIndexedList("IndexedContacts");
            this.FilteredContacts = new List<IndexedContact>();

            foreach (IndexedContact contact in CBZ.IndexedContacts)
            {
                if (contact.Person.Name.Remove(3) == TextBoxContactSearch.Text.Remove(3))
                {
                    this.FilteredContacts.Add(contact);
                }
            }
        }

        /// <summary>
        /// Method, that retrieves a switch string
        /// </summary>
        /// <param name="countDeleted">int</param>
        /// <param name="maxCount">int</param>
        /// <returns>string</returns>
        private string GetSwitch(int countDeleted, int maxCount)
        {
            string result = "";

            if (countDeleted == maxCount)
            {
                result = "MAX";
            }
            else if (countDeleted < maxCount && countDeleted >= 2)
            {
                result = "SOME";
            }
            else
            {
                result = "ONE";
            }

            return result;
        }

        #endregion
    }
}
