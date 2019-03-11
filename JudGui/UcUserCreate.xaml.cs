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
    /// Interaction logic for UcUserCreate.xaml
    /// </summary>
    public partial class UcUserCreate : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcUserCreate(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Du er ved at lukke 'Opret Bruger'. Data der ikke er glemt, bliver mistet.", "Brugere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Refresh Contacts list
                    CBZ.RefreshList("Users");
                    CBZ.TempUser = new User();

                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Refresh Contacts list
                CBZ.RefreshList("Users");
                CBZ.TempUser = new User();

                //Close main UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new Builder
            bool result = CreateUserInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Brugeren blev oprettet", "Brugere", MessageBoxButton.OK, MessageBoxImage.Information);

                //Refresh Users list
                CBZ.RefreshList("Users");
                CBZ.TempUser = new User();

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Brugeren blev ikke oprettet. Prøv igen.", "Brugere", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateUserInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Kontakten blev oprettet", "Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ComboBoxJobDescription.SelectedIndex = -1;
                ComboBoxJobDescription.ItemsSource = "";
                ComboBoxUserLevel.SelectedIndex = -1;
                ComboBoxUserLevel.ItemsSource = "";

                TextBoxName.Text = "";
                TextBoxPhone.Text = "";
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";
                TextBoxInitials.Text = "";

                //Refresh Users list
                CBZ.RefreshList("Users");
                CBZ.TempUser = new User();

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Brugeren blev ikke oprettet. Prøv igen.", "Brugere", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        #region Events
        private void ComboBoxJobDescription_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JobDescription description = new JobDescription((JobDescription)ComboBoxJobDescription.SelectedItem);
            if (ComboBoxJobDescription.SelectedIndex >= 0)
            {

                if (description.Id != CBZ.TempUser.JobDescription.Id)
                {
                    CBZ.TempUser.JobDescription = new JobDescription((JobDescription)ComboBoxJobDescription.SelectedItem);

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }

        }

        private void ComboBoxUserLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserLevel level = new UserLevel((UserLevel)ComboBoxUserLevel.SelectedItem);
            if (ComboBoxUserLevel.SelectedIndex >= 0)
            {
                if (level.Id != CBZ.TempUser.Authentication.UserLevel.Id)
                {
                    CBZ.TempUser.Authentication.UserLevel = new UserLevel((UserLevel)ComboBoxUserLevel.SelectedItem);

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }

        }
        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.Name = TextBoxName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Email = TextBoxEmail.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Fax = TextBoxFax.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxInitials_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Initials = TextBoxInitials.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Mobile = TextBoxMobile.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Phone = TextBoxPhone.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// Method, that creates an Authentication in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateAuthenticationInDb()
        {
            bool result = false;

            try
            {
                int authenticationId = CBZ.CreateInDb(CBZ.TempUser.Authentication);
                CBZ.TempUser.Authentication.SetId(authenticationId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Brugerrettigheder blev ikke gemt\n" + ex, "Brugere", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// Method, that creates a User in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateUserInDb()
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

            //Create Authentication in Db
            bool authenticationExist = false;

            if (personExist)
            {
                authenticationExist = CreateAuthenticationInDb();
            }

            //Create User in Db
            int userId = 0;

            if (authenticationExist)
            {
                userId = CBZ.CreateInDb(CBZ.TempUser);
            }

            //Check result
            if (userId >= 1)
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
                int contactInfoId = CBZ.CreateInDb(CBZ.TempUser.Person.ContactInfo);
                CBZ.TempUser.Person.ContactInfo.SetId(contactInfoId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kontaktoplysningerne blev ikke gemt\n" + ex, "Brugere", MessageBoxButton.OK, MessageBoxImage.Error);
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
                int personId = CBZ.CreateInDb(CBZ.TempUser.Person);
                CBZ.TempUser.Person.SetId(personId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personen blev ikke gemt\n" + ex, "Brugere", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        #endregion

    }
}
