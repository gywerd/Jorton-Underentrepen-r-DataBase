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
    /// Interaction logic for UcContactsEdit.xaml
    /// </summary>
    public partial class UcContactsEdit : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<IndexedContact> FilteredContacts = new List<IndexedContact>();
        public List<IndexedEntrepeneur> FilteredEntrepeneurs = new List<IndexedEntrepeneur>();

        #endregion

        #region Constructors
        public UcContactsEdit(Bizz cbz, UserControl ucRight)
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
                if (MessageBox.Show("Vil du annullere opdatering af Kontakt?", "Kontakter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Refresh Contacts list
                    CBZ.RefreshList("Contacts");
                    CBZ.TempContact = new Contact();
                    CBZ.TempEntrepeneur = new Entrepeneur();

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
                CBZ.TempEntrepeneur = new Entrepeneur();

                //Close main UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            bool result = UpdateContactInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Kontakten blev opdateret", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxContacts.SelectedIndex = -1;
                ListBoxContacts.ItemsSource = "";
                ListBoxEntrepeneurs.SelectedIndex = -1;
                ListBoxEntrepeneurs.ItemsSource = "";
                TextBoxEntrepeneurSearch.Text = "";
                TextBoxName.Text = "";
                TextBoxPhone.Text = "";
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";
                TextBoxArea.Text = "";

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
        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxEntrepeneurs.SelectedIndex >= 0)
            {
                CBZ.TempEntrepeneur = new Entrepeneur((Entrepeneur)ListBoxEntrepeneurs.SelectedItem);

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
            if (ListBoxContacts.SelectedIndex >= 0)
            {
                CBZ.TempContact = new Contact((Contact)ListBoxContacts.SelectedItem);
                TextBoxEmail.Text = CBZ.TempContact.Person.ContactInfo.Email;
                TextBoxFax.Text = CBZ.TempContact.Person.ContactInfo.Fax;
                TextBoxMobile.Text = CBZ.TempContact.Person.ContactInfo.Mobile;
                TextBoxName.Text = CBZ.TempContact.Person.Name;
                TextBoxPhone.Text = CBZ.TempContact.Person.ContactInfo.Phone;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }


        private void TextBoxArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempContact.Area = TextBoxArea.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempContact.Person.ContactInfo.Email = TextBoxEmail.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxEntrepeneursSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxEntrepeneurSearch.Text.Length >= 3)
            {
                GetFilteredEntrepeneurs();
                ListBoxEntrepeneurs.SelectedIndex = -1;
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = FilteredEntrepeneurs;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempContact.Person.ContactInfo.Fax = TextBoxFax.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempContact.Person.ContactInfo.Mobile = TextBoxMobile.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempContact.Person.Name = TextBoxName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempContact.Person.ContactInfo.Phone = TextBoxPhone.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that retrieves a list of filtered Entrepeneurs for ListBoxEntrepeneurs
        /// </summary>
        private void GetFilteredContacts()
        {
            CBZ.RefreshIndexedList("IndexedContacts");
            this.FilteredContacts = new List<IndexedContact>();

            foreach (IndexedContact contact in CBZ.IndexedContacts)
            {
                if (contact.Entrepeneur == CBZ.TempEntrepeneur)
                {
                    this.FilteredContacts.Add(contact);
                }
            }
        }

        /// <summary>
        /// Method, that retrieves a list of filtered Entrepeneurs for ListBoxEntrepeneurs
        /// </summary>
        private void GetFilteredEntrepeneurs()
        {
            CBZ.RefreshIndexedList("IndexedEntrepeneurs");
            this.FilteredEntrepeneurs = new List<IndexedEntrepeneur>();

            foreach (IndexedEntrepeneur entrepeneur in CBZ.IndexedEntrepeneurs)
            {
                if (entrepeneur.Entity.Name.Remove(3) == TextBoxEntrepeneurSearch.Text.Remove(3) || entrepeneur.Entity.Name.Remove(7) == "A/S " + TextBoxEntrepeneurSearch.Text.Remove(3))
                {
                    this.FilteredEntrepeneurs.Add(entrepeneur);
                }
            }
        }

        /// <summary>
        /// Method, that updates a Contact in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateContactInDb()
        {
            //Code that creates a new Builder
            bool result = false;

            //Create ContactInfo in Db
            bool contactInfoUpdated = UpdateContactInfo();

            //Create Address in Db
            bool personUpdated = false;

            if (contactInfoUpdated)
            {
                personUpdated = UpdatePersonInDb();
            }


            //Create Builder in Db
            if (personUpdated)
            {
                result = CBZ.UpdateInDb(CBZ.TempContact);
            }

            return result;
        }

        /// <summary>
        /// Method, that updates Contact Info in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateContactInfo()
        {
            bool result = CBZ.UpdateInDb(CBZ.TempContact.Person.ContactInfo);

            if (!result)
            {
                MessageBox.Show("Kontaktoplysningerne blev ikke opdateret", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// <summary>
        /// Method, that updates an Address in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdatePersonInDb()
        {
            bool result = CBZ.UpdateInDb(CBZ.TempContact.Person);

            if (!result)
            {
                MessageBox.Show("Personen blev ikke opdateret", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        #endregion

    }
}
