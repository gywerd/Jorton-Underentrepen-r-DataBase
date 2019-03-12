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
    /// Interaction logic for UcContactCreate.xaml
    /// </summary>
    public partial class UcContactCreate : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<IndexedEntrepeneur> FilteredEntrepeneurs = new List<IndexedEntrepeneur>();

        #endregion

        #region Constructors
        public UcContactCreate(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempContact = new Contact();

            GetFilteredEntrepeneurs();
            ListBoxEntrepeneurs.ItemsSource = FilteredEntrepeneurs;
            ListBoxEntrepeneurs.SelectedIndex = -1;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempContact != new Contact())
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du annullere oprettelse af Kontakt?", "Kontakter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new Builder
            bool result = CreateContactInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Kontakten blev oprettet", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Refresh Contacts list
                CBZ.RefreshList("Contacts");
                CBZ.TempContact = new Contact();

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Kontakten blev ikke oprettet. Prøv igen.", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateContactInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Kontakten blev oprettet", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxPhone.Text = "";
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";
                TextBoxArea.Text = "";

                //Refresh Contacts list
                CBZ.RefreshList("Contacts");
                CBZ.TempContact = new Contact();

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Kontakten blev ikke oprettet. Prøv igen.", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        #region Events
        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxEntrepeneurs.SelectedIndex >= 0)
            {
                CBZ.TempContact.Entrepeneur = new Entrepeneur((Entrepeneur)ListBoxEntrepeneurs.SelectedItem);
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
            GetFilteredEntrepeneurs();
            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = FilteredEntrepeneurs;
            ListBoxEntrepeneurs.SelectedIndex = -1;


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
        /// Method, that creates an Contact in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateContactInDb()
        {
            bool result = false;

            //Create ContactInfo in Db
            bool contactInfoExist = CreateContactInfo();

            //Create Person in Db
            bool personExist = false;

            if (contactInfoExist)
            {
                personExist = CreatePersonInDb();
            }


            //Create Builder in Db
            int contactId = 0;

            if (personExist)
            {
                contactId = CBZ.CreateInDb(CBZ.TempContact);
            }

            //Check result
            if (contactId >= 1)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that creates Contact Info in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateContactInfo()
        {
            bool result = false;

            try
            {
                int contactInfoId = CBZ.CreateInDb(CBZ.TempContact.Person.ContactInfo);
                CBZ.TempContact.Person.ContactInfo.SetId(contactInfoId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kontaktoplysningerne blev ikke gemt\n" + ex, "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// <summary>
        /// Method, that creates an Address in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreatePersonInDb()
        {
            bool result = false;

            try
            {
                int personId = CBZ.CreateInDb(CBZ.TempContact.Person);
                CBZ.TempContact.Person.SetId(personId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personen blev ikke gemt\n" + ex, "Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a list of filtered Entrepeneurs for ListBoxEntrepeneurs
        /// </summary>
        private void GetFilteredEntrepeneurs()
        {
            CBZ.RefreshIndexedList("IndexedEntrepeneurs");
            this.FilteredEntrepeneurs = new List<IndexedEntrepeneur>();
            int length = TextBoxEntrepeneurSearch.Text.Length;

            foreach (IndexedEntrepeneur entrepeneur in CBZ.IndexedEntrepeneurs)
            {
                if (entrepeneur.Entity.Name.Remove(length) == TextBoxEntrepeneurSearch.Text || entrepeneur.Entity.Name.Remove(length + 4) == "A/S " + TextBoxEntrepeneurSearch.Text)
                {
                    this.FilteredEntrepeneurs.Add(entrepeneur);
                }
            }
        }

        #endregion

    }
}
